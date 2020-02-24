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
    public class PatiIntakeTypeController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public PatiIntakeTypeController()
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

        [Route("API/PatiIntakeType/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Patient_Intake_Type PITMD)
        {
            Db = con.SurgeryCenterDb(PITMD.Slug);
            PatiIntakeTypeResponse Response = new PatiIntakeTypeResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PITMD.PITT_Unique_ID = UniqueID;
                Query ObjQuery = Db.Collection("MT_Patient_Intake_Type");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PITMD.PITT_Category_Type_Code = "NTT000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    PITMD.PITT_Category_Type_Code = "NTT000" + "1";
                }
                PITMD.PITT_Create_Date = con.ConvertTimeZone(PITMD.PITT_TimeZone, Convert.ToDateTime(PITMD.PITT_Create_Date));
                PITMD.PITT_Modify_Date = con.ConvertTimeZone(PITMD.PITT_TimeZone, Convert.ToDateTime(PITMD.PITT_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Patient_Intake_Type").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PITMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PITMD;
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


        [Route("API/PatiIntakeType/GetNotiTypeListFilterWithCatID")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetNotiTypeListFilterWithCatID(MT_Patient_Intake_Type PITMD)
        {
            Db = con.SurgeryCenterDb(PITMD.Slug);
            PatiIntakeTypeResponse Response = new PatiIntakeTypeResponse();
            try
            {
                List<MT_Patient_Intake_Type> NotiCateList = new List<MT_Patient_Intake_Type>();
                Query ObjQuery = Db.Collection("MT_Patient_Intake_Type").WhereEqualTo("PITT_Is_Deleted", false).WhereEqualTo("PITT_Is_Active", true).WhereEqualTo("PITT_Category_ID", PITMD.PITT_Category_ID).OrderBy("PITT_Category_Name");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        NotiCateList.Add(Docsnap.ConvertTo<MT_Patient_Intake_Type>());
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
