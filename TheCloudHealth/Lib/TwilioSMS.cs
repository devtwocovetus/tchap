using PhoneNumbers;
using System;
using System.Net;
using TheCloudHealth.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace TheCloudHealth.Lib
{
    public class TwilioSMS : ITwilioSMS
    {
        public string accountSid;
        public string authToken;
        public string RegisterPhoneNo;
        ConnectionClass con;
        public TwilioSMS()
        {
            con = new ConnectionClass();
            accountSid = con.DecryptData("0eMigIRbyiBF0YL+S6fmIT+mkktI1K2DfQs7PLn7PfnRGZT2k0PvLM0qLPT6E1VA6G8dBAPpaRjv7C71OZxNXQ==");
            authToken = con.DecryptData("8c8q5st01qrCIzdnV2yHfQX9MP8tTVFvxQHks0Ij9yem+g7e/Q4xT1B+4ukcKWGRfySDDKGZNUzv7C71OZxNXQ==");
            RegisterPhoneNo= con.DecryptData("XeS5kn9Pido2fCCpjeQcWOVQi62rKZOY7+wu9TmcTV0=");
        }

        public MessageResource SendSMS(Sms sms)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: sms.Message_Body,
                from: new Twilio.Types.PhoneNumber(RegisterPhoneNo),
                to: new Twilio.Types.PhoneNumber(sms.Receiver_Contact_No)
            );
            return message;
        }

        public MessagingResponse ResponseSMS()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);

            var response = new MessagingResponse();
            var message = new Message();
            message.Body("Hello World!");
            response.Append(message);
            //response.Redirect(url: new Uri("http://tchapi.thecloudhealth.com/API/Sms/GenTwiML?Text='hii sameer, How are you?'"));
            return response;
        }

        public MessageResource SendMessageWithWhatsapp(Sms sms)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);

            var to = "whatsapp:" + sms.Receiver_Contact_No;

            var messageOptions = new CreateMessageOptions(
            new Twilio.Types.PhoneNumber(to.ToString()));
            messageOptions.From = new Twilio.Types.PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = sms.Message_Body;
            var message = MessageResource.Create(messageOptions);
            return message;

        }
    }
}