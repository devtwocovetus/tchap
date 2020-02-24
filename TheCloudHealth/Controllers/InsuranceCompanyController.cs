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
    public class InsuranceCompanyController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public InsuranceCompanyController()
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

        [Route("API/InsuranceCompany/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Insurance_Company ICMD)
        {
            Db = con.SurgeryCenterDb(ICMD.Slug);
            InsurCompanyResponse Response = new InsurCompanyResponse();
            try
            {
                List<MT_Insurance_Company> SpelitiesList = new List<MT_Insurance_Company>();
                UniqueID = con.GetUniqueKey();
                ICMD.INC_Unique_ID = UniqueID;
                ICMD.INC_Create_Date = con.ConvertTimeZone(ICMD.INC_TimeZone, Convert.ToDateTime(ICMD.INC_Create_Date));
                ICMD.INC_Modify_Date = con.ConvertTimeZone(ICMD.INC_TimeZone, Convert.ToDateTime(ICMD.INC_Modify_Date));

                DocumentReference docRef = Db.Collection("MT_Insurance_Company").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(ICMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = ICMD;
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


        [Route("API/InsuranceCompany/GetInsurCompanyFilterWithName")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetInsurCompanyFilterWithName(MT_Insurance_Company ICMD)
        {
            Db = con.SurgeryCenterDb(ICMD.Slug);
            InsurCompanyResponse Response = new InsurCompanyResponse();
            try
            {
                List<MT_Insurance_Company> InsuranceList = new List<MT_Insurance_Company>();
                Query docRef = Db.Collection("MT_Insurance_Company").WhereEqualTo("INC_Is_Deleted", false).OrderBy("INC_Company_Name").StartAt(ICMD.INC_Company_Name.ToUpper()).EndAt(ICMD.INC_Company_Name.ToUpper() + '\uf8ff');
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        InsuranceList.Add(Docsnapshot.ConvertTo<MT_Insurance_Company>());
                    }
                    Response.DataList = InsuranceList;
                    Response.Message = con.MessageSuccess;
                    Response.Status = con.StatusSuccess;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/InsuranceCompany/Count")]
        [HttpPost]
        public async Task<HttpResponseMessage> Count(MT_Insurance_Company ICMD)
        {
            Db = con.SurgeryCenterDb(ICMD.Slug);
            InsurCompanyResponse Response = new InsurCompanyResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                Query docRef = Db.Collection("MT_Insurance_Company").WhereEqualTo("INC_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Count = ObjQuerySnap.Documents.Count;
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
