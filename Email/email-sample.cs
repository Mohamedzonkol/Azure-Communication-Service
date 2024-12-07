using System;
using System.Collections.Generic;
using Azure;
using Azure.Communication.Email;

string connectionString = "endpoint=https://test-788.africa.communication.azure.com/;accesskey=Ddqx8i655LBHE0wOJ6gPMbAXmbDfnKjy5IpsO5q00jAJ0P4FzEr5JQQJ99ALACULyCpxOwjyAAAAAZCScBpW";
var emailClient = new EmailClient(connectionString);


var emailMessage = new EmailMessage(
    senderAddress: "DoNotReply@e41a2764-8af0-4fb1-846c-da8a424b602c.azurecomm.net",
    content: new EmailContent("Test Email")
    {
        PlainText = "Dear Mohamed Elsayed ,\n\nWe are pleased to inform you that this email was successfully sent using Azure Communication Services. This is a demonstration of how you can utilize our platform for efficient and reliable email communication.\n\nIf you have any questions or need further assistance, please do not hesitate to contact us.\n\nBest regards,\nThe Azure Email Service Team",
        Html = @"
<html>
    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0;'>
        <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px; background-color: #f9f9f9;'>
            <h1 style='color: #0066cc; font-size: 24px; text-align: center;'>Greetings from Azure Email Service</h1>
            <p style='font-size: 16px;'>Dear Mohamed Elsayed,</p>
            <p style='font-size: 16px;'>
                We are pleased to inform you that this email was successfully sent using 
                <strong>Azure Communication Services</strong>. This is a demonstration of how you can utilize 
                our platform for efficient and reliable email communication.
            </p>
            <p style='font-size: 16px;'>
                If you have any questions or need further assistance, please do not hesitate to contact us.
            </p>
            <p style='font-size: 16px;'>Best regards,</p>
            <p style='font-size: 16px; font-weight: bold;'>Your Love Mohamed Zonkol</p>
        </div>
    </body>
</html>"
    },
    recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress("mo.zonkol@gmail.com") }));
    

EmailSendOperation emailSendOperation = emailClient.Send(
    WaitUntil.Completed,
    emailMessage);
