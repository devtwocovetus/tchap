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
    public class ProcedureController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public ProcedureController()
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

        [Route("API/Procedure/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PMD.Pro_Unique_ID = UniqueID;
                PMD.Pro_Procedure_Code_Category = PMD.Pro_Procedure_Code_Category.ToUpper();
                PMD.Pro_Create_Date = con.ConvertTimeZone(PMD.Pro_TimeZone, Convert.ToDateTime(PMD.Pro_Create_Date));
                PMD.Pro_Modify_Date = con.ConvertTimeZone(PMD.Pro_TimeZone, Convert.ToDateTime(PMD.Pro_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Procedures").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PMD;
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

        [Route("API/Procedure/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Pro_Procedure_Code_Category",PMD.Pro_Procedure_Code_Category.ToUpper()},
                    {"Pro_Name",PMD.Pro_Name},
                    {"Pro_Description",PMD.Pro_Description},
                    {"Pro_Status",PMD.Pro_Status},
                    {"Pro_Modify_Date",con.ConvertTimeZone(PMD.Pro_TimeZone,Convert.ToDateTime(PMD.Pro_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Procedures").Document(PMD.Pro_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PMD;
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

        [Route("API/Procedure/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Pro_Modify_Date",con.ConvertTimeZone(PMD.Pro_TimeZone,Convert.ToDateTime(PMD.Pro_Modify_Date))},
                    {"Pro_Is_Active",PMD.Pro_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Procedures").Document(PMD.Pro_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PMD;
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

        [Route("API/Procedure/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Pro_Modify_Date",con.ConvertTimeZone(PMD.Pro_TimeZone,Convert.ToDateTime(PMD.Pro_Modify_Date))},
                    {"Pro_Is_Deleted",PMD.Pro_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Procedures").Document(PMD.Pro_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PMD;
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

        [Route("API/Procedure/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Procedures").Document(PMD.Pro_Unique_ID);
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

        [Route("API/Procedure/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                MT_Procedures ICD = new MT_Procedures();
                Query docRef = Db.Collection("MT_Procedures").WhereEqualTo("Pro_Unique_ID", PMD.Pro_Unique_ID).WhereEqualTo("Pro_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    ICD = ObjQuerySnap.Documents[0].ConvertTo<MT_Procedures>();
                    Response.Data = ICD;
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

        [Route("API/Procedure/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                List<MT_Procedures> AnesList = new List<MT_Procedures>();
                Query docRef = Db.Collection("MT_Procedures").WhereEqualTo("Pro_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Procedures>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pro_Procedure_Code_Category).ThenBy(o => o.Pro_Name).ToList();
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


        [Route("API/Procedure/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                List<MT_Procedures> AnesList = new List<MT_Procedures>();
                Query docRef = Db.Collection("MT_Procedures").WhereEqualTo("Pro_Is_Deleted", false).WhereEqualTo("Pro_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Procedures>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pro_Procedure_Code_Category).ThenBy(o => o.Pro_Name).ToList();
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

        [Route("API/Procedure/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                List<MT_Procedures> AnesList = new List<MT_Procedures>();
                Query docRef = Db.Collection("MT_Procedures").WhereEqualTo("Pro_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Procedures>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pro_Procedure_Code_Category).ThenBy(o => o.Pro_Name).ToList();
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

        [Route("API/Procedure/GetProcedureFilterWithCategory")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetProcedureFilterWithCategory(MT_Procedures PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            ProcedureResponse Response = new ProcedureResponse();
            try
            {
                List<MT_Procedures> AnesList = new List<MT_Procedures>();
                Query docRef = Db.Collection("MT_Procedures").WhereEqualTo("Pro_Is_Deleted", false).WhereEqualTo("Pro_Is_Active", true).WhereEqualTo("Pro_Procedure_Code_Category", PMD.Pro_Procedure_Code_Category.ToUpper());
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Procedures>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pro_Procedure_Code_Category).ThenBy(o => o.Pro_Name).ToList();
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
