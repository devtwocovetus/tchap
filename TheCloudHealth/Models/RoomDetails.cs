using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    public class RoomDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ParticipantCount { get; set; }
        public int MaxParticipants { get; set; }
        public string Status { get; set; }
    }
    public class TwilioSettings
    {
        /// <summary>
        /// The primary Twilio account SID, displayed prominently on your twilio.com/console dashboard.
        /// </summary>
        public string AccountSid { get; set; }

        /// <summary>
        /// The auth token for your primary Twilio account, hidden on your twilio.com/console dashboard.
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Signing Key SID, also known as the API SID or API Key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The API Secret that corresponds to the <see cref="ApiKey"/>.
        /// </summary>
        public string ApiSecret { get; set; }
    }

    public class RoomResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public RoomDetails Data { get; set; }
        public List<RoomDetails> DataList { get; set; }
    }
}