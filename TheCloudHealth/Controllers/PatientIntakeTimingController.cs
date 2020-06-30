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
    public class PatientIntakeTimingController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public PatientIntakeTimingController()
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

        [Route("API/PatientIntakeTiming/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_PatientIntakeTiming PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatInTimingResponse Response = new PatInTimingResponse();
            try
            {
                MT_PatientIntakeTiming timing = new MT_PatientIntakeTiming();
                Query ObjQuery = Db.Collection("MT_PatientIntakeTiming").WhereEqualTo("PITT_Is_Deleted", false).WhereEqualTo("PITT_Is_Active", true).WhereEqualTo("PITT_Surgery_Physician_Center_ID", PIMD.PITT_Surgery_Physician_Center_ID).OrderByDescending("PITT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap.Documents.Count == 0)
                {
                    UniqueID = con.GetUniqueKey();
                    PIMD.PITT_Unique_ID = UniqueID;
                    PIMD.PITT_Create_Date = con.ConvertTimeZone(PIMD.PITT_TimeZone, Convert.ToDateTime(PIMD.PITT_Create_Date));
                    PIMD.PITT_Modify_Date = con.ConvertTimeZone(PIMD.PITT_TimeZone, Convert.ToDateTime(PIMD.PITT_Modify_Date));
                    DocumentReference docRef = Db.Collection("MT_PatientIntakeTiming").Document(UniqueID);
                    WriteResult Result = await docRef.SetAsync(PIMD);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = PIMD;
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
                    timing = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientIntakeTiming>();
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PITT_Time", PIMD.PITT_Time},
                        {"PITT_Modify_Date", con.ConvertTimeZone(PIMD.PITT_TimeZone, Convert.ToDateTime(PIMD.PITT_Modify_Date))},
                    };

                    DocumentReference docRef = Db.Collection("MT_PatientIntakeTiming").Document(timing.PITT_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = PIMD;
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

        [Route("API/PatientIntakeTiming/Edit")]
        [HttpPost]
        public async Task<HttpResponseMessage> Edit(MT_PatientIntakeTiming PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatInTimingResponse Response = new PatInTimingResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PITT_Time", PIMD.PITT_Time},
                        {"PITT_Modify_Date", con.ConvertTimeZone(PIMD.PITT_TimeZone, Convert.ToDateTime(PIMD.PITT_Modify_Date))},
                    };

                DocumentReference docRef = Db.Collection("MT_PatientIntakeTiming").Document(PIMD.PITT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntakeTiming/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_PatientIntakeTiming PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatInTimingResponse Response = new PatInTimingResponse();
            try
            {
                MT_PatientIntakeTiming timing = new MT_PatientIntakeTiming();
                Query ObjQuery = Db.Collection("MT_PatientIntakeTiming").WhereEqualTo("PITT_Is_Deleted", false).WhereEqualTo("PITT_Is_Active", true).WhereEqualTo("PITT_Unique_ID", PIMD.PITT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientIntakeTiming>();
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

        [Route("API/PatientIntakeTiming/GetPatientIntakeTiming")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetNotificationTiming(MT_PatientIntakeTiming PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatInTimingResponse Response = new PatInTimingResponse();
            try
            {
                Query ObjQuery = Db.Collection("MT_PatientIntakeTiming").WhereEqualTo("PITT_Is_Deleted", false).WhereEqualTo("PITT_Is_Active", true).WhereEqualTo("PITT_Surgery_Physician_Center_ID", PIMD.PITT_Surgery_Physician_Center_ID).OrderByDescending("PITT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null && ObjQuerySnap.Documents.Count > 0)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientIntakeTiming>();
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
