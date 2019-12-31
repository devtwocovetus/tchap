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
using System.Web;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TheCloudHealth.Controllers
{
    public class NotificationsController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public NotificationsController()
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

        [Route("API/Notifications/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {
                List<Notification_Action> ActionList = new List<Notification_Action>();
                UniqueID = con.GetUniqueKey();
                NSMD.NFT_Unique_ID = UniqueID;
                NSMD.NFT_Create_Date = con.ConvertTimeZone(NSMD.NFT_TimeZone, Convert.ToDateTime(NSMD.NFT_Create_Date));
                NSMD.NFT_Modify_Date = con.ConvertTimeZone(NSMD.NFT_TimeZone, Convert.ToDateTime(NSMD.NFT_Modify_Date));
                NSMD.NFT_Actions = ActionList;
                DocumentReference docRef = Db.Collection("MT_Notifications").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(NSMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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

        [Route("API/Notifications/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"NFT_Category_ID", NSMD.NFT_Category_ID},
                    {"NFT_Category_Name", NSMD.NFT_Category_Name},
                    {"NFT_Category_Type_ID", NSMD.NFT_Category_Type_ID},
                    {"NFT_Category_Type_Name", NSMD.NFT_Category_Type_Name},
                    {"NFT_Name",NSMD.NFT_Name},
                    {"NFT_Description",NSMD.NFT_Description},
                    {"NFT_Modify_Date",con.ConvertTimeZone(NSMD.NFT_TimeZone,Convert.ToDateTime(NSMD.NFT_Modify_Date))},                    
                    {"NFT_TimeZone", NSMD.NFT_TimeZone}                    
                 };

                DocumentReference docRef = Db.Collection("MT_Notifications").Document(NSMD.NFT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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

        [Route("API/Notifications/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {                    
                    {"NFT_Is_Active",NSMD.NFT_Is_Active},
                    {"NFT_Modify_Date",con.ConvertTimeZone(NSMD.NFT_TimeZone,Convert.ToDateTime(NSMD.NFT_Modify_Date))},
                    {"NFT_TimeZone", NSMD.NFT_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Notifications").Document(NSMD.NFT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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

        [Route("API/Notifications/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"NFT_Is_Deleted",NSMD.NFT_Is_Deleted},
                    {"NFT_Modify_Date",con.ConvertTimeZone(NSMD.NFT_TimeZone,Convert.ToDateTime(NSMD.NFT_Modify_Date))},
                    {"NFT_TimeZone", NSMD.NFT_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Notifications").Document(NSMD.NFT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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

        [Route("API/Notifications/SelectAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> SelectAction(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true).WhereEqualTo("NFT_Unique_ID", NSMD.NFT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Notifications noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Notifications>();
                    if (noti.NFT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.NFT_Actions)
                        {
                            if (action.NFA_Unique_ID == NSMD.NFT_Actions[0].NFA_Unique_ID)
                            {
                                ActionList.Add(action);
                            }
                            
                        }
                    }
                    noti.NFT_Actions = ActionList;
                    Response.Data = noti;
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

        [Route("API/Notifications/AddAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddAction(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true).WhereEqualTo("NFT_Unique_ID", NSMD.NFT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Notifications noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Notifications>();
                    if (noti.NFT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.NFT_Actions)
                        {
                            ActionList.Add(action);
                        }
                    }
                }

                if (NSMD.NFT_Actions != null)
                {
                    foreach (Notification_Action Notia in NSMD.NFT_Actions)
                    {
                        Notia.NFA_Unique_ID = con.GetUniqueKey();
                        Notia.NFA_Create_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Create_Date));
                        Notia.NFA_Modify_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Modify_Date));
                        ActionList.Add(Notia);
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                     {"NFT_Actions", ActionList},
                     {"NFT_Modify_Date", con.ConvertTimeZone(NSMD.NFT_TimeZone, Convert.ToDateTime(NSMD.NFT_Modify_Date))},
                 };

                DocumentReference docRef = Db.Collection("MT_Notifications").Document(NSMD.NFT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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

        [Route("API/Notifications/EditAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditAction(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true).WhereEqualTo("NFT_Unique_ID", NSMD.NFT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Notifications noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Notifications>();
                    if (noti.NFT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.NFT_Actions)
                        {
                            if (action.NFA_Unique_ID == NSMD.NFT_Actions[0].NFA_Unique_ID)
                            {
                                action.NFA_Action_Type = NSMD.NFT_Actions[0].NFA_Action_Type;
                                action.NFA_Action_Title = NSMD.NFT_Actions[0].NFA_Action_Title;
                                action.NFA_Action_Icon = NSMD.NFT_Actions[0].NFA_Action_Icon;
                                action.NFA_Action_Subject = NSMD.NFT_Actions[0].NFA_Action_Subject;
                                action.NFA_Be_Af = NSMD.NFT_Actions[0].NFA_Be_Af;
                                action.NFA_Days = NSMD.NFT_Actions[0].NFA_Days;
                                action.NFA_DayOrWeek = NSMD.NFT_Actions[0].NFA_DayOrWeek;
                                action.NFA_Message = NSMD.NFT_Actions[0].NFA_Message;
                                action.NFA_Modify_Date = con.ConvertTimeZone(NSMD.NFT_Actions[0].NFA_TimeZone, Convert.ToDateTime(NSMD.NFT_Actions[0].NFA_Modify_Date));
                                action.NFA_TimeZone = NSMD.NFT_Actions[0].NFA_TimeZone;
                                ActionList.Add(action);
                            }
                            else
                            {
                                ActionList.Add(action);
                            }                            
                        }
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                     {"NFT_Actions", ActionList},
                     {"NFT_Modify_Date", con.ConvertTimeZone(NSMD.NFT_TimeZone, Convert.ToDateTime(NSMD.NFT_Modify_Date))},
                 };

                DocumentReference docRef = Db.Collection("MT_Notifications").Document(NSMD.NFT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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

        [Route("API/Notifications/DeleteAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAction(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true).WhereEqualTo("NFT_Unique_ID", NSMD.NFT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Notifications noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Notifications>();
                    if (noti.NFT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.NFT_Actions)
                        {
                            if (action.NFA_Unique_ID != NSMD.NFT_Actions[0].NFA_Unique_ID)
                            {
                                ActionList.Add(action);
                            }
                        }
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                     {"NFT_Actions", ActionList},                     
                 };

                DocumentReference docRef = Db.Collection("MT_Notifications").Document(NSMD.NFT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NSMD;
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


        [Route("API/Notifications/GetNotiListFilterWithPO")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetNotiListFilterWithPO(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {
                List<MT_Notifications> NotiList = new List<MT_Notifications>();
                Query ObjQuery = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true).WhereEqualTo("NFT_Surgery_Physician_Id", NSMD.NFT_Surgery_Physician_Id).OrderByDescending("NFT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiList.Add(Docsnap.ConvertTo<MT_Notifications>());
                    }
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.DataList = NotiList;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Notifications/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Notifications NSMD)
        {
            Db = con.SurgeryCenterDb(NSMD.Slug);
            NotificationsResponse Response = new NotificationsResponse();
            try
            {
                MT_Notifications notification = new MT_Notifications();
                Query ObjQuery = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true).WhereEqualTo("NFT_Unique_ID", NSMD.NFT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    notification = ObjQuerySnap.Documents[0].ConvertTo<MT_Notifications>();
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = notification;
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
