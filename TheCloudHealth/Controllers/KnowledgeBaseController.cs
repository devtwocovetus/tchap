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
    public class KnowledgeBaseController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public KnowledgeBaseController()
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

        [Route("API/KnowledgeBase/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                List<string> List = new List<string>();
                UniqueID = con.GetUniqueKey();
                KCMD.KNB_Unique_ID = UniqueID;
                KCMD.KNB_Create_Date = con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Create_Date));
                KCMD.KNB_Modify_Date = con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date));
                KCMD.KNB_Category = KCMD.KNB_Category.ToUpper();

                DocumentReference docRef = Db.Collection("MT_Knowledger_Base").Document(UniqueID);
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

        [Route("API/KnowledgeBase/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Category", KCMD.KNB_Category},
                    {"KNB_Sub_Category", KCMD.KNB_Sub_Category},
                    {"KNB_Short_Description", KCMD.KNB_Short_Description},
                    {"KNB_Long_Description", KCMD.KNB_Long_Description},
                    {"KNB_Document", KCMD.KNB_Document},
                    {"KNB_References", KCMD.KNB_References},
                 };

                DocumentReference docRef = Db.Collection("MT_Knowledger_Base").Document(KCMD.KNB_Unique_ID);
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


        [Route("API/KnowledgeBase/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Is_Active", KCMD.KNB_Is_Active},
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
                    {"KNB_TimeZone", KCMD.KNB_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Knowledger_Base").Document(KCMD.KNB_Unique_ID);
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

        [Route("API/KnowledgeBase/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Is_Deleted", KCMD.KNB_Is_Deleted},
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
                    {"KNB_TimeZone", KCMD.KNB_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Knowledger_Base").Document(KCMD.KNB_Unique_ID);
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

        [Route("API/KnowledgeBase/AddSuggestion")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSuggestion(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            Boolean IsExist = false;
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                List<Suggestion> suggestion = new List<Suggestion>();
                Query qry = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).WhereEqualTo("KNB_Unique_ID", KCMD.KNB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await qry.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Knowledger_Base KB = ObjQuerySnap.Documents[0].ConvertTo<MT_Knowledger_Base>();
                    if (KB.KNB_Suggestions != null)
                    {
                        foreach (Suggestion sug in KB.KNB_Suggestions)
                        {
                            if (sug.SG_User_ID == KCMD.KNB_Suggestions[0].SG_User_ID)
                            {
                                sug.SG_Is_Useful = KCMD.KNB_Suggestions[0].SG_Is_Useful;
                                sug.SG_User_ID = KCMD.KNB_Suggestions[0].SG_User_ID;
                                suggestion.Add(sug);
                                IsExist = true;
                            }
                            else
                            {
                                suggestion.Add(sug);
                            }
                            
                        }
                    }
                }
                if (KCMD.KNB_Suggestions != null && IsExist==false)
                {
                    foreach (Suggestion sug in KCMD.KNB_Suggestions)
                    {
                        suggestion.Add(sug);
                    }
                }
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Suggestions", suggestion },
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
                    {"KNB_TimeZone", KCMD.KNB_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Knowledger_Base").Document(KCMD.KNB_Unique_ID);
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

        [Route("API/KnowledgeBase/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                List<MT_Knowledger_Base> List = new List<MT_Knowledger_Base>();
                Query docRef = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).OrderBy("KNB_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Knowledger_Base>());
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

        [Route("API/KnowledgeBase/ListDD")]
        [HttpPost]
        public async Task<HttpResponseMessage> ListDD(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                List<MT_Knowledger_Base> List = new List<MT_Knowledger_Base>();
                Query docRef = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).OrderBy("KNB_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Knowledger_Base>());
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

        [Route("API/KnowledgeBase/GetDeletedList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                List<MT_Knowledger_Base> List = new List<MT_Knowledger_Base>();
                Query docRef = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", true).OrderBy("KNB_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Knowledger_Base>());
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

        [Route("API/KnowledgeBase/GetKBFilterWithCategory")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetKBFilterWithCategory(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                List<MT_Knowledger_Base> List = new List<MT_Knowledger_Base>();
                Query docRef = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).OrderBy("KNB_Category").StartAt(KCMD.KNB_Category.ToUpper()).EndAt(KCMD.KNB_Category.ToUpper() +'\uf8ff');
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        List.Add(Docsnapshot.ConvertTo<MT_Knowledger_Base>());
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
