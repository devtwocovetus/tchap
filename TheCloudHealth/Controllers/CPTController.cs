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
    public class CPTController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public CPTController()
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

        [Route("API/CPT/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                CMD.CPT_Unique_ID = UniqueID;
                CMD.CPT_Procedure_Code_Category = CMD.CPT_Procedure_Code_Category.ToUpper();
                CMD.CPT_Create_Date = con.ConvertTimeZone(CMD.CPT_TimeZone, Convert.ToDateTime(CMD.CPT_Create_Date));
                CMD.CPT_Modify_Date = con.ConvertTimeZone(CMD.CPT_TimeZone, Convert.ToDateTime(CMD.CPT_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_CPT").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(CMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = CMD;
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

        [Route("API/CPT/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "CPT_Code", CMD.CPT_Code },
                    { "CPT_Code_Description", CMD.CPT_Code_Description },
                    { "CPT_Status", CMD.CPT_Status },
                    { "CPT_Procedure_Code_Category", CMD.CPT_Procedure_Code_Category.ToUpper() },
                    { "CPT_Modify_Date",con.ConvertTimeZone(CMD.CPT_TimeZone, Convert.ToDateTime(CMD.CPT_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_CPT").Document(CMD.CPT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = CMD;
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

        [Route("API/CPT/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "CPT_Modify_Date",con.ConvertTimeZone(CMD.CPT_TimeZone, Convert.ToDateTime(CMD.CPT_Modify_Date))},
                    { "CPT_Is_Active",CMD.CPT_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_CPT").Document(CMD.CPT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = CMD;
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

        [Route("API/CPT/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "CPT_Modify_Date",con.ConvertTimeZone(CMD.CPT_TimeZone, Convert.ToDateTime(CMD.CPT_Modify_Date))},
                    { "CPT_Is_Deleted",CMD.CPT_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_CPT").Document(CMD.CPT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = CMD;
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

        [Route("API/CPT/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_CPT").Document(CMD.CPT_Unique_ID);
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

        [Route("API/CPT/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                MT_CPT cpt = new MT_CPT();
                Query docRef = Db.Collection("MT_CPT").WhereEqualTo("CPT_Unique_ID", CMD.CPT_Unique_ID).WhereEqualTo("CPT_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    cpt = ObjQuerySnap.Documents[0].ConvertTo<MT_CPT>();
                    Response.Data = cpt;
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

        [Route("API/CPT/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                List<MT_CPT> AnesList = new List<MT_CPT>();
                Query docRef = Db.Collection("MT_CPT").WhereEqualTo("CPT_Is_Deleted", false).OrderBy("CPT_Procedure_Code_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_CPT>());
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

        [Route("API/CPT/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                List<MT_CPT> AnesList = new List<MT_CPT>();
                Query docRef = Db.Collection("MT_CPT").WhereEqualTo("CPT_Is_Deleted", false).WhereEqualTo("CPT_Is_Active", true).OrderBy("CPT_Procedure_Code_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_CPT>());
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

        [Route("API/CPT/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                List<MT_CPT> AnesList = new List<MT_CPT>();
                Query docRef = Db.Collection("MT_CPT").WhereEqualTo("CPT_Is_Deleted", true).OrderBy("CPT_Procedure_Code_Category");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_CPT>());
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

        [Route("API/CPT/GetCPTListFilterWithCategory")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetCPTListFilterWithCategory(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                List<MT_CPT> AnesList = new List<MT_CPT>();
                Query docRef = Db.Collection("MT_CPT").WhereEqualTo("CPT_Is_Deleted", false).WhereEqualTo("CPT_Procedure_Code_Category", CMD.CPT_Procedure_Code_Category.ToUpper());
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_CPT>());
                    }
                    AnesList.OrderBy(o => o.CPT_Procedure_Code_Category).ToList();
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

        [Route("API/CPT/GetCPTListFilterWithCode")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetCPTListFilterWithCode(MT_CPT CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CPTResponse Response = new CPTResponse();
            try
            {
                List<MT_CPT> AnesList = new List<MT_CPT>();
                Query docRef = Db.Collection("MT_CPT").WhereEqualTo("CPT_Is_Deleted", false).OrderBy("CPT_Code").StartAt(CMD.CPT_Code).EndAt(CMD.CPT_Code + '\uf8ff');
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_CPT>());
                    }
                    AnesList.OrderBy(o => o.CPT_Procedure_Code_Category).ToList();
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
