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
using System.Linq;

namespace TheCloudHealth.Controllers
{
    public class StaffMController : ApiController
    {
        FirestoreDb Db;
        FirestoreDb DbLog;

        ConnectionClass con;
        ICryptoEngine ObjectCrypto;
        string UniqueID = "";
        public StaffMController()
        {
            con = new ConnectionClass();
            //Db = con.Db();
            //DbLog = con.DbLog();
            ObjectCrypto = new CryptoEngineData();
        }
        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/StaffM/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                SMD.Staff_SSN_No = ObjectCrypto.Encrypt(ObjectCrypto.Encrypt(SMD.Staff_SSN_No, "sblw-3hn8-sqoy19"), "sblw-3hn8-sqoy19");
                SMD.Staff_Unique_ID = UniqueID;
                SMD.Staff_Create_Date = con.ConvertTimeZone(SMD.Staff_TimeZone, Convert.ToDateTime(SMD.Staff_Create_Date));
                SMD.Staff_Modify_Date = con.ConvertTimeZone(SMD.Staff_TimeZone, Convert.ToDateTime(SMD.Staff_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Staff_Members").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/StaffM/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Staff_Name",SMD.Staff_Name},
                    {"Staff_Last_Name",SMD.Staff_Last_Name},
                    {"Staff_Email",SMD.Staff_Email},
                    {"Staff_PhoneNo",SMD.Staff_PhoneNo},
                    {"Staff_AlternateNo",SMD.Staff_AlternateNo},
                    {"Staff_Emergency_ContactNo",SMD.Staff_Emergency_ContactNo},
                    {"Staff_Address1",SMD.Staff_Address1},
                    {"Staff_Address1",SMD.Staff_Address2},
                    {"Staff_City",SMD.Staff_City},
                    {"Staff_State",SMD.Staff_State},
                    {"Staff_Country",SMD.Staff_Country},
                    {"Staff_ZipCode",SMD.Staff_ZipCode},
                    {"Staff_Age",SMD.Staff_Age},
                    {"Staff_Sex",SMD.Staff_Sex},
                    {"Staff_DOB",SMD.Staff_DOB},
                    {"Staff_SSN_No",ObjectCrypto.Encrypt(ObjectCrypto.Encrypt(SMD.Staff_SSN_No, "sblw-3hn8-sqoy19"), "sblw-3hn8-sqoy19")},
                    {"Staff_DOJ",SMD.Staff_DOJ},
                    {"Staff_Emp_Code",SMD.Staff_Emp_Code},
                    {"Staff_Doc_Code",SMD.Staff_Doc_Code},
                    {"Staff_Designation",SMD.Staff_Designation},
                    {"Staff_Role_ID",SMD.Staff_Role_ID},
                    {"Staff_Role_Name",SMD.Staff_Role_Name},
                    {"Staff_Modify_Date",con.ConvertTimeZone(SMD.Staff_TimeZone, Convert.ToDateTime(SMD.Staff_Modify_Date))},
                    {"Staff_Is_Active",SMD.Staff_Is_Active},
                    {"Staff_Is_Deleted",SMD.Staff_Is_Deleted}

                };

                DocumentReference docRef = Db.Collection("MT_Staff_Members").Document(SMD.Staff_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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


        [Route("API/StaffM/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Staff_Modify_Date",con.ConvertTimeZone(SMD.Staff_TimeZone, Convert.ToDateTime(SMD.Staff_Modify_Date))},
                    {"Staff_Is_Active",SMD.Staff_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Staff_Members").Document(SMD.Staff_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/StaffM/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Staff_Modify_Date",con.ConvertTimeZone(SMD.Staff_TimeZone, Convert.ToDateTime(SMD.Staff_Modify_Date))},
                    {"Staff_Is_Deleted",SMD.Staff_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Staff_Members").Document(SMD.Staff_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SMD;
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

        [Route("API/StaffM/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Staff_Members").Document(SMD.Staff_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = null;
                }
                else
                {
                    Response.Status = con.StatusNotDeleted;
                    Response.Message = con.MessageNotDeleted;
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
        [Route("API/StaffM/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                MT_Staff_Members staff = new MT_Staff_Members();
                Query docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Unique_ID", SMD.Staff_Unique_ID).WhereEqualTo("Staff_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_Staff_Members>();
                }
                else
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
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

        [Route("API/StaffM/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                List<MT_Staff_Members> AnesList = new List<MT_Staff_Members>();
                Query docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Staff_Members>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Staff_Name).ThenBy(o => o.Staff_Last_Name).ToList();
                }
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/StaffM/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                List<MT_Staff_Members> AnesList = new List<MT_Staff_Members>();
                Query docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Deleted", false).WhereEqualTo("Staff_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Staff_Members>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Staff_Name).ThenBy(o => o.Staff_Last_Name).ToList();
                }
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/StaffM/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffMResponse Response = new StaffMResponse();
            try
            {
                List<MT_Staff_Members> AnesList = new List<MT_Staff_Members>();
                Query docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Deleted", true).WhereEqualTo("Staff_Surgery_Physician_Office_ID", SMD.Staff_Surgery_Physician_Office_ID).WhereEqualTo("Staff_Office_Type", SMD.Staff_Office_Type);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Staff_Members>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Staff_Name).ThenBy(o => o.Staff_Last_Name).ToList();
                }
                Response.Status = con.StatusSuccess;
                Response.Message = con.MessageSuccess;
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/StaffM/CheckCodeExist")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> CheckCodeExist(MT_Staff_Members SMD)
        {
            Db = con.SurgeryCenterDb(SMD.Slug);
            StaffAResponse Response = new StaffAResponse();
            try
            {
                List<MT_Staff_Members> AnesList = new List<MT_Staff_Members>();
                Query docRef;
                if (SMD.Staff_Is_Emp_Code == true)
                {
                    docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Deleted", false).WhereEqualTo("Staff_Emp_Code", SMD.Staff_Emp_Code);
                }
                else
                {
                    docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Deleted", false).WhereEqualTo("Staff_Doc_Code", SMD.Staff_Doc_Code);
                }

                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap.Count == 0)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Is_Available = true;
                }
                else
                {
                    Response.Status = con.StatusAE;
                    Response.Message = con.MessageAE;
                    Response.Is_Available = false;
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
