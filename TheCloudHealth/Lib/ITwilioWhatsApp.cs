using TheCloudHealth.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;

namespace TheCloudHealth.Lib
{
    interface ITwilioWhatsApp
    {
        MessageResource SendMessageWithWhatsapp(Whatsapp whatsapp);
    }
}
