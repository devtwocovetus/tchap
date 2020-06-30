using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCloudHealth.Models;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;

namespace TheCloudHealth.Lib
{
    interface ITwilioVideo
    {
        RoomDetails CreateRoom(string RoomUniqueName);
        RoomDetails GetRoomByUniqueName(string RoomUniqueName);
        RoomDetails GetRoomBySID(string RoomSID);
        List<RoomDetails> GetRoomListByStatus(string RoomStatus);
        ParticipantResource JoinRoom();
    }
}
