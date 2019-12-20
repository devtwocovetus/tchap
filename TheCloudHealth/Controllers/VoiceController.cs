using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;
using Twilio.TwiML.Voice;
using Twilio.TwiML;
namespace TheCloudHealth.Controllers
{
    public class VoiceController : ApiController
    {
        ConnectionClass con;
        ITwilioVoice ObjVoice;
        public VoiceController()
        {
            con = new ConnectionClass();
            ObjVoice = new TwilioVoice();
        }
        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Voice/SetCall")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage SetCall(Voice voice)
        {
            VoiResponse Response = new VoiResponse();
            try
            {
                var message = ObjVoice.SetVoiceCall(voice);
                Response.Status = con.StatusSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Voice/GetCall")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage GetCall()
        {

            var message = ObjVoice.ResponseVoiceCall();
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(message.ToString(), Encoding.UTF8, "text/xml");
            return res;
        }

        [Route("API/Voice/Welcome")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage Welcome()
        {
            var response = new Twilio.TwiML.VoiceResponse();

            var gather = new Gather(action: new Uri(string.Format("http://tchapi.thecloudhealth.com/API/Voice/Show?digits="))
                , numDigits: 1
                );
            gather.Say("Hi sameer, you have appointment tomorrow at oak surgery center " +
                       //"DoctorName will be doing the surgery. " +
                       "Press 1 to confirm the appointment, press 2 to cancelled the appointment.");
            response.Append(gather);
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(response.ToString(), Encoding.UTF8, "text/xml");
            return res;
        }

        [Route("API/Voice/Show")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage Show(string digits)
        {
            var response = new Twilio.TwiML.VoiceResponse();
            var selectedOption = digits;
            switch (selectedOption)
            {
                case "1":
                    response.Say("Appointment confirmed, Thank you for calling");
                    break;
                case "2":
                    response.Say("Appointment cancelled, Thank you for calling");
                    break;
            }
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(response.ToString(), Encoding.UTF8, "text/xml");
            return res;
        }
    }
}
