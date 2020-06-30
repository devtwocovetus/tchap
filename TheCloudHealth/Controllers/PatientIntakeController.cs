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
    public class PatientIntakeController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public PatientIntakeController()
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

        [Route("API/PatientIntake/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {
                List<Notification_Action> ActionList = new List<Notification_Action>();
                UniqueID = con.GetUniqueKey();
                PIMD.PIT_Unique_ID = UniqueID;
                PIMD.PIT_Create_Date = con.ConvertTimeZone(PIMD.PIT_TimeZone, Convert.ToDateTime(PIMD.PIT_Create_Date));
                PIMD.PIT_Modify_Date = con.ConvertTimeZone(PIMD.PIT_TimeZone, Convert.ToDateTime(PIMD.PIT_Modify_Date));
                PIMD.PIT_Actions = ActionList;
                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PIMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntake/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"PIT_Category_ID", PIMD.PIT_Category_ID},
                    {"PIT_Category_Name", PIMD.PIT_Category_Name},
                    {"PIT_Category_Type_ID", PIMD.PIT_Category_Type_ID},
                    {"PIT_Category_Type_Name", PIMD.PIT_Category_Type_Name},
                    {"PIT_Name",PIMD.PIT_Name},
                    {"PIT_Description",PIMD.PIT_Description},
                    {"PIT_Modify_Date",con.ConvertTimeZone(PIMD.PIT_TimeZone,Convert.ToDateTime(PIMD.PIT_Modify_Date))},
                    {"PIT_TimeZone", PIMD.PIT_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(PIMD.PIT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntake/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"PIT_Is_Active",PIMD.PIT_Is_Active},
                    {"PIT_Modify_Date",con.ConvertTimeZone(PIMD.PIT_TimeZone,Convert.ToDateTime(PIMD.PIT_Modify_Date))},
                    {"PIT_TimeZone", PIMD.PIT_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(PIMD.PIT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntake/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                    {"PIT_Is_Deleted",PIMD.PIT_Is_Deleted},
                    {"PIT_Modify_Date",con.ConvertTimeZone(PIMD.PIT_TimeZone,Convert.ToDateTime(PIMD.PIT_Modify_Date))},
                    {"PIT_TimeZone", PIMD.PIT_TimeZone}
                 };

                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(PIMD.PIT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntake/SelectAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> SelectAction(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).WhereEqualTo("PIT_Unique_ID", PIMD.PIT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Patient_Intake noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Intake>();
                    if (noti.PIT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.PIT_Actions)
                        {
                            if (action.NFA_Unique_ID == PIMD.PIT_Actions[0].NFA_Unique_ID)
                            {
                                ActionList.Add(action);
                            }

                        }
                    }
                    noti.PIT_Actions = ActionList;
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

        [Route("API/PatientIntake/AddAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddAction(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).WhereEqualTo("PIT_Unique_ID", PIMD.PIT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Patient_Intake noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Intake>();
                    if (noti.PIT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.PIT_Actions)
                        {
                            ActionList.Add(action);
                        }
                    }
                }

                if (PIMD.PIT_Actions != null)
                {
                    foreach (Notification_Action Notia in PIMD.PIT_Actions)
                    {
                        Notia.NFA_Unique_ID = con.GetUniqueKey();
                        Notia.NFA_Create_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Create_Date));
                        Notia.NFA_Modify_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Modify_Date));
                        ActionList.Add(Notia);
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                     {"PIT_Actions", ActionList},
                     {"PIT_Modify_Date", con.ConvertTimeZone(PIMD.PIT_TimeZone, Convert.ToDateTime(PIMD.PIT_Modify_Date))},
                 };

                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(PIMD.PIT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntake/EditAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditAction(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).WhereEqualTo("PIT_Unique_ID", PIMD.PIT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Patient_Intake noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Intake>();
                    if (noti.PIT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.PIT_Actions)
                        {
                            if (action.NFA_Unique_ID == PIMD.PIT_Actions[0].NFA_Unique_ID)
                            {
                                action.NFA_Action_Type = PIMD.PIT_Actions[0].NFA_Action_Type;
                                action.NFA_Action_Title = PIMD.PIT_Actions[0].NFA_Action_Title;
                                action.NFA_Action_Icon = PIMD.PIT_Actions[0].NFA_Action_Icon;
                                action.NFA_Action_Subject = PIMD.PIT_Actions[0].NFA_Action_Subject;
                                action.NFA_Be_Af = PIMD.PIT_Actions[0].NFA_Be_Af;
                                action.NFA_Days = PIMD.PIT_Actions[0].NFA_Days;
                                action.NFA_DayOrWeek = PIMD.PIT_Actions[0].NFA_DayOrWeek;
                                action.NFA_Message = PIMD.PIT_Actions[0].NFA_Message;
                                action.NFA_Modify_Date = con.ConvertTimeZone(PIMD.PIT_Actions[0].NFA_TimeZone, Convert.ToDateTime(PIMD.PIT_Actions[0].NFA_Modify_Date));
                                action.NFA_TimeZone = PIMD.PIT_Actions[0].NFA_TimeZone;
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
                     {"PIT_Actions", ActionList},
                     {"PIT_Modify_Date", con.ConvertTimeZone(PIMD.PIT_TimeZone, Convert.ToDateTime(PIMD.PIT_Modify_Date))},
                     {"PIT_TimeZone",PIMD.PIT_TimeZone }
                 };

                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(PIMD.PIT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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

        [Route("API/PatientIntake/DeleteAction")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAction(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {

                List<Notification_Action> ActionList = new List<Notification_Action>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).WhereEqualTo("PIT_Unique_ID", PIMD.PIT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Patient_Intake noti = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Intake>();
                    if (noti.PIT_Actions != null)
                    {
                        foreach (Notification_Action action in noti.PIT_Actions)
                        {
                            if (action.NFA_Unique_ID != PIMD.PIT_Actions[0].NFA_Unique_ID)
                            {
                                ActionList.Add(action);
                            }
                        }
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                 {
                     {"PIT_Actions", ActionList}
                 };

                DocumentReference docRef = Db.Collection("MT_Patient_Intake").Document(PIMD.PIT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PIMD;
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


        [Route("API/PatientIntake/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {
                List<MT_Patient_Intake> NotiList = new List<MT_Patient_Intake>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).OrderByDescending("PIT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiList.Add(Docsnap.ConvertTo<MT_Patient_Intake>());
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

        [Route("API/PatientIntake/GetPatiIntakeListFilterWithPO")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetPatiIntakeListFilterWithPO(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {
                List<MT_Patient_Intake> NotiList = new List<MT_Patient_Intake>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).WhereEqualTo("PIT_Surgery_Physician_Id", PIMD.PIT_Surgery_Physician_Id).OrderByDescending("PIT_Create_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiList.Add(Docsnap.ConvertTo<MT_Patient_Intake>());
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

        [Route("API/PatientIntake/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Patient_Intake PIMD)
        {
            Db = con.SurgeryCenterDb(PIMD.Slug);
            PatientIntakeResponse Response = new PatientIntakeResponse();
            try
            {
                MT_Patient_Intake notification = new MT_Patient_Intake();
                Query ObjQuery = Db.Collection("MT_Patient_Intake").WhereEqualTo("PIT_Is_Deleted", false).WhereEqualTo("PIT_Is_Active", true).WhereEqualTo("PIT_Unique_ID", PIMD.PIT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    notification = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Intake>();
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
