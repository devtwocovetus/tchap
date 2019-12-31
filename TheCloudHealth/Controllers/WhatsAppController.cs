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
    public class WhatsAppController : ApiController
    {
        ConnectionClass con;
        ITwilioWhatsApp Objwhatsapp;

        public WhatsAppController()
        {
            con = new ConnectionClass();
            Objwhatsapp = new TwilioWhatsApp();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/WhatsApp/MessageToWhatsapp")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage MessageToWhatsapp(Whatsapp whatsapp)
        {
            SmsResponse Response = new SmsResponse();
            try
            {
                var message = Objwhatsapp.SendMessageWithWhatsapp(whatsapp);
                Response.Message = con.MessageSuccess; ;
                Response.Status = con.StatusSuccess;
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
