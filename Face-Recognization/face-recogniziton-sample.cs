using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize FaceClient with subscription key and endpoint
        IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials("BQUs0thK3740m0GamY2d2pQEySQudawg61QOoDv1dTN53tbZ6soEJQQJ99ALACYeBjFXJ3w3AAAKACOGIwE5"))
        {
            Endpoint = "https://face-test3213.cognitiveservices.azure.com/"
        };

        string personGroupId = "zonkolgroupid";

        try
        {
            // Step 1: Create a person group
            Console.WriteLine("Creating a person group...");
            await faceClient.PersonGroup.CreateAsync(personGroupId, "My Person Group");

            // Step 2: Add a person to the person group
            Console.WriteLine("Adding a person to the person group...");
            Person person = await faceClient.PersonGroupPerson.CreateAsync(personGroupId, "Sample Person");
            Console.WriteLine($"Created a person with ID: {person.PersonId}");

            string imageUrl = "https://raw.githubusercontent.com/Mohamedzonkol/Azure-Communication-Service/main/Face-Recognization/images/Face.jpg";
            using (HttpClient httpClient = new HttpClient())
            using (Stream imageStream = await httpClient.GetStreamAsync(imageUrl))
            {
                IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithStreamAsync(
                    imageStream, true, false, new List<FaceAttributeType> { FaceAttributeType.Age });

                if (detectedFaces.Count == 0)
                {
                    Console.WriteLine("No faces detected in the image.");
                    return;
                }

                // Add the first detected face to the person
                Guid detectedFaceId = detectedFaces[0].FaceId.Value;
                Console.WriteLine($"Detected face with ID: {detectedFaceId}");
                await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(
                    personGroupId, person.PersonId, new MemoryStream(await httpClient.GetByteArrayAsync(imageUrl)));

                // Step 4: Train the person group
                Console.WriteLine("Training the person group...");
                await faceClient.PersonGroup.TrainAsync(personGroupId);

                // Wait for the training to complete
                TrainingStatus trainingStatus;
                do
                {
                    trainingStatus = await faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId);
                    Console.WriteLine($"Training status: {trainingStatus.Status}");
                    await Task.Delay(1000);
                } while (trainingStatus.Status != TrainingStatusType.Succeeded);

                // Step 5: Identify the face in the image
                Console.WriteLine("Identifying the face in the image...");
                IList<IdentifyResult> results = await faceClient.Face.IdentifyAsync(new List<Guid> { detectedFaceId }, personGroupId);

                foreach (var identifyResult in results)
                {
                    Console.WriteLine($"Result for face: {identifyResult.FaceId}");
                    foreach (var candidate in identifyResult.Candidates)
                    {
                        Console.WriteLine($"Identified as {candidate.PersonId} with confidence {candidate.Confidence}");
                    }
                }
            }
        }
        catch (APIErrorException apiEx)
        {
            Console.WriteLine($"API Error: {apiEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
