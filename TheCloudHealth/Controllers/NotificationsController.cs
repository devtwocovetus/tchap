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
                if (NSMD.NFT_Actions != null)
                {
                    foreach (Notification_Action Notia in NSMD.NFT_Actions)
                    {
                        Notia.NFA_Unique_ID = con.GetUniqueKey();
                        Notia.NFA_Create_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Create_Date));
                        Notia.NFA_Modify_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Modify_Date));
                        //if (Notia.NFA_Be_Af == 1)
                        //{
                        //    Notia.NFA_Action_Title = Notia.NFA_Action_Type.ToUpper() + Notia.NFA_Days.ToString() + "Days Before";
                        //    Notia.NFA_Create_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Create_Date));
                        //    Notia.NFA_Modify_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Modify_Date));
                        //}
                        //else if (Notia.NFA_Be_Af == 2)
                        //{
                        //    Notia.NFA_Action_Title = Notia.NFA_Action_Type.ToUpper() + Notia.NFA_Days.ToString() + "Days After";
                        //    Notia.NFA_Create_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Create_Date));
                        //    Notia.NFA_Modify_Date = con.ConvertTimeZone(Notia.NFA_TimeZone, Convert.ToDateTime(Notia.NFA_Modify_Date));
                        //}

                        ActionList.Add(Notia);
                    }
                }
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
    }
}
