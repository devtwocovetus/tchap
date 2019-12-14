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
using System.Linq;

namespace TheCloudHealth.Controllers
{
    public class IncidentController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public IncidentController()
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

        [Route("API/Incident/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                IMD.Inci_Unique_ID = UniqueID;
                IMD.Inci_Create_Date = con.ConvertTimeZone(IMD.Inci_TimeZone, Convert.ToDateTime(IMD.Inci_Create_Date));
                IMD.Inci_Modify_Date = con.ConvertTimeZone(IMD.Inci_TimeZone, Convert.ToDateTime(IMD.Inci_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Incident").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(IMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/Incident/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Inci_Name", IMD.Inci_Name },
                    { "Inci_Type", IMD.Inci_Type },
                    { "Inci_Modify_Date",con.ConvertTimeZone(IMD.Inci_TimeZone, Convert.ToDateTime(IMD.Inci_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Incident").Document(IMD.Inci_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/Incident/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Inci_Modify_Date",con.ConvertTimeZone(IMD.Inci_TimeZone, Convert.ToDateTime(IMD.Inci_Modify_Date))},
                    { "Inci_Is_Active",IMD.Inci_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Incident").Document(IMD.Inci_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/Incident/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Inci_Modify_Date",con.ConvertTimeZone(IMD.Inci_TimeZone, Convert.ToDateTime(IMD.Inci_Modify_Date))},
                    { "Inci_Is_Deleted",IMD.Inci_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Incident").Document(IMD.Inci_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/Incident/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Incident").Document(IMD.Inci_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = null;
                }
                else
                {
                    Response.Status = con.StatusNotDeleted;
                    Response.Message = con.MessageNotDeleted;
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

        [Route("API/Incident/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                MT_Incident Inci = new MT_Incident();
                Query docRef = Db.Collection("MT_Incident").WhereEqualTo("Inci_Unique_ID", IMD.Inci_Unique_ID).WhereEqualTo("Inci_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Inci = ObjQuerySnap.Documents[0].ConvertTo<MT_Incident>();
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

        [Route("API/Incident/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                List<MT_Incident> AnesList = new List<MT_Incident>();
                Query docRef = Db.Collection("MT_Incident").WhereEqualTo("Inci_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Incident>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Inci_Name).ToList();
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

        [Route("API/Incident/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                List<MT_Incident> AnesList = new List<MT_Incident>();
                Query docRef = Db.Collection("MT_Incident").WhereEqualTo("Inci_Is_Deleted", false).WhereEqualTo("Inci_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Incident>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Inci_Name).ToList();
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

        [Route("API/Incident/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Incident IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            IncidentResponse Response = new IncidentResponse();
            try
            {
                List<MT_Incident> AnesList = new List<MT_Incident>();
                Query docRef = Db.Collection("MT_Incident").WhereEqualTo("Inci_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Incident>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Inci_Name).ToList();
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
