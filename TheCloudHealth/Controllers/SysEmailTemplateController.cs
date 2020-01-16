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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Net.Http.Headers;

namespace TheCloudHealth.Controllers
{
    public class SysEmailTemplateController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        ICreatePDF ObjPDF;
        string UniqueID = "";
        public SysEmailTemplateController()
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

        [Route("API/SysEmailTemplate/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SEMD.SET_Unique_ID = UniqueID;
                SEMD.SET_Create_Date = con.ConvertTimeZone(SEMD.SET_TimeZone, Convert.ToDateTime(SEMD.SET_Create_Date));
                SEMD.SET_Modify_Date = con.ConvertTimeZone(SEMD.SET_TimeZone, Convert.ToDateTime(SEMD.SET_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_System_EmailTemplates").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SEMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SEMD;
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


        [Route("API/SysEmailTemplate/Edit")]
        [HttpPost]
        public async Task<HttpResponseMessage> Edit(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"SET_Name", SEMD.SET_Name},
                    {"SET_Description", SEMD.SET_Description},
                    {"SET_From_Email", SEMD.SET_From_Email},
                    {"SET_From_Name", SEMD.SET_From_Name},
                    {"SET_CC", SEMD.SET_CC},
                    {"SET_Message", SEMD.SET_Message},
                    {"SET_Footer", SEMD.SET_Footer},
                    {"SET_Modify_Date", con.ConvertTimeZone(SEMD.SET_TimeZone, Convert.ToDateTime(SEMD.SET_Modify_Date))},
                    {"SET_TimeZone", SEMD.SET_TimeZone}
                };
                DocumentReference docRef = Db.Collection("MT_System_EmailTemplates").Document(SEMD.SET_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SEMD;
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

        [Route("API/SysEmailTemplate/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"SET_Is_Active", SEMD.SET_Is_Active},
                    {"SET_Modify_Date", con.ConvertTimeZone(SEMD.SET_TimeZone, Convert.ToDateTime(SEMD.SET_Modify_Date))},
                    {"SET_TimeZone", SEMD.SET_TimeZone}
                };
                DocumentReference docRef = Db.Collection("MT_System_EmailTemplates").Document(SEMD.SET_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SEMD;
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

        [Route("API/SysEmailTemplate/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"SET_Is_Deleted", SEMD.SET_Is_Deleted},
                    {"SET_Modify_Date", con.ConvertTimeZone(SEMD.SET_TimeZone, Convert.ToDateTime(SEMD.SET_Modify_Date))},
                    {"SET_TimeZone", SEMD.SET_TimeZone}
                };
                DocumentReference docRef = Db.Collection("MT_System_EmailTemplates").Document(SEMD.SET_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SEMD;
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

        [Route("API/SysEmailTemplate/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                Query TempQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Active", true).WhereEqualTo("SET_Is_Deleted", false).WhereEqualTo("SET_Unique_ID", SEMD.SET_Unique_ID);
                QuerySnapshot ObjQuerySnap = await TempQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_System_EmailTemplates>();
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                }
                else
                {
                    Response.Data = null;
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/SysEmailTemplate/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                List<MT_System_EmailTemplates> TempList = new List<MT_System_EmailTemplates>();
                Query TempQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Deleted", false).WhereEqualTo("SET_Surgery_Physician_Id", SEMD.SET_Surgery_Physician_Id).OrderBy("SET_Name");
                QuerySnapshot ObjQuerySnap = await TempQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        TempList.Add(Docsnap.ConvertTo<MT_System_EmailTemplates>());
                    }
                    Response.DataList = TempList;
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                }
                else
                {
                    Response.Data = null;
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/SysEmailTemplate/GetDeletedList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDeletedList(MT_System_EmailTemplates SEMD)
        {
            Db = con.SurgeryCenterDb(SEMD.Slug);
            SETemplatesResponse Response = new SETemplatesResponse();
            try
            {
                Query TempQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Deleted", true).WhereEqualTo("SET_Surgery_Physician_Id", SEMD.SET_Surgery_Physician_Id).OrderBy("SET_Name");
                QuerySnapshot ObjQuerySnap = await TempQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_System_EmailTemplates>();
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                }
                else
                {
                    Response.Data = null;
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
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
