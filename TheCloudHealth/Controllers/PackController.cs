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
    public class PackController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public PackController()
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

        [Route("API/Pack/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                Pck.Pack_Unique_ID = UniqueID;
                Pck.Pack_Create_Date = con.ConvertTimeZone(Pck.Pack_TimeZone, Convert.ToDateTime(Pck.Pack_Create_Date));
                Pck.Pack_Modify_Date = con.ConvertTimeZone(Pck.Pack_TimeZone, Convert.ToDateTime(Pck.Pack_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Pack").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(Pck);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = Pck;
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


        [Route("API/Pack/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Pack_Name" , Pck.Pack_Name},
                    {"Pack_Modify_Date" , con.ConvertTimeZone(Pck.Pack_TimeZone,Convert.ToDateTime(Pck.Pack_Modify_Date))},
                    {"Pack_Is_Active" , Pck.Pack_Is_Active},
                    {"Pack_Is_Deleted" , Pck.Pack_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Pack").Document(Pck.Pack_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = Pck;
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

        [Route("API/Pack/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Pack_Modify_Date" , con.ConvertTimeZone(Pck.Pack_TimeZone,Convert.ToDateTime(Pck.Pack_Modify_Date))},
                    {"Pack_Is_Active" , Pck.Pack_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Pack").Document(Pck.Pack_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = Pck;
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

        [Route("API/Pack/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Pack_Modify_Date" , con.ConvertTimeZone(Pck.Pack_TimeZone,Convert.ToDateTime(Pck.Pack_Modify_Date))},
                    {"Pack_Is_Deleted" , Pck.Pack_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Pack").Document(Pck.Pack_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = Pck;
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


        [Route("API/Pack/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Pack").Document(Pck.Pack_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = Pck;
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


        [Route("API/Pack/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                List<MT_Pack> AnesList = new List<MT_Pack>();
                Query docRef = Db.Collection("MT_Pack").WhereEqualTo("Pack_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Pack>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pack_Name).ToList();
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

        [Route("API/Pack/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                List<MT_Pack> AnesList = new List<MT_Pack>();
                Query docRef = Db.Collection("MT_Pack").WhereEqualTo("Pack_Is_Deleted", false).WhereEqualTo("Pack_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Pack>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pack_Name).ToList();
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

        [Route("API/Pack/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                List<MT_Pack> AnesList = new List<MT_Pack>();
                Query docRef = Db.Collection("MT_Pack").WhereEqualTo("Pack_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Pack>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Pack_Name).ToList();
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

        [Route("API/Pack/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_Pack Pck)
        {
            Db = con.SurgeryCenterDb(Pck.Slug);
            PackResponse Response = new PackResponse();
            try
            {
                MT_Pack AnesList = new MT_Pack();
                Query docRef = Db.Collection("MT_Pack").WhereEqualTo("Pack_Is_Deleted", false).WhereEqualTo("Pack_Unique_ID", Pck.Pack_Unique_ID);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    AnesList = ObjQuerySnap.Documents[0].ConvertTo<MT_Pack>();
                    Response.Data = AnesList;
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
