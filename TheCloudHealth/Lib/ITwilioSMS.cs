using TheCloudHealth.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;

namespace TheCloudHealth.Lib
{
    interface ITwilioSMS
    {
        MessageResource SendSMS(Sms sms);
        MessagingResponse ResponseSMS();
        MessageResource SendMessageWithWhatsapp(Sms sms);
    }
}
