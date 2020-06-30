using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;

namespace TheCloudHealth.Controllers
{
    public class VideoController : ApiController
    {
        ITwilioVideo ObjVideo;
        ConnectionClass con;
        public VideoController()
        {
            ObjVideo = new TwilioVideo();
            con = new ConnectionClass();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Video/CreateRoom")]
        [HttpPost]
        public HttpResponseMessage CreateRoom(RoomDetails room)
        {
            RoomResponse Response = new RoomResponse();
            RoomDetails roomdetail = new RoomDetails();
            try
            {
                roomdetail = ObjVideo.CreateRoom(room.Name);
                Response.Status = con.StatusSuccess;
                Response.Data = roomdetail;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Video/GetRoomDetailsByName")]
        [HttpPost]
        public HttpResponseMessage GetRoomDetailsByName(RoomDetails room)
        {
            RoomResponse Response = new RoomResponse();
            RoomDetails roomdetail = new RoomDetails();
            try
            {
                roomdetail = ObjVideo.GetRoomByUniqueName(room.Name);
                Response.Status = con.StatusSuccess;
                Response.Data = roomdetail;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Video/GetRoomDetailsByID")]
        [HttpPost]
        public HttpResponseMessage GetRoomDetailsByID(RoomDetails room)
        {
            RoomResponse Response = new RoomResponse();
            RoomDetails roomdetail = new RoomDetails();
            try
            {
                roomdetail = ObjVideo.GetRoomBySID(room.Id);
                Response.Status = con.StatusSuccess;
                Response.Data = roomdetail;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Video/GetCompletedRoomList")]
        [HttpPost]
        public HttpResponseMessage GetCompletedRoomList(RoomDetails room)
        {
            RoomResponse Response = new RoomResponse();
            List<RoomDetails> roomlist = new List<RoomDetails>();
            try
            {
                roomlist = ObjVideo.GetRoomListByStatus(room.Status);
                Response.Status = con.StatusSuccess;
                Response.DataList = roomlist;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }
    }
}
