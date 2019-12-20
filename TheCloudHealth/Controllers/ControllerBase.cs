using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;

namespace TheCloudHealth.Controllers
{
    public abstract class ControllerBase : TwilioController
    {
        public TwiMLResult RedirectWelcome()
        {
            var response = new VoiceResponse();
            response.Say("Returning to the main menu",
                voice: "alice",
                language: "en-US"
            );
            response.Redirect(Url.ActionUri("Welcome", "IVR"));

            return TwiML(response);
        }
    }
}