using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;
using Twilio.Clients;
using Twilio.TwiML;

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
        public HttpResponseMessage ResponseSMS(string From, string To, string Body)
        {
            SmsResponse Response = new SmsResponse();
            try
            {
                var response = new VoiceResponse();
                response.Say("Hello World");
                Response.Message = $"<Response><Message>{response.ToString()}</Message></Response>".ToString();
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
