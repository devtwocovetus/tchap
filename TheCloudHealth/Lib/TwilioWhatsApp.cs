using System.Net;
using TheCloudHealth.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace TheCloudHealth.Lib
{
    public class TwilioWhatsApp :  ITwilioWhatsApp
    {
        public string accountSid;
        public string authToken;
        public string RegisterPhoneNo;
        ConnectionClass con;
        public TwilioWhatsApp()
        {
            con = new ConnectionClass();
            accountSid = con.DecryptData("0eMigIRbyiBF0YL+S6fmIT+mkktI1K2DfQs7PLn7PfnRGZT2k0PvLM0qLPT6E1VA6G8dBAPpaRjv7C71OZxNXQ==");
            authToken = con.DecryptData("8c8q5st01qrCIzdnV2yHfQX9MP8tTVFvxQHks0Ij9yem+g7e/Q4xT1B+4ukcKWGRfySDDKGZNUzv7C71OZxNXQ==");
            RegisterPhoneNo = con.DecryptData("XeS5kn9Pido2fCCpjeQcWOVQi62rKZOY7+wu9TmcTV0=");
        }

        public MessageResource SendMessageWithWhatsapp(Whatsapp whatsapp)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);

            var to = "whatsapp:" + whatsapp.Receiver_Contact_No;

            var messageOptions = new CreateMessageOptions(
            new Twilio.Types.PhoneNumber(to.ToString()));
            messageOptions.From = new Twilio.Types.PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = whatsapp.Message_Body;
            var message = MessageResource.Create(messageOptions);
            return message;

        }
    }
}