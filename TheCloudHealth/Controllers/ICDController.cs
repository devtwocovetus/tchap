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
    public class ICDController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public ICDController()
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

        [Route("API/ICD/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                IMD.ICD_Unique_ID = UniqueID;
                IMD.ICD_Procedure_Code_Category = IMD.ICD_Procedure_Code_Category.ToUpper();
                IMD.ICD_Create_Date = con.ConvertTimeZone(IMD.ICD_TimeZone, Convert.ToDateTime(IMD.ICD_Create_Date));
                IMD.ICD_Modify_Date = con.ConvertTimeZone(IMD.ICD_TimeZone, Convert.ToDateTime(IMD.ICD_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_ICD").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(IMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/ICD/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "ICD_ICD10_PCS_Code", IMD.ICD_ICD10_PCS_Code },
                    { "ICD_Code_Description", IMD.ICD_Code_Description },
                    { "ICD_Procedure_Code_Category", IMD.ICD_Procedure_Code_Category.ToUpper() },
                    { "ICD_Status", IMD.ICD_Status },
                    { "ICD_Modify_Date",con.ConvertTimeZone(IMD.ICD_TimeZone, Convert.ToDateTime(IMD.ICD_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_ICD").Document(IMD.ICD_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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


        [Route("API/ICD/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "ICD_Modify_Date",con.ConvertTimeZone(IMD.ICD_TimeZone, Convert.ToDateTime(IMD.ICD_Modify_Date))},
                    { "ICD_Is_Active",IMD.ICD_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_ICD").Document(IMD.ICD_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/ICD/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "ICD_Modify_Date",con.ConvertTimeZone(IMD.ICD_TimeZone, Convert.ToDateTime(IMD.ICD_Modify_Date))},
                    { "ICD_Is_Deleted",IMD.ICD_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_ICD").Document(IMD.ICD_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = IMD;
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

        [Route("API/ICD/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_ICD").Document(IMD.ICD_Unique_ID);
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

        [Route("API/ICD/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                MT_ICD ICD = new MT_ICD();
                Query docRef = Db.Collection("MT_ICD").WhereEqualTo("ICD_Unique_ID", IMD.ICD_Unique_ID).WhereEqualTo("ICD_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    ICD = ObjQuerySnap.Documents[0].ConvertTo<MT_ICD>();
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

        [Route("API/ICD/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                List<MT_ICD> AnesList = new List<MT_ICD>();
                Query docRef = Db.Collection("MT_ICD").WhereEqualTo("ICD_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_ICD>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ICD_Procedure_Code_Category).ThenBy(o => o.ICD_ICD10_PCS_Code).ToList();
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

        [Route("API/ICD/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                List<MT_ICD> AnesList = new List<MT_ICD>();
                Query docRef = Db.Collection("MT_ICD").WhereEqualTo("ICD_Is_Deleted", false).WhereEqualTo("ICD_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_ICD>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ICD_Procedure_Code_Category).ThenBy(o => o.ICD_ICD10_PCS_Code).ToList();
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

        [Route("API/ICD/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                List<MT_ICD> AnesList = new List<MT_ICD>();
                Query docRef = Db.Collection("MT_ICD").WhereEqualTo("ICD_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_ICD>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ICD_Procedure_Code_Category).ThenBy(o => o.ICD_ICD10_PCS_Code).ToList();
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

        [Route("API/ICD/GetICDFilterWithCategory")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetICDFilterWithCategory(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                List<MT_ICD> AnesList = new List<MT_ICD>();
                Query docRef = Db.Collection("MT_ICD").WhereEqualTo("ICD_Is_Deleted", false).WhereEqualTo("ICD_Procedure_Code_Category", IMD.ICD_Procedure_Code_Category.ToUpper());
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_ICD>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ICD_Procedure_Code_Category).ThenBy(o => o.ICD_ICD10_PCS_Code).ToList();
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


        [Route("API/ICD/GetICDFilterWithCode")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetICDFilterWithCode(MT_ICD IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            ICDResponse Response = new ICDResponse();
            try
            {
                List<MT_ICD> AnesList = new List<MT_ICD>();
                Query docRef = Db.Collection("MT_ICD").WhereEqualTo("ICD_Is_Deleted", false).OrderBy("ICD_ICD10_PCS_Code").StartAt(IMD.ICD_ICD10_PCS_Code).EndAt(IMD.ICD_ICD10_PCS_Code + '\uf8ff');//.WhereEqualTo("ICD_ICD10_PCS_Code", ICDCode);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_ICD>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ICD_Procedure_Code_Category).ThenBy(o => o.ICD_ICD10_PCS_Code).ToList();
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
