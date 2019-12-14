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
    public class CloudUploadController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        //string UniqueSettingID = "";

        public CloudUploadController()
        {
            con = new ConnectionClass();
            Db = con.Db();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/CloudUpload/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Physician_Office POMD)
        {
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                List<MT_Specilities> SpelitiesList = new List<MT_Specilities>();
                UniqueID = con.GetUniqueKey();
                POMD.PhyO_Unique_ID = UniqueID;
                POMD.PhyO_Create_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Create_Date));
                POMD.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));

                if (POMD.PhyO_ContactSetting != null)
                {
                    foreach (MT_PhyO_Contact_Setting conset in POMD.PhyO_ContactSetting)
                    {
                        conset.PCS_Unique_ID = con.GetUniqueKey();
                    }
                }
                DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(POMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = POMD;
                }
                else
                {
                    Response.Status = con.StatusNotInsert;
                    Response.Message = con.MessageNotInsert;
                    Response.Data = null;
                }

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
