using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCloudHealth.Models;
using Twilio.Rest.Api.V2010.Account;

namespace TheCloudHealth.Lib
{
    interface ITwilioVoice
    {
        CallResource SetVoiceCall(Voice voice);
        Twilio.TwiML.VoiceResponse ResponseVoiceCall();
    }
}
