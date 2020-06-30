using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheCloudHealth.Models;
using Twilio;
using Twilio.Base;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using ParticipantStatus = Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum;

namespace TheCloudHealth.Lib
{
    public class TwilioVideo : ITwilioVideo
    {
        public string accountSid;
        public string authToken;
        public string accountApiKey;
        public string accountSecretKey;
        public string RegisterPhoneNo;
        ConnectionClass con;

        public TwilioVideo()
        {
            con = new ConnectionClass();
            accountSid = con.DecryptData("0eMigIRbyiBF0YL+S6fmIT+mkktI1K2DfQs7PLn7PfnRGZT2k0PvLM0qLPT6E1VA6G8dBAPpaRjv7C71OZxNXQ==");
            authToken = con.DecryptData("8c8q5st01qrCIzdnV2yHfQX9MP8tTVFvxQHks0Ij9yem+g7e/Q4xT1B+4ukcKWGRfySDDKGZNUzv7C71OZxNXQ==");
            accountApiKey = con.DecryptData("4k5IkzpMuiU5JX7+C2bzrvTHv31gY+uVQs0kYMzSBwReb3GemjNN/xusW8o59Rmn3QSSCA54naHv7C71OZxNXQ==");
            accountSecretKey = con.DecryptData("cBOY9qap/mj8UPgTfBI2Z7iXYuDiGOEPHeUoFK+10gte6mHpZTAUBfKkqAB7TdnpfySDDKGZNUzv7C71OZxNXQ==");
            RegisterPhoneNo = con.DecryptData("XeS5kn9Pido2fCCpjeQcWOVQi62rKZOY7+wu9TmcTV0=");
        }

        public RoomDetails CreateRoom(string RoomUniqueName)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);
            RoomDetails RoomDetail = new RoomDetails();
            try
            {
                var room = RoomResource.Create(
                enableTurn: true,
                type: RoomResource.RoomTypeEnum.PeerToPeer,
                uniqueName: RoomUniqueName);
                RoomDetail.Id = room.Sid;
                RoomDetail.Name = room.UniqueName;
                RoomDetail.MaxParticipants = Convert.ToInt32(room.MaxParticipants);
                RoomDetail.Status = room.Status.ToString();

            }
            catch (Exception ex)
            {

                throw;
            }
            return RoomDetail;
        }

        public RoomDetails GetRoomByUniqueName(string RoomUniqueName)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);
            RoomDetails RoomDetail = new RoomDetails();
            try
            {
                var room = RoomResource.Fetch(pathSid: RoomUniqueName);
                RoomDetail.Id = room.Sid;
                RoomDetail.Name = room.UniqueName;
                RoomDetail.MaxParticipants = Convert.ToInt32(room.MaxParticipants);
                RoomDetail.Status = room.Status.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
            return RoomDetail;
        }

        public RoomDetails GetRoomBySID(string RoomSID)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);
            RoomDetails RoomDetail = new RoomDetails();
            try
            {
                var room = RoomResource.Fetch(pathSid: RoomSID);
                RoomDetail.Id = room.Sid;
                RoomDetail.Name = room.UniqueName;
                RoomDetail.MaxParticipants = Convert.ToInt32(room.MaxParticipants);
                RoomDetail.Status = room.Status.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
            return RoomDetail;
        }

        public List<RoomDetails> GetRoomListByStatus(string RoomStatus)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            TwilioClient.Init(accountSid, authToken);
            List<RoomDetails> RoomDetailList = new List<RoomDetails>();
            RoomDetails RD = new RoomDetails();
            try
            {
                if (RoomStatus.ToUpper() == "COMPLETED")
                {
                    var rooms = RoomResource.Read(
                    status: RoomResource.RoomStatusEnum.Completed,
                    limit: 20
                    );
                    foreach (var rm in rooms)
                    {
                        RD.Id = rm.AccountSid;
                        RD.Name = rm.UniqueName;
                        RD.MaxParticipants = Convert.ToInt32(rm.MaxParticipants);
                        RD.Status = rm.Status.ToString();
                        RoomDetailList.Add(RD);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return RoomDetailList;
        }

        public ParticipantResource JoinRoom()
        {
            ParticipantResource participant = ParticipantResource.Fetch(
                    "DailyStandup",
                    "Alice");

            
            return participant;
        }

        public RoomResource CompleteRoom(string RoomSID)
        {
            TwilioClient.Init(accountSid, authToken);

            var room = RoomResource.Update(
                status: RoomResource.RoomStatusEnum.Completed,
                pathSid: RoomSID
            );
            return room;
        }


        public string GetTwilioJwt(string identity)
            => new Twilio.Jwt.AccessToken.Token(accountSid,
                         accountApiKey,
                         accountSecretKey,
                         identity ?? Guid.NewGuid().ToString(),
                         grants: new HashSet<IGrant> { new VideoGrant() }).ToJwt();

        public async Task<IEnumerable<RoomDetails>> GetAllRoomsAsync()
        {
            var rooms = await RoomResource.ReadAsync();
            var tasks = rooms.Select(
                room => GetRoomDetailsAsync(
                    room,
                    ParticipantResource.ReadAsync(
                        room.Sid,
                        ParticipantStatus.Connected)));

            return await Task.WhenAll(tasks);

            async Task<RoomDetails> GetRoomDetailsAsync(
                RoomResource room,
                Task<ResourceSet<ParticipantResource>> participantTask)
            {
                var participants = await participantTask;
                return new RoomDetails
                {
                    Name = room.UniqueName,
                    MaxParticipants = room.MaxParticipants ?? 0,
                    ParticipantCount = participants.ToList().Count
                };
            }
        }
    }
}