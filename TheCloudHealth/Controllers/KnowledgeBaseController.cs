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
                    {"KNB_Name", KCMD.KNB_Name},
                    {"KNB_Category", KCMD.KNB_Category},
                    {"KNB_Sub_Category", KCMD.KNB_Sub_Category},
                    {"KNB_Short_Description", KCMD.KNB_Short_Description},
                    {"KNB_Long_Description", KCMD.KNB_Long_Description},
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone,Convert.ToDateTime(KCMD.KNB_Modify_Date))},
                    //{"KNB_Document", KCMD.KNB_Document},
                    //{"KNB_References", KCMD.KNB_References},
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

        [Route("API/KnowledgeBase/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                Query Qty = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).WhereEqualTo("KNB_Unique_ID", KCMD.KNB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await Qty.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_Knowledger_Base>();
                    Response.Message = con.MessageSuccess;
                    Response.Status = con.StatusSuccess;
                }
                else
                {
                    Response.Message = con.MessageDNE;
                    Response.Status = con.StatusDNE;
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

        [Route("API/KnowledgeBase/AddReference")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddReference(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                MT_Knowledger_Base KBase = new MT_Knowledger_Base();
                List<KBReference> RefList = new List<KBReference>();
                Query Qty = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).WhereEqualTo("KNB_Unique_ID", KCMD.KNB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await Qty.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    KBase = ObjQuerySnap.Documents[0].ConvertTo<MT_Knowledger_Base>();
                    if (KBase.KNB_References != null)
                    {
                        foreach (KBReference refe in KBase.KNB_References)
                        {
                            RefList.Add(refe);
                        }
                    }
                }

                if (KCMD.KNB_References != null)
                {
                    foreach (KBReference refe in KCMD.KNB_References)
                    {
                        refe.KBR_Unique_ID = con.GetUniqueKey();
                        refe.KBR_Create_Date = con.ConvertTimeZone(refe.KBR_TimeZone, Convert.ToDateTime(refe.KBR_Create_Date));
                        refe.KBR_Modify_Date = con.ConvertTimeZone(refe.KBR_TimeZone, Convert.ToDateTime(refe.KBR_Modify_Date));
                        RefList.Add(refe);
                    }
                }

                KCMD.KNB_References = RefList;

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
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

        public MT_Knowledger_Base GetImages()
        {
            MT_Knowledger_Base Surcenter = new MT_Knowledger_Base();
            try
            {
                List<KBAttachment> UploadedAttachList = new List<KBAttachment>();

                string[] DBPath;
                int i = 0;
                var httpRequest = HttpContext.Current.Request;
                var Descriplist = httpRequest.Form[0];
                Dictionary<string, string> Description = JsonConvert.DeserializeObject<Dictionary<string, string>>(Descriplist);

                var postedData = httpRequest.Form[1];
                string str = postedData.Substring(1, postedData.Length - 2);
                JObject jobject = JObject.Parse(str);
                Surcenter = JsonConvert.DeserializeObject<MT_Knowledger_Base>(jobject.ToString());

                

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        int hasheddate = DateTime.Now.GetHashCode();

                        //var myKey = Description.FirstOrDefault(x => x.Value == postedFile.FileName.ToString()).Key;

                        string changed_name = i.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + postedFile.FileName.Substring(postedFile.FileName.IndexOf('.'), (postedFile.FileName.Length - postedFile.FileName.IndexOf('.')));

                        var filePath = HttpContext.Current.Server.MapPath("~/KBAttachment/" + changed_name);

                        postedFile.SaveAs(filePath); // save the file to a folder "Images" in the root of your app

                        //changed_name = @"~\Images\" + changed_name; //store this complete path to database
                        KBAttachment DocUpld = new KBAttachment();
                        DocUpld.KBA_Unique_ID = con.GetUniqueKey();
                        DocUpld.KBA_Description = Description[i.ToString()];
                        DocUpld.KBA_Attachement = @"~\KBAttachment\" + changed_name;
                        DocUpld.KBA_Attachement_Name = changed_name;
                        DocUpld.KBA_Created_By = Surcenter.KNB_Created_By;
                        DocUpld.KBA_User_Name = Surcenter.KNB_User_Name;
                        DocUpld.KBA_Create_Date = con.ConvertTimeZone(Surcenter.KNB_TimeZone, Convert.ToDateTime(Surcenter.KNB_Modify_Date));
                        DocUpld.KBA_Modify_Date = con.ConvertTimeZone(Surcenter.KNB_TimeZone, Convert.ToDateTime(Surcenter.KNB_Modify_Date));
                        DocUpld.KBA_Is_Active = true;
                        DocUpld.KBA_Is_Deleted = false;
                        DocUpld.KBA_TimeZone = Surcenter.KNB_TimeZone;

                        UploadedAttachList.Add(DocUpld);
                        i++;
                    }
                    Surcenter.KNB_Document = UploadedAttachList;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Surcenter;
        }

        [Route("API/KnowledgeBase/AddAttachment")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddAttachment()
        {
            MT_Knowledger_Base KCMD = GetImages();
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                MT_Knowledger_Base KBase = new MT_Knowledger_Base();
                List<KBAttachment> AttcList = new List<KBAttachment>();
                Query Qty = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).WhereEqualTo("KNB_Unique_ID", KCMD.KNB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await Qty.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    KBase = ObjQuerySnap.Documents[0].ConvertTo<MT_Knowledger_Base>();
                    if (KBase.KNB_Document != null)
                    {
                        foreach (KBAttachment kbatt in KBase.KNB_Document)
                        {
                            AttcList.Add(kbatt);
                        }
                    }
                }

                if (KCMD.KNB_Document != null)
                {
                    foreach (KBAttachment kbatt in KCMD.KNB_Document)
                    {
                        kbatt.KBA_Unique_ID = con.GetUniqueKey();
                        kbatt.KBA_Create_Date = con.ConvertTimeZone(kbatt.KBA_TimeZone, Convert.ToDateTime(kbatt.KBA_Create_Date));
                        kbatt.KBA_Modify_Date = con.ConvertTimeZone(kbatt.KBA_TimeZone, Convert.ToDateTime(kbatt.KBA_Modify_Date));
                        AttcList.Add(kbatt);
                    }
                }                
                KCMD.KNB_Document = AttcList;

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
                    {"KNB_Document", KCMD.KNB_Document},
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

        [Route("API/KnowledgeBase/IsDeletedAttachment")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeletedAttachment(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                MT_Knowledger_Base KBase = new MT_Knowledger_Base();
                List<KBAttachment> AttcList = new List<KBAttachment>();
                Query Qty = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).WhereEqualTo("KNB_Unique_ID", KCMD.KNB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await Qty.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    KBase = ObjQuerySnap.Documents[0].ConvertTo<MT_Knowledger_Base>();
                    if (KBase.KNB_Document != null)
                    {
                        foreach (KBAttachment kbatt in KBase.KNB_Document)
                        {
                            if (kbatt.KBA_Unique_ID != KCMD.KNB_Document[0].KBA_Unique_ID)
                            {
                                AttcList.Add(kbatt);
                            }                            
                        }
                    }
                }
                KCMD.KNB_Document = AttcList;

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
                    {"KNB_Document", KCMD.KNB_Document},
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

        [Route("API/KnowledgeBase/IsDeletedReference")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeletedReference(MT_Knowledger_Base KCMD)
        {
            Db = con.SurgeryCenterDb(KCMD.Slug);
            KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
            try
            {
                MT_Knowledger_Base KBase = new MT_Knowledger_Base();
                List<KBReference> RefList = new List<KBReference>();
                Query Qty = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).WhereEqualTo("KNB_Unique_ID", KCMD.KNB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await Qty.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    KBase = ObjQuerySnap.Documents[0].ConvertTo<MT_Knowledger_Base>();
                    if (KBase.KNB_References != null)
                    {
                        foreach (KBReference refe in KBase.KNB_References)
                        {
                            if (refe.KBR_Unique_ID != KCMD.KNB_References[0].KBR_Unique_ID)
                            {
                                RefList.Add(refe);
                            }
                        }
                    }
                }
                
                KCMD.KNB_Modify_Date = con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date));
                KCMD.KNB_References = RefList;

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"KNB_Modify_Date", con.ConvertTimeZone(KCMD.KNB_TimeZone, Convert.ToDateTime(KCMD.KNB_Modify_Date))},
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

        //[Route("API/KnowledgeBase/GetKBFilterWithCategory")]
        //[HttpPost]
        //public async Task<HttpResponseMessage> GetKBFilterWithCategory(MT_Knowledger_Base KCMD)
        //{
        //    Db = con.SurgeryCenterDb(KCMD.Slug);
        //    KnowledgeBaseResponse Response = new KnowledgeBaseResponse();
        //    try
        //    {
        //        List<MT_Knowledger_Base> List = new List<MT_Knowledger_Base>();
        //        Query docRef = Db.Collection("MT_Knowledger_Base").WhereEqualTo("KNB_Is_Deleted", false).WhereEqualTo("KNB_Is_Active", true).OrderBy("KNB_Category").StartAt(KCMD.KNB_Category.ToUpper()).EndAt(KCMD.KNB_Category.ToUpper() +'\uf8ff');
        //        QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
        //        if (ObjQuerySnap != null)
        //        {
        //            foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
        //            {
        //                List.Add(Docsnapshot.ConvertTo<MT_Knowledger_Base>());
        //            }
        //            Response.DataList = List;
        //        }
        //        Response.Status = con.StatusSuccess;
        //        Response.Message = con.MessageSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Status = con.StatusFailed;
        //        Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
        //    }
        //    return ConvertToJSON(Response);
        //}
    }
}
