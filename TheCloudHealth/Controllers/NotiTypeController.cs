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
    public class NotiTypeController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public NotiTypeController()
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

        [Route("API/NotiType/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Notification_Type NTMD)
        {
            Db = con.SurgeryCenterDb(NTMD.Slug);
            NotiTypeResponse Response = new NotiTypeResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                NTMD.NT_Unique_ID = UniqueID;
                Query ObjQuery = Db.Collection("MT_Notification_Type");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    NTMD.NT_Category_Type_Code = "NTT000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    NTMD.NT_Category_Type_Code = "NTT000" + "1";
                }
                NTMD.NT_Create_Date = con.ConvertTimeZone(NTMD.NT_TimeZone, Convert.ToDateTime(NTMD.NT_Create_Date));
                NTMD.NT_Modify_Date = con.ConvertTimeZone(NTMD.NT_TimeZone, Convert.ToDateTime(NTMD.NT_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Notification_Type").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(NTMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NTMD;
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


        [Route("API/NotiType/GetNotiTypeListFilterWithCatID")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetNotiTypeListFilterWithCatID(MT_Notification_Type NTMD)
        {
            Db = con.SurgeryCenterDb(NTMD.Slug);
            NotiTypeResponse Response = new NotiTypeResponse();
            try
            {
                List<MT_Notification_Type> NotiCateList = new List<MT_Notification_Type>();
                Query ObjQuery = Db.Collection("MT_Notification_Type").WhereEqualTo("NT_Is_Deleted", false).WhereEqualTo("NT_Is_Active", true).WhereEqualTo("NT_Category_ID", NTMD.NT_Category_ID).OrderBy("NT_Category_Name");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiCateList.Add(Docsnap.ConvertTo<MT_Notification_Type>());
                    }
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.DataList = NotiCateList;
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
