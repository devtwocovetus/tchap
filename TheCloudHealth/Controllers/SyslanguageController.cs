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
    public class SyslanguageController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public SyslanguageController()
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


        [Route("API/Syslanguage/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                LMD.Lan_Unique_ID = UniqueID;
                LMD.Lan_Create_Date = con.ConvertTimeZone(LMD.Lan_TimeZone, Convert.ToDateTime(LMD.Lan_Create_Date));
                LMD.Lan_Modify_Date = con.ConvertTimeZone(LMD.Lan_TimeZone, Convert.ToDateTime(LMD.Lan_Modify_Date));
                DocumentReference docRef = Db.Collection("Language").Document(UniqueID);
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

        [Route("API/Syslanguage/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Lan_Name", LMD.Lan_Name },
                    { "Lan_Shotname", LMD.Lan_Shotname },
                    { "Lan_Modify_Date",con.ConvertTimeZone(LMD.Lan_TimeZone, Convert.ToDateTime(LMD.Lan_Modify_Date))},
                    { "Lan_Is_Active",LMD.Lan_Is_Active},
                    { "Lan_Is_Deleted",LMD.Lan_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("Language").Document(LMD.Lan_Unique_ID);
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

        [Route("API/Syslanguage/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Lan_Modify_Date",con.ConvertTimeZone(LMD.Lan_TimeZone, Convert.ToDateTime(LMD.Lan_Modify_Date))},
                    { "Lan_Is_Active",LMD.Lan_Is_Active}
                };

                DocumentReference docRef = Db.Collection("Language").Document(LMD.Lan_Unique_ID);
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


        [Route("API/Syslanguage/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Lan_Modify_Date",con.ConvertTimeZone(LMD.Lan_TimeZone, Convert.ToDateTime(LMD.Lan_Modify_Date))},
                    { "Lan_Is_Deleted",LMD.Lan_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("Language").Document(LMD.Lan_Unique_ID);
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

        [Route("API/Syslanguage/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                DocumentReference docRef = Db.Collection("Language").Document(LMD.Lan_Unique_ID);
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
        [Route("API/Syslanguage/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                Language Lang = new Language();
                Query docRef = Db.Collection("Language").WhereEqualTo("Lan_Unique_ID", LMD.Lan_Unique_ID).WhereEqualTo("Lan_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Lang = ObjQuerySnap.Documents[0].ConvertTo<Language>();
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

        [Route("API/Syslanguage/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                List<Language> AnesList = new List<Language>();
                Query docRef = Db.Collection("Language").WhereEqualTo("Lan_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<Language>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Lan_Name).ToList();
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

        [Route("API/Syslanguage/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                List<Language> AnesList = new List<Language>();
                Query docRef = Db.Collection("Language").WhereEqualTo("Lan_Is_Deleted", false).WhereEqualTo("Lan_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<Language>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Lan_Name).ToList();
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

        [Route("API/Syslanguage/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(Language LMD)
        {
            Db = con.SurgeryCenterDb(LMD.Slug);
            LangResponse Response = new LangResponse();
            try
            {
                List<Language> AnesList = new List<Language>();
                Query docRef = Db.Collection("Language").WhereEqualTo("Lan_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<Language>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Lan_Name).ToList();
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
