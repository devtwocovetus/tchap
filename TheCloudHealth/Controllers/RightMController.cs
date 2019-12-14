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
    public class RightMController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        string UniqueID_Detail = "";
        public RightMController()
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

        [Route("API/RightM/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Right RMD)
        {
            RightResponse Response = new RightResponse();
            try
            {
                //Right Master
                UniqueID = con.GetUniqueKey();
                UniqueID_Detail = con.GetUniqueKey();
                RMD.RM_Unique_ID = UniqueID;
                RMD.RM_Create_Date = con.ConvertTimeZone(RMD.RM_TimeZone, Convert.ToDateTime(RMD.RM_Create_Date));
                RMD.RM_Modify_Date = con.ConvertTimeZone(RMD.RM_TimeZone, Convert.ToDateTime(RMD.RM_Modify_Date));
                //Right Details
                RMD.RM_Right_Details.RD_Unique_ID = UniqueID_Detail;
                RMD.RM_Right_Details.RD_Right_Master_ID = UniqueID;
                DocumentReference docRef = Db.Collection("MT_Right").Document(UniqueID);
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

        [Route("API/RightM/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Right RMD)
        {
            RightResponse Response = new RightResponse();
            try
            {
                MT_Right_Details RDoc = new MT_Right_Details();
                RDoc.RD_Unique_ID = RMD.RM_Right_Details.RD_Unique_ID;
                RDoc.RD_Right_Master_ID = RMD.RM_Right_Details.RD_Right_Master_ID;
                RDoc.RD_Add = RMD.RM_Right_Details.RD_Add;
                RDoc.RD_Edit = RMD.RM_Right_Details.RD_Edit;
                RDoc.RD_Delete = RMD.RM_Right_Details.RD_Delete;
                RDoc.RD_View = RMD.RM_Right_Details.RD_View;
                RDoc.RD_Is_Active = RMD.RM_Right_Details.RD_Is_Active;
                RDoc.RD_Is_Deleted = RMD.RM_Right_Details.RD_Is_Deleted;
                RDoc.RD_Created_By = RMD.RM_Right_Details.RD_Created_By;
                RDoc.RD_Created_By_Type = RMD.RM_Right_Details.RD_Created_By_Type;
                
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "RM_Category_Name", RMD.RM_Category_Name },
                    { "RM_Sub_Category_Name", RMD.RM_Sub_Category_Name },
                    { "RM_Page_Name", RMD.RM_Page_Name },
                    { "RM_Is_Active", RMD.RM_Is_Active },
                    { "RM_Is_Deleted", RMD.RM_Is_Deleted },
                    { "RM_Modify_Date",con.ConvertTimeZone(RMD.RM_TimeZone, Convert.ToDateTime(RMD.RM_Modify_Date))},
                    { "RM_Right_Details",RDoc}
                };
                DocumentReference docRef = Db.Collection("MT_Right").Document(RMD.RM_Unique_ID);
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

        [Route("API/RightM/Delete")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAsync(MT_Right RMD)
        {
            RightResponse Response = new RightResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Right").Document(RMD.RM_Unique_ID);
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
        [Route("API/RightM/Select")]
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(string UniqueID)
        {
            RightResponse Response = new RightResponse();
            try
            {
                MT_Right right = new MT_Right();
                Query docRef = Db.Collection("MT_Right").WhereEqualTo("RM_Unique_ID", UniqueID).WhereEqualTo("RM_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    right = ObjQuerySnap.Documents[0].ConvertTo<MT_Right>();
                    Response.Data = right;
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

        [Route("API/RightM/List")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetAsync()
        {
            RightResponse Response = new RightResponse();
            try
            {
                List<MT_Right> AnesList = new List<MT_Right>();
                Query docRef = Db.Collection("MT_Right").WhereEqualTo("RM_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Right>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.RM_Category_Name).ThenBy(o => o.RM_Page_Name).ToList();
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

        [Route("API/RightM/GetDeletedList")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList()
        {
            RightResponse Response = new RightResponse();
            try
            {
                List<MT_Right> AnesList = new List<MT_Right>();
                Query docRef = Db.Collection("MT_Right").WhereEqualTo("RM_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Right>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.RM_Category_Name).ThenBy(o => o.RM_Page_Name).ToList();
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



        [Route("API/RightM/GetGroupList")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetGroupList()
        {
            RightResponse Response = new RightResponse();
            try
            {
                List<string> grouplist = new List<string>();
                Query docRef = Db.Collection("MT_Right").WhereEqualTo("RM_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (!grouplist.Contains(Docsnapshot.ConvertTo<MT_Right>().RM_Category_Name))
                        {
                            grouplist.Add(Docsnapshot.ConvertTo<MT_Right>().RM_Category_Name);
                        }
                    }
                }
                var jObject = JsonConvert.SerializeObject(grouplist);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("API/RightM/GetListFilterWithGroup")]
        [HttpGet]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetListFilterWithGroup(string groupname)
        {
            RightResponse Response = new RightResponse();
            try
            {
                MT_Right Equip = new MT_Right();
                MT_User usr = new MT_User();

                List<MT_Right> RightList = new List<MT_Right>();
                Query docRef = Db.Collection("MT_Right").WhereEqualTo("RM_Is_Deleted", false).WhereEqualTo("RM_Category_Name", groupname);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        RightList.Add(Docsnapshot.ConvertTo<MT_Right>());
                    }
                    Response.DataList = RightList.OrderBy(o => o.RM_Category_Name).ThenBy(o => o.RM_Page_Name).ToList();
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
