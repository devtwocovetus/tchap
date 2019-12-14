using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;

namespace TheCloudHealth.Controllers
{
    public class EmailController : ApiController
    {
        ConnectionClass con;
        IEmailer ObjEmailer;
        public EmailController() 
        {
            con = new ConnectionClass();
            ObjEmailer = new Emailer();
        }
        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Email/SendMail")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> SendMail(Email email)
        {
            EmailResponse Response = new EmailResponse();
            try
            {
                var message = await ObjEmailer.Send(email);
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
    }
}
