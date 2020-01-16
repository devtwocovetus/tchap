using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCloudHealth.Filters;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;

namespace TheCloudHealth.Controllers
{
    [AllowCrossSiteJson]
    public class UserController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public UserController()
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

        [Route("API/User/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                UMD.UM_Unique_ID = UniqueID;
                UMD.UM_Create_Date = con.ConvertTimeZone(UMD.UM_TimeZone, Convert.ToDateTime(UMD.UM_Create_Date));
                UMD.UM_Modify_Date = con.ConvertTimeZone(UMD.UM_TimeZone, Convert.ToDateTime(UMD.UM_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_User").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(UMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = UMD;
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

        [Route("API/User/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "UM_Password", UMD.UM_Password },
                    { "UM_Email", UMD.UM_Email },
                    { "UM_PhoneNo", UMD.UM_PhoneNo },
                    { "UM_Role_Type", UMD.UM_Role_Type },
                    { "UM_Is_Deleted", UMD.UM_Is_Deleted },
                    { "UM_Modify_Date", con.ConvertTimeZone(UMD.UM_TimeZone, Convert.ToDateTime(UMD.UM_Modify_Date))},
                    { "UM_Is_Active",UMD.UM_Is_Active},
                    { "UM_Is_Deleted",UMD.UM_Is_Deleted}
                };
                DocumentReference docRef = Db.Collection("MT_User").Document(UMD.UM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = UMD;
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

        [Route("API/User/IsActiveWithUMID")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActiveWithUMID(MT_User UMD)
        {
            
            UserResponse Response = new UserResponse();
            try
            {
                MT_User user = new MT_User();
                Query UserQuery = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Member_ID", UMD.UM_Member_ID);
                QuerySnapshot ObjDocSnap = await UserQuery.GetSnapshotAsync();
                if (ObjDocSnap != null)
                {
                    user = ObjDocSnap.Documents[0].ConvertTo<MT_User>();
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        { "UM_Is_Active",UMD.UM_Is_Active}
                    };

                    DocumentReference docRef = Db.Collection("MT_User").Document(user.UM_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = UMD;
                    }
                    else
                    {
                        Response.Status = con.StatusNotUpdate;
                        Response.Message = con.MessageNotUpdate;
                        Response.Data = null;
                    }
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                }
                
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/User/IsDeletedWithUMID")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeletedWithUMID(MT_User UMD)
        {
            UserResponse Response = new UserResponse();
            try
            {
                MT_User user = new MT_User();
                Query UserQuery = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Member_ID", UMD.UM_Member_ID);
                QuerySnapshot ObjDocSnap = await UserQuery.GetSnapshotAsync();
                if (ObjDocSnap != null)
                {
                    user = ObjDocSnap.Documents[0].ConvertTo<MT_User>();
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        { "UM_Is_Deleted",UMD.UM_Is_Deleted}
                    };

                    DocumentReference docRef = Db.Collection("MT_User").Document(user.UM_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = UMD;
                    }
                    else
                    {
                        Response.Status = con.StatusNotUpdate;
                        Response.Message = con.MessageNotUpdate;
                        Response.Data = null;
                    }
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                }

            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/User/Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAsync(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_User").Document(UMD.UM_Unique_ID);
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
        [Route("API/User/Select")]
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(string UniqueID)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {
                Query docRef = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Unique_ID", UniqueID);
                QuerySnapshot ObjDocSnap = await docRef.GetSnapshotAsync();
                if (ObjDocSnap!=null)
                {
                    Response.Data = ObjDocSnap.Documents[0].ConvertTo<MT_User>();
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

        [Route("API/User/Login")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> LoginAsync(MT_User Objuser)
        {
            //Db = con.SurgeryCenterDb(Objuser.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {

                MT_User user = new MT_User();
                Query colref = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Is_Active", true);
                QuerySnapshot ObjDocSnap = await colref.GetSnapshotAsync();
                if (ObjDocSnap!=null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjDocSnap.Documents)
                    {
                        user = Docsnapshot.ConvertTo<MT_User>();
                        if (user != null)
                        {
                            if ((user.UM_Email.ToLower() == Objuser.UM_Email.ToLower() || user.UM_PhoneNo == Objuser.UM_PhoneNo) && user.UM_Password == Objuser.UM_Password)
                            {
                                Response.Data = user;
                                Response.Status = con.StatusSuccess;
                                Response.Message = con.MessageSuccess;
                            }
                        }
                        else
                        {
                            Response.Data = null;
                            Response.Status = con.StatusSuccess;
                            Response.Message = con.MessageSuccess;
                        }
                    }
                    
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

        [Route("API/User/VarifyPasscode")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> VarifyPasscode(MT_User Objuser)
        {
            //Db = con.SurgeryCenterDb(Objuser.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {

                MT_User user = new MT_User();
                Query colref = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Is_Active", true).WhereEqualTo("UM_Passcode", Objuser.UM_Passcode).WhereEqualTo("UM_Unique_ID", Objuser.UM_Unique_ID);
                QuerySnapshot ObjDocSnap = await colref.GetSnapshotAsync();
                if (ObjDocSnap != null)
                {
                    user = ObjDocSnap.Documents[0].ConvertTo<MT_User>();
                    if (user.UM_Unique_ID != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                    }
                    else
                    {
                        Response.Status = con.StatusDNE;
                        Response.Message = con.MessageDNE;
                    }
                }
                else
                {
                    Response.Data = null;
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = "Passcode Expired";
            }
            return ConvertToJSON(Response);
        }

        [Route("API/User/ResetPassword")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ResetPassword(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(Objuser.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {

                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "UM_Password", UMD.UM_Password },
                    { "UM_Passcode", UMD.UM_Passcode }
                };
                DocumentReference docRef = Db.Collection("MT_User").Document(UMD.UM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = UMD;
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

        [Route("API/User/List")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List()
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {
                List<MT_User> AnesList = new List<MT_User>();
                CollectionReference docRef = con.Db().Collection("MT_User");
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_User>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.UM_User_Name).ToList();
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

        [Route("API/User/GetDeletedList")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {
                List<MT_User> AnesList = new List<MT_User>();
                Query docRef = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted",true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_User>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.UM_User_Name).ToList();
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

        [Route("API/User/CheckEmail")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> CheckEmailAsync(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserCEmailResponse Response = new UserCEmailResponse();
            try
            {
                List<MT_User> AnesList = new List<MT_User>();
                Query docRef = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Email", UMD.UM_Email);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap.Documents.Count == 0)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Is_Available = true;
                }
                else
                {
                    Response.Status = con.StatusAE;
                    Response.Message = con.MessageAE;
                    Response.Is_Available = false;
                }
                
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/User/CheckOldPassword")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> CheckOldPassword(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(UMD.Project_ID);
            UserCEmailResponse Response = new UserCEmailResponse();
            try
            {
                List<MT_User> AnesList = new List<MT_User>();
                Query docRef = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Unique_ID", UMD.UM_Unique_ID).WhereEqualTo("UM_Password", UMD.UM_Password);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap!=null && ObjQuerySnap.Documents.Count>0)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Is_Available = true;
                }
                else
                {
                    Response.Status = con.StatusAE;
                    Response.Message = con.MessageAE;
                    Response.Is_Available = false;
                }

            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/User/PatientResetPassword")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> PatientResetPassword(MT_User UMD)
        {
            //Db = con.SurgeryCenterDb(Objuser.Project_ID);
            UserResponse Response = new UserResponse();
            try
            {

                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "UM_Password", UMD.UM_Password }
                };
                DocumentReference docRef = Db.Collection("MT_User").Document(UMD.UM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = UMD;
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

        [Route("API/User/Dcrypt")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public HttpResponseMessage Dcrypt(EnDe Ende)
        {
            DeEn Response = new DeEn();
            try
            {
                Response.Output = con.DecryptData(Ende.Input);
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response); ;
        }

    }
}
