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
    public class LanguageController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public LanguageController()
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


        [Route("API/Language/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                LMD.Lang_Unique_ID = UniqueID;
                LMD.Lang_Create_Date = con.ConvertTimeZone(LMD.Lang_TimeZone, Convert.ToDateTime(LMD.Lang_Create_Date));
                LMD.Lang_Modify_Date = con.ConvertTimeZone(LMD.Lang_TimeZone, Convert.ToDateTime(LMD.Lang_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Language").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(LMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LMD;
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

        [Route("API/Language/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Lang_Name", LMD.Lang_Name },
                    { "Lang_Shotname", LMD.Lang_Shotname },
                    { "Lang_Modify_Date",con.ConvertTimeZone(LMD.Lang_TimeZone, Convert.ToDateTime(LMD.Lang_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Language").Document(LMD.Lang_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LMD;
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

        [Route("API/Language/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Lang_Modify_Date",con.ConvertTimeZone(LMD.Lang_TimeZone, Convert.ToDateTime(LMD.Lang_Modify_Date))},
                    { "Lang_Is_Active",LMD.Lang_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Language").Document(LMD.Lang_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LMD;
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

        [Route("API/Language/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Lang_Modify_Date",con.ConvertTimeZone(LMD.Lang_TimeZone, Convert.ToDateTime(LMD.Lang_Modify_Date))},
                    { "Lang_Is_Deleted",LMD.Lang_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Language").Document(LMD.Lang_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LMD;
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

        [Route("API/Language/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Language").Document(LMD.Lang_Unique_ID);
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


        [Route("API/Language/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                MT_Language Lang = new MT_Language();
                Query docRef = Db.Collection("MT_Language").WhereEqualTo("Lang_Unique_ID", LMD.Lang_Unique_ID).WhereEqualTo("Lang_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Lang = ObjQuerySnap.Documents[0].ConvertTo<MT_Language>();
                    Response.Data = Lang;
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

        [Route("API/Language/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                List<MT_Language> AnesList = new List<MT_Language>();
                Query docRef = Db.Collection("MT_Language").WhereEqualTo("Lang_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Language>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Lang_Name).ToList();
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

        [Route("API/Language/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                List<MT_Language> AnesList = new List<MT_Language>();
                Query docRef = Db.Collection("MT_Language").WhereEqualTo("Lang_Is_Deleted", false).WhereEqualTo("Lang_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Language>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Lang_Name).ToList();
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

        [Route("API/Language/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LanguageResponse Response = new LanguageResponse();
            try
            {
                List<MT_Language> AnesList = new List<MT_Language>();
                Query docRef = Db.Collection("MT_Language").WhereEqualTo("Lang_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Language>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Lang_Name).ToList();
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
