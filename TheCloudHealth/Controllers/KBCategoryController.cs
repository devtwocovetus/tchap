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
    public class KBCategoryController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public KBCategoryController()
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

        [Route("API/KBCategory/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                List<string> List = new List<string>();
                UniqueID = con.GetUniqueKey();
                KCMD.KBC_Unique_ID = UniqueID;
                KCMD.KBC_Create_Date = con.ConvertTimeZone(KCMD.KBC_TimeZone, Convert.ToDateTime(KCMD.KBC_Create_Date));
                KCMD.KBC_Modify_Date = con.ConvertTimeZone(KCMD.KBC_TimeZone, Convert.ToDateTime(KCMD.KBC_Modify_Date));

                DocumentReference docRef = Db.Collection("MT_KBCategory").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(KCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = KCMD;
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

        [Route("API/KBCategory/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                List<string> List = new List<string>();

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KBC_Sub_Category", KCMD.KBC_Sub_Category},
                    {"KBC_Category", KCMD.KBC_Category},
                    {"KBC_Description", KCMD.KBC_Description},
                    {"KBC_Modify_Date", con.ConvertTimeZone(KCMD.KBC_TimeZone, Convert.ToDateTime(KCMD.KBC_Modify_Date))},
                    {"KBC_TimeZone", KCMD.KBC_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_KBCategory").Document(KCMD.KBC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = KCMD;
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


        [Route("API/KBCategory/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KBC_Is_Active", KCMD.KBC_Is_Active},
                    {"KBC_Modify_Date", con.ConvertTimeZone(KCMD.KBC_TimeZone, Convert.ToDateTime(KCMD.KBC_Modify_Date))},
                    {"KBC_TimeZone", KCMD.KBC_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_KBCategory").Document(KCMD.KBC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = KCMD;
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

        [Route("API/KBCategory/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KBC_Is_Deleted", KCMD.KBC_Is_Deleted},
                    {"KBC_Modify_Date", con.ConvertTimeZone(KCMD.KBC_TimeZone, Convert.ToDateTime(KCMD.KBC_Modify_Date))},
                    {"KBC_TimeZone", KCMD.KBC_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_KBCategory").Document(KCMD.KBC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = KCMD;
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

        [Route("API/KBCategory/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                List<MT_KBCategory> List = new List<MT_KBCategory>();
                Query docRef = Db.Collection("MT_KBCategory").WhereEqualTo("KBC_Is_Deleted", false).OrderBy("KBC_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_KBCategory>());
                    }
                    Response.DataList = List;
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

        [Route("API/KBCategory/ListDD")]
        [HttpPost]
        public async Task<HttpResponseMessage> ListDD(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                List<MT_KBCategory> List = new List<MT_KBCategory>();
                Query docRef = Db.Collection("MT_KBCategory").WhereEqualTo("KBC_Is_Deleted", false).WhereEqualTo("KBC_Is_Active", true).WhereEqualTo("KBC_Sub_Category", null).OrderBy("KBC_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_KBCategory>());
                    }
                    Response.DataList = List;
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

        [Route("API/KBCategory/GetDeletedList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDeletedList(MT_KBCategory KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KBCategoryResponse Response = new KBCategoryResponse();
            try
            {
                List<MT_KBCategory> List = new List<MT_KBCategory>();
                Query docRef = Db.Collection("MT_KBCategory").WhereEqualTo("KBC_Is_Deleted", true).OrderBy("KBC_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_KBCategory>());
                    }
                    Response.DataList = List;
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
