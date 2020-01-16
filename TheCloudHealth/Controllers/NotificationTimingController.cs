using Google.Cloud.Firestore;
using System;
using TheCloudHealth.Models;
using System.Threading.Tasks;
using TheCloudHealth.Lib;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace TheCloudHealth.Controllers
{
    public class NotificationTimingController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public NotificationTimingController()
        {
            con = new ConnectionClass();
        }
        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/NotificationTiming/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_NotificationTiming NTMD)
        {
            Db = con.SurgeryCenterDb(NTMD.Slug);
            NotiTimingResponse Response = new NotiTimingResponse();
            try
            {
                MT_NotificationTiming timing = new MT_NotificationTiming();
                Query ObjQuery = Db.Collection("MT_NotificationTiming").WhereEqualTo("NTT_Is_Deleted", false).WhereEqualTo("NTT_Is_Active", true).WhereEqualTo("NTT_Surgery_Physician_Center_ID", NTMD.NTT_Surgery_Physician_Center_ID).OrderByDescending("NTT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap.Documents.Count == 0)
                {
                    UniqueID = con.GetUniqueKey();
                    NTMD.NTT_Unique_ID = UniqueID;
                    NTMD.NTT_Create_Date = con.ConvertTimeZone(NTMD.NTT_TimeZone, Convert.ToDateTime(NTMD.NTT_Create_Date));
                    NTMD.NTT_Modify_Date = con.ConvertTimeZone(NTMD.NTT_TimeZone, Convert.ToDateTime(NTMD.NTT_Modify_Date));
                    DocumentReference docRef = Db.Collection("MT_NotificationTiming").Document(UniqueID);
                    WriteResult Result = await docRef.SetAsync(NTMD);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = NTMD;
                    }
                    else
                    {
                        Response.Status = con.StatusNotInsert;
                        Response.Message = con.MessageNotInsert;
                        Response.Data = null;
                    }
                }

                else
                {
                    timing = ObjQuerySnap.Documents[0].ConvertTo<MT_NotificationTiming>();
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"NTT_Time", NTMD.NTT_Time},
                        {"NTT_Modify_Date", con.ConvertTimeZone(NTMD.NTT_TimeZone, Convert.ToDateTime(NTMD.NTT_Modify_Date))},
                    };

                    DocumentReference docRef = Db.Collection("MT_NotificationTiming").Document(timing.NTT_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = NTMD;
                    }
                    else
                    {
                        Response.Status = con.StatusNotInsert;
                        Response.Message = con.MessageNotInsert;
                        Response.Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/NotificationTiming/Edit")]
        [HttpPost]
        public async Task<HttpResponseMessage> Edit(MT_NotificationTiming NTMD)
        {
            Db = con.SurgeryCenterDb(NTMD.Slug);
            NotiTimingResponse Response = new NotiTimingResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"NTT_Time", NTMD.NTT_Time},
                        {"NTT_Modify_Date", con.ConvertTimeZone(NTMD.NTT_TimeZone, Convert.ToDateTime(NTMD.NTT_Modify_Date))},
                    };

                DocumentReference docRef = Db.Collection("MT_NotificationTiming").Document(NTMD.NTT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NTMD;
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

        [Route("API/NotificationTiming/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_NotificationTiming NTMD)
        {
            Db = con.SurgeryCenterDb(NTMD.Slug);
            NotiTimingResponse Response = new NotiTimingResponse();
            try
            {
                MT_NotificationTiming timing = new MT_NotificationTiming();
                Query ObjQuery = Db.Collection("MT_NotificationTiming").WhereEqualTo("NTT_Is_Deleted", false).WhereEqualTo("NTT_Is_Active", true).WhereEqualTo("NTT_Unique_ID", NTMD.NTT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_NotificationTiming>();
                    Response.Message = con.MessageSuccess;
                    Response.Status = con.StatusSuccess;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/NotificationTiming/GetNotificationTiming")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetNotificationTiming(MT_NotificationTiming NTMD)
        {
            Db = con.SurgeryCenterDb(NTMD.Slug);
            NotiTimingResponse Response = new NotiTimingResponse();
            try
            {
                Query ObjQuery = Db.Collection("MT_NotificationTiming").WhereEqualTo("NTT_Is_Deleted", false).WhereEqualTo("NTT_Is_Active", true).WhereEqualTo("NTT_Surgery_Physician_Center_ID", NTMD.NTT_Surgery_Physician_Center_ID).OrderByDescending("NTT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null && ObjQuerySnap.Documents.Count > 0)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_NotificationTiming>();
                    Response.Message = con.MessageSuccess;
                    Response.Status = con.StatusSuccess;
                }
                else
                {
                    Response.Message = con.MessageDNE;
                    Response.Status = con.StatusDNE;
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
