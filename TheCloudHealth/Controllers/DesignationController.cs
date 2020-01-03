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
    public class DesignationController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public DesignationController()
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

        [Route("API/Designation/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                DMD.Desi_UniqueID = UniqueID;
                DMD.Desi_Create_Date = con.ConvertTimeZone(DMD.Desi_TimeZone, Convert.ToDateTime(DMD.Desi_Create_Date));
                DMD.Desi_Modify_Date = con.ConvertTimeZone(DMD.Desi_TimeZone, Convert.ToDateTime(DMD.Desi_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Designation").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(DMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DMD;
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



        [Route("API/Designation/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Desi_Name", DMD.Desi_Name },
                    { "Desi_Description", DMD.Desi_Description },
                    { "Desi_Modify_Date",con.ConvertTimeZone(DMD.Desi_TimeZone, Convert.ToDateTime(DMD.Desi_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Designation").Document(DMD.Desi_UniqueID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DMD;
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

        [Route("API/Designation/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Desi_Modify_Date",con.ConvertTimeZone(DMD.Desi_TimeZone, Convert.ToDateTime(DMD.Desi_Modify_Date))},
                    { "Desi_Is_Active",DMD.Desi_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Designation").Document(DMD.Desi_UniqueID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DMD;
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

        [Route("API/Designation/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Desi_Modify_Date",con.ConvertTimeZone(DMD.Desi_TimeZone, Convert.ToDateTime(DMD.Desi_Modify_Date))},
                    { "Desi_Is_Deleted",DMD.Desi_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Designation").Document(DMD.Desi_UniqueID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = DMD;
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

        [Route("API/Designation/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Designation").Document(DMD.Desi_UniqueID);
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
        [Route("API/Designation/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                Query docRef = Db.Collection("MT_Designation").WhereEqualTo("Desi_UniqueID", DMD.Desi_UniqueID).WhereEqualTo("Desi_Is_Deleted", false);
                QuerySnapshot ObjDocSnap = await docRef.GetSnapshotAsync();
                if (ObjDocSnap != null)
                {
                    Response.Data = ObjDocSnap.Documents[0].ConvertTo<MT_Designation>();
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                }
                else
                {
                    Response.Data = null;
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Designation/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                List<MT_Designation> AnesList = new List<MT_Designation>();
                Query docRef = con.Db().Collection("MT_Designation").WhereEqualTo("Desi_Is_Deleted", false).WhereEqualTo("Desi_Created_By", DMD.Desi_Created_By);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Designation>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Desi_Name).ToList();
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

        [Route("API/Designation/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                List<MT_Designation> AnesList = new List<MT_Designation>();
                Query docRef = con.Db().Collection("MT_Designation").WhereEqualTo("Desi_Is_Deleted", false).WhereEqualTo("Desi_Is_Active", true).WhereEqualTo("Desi_Created_By", DMD.Desi_Created_By);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Designation>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Desi_Name).ToList();
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

        [Route("API/Designation/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Designation DMD)
        {
            Db = con.SurgeryCenterDb(DMD.Slug);
            DesignationResponse Response = new DesignationResponse();
            try
            {
                List<MT_Designation> AnesList = new List<MT_Designation>();
                Query docRef = con.Db().Collection("MT_Designation").WhereEqualTo("Desi_Is_Deleted", true).WhereEqualTo("Desi_Created_By", DMD.Desi_Created_By);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Designation>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Desi_Name).ToList();
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
