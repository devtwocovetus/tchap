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
    public class FormsController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public FormsController()
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

        [Route("API/Forms/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                FMD.Form_Unique_ID = UniqueID;
                FMD.Form_Create_Date = con.ConvertTimeZone(FMD.Form_TimeZone, Convert.ToDateTime(FMD.Form_Create_Date));
                FMD.Form_Modify_Date = con.ConvertTimeZone(FMD.Form_TimeZone, Convert.ToDateTime(FMD.Form_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Forms").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(FMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = FMD;
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

        [Route("API/Forms/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Form_Pack_ID",FMD.Form_Pack_ID},
                    {"Form_Pack_Name",FMD.Form_Pack_Name},
                    {"Form_Name",FMD.Form_Name},
                    {"Form_Description",FMD.Form_Description},
                    {"Form_Data",FMD.Form_Data},
                    {"Form_Modify_Date",con.ConvertTimeZone(FMD.Form_TimeZone, Convert.ToDateTime(FMD.Form_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Forms").Document(FMD.Form_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = FMD;
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

        [Route("API/Forms/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Form_Modify_Date",con.ConvertTimeZone(FMD.Form_TimeZone, Convert.ToDateTime(FMD.Form_Modify_Date))},
                    {"Form_Is_Active",FMD.Form_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Forms").Document(FMD.Form_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = FMD;
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

        [Route("API/Forms/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Form_Modify_Date",con.ConvertTimeZone(FMD.Form_TimeZone, Convert.ToDateTime(FMD.Form_Modify_Date))},
                    {"Form_Is_Deleted",FMD.Form_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Forms").Document(FMD.Form_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = FMD;
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

        [Route("API/Forms/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Forms").Document(FMD.Form_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = FMD;
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



        [Route("API/Forms/SaveFormBuilderData")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveFormBuilderData(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Form_Data",FMD.Form_Data},
                    {"Form_Logo",FMD.Form_Logo},
                    {"Form_Signature",FMD.Form_Signature},
                    {"Form_Modify_Date",con.ConvertTimeZone(FMD.Form_TimeZone, Convert.ToDateTime(FMD.Form_Modify_Date))}
                };

                DocumentReference docRef = Db.Collection("MT_Forms").Document(FMD.Form_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = FMD;
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

        [Route("API/Forms/Select")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> Select(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                MT_Forms Forms = new MT_Forms();
                Query docRef = Db.Collection("MT_Forms").WhereEqualTo("Form_Unique_ID", FMD.Form_Unique_ID).WhereEqualTo("Form_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Forms = ObjQuerySnap.Documents[0].ConvertTo<MT_Forms>();
                    Response.Data = Forms;
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

        [Route("API/Forms/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                List<MT_Forms> AnesList = new List<MT_Forms>();
                Query docRef = Db.Collection("MT_Forms").WhereEqualTo("Form_Is_Deleted", false).WhereEqualTo("Form_Surgery_Physician_Id", FMD.Form_Surgery_Physician_Id).OrderByDescending("Form_Create_Date");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Forms>());
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

        [Route("API/Forms/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                List<MT_Forms> AnesList = new List<MT_Forms>();
                Query docRef = Db.Collection("MT_Forms").WhereEqualTo("Form_Is_Deleted", false).WhereEqualTo("Form_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Forms>());
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

        [Route("API/Forms/GetFormFilterWithPackID")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetFormFilterWithPackID(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                List<MT_Forms> AnesList = new List<MT_Forms>();
                Query docRef = Db.Collection("MT_Forms").WhereEqualTo("Form_Is_Deleted", false).WhereEqualTo("Form_Pack_ID", FMD.Form_Pack_ID);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (Docsnapshot.ConvertTo<MT_Forms>().Form_Surgery_Physician_Id == "0" || Docsnapshot.ConvertTo<MT_Forms>().Form_Surgery_Physician_Id == FMD.Form_Surgery_Physician_Id)
                        {
                            AnesList.Add(Docsnapshot.ConvertTo<MT_Forms>());
                        }
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

        [Route("API/Forms/GetFormFilterWithSurgeryPhysicianCenterID")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetFormFilterWithSurgeryPhysicianCenterID(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                List<MT_Forms> AnesList = new List<MT_Forms>();
                Query docRef = Db.Collection("MT_Forms").WhereEqualTo("Form_Is_Deleted", false).WhereEqualTo("Form_Is_Active", true).WhereEqualTo("Form_Surgery_Physician_Id", FMD.Form_Surgery_Physician_Id);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Forms>());
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

        [Route("API/Forms/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Forms FMD)
        {
            Db = con.SurgeryCenterDb(FMD.Slug);
            FormsResponse Response = new FormsResponse();
            try
            {
                List<MT_Forms> AnesList = new List<MT_Forms>();
                Query docRef = Db.Collection("MT_Forms").WhereEqualTo("Form_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Forms>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Form_Pack_Name).ThenBy(o => o.Form_Name).ToList();
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
