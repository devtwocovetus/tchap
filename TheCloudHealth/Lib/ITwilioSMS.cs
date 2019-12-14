using TheCloudHealth.Models;
using Twilio.Rest.Api.V2010.Account;

namespace TheCloudHealth.Lib
{
    interface ITwilioSMS
    {
        MessageResource SendSMS(Sms sms);
    }
}
