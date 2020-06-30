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
    public class QBBookingController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        //ICreatePDF ObjPDF;
        //string UniqueID = "";
        public QBBookingController()
        {
            con = new ConnectionClass();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/QBBooking/AddBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditBooking(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Patient_Booking booking = new MT_Patient_Booking();
                Query Qty = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID);
                QuerySnapshot ObjBooking = await Qty.GetSnapshotAsync();
                if (ObjBooking != null)
                {
                    booking = ObjBooking.Documents[0].ConvertTo<MT_Patient_Booking>();
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Patient_Name", PMD.PB_Patient_Name},
                        {"PB_Patient_Last_Name", PMD.PB_Patient_Last_Name},
                        {"PB_Booking_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Booking_Date))},
                        {"PB_Booking_Surgery_Center_ID",PMD.PB_Booking_Surgery_Center_ID },
                        {"PB_Booking_Surgery_Center_Name",PMD.PB_Booking_Surgery_Center_Name },
                        {"PB_Booking_Time", PMD.PB_Booking_Time},
                        {"PB_Booking_Duration", PMD.PB_Booking_Duration},
                        {"PB_Alerts", PMD.PB_Alerts},
                        {"PB_Notes", PMD.PB_Notes},
                        {"PB_Status", PMD.PB_Status},
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_TimeZone", PMD.PB_TimeZone},
                    };

                    if (PMD.PB_Incient_Detail != null)
                    {
                        Dictionary<string, object> Incient_Detail;
                        if (booking.PB_Incient_Detail != null)
                        {
                            Incient_Detail = new Dictionary<string, object>
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
                        }
                        else
                        {
                            Incient_Detail = new Dictionary<string, object>
                            {
                                {"Inci_Unique_ID", con.GetUniqueKey()},
                                {"Inci_Type_ID", PMD.PB_Incient_Detail.Inci_Type_ID},
                                {"Inci_Type", PMD.PB_Incient_Detail.Inci_Type},
                                {"Inci_Claim_No", PMD.PB_Incient_Detail.Inci_Claim_No},
                                {"Inci_DOI",con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone,Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_DOI))},
                                {"Inci_Employee_Name", PMD.PB_Incient_Detail.Inci_Employee_Name},
                                {"Inci_Employee_Address", PMD.PB_Incient_Detail.Inci_Employee_Address},
                                {"Inci_Employee_Phone_No", PMD.PB_Incient_Detail.Inci_Employee_Phone_No},
                                {"Inci_Is_This_Lien", PMD.PB_Incient_Detail.Inci_Is_This_Lien},
                                {"Inci_Attorney_Name", PMD.PB_Incient_Detail.Inci_Attorney_Name},
                                {"Inci_Attorney_Phone_No", PMD.PB_Incient_Detail.Inci_Attorney_Phone_No},
                                {"Inci_TimeOfIncident", PMD.PB_Incient_Detail.Inci_TimeOfIncident},
                                {"Inci_Comment", PMD.PB_Incient_Detail.Inci_Comment},
                                {"Inci_Created_By", PMD.PB_Incient_Detail.Inci_Created_By},
                                {"Inci_User_Name", PMD.PB_Incient_Detail.Inci_User_Name},
                                {"Inci_Create_Date", con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone,Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Create_Date))},
                                {"Inci_Modify_Date", con.ConvertTimeZone(PMD.PB_Incient_Detail.Inci_TimeZone,Convert.ToDateTime(PMD.PB_Incient_Detail.Inci_Modify_Date))},
                                {"Inci_Is_Active", PMD.PB_Incient_Detail.Inci_Is_Active},
                                {"Inci_Is_Deleted", PMD.PB_Incient_Detail.Inci_Is_Deleted},
                                {"Inci_TimeZone", PMD.PB_Incient_Detail.Inci_TimeZone},
                            };
                        }

                        initialData.Add("PB_Incient_Detail", Incient_Detail);
                    }

                    if (PMD.PB_Surgical_Procedure_Information != null)
                    {
                        Dictionary<string, object> Surgical_Procedure_Information;
                        if (booking.PB_Surgical_Procedure_Information != null)
                        {
                            Surgical_Procedure_Information = new Dictionary<string, object>
                            {
                                {"SPI_Surgery_Center_ID", PMD.PB_Surgical_Procedure_Information.SPI_Surgery_Center_ID},
                                {"SPI_Surgery_Center_Name", PMD.PB_Surgical_Procedure_Information.SPI_Surgery_Center_Name},
                                {"SPI_Surgeon_ID", PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_ID},
                                {"SPI_Surgeon_Name", PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_Name},
                                {"SPI_Assi_Surgeon_ID", PMD.PB_Surgical_Procedure_Information.SPI_Assi_Surgeon_ID},
                                {"SPI_Assi_Surgeon_Name", PMD.PB_Surgical_Procedure_Information.SPI_Assi_Surgeon_Name},
                                {"SPI_Date", con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone,Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date),1)},
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
                        }
                        else
                        {
                            Surgical_Procedure_Information = new Dictionary<string, object>
                            {
                                {"SPI_Unique_ID", con.GetUniqueKey()},
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
                                {"SPI_Block_Type", PMD.PB_Surgical_Procedure_Information.SPI_Block_Type},
                                {"SPI_Procedure_SelectedList", PMD.PB_Surgical_Procedure_Information.SPI_Procedure_SelectedList},
                                {"SPI_CPT_SelectedList", PMD.PB_Surgical_Procedure_Information.SPI_CPT_SelectedList},
                                {"SPI_ICD_SelectedList", PMD.PB_Surgical_Procedure_Information.SPI_ICD_SelectedList},
                                {"SPI_Created_By", PMD.PB_Surgical_Procedure_Information.SPI_Created_By},
                                {"SPI_User_Name", PMD.PB_Surgical_Procedure_Information.SPI_User_Name},
                                {"SPI_Create_Date", con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone,Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Create_Date))},
                                {"SPI_Modify_Date", con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone,Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date))},
                                {"SPI_Is_Active", PMD.PB_Surgical_Procedure_Information.SPI_Is_Active},
                                {"SPI_Is_Deleted", PMD.PB_Surgical_Procedure_Information.SPI_Is_Deleted},
                                {"SPI_TimeZone", PMD.PB_Surgical_Procedure_Information.SPI_TimeZone},
                            };
                        }

                        initialData.Add("PB_Surgical_Procedure_Information", Surgical_Procedure_Information);
                    }

                    if (PMD.PB_Preoprative_Medical_Clearance != null)
                    {
                        Dictionary<string, object> Preoprative_Medical_Clearance;
                        if (booking.PB_Preoprative_Medical_Clearance != null)
                        {
                            Preoprative_Medical_Clearance = new Dictionary<string, object>
                            {
                                {"PMC_Is_Require_Pre_Op_Medi_Clearance", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_Pre_Op_Medi_Clearance},
                                {"PMC_Clearing_Physician_Name", PMD.PB_Preoprative_Medical_Clearance.PMC_Clearing_Physician_Name},
                                {"PMC_Is_Require_EKG", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_EKG},
                                {"PMC_Modify_Date", con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone, Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Modify_Date))},
                                {"PMC_TimeZone", PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone}
                            };
                        }
                        else
                        {
                            Preoprative_Medical_Clearance = new Dictionary<string, object>
                            {
                                {"PMC_Unique_ID", con.GetUniqueKey()},
                                {"PMC_Is_Require_Pre_Op_Medi_Clearance", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_Pre_Op_Medi_Clearance},
                                {"PMC_Clearing_Physician_Name", PMD.PB_Preoprative_Medical_Clearance.PMC_Clearing_Physician_Name},
                                {"PMC_Is_Require_EKG", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Require_EKG},
                                {"PMC_Created_By", PMD.PB_Preoprative_Medical_Clearance.PMC_Created_By},
                                {"PMC_User_Name", PMD.PB_Preoprative_Medical_Clearance.PMC_User_Name},
                                {"PMC_Create_Date",con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone,Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Create_Date))},
                                {"PMC_Modify_Date", con.ConvertTimeZone(PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone,Convert.ToDateTime(PMD.PB_Preoprative_Medical_Clearance.PMC_Modify_Date))},
                                {"PMC_Is_Active", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Active},
                                {"PMC_Is_Deleted", PMD.PB_Preoprative_Medical_Clearance.PMC_Is_Deleted},
                                {"PMC_TimeZone", PMD.PB_Preoprative_Medical_Clearance.PMC_TimeZone},
                            };
                        }

                        initialData.Add("PB_Preoprative_Medical_Clearance", Preoprative_Medical_Clearance);

                    }

                    if (PMD.PB_Special_Request != null)
                    {
                        Dictionary<string, object> Special_Request;
                        if (booking.PB_Special_Request != null)
                        {
                            Special_Request = new Dictionary<string, object>
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
                        }
                        else
                        {
                            Special_Request = new Dictionary<string, object>
                            {
                                {"SR_Unique_ID",con.GetUniqueKey()},
                                {"SR_Is_Special_Equip_Req",PMD.PB_Special_Request.SR_Is_Special_Equip_Req},
                                {"SR_Equip_ID",PMD.PB_Special_Request.SR_Equip_ID},
                                {"SR_Equip_Name",PMD.PB_Special_Request.SR_Equip_Name},
                                {"SR_Supplies_ID",PMD.PB_Special_Request.SR_Supplies_ID},
                                {"SR_Supplies_Name",PMD.PB_Special_Request.SR_Supplies_Name},
                                {"SR_Instrumentation_ID",PMD.PB_Special_Request.SR_Instrumentation_ID},
                                {"SR_Instrumentation_Name",PMD.PB_Special_Request.SR_Instrumentation_Name},
                                {"SR_Other",PMD.PB_Special_Request.SR_Other},
                                {"SR_Created_By",PMD.PB_Special_Request.SR_Created_By},
                                {"SR_User_Name",PMD.PB_Special_Request.SR_User_Name},
                                {"SR_Create_Date",con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone,Convert.ToDateTime(PMD.PB_Special_Request.SR_Create_Date))},
                                {"SR_Modify_Date",con.ConvertTimeZone(PMD.PB_Special_Request.SR_TimeZone,Convert.ToDateTime(PMD.PB_Special_Request.SR_Modify_Date))},
                                {"SR_Is_Active",PMD.PB_Special_Request.SR_Is_Active},
                                {"SR_Is_Deleted",PMD.PB_Special_Request.SR_Is_Deleted},
                                {"SR_TimeZone",PMD.PB_Special_Request.SR_TimeZone},
                            };
                        }

                        initialData.Add("PB_Special_Request", Special_Request);
                    }

                    if (PMD.PB_Insurance_Precertification_Authorization != null)
                    {
                        Dictionary<string, object> Insurance_Precertification_Authorization;
                        if (booking.PB_Insurance_Precertification_Authorization != null)
                        {
                            Insurance_Precertification_Authorization = new Dictionary<string, object>
                            {
                                {"IPA_Insurace_Company_Phone_No", PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Phone_No},
                                {"IPA_Insurace_Company_Representative", PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Representative},
                                {"IPA_Authorization_Name", PMD.PB_Insurance_Precertification_Authorization.IPA_Authorization_Name},
                                {"IPA_DOA", con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_DOA))},
                                {"IPA_Modify_Date", con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone, Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Modify_Date))},
                                {"IPA_TimeZone", PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone}
                            };
                        }
                        else
                        {
                            Insurance_Precertification_Authorization = new Dictionary<string, object>
                            {
                                {"IPA_Unique_ID",con.GetUniqueKey()},
                                {"IPA_Insurace_Company_Phone_No",PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Phone_No},
                                {"IPA_Insurace_Company_Representative",PMD.PB_Insurance_Precertification_Authorization.IPA_Insurace_Company_Representative},
                                {"IPA_Authorization_Name",PMD.PB_Insurance_Precertification_Authorization.IPA_Authorization_Name},
                                {"IPA_DOA",con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone,Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_DOA))},
                                {"IPA_Created_By",PMD.PB_Insurance_Precertification_Authorization.IPA_Created_By},
                                {"IPA_User_Name",PMD.PB_Insurance_Precertification_Authorization.IPA_User_Name},
                                {"IPA_Create_Date",con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone,Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Create_Date))},
                                {"IPA_Modify_Date",con.ConvertTimeZone(PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone,Convert.ToDateTime(PMD.PB_Insurance_Precertification_Authorization.IPA_Modify_Date))},
                                {"IPA_Is_Active",PMD.PB_Insurance_Precertification_Authorization.IPA_Is_Active},
                                {"IPA_Is_Deleted",PMD.PB_Insurance_Precertification_Authorization.IPA_Is_Deleted},
                                {"IPA_TimeZone",PMD.PB_Insurance_Precertification_Authorization.IPA_TimeZone},
                            };
                        }

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
