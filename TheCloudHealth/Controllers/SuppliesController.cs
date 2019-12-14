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
    public class SuppliesController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public SuppliesController()
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

        [Route("API/Supplies/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SMD.Supp_Unique_ID = UniqueID;
                SMD.Supp_Create_Date = con.ConvertTimeZone(SMD.Supp_TimeZone, Convert.ToDateTime(SMD.Supp_Create_Date));
                SMD.Supp_Modify_Date = con.ConvertTimeZone(SMD.Supp_TimeZone, Convert.ToDateTime(SMD.Supp_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Supplies").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/Supplies/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Supp_Type", SMD.Supp_Type },
                    { "Supp_Name", SMD.Supp_Name },
                    { "Supp_Description", SMD.Supp_Description },
                    { "Supp_Modify_Date",con.ConvertTimeZone(SMD.Supp_TimeZone, Convert.ToDateTime(SMD.Supp_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Supplies").Document(SMD.Supp_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/Supplies/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Supp_Is_Active", SMD.Supp_Is_Active },
                    { "Supp_Modify_Date",con.ConvertTimeZone(SMD.Supp_TimeZone, Convert.ToDateTime(SMD.Supp_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Supplies").Document(SMD.Supp_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/Supplies/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Supp_Is_Deleted", SMD.Supp_Is_Deleted },
                    { "Supp_Modify_Date",con.ConvertTimeZone(SMD.Supp_TimeZone, Convert.ToDateTime(SMD.Supp_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Supplies").Document(SMD.Supp_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/Supplies/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Supplies").Document(SMD.Supp_Unique_ID);
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
        [Route("API/Supplies/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                MT_Supplies Supp = new MT_Supplies();
                Query docRef = Db.Collection("MT_Supplies").WhereEqualTo("Supp_Unique_ID", SMD.Supp_Unique_ID).WhereEqualTo("Supp_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Supp = ObjQuerySnap.Documents[0].ConvertTo<MT_Supplies>();
                    Response.Data = Supp;
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

        [Route("API/Supplies/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                List<MT_Supplies> AnesList = new List<MT_Supplies>();
                Query docRef = Db.Collection("MT_Supplies").WhereEqualTo("Supp_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Supplies>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Supp_Name).ToList();
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

        [Route("API/Supplies/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                List<MT_Supplies> AnesList = new List<MT_Supplies>();
                Query docRef = Db.Collection("MT_Supplies").WhereEqualTo("Supp_Is_Deleted", false).WhereEqualTo("Supp_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Supplies>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Supp_Name).ToList();
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

        [Route("API/Supplies/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Supplies SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            SuppliesResponse Response = new SuppliesResponse();
            try
            {
                List<MT_Supplies> AnesList = new List<MT_Supplies>();
                Query docRef = Db.Collection("MT_Supplies").WhereEqualTo("Supp_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Supplies>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Supp_Name).ToList();
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
