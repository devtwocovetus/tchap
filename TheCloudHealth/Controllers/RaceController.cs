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
    public class RaceController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public RaceController()
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


        [Route("API/Race/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                RAMD.Race_Unique_ID = UniqueID;
                RAMD.Race_Create_Date = con.ConvertTimeZone(RAMD.Race_TimeZone, Convert.ToDateTime(RAMD.Race_Create_Date));
                RAMD.Race_Modify_Date = con.ConvertTimeZone(RAMD.Race_TimeZone, Convert.ToDateTime(RAMD.Race_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Race").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(RAMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RAMD;
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

        [Route("API/Race/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Race_Type", RAMD.Race_Type },
                    { "Race_Name", RAMD.Race_Name },
                    { "PhyO_Modify_Date",con.ConvertTimeZone(RAMD.Race_TimeZone, Convert.ToDateTime(RAMD.Race_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Race").Document(RAMD.Race_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RAMD;
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

        [Route("API/Race/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Race_Is_Active", RAMD.Race_Is_Active },
                    { "PhyO_Modify_Date",con.ConvertTimeZone(RAMD.Race_TimeZone, Convert.ToDateTime(RAMD.Race_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Race").Document(RAMD.Race_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RAMD;
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


        [Route("API/Race/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Race_Is_Deleted", RAMD.Race_Is_Deleted },
                    { "PhyO_Modify_Date",con.ConvertTimeZone(RAMD.Race_TimeZone, Convert.ToDateTime(RAMD.Race_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Race").Document(RAMD.Race_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RAMD;
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

        [Route("API/Race/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Race").Document(RAMD.Race_Unique_ID);
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
        [Route("API/Race/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                MT_Race Race = new MT_Race();
                Query docRef = Db.Collection("MT_Race").WhereEqualTo("Race_Unique_ID", UniqueID).WhereEqualTo("Race_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Race = ObjQuerySnap.Documents[0].ConvertTo<MT_Race>();
                    Response.Data = Race;
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

        [Route("API/Race/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                List<MT_Race> AnesList = new List<MT_Race>();
                Query docRef = Db.Collection("MT_Race").WhereEqualTo("Race_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Race>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Race_Name).ToList();
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

        [Route("API/Race/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                List<MT_Race> AnesList = new List<MT_Race>();
                Query docRef = Db.Collection("MT_Race").WhereEqualTo("Race_Is_Deleted", false).WhereEqualTo("Race_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Race>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Race_Name).ToList();
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

        [Route("API/Race/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Race RAMD)
        {
            Db = con.SurgeryCenterDb(RAMD.Slug);
            RaceResponse Response = new RaceResponse();
            try
            {
                List<MT_Race> AnesList = new List<MT_Race>();
                Query docRef = Db.Collection("MT_Race").WhereEqualTo("Race_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Race>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Race_Name).ToList();
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
