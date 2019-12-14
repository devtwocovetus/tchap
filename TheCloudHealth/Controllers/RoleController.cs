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
    public class RoleController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public RoleController()
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

        [Route("API/Role/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_User_Right_Priviliages> priviliages = new List<MT_User_Right_Priviliages>();
                UniqueID = con.GetUniqueKey();

                RMD.ROM_Unique_ID = UniqueID;
                RMD.ROM_Create_Date = con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Create_Date));
                RMD.ROM_Modify_Date = con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date));
                if (RMD.ROM_Priviliages != null)
                {
                    foreach (MT_User_Right_Priviliages URP in RMD.ROM_Priviliages)
                    {
                        URP.URP_Unique_ID = con.GetUniqueKey();
                        URP.URP_Role_ID = UniqueID;
                        priviliages.Add(URP);
                    }
                }
                RMD.ROM_Priviliages = priviliages;
                DocumentReference docRef = Db.Collection("MT_Role").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(RMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RMD;
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

        [Route("API/Role/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_User_Right_Priviliages> priviliages = new List<MT_User_Right_Priviliages>();
                Dictionary<string, object> initialData;
                if (RMD.ROM_Priviliages != null)
                {
                    foreach (MT_User_Right_Priviliages URP in RMD.ROM_Priviliages)
                    {
                        priviliages.Add(URP);
                    }
                    RMD.ROM_Priviliages = priviliages;
                    initialData = new Dictionary<string, object>
                    {
                        { "ROM_Name", RMD.ROM_Name },
                        { "ROM_Description", RMD.ROM_Description },
                        { "ROM_Is_Active", RMD.ROM_Is_Active },
                        { "ROM_Is_Deleted", RMD.ROM_Is_Deleted },
                        { "ROM_Modify_Date",con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date))},
                        { "ROM_Priviliages",priviliages}
                    };
                }
                else
                {
                    initialData = new Dictionary<string, object>
                    {
                        { "ROM_Name", RMD.ROM_Name },
                        { "ROM_Description", RMD.ROM_Description },
                        { "ROM_Is_Active", RMD.ROM_Is_Active },
                        { "ROM_Is_Deleted", RMD.ROM_Is_Deleted },
                        { "ROM_Modify_Date",con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date))}
                    };
                }

                DocumentReference docRef = Db.Collection("MT_Role").Document(RMD.ROM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RMD;
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

        [Route("API/Role/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_User_Right_Priviliages> priviliages = new List<MT_User_Right_Priviliages>();
                Dictionary<string, object> initialData;
                if (RMD.ROM_Priviliages != null)
                {
                    foreach (MT_User_Right_Priviliages URP in RMD.ROM_Priviliages)
                    {
                        priviliages.Add(URP);
                    }
                    RMD.ROM_Priviliages = priviliages;
                    initialData = new Dictionary<string, object>
                    {
                        { "ROM_Is_Active", RMD.ROM_Is_Active },
                        { "ROM_Modify_Date",con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date))},
                    };
                }
                else
                {
                    initialData = new Dictionary<string, object>
                    {
                        { "ROM_Is_Active", RMD.ROM_Is_Active },
                        { "ROM_Modify_Date",con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date))}
                    };
                }

                DocumentReference docRef = Db.Collection("MT_Role").Document(RMD.ROM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RMD;
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

        [Route("API/Role/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_User_Right_Priviliages> priviliages = new List<MT_User_Right_Priviliages>();
                Dictionary<string, object> initialData;
                if (RMD.ROM_Priviliages != null)
                {
                    foreach (MT_User_Right_Priviliages URP in RMD.ROM_Priviliages)
                    {
                        priviliages.Add(URP);
                    }
                    RMD.ROM_Priviliages = priviliages;
                    initialData = new Dictionary<string, object>
                    {
                        { "ROM_Is_Deleted", RMD.ROM_Is_Deleted },
                        { "ROM_Modify_Date",con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date))},
                    };
                }
                else
                {
                    initialData = new Dictionary<string, object>
                    {
                        { "ROM_Is_Deleted", RMD.ROM_Is_Deleted },
                        { "ROM_Modify_Date",con.ConvertTimeZone(RMD.ROM_TimeZone, Convert.ToDateTime(RMD.ROM_Modify_Date))}
                    };
                }

                DocumentReference docRef = Db.Collection("MT_Role").Document(RMD.ROM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = RMD;
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

        [Route("API/Role/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Role").Document(RMD.ROM_Unique_ID);
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
        [Route("API/Role/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                MT_Role role = new MT_Role();
                Query docRef = Db.Collection("MT_Role").WhereEqualTo("ROM_Unique_ID", RMD.ROM_Unique_ID).WhereEqualTo("ROM_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    role = ObjQuerySnap.Documents[0].ConvertTo<MT_Role>();
                    Response.Data = role;
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

        [Route("API/Role/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_Role> AnesList = new List<MT_Role>();
                Query docRef = Db.Collection("MT_Role").WhereEqualTo("ROM_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Role>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ROM_Name).ToList();
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

        [Route("API/Role/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_Role> AnesList = new List<MT_Role>();
                Query docRef = Db.Collection("MT_Role").WhereEqualTo("ROM_Is_Deleted", false).WhereEqualTo("ROM_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Role>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ROM_Name).ToList();
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

        [Route("API/Role/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_Role> AnesList = new List<MT_Role>();
                Query docRef = Db.Collection("MT_Role").WhereEqualTo("ROM_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Role>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ROM_Name).ToList();
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

        [Route("API/Role/GetRolesFilterwithSC")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetRolesFilterwithSC(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_Role> AnesList = new List<MT_Role>();
                Query docRef = Db.Collection("MT_Role").WhereEqualTo("ROM_Is_Deleted", false).WhereEqualTo("ROM_Surgery_Physician_Center_ID", RMD.ROM_Surgery_Physician_Center_ID).WhereEqualTo("ROM_Office_Type", RMD.ROM_Office_Type);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Role>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ROM_Name).ToList();
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

        [Route("API/Role/GetRolesFilterwithPO")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetRolesFilterwithPO(MT_Role RMD)
        {
            Db = con.SurgeryCenterDb(RMD.Slug);
            RoleResponse Response = new RoleResponse();
            try
            {
                List<MT_Role> AnesList = new List<MT_Role>();
                Query docRef = Db.Collection("MT_Role").WhereEqualTo("ROM_Is_Deleted", false).WhereEqualTo("ROM_Is_Active", true).WhereEqualTo("ROM_Surgery_Physician_Center_ID", RMD.ROM_Surgery_Physician_Center_ID).WhereEqualTo("ROM_Office_Type", RMD.ROM_Office_Type);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Role>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.ROM_Name).ToList();
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
