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
    public class InstrumentsController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public InstrumentsController()
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

        [Route("API/Instruments/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                IMD.Instru_Unique_ID = UniqueID;
                IMD.Instru_Create_Date = con.ConvertTimeZone(IMD.Instru_TimeZone, Convert.ToDateTime(IMD.Instru_Create_Date));
                IMD.Instru_Modify_Date = con.ConvertTimeZone(IMD.Instru_TimeZone, Convert.ToDateTime(IMD.Instru_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Instrument").Document(UniqueID);
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

        [Route("API/Instruments/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Instru_Name", IMD.Instru_Name },
                    { "Instru_Type", IMD.Instru_Type },
                    { "Instru_Description", IMD.Instru_Description },
                    { "Instru_Modify_Date",con.ConvertTimeZone(IMD.Instru_TimeZone, Convert.ToDateTime(IMD.Instru_Modify_Date))},
                    { "Instru_Surgery_Physician_Id",IMD.Instru_Surgery_Physician_Id}
                };

                DocumentReference docRef = Db.Collection("MT_Instrument").Document(IMD.Instru_Unique_ID);
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

        [Route("API/Instruments/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Instru_Modify_Date",con.ConvertTimeZone(IMD.Instru_TimeZone, Convert.ToDateTime(IMD.Instru_Modify_Date))},
                    { "Instru_Is_Active",IMD.Instru_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Instrument").Document(IMD.Instru_Unique_ID);
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

        [Route("API/Instruments/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Instru_Modify_Date",con.ConvertTimeZone(IMD.Instru_TimeZone, Convert.ToDateTime(IMD.Instru_Modify_Date))},
                    { "Instru_Is_Deleted",IMD.Instru_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Instrument").Document(IMD.Instru_Unique_ID);
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

        [Route("API/Instruments/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Instrument").Document(IMD.Instru_Unique_ID);
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

        [Route("API/Instruments/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                MT_Instrument AnesList = new MT_Instrument();
                Query docRef = Db.Collection("MT_Instrument").WhereEqualTo("Instru_Unique_ID", IMD.Instru_Unique_ID).WhereEqualTo("Instru_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    AnesList = ObjQuerySnap.Documents[0].ConvertTo<MT_Instrument>();
                    Response.Data = AnesList;
                }
                Response.Status = con.StatusNotDeleted;
                Response.Message = con.MessageNotDeleted;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Instruments/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                List<MT_Instrument> AnesList = new List<MT_Instrument>();
                Query docRef = Db.Collection("MT_Instrument").WhereEqualTo("Instru_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Instrument>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Instru_Name).ToList();
                }
                Response.Status = con.StatusNotDeleted;
                Response.Message = con.MessageNotDeleted;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }


        [Route("API/Instruments/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                List<MT_Instrument> AnesList = new List<MT_Instrument>();
                Query docRef = Db.Collection("MT_Instrument").WhereEqualTo("Instru_Is_Deleted", false).WhereEqualTo("Instru_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Instrument>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Instru_Name).ToList();
                }
                Response.Status = con.StatusNotDeleted;
                Response.Message = con.MessageNotDeleted;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Instruments/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Instrument IMD)
        {
            Db = con.SurgeryCenterDb(IMD.Slug);
            InstrumentResponse Response = new InstrumentResponse();
            try
            {
                List<MT_Instrument> AnesList = new List<MT_Instrument>();
                Query docRef = Db.Collection("MT_Instrument").WhereEqualTo("Instru_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Instrument>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Instru_Name).ToList();
                }
                Response.Status = con.StatusNotDeleted;
                Response.Message = con.MessageNotDeleted;
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
