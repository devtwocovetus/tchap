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
    public class SpecilityController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public SpecilityController()
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


        [Route("API/Specility/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SCMD.Spec_Unique_ID = UniqueID;
                SCMD.Spec_Create_Date = con.ConvertTimeZone(SCMD.Spec_TimeZone, Convert.ToDateTime(SCMD.Spec_Create_Date));
                SCMD.Spec_Modify_Date = con.ConvertTimeZone(SCMD.Spec_TimeZone, Convert.ToDateTime(SCMD.Spec_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Specilities").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/Specility/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Spec_Name", SCMD.Spec_Name },
                    { "Spec_Type", SCMD.Spec_Type },
                    { "Spec_Description", SCMD.Spec_Description },
                    { "Spec_User_Name", SCMD.Spec_User_Name },
                    { "Spec_Is_Active", SCMD.Spec_Is_Active },
                    { "Spec_Is_Deleted", SCMD.Spec_Is_Deleted },
                    { "Spec_Modify_Date",con.ConvertTimeZone(SCMD.Spec_TimeZone, Convert.ToDateTime(SCMD.Spec_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Specilities").Document(SCMD.Spec_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/Specility/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Spec_Is_Active", SCMD.Spec_Is_Active },
                    { "Spec_Modify_Date",con.ConvertTimeZone(SCMD.Spec_TimeZone, Convert.ToDateTime(SCMD.Spec_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Specilities").Document(SCMD.Spec_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/Specility/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Spec_Is_Deleted", SCMD.Spec_Is_Deleted },
                    { "Spec_Modify_Date",con.ConvertTimeZone(SCMD.Spec_TimeZone, Convert.ToDateTime(SCMD.Spec_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Specilities").Document(SCMD.Spec_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/Specility/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Specilities").Document(SCMD.Spec_Unique_ID);
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
        [Route("API/Specility/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                MT_Specilities Spec = new MT_Specilities();
                Query docRef = Db.Collection("MT_Specilities").WhereEqualTo("Spec_Unique_ID", SCMD.Spec_Unique_ID).WhereEqualTo("Spec_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Spec = ObjQuerySnap.Documents[0].ConvertTo<MT_Specilities>();
                    Response.Data = Spec;
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

        [Route("API/Specility/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                List<MT_Specilities> AnesList = new List<MT_Specilities>();
                Query docRef = Db.Collection("MT_Specilities").WhereEqualTo("Spec_Is_Deleted", false).OrderBy("Spec_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Specilities>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Spec_Name).ToList();
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

        [Route("API/Specility/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                List<MT_Specilities> AnesList = new List<MT_Specilities>();
                Query docRef = Db.Collection("MT_Specilities").WhereEqualTo("Spec_Is_Deleted", false).WhereEqualTo("Spec_Is_Active", true).OrderBy("Spec_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Specilities>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Spec_Name).ToList();
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

        [Route("API/Specility/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Specilities SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SpecilityResponse Response = new SpecilityResponse();
            try
            {
                List<MT_Specilities> AnesList = new List<MT_Specilities>();
                Query docRef = Db.Collection("MT_Specilities").WhereEqualTo("Spec_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Specilities>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Spec_Name).ToList();
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
