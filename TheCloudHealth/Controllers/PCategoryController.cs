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
    public class PCategoryController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public PCategoryController()
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

        [Route("API/PCategory/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Category_Master CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CategoryResponse Response = new CategoryResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                CMD.CM_Unique_ID = UniqueID;
                if (CMD.CM_Detail != null)
                {
                    foreach (MT_Category_Detail detail in CMD.CM_Detail)
                    {
                        detail.CD_Unique_ID = con.GetUniqueKey();
                    }
                }
                DocumentReference docRef = Db.Collection("MT_Category_Master").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(CMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = CMD;
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

        [Route("API/PCategory/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Category_Master CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CategoryResponse Response = new CategoryResponse();
            try
            {
                MT_Category_Master CMaster = new MT_Category_Master();
                List<MT_Category_Detail> CDetails = new List<MT_Category_Detail>();
                Query ObjQuery = Db.Collection("MT_Category_Master").WhereEqualTo("CM_UniqueID", CMD.CM_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    CMaster = ObjQuerySnap.Documents[0].ConvertTo<MT_Category_Master>();
                    if (CMaster.CM_Detail != null)
                    {
                        foreach (MT_Category_Detail detail in CMaster.CM_Detail)
                        {
                            if (detail.CD_Unique_ID == CMD.CM_Detail[0].CD_Unique_ID)
                            {
                                detail.CD_Is_Assigned = CMD.CM_Detail[0].CD_Is_Assigned;
                                CDetails.Add(detail);
                            }
                            else
                            {
                                CDetails.Add(detail);
                            }
                        }
                    }
                }
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"CM_Detail", CDetails}
                };
                DocumentReference docRef = Db.Collection("MT_Category_Master").Document(CMD.CM_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = CMD;
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

        [Route("API/PCategory/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Category_Master CMD)
        {
            Db = con.SurgeryCenterDb(CMD.Slug);
            CategoryResponse Response = new CategoryResponse();
            try
            {

                MT_Category_Master CMMaster = new MT_Category_Master();
                List<MT_Category_Detail> DList = new List<MT_Category_Detail>();
                Query ObjQuery = Db.Collection("MT_Category_Master").WhereEqualTo("CM_Login_From", CMD.CM_Login_From);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    CMMaster = ObjQuerySnap.Documents[0].ConvertTo<MT_Category_Master>();
                    if (CMMaster != null)
                    {
                        foreach (MT_Category_Detail detail in CMMaster.CM_Detail)
                        {
                            if (detail.CD_Is_Assigned == true)
                            {
                                DList.Add(detail);
                            }
                        }
                        CMMaster.CM_Detail = DList;
                    }

                    Response.Data = CMMaster;
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
