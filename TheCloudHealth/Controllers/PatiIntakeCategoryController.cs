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
    public class PatiIntakeCategoryController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public PatiIntakeCategoryController()
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

        [Route("API/PatiIntakeCategory/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Patient_Intake_Category PICMD)
        {
            Db = con.SurgeryCenterDb(PICMD.Slug);
            PatiIntakeCateResponse Response = new PatiIntakeCateResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PICMD.PITC_Unique_ID = UniqueID;
                Query ObjQuery = Db.Collection("MT_Patient_Intake_Category");//.WhereEqualTo("PB_Surgery_Physician_Center_ID", PMD.PB_Surgery_Physician_Center_ID).WhereEqualTo("PB_Office_Type", PMD.PB_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PICMD.PITC_Category_Code = "PITC000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    PICMD.PITC_Category_Code = "PITC000" + "1";
                }
                PICMD.PITC_Create_Date = con.ConvertTimeZone(PICMD.PITC_TimeZone, Convert.ToDateTime(PICMD.PITC_Create_Date));
                PICMD.PITC_Modify_Date = con.ConvertTimeZone(PICMD.PITC_TimeZone, Convert.ToDateTime(PICMD.PITC_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Patient_Intake_Category").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PICMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PICMD;
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


        [Route("API/PatiIntakeCategory/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Patient_Intake_Category PICMD)
        {
            Db = con.SurgeryCenterDb(PICMD.Slug);
            PatiIntakeCateResponse Response = new PatiIntakeCateResponse();
            try
            {
                List<MT_Patient_Intake_Category> NotiCateList = new List<MT_Patient_Intake_Category>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake_Category").WhereEqualTo("PITC_Is_Deleted", false).WhereEqualTo("PITC_Is_Active", true).OrderBy("PITC_Category_Name");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiCateList.Add(Docsnap.ConvertTo<MT_Patient_Intake_Category>());
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
