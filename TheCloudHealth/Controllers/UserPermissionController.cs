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
    public class UserPermissionController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public UserPermissionController()
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

        [Route("API/UserPermission/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_User_Permission UPMD)
        {
            Db = con.SurgeryCenterDb(UPMD.Slug);
            UserPermissionResponse Response = new UserPermissionResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                UPMD.UP_Unique_ID = UniqueID;
                DocumentReference docRef = Db.Collection("MT_User_Permission").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(UPMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = UPMD;
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

        [Route("API/UserPermission/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_User_Permission UPMD)
        {
            Db = con.SurgeryCenterDb(UPMD.Slug);
            UserPermissionResponse Response = new UserPermissionResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Is_View", UPMD.Is_View},
                    {"Is_Add", UPMD.Is_Add},
                    {"Is_Edit", UPMD.Is_Edit},
                    {"Is_Delete", UPMD.Is_Delete},
                };
                DocumentReference docRef = Db.Collection("MT_User_Permission").Document(UPMD.UP_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = UPMD;
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

        [Route("API/UserPermission/ListFilterWithUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_User_Permission UPMD)
        {
            Db = con.SurgeryCenterDb(UPMD.Slug);
            UserPermissionResponse Response = new UserPermissionResponse();
            try
            {
                List<MT_User_Permission> PMList = new List<MT_User_Permission>();
                Query ObjQuery = Db.Collection("MT_User_Permission").WhereEqualTo("User_ID", UPMD.User_ID).WhereEqualTo("Category_Name", UPMD.Category_Name);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        PMList.Add(Docsnap.ConvertTo<MT_User_Permission>());
                    }

                    Response.DataList = PMList.OrderBy(o=>o.Category_Name).ToList();
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
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

        [Route("API/UserPermission/AssignDefaultPermission")]
        [HttpPost]
        public async Task<HttpResponseMessage> AssignDefaultPermission(MT_User_Permission UPMD)
        {
            Db = con.SurgeryCenterDb(UPMD.Slug);
            UserPermissionResponse Response = new UserPermissionResponse();
            try
            {
                MT_Page_Master PMaster = new MT_Page_Master();
                MT_User_Permission UPermission=new MT_User_Permission();
                Query QueryPageMater = Db.Collection("MT_Page_Master");
                QuerySnapshot ObjPageMasterSnap = await QueryPageMater.GetSnapshotAsync();
                if (ObjPageMasterSnap != null)
                {
                    foreach (DocumentSnapshot Snapshot in ObjPageMasterSnap.Documents)
                    {
                        PMaster = Snapshot.ConvertTo<MT_Page_Master>();
                        
                        UniqueID = con.GetUniqueKey();
                        UPermission.UP_Unique_ID = UniqueID;
                        UPermission.User_ID = UPMD.User_ID;
                        UPermission.Page_ID = PMaster.PM_Unique_ID;
                        UPermission.Page_Name = PMaster.Page_Name;
                        UPermission.Category_Name = PMaster.Category_Name;
                        UPermission.Is_View = true;
                        UPermission.Is_Add = true;
                        UPermission.Is_Edit = true;
                        UPermission.Is_Delete = true;

                        DocumentReference docRef = Db.Collection("MT_User_Permission").Document(UniqueID);
                        WriteResult Result = await docRef.SetAsync(UPermission);
                        if (Result != null)
                        {
                            Response.Status = con.StatusSuccess;
                            Response.Message = con.MessageSuccess;
                            Response.Data = UPMD;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/UserPermission/ListFilterWithUserID")]
        [HttpPost]
        public async Task<HttpResponseMessage> ListFilterWithUserID(MT_User_Permission UPMD)
        {
            Db = con.SurgeryCenterDb(UPMD.Slug);
            UserPermissionResponse Response = new UserPermissionResponse();
            try
            {
                List<MT_User_Permission> PMList = new List<MT_User_Permission>();
                Query ObjQuery = Db.Collection("MT_User_Permission").WhereEqualTo("User_ID", UPMD.User_ID).OrderBy("Category_Name");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        PMList.Add(Docsnap.ConvertTo<MT_User_Permission>());
                    }
                    Response.DataList = PMList;
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
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
    }
}
