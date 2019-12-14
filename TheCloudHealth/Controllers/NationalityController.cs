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
    public class NationalityController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public NationalityController()
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


        [Route("API/Nationality/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                UniqueID = con.GetUniqueKey();
                NMD.Nati_Unique_ID = UniqueID;
                NMD.Nati_Create_Date = con.ConvertTimeZone(NMD.Nati_TimeZone, Convert.ToDateTime(NMD.Nati_Create_Date));
                NMD.Nati_Modify_Date = con.ConvertTimeZone(NMD.Nati_TimeZone, Convert.ToDateTime(NMD.Nati_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Nationality").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(NMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Nationality/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Nati_Name", NMD.Nati_Name },
                    { "Nati_Modify_Date",con.ConvertTimeZone(NMD.Nati_TimeZone, Convert.ToDateTime(NMD.Nati_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Nationality").Document(NMD.Nati_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Nationality/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Nati_Modify_Date",con.ConvertTimeZone(NMD.Nati_TimeZone, Convert.ToDateTime(NMD.Nati_Modify_Date))},
                    { "Nati_Is_Active",NMD.Nati_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Nationality").Document(NMD.Nati_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Nationality/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Nati_Modify_Date",con.ConvertTimeZone(NMD.Nati_TimeZone, Convert.ToDateTime(NMD.Nati_Modify_Date))},
                    { "Nati_Is_Deleted",NMD.Nati_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Nationality").Document(NMD.Nati_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NMD;
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

        [Route("API/Nationality/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Nationality").Document(NMD.Nati_Unique_ID);
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
        [Route("API/Nationality/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                MT_Nationality Nati = new MT_Nationality();
                Query docRef = Db.Collection("MT_Nationality").WhereEqualTo("Nati_Unique_ID", NMD.Nati_Unique_ID).WhereEqualTo("Nati_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Nati = ObjQuerySnap.Documents[0].ConvertTo<MT_Nationality>();
                    Response.Data = Nati;
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

        [Route("API/Nationality/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                List<MT_Nationality> AnesList = new List<MT_Nationality>();
                Query docRef = Db.Collection("MT_Nationality").WhereEqualTo("Nati_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Nationality>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Nati_Name).ToList();
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

        [Route("API/Nationality/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                List<MT_Nationality> AnesList = new List<MT_Nationality>();
                Query docRef = Db.Collection("MT_Nationality").WhereEqualTo("Nati_Is_Deleted", false).WhereEqualTo("Nati_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Nationality>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Nati_Name).ToList();
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

        [Route("API/Nationality/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Nationality NMD)
        {
            Db = con.SurgeryCenterDb(NMD.Slug);
            NationalityResonse Response = new NationalityResonse();
            try
            {
                List<MT_Nationality> AnesList = new List<MT_Nationality>();
                Query docRef = Db.Collection("MT_Nationality").WhereEqualTo("Nati_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Nationality>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Nati_Name).ToList();
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
