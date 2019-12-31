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

namespace TheCloudHealth.Controllers
{
    public class SlugController : ApiController
    {
        FirestoreDb Db;
        //FirestoreDb DbLog;

        ConnectionClass con;
        string UniqueID = "";
        public SlugController()
        {
            con = new ConnectionClass();
            Db = con.Db();
            //DbLog = con.DbLog();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Slug/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Slug SMD)
        {
            SlugMResponse Response = new SlugMResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SMD.Slug_Unique_ID = UniqueID;
                DocumentReference docRef = Db.Collection("MT_Slug").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/Slug/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Slug SMD)
        {
            SlugMResponse Response = new SlugMResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Staff_Name",SMD.Slug_Name},
                    {"Slug_Is_Active",SMD.Slug_Is_Active},
                    {"Slug_Is_Deleted",SMD.Slug_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Slug").Document(SMD.Slug_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
                }
                else
                {
                    Response.Status = con.StatusNotUpdate;
                    Response.Message = con.MessageNotUpdate;
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
        
        [Route("API/Slug/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(MT_Slug SMD)
        {
            SlugMResponse Response = new SlugMResponse();
            try
            {
                MT_Slug staff = new MT_Slug();
                Query docRef = Db.Collection("MT_Slug").WhereEqualTo("Slug_SCPO_ID", SMD.Slug_SCPO_ID).WhereEqualTo("Slug_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_Slug>();
                }
                else
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
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
