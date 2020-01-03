using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using TheCloudHealth.Models;
using System.Threading.Tasks;
using TheCloudHealth.Lib;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TheCloudHealth.Controllers
{
    public class FileUploadController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        private readonly ImageUploader _imageUploader;
        public FileUploadController()
        {
            con = new ConnectionClass();
            //_imageUploader = new ImageUploader("thecloudhealth_bucket-1");
            //Db = con.Db();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/FileUpload/Upload")]
        [HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            string imageUrl = "";
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        int hasheddate = DateTime.Now.GetHashCode();

                        string changed_name = DateTime.Now.ToString("yyyyMMddHHmmss") + postedFile.FileName.Substring(postedFile.FileName.IndexOf('.'), (postedFile.FileName.Length - postedFile.FileName.IndexOf('.')));

                       // imageUrl = await _imageUploader.UploadImage(postedFile, 10);
                    }
                }
                Response.Status = con.StatusSuccess;
                Response.Message = "Path : " + imageUrl;
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
