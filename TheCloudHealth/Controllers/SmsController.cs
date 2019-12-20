using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;
using Twilio.Clients;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace TheCloudHealth.Controllers
{
    public class SmsController : ApiController
    {
        ConnectionClass con;
        ITwilioSMS Objsms;
        public SmsController()
        {
            con = new ConnectionClass();
            Objsms = new TwilioSMS();
        }
        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        public HttpResponseMessage ConvertToXML(MessagingResponse objectToConvert)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("<Response/>", Encoding.UTF8, "text/xml");
            return response;
        }

        [Route("API/Sms/SendSMS")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage SendSMS(Sms sms)
        {
            SmsResponse Response = new SmsResponse();
            try
            {
                var message = Objsms.SendSMS(sms);
                Response.Message = $"<Response><Message>{message}</Message></Response>".ToString();
                Response.Status = con.StatusSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Sms/ResponseSMS")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage ResponseSMS()
        {
            var message = Objsms.ResponseSMS();
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(message.ToString(), Encoding.UTF8, "text/xml");
            return res;
        }

        [Route("API/Sms/GenTwiML")]
        [HttpGet]
        public HttpResponseMessage GenTwiML(string Text)
        {
            var response = new MessagingResponse();
            var res = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                var message = new Message();
                message.Body(Text);
                response.Append(message);
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/XML/voice.xml"), response.ToString(), Encoding.ASCII);
                res.Content = new StringContent(response.ToString(), Encoding.UTF8, "text/xml");
                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }


        [Route("API/Sms/MessageToWhatsapp")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage MessageToWhatsapp(Sms sms)
        {
            SmsResponse Response = new SmsResponse();
            try
            {
                var message = Objsms.SendMessageWithWhatsapp(sms);
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
