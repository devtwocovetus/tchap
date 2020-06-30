using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCloudHealth.Domain;
using TheCloudHealth.Models;

namespace TheCloudHealth.Controllers
{
    public class TokenController : ApiController
    {
        private readonly ITokenGenerator _tokenGenerator;

        public TokenController() : this(new TokenGenerator()) { }

        public TokenController(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Token/Create")]
        [HttpPost]
        public HttpResponseMessage Create(Token tok)
        {
            if (tok.device == null || tok.identity == null) return null;

            const string appName = "TwilioChatDemo";
            var endpointId = string.Format("{0}:{1}:{2}", appName, tok.identity, tok.device);

            var token = _tokenGenerator.Generate(tok.identity, endpointId);
            return ConvertToJSON(new { tok.identity, token });
        }

        [Route("API/Token/GetToken")]
        [HttpGet]
        public HttpResponseMessage GetToken(string Identity)
        {
            Token tok = new Token();
            tok.device = "browser";
            tok.identity = Identity;
            if (tok.device == null || tok.identity == null) return null;
            var token = _tokenGenerator.GenerateForVideo(Identity);
            return ConvertToJSON(new { tok.identity, token });
        }
    }
}
