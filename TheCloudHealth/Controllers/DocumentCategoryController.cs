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
    public class DocumentCategoryController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public DocumentCategoryController()
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

        [Route("API/DocumentCategory/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                List<string> List = new List<string>();
                UniqueID = con.GetUniqueKey();
                DCMD.DOC_Unique_ID = UniqueID;
                DCMD.DOC_Create_Date = con.ConvertTimeZone(DCMD.DOC_TimeZone, Convert.ToDateTime(DCMD.DOC_Create_Date));
                DCMD.DOC_Modify_Date = con.ConvertTimeZone(DCMD.DOC_TimeZone, Convert.ToDateTime(DCMD.DOC_Modify_Date));

                DocumentReference docRef = Db.Collection("MT_Document_Category").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(DCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DCMD;
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

        [Route("API/DocumentCategory/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                List<string> List = new List<string>();

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"DOC_Sub_Category", DCMD.DOC_Sub_Category},
                    {"DOC_Category", DCMD.DOC_Category},
                    {"DOC_Description", DCMD.DOC_Description},
                    {"DOC_Modify_Date", con.ConvertTimeZone(DCMD.DOC_TimeZone, Convert.ToDateTime(DCMD.DOC_Modify_Date))},
                    {"DOC_TimeZone", DCMD.DOC_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Document_Category").Document(DCMD.DOC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DCMD;
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


        [Route("API/DocumentCategory/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"DOC_Is_Active", DCMD.DOC_Is_Active},
                    {"DOC_Modify_Date", con.ConvertTimeZone(DCMD.DOC_TimeZone, Convert.ToDateTime(DCMD.DOC_Modify_Date))},
                    {"DOC_TimeZone", DCMD.DOC_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Document_Category").Document(DCMD.DOC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DCMD;
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

        [Route("API/DocumentCategory/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"DOC_Is_Deleted", DCMD.DOC_Is_Deleted},
                    {"DOC_Modify_Date", con.ConvertTimeZone(DCMD.DOC_TimeZone, Convert.ToDateTime(DCMD.DOC_Modify_Date))},
                    {"DOC_TimeZone", DCMD.DOC_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Document_Category").Document(DCMD.DOC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DCMD;
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

        [Route("API/DocumentCategory/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                List<MT_Document_Category> List = new List<MT_Document_Category>();
                Query docRef = Db.Collection("MT_Document_Category").WhereEqualTo("DOC_Is_Deleted", false).OrderBy("DOC_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Document_Category>());
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

        [Route("API/DocumentCategory/ListDD")]
        [HttpPost]
        public async Task<HttpResponseMessage> ListDD(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                List<MT_Document_Category> List = new List<MT_Document_Category>();
                Query docRef = Db.Collection("MT_Document_Category").WhereEqualTo("DOC_Is_Deleted", false).WhereEqualTo("DOC_Is_Active", true).WhereEqualTo("DOC_Sub_Category", null).OrderBy("DOC_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Document_Category>());
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

        [Route("API/DocumentCategory/GetDeletedList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Document_Category DCMD)
        {
            Db = con.SurgeryCenterDb(DCMD.Slug);
            DocuCategoryResponse Response = new DocuCategoryResponse();
            try
            {
                List<MT_Document_Category> List = new List<MT_Document_Category>();
                Query docRef = Db.Collection("MT_Document_Category").WhereEqualTo("DOC_Is_Deleted", true).OrderBy("DOC_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Document_Category>());
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
