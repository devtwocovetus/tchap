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
using System.IO;

namespace TheCloudHealth.Controllers
{
    public class AnesthesiaController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        //FirestoreDb fireStoreDb;
        string UniqueID = "";
        public AnesthesiaController()
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



        [Route("API/Anesthesia/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                AMD.Anes_Unique_ID = UniqueID;
                AMD.Anes_Create_Date = con.ConvertTimeZone(AMD.Anes_TimeZone, Convert.ToDateTime(AMD.Anes_Create_Date));
                AMD.Anes_Modify_Date = con.ConvertTimeZone(AMD.Anes_TimeZone, Convert.ToDateTime(AMD.Anes_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Anesthesia").Document(UniqueID);
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

        [Route("API/Anesthesia/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Anes_Type" , AMD.Anes_Type},
                    {"Anes_Name" , AMD.Anes_Name},
                    {"Anes_Modify_Date" , con.ConvertTimeZone(AMD.Anes_TimeZone,Convert.ToDateTime(AMD.Anes_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Anesthesia").Document(AMD.Anes_Unique_ID);
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

        [Route("API/Anesthesia/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Anes_Modify_Date" , con.ConvertTimeZone(AMD.Anes_TimeZone,Convert.ToDateTime(AMD.Anes_Modify_Date))},
                    {"Anes_Is_Active" , AMD.Anes_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Anesthesia").Document(AMD.Anes_Unique_ID);
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

        [Route("API/Anesthesia/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Anes_Modify_Date" , con.ConvertTimeZone(AMD.Anes_TimeZone,Convert.ToDateTime(AMD.Anes_Modify_Date))},
                    {"Anes_Is_Deleted" , AMD.Anes_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Anesthesia").Document(AMD.Anes_Unique_ID);
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

        [Route("API/Anesthesia/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAsync(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Anesthesia").Document(AMD.Anes_Unique_ID);
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

        [Route("API/Anesthesia/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                MT_Anesthesia Anesthesia = new MT_Anesthesia();
                Query docRef = Db.Collection("MT_Anesthesia").WhereEqualTo("Anes_Unique_ID", AMD.Anes_Unique_ID).WhereEqualTo("Anes_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Anesthesia = ObjQuerySnap.Documents[0].ConvertTo<MT_Anesthesia>();
                    Response.Data = Anesthesia;
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

        [Route("API/Anesthesia/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                List<MT_Anesthesia> AnesList = new List<MT_Anesthesia>();
                Query docRef = Db.Collection("MT_Anesthesia").WhereEqualTo("Anes_Is_Deleted", false).OrderBy("Anes_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Anesthesia>());
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

        [Route("API/Anesthesia/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                List<MT_Anesthesia> AnesList = new List<MT_Anesthesia>();
                Query docRef = Db.Collection("MT_Anesthesia").WhereEqualTo("Anes_Is_Deleted", false).WhereEqualTo("Anes_Is_Active", true).OrderBy("Anes_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Anesthesia>());
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

        [Route("API/Anesthesia/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Anesthesia AMD)
        {
            Db = con.SurgeryCenterDb(AMD.Slug);
            AnesthesiaResponse Response = new AnesthesiaResponse();
            try
            {
                List<MT_Anesthesia> AnesList = new List<MT_Anesthesia>();
                Query docRef = Db.Collection("MT_Anesthesia").WhereEqualTo("Anes_Is_Deleted", true).OrderBy("Anes_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Anesthesia>());
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
