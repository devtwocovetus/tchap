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
    public class SPITemplatesController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public SPITemplatesController()
        {
            con = new ConnectionClass();
            //Db = con.Db();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/SPITemplates/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_SurgicalProcedureInformationTemplates SPIT)
        {
            Db = con.SurgeryCenterDb(SPIT.Slug);
            SPITemplateResponse Response = new SPITemplateResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SPIT.Temp_Unique_ID = UniqueID;
                SPIT.Temp_Create_Date = con.ConvertTimeZone(SPIT.Temp_TimeZone, Convert.ToDateTime(SPIT.Temp_Create_Date));
                SPIT.Temp_Modify_Date = con.ConvertTimeZone(SPIT.Temp_TimeZone, Convert.ToDateTime(SPIT.Temp_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_SurgicalProcedureInformationTemplates").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SPIT);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPIT;
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


        [Route("API/SPITemplates/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_SurgicalProcedureInformationTemplates SPIT)
        {
            Db = con.SurgeryCenterDb(SPIT.Slug);
            SPITemplateResponse Response = new SPITemplateResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_SurgicalProcedureInformationTemplates").Document(SPIT.Temp_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPIT;
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


        [Route("API/SPITemplates/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Get(MT_SurgicalProcedureInformationTemplates SPIT)
        {
            Db = con.SurgeryCenterDb(SPIT.Slug);
            SPITemplateResponse Response = new SPITemplateResponse();
            try
            {
                List<MT_SurgicalProcedureInformationTemplates> AnesList = new List<MT_SurgicalProcedureInformationTemplates>();
                Query docRef = Db.Collection("MT_SurgicalProcedureInformationTemplates").WhereEqualTo("Temp_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_SurgicalProcedureInformationTemplates>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Temp_Surgical_Center_Name).ToList();
                }
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/SPITemplates/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_SurgicalProcedureInformationTemplates SPIT)
        {
            Db = con.SurgeryCenterDb(SPIT.Slug);
            SPITemplateResponse Response = new SPITemplateResponse();
            try
            {
                List<MT_SurgicalProcedureInformationTemplates> AnesList = new List<MT_SurgicalProcedureInformationTemplates>();
                Query docRef = Db.Collection("MT_SurgicalProcedureInformationTemplates").WhereEqualTo("Temp_Is_Deleted", false).WhereEqualTo("Temp_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_SurgicalProcedureInformationTemplates>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Temp_Surgical_Center_Name).ToList();
                }
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/SPITemplates/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_SurgicalProcedureInformationTemplates SPIT)
        {
            Db = con.SurgeryCenterDb(SPIT.Slug);
            SPITemplateResponse Response = new SPITemplateResponse();
            try
            {
                List<MT_SurgicalProcedureInformationTemplates> AnesList = new List<MT_SurgicalProcedureInformationTemplates>();
                Query docRef = Db.Collection("MT_SurgicalProcedureInformationTemplates").WhereEqualTo("Temp_Is_Deleted", false).WhereEqualTo("Temp_Unique_ID", SPIT.Temp_Unique_ID);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_SurgicalProcedureInformationTemplates>());
                    }
                    Response.DataList = AnesList;
                }
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
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
