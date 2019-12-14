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

namespace TheCloudHealth.Controllers
{
    public class BlockController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public BlockController()
        {
            con = new ConnectionClass();
            Db = con.Db();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Block/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                BMD.Block_Unique_ID = UniqueID;
                BMD.Block_Create_Date = con.ConvertTimeZone(BMD.Block_TimeZone, Convert.ToDateTime(BMD.Block_Create_Date));
                BMD.Block_Modify_Date = con.ConvertTimeZone(BMD.Block_TimeZone, Convert.ToDateTime(BMD.Block_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Block").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(BMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = BMD;
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

        [Route("API/Block/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Block_Type", BMD.Block_Type },
                    { "Block_Name", BMD.Block_Name },
                    { "Block_Modify_Date",con.ConvertTimeZone(BMD.Block_TimeZone, Convert.ToDateTime(BMD.Block_Modify_Date))},
                    { "Block_Is_Active",BMD.Block_Is_Active},
                    { "Block_Is_Deleted",BMD.Block_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Block").Document(BMD.Block_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = BMD;
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

        [Route("API/Block/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Block_Modify_Date",con.ConvertTimeZone(BMD.Block_TimeZone, Convert.ToDateTime(BMD.Block_Modify_Date))},
                    { "Block_Is_Active",BMD.Block_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Block").Document(BMD.Block_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = BMD;
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

        [Route("API/Block/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Block_Modify_Date",con.ConvertTimeZone(BMD.Block_TimeZone, Convert.ToDateTime(BMD.Block_Modify_Date))},
                    { "Block_Is_Deleted",BMD.Block_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Block").Document(BMD.Block_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = BMD;
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

        [Route("API/Block/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Block").Document(BMD.Block_Unique_ID);
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

        [Route("API/Block/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                List<MT_Block> AnesList = new List<MT_Block>();
                Query docRef = Db.Collection("MT_Block").WhereEqualTo("Block_Is_Deleted", false).OrderBy("Block_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Block>());
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

        [Route("API/Block/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                List<MT_Block> AnesList = new List<MT_Block>();
                Query docRef = Db.Collection("MT_Block").WhereEqualTo("Block_Is_Deleted", false).WhereEqualTo("Block_Is_Active", true).OrderBy("Block_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Block>());
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

        [Route("API/Block/ListFilterWithAnesthesia")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListFilterWithAnesthesia(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                List<MT_Block> AnesList = new List<MT_Block>();
                Query docRef = Db.Collection("MT_Block").WhereEqualTo("Block_Anesthesia_Id", BMD.Block_Anesthesia_Id).WhereEqualTo("Block_Is_Deleted", false).OrderBy("Block_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Block>());
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

        [Route("API/Block/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Block BMD)
        {
            Db = con.SurgeryCenterDb(BMD.Slug);
            BlockResponse Response = new BlockResponse();
            try
            {
                List<MT_Block> AnesList = new List<MT_Block>();
                Query docRef = Db.Collection("MT_Block").WhereEqualTo("Block_Is_Deleted", true).OrderBy("Block_Name");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Block>());
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
    }
}
