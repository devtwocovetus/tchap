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
    public class AlertController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public AlertController()
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

        [Route("API/Alert/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                AMD.Alert_Unique_ID = UniqueID;
                AMD.Alert_Create_Date = con.ConvertTimeZone(AMD.Alert_TimeZone, Convert.ToDateTime(AMD.Alert_Create_Date));
                AMD.Alert_Modify_Date = con.ConvertTimeZone(AMD.Alert_TimeZone, Convert.ToDateTime(AMD.Alert_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Alert").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(AMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = AMD;
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

        [Route("API/Alert/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Alert_Name" , AMD.Alert_Name},
                    {"Alert_Modify_Date" , con.ConvertTimeZone(AMD.Alert_TimeZone,Convert.ToDateTime(AMD.Alert_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Alert").Document(AMD.Alert_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = AMD;
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

        [Route("API/Alert/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Alert_Modify_Date" , con.ConvertTimeZone(AMD.Alert_TimeZone,Convert.ToDateTime(AMD.Alert_Modify_Date))},
                    {"Alert_Is_Active" , AMD.Alert_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Alert").Document(AMD.Alert_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = AMD;
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

        [Route("API/Alert/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Alert_Modify_Date" , con.ConvertTimeZone(AMD.Alert_TimeZone,Convert.ToDateTime(AMD.Alert_Modify_Date))},
                    {"Alert_Is_Deleted" , AMD.Alert_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Alert").Document(AMD.Alert_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = AMD;
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

        [Route("API/Alert/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Alert").Document(AMD.Alert_Unique_ID);
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

        [Route("API/Alert/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                MT_Alert Inci = new MT_Alert();
                Query docRef = Db.Collection("MT_Alert").WhereEqualTo("Alert_Unique_ID", AMD.Alert_Unique_ID).WhereEqualTo("Alert_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_Alert>();
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

        [Route("API/Alert/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                List<MT_Alert> AnesList = new List<MT_Alert>();
                Query docRef = Db.Collection("MT_Alert").WhereEqualTo("Alert_Is_Deleted", false).OrderBy("Alert_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Alert>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Alert_Name).ToList();
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

        [Route("API/Alert/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                List<MT_Alert> AnesList = new List<MT_Alert>();
                Query docRef = Db.Collection("MT_Alert").WhereEqualTo("Alert_Is_Deleted", false).WhereEqualTo("Alert_Is_Active", true).OrderBy("Alert_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Alert>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Alert_Name).ToList();
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

        [Route("API/Alert/GetAlertListFilterWithSCPO")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetAlertListFilterWithSCPO(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                List<MT_Alert> AnesList = new List<MT_Alert>();
                MT_Alert Alert = new MT_Alert();
                Query docRef = Db.Collection("MT_Alert").WhereEqualTo("Alert_Is_Deleted", false).WhereEqualTo("Alert_Is_Active", true).OrderBy("Alert_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        Alert = Docsnapshot.ConvertTo<MT_Alert>();
                        if (AMD.Alert_Surgery_Physician_Id == Alert.Alert_Surgery_Physician_Id)
                        {
                            AnesList.Add(Alert);
                        }
                        else if (Alert.Alert_Surgery_Physician_Id == "0")
                        {
                            AnesList.Add(Alert);
                        }
                        
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Alert_Name).ToList();
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

        [Route("API/Alert/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Alert AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AlertResponse Response = new AlertResponse();
            try
            {
                List<MT_Alert> AnesList = new List<MT_Alert>();
                Query docRef = Db.Collection("MT_Alert").WhereEqualTo("Alert_Is_Deleted", true).WhereEqualTo("Alert_Create_By", AMD.Alert_Create_By);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Alert>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Alert_Name).ToList();
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
