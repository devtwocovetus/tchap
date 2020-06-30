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

namespace TheCloudHealth.Controllers
{
    public class PageMasterController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public PageMasterController()
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

        [Route("API/PageMaster/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Page_Master PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PageMasterResponse Response = new PageMasterResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PMD.PM_Unique_ID = UniqueID;
                DocumentReference docRef = Db.Collection("MT_Page_Master").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PMD;
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

        [Route("API/PageMaster/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Page_Master PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PageMasterResponse Response = new PageMasterResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"PM_Is_View", PMD.Is_View},
                    {"PM_Is_Add", PMD.Is_Add},
                    {"PM_Is_Edit", PMD.Is_Edit},
                    {"PM_Is_Deleted", PMD.Is_Delete},
                };
                DocumentReference docRef = Db.Collection("MT_Page_Master").Document(PMD.PM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PMD;
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

        [Route("API/PageMaster/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Page_Master PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PageMasterResponse Response = new PageMasterResponse();
            try
            {
                List<MT_Page_Master> PMList = new List<MT_Page_Master>();
                Query ObjQuery = Db.Collection("MT_Page_Master").WhereEqualTo("Category_Name", PMD.Category_Name);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach(DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        PMList.Add(Docsnap.ConvertTo<MT_Page_Master>());
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
