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
    public class SPTemplatesController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";

        public SPTemplatesController()
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

        [Route("API/SPTemplates/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SPMD.SPT_Unique_ID = UniqueID;
                SPMD.SPT_Surgery_Date = con.ConvertTimeZone(SPMD.SPT_TimeZone, Convert.ToDateTime(SPMD.SPT_Surgery_Date));
                SPMD.SPT_Create_Date = con.ConvertTimeZone(SPMD.SPT_TimeZone, Convert.ToDateTime(SPMD.SPT_Create_Date));
                SPMD.SPT_Modify_Date = con.ConvertTimeZone(SPMD.SPT_TimeZone, Convert.ToDateTime(SPMD.SPT_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Surgical_Procedure_Templates").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SPMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPMD;
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

        [Route("API/SPTemplates/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"SPT_Template_Name" , SPMD.SPT_Template_Name},
                    {"SPT_Surgical_Center_Name" , SPMD.SPT_Surgical_Center_Name},
                    {"SPT_Surgeon_Name" , SPMD.SPT_Surgeon_Name},
                    {"SPT_Co_Surgeon_Name" , SPMD.SPT_Co_Surgeon_Name},
                    {"SPT_Surgery_Date" , SPMD.SPT_Surgery_Date},
                    {"SPT_Surgery_Time" , SPMD.SPT_Surgery_Time},
                    {"SPT_Surgery_Duration" , SPMD.SPT_Surgery_Duration},
                    {"SPT_Anesthesia_Type" , SPMD.SPT_Anesthesia_Type},
                    {"SPT_Block" , SPMD.SPT_Block},
                    {"SPT_Procedure_SelectedList" , SPMD.SPT_Procedure_SelectedList},
                    {"SPT_CPT_SelectedList" , SPMD.SPT_CPT_SelectedList},
                    {"SPT_ICD_SelectedList" , SPMD.SPT_ICD_SelectedList},
                    {"SPT_Surgery_Physician_Center_ID" , SPMD.SPT_Surgery_Physician_Center_ID},
                    {"SPT_Surgery_Physician_Center_Name" , SPMD.SPT_Surgery_Physician_Center_Name},
                    {"SPI_Created_By" , SPMD.SPI_Created_By},
                    {"SPI_User_Name" , SPMD.SPI_User_Name},
                    {"SPT_Modify_Date" , con.ConvertTimeZone(SPMD.SPT_TimeZone, Convert.ToDateTime(SPMD.SPT_Modify_Date))},
                    {"SPT_TimeZone" , SPMD.SPT_TimeZone}
                };

                DocumentReference docRef = Db.Collection("MT_Surgical_Procedure_Templates").Document(SPMD.SPT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPMD;
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

        [Route("API/SPTemplates/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"SPT_Is_Active" , SPMD.SPT_Is_Active},
                    {"SPT_Modify_Date" , con.ConvertTimeZone(SPMD.SPT_TimeZone, Convert.ToDateTime(SPMD.SPT_Modify_Date))},
                    {"SPT_TimeZone" , SPMD.SPT_TimeZone}
                };

                DocumentReference docRef = Db.Collection("MT_Surgical_Procedure_Templates").Document(SPMD.SPT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPMD;
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

        [Route("API/SPTemplates/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"SPT_Is_Deleted" , SPMD.SPT_Is_Deleted},
                    {"SPT_Modify_Date" , con.ConvertTimeZone(SPMD.SPT_TimeZone, Convert.ToDateTime(SPMD.SPT_Modify_Date))},
                    {"SPT_TimeZone" , SPMD.SPT_TimeZone}
                };

                DocumentReference docRef = Db.Collection("MT_Surgical_Procedure_Templates").Document(SPMD.SPT_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPMD;
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

        [Route("API/SPTemplates/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Surgical_Procedure_Templates").Document(SPMD.SPT_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SPMD;
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

        [Route("API/SPTemplates/GetTemplateFilterWithSurgeon")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetTemplateFilterWithSurgeon(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                List<MT_Surgical_Procedure_Templates> TempList = new List<MT_Surgical_Procedure_Templates>();
                Query ObjQuery = Db.Collection("MT_Surgical_Procedure_Templates").WhereEqualTo("SPT_Is_Active", true).WhereEqualTo("SPT_Is_Deleted", false).WhereEqualTo("SPT_Surgeon_Name", SPMD.SPT_Surgeon_Name).WhereEqualTo("SPT_Surgery_Physician_Center_ID", SPMD.SPT_Surgery_Physician_Center_ID).OrderBy("SPT_Template_Name");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        TempList.Add(Docsnap.ConvertTo<MT_Surgical_Procedure_Templates>());
                    }
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.DataList = TempList;
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

        [Route("API/SPTemplates/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                Query ObjQuery = Db.Collection("MT_Surgical_Procedure_Templates").WhereEqualTo("SPT_Is_Active", true).WhereEqualTo("SPT_Is_Deleted", false).WhereEqualTo("SPT_Unique_ID", SPMD.SPT_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_Surgical_Procedure_Templates>();
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

        [Route("API/SPTemplates/IsExist")]
        [HttpPost]
        public async Task<HttpResponseMessage> AlreadyExist(MT_Surgical_Procedure_Templates SPMD)
        {
            Db = con.SurgeryCenterDb(SPMD.Slug);
            TemplateResponse Response = new TemplateResponse();
            try
            {
                Query ObjQuery = Db.Collection("MT_Surgical_Procedure_Templates").WhereEqualTo("SPT_Is_Active", true).WhereEqualTo("SPT_Is_Deleted", false).WhereEqualTo("SPT_Template_Name", SPMD.SPT_Template_Name);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    if (ObjQuerySnap.Documents.Count == 0)
                    {
                        Response.Exist = true;
                    }
                    else
                    {
                        Response.Exist = false;
                    }
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
