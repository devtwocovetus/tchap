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
    public class NotiCategoryController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public NotiCategoryController()
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

        [Route("API/NotiCategory/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Notification_Category NCMD)
        {
            Db = con.SurgeryCenterDb(NCMD.Slug);
            NotiCateResponse Response = new NotiCateResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                NCMD.NC_Unique_ID = UniqueID;
                Query ObjQuery = Db.Collection("MT_Notification_Category");//.WhereEqualTo("PB_Surgery_Physician_Center_ID", PMD.PB_Surgery_Physician_Center_ID).WhereEqualTo("PB_Office_Type", PMD.PB_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    NCMD.NC_Category_Code = "NTC000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    NCMD.NC_Category_Code = "NTC000" + "1";
                }
                NCMD.NC_Create_Date = con.ConvertTimeZone(NCMD.NC_TimeZone, Convert.ToDateTime(NCMD.NC_Create_Date));
                NCMD.NC_Modify_Date = con.ConvertTimeZone(NCMD.NC_TimeZone, Convert.ToDateTime(NCMD.NC_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Notification_Category").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(NCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = NCMD;
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


        [Route("API/NotiCategory/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Notification_Category NCMD)
        {
            Db = con.SurgeryCenterDb(NCMD.Slug);
            NotiCateResponse Response = new NotiCateResponse();
            try
            {
                List<MT_Notification_Category> NotiCateList = new List<MT_Notification_Category>();
                Query ObjQuery = Db.Collection("MT_Notification_Category").WhereEqualTo("NC_Is_Deleted", false).WhereEqualTo("NC_Is_Active", true).OrderBy("NC_Category_Name");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiCateList.Add(Docsnap.ConvertTo<MT_Notification_Category>());
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
