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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Net.Http.Headers;

namespace TheCloudHealth.Controllers
{
    public class BookingController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        ICreatePDF ObjPDF;
        string UniqueID = "";        
        public BookingController() {
            con = new ConnectionClass();
            ObjPDF = new CreatePDF();
            //Db = con.Db();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Booking/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PMD.PB_Unique_ID= UniqueID;
                Query ObjQuery = Db.Collection("MT_Patient_Booking");//.WhereEqualTo("PB_Surgery_Physician_Center_ID", PMD.PB_Surgery_Physician_Center_ID).WhereEqualTo("PB_Office_Type", PMD.PB_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PMD.PB_Booking_No = "BKN000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    PMD.PB_Booking_No = "BKN000" + "1";
                }
                PMD.PB_Booking_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Booking_Date));
                PMD.PB_Create_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Create_Date));
                PMD.PB_Modify_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date));
                PMD.PB_Is_Deleted = true;
                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(UniqueID);
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

        [Route("API/Booking/AddIncident")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddIncident(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Incident_Details InciDetails = new Incident_Details();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    InciDetails.Inci_Unique_ID = con.GetUniqueKey();
                    InciDetails.Inci_Type_ID = PMD.PB_Incient_Detail.Inci_Type_ID;
                    InciDetails.Inci_Type = PMD.PB_Incient_Detail.Inci_Type;
                    InciDetails.Inci_Claim_No = PMD.PB_Incient_Detail.Inci_Claim_No;
                    InciDetails.Inci_DOI = con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_DOI));
                    InciDetails.Inci_Employee_Name = PMD.PB_Incient_Detail.Inci_Employee_Name;
                    InciDetails.Inci_Employee_Address = PMD.PB_Incient_Detail.Inci_Employee_Address;
                    InciDetails.Inci_Employee_Phone_No = PMD.PB_Incient_Detail.Inci_Employee_Phone_No;
                    InciDetails.Inci_Is_This_Lien = PMD.PB_Incient_Detail.Inci_Is_This_Lien;
                    InciDetails.Inci_Attorney_Name = PMD.PB_Incient_Detail.Inci_Attorney_Name;
                    InciDetails.Inci_Attorney_Phone_No = PMD.PB_Incient_Detail.Inci_Attorney_Phone_No;
                    InciDetails.Inci_TimeOfIncident = PMD.PB_Incient_Detail.Inci_TimeOfIncident;
                    InciDetails.Inci_Comment = PMD.PB_Incient_Detail.Inci_Comment;
                    InciDetails.Inci_Created_By = PMD.PB_Incient_Detail.Inci_Created_By;
                    InciDetails.Inci_User_Name = PMD.PB_Incient_Detail.Inci_User_Name;
                    InciDetails.Inci_Create_Date = con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Create_Date));
                    InciDetails.Inci_Modify_Date = con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Modify_Date));
                    InciDetails.Inci_Is_Active = PMD.PB_Incient_Detail.Inci_Is_Active;
                    InciDetails.Inci_Is_Deleted = PMD.PB_Incient_Detail.Inci_Is_Deleted;
                    InciDetails.Inci_TimeZone = PMD.PB_Incient_Detail.Inci_TimeZone;

                    BookingInfo.PB_TimeZone = PMD.PB_TimeZone;
                    BookingInfo.PB_Incient_Detail = InciDetails;
                    BookingInfo.PB_Is_Deleted = false;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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

        [Route("API/Booking/AddSurgicalProcedureInformation")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSurgicalProcedureInformation(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Surgical_Procedure_Information SPInfo = new Surgical_Procedure_Information();
                //List<CPTSelected> CPTSelList = new List<CPTSelected>();
                //List<ICDSelected> ICDSelList = new List<ICDSelected>();
                //List<ProcedureSelected> ProcSelList = new List<ProcedureSelected>();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    SPInfo.SPI_Unique_ID = con.GetUniqueKey();
                    SPInfo.SPI_Surgery_Center_ID = PMD.PB_Surgical_Procedure_Information.SPI_Surgery_Center_ID;
                    SPInfo.SPI_Surgery_Center_Name = PMD.PB_Surgical_Procedure_Information.SPI_Surgery_Center_Name;
                    SPInfo.SPI_Surgeon_ID = PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_ID;
                    SPInfo.SPI_Surgeon_Name = PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_Name;
                    SPInfo.SPI_Assi_Surgeon_ID = PMD.PB_Surgical_Procedure_Information.SPI_Assi_Surgeon_ID;
                    SPInfo.SPI_Assi_Surgeon_Name = PMD.PB_Surgical_Procedure_Information.SPI_Assi_Surgeon_Name;
                    SPInfo.SPI_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date));
                    SPInfo.SPI_Time = PMD.PB_Surgical_Procedure_Information.SPI_Time;
                    SPInfo.SPI_Duration = PMD.PB_Surgical_Procedure_Information.SPI_Duration;
                    SPInfo.SPI_Anesthesia_Type_ID = PMD.PB_Surgical_Procedure_Information.SPI_Anesthesia_Type_ID;
                    SPInfo.SPI_Anesthesia_Type = PMD.PB_Surgical_Procedure_Information.SPI_Anesthesia_Type;
                    SPInfo.SPI_Block_ID = PMD.PB_Surgical_Procedure_Information.SPI_Block_ID;
                    SPInfo.SPI_Block_Type = PMD.PB_Surgical_Procedure_Information.SPI_Block_Type;
                    // SPInfo.SPI_Procedure = PMD.PB_Surgical_Procedure_Information.SPI_Procedure;
                    SPInfo.SPI_Created_By = PMD.PB_Surgical_Procedure_Information.SPI_Created_By;
                    SPInfo.SPI_User_Name = PMD.PB_Surgical_Procedure_Information.SPI_User_Name;
                    SPInfo.SPI_Create_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Create_Date));
                    SPInfo.SPI_Modify_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date));
                    SPInfo.SPI_Is_Active = PMD.PB_Surgical_Procedure_Information.SPI_Is_Active;
                    SPInfo.SPI_Is_Deleted = PMD.PB_Surgical_Procedure_Information.SPI_Is_Deleted;
                    SPInfo.SPI_TimeZone = PMD.PB_Surgical_Procedure_Information.SPI_TimeZone;

                    //if (PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList != null)
                    //{
                    //    foreach (CPTSelected Sel in PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList)
                    //    {
                    //        CPTSelected cptsel = Sel;
                    //        cptsel.CPTS_Unique_ID = con.GetUniqueKey();
                    //        //cptsel.CPTS_Create_Date = con.ConvertTimeZone(cptsel.CPTS_TimeZone, Convert.ToDateTime(cptsel.CPTS_Create_Date));
                    //        //cptsel.CPTS_Modify_Date = con.ConvertTimeZone(cptsel.CPTS_TimeZone, Convert.ToDateTime(cptsel.CPTS_Modify_Date));
                    //        CPTSelList.Add(cptsel);
                    //    }
                    //}

                    //if (PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList != null)
                    //{
                    //    foreach (ICDSelected Sel in PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList)
                    //    {
                    //        ICDSelected icdsel = Sel;
                    //        icdsel.ICD_Unique_ID = con.GetUniqueKey();
                    //        //icdsel.ICD_Create_Date = con.ConvertTimeZone(icdsel.ICD_TimeZone, Convert.ToDateTime(icdsel.ICD_Create_Date));
                    //        //icdsel.ICD_Modify_Date = con.ConvertTimeZone(icdsel.ICD_TimeZone, Convert.ToDateTime(icdsel.ICD_Modify_Date));
                    //        ICDSelList.Add(icdsel);
                    //    }
                    //}

                    //if (PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList != null)
                    //{
                    //    foreach (ProcedureSelected Sel in PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList)
                    //    {
                    //        ProcedureSelected Procsel = Sel;
                    //        Procsel.Proc_Unique_ID = con.GetUniqueKey();
                    //        //Procsel.Proc_Create_Date = con.ConvertTimeZone(Procsel.Proc_TimeZone, Convert.ToDateTime(Procsel.Proc_Create_Date));
                    //        //Procsel.Proc_Modify_Date = con.ConvertTimeZone(Procsel.Proc_TimeZone, Convert.ToDateTime(Procsel.Proc_Modify_Date));
                    //        ProcSelList.Add(Procsel);
                    //    }
                    //}

                    //SPInfo.SPI_ICD_SelectedList = ICDSelList;
                    //SPInfo.SPI_CPT_SelectedList = CPTSelList;
                    //SPInfo.SPI_Procedure_SelectedList = ProcSelList;
                    BookingInfo.PB_Booking_Surgery_Center_ID = PMD.PB_Booking_Surgery_Center_ID;
                    BookingInfo.PB_Booking_Surgery_Center_Name = PMD.PB_Booking_Surgery_Center_Name;
                    BookingInfo.PB_TimeZone = PMD.PB_TimeZone;
                    BookingInfo.PB_Surgical_Procedure_Information = SPInfo;
                    BookingInfo.PB_Booking_Date= con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date));
                    BookingInfo.PB_Booking_Duration = PMD.PB_Surgical_Procedure_Information.SPI_Duration;
                    BookingInfo.PB_Booking_Time = PMD.PB_Surgical_Procedure_Information.SPI_Time;
                    BookingInfo.PB_Is_Deleted = false;


                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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

        [Route("API/Booking/AddPreoperativeMedicalClearance")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddPreoperativeMedicalClearance(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Preoprative_Medical_Clearance PMClearance = new Preoprative_Medical_Clearance();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    PMClearance.PMC_Unique_ID = con.GetUniqueKey();
                    PMClearance.PMC_Is_Require_Pre_Op_Medi_Clearance = PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_Pre_Op_Medi_Clearance;
                    PMClearance.PMC_Clearing_Physician_Name = PMD.PB_Preoprative_Medical_Clearance.PMC_Clearing_Physician_Name;
                    PMClearance.PMC_Is_Require_EKG = PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_EKG;
                    PMClearance.PMC_Created_By = PMD.PB_Preoprative_Medical_Clearance.PMC_Created_By;
                    PMClearance.PMC_User_Name = PMD.PB_Preoprative_Medical_Clearance.PMC_User_Name;
                    PMClearance.PMC_Create_Date = con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone, Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Create_Date));
                    PMClearance.PMC_Modify_Date = con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone, Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Modify_Date));
                    PMClearance.PMC_Is_Active = PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Active;
                    PMClearance.PMC_Is_Deleted = PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Deleted;
                    PMClearance.PMC_TimeZone = PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone;

                    BookingInfo.PB_TimeZone = PMD.PB_TimeZone;
                    BookingInfo.PB_Preoprative_Medical_Clearance = PMClearance;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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


        [Route("API/Booking/AddSpecialRequest")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSpecialRequest(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Special_Request SpeRequest = new Special_Request();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    SpeRequest.SR_Unique_ID = con.GetUniqueKey();
                    SpeRequest.SR_Is_Special_Equip_Req = PMD.PB_Special_Request.SR_Is_Special_Equip_Req;
                    SpeRequest.SR_Equip_ID = PMD.PB_Special_Request.SR_Equip_ID;
                    SpeRequest.SR_Equip_Name = PMD.PB_Special_Request.SR_Equip_Name;
                    SpeRequest.SR_Supplies_ID = PMD.PB_Special_Request.SR_Supplies_ID;
                    SpeRequest.SR_Supplies_Name = PMD.PB_Special_Request.SR_Supplies_Name;
                    SpeRequest.SR_Instrumentation_ID = PMD.PB_Special_Request.SR_Instrumentation_ID;
                    SpeRequest.SR_Instrumentation_Name = PMD.PB_Special_Request.SR_Instrumentation_Name;
                    SpeRequest.SR_Other = PMD.PB_Special_Request.SR_Other;
                    SpeRequest.SR_Created_By = PMD.PB_Special_Request.SR_Created_By;
                    SpeRequest.SR_User_Name = PMD.PB_Special_Request.SR_User_Name;
                    SpeRequest.SR_Create_Date = con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone, Convert.ToDateTime(PMD.PB_Special_Request.SR_Create_Date));
                    SpeRequest.SR_Modify_Date = con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone, Convert.ToDateTime(PMD.PB_Special_Request.SR_Modify_Date));
                    SpeRequest.SR_Is_Active = PMD.PB_Special_Request.SR_Is_Active;
                    SpeRequest.SR_Is_Deleted = PMD.PB_Special_Request.SR_Is_Deleted;
                    SpeRequest.SR_TimeZone = PMD.PB_Special_Request.SR_TimeZone;

                    BookingInfo.PB_TimeZone = PMD.PB_TimeZone;
                    BookingInfo.PB_Special_Request = SpeRequest;
                    BookingInfo.PB_Is_Deleted = false;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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


        [Route("API/Booking/AddAlerts")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddAlerts(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<Alerts> AlrtList = new List<Alerts>();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (PMD.PB_Alerts != null)
                    {
                        foreach (Alerts Alt in PMD.PB_Alerts)
                        {
                            //Alt.Alrt_Unique_ID = con.GetUniqueKey();
                            AlrtList.Add(Alt);
                        }
                    }

                    BookingInfo.PB_TimeZone = PMD.PB_TimeZone;
                    BookingInfo.PB_Alerts = AlrtList.OrderBy(o =>o.Alrt_Name).ToList();
                    BookingInfo.PB_Is_Deleted = false;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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

        [Route("API/Booking/AddInsurancePrecertificationAuthorization")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddInsurancePrecertificationAuthorization(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Insurance_Precertification_Authorization PIPA = new Insurance_Precertification_Authorization();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    PIPA.IPA_Unique_ID = con.GetUniqueKey();
                    PIPA.IPA_Insurace_Company_Phone_No = PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Phone_No;
                    PIPA.IPA_Insurace_Company_Representative = PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Representative;
                    PIPA.IPA_Authorization_Name = PMD.PB_Insurance_Precertification_Authorization.IPA_Authorization_Name;
                    PIPA.IPA_DOA = con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_DOA));
                    PIPA.IPA_Created_By = PMD.PB_Insurance_Precertification_Authorization.IPA_Created_By;
                    PIPA.IPA_User_Name = PMD.PB_Insurance_Precertification_Authorization.IPA_User_Name;
                    PIPA.IPA_Create_Date = con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Create_Date));
                    PIPA.IPA_Modify_Date = con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Modify_Date));
                    PIPA.IPA_Is_Active = PMD.PB_Insurance_Precertification_Authorization.IPA_Is_Active;
                    PIPA.IPA_Is_Deleted = PMD.PB_Insurance_Precertification_Authorization.IPA_Is_Deleted;
                    PIPA.IPA_TimeZone = PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone;

                    BookingInfo.PB_TimeZone = PMD.PB_TimeZone;
                    BookingInfo.PB_Insurance_Precertification_Authorization = PIPA;
                    BookingInfo.PB_Is_Deleted = false;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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

        public MT_Patient_Booking GetImages()
        {
            MT_Patient_Booking Surcenter = new MT_Patient_Booking();
            try
            {
                List<Doc_Uploaded> DocUplodList = new List<Doc_Uploaded>();

                string[] DBPath;
                int i = 0;
                var httpRequest = HttpContext.Current.Request;
                var postedData = httpRequest.Form[1];
                string str = postedData.Substring(1, postedData.Length - 2);
                JObject jobject = JObject.Parse(str);
                Surcenter = JsonConvert.DeserializeObject<MT_Patient_Booking>(jobject.ToString());

                var Doclist = httpRequest.Form[0];
                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Doclist);

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        int hasheddate = DateTime.Now.GetHashCode();

                        var myKey = dict.FirstOrDefault(x => x.Value == postedFile.FileName.ToString()).Key;

                        string changed_name = myKey + DateTime.Now.ToString("yyyyMMddHHmmss") + postedFile.FileName.Substring(postedFile.FileName.IndexOf('.') , (postedFile.FileName.Length - postedFile.FileName.IndexOf('.')));

                        var filePath = HttpContext.Current.Server.MapPath("~/Images/" + changed_name);

                        postedFile.SaveAs(filePath); // save the file to a folder "Images" in the root of your app

                        //changed_name = @"~\Images\" + changed_name; //store this complete path to database
                        Doc_Uploaded DocUpld = new Doc_Uploaded();
                        DocUpld.DU_Unique_ID = con.GetUniqueKey();
                        DocUpld.DU_Attachment_Type = myKey;
                        DocUpld.DU_Doc_Path = @"~\Images\" + changed_name;
                        DocUpld.DU_Doc_Name = changed_name.Substring(0,10);
                        DocUpld.DU_Created_By = Surcenter.PB_Created_By;
                        DocUpld.DU_User_Name = Surcenter.PB_User_Name;
                        DocUpld.DU_Create_Date = con.ConvertTimeZone(Surcenter.PB_TimeZone, Convert.ToDateTime(Surcenter.PB_Modify_Date));
                        DocUpld.DU_Modify_Date = con.ConvertTimeZone(Surcenter.PB_TimeZone, Convert.ToDateTime(Surcenter.PB_Modify_Date));
                        DocUpld.DU_Is_Active = true;
                        DocUpld.DU_Is_Deleted = false;
                        DocUpld.DU_TimeZone = Surcenter.PB_TimeZone;

                        DocUplodList.Add(DocUpld);
                        i++;
                    }
                    Surcenter.PB_Doc_Uploaded_List = DocUplodList;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Surcenter;
        }

        [Route("API/Booking/UploadPatientDocuments")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadPatientDocuments()
        {
            MT_Patient_Booking PMD = GetImages();
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<Doc_Uploaded> DocList = new List<Doc_Uploaded>();
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (Booking.PB_Doc_Uploaded_List != null)
                    {
                        foreach (Doc_Uploaded DU in Booking.PB_Doc_Uploaded_List)
                        {
                            DocList.Add(DU);
                        }
                    }
                }

                if (PMD.PB_Doc_Uploaded_List != null)
                {
                    foreach (Doc_Uploaded DU in PMD.PB_Doc_Uploaded_List)
                    {
                        DocList.Add(DU);
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_TimeZone" , PMD.PB_TimeZone},
                        {"PB_Doc_Uploaded_List" , DocList},
                        {"PB_Modify_Date" , con.ConvertTimeZone(PMD.PB_TimeZone,Convert.ToDateTime(PMD.PB_Modify_Date))}
                    };
                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(PMD.PB_Unique_ID);
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

        [Route("API/Booking/IsDeletedUploadPatientDocuments")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeletedUploadPatientDocuments(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<Doc_Uploaded> DocList = new List<Doc_Uploaded>();
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID).WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (Booking.PB_Doc_Uploaded_List != null)
                    {
                        foreach (Doc_Uploaded DU in Booking.PB_Doc_Uploaded_List)
                        {
                            if (DU.DU_Unique_ID != PMD.PB_Doc_Uploaded_List[0].DU_Unique_ID)
                            {
                                DocList.Add(DU);
                            }
                        }
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_TimeZone" , PMD.PB_TimeZone},
                        {"PB_Doc_Uploaded_List" , DocList},
                        {"PB_Modify_Date" , con.ConvertTimeZone(PMD.PB_TimeZone,Convert.ToDateTime(PMD.PB_Modify_Date))}
                    };
                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(PMD.PB_Unique_ID);
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

        [Route("API/Booking/PatientForms")]
        [HttpPost]
        public async Task<HttpResponseMessage> PatientForms(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<Patient_Forms> FormsList = new List<Patient_Forms>();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);//.WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (BookingInfo.PB_Forms != null)
                    {
                        foreach (Patient_Forms PF in BookingInfo.PB_Forms)
                        {
                            FormsList.Add(PF);
                        }
                    }
                    if (PMD.PB_Forms != null)
                    {
                        foreach (Patient_Forms PF in PMD.PB_Forms)
                        {
                            PF.PF_Create_Date = con.ConvertTimeZone(PF.PF_TimeZone, Convert.ToDateTime(PF.PF_Create_Date));
                            PF.PF_Modify_Date = con.ConvertTimeZone(PF.PF_TimeZone, Convert.ToDateTime(PF.PF_Modify_Date));
                            FormsList.Add(PF);
                        }
                    }
                    BookingInfo.PB_Modify_Date = con.ConvertTimeZone(PMD.PB_TimeZone,Convert.ToDateTime(PMD.PB_Modify_Date));                    
                    BookingInfo.PB_Forms = FormsList;
                    BookingInfo.PB_Is_Deleted = false;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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

        [Route("API/Booking/GetBookingList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetBookingList(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<MT_Patient_Booking> patilist = new List<MT_Patient_Booking>();
                Query ObjQuery;
                if (PMD.PB_Booking_Surgery_Center_ID == "0")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false);
                }
                else if (PMD.PB_Booking_Surgery_Center_ID != "0" && PMD.PB_Booking_Surgery_Center_ID != null)
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Surgery_Center_ID", PMD.PB_Booking_Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID);
                }

                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (PMD.PB_Booking_Physician_Office_ID != null)
                        {
                            patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
                        }
                        else if (PMD.PB_Booking_Surgery_Center_ID == "0")
                        {
                            patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
                        }
                        else 
                        {
                            if (Docsnapshot.ConvertTo<MT_Patient_Booking>().PB_Status != "Draft")
                            {
                                patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
                            }
                        }
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

        [Route("API/Booking/GetBookingListFilterWithPO")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetBookingListFilterWithPO(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<MT_Patient_Booking> patilist = new List<MT_Patient_Booking>();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID).OrderByDescending("PB_Booking_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
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

        [Route("API/Booking/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Patient_Booking Pbooking = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking")
                    .WhereEqualTo("PB_Is_Deleted", false)
                    .WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);
                    //.WhereEqualTo("DU_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null && ObjQuerySnap.Documents.Count>0)
                {
                    Response.Data = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
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

        [Route("API/Booking/UpdateBookingStatus")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateBookingStatus(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Incident_Details InciDetails = new Incident_Details();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID).WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    BookingInfo.PB_Modify_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date));
                    BookingInfo.PB_Status = PMD.PB_Status;
                    BookingInfo.PB_Reason = PMD.PB_Reason;

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(BookingInfo, SetOptions.Overwrite);
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

        [Route("API/Booking/GetBookingCount")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetBookingCount(string Surgery_Center_ID, string Office_Type,string Slug)
        {
            Db = con.SurgeryCenterDb(Slug);
            CountResponse Response = new CountResponse();
            try
            {
                Incident_Details InciDetails = new Incident_Details();
                MT_Patient_Booking BookingInfo = new MT_Patient_Booking();

                //Approved Count
                Query ObjQuery;
                QuerySnapshot ObjQuerySnap;
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Approved").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Approved").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Approved");
                }
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Approved = ObjQuerySnap.Count;
                //Action Required

                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Action Required");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Action_Required = ObjQuerySnap.Count;
                //In Review
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Action Required");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.In_Review = ObjQuerySnap.Count;
                //Reject
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Rejected").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Rejected").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Rejected");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Rejected = ObjQuerySnap.Count;
                //Draft
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Draft").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Draft").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Draft");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Draft = ObjQuerySnap.Count;
                //Total

                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false);
                }
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Total = ObjQuerySnap.Count;

                //InComplete

                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Incomplete").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Incomplete").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Incomplete");
                }
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.InComplete = ObjQuerySnap.Count;

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

        [Route("API/Booking/AddBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddBooking(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                //Main section 
                UniqueID = con.GetUniqueKey();
                PMD.PB_Unique_ID = UniqueID;
                //List<CPTSelected> CPTSelList = new List<CPTSelected>();
                //List<ICDSelected> ICDSelList = new List<ICDSelected>();
                //List<ProcedureSelected> ProcSelList = new List<ProcedureSelected>();
                List<Alerts> AlrtList = new List<Alerts>();
                Query ObjQuery = Db.Collection("MT_Patient_Booking");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PMD.PB_Booking_No = "BKN000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    PMD.PB_Booking_No = "BKN000" + "1";
                }
                PMD.PB_Booking_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Booking_Date));
                PMD.PB_Create_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Create_Date));
                PMD.PB_Modify_Date = con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date));
                PMD.PB_Is_Deleted = false;
                //Main section Edit

                //Incident_Details

                if (PMD.PB_Incient_Detail != null)
                {
                    PMD.PB_Incient_Detail.Inci_Unique_ID = con.GetUniqueKey();
                    PMD.PB_Incient_Detail.Inci_DOI = con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_DOI));
                    PMD.PB_Incient_Detail.Inci_Create_Date = con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Create_Date));
                    PMD.PB_Incient_Detail.Inci_Modify_Date = con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Modify_Date));
                    PMD.PB_Incient_Detail.Inci_TimeZone = PMD.PB_Incient_Detail.Inci_TimeZone;
                }

                //Incident_Details End


                //Surgical_Procedure_Information

                if (PMD.PB_Surgical_Procedure_Information != null)
                {
                    PMD.PB_Surgical_Procedure_Information.SPI_Unique_ID = con.GetUniqueKey();
                    PMD.PB_Surgical_Procedure_Information.SPI_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date));
                    PMD.PB_Surgical_Procedure_Information.SPI_Create_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Create_Date));
                    PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date));
                    PMD.PB_Surgical_Procedure_Information.SPI_TimeZone = PMD.PB_Surgical_Procedure_Information.SPI_TimeZone;

                    //if (PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList != null)
                    //{
                    //    foreach (CPTSelected Sel in PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList)
                    //    {
                    //        CPTSelected cptsel = Sel;
                    //        cptsel.CPTS_Unique_ID = con.GetUniqueKey();
                    //        //cptsel.CPTS_Create_Date = con.ConvertTimeZone(cptsel.CPTS_TimeZone, Convert.ToDateTime(cptsel.CPTS_Create_Date));
                    //        //cptsel.CPTS_Modify_Date = con.ConvertTimeZone(cptsel.CPTS_TimeZone, Convert.ToDateTime(cptsel.CPTS_Modify_Date));
                    //        CPTSelList.Add(cptsel);
                    //    }
                    //}
                    

                    //if (PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList != null)
                    //{
                    //    foreach (ICDSelected Sel in PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList)
                    //    {
                    //        ICDSelected icdsel = Sel;
                    //        icdsel.ICD_Unique_ID = con.GetUniqueKey();
                    //        //icdsel.ICD_Create_Date = con.ConvertTimeZone(icdsel.ICD_TimeZone, Convert.ToDateTime(icdsel.ICD_Create_Date));
                    //        //icdsel.ICD_Modify_Date = con.ConvertTimeZone(icdsel.ICD_TimeZone, Convert.ToDateTime(icdsel.ICD_Modify_Date));
                    //        ICDSelList.Add(icdsel);
                    //    }
                    //}

                    //if (PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList.len != null)
                    //{
                    //    foreach (ProcedureSelected Sel in PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList)
                    //    {
                    //        ProcedureSelected Procsel = Sel;
                    //        Procsel.Proc_Unique_ID = con.GetUniqueKey();
                    //        //Procsel.Proc_Create_Date = con.ConvertTimeZone(Procsel.Proc_TimeZone, Convert.ToDateTime(Procsel.Proc_Create_Date));
                    //        //Procsel.Proc_Modify_Date = con.ConvertTimeZone(Procsel.Proc_TimeZone, Convert.ToDateTime(Procsel.Proc_Modify_Date));
                    //        ProcSelList.Add(Procsel);
                    //    }
                    //}

                    //PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList = ICDSelList;
                    //PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList = CPTSelList;
                    //PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList = ProcSelList;
                    PMD.PB_Booking_Surgery_Center_ID = PMD.PB_Booking_Surgery_Center_ID;
                    PMD.PB_Booking_Surgery_Center_Name = PMD.PB_Booking_Surgery_Center_Name;
                    PMD.PB_TimeZone = PMD.PB_TimeZone;
                    PMD.PB_Booking_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date));
                    PMD.PB_Booking_Duration = PMD.PB_Surgical_Procedure_Information.SPI_Duration;
                    PMD.PB_Booking_Time = PMD.PB_Surgical_Procedure_Information.SPI_Time;
                    
                }

                //Surgical_Procedure_Information End


                //PreoperativeMedicalClearance

                if (PMD.PB_Preoprative_Medical_Clearance != null) 
                {
                    PMD.PB_Preoprative_Medical_Clearance.PMC_Unique_ID = con.GetUniqueKey();
                    PMD.PB_Preoprative_Medical_Clearance.PMC_Create_Date = con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone, Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Create_Date));
                    PMD.PB_Preoprative_Medical_Clearance.PMC_Modify_Date = con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone, Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Modify_Date));
                    PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone = PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone;
                    
                }
                //PreoperativeMedicalClearance End


                //Special_Request

                if (PMD.PB_Special_Request != null) 
                {
                    PMD.PB_Special_Request.SR_Unique_ID = con.GetUniqueKey();
                    PMD.PB_Special_Request.SR_Create_Date = con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone, Convert.ToDateTime(PMD.PB_Special_Request.SR_Create_Date));
                    PMD.PB_Special_Request.SR_Modify_Date = con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone, Convert.ToDateTime(PMD.PB_Special_Request.SR_Modify_Date));
                    PMD.PB_Special_Request.SR_TimeZone = PMD.PB_Special_Request.SR_TimeZone;

                }

                //Special_Request End

                //InsurancePrecertificationAuthorization 

                if (PMD.PB_Insurance_Precertification_Authorization != null)
                {
                    PMD.PB_Insurance_Precertification_Authorization.IPA_Unique_ID = con.GetUniqueKey();
                    PMD.PB_Insurance_Precertification_Authorization.IPA_DOA = con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_DOA));
                    PMD.PB_Insurance_Precertification_Authorization.IPA_Create_Date = con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Create_Date));
                    PMD.PB_Insurance_Precertification_Authorization.IPA_Modify_Date = con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Modify_Date));
                    PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone = PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone;
                }

                //InsurancePrecertificationAuthorization End


                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(UniqueID);
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

        [Route("API/Booking/EditBooking")]
        [HttpPost]        
        public async Task<HttpResponseMessage> EditBooking(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Booking_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Booking_Date))},
                        {"PB_Booking_Time", PMD.PB_Booking_Time},
                        {"PB_Booking_Duration", PMD.PB_Booking_Duration},
                        {"PB_Alerts", PMD.PB_Alerts},
                        {"PB_Booking_Surgery_Center_ID", PMD.PB_Booking_Surgery_Center_ID},
                        {"PB_Booking_Surgery_Center_Name", PMD.PB_Booking_Surgery_Center_Name},
                        {"PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID},
                        {"PB_Booking_Physician_Office_Name", PMD.PB_Booking_Physician_Office_Name},
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_Status", PMD.PB_Status},
                        {"PB_TimeZone", PMD.PB_TimeZone},
                    };

                if (PMD.PB_Incient_Detail != null)
                {
                    Dictionary<string, object> Incient_Detail = new Dictionary<string, object>
                    {
                        {"Inci_Type_ID",PMD.PB_Incient_Detail.Inci_Type_ID},
                        {"Inci_Type",PMD.PB_Incient_Detail.Inci_Type},
                        {"Inci_Claim_No",PMD.PB_Incient_Detail.Inci_Claim_No},
                        {"Inci_DOI",con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone, Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_DOI))},
                        {"Inci_Employee_Name",PMD.PB_Incient_Detail.Inci_Employee_Name},
                        {"Inci_Employee_Address",PMD.PB_Incient_Detail.Inci_Employee_Address},
                        {"Inci_Employee_Phone_No",PMD.PB_Incient_Detail.Inci_Employee_Phone_No},
                        {"Inci_Is_This_Lien",PMD.PB_Incient_Detail.Inci_Is_This_Lien},
                        {"Inci_Attorney_Name",PMD.PB_Incient_Detail.Inci_Attorney_Name},
                        {"Inci_Attorney_Phone_No",PMD.PB_Incient_Detail.Inci_Attorney_Phone_No},
                        {"Inci_TimeOfIncident",PMD.PB_Incient_Detail.Inci_TimeOfIncident},
                        {"Inci_Comment",PMD.PB_Incient_Detail.Inci_Comment},
                        {"Inci_Modify_Date",con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone,Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Modify_Date))},
                        {"Inci_TimeZone",PMD.PB_Incient_Detail.Inci_TimeZone}
                    };
                    initialData.Add("PB_Incient_Detail", Incient_Detail);
                }

                if (PMD.PB_Surgical_Procedure_Information != null)
                {
                    Dictionary<string, object> Surgical_Procedure_Information = new Dictionary<string, object>
                    {
                        {"SPI_Surgery_Center_ID", PMD.PB_Surgical_Procedure_Information.SPI_Surgery_Center_ID},
                        {"SPI_Surgery_Center_Name", PMD.PB_Surgical_Procedure_Information.SPI_Surgery_Center_Name},
                        {"SPI_Surgeon_ID", PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_ID},
                        {"SPI_Surgeon_Name", PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_Name},
                        {"SPI_Assi_Surgeon_ID", PMD.PB_Surgical_Procedure_Information.SPI_Assi_Surgeon_ID},
                        {"SPI_Assi_Surgeon_Name", PMD.PB_Surgical_Procedure_Information.SPI_Assi_Surgeon_Name},
                        {"SPI_Date", con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone,Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date))},
                        {"SPI_Time", PMD.PB_Surgical_Procedure_Information.SPI_Time},
                        {"SPI_Duration", PMD.PB_Surgical_Procedure_Information.SPI_Duration},
                        {"SPI_Anesthesia_Type_ID", PMD.PB_Surgical_Procedure_Information.SPI_Anesthesia_Type_ID},
                        {"SPI_Anesthesia_Type", PMD.PB_Surgical_Procedure_Information.SPI_Anesthesia_Type},
                        {"SPI_Block_ID", PMD.PB_Surgical_Procedure_Information.SPI_Block_ID},
                        {"SPI_Block_Type", PMD.PB_Surgical_Procedure_Information.        SPI_Block_Type},
                        {"SPI_Procedure_SelectedList", PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList},
                        {"SPI_CPT_SelectedList", PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList},
                        {"SPI_ICD_SelectedList", PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList},
                        {"SPI_Modify_Date", con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone,Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date))},
                        {"SPI_TimeZone", PMD.PB_Surgical_Procedure_Information.SPI_TimeZone}
                    };
                    initialData.Add("PB_Surgical_Procedure_Information", Surgical_Procedure_Information);
                }

                if (PMD.PB_Preoprative_Medical_Clearance != null)
                {
                    Dictionary<string, object> Preoprative_Medical_Clearance = new Dictionary<string, object>
                    {
                        {"PMC_Is_Require_Pre_Op_Medi_Clearance", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_Pre_Op_Medi_Clearance},
                        {"PMC_Clearing_Physician_Name", PMD.PB_Preoprative_Medical_Clearance.PMC_Clearing_Physician_Name},
                        {"PMC_Is_Require_EKG", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_EKG},
                        {"PMC_Modify_Date", con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone, Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Modify_Date))},
                        {"PMC_TimeZone", PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone}
                    };
                    initialData.Add("PB_Preoprative_Medical_Clearance", Preoprative_Medical_Clearance);

                }

                if (PMD.PB_Special_Request != null)
                {
                    Dictionary<string, object> Special_Request = new Dictionary<string, object>
                    {
                        {"SR_Is_Special_Equip_Req", PMD.PB_Special_Request.SR_Is_Special_Equip_Req},
                        {"SR_Equip_ID", PMD.PB_Special_Request.SR_Equip_ID},
                        {"SR_Equip_Name", PMD.PB_Special_Request.SR_Equip_Name},
                        {"SR_Supplies_ID", PMD.PB_Special_Request.SR_Supplies_ID},
                        {"SR_Supplies_Name", PMD.PB_Special_Request.SR_Supplies_Name},
                        {"SR_Instrumentation_ID", PMD.PB_Special_Request.SR_Instrumentation_ID},
                        {"SR_Instrumentation_Name", PMD.PB_Special_Request.SR_Instrumentation_Name},
                        {"SR_Other", PMD.PB_Special_Request.SR_Other},
                        {"SR_Modify_Date", con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone, Convert.ToDateTime(PMD.PB_Special_Request.SR_Modify_Date))},
                        {"SR_TimeZone", PMD.PB_Special_Request.SR_TimeZone}
                    };
                    initialData.Add("PB_Special_Request", Special_Request);
                }

                if (PMD.PB_Insurance_Precertification_Authorization != null)
                {
                    Dictionary<string, object> Insurance_Precertification_Authorization = new Dictionary<string, object>
                    {
                        {"IPA_Insurace_Company_Phone_No", PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Phone_No},
                        {"IPA_Insurace_Company_Representative", PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Representative},
                        {"IPA_Authorization_Name", PMD.PB_Insurance_Precertification_Authorization.IPA_Authorization_Name},
                        {"IPA_DOA", con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_DOA))},
                        {"IPA_Modify_Date", con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Modify_Date))},
                        {"IPA_TimeZone", PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone}
                    };
                    initialData.Add("PB_Insurance_Precertification_Authorization", Insurance_Precertification_Authorization);
                }
                //Main section 
                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(PMD.PB_Unique_ID);
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
        [Route("API/Booking/DownloadPDF")]
        [HttpGet]
        //[Obsolete]
        public async Task<HttpResponseMessage> DownloadPDF(string BookingID,string PatientID,string Slug)
        {
            string HTMLContent = "<html>";
            Db = con.SurgeryCenterDb(Slug);
            Query ObjQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PatientID).WhereEqualTo("Patient_Is_Active",true).WhereEqualTo("Patient_Is_Deleted", false);
            QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
            if (ObjQuerySnap != null)
            {
                MT_PatientInfomation patient = ObjQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                HTMLContent = HTMLContent + "<body style='padding:10px 10px 10px 10px'>" +
                "<div id='Header' style='text-align: center;font-size: 12px;background: red;padding: 10px 0px 10px 0px;font-weight: bold;'>" +
                    "Clinical Summary" +
                "</div> " +
                "<div id='DemographicsSection' style='padding: 10px 10px 10px 10px;'>" +
                    //"<div  style = 'padding: 5px 0px 5px 5px;font-weight: bold;font-size: 14px'>" +
                    "<table bgcolor='#B8DBFD' border='1' bordercolor='LightGray'><tr><td>" +
                    "Demographics " +
                    "<tr><td></table>" +
                    //"</div>" +
                        "<table  style='width:100%;padding: 2px 0px 0px 0px;border: solid 1px lightgray;' >" +
                            "<tr>" +
                                "<td>Name :</td>" +
                                "<td>"+ patient.Patient_First_Name + " " + patient.Patient_Last_Name + "</td>" +
                                "<td>Patient ID :</td>" +
                                "<td>"+ patient.Patient_Code + "</td>" +
                            "</tr>" +

                            "<tr>" +
                                "<td>Birth Date :</td>" +
                                "<td>"+ patient.Patient_DOB + "</td>" +
                                "<td>Gender :</td>" +
                                "<td>"+ patient.Patient_Sex + "</td>" +
                            "</tr>" +

                            "<tr>" +
                                "<td>Marital Status :</td>" +
                                "<td>"+ patient.Patient_Marital_Status + "</td>" +
                                "<td>Religious :</td>" +
                                "<td>"+ patient.Patient_Religion + "</td>" +
                            "</tr>" +

                            "<tr>" +
                                "<td>Race :</td>" +
                                "<td>"+ patient.Patient_Race + "</td>" +
                                "<td>Ethinic :</td>" +
                                "<td>"+ patient.Patient_Ethinicity + "</td>" +
                            "</tr>" +

                            "<tr>" +
                                "<td>Contact Information :</td>" +
                                "<td> Tel(Primary Home)"+ patient.Patient_Primary_No + "</td>" +
                                "<td>Primary Home :</td>" +
                                "<td>" + patient.Patient_Address1 + "<br>"+ patient.Patient_Address2 + "</td>" +
                            "</tr>" +

                            "<tr>" +
                                "<td>Language :</td>" +
                                "<td>"+ patient.Patient_Language + "</td>" +
                                "<td></td>" +
                                "<td></td>" +
                            "</tr>" +
                        "</table>" +

                        //Query ObjQuery1 = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PatientID).WhereEqualTo("Patient_Is_Active", true).WhereEqualTo("Patient_Is_Deleted", false);
                        //QuerySnapshot ObjQuerySnap1 = await ObjQuery.GetSnapshotAsync();
                        //if (ObjQuerySnap != null)
                        //{ 

                        //}

                    "</div>";
            }
            HTMLContent = HTMLContent + "</body></html>";
            
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Set the File Path.
            //string filePath = HttpContext.Current.Server.MapPath("~/Images/") + "SCNavi_20191118194837.jpg";

            ////Check whether File exists.
            //if (!File.Exists(filePath))
            //{
            //    //Throw 404 (Not Found) exception if File not found.
            //    response.StatusCode = HttpStatusCode.NotFound;
            //    response.ReasonPhrase = string.Format("File not found: {0} .", "SCNavi_20191118194837.jpg");
            //    throw new HttpResponseException(response);
            //}

            //Read the File into a Byte Array.
            byte[] bytes = GetPDF(HTMLContent);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "123.pdf";

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping("123.pdf"));
            return response;
        }

        [Obsolete]
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }


    }
}
