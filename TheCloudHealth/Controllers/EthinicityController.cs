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
    public class EthinicityController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public EthinicityController()
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

        [Route("API/Ethinicity/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                EMD.Ethi_Unique_ID = UniqueID;
                EMD.Ethi_Create_Date = con.ConvertTimeZone(EMD.Ethi_TimeZone, Convert.ToDateTime(EMD.Ethi_Create_Date));
                EMD.Ethi_Modify_Date = con.ConvertTimeZone(EMD.Ethi_TimeZone, Convert.ToDateTime(EMD.Ethi_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Ethinicity").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(EMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
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

        [Route("API/Ethinicity/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Ethi_Name", EMD.Ethi_Name },
                    { "Ethi_Type", EMD.Ethi_Type },
                    { "Ethi_Modify_Date",con.ConvertTimeZone(EMD.Ethi_TimeZone, Convert.ToDateTime(EMD.Ethi_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Ethinicity").Document(EMD.Ethi_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
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

        [Route("API/Ethinicity/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Ethi_Modify_Date",con.ConvertTimeZone(EMD.Ethi_TimeZone, Convert.ToDateTime(EMD.Ethi_Modify_Date))},
                    { "Ethi_Is_Active",EMD.Ethi_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Ethinicity").Document(EMD.Ethi_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
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

        [Route("API/Ethinicity/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Ethi_Modify_Date",con.ConvertTimeZone(EMD.Ethi_TimeZone, Convert.ToDateTime(EMD.Ethi_Modify_Date))},
                    { "Ethi_Is_Deleted",EMD.Ethi_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Ethinicity").Document(EMD.Ethi_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
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

        [Route("API/Ethinicity/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Ethinicity").Document(EMD.Ethi_Unique_ID);
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

        [Route("API/Ethinicity/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                MT_Ethinicity Ethi = new MT_Ethinicity();
                Query docRef = Db.Collection("MT_Ethinicity").WhereEqualTo("Ethi_Unique_ID", EMD.Ethi_Unique_ID).WhereEqualTo("Ethi_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Ethi = ObjQuerySnap.Documents[0].ConvertTo<MT_Ethinicity>();
                    Response.Data = Ethi;
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

        [Route("API/Ethinicity/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                List<MT_Ethinicity> AnesList = new List<MT_Ethinicity>();
                Query docRef = Db.Collection("MT_Ethinicity").WhereEqualTo("Ethi_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Ethinicity>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Ethi_Name).ToList();
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

        [Route("API/Ethinicity/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                List<MT_Ethinicity> AnesList = new List<MT_Ethinicity>();
                Query docRef = Db.Collection("MT_Ethinicity").WhereEqualTo("Ethi_Is_Deleted", false).WhereEqualTo("Ethi_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Ethinicity>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Ethi_Name).ToList();
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

        [Route("API/Ethinicity/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Ethinicity EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EthinicityResponse Response = new EthinicityResponse();
            try
            {
                List<MT_Ethinicity> AnesList = new List<MT_Ethinicity>();
                Query docRef = Db.Collection("MT_Ethinicity").WhereEqualTo("Ethi_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Ethinicity>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Ethi_Name).ToList();
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
