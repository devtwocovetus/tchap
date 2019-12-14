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
    public class RelationController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public RelationController()
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


        [Route("API/Relation/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                RPMD.Rtop_Unique_ID = UniqueID;
                RPMD.Rtop_Create_Date = con.ConvertTimeZone(RPMD.Rtop_TimeZone, Convert.ToDateTime(RPMD.Rtop_Create_Date));
                RPMD.Rtop_Modify_Date = con.ConvertTimeZone(RPMD.Rtop_TimeZone, Convert.ToDateTime(RPMD.Rtop_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_RelationToParent").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(RPMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RPMD;
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

        [Route("API/Relation/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Rtop_Type", RPMD.Rtop_Type },
                    { "Rtop_Name", RPMD.Rtop_Name },
                    { "Rtop_Modify_Date",con.ConvertTimeZone(RPMD.Rtop_TimeZone, Convert.ToDateTime(RPMD.Rtop_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_RelationToParent").Document(RPMD.Rtop_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RPMD;
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


        [Route("API/Relation/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Rtop_Is_Active", RPMD.Rtop_Is_Active },
                    { "Rtop_Modify_Date",con.ConvertTimeZone(RPMD.Rtop_TimeZone, Convert.ToDateTime(RPMD.Rtop_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_RelationToParent").Document(RPMD.Rtop_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RPMD;
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

        [Route("API/Relation/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Rtop_Is_Deleted", RPMD.Rtop_Is_Deleted },
                    { "Rtop_Modify_Date",con.ConvertTimeZone(RPMD.Rtop_TimeZone, Convert.ToDateTime(RPMD.Rtop_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_RelationToParent").Document(RPMD.Rtop_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RPMD;
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

        [Route("API/Relation/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_RelationToParent").Document(RPMD.Rtop_Unique_ID);
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
        [Route("API/Relation/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                MT_RelationToParent RTP = new MT_RelationToParent();
                Query docRef = Db.Collection("MT_RelationToParent").WhereEqualTo("Rtop_Unique_ID", RPMD.Rtop_Unique_ID).WhereEqualTo("Rtop_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    RTP = ObjQuerySnap.Documents[0].ConvertTo<MT_RelationToParent>();
                    Response.Data = RTP;
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

        [Route("API/Relation/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                List<MT_RelationToParent> AnesList = new List<MT_RelationToParent>();
                Query docRef = Db.Collection("MT_RelationToParent").WhereEqualTo("Rtop_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_RelationToParent>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Rtop_Name).ToList();
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

        [Route("API/Relation/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                List<MT_RelationToParent> AnesList = new List<MT_RelationToParent>();
                Query docRef = Db.Collection("MT_RelationToParent").WhereEqualTo("Rtop_Is_Deleted", false).WhereEqualTo("Rtop_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_RelationToParent>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Rtop_Name).ToList();
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

        [Route("API/Relation/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_RelationToParent RPMD)
        {
            Db = con.SurgeryCenterDb(RPMD.Slug);
            RelaToParentResponse Response = new RelaToParentResponse();
            try
            {
                List<MT_RelationToParent> AnesList = new List<MT_RelationToParent>();
                Query docRef = Db.Collection("MT_RelationToParent").WhereEqualTo("Rtop_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_RelationToParent>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Rtop_Name).ToList();
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
