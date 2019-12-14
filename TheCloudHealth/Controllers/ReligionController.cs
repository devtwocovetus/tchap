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
    public class ReligionController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public ReligionController()
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

        [Route("API/Religion/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                NMD.Reli_Unique_ID = UniqueID;
                NMD.Reli_Create_Date = con.ConvertTimeZone(NMD.Reli_TimeZone, Convert.ToDateTime(NMD.Reli_Create_Date));
                NMD.Reli_Modify_Date = con.ConvertTimeZone(NMD.Reli_TimeZone, Convert.ToDateTime(NMD.Reli_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Religion").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(NMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Religion/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Reli_Name", NMD.Reli_Name },
                    { "Reli_Is_Active", NMD.Reli_Is_Active },
                    { "Reli_Is_Deleted", NMD.Reli_Is_Deleted },
                    { "Reli_Modify_Date",con.ConvertTimeZone(NMD.Reli_TimeZone, Convert.ToDateTime(NMD.Reli_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Religion").Document(NMD.Reli_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Religion/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Reli_Is_Active", NMD.Reli_Is_Active },
                    { "Reli_Modify_Date",con.ConvertTimeZone(NMD.Reli_TimeZone, Convert.ToDateTime(NMD.Reli_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Religion").Document(NMD.Reli_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Religion/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Reli_Is_Deleted", NMD.Reli_Is_Deleted },
                    { "Reli_Modify_Date",con.ConvertTimeZone(NMD.Reli_TimeZone, Convert.ToDateTime(NMD.Reli_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Religion").Document(NMD.Reli_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Religion/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Religion").Document(NMD.Reli_Unique_ID);
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
        [Route("API/Religion/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                MT_Religion reli = new MT_Religion();
                Query docRef = Db.Collection("MT_Religion").WhereEqualTo("Reli_Unique_ID", NMD.Reli_Unique_ID).WhereEqualTo("Reli_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    reli = ObjQuerySnap.Documents[0].ConvertTo<MT_Religion>();
                    Response.Data = reli;
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

        [Route("API/Religion/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                List<MT_Religion> AnesList = new List<MT_Religion>();
                Query docRef = Db.Collection("MT_Religion").WhereEqualTo("Reli_Is_Deleted", false).OrderBy("Reli_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Religion>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Reli_Name).ToList();
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

        [Route("API/Religion/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                List<MT_Religion> AnesList = new List<MT_Religion>();
                Query docRef = Db.Collection("MT_Religion").WhereEqualTo("Reli_Is_Deleted", false).WhereEqualTo("Reli_Is_Active", true).OrderBy("Reli_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Religion>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Reli_Name).ToList();
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

        [Route("API/Religion/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Religion NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            ReligionResponse Response = new ReligionResponse();
            try
            {
                List<MT_Religion> AnesList = new List<MT_Religion>();
                Query docRef = Db.Collection("MT_Religion").WhereEqualTo("Reli_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Religion>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Reli_Name).ToList();
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
