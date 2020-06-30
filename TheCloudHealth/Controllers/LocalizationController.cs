using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;

namespace TheCloudHealth.Controllers
{
    public class LocalizationController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";        
        public LocalizationController()
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

        [Route("API/Localization/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Localization LOMD)
        {
            Db = con.SurgeryCenterDb(LOMD.Slug);
            LocalizationResponse Response = new LocalizationResponse();
            try
            {

                UniqueID = con.GetUniqueKey();
                LOMD.Loc_Unique_ID = UniqueID;
                LOMD.Loc_Create_Date = con.ConvertTimeZone(LOMD.Loc_TimeZone, Convert.ToDateTime(LOMD.Loc_Create_Date));
                LOMD.Loc_Modify_Date = con.ConvertTimeZone(LOMD.Loc_TimeZone, Convert.ToDateTime(LOMD.Loc_Modify_Date));

                DocumentReference docRef = Db.Collection("MT_Localization").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(LOMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LOMD;
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

        [Route("API/Localization/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Localization LOMD)
        {
            Db = con.SurgeryCenterDb(LOMD.Slug);
            LocalizationResponse Response = new LocalizationResponse();
            try
            {

                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Loc_Resource_ID",LOMD.Loc_Resource_ID },
                    {"Loc_Resource_Type",LOMD.Loc_Resource_Type},
                    {"Loc_Value",LOMD.Loc_Value },
                    {"Loc_Modify_Date",con.ConvertTimeZone(LOMD.Loc_TimeZone, Convert.ToDateTime(LOMD.Loc_Modify_Date))},
                    {"Loc_TimeZone",LOMD.Loc_TimeZone}
                };

                DocumentReference docRef = Db.Collection("MT_Localization").Document(LOMD.Loc_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LOMD;
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

        [Route("API/Localization/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Localization LOMD)
        {
            Db = con.SurgeryCenterDb(LOMD.Slug);
            LocalizationResponse Response = new LocalizationResponse();
            try
            {

                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Loc_Is_Active",LOMD.Loc_Is_Active },
                    {"Loc_Modify_Date",con.ConvertTimeZone(LOMD.Loc_TimeZone, Convert.ToDateTime(LOMD.Loc_Modify_Date))},
                    {"Loc_TimeZone",LOMD.Loc_TimeZone}
                };

                DocumentReference docRef = Db.Collection("MT_Localization").Document(LOMD.Loc_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LOMD;
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

        [Route("API/Localization/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Localization LOMD)
        {
            Db = con.SurgeryCenterDb(LOMD.Slug);
            LocalizationResponse Response = new LocalizationResponse();
            try
            {

                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Loc_Is_Deleted",LOMD.Loc_Is_Deleted },
                    {"Loc_Modify_Date",con.ConvertTimeZone(LOMD.Loc_TimeZone, Convert.ToDateTime(LOMD.Loc_Modify_Date))},
                    {"Loc_TimeZone",LOMD.Loc_TimeZone}
                };

                DocumentReference docRef = Db.Collection("MT_Localization").Document(LOMD.Loc_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LOMD;
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

        [Route("API/Localization/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Localization LOMD)
        {
            Db = con.SurgeryCenterDb(LOMD.Slug);
            LocalizationResponse Response = new LocalizationResponse();
            try
            {
                List<MT_Localization> Loclist = new List<MT_Localization>();
                Query Qry = Db.Collection("MT_Localization").WhereEqualTo("Loc_Is_Deleted", false);
                QuerySnapshot QSnapshot = await Qry.GetSnapshotAsync();
                if (QSnapshot != null)
                {
                    foreach (DocumentSnapshot Dsnap in QSnapshot.Documents)
                    {
                        Loclist.Add(Dsnap.ConvertTo<MT_Localization>());
                    }
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.DataList = Loclist.OrderBy(o => o.Loc_Resource_ID).ToList();
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
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

        [Route("API/Localization/ListFilterWithLanguageShorname")]
        [HttpPost]
        public async Task<HttpResponseMessage> ListFilterWithLanguageShorname(MT_Localization LOMD)
        {
            Db = con.SurgeryCenterDb(LOMD.Slug);
            LocalizationResponse Response = new LocalizationResponse();
            try
            {
                List<MT_Localization> Loclist = new List<MT_Localization>();
                Query Qry = Db.Collection("MT_Localization").WhereEqualTo("Loc_Is_Deleted", false).WhereEqualTo("Loc_Is_Active", true).WhereEqualTo("Loc_Language_Shortname", LOMD.Loc_Language_Shortname);
                QuerySnapshot QSnapshot = await Qry.GetSnapshotAsync();
                if (QSnapshot != null)
                {
                    foreach (DocumentSnapshot Dsnap in QSnapshot.Documents)
                    {
                        Loclist.Add(Dsnap.ConvertTo<MT_Localization>());
                    }
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.DataList = Loclist.OrderBy(o => o.Loc_Resource_ID).ToList();
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
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
    }
}
