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
using System.Net.Http.Headers;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace TheCloudHealth.Controllers
{
    public class PatientController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public PatientController()
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

        [Route("API/Patient/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PMD.Patient_Unique_ID = UniqueID;
                PMD.Patient_First_Name = PMD.Patient_First_Name.ToUpper();
                PMD.Patient_Middle_Name = PMD.Patient_Middle_Name.ToUpper();
                PMD.Patient_Last_Name = PMD.Patient_Last_Name.ToUpper();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).WhereEqualTo("Patient_Office_Type", PMD.Patient_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PMD.Patient_Code = PMD.Patient_User_Name.Substring(0, 3).ToUpper() + "000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    PMD.Patient_Code = PMD.Patient_User_Name.Substring(0, 3).ToUpper() + "000" + "1";
                }
                PMD.Patient_DOB = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_DOB),1);
                PMD.Patient_Create_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Create_Date));
                PMD.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(UniqueID);
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

        [Route("API/Patient/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Patient_Prefix" ,PMD.Patient_Prefix},
                    {"Patient_First_Name" ,PMD.Patient_First_Name.ToUpper()},
                    {"Patient_Middle_Name" ,PMD.Patient_Middle_Name.ToUpper()},
                    {"Patient_Last_Name" ,PMD.Patient_Last_Name.ToUpper()},
                    {"Patient_DOB" ,con.ConvertTimeZone(PMD.Patient_TimeZone,Convert.ToDateTime(PMD.Patient_DOB),1)},
                    {"Patient_Sex" ,PMD.Patient_Sex},
                    {"Patient_SSN" ,PMD.Patient_SSN},
                    {"Patient_Address1" ,PMD.Patient_Address1},
                    {"Patient_Address2" ,PMD.Patient_Address2},
                    {"Patient_City" ,PMD.Patient_City},
                    {"Patient_State" ,PMD.Patient_State},
                    {"Patient_Zipcode" ,PMD.Patient_Zipcode},
                    {"Patient_Primary_No" ,PMD.Patient_Primary_No},
                    {"Patient_Secondary_No" ,PMD.Patient_Secondary_No},
                    {"Patient_Work_No" ,PMD.Patient_Work_No},
                    {"Patient_Emergency_No" ,PMD.Patient_Emergency_No},
                    {"Patient_Email" ,PMD.Patient_Email},
                    {"Patient_Religion" ,PMD.Patient_Religion},
                    {"Patient_Ethinicity" ,PMD.Patient_Ethinicity},
                    {"Patient_Race" ,PMD.Patient_Race},
                    {"Patient_Marital_Status" ,PMD.Patient_Marital_Status},
                    {"Patient_Nationality" ,PMD.Patient_Nationality},
                    {"Patient_Language" ,PMD.Patient_Language},
                    {"Patient_Height_In_Ft" ,PMD.Patient_Height_In_Ft},
                    {"Patient_Height_In_Inch" ,PMD.Patient_Height_In_Inch},
                    {"Patient_Weight" ,PMD.Patient_Weight},
                    {"Patient_Body_Mass_Index" ,PMD.Patient_Body_Mass_Index},
                    {"Patient_Modify_Date" ,con.ConvertTimeZone(PMD.Patient_TimeZone,Convert.ToDateTime(PMD.Patient_Modify_Date))},
                    {"Patient_Is_Active" ,PMD.Patient_Is_Active}
                };
                DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(PMD.Patient_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
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

        [Route("API/Patient/AddPrimaryInsurance")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddPrimaryInsurance(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                Patient_Primary_Insurance_Details PPID = new Patient_Primary_Insurance_Details();
                MT_PatientInfomation patiinfo = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID).WhereEqualTo("Patient_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    patiinfo = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                    PPID.PPID_Unique_ID = con.GetUniqueKey();
                    PPID.PPID_Relation_To_Patient = PMD.Patient_Primary_Insurance_Details.PPID_Relation_To_Patient;
                    PPID.PPID_Subscriber_Name = PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_Name;
                    PPID.PPID_Subscriber_SSN_No = PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No;
                    PPID.PPID_DOB = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_DOB),1);
                    PPID.PPID_Policy_No = PMD.Patient_Primary_Insurance_Details.PPID_Policy_No;
                    PPID.PPID_Primary_Insurance = PMD.Patient_Primary_Insurance_Details.PPID_Primary_Insurance;
                    PPID.PPID_PO_Box_No = PMD.Patient_Primary_Insurance_Details.PPID_PO_Box_No;
                    PPID.PPID_Address = PMD.Patient_Primary_Insurance_Details.PPID_Address;
                    PPID.PPID_Address2 = PMD.Patient_Primary_Insurance_Details.PPID_Address2;
                    PPID.PPID_State = PMD.Patient_Primary_Insurance_Details.PPID_State;
                    PPID.PPID_City = PMD.Patient_Primary_Insurance_Details.PPID_City;
                    PPID.PPID_Zip_Code = PMD.Patient_Primary_Insurance_Details.PPID_Zip_Code;
                    PPID.PPID_Created_By = PMD.Patient_Primary_Insurance_Details.PPID_Created_By;
                    PPID.PPID_User_Name = PMD.Patient_Primary_Insurance_Details.PPID_User_Name;
                    PPID.PPID_Create_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Create_Date));
                    PPID.PPID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date));
                    PPID.PPID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date));
                    PPID.PPID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date));
                    PPID.PPID_Is_Active = PMD.Patient_Primary_Insurance_Details.PPID_Is_Active;
                    PPID.PPID_Is_Deleted = PMD.Patient_Primary_Insurance_Details.PPID_Is_Deleted;
                    PPID.PPID_TimeZone = PMD.Patient_Primary_Insurance_Details.PPID_TimeZone;
                    patiinfo.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                    patiinfo.Patient_TimeZone = PMD.Patient_TimeZone;
                    patiinfo.Patient_Primary_Insurance_Details = PPID;

                    DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(patiinfo.Patient_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(patiinfo, SetOptions.Overwrite);
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

        [Route("API/Patient/AddFSecondaryInsurance")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddFSecondaryInsurance(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                Patient_Secondary1_Insurance_Details PS1ID = new Patient_Secondary1_Insurance_Details();
                MT_PatientInfomation patiinfo = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID).WhereEqualTo("Patient_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    patiinfo = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                    PS1ID.PSID_Unique_ID = con.GetUniqueKey();
                    PS1ID.PSID_Relation_To_Patient = PMD.Patient_Secondary1_Insurance_Details.PSID_Relation_To_Patient;
                    PS1ID.PSID_Subscriber_Name = PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_Name;
                    PS1ID.PSID_Subscriber_SSN_No = PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No;
                    PS1ID.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_DOB),1);
                    PS1ID.PSID_Policy_No = PMD.Patient_Secondary1_Insurance_Details.PSID_Policy_No;
                    PS1ID.PSID_Primary_Insurance = PMD.Patient_Secondary1_Insurance_Details.PSID_Primary_Insurance;
                    PS1ID.PSID_PO_Box_No = PMD.Patient_Secondary1_Insurance_Details.PSID_PO_Box_No;
                    PS1ID.PSID_Address = PMD.Patient_Secondary1_Insurance_Details.PSID_Address;
                    PS1ID.PSID_Address2 = PMD.Patient_Secondary1_Insurance_Details.PSID_Address2;
                    PS1ID.PSID_State = PMD.Patient_Secondary1_Insurance_Details.PSID_State;
                    PS1ID.PSID_City = PMD.Patient_Secondary1_Insurance_Details.PSID_City;
                    PS1ID.PSID_Zip_Code = PMD.Patient_Secondary1_Insurance_Details.PSID_Zip_Code;
                    PS1ID.PSID_Created_By = PMD.Patient_Secondary1_Insurance_Details.PSID_Created_By;
                    PS1ID.PSID_User_Name = PMD.Patient_Secondary1_Insurance_Details.PSID_User_Name;
                    PS1ID.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Create_Date));
                    PS1ID.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date));
                    PS1ID.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date));
                    PS1ID.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date));
                    PS1ID.PSID_Is_Active = PMD.Patient_Secondary1_Insurance_Details.PSID_Is_Active;
                    PS1ID.PSID_Is_Deleted = PMD.Patient_Secondary1_Insurance_Details.PSID_Is_Deleted;
                    PS1ID.PSID_TimeZone = PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone;
                    patiinfo.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                    patiinfo.Patient_TimeZone = PMD.Patient_TimeZone;
                    patiinfo.Patient_Secondary1_Insurance_Details = PS1ID;

                    DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(patiinfo.Patient_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(patiinfo, SetOptions.Overwrite);
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

        [Route("API/Patient/AddSSecondaryInsurance")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSSecondaryInsurance(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                Patient_Secondary2_Insurance_Details PS2ID = new Patient_Secondary2_Insurance_Details();
                MT_PatientInfomation patiinfo = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID).WhereEqualTo("Patient_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    patiinfo = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                    PS2ID.PSID_Unique_ID = con.GetUniqueKey();
                    PS2ID.PSID_Relation_To_Patient = PMD.Patient_Secondary2_Insurance_Details.PSID_Relation_To_Patient;
                    PS2ID.PSID_Subscriber_Name = PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_Name;
                    PS2ID.PSID_Subscriber_SSN_No = PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No;
                    PS2ID.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_DOB),1);
                    PS2ID.PSID_Policy_No = PMD.Patient_Secondary2_Insurance_Details.PSID_Policy_No;
                    PS2ID.PSID_Primary_Insurance = PMD.Patient_Secondary2_Insurance_Details.PSID_Primary_Insurance;
                    PS2ID.PSID_PO_Box_No = PMD.Patient_Secondary2_Insurance_Details.PSID_PO_Box_No;
                    PS2ID.PSID_Address = PMD.Patient_Secondary2_Insurance_Details.PSID_Address;
                    PS2ID.PSID_Address2 = PMD.Patient_Secondary2_Insurance_Details.PSID_Address2;
                    PS2ID.PSID_State = PMD.Patient_Secondary2_Insurance_Details.PSID_State;
                    PS2ID.PSID_City = PMD.Patient_Secondary2_Insurance_Details.PSID_City;
                    PS2ID.PSID_Zip_Code = PMD.Patient_Secondary2_Insurance_Details.PSID_Zip_Code;
                    PS2ID.PSID_Created_By = PMD.Patient_Secondary2_Insurance_Details.PSID_Created_By;
                    PS2ID.PSID_User_Name = PMD.Patient_Secondary2_Insurance_Details.PSID_User_Name;
                    PS2ID.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Create_Date));
                    PS2ID.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date));
                    PS2ID.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date));
                    PS2ID.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date));
                    PS2ID.PSID_Is_Active = PMD.Patient_Secondary2_Insurance_Details.PSID_Is_Active;
                    PS2ID.PSID_Is_Deleted = PMD.Patient_Secondary2_Insurance_Details.PSID_Is_Deleted;
                    PS2ID.PSID_TimeZone = PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone;
                    patiinfo.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                    patiinfo.Patient_TimeZone = PMD.Patient_TimeZone;
                    patiinfo.Patient_Secondary2_Insurance_Details = PS2ID;

                    DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(patiinfo.Patient_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(patiinfo, SetOptions.Overwrite);
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
        [Route("API/Patient/AddTSecondaryInsurance")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddTSecondaryInsurance(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                Patient_Secondary3_Insurance_Details PS3ID = new Patient_Secondary3_Insurance_Details();
                MT_PatientInfomation patiinfo = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID).WhereEqualTo("Patient_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    patiinfo = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                    PS3ID.PSID_Unique_ID = con.GetUniqueKey();
                    PS3ID.PSID_Relation_To_Patient = PMD.Patient_Secondary3_Insurance_Details.PSID_Relation_To_Patient;
                    PS3ID.PSID_Subscriber_Name = PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_Name;
                    PS3ID.PSID_Subscriber_SSN_No = PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No;
                    PS3ID.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_DOB),1);
                    PS3ID.PSID_Policy_No = PMD.Patient_Secondary3_Insurance_Details.PSID_Policy_No;
                    PS3ID.PSID_Primary_Insurance = PMD.Patient_Secondary3_Insurance_Details.PSID_Primary_Insurance;
                    PS3ID.PSID_PO_Box_No = PMD.Patient_Secondary3_Insurance_Details.PSID_PO_Box_No;
                    PS3ID.PSID_Address = PMD.Patient_Secondary3_Insurance_Details.PSID_Address;
                    PS3ID.PSID_Address2 = PMD.Patient_Secondary3_Insurance_Details.PSID_Address2;
                    PS3ID.PSID_State = PMD.Patient_Secondary3_Insurance_Details.PSID_State;
                    PS3ID.PSID_City = PMD.Patient_Secondary3_Insurance_Details.PSID_City;
                    PS3ID.PSID_Zip_Code = PMD.Patient_Secondary3_Insurance_Details.PSID_Zip_Code;
                    PS3ID.PSID_Created_By = PMD.Patient_Secondary3_Insurance_Details.PSID_Created_By;
                    PS3ID.PSID_User_Name = PMD.Patient_Secondary3_Insurance_Details.PSID_User_Name;
                    PS3ID.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Create_Date));
                    PS3ID.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date));
                    PS3ID.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date));
                    PS3ID.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date));
                    PS3ID.PSID_Is_Active = PMD.Patient_Secondary3_Insurance_Details.PSID_Is_Active;
                    PS3ID.PSID_Is_Deleted = PMD.Patient_Secondary3_Insurance_Details.PSID_Is_Deleted;
                    PS3ID.PSID_TimeZone = PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone;
                    patiinfo.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                    patiinfo.Patient_TimeZone = PMD.Patient_TimeZone;
                    patiinfo.Patient_Secondary3_Insurance_Details = PS3ID;

                    DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(patiinfo.Patient_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(patiinfo, SetOptions.Overwrite);
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

        [Route("API/Patient/SearchWithPatientCode")]
        [HttpPost]
        public async Task<HttpResponseMessage> SearchWithPatientCode(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                List<MT_PatientInfomation> patilist = new List<MT_PatientInfomation>();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Code", PMD.Patient_Code).WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).WhereEqualTo("Patient_Office_Type", PMD.Patient_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        patilist.Add(Docsnapshot.ConvertTo<MT_PatientInfomation>());
                    }
                    Response.DataList = patilist;
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

        [Route("API/Patient/GetPatientList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetPatientList(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                List<MT_PatientInfomation> patilist = new List<MT_PatientInfomation>();
                Query ObjQuery;
                if (PMD.Patient_Office_Type == "A" && PMD.Patient_Surgery_Physician_Center_ID == "0")
                {
                    ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).WhereEqualTo("Patient_Office_Type", PMD.Patient_Office_Type);
                }

                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        patilist.Add(Docsnapshot.ConvertTo<MT_PatientInfomation>());
                    }
                    Response.DataList = patilist.OrderByDescending(o => o.Patient_Create_Date).ThenBy(o => o.Patient_First_Name).ThenBy(o => o.Patient_Middle_Name).ThenBy(o => o.Patient_Last_Name).ToList();
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

        [Route("API/Patient/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                MT_PatientInfomation patient = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    patient = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                    Response.Data = patient;
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

        [Route("API/Patient/GetDeletedPatientList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDeletedPatientList(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                List<MT_PatientInfomation> patilist = new List<MT_PatientInfomation>();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        patilist.Add(Docsnapshot.ConvertTo<MT_PatientInfomation>());
                    }
                    Response.DataList = patilist.OrderBy(o => o.Patient_First_Name).ThenBy(o => o.Patient_Middle_Name).ThenBy(o => o.Patient_Last_Name).ToList();
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

        [Route("API/Patient/CheckInsuranceAuthentication")]
        [HttpPost]
        [Obsolete]
        public async Task<HttpResponseMessage> CheckInsuranceAuthentication(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddYears(1).AddDays(-1);
                string FilePath = "";
                MT_PatientInfomation Patient = new MT_PatientInfomation();
                string apiUrl = con.WaystarURL + "?UserID=" + con.DecryptData(con.WaystarUID) + "&Password=" + con.DecryptData(con.WaystarPWord) + "&DataFormat=" + con.WaystarDFormat + "&ResponseType=" + con.WaystarRSType + "&Data=" + con.WaystarTData;// PMD.Patient_Data;                
                HttpResponseMessage response = await con.VerifyInsureanceAsync(apiUrl);//await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    FilePath = con.CreateFile(data.ToString(), @"~/InsurDoc/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html");
                    con.HTMLToPDF(data.ToString(), @"~/InsurDoc/WSResponse_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");

                    Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID).WhereEqualTo("Patient_Is_Deleted", false);
                    QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {

                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.DataString = data;

                        //Patient = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                        //if (PMD.Patient_Insurance_Type == "1")
                        //{
                        //    Patient.Patient_Primary_Insurance_Details.PPID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Primary_Insurance_Details.PPID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Primary_Insurance_Details.PPID_Trace_Number = 0;
                        //    Patient.Patient_Primary_Insurance_Details.PPID_DocPath = FilePath;
                        //}
                        //else if (PMD.Patient_Insurance_Type == "2")
                        //{
                        //    Patient.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Secondary1_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Secondary1_Insurance_Details.PSID_Trace_Number = 0;
                        //    Patient.Patient_Secondary1_Insurance_Details.PSID_DocPath = FilePath;
                        //}
                        //else if (PMD.Patient_Insurance_Type == "3")
                        //{
                        //    Patient.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Secondary2_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Secondary2_Insurance_Details.PSID_Trace_Number = 0;
                        //    Patient.Patient_Secondary2_Insurance_Details.PSID_DocPath = FilePath;
                        //}
                        //else if (PMD.Patient_Insurance_Type == "4")
                        //{
                        //    Patient.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Secondary3_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));
                        //    Patient.Patient_Secondary3_Insurance_Details.PSID_Trace_Number = 0;
                        //    Patient.Patient_Secondary3_Insurance_Details.PSID_DocPath = FilePath;
                        //}

                        //DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(PMD.Patient_Unique_ID);
                        //WriteResult Result = await docRef.SetAsync(Patient, SetOptions.Overwrite);
                        //if (Result != null)
                        //{
                        //    Response.Status = con.StatusSuccess;
                        //    Response.Message = con.MessageSuccess;
                        //    Response.Data = PMD;
                        //    Response.DataString = data;
                        //}
                        //else
                        //{
                        //    Response.Status = con.StatusNotInsert;
                        //    Response.Message = con.MessageNotInsert;
                        //    Response.Data = null;
                        //    Response.DataString = data;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }


        [Route("API/Patient/CheckInsuranceAuthenticationNew")]
        [HttpPost]
        [Obsolete]
        public async Task<HttpResponseMessage> CheckInsuranceAuthenticationNew(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                DateTime now = DateTime.Now;
                string StartDate = new DateTime(now.Year, now.Month, 1).Date.ToString();
                string EndDate = new DateTime(now.Year, now.Month, 1).AddYears(1).AddDays(-1).Date.ToString();

                MT_PatientInfomation Patient = new MT_PatientInfomation();
                string apiUrlVerify = con.WaystarURL + "?UserID=" + con.DecryptData(con.WaystarUID) + "&Password=" + con.DecryptData(con.WaystarPWord) + "&DataFormat=" + con.WaystarDFormat + "&ResponseType=" + con.WaystarRSType + "&Data=" + PMD.Patient_Data;
                string apiUrlReverify = con.WaystarURL + "?UserID=" + con.DecryptData(con.WaystarUID) + "&Password=" + con.DecryptData(con.WaystarPWord) + "&TraceNumber=" + con.WaystarDFormat;
                HttpResponseMessage response = await con.VerifyInsureanceAsync(apiUrlVerify);//await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    //int Findex = data.ToString().IndexOf("ELI ID~");
                    int TraceNo = 000000000; //Convert.ToInt32(data.ToString().Substring(Findex, 9));

                    Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID).WhereEqualTo("Patient_Is_Deleted", false);
                    QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {
                        Patient = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                        Patient.Patient_Insurance_Type = PMD.Patient_Insurance_Type;
                        if (PMD.Patient_Insurance_Type == "1")
                        {
                            Patient.Patient_Primary_Insurance_Details.PPID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(StartDate));
                            Patient.Patient_Primary_Insurance_Details.PPID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(EndDate));
                            Patient.Patient_Primary_Insurance_Details.PPID_Trace_Number = Convert.ToInt32(TraceNo);
                        }
                        else if (PMD.Patient_Insurance_Type == "2")
                        {
                            Patient.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(StartDate));
                            Patient.Patient_Secondary1_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(EndDate));
                            Patient.Patient_Secondary1_Insurance_Details.PSID_Trace_Number = Convert.ToInt32(TraceNo);
                        }
                        else if (PMD.Patient_Insurance_Type == "3")
                        {
                            Patient.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(StartDate));
                            Patient.Patient_Secondary2_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(EndDate));
                            Patient.Patient_Secondary2_Insurance_Details.PSID_Trace_Number = Convert.ToInt32(TraceNo);

                        }
                        else if (PMD.Patient_Insurance_Type == "4")
                        {
                            Patient.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(StartDate));
                            Patient.Patient_Secondary3_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(EndDate));
                            Patient.Patient_Secondary3_Insurance_Details.PSID_Trace_Number = Convert.ToInt32(TraceNo);
                        }
                    }

                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.DataString = data;
                    Response.Data = Patient;
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                    Response.DataString = "";
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

        [Route("API/Patient/SaveInsuranceInformation")]
        [HttpPost]
        [Obsolete]
        public async Task<HttpResponseMessage> SaveInsuranceInformation(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();

            string Htmldocpath = con.CreateFile(PMD.Patient_Response_Data.ToString(), @"~/InsurDoc/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html");
            string Pdfdocpath = con.HTMLToPDF(PMD.Patient_Response_Data.ToString(), @"~/InsurDoc/WSResponse_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            try
            {
                if (PMD != null)
                {
                    if (PMD.Patient_Insurance_Type == "1")
                    {
                        PMD.Patient_Primary_Insurance_Details.PPID_DocPath = Htmldocpath;
                    }
                    else if (PMD.Patient_Insurance_Type == "2")
                    {
                        PMD.Patient_Secondary1_Insurance_Details.PSID_DocPath = Htmldocpath;
                    }
                    else if (PMD.Patient_Insurance_Type == "3")
                    {
                        PMD.Patient_Secondary2_Insurance_Details.PSID_DocPath = Htmldocpath;
                    }
                    else if (PMD.Patient_Insurance_Type == "4")
                    {
                        PMD.Patient_Secondary3_Insurance_Details.PSID_DocPath = Htmldocpath;
                    }

                    DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(PMD.Patient_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(PMD, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = PMD;
                        Response.DocPath = Pdfdocpath;
                    }
                    else
                    {
                        Response.Status = con.StatusNotInsert;
                        Response.Message = con.MessageNotInsert;
                        Response.Data = null;
                        Response.DocPath = "";
                    }
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
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

        [Route("API/Patient/CheckEmail")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> CheckEmailAsync(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientEResponse Response = new PatientEResponse();
            try
            {
                Query docRef = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Email", PMD.Patient_Email).WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap.Documents.Count == 0)
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

        [Route("API/Patient/CheckSSN")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> CheckSSN(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientEResponse Response = new PatientEResponse();
            try
            {
                Query docRef = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_SSN", PMD.Patient_SSN);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap.Documents.Count > 0)
                {
                    Response.Status = con.StatusDAE;
                    Response.Message = con.MessageDAE;
                    Response.Is_Available = true;
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
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

        [Route("API/Patient/SearchPatient")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> SearchPatient(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientEResponse Response = new PatientEResponse();
            try
            {
                Query docRef = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_First_Name", PMD.Patient_First_Name);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap.Documents.Count == 0)
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

        [Route("API/Patient/SearchWithSSN")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> SearchWithSSN(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                List<MT_PatientInfomation> PatientList = new List<MT_PatientInfomation>();
                Query docRef = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Is_Active", true).WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).OrderBy("Patient_First_Name").StartAt(PMD.Patient_First_Name.ToUpper()).EndAt(PMD.Patient_First_Name.ToUpper() + '\uf8ff');
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        PatientList.Add(Docsnap.ConvertTo<MT_PatientInfomation>());
                    }
                }

                docRef = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Is_Active", true).WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).OrderBy("Patient_SSN").StartAt(PMD.Patient_First_Name).EndAt(PMD.Patient_First_Name + '\uf8ff');
                ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        PatientList.Add(Docsnap.ConvertTo<MT_PatientInfomation>());
                    }
                }
                Response.DataList = PatientList.OrderBy(prop => prop.Patient_First_Name).ToList();
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


        [Route("API/Patient/GetEmail")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetEmail(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                MT_PatientInfomation patient = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Unique_ID", PMD.Patient_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    patient = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                    Response.DataString = patient.Patient_Email;
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


        [Route("API/Patient/AddPatient")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddPatient(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PMD.Patient_Unique_ID = UniqueID;
                PMD.Patient_First_Name = PMD.Patient_First_Name.ToUpper();
                PMD.Patient_Middle_Name = PMD.Patient_Middle_Name.ToUpper();
                PMD.Patient_Last_Name = PMD.Patient_Last_Name.ToUpper();
                Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).WhereEqualTo("Patient_Office_Type", PMD.Patient_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PMD.Patient_Code = PMD.Patient_User_Name.Substring(0, 3).ToUpper() + "000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    PMD.Patient_Code = PMD.Patient_User_Name.Substring(0, 3).ToUpper() + "000" + "1";
                }
                PMD.Patient_DOB = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_DOB),1);
                PMD.Patient_Create_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Create_Date));
                PMD.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));

                // Primary Insurance

                if (PMD.Patient_Primary_Insurance_Details != null)
                {
                    PMD.Patient_Primary_Insurance_Details.PPID_Unique_ID = con.GetUniqueKey();
                    PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No = PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No;
                    PMD.Patient_Primary_Insurance_Details.PPID_DOB = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_DOB),1);
                    PMD.Patient_Primary_Insurance_Details.PPID_Create_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Create_Date));
                    PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date));
                    PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date));
                    PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date));
                }

                // Primary Insurance End


                // 1st Secondary Insurance

                if (PMD.Patient_Secondary1_Insurance_Details != null)
                {
                    PMD.Patient_Secondary1_Insurance_Details.PSID_Unique_ID = con.GetUniqueKey();
                    PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No = PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No;
                    PMD.Patient_Secondary1_Insurance_Details.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_DOB),1);
                    PMD.Patient_Secondary1_Insurance_Details.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Create_Date));
                    PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date));
                    PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date));
                    PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date));
                }

                // 1st Secondary Insurance End

                // 2st Secondary Insurance

                if (PMD.Patient_Secondary2_Insurance_Details != null)
                {
                    PMD.Patient_Secondary2_Insurance_Details.PSID_Unique_ID = con.GetUniqueKey();
                    PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No = PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No;
                    PMD.Patient_Secondary2_Insurance_Details.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_DOB),1);
                    PMD.Patient_Secondary2_Insurance_Details.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Create_Date));
                    PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date));
                    PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date));
                    PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date));
                }

                // 2st Secondary Insurance End


                // 3st Secondary Insurance

                if (PMD.Patient_Secondary3_Insurance_Details != null)
                {
                    PMD.Patient_Secondary3_Insurance_Details.PSID_Unique_ID = con.GetUniqueKey();
                    PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No = PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No;
                    PMD.Patient_Secondary3_Insurance_Details.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_DOB),1);
                    PMD.Patient_Secondary3_Insurance_Details.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Create_Date));
                    PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date));
                    PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date));
                    PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date));
                }

                // 3st Secondary Insurance End

                DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(UniqueID);
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

        [Route("API/Patient/EditPatient")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditPatient(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {

                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    {"Patient_Prefix", PMD.Patient_Prefix},
                    {"Patient_First_Name", PMD.Patient_First_Name.ToUpper()},
                    {"Patient_Middle_Name", PMD.Patient_Middle_Name.ToUpper()},
                    {"Patient_Last_Name", PMD.Patient_Last_Name.ToUpper()},
                    {"Patient_DOB", con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_DOB),1)},
                    {"Patient_Sex", PMD.Patient_Sex},
                    {"Patient_SSN", PMD.Patient_SSN},
                    {"Patient_Address1", PMD.Patient_Address1},
                    {"Patient_Address2", PMD.Patient_Address2},
                    {"Patient_City", PMD.Patient_City},
                    {"Patient_State", PMD.Patient_State},
                    {"Patient_Zipcode", PMD.Patient_Zipcode},
                    {"Patient_Primary_No", PMD.Patient_Primary_No},
                    {"Patient_Secondary_No", PMD.Patient_Secondary_No},
                    {"Patient_Spouse_No", PMD.Patient_Spouse_No},
                    {"Patient_Work_No", PMD.Patient_Work_No},
                    {"Patient_Emergency_No", PMD.Patient_Emergency_No},
                    {"Patient_Email", PMD.Patient_Email},
                    {"Patient_Religion", PMD.Patient_Religion},
                    {"Patient_Ethinicity", PMD.Patient_Ethinicity},
                    {"Patient_Race", PMD.Patient_Race},
                    {"Patient_Marital_Status", PMD.Patient_Marital_Status},
                    {"Patient_Nationality", PMD.Patient_Nationality},
                    {"Patient_Language", PMD.Patient_Language},
                    {"Patient_Height_In_Ft", PMD.Patient_Height_In_Ft},
                    {"Patient_Height_In_Inch", PMD.Patient_Height_In_Inch},
                    {"Patient_Weight", PMD.Patient_Weight},
                    {"Patient_Body_Mass_Index", PMD.Patient_Body_Mass_Index},
                    {"Patient_Data", PMD.Patient_Data},
                    {"Patient_Insurance_Type", PMD.Patient_Insurance_Type},
                    {"Patient_Response_Data", PMD.Patient_Response_Data},
                    {"Patient_Modify_Date", con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date))},
                    {"Patient_TimeZone", PMD.Patient_TimeZone}
                };

                if (PMD.Patient_Primary_Insurance_Details != null)
                {
                    Dictionary<string, object> Primary_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PPID_Relation_To_Patient", PMD.Patient_Primary_Insurance_Details.PPID_Relation_To_Patient},
                        {"PPID_Subscriber_Name", PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_Name},
                        {"PPID_Subscriber_SSN_No", PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No},
                        {"PPID_DOB", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_DOB),1)},
                        {"PPID_Policy_No", PMD.Patient_Primary_Insurance_Details.PPID_Policy_No},
                        {"PPID_Primary_Insurance", PMD.Patient_Primary_Insurance_Details.PPID_Primary_Insurance},
                        {"PPID_PO_Box_No", PMD.Patient_Primary_Insurance_Details.PPID_PO_Box_No},
                        {"PPID_Address", PMD.Patient_Primary_Insurance_Details.PPID_Address},
                        {"PPID_Address2", PMD.Patient_Primary_Insurance_Details.PPID_Address2},
                        {"PPID_State", PMD.Patient_Primary_Insurance_Details.PPID_State},
                        {"PPID_City", PMD.Patient_Primary_Insurance_Details.PPID_City},
                        {"PPID_Zip_Code", PMD.Patient_Primary_Insurance_Details.PPID_Zip_Code},
                        {"PPID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date))},
                        {"PPID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date))},
                        {"PPID_Trace_Number", PMD.Patient_Primary_Insurance_Details.PPID_Trace_Number},
                        {"PPID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date))},
                        {"PPID_TimeZone", PMD.Patient_Primary_Insurance_Details.PPID_TimeZone}
                    };

                    initialData.Add("Patient_Primary_Insurance_Details", Primary_Insurance_Details);
                }

                if (PMD.Patient_Secondary1_Insurance_Details != null)
                {
                    Dictionary<string, object> Secondary1_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PSID_Relation_To_Patient", PMD.Patient_Secondary1_Insurance_Details.PSID_Relation_To_Patient},
                        {"PSID_Subscriber_Name", PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_Name},
                        {"PSID_Subscriber_SSN_No", PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No},
                        {"PSID_DOB", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_DOB),1)},
                        {"PSID_Policy_No", PMD.Patient_Secondary1_Insurance_Details.PSID_Policy_No},
                        {"PSID_Primary_Insurance", PMD.Patient_Secondary1_Insurance_Details.PSID_Primary_Insurance},
                        {"PSID_PO_Box_No", PMD.Patient_Secondary1_Insurance_Details.PSID_PO_Box_No},
                        {"PSID_Address", PMD.Patient_Secondary1_Insurance_Details.PSID_Address},
                        {"PSID_Address2", PMD.Patient_Secondary1_Insurance_Details.PSID_Address2},
                        {"PSID_State", PMD.Patient_Secondary1_Insurance_Details.PSID_State},
                        {"PSID_City", PMD.Patient_Secondary1_Insurance_Details.PSID_City},
                        {"PSID_Zip_Code", PMD.Patient_Secondary1_Insurance_Details.PSID_Zip_Code},
                        {"PSID_DocPath", PMD.Patient_Secondary1_Insurance_Details.PSID_DocPath},
                        {"PSID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date))},
                        {"PSID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date))},
                        {"PSID_Trace_Number", PMD.Patient_Secondary1_Insurance_Details.PSID_Trace_Number},
                        {"PSID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date))},
                        {"PSID_TimeZone", PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone}
                    };

                    initialData.Add("Patient_Secondary1_Insurance_Details", Secondary1_Insurance_Details);
                }

                if (PMD.Patient_Secondary2_Insurance_Details != null)
                {
                    Dictionary<string, object> Secondary2_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PSID_Relation_To_Patient", PMD.Patient_Secondary2_Insurance_Details.PSID_Relation_To_Patient},
                        {"PSID_Subscriber_Name", PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_Name},
                        {"PSID_Subscriber_SSN_No", PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No},
                        {"PSID_DOB", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_DOB),1)},
                        {"PSID_Policy_No", PMD.Patient_Secondary2_Insurance_Details.PSID_Policy_No},
                        {"PSID_Primary_Insurance", PMD.Patient_Secondary2_Insurance_Details.PSID_Primary_Insurance},
                        {"PSID_PO_Box_No", PMD.Patient_Secondary2_Insurance_Details.PSID_PO_Box_No},
                        {"PSID_Address", PMD.Patient_Secondary2_Insurance_Details.PSID_Address},
                        {"PSID_Address2", PMD.Patient_Secondary2_Insurance_Details.PSID_Address2},
                        {"PSID_State", PMD.Patient_Secondary2_Insurance_Details.PSID_State},
                        {"PSID_City", PMD.Patient_Secondary2_Insurance_Details.PSID_City},
                        {"PSID_Zip_Code", PMD.Patient_Secondary2_Insurance_Details.PSID_Zip_Code},
                        {"PSID_DocPath", PMD.Patient_Secondary2_Insurance_Details.PSID_DocPath},
                        {"PSID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date))},
                        {"PSID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date))},
                        {"PSID_Trace_Number", PMD.Patient_Secondary2_Insurance_Details.PSID_Trace_Number},
                        {"PSID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date))},
                        {"PSID_TimeZone", PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone}
                    };

                    initialData.Add("Patient_Secondary2_Insurance_Details", Secondary2_Insurance_Details);
                }

                if (PMD.Patient_Secondary3_Insurance_Details != null)
                {
                    Dictionary<string, object> Secondary3_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PSID_Relation_To_Patient", PMD.Patient_Secondary3_Insurance_Details.PSID_Relation_To_Patient},
                        {"PSID_Subscriber_Name", PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_Name},
                        {"PSID_Subscriber_SSN_No", PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No},
                        {"PSID_DOB", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_DOB),1)},
                        {"PSID_Policy_No", PMD.Patient_Secondary3_Insurance_Details.PSID_Policy_No},
                        {"PSID_Primary_Insurance", PMD.Patient_Secondary3_Insurance_Details.PSID_Primary_Insurance},
                        {"PSID_PO_Box_No", PMD.Patient_Secondary3_Insurance_Details.PSID_PO_Box_No},
                        {"PSID_Address", PMD.Patient_Secondary3_Insurance_Details.PSID_Address},
                        {"PSID_Address2", PMD.Patient_Secondary3_Insurance_Details.PSID_Address2},
                        {"PSID_State", PMD.Patient_Secondary3_Insurance_Details.PSID_State},
                        {"PSID_City", PMD.Patient_Secondary3_Insurance_Details.PSID_City},
                        {"PSID_Zip_Code", PMD.Patient_Secondary3_Insurance_Details.PSID_Zip_Code},
                        {"PSID_DocPath", PMD.Patient_Secondary3_Insurance_Details.PSID_DocPath},
                        {"PSID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date))},
                        {"PSID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date))},
                        {"PSID_Trace_Number", PMD.Patient_Secondary3_Insurance_Details.PSID_Trace_Number},
                        {"PSID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date))},
                        {"PSID_TimeZone", PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone}
                    };

                    initialData.Add("Patient_Secondary3_Insurance_Details", Secondary3_Insurance_Details);
                }

                DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(PMD.Patient_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
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

        [Route("API/Patient/FillDetailsByPatient")]
        [HttpPost]
        public async Task<HttpResponseMessage> FillDetailsByPatient(MT_PatientInfomation PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PatientInfoResponse Response = new PatientInfoResponse();
            try
            {
                Dictionary<string, object> initialData1 = new Dictionary<string, object>
                {
                    {"Patient_Prefix", PMD.Patient_Prefix},
                    {"Patient_First_Name", PMD.Patient_First_Name.ToUpper()},
                    {"Patient_Middle_Name", PMD.Patient_Middle_Name.ToUpper()},
                    {"Patient_Last_Name", PMD.Patient_Last_Name.ToUpper()},
                    {"Patient_DOB", con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_DOB),1)},
                    {"Patient_Sex", PMD.Patient_Sex},
                    {"Patient_SSN", PMD.Patient_SSN},
                    {"Patient_Address1", PMD.Patient_Address1},
                    {"Patient_Address2", PMD.Patient_Address2},
                    {"Patient_City", PMD.Patient_City},
                    {"Patient_State", PMD.Patient_State},
                    {"Patient_Zipcode", PMD.Patient_Zipcode},
                    {"Patient_Primary_No", PMD.Patient_Primary_No},
                    {"Patient_Secondary_No", PMD.Patient_Secondary_No},
                    {"Patient_Spouse_No", PMD.Patient_Spouse_No},
                    {"Patient_Work_No", PMD.Patient_Work_No},
                    {"Patient_Emergency_No", PMD.Patient_Emergency_No},
                    {"Patient_Email", PMD.Patient_Email},
                    {"Patient_Religion", PMD.Patient_Religion},
                    {"Patient_Ethinicity", PMD.Patient_Ethinicity},
                    {"Patient_Race", PMD.Patient_Race},
                    {"Patient_Marital_Status", PMD.Patient_Marital_Status},
                    {"Patient_Nationality", PMD.Patient_Nationality},
                    {"Patient_Language", PMD.Patient_Language},
                    {"Patient_Height_In_Ft", PMD.Patient_Height_In_Ft},
                    {"Patient_Height_In_Inch", PMD.Patient_Height_In_Inch},
                    {"Patient_Weight", PMD.Patient_Weight},
                    {"Patient_Body_Mass_Index", PMD.Patient_Body_Mass_Index},
                    {"Patient_Data", PMD.Patient_Data},
                    {"Patient_Insurance_Type", PMD.Patient_Insurance_Type},
                    {"Patient_Response_Data", PMD.Patient_Response_Data},
                    {"Patient_Modify_Date", con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date))},
                    {"Patient_TimeZone", PMD.Patient_TimeZone}
                };

                if (PMD.Patient_Primary_Insurance_Details != null)
                {
                    Dictionary<string, object> Primary_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PPID_Relation_To_Patient", PMD.Patient_Primary_Insurance_Details.PPID_Relation_To_Patient},
                        {"PPID_Subscriber_Name", PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_Name},
                        {"PPID_Subscriber_SSN_No", PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No},
                        {"PPID_DOB", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_DOB),1)},
                        {"PPID_Policy_No", PMD.Patient_Primary_Insurance_Details.PPID_Policy_No},
                        {"PPID_Primary_Insurance", PMD.Patient_Primary_Insurance_Details.PPID_Primary_Insurance},
                        {"PPID_PO_Box_No", PMD.Patient_Primary_Insurance_Details.PPID_PO_Box_No},
                        {"PPID_Address", PMD.Patient_Primary_Insurance_Details.PPID_Address},
                        {"PPID_Address2", PMD.Patient_Primary_Insurance_Details.PPID_Address2},
                        {"PPID_State", PMD.Patient_Primary_Insurance_Details.PPID_State},
                        {"PPID_City", PMD.Patient_Primary_Insurance_Details.PPID_City},
                        {"PPID_Zip_Code", PMD.Patient_Primary_Insurance_Details.PPID_Zip_Code},
                        {"PPID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date))},
                        {"PPID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date))},
                        {"PPID_Trace_Number", PMD.Patient_Primary_Insurance_Details.PPID_Trace_Number},
                        {"PPID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date))},
                        {"PPID_TimeZone", PMD.Patient_Primary_Insurance_Details.PPID_TimeZone}
                    };

                    initialData1.Add("Patient_Primary_Insurance_Details", Primary_Insurance_Details);
                }

                if (PMD.Patient_Secondary1_Insurance_Details != null)
                {
                    Dictionary<string, object> Secondary1_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PSID_Relation_To_Patient", PMD.Patient_Secondary1_Insurance_Details.PSID_Relation_To_Patient},
                        {"PSID_Subscriber_Name", PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_Name},
                        {"PSID_Subscriber_SSN_No", PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No},
                        {"PSID_DOB", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_DOB),1)},
                        {"PSID_Policy_No", PMD.Patient_Secondary1_Insurance_Details.PSID_Policy_No},
                        {"PSID_Primary_Insurance", PMD.Patient_Secondary1_Insurance_Details.PSID_Primary_Insurance},
                        {"PSID_PO_Box_No", PMD.Patient_Secondary1_Insurance_Details.PSID_PO_Box_No},
                        {"PSID_Address", PMD.Patient_Secondary1_Insurance_Details.PSID_Address},
                        {"PSID_Address2", PMD.Patient_Secondary1_Insurance_Details.PSID_Address2},
                        {"PSID_State", PMD.Patient_Secondary1_Insurance_Details.PSID_State},
                        {"PSID_City", PMD.Patient_Secondary1_Insurance_Details.PSID_City},
                        {"PSID_Zip_Code", PMD.Patient_Secondary1_Insurance_Details.PSID_Zip_Code},
                        {"PSID_DocPath", PMD.Patient_Secondary1_Insurance_Details.PSID_DocPath},
                        {"PSID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date))},
                        {"PSID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date))},
                        {"PSID_Trace_Number", PMD.Patient_Secondary1_Insurance_Details.PSID_Trace_Number},
                        {"PSID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date))},
                        {"PSID_TimeZone", PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone}
                    };

                    initialData1.Add("Patient_Secondary1_Insurance_Details", Secondary1_Insurance_Details);
                }

                if (PMD.Patient_Secondary2_Insurance_Details != null)
                {
                    Dictionary<string, object> Secondary2_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PSID_Relation_To_Patient", PMD.Patient_Secondary2_Insurance_Details.PSID_Relation_To_Patient},
                        {"PSID_Subscriber_Name", PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_Name},
                        {"PSID_Subscriber_SSN_No", PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No},
                        {"PSID_DOB", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_DOB),1)},
                        {"PSID_Policy_No", PMD.Patient_Secondary2_Insurance_Details.PSID_Policy_No},
                        {"PSID_Primary_Insurance", PMD.Patient_Secondary2_Insurance_Details.PSID_Primary_Insurance},
                        {"PSID_PO_Box_No", PMD.Patient_Secondary2_Insurance_Details.PSID_PO_Box_No},
                        {"PSID_Address", PMD.Patient_Secondary2_Insurance_Details.PSID_Address},
                        {"PSID_Address2", PMD.Patient_Secondary2_Insurance_Details.PSID_Address2},
                        {"PSID_State", PMD.Patient_Secondary2_Insurance_Details.PSID_State},
                        {"PSID_City", PMD.Patient_Secondary2_Insurance_Details.PSID_City},
                        {"PSID_Zip_Code", PMD.Patient_Secondary2_Insurance_Details.PSID_Zip_Code},
                        {"PSID_DocPath", PMD.Patient_Secondary2_Insurance_Details.PSID_DocPath},
                        {"PSID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date))},
                        {"PSID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date))},
                        {"PSID_Trace_Number", PMD.Patient_Secondary2_Insurance_Details.PSID_Trace_Number},
                        {"PSID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date))},
                        {"PSID_TimeZone", PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone}
                    };

                    initialData1.Add("Patient_Secondary2_Insurance_Details", Secondary2_Insurance_Details);
                }

                if (PMD.Patient_Secondary3_Insurance_Details != null)
                {
                    Dictionary<string, object> Secondary3_Insurance_Details = new Dictionary<string, object>
                    {
                        {"PSID_Relation_To_Patient", PMD.Patient_Secondary3_Insurance_Details.PSID_Relation_To_Patient},
                        {"PSID_Subscriber_Name", PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_Name},
                        {"PSID_Subscriber_SSN_No", PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No},
                        {"PSID_DOB", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_DOB),1)},
                        {"PSID_Policy_No", PMD.Patient_Secondary3_Insurance_Details.PSID_Policy_No},
                        {"PSID_Primary_Insurance", PMD.Patient_Secondary3_Insurance_Details.PSID_Primary_Insurance},
                        {"PSID_PO_Box_No", PMD.Patient_Secondary3_Insurance_Details.PSID_PO_Box_No},
                        {"PSID_Address", PMD.Patient_Secondary3_Insurance_Details.PSID_Address},
                        {"PSID_Address2", PMD.Patient_Secondary3_Insurance_Details.PSID_Address2},
                        {"PSID_State", PMD.Patient_Secondary3_Insurance_Details.PSID_State},
                        {"PSID_City", PMD.Patient_Secondary3_Insurance_Details.PSID_City},
                        {"PSID_Zip_Code", PMD.Patient_Secondary3_Insurance_Details.PSID_Zip_Code},
                        {"PSID_DocPath", PMD.Patient_Secondary3_Insurance_Details.PSID_DocPath},
                        {"PSID_V_Start_Date", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date))},
                        {"PSID_V_End_Date", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date))},
                        {"PSID_Trace_Number", PMD.Patient_Secondary3_Insurance_Details.PSID_Trace_Number},
                        {"PSID_Modify_Date", con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date))},
                        {"PSID_TimeZone", PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone}
                    };

                    initialData1.Add("Patient_Secondary3_Insurance_Details", Secondary3_Insurance_Details);
                }

                //PMD.Patient_First_Name = PMD.Patient_First_Name.ToUpper();
                //PMD.Patient_Middle_Name = PMD.Patient_Middle_Name.ToUpper();
                //PMD.Patient_Last_Name = PMD.Patient_Last_Name.ToUpper();
                //Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Surgery_Physician_Center_ID", PMD.Patient_Surgery_Physician_Center_ID).WhereEqualTo("Patient_Office_Type", PMD.Patient_Office_Type);
                //QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                //if (ObjQuerySnap != null)
                //{
                //    PMD.Patient_Code = PMD.Patient_User_Name.Substring(0, 3).ToUpper() + "000" + ObjQuerySnap.Documents.Count + 1.ToString();
                //}
                //else
                //{
                //    PMD.Patient_Code = PMD.Patient_User_Name.Substring(0, 3).ToUpper() + "000" + "1";
                //}
                //PMD.Patient_DOB = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_DOB), 1);
                ////PMD.Patient_Create_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Create_Date));
                //PMD.Patient_Modify_Date = con.ConvertTimeZone(PMD.Patient_TimeZone, Convert.ToDateTime(PMD.Patient_Modify_Date));

                //// Primary Insurance

                //if (PMD.Patient_Primary_Insurance_Details != null)
                //{
                //    PMD.Patient_Primary_Insurance_Details.PPID_Unique_ID = con.GetUniqueKey();
                //    PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No = PMD.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No;
                //    PMD.Patient_Primary_Insurance_Details.PPID_DOB = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_DOB), 1);
                //    PMD.Patient_Primary_Insurance_Details.PPID_Create_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Create_Date));
                //    PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_Modify_Date));
                //    PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_Start_Date));
                //    PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Primary_Insurance_Details.PPID_TimeZone, Convert.ToDateTime(PMD.Patient_Primary_Insurance_Details.PPID_V_End_Date));
                //}

                //// Primary Insurance End


                //// 1st Secondary Insurance

                //if (PMD.Patient_Secondary1_Insurance_Details != null)
                //{
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_Unique_ID = con.GetUniqueKey();
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No = PMD.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No;
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_DOB), 1);
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Create_Date));
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_Modify_Date));
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date));
                //    PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary1_Insurance_Details.PSID_V_End_Date));
                //}

                //// 1st Secondary Insurance End

                //// 2st Secondary Insurance

                //if (PMD.Patient_Secondary2_Insurance_Details != null)
                //{
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_Unique_ID = con.GetUniqueKey();
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No = PMD.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No;
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_DOB), 1);
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Create_Date));
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_Modify_Date));
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date));
                //    PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary2_Insurance_Details.PSID_V_End_Date));
                //}

                //// 2st Secondary Insurance End


                //// 3st Secondary Insurance

                //if (PMD.Patient_Secondary3_Insurance_Details != null)
                //{
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_Unique_ID = con.GetUniqueKey();
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No = PMD.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No;
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_DOB = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_DOB), 1);
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_Create_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Create_Date));
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_Modify_Date));
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date));
                //    PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date = con.ConvertTimeZone(PMD.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PMD.Patient_Secondary3_Insurance_Details.PSID_V_End_Date));
                //}

                //// 3st Secondary Insurance End

                DocumentReference docRef = Db.Collection("MT_PatientInfomation").Document(PMD.Patient_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData1);
                if (Result != null)
                {
                    MT_Patient_Booking booking = new MT_Patient_Booking();
                    Query Qty = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Patient_ID", PMD.Patient_Unique_ID);
                    QuerySnapshot ObjBooking = await Qty.GetSnapshotAsync();
                    if (ObjBooking != null)
                    {
                        foreach (DocumentSnapshot docsnap in ObjBooking.Documents)
                        {
                            booking = docsnap.ConvertTo<MT_Patient_Booking>();
                            Dictionary<string, object> initialData = new Dictionary<string, object>
                            {
                                { "PB_Patient_Name",PMD.Patient_First_Name},
                                { "PB_Patient_Last_Name",PMD.Patient_Last_Name}
                            };

                            DocumentReference DocReff=Db.Collection("MT_Patient_Booking").Document(booking.PB_Unique_ID);
                            WriteResult Results = await DocReff.UpdateAsync(initialData);
                        }
                    }
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
    }
}
