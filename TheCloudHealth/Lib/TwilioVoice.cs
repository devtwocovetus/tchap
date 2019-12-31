using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TheCloudHealth.Models;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Twilio.Rest.Api.V2010.Account;
//using System.Security.Policy;

namespace TheCloudHealth.Lib
{
    public class TwilioVoice : ITwilioVoice
    {
        public string accountSid;
        public string authToken;
        public string RegisterPhoneNo;
        ConnectionClass con;
        public TwilioVoice()
        {
            con = new ConnectionClass();
            accountSid = con.DecryptData("0eMigIRbyiBF0YL+S6fmIT+mkktI1K2DfQs7PLn7PfnRGZT2k0PvLM0qLPT6E1VA6G8dBAPpaRjv7C71OZxNXQ==");
            authToken = con.DecryptData("8c8q5st01qrCIzdnV2yHfQX9MP8tTVFvxQHks0Ij9yem+g7e/Q4xT1B+4ukcKWGRfySDDKGZNUzv7C71OZxNXQ==");
            RegisterPhoneNo = con.DecryptData("XeS5kn9Pido2fCCpjeQcWOVQi62rKZOY7+wu9TmcTV0=");
        }

        public CallResource SetVoiceCall(Voice voice)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);
                var to = voice.Voice_Receiver_Contact_No;

                var call = CallResource.Create(
                    new Twilio.Types.PhoneNumber(voice.Voice_Receiver_Contact_No),
                    new Twilio.Types.PhoneNumber(RegisterPhoneNo),
                    //url: new Uri("https://handler.twilio.com/twiml/EHf1de6e7e87a1ff389c6b5b06bbfaa58d?Name=" + voice.Voice_Receiver_Name + "&Body=" + voice.Voice_Call_Body)
                    url:new Uri("https://handler.twilio.com/twiml/EH108a09c76cc8cf8c7148d7d5c81eeeef?Name=" + voice.Voice_Receiver_Name + "&Message=" + voice.Voice_Call_Body)
                    );
                return call;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Twilio.TwiML.VoiceResponse ResponseVoiceCall()
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);

                var response = new Twilio.TwiML.VoiceResponse();
                response.Say("Thank you for calling, we will call you soon", voice: "alice", language: "en-US");
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}