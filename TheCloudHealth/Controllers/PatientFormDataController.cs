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
    public class PatientFormDataController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        ICreatePDF ObjPDF;
        string UniqueID = "";
        public PatientFormDataController()
        {
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

        [Route("API/PatientFormData/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Patient_Form_Data PFMD)
        {
            Db = con.SurgeryCenterDb(PFMD.Slug);
            PatientFormResponse Response = new PatientFormResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PFMD.PFD_Unique_ID = UniqueID;
                DocumentReference docRef = Db.Collection("MT_Patient_Form_Data").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PFMD);
                if (Result != null)
                {
                    MT_Patient_Booking BookingInfo = new MT_Patient_Booking();
                    MT_Staff_Members staffinfo = new MT_Staff_Members();
                    MT_System_EmailTemplates sysemail = new MT_System_EmailTemplates();
                    Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Active", true).WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Unique_ID", PFMD.PFD_Booking_ID);
                    QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {
                        BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                        ObjQuery = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Active", true).WhereEqualTo("Staff_Is_Deleted", false).WhereEqualTo("Staff_Unique_ID", BookingInfo.PB_Surgical_Procedure_Information.SPI_Surgeon_ID);
                        ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                        if (ObjQuerySnap != null)
                        {
                            staffinfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Staff_Members>();
                            ObjQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Active", true).WhereEqualTo("SET_Is_Deleted", false).WhereEqualTo("SET_Name", "Appointment Cancellation");
                            ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                            if (ObjQuerySnap != null)
                            {
                                sysemail = ObjQuerySnap.Documents[0].ConvertTo<MT_System_EmailTemplates>();
                                Email email = new Email();
                                if (sysemail.SET_Bcc != null && sysemail.SET_Bcc.Length > 0)
                                {
                                    email.Bcc_Email = sysemail.SET_Bcc.ToList();
                                }

                                if (sysemail.SET_CC != null && sysemail.SET_CC.Length > 0)
                                {
                                    email.Cc_Email = sysemail.SET_CC.ToList();
                                }
                                List<string> PatiEmail = new List<string>();
                                List<string> PatiName = new List<string>();
                                PatiEmail.Add(staffinfo.Staff_Email);
                                PatiName.Add(staffinfo.Staff_Name);
                                email.To_Email = PatiEmail;
                                email.To_Name = PatiName;
                                email.From_Name = sysemail.SET_From_Name;
                                email.From_Email = sysemail.SET_From_Email;
                                email.HtmlContent = (sysemail.SET_Header == null ? "" : sysemail.SET_Header)
                                    + "<br/>"
                                    + "<br/>"
                                    + sysemail.SET_Message.ToString().Replace("[FirstName]", BookingInfo.PB_Patient_Name)
                                    .Replace("[Dr Name]", BookingInfo.PB_Surgical_Procedure_Information == null ? "" : BookingInfo.PB_Surgical_Procedure_Information.SPI_Surgeon_Name)
                                    .Replace("[mm/dd/yy]", BookingInfo.PB_Booking_Date.ToString("mm/dd/yy"))
                                    .Replace("[hr:mn]", BookingInfo.PB_Booking_Time.ToString())
                                    .Replace("[FacilityName]", BookingInfo.PB_Booking_Surgery_Center_Name)
                                    + "<br/>"
                                    + "<br/>"
                                    + sysemail.SET_Footer.ToString().Replace("[FacilityName]", BookingInfo.PB_Booking_Surgery_Center_Name)
                                    .Replace("[Email]", (BookingInfo.PB_Patient_Email == null ? "" : BookingInfo.PB_Patient_Email));
                                email.Subject = "[FirstName], your appointment has been cancelled".Replace("[FirstName]", BookingInfo.PB_Patient_Name);
                                //var message = await ObjEmailer.Send(email);
                                Response.Status = con.StatusSuccess;
                                Response.Message = con.MessageSuccess + " Email Status : " + "";
                            }
                            else
                            {
                                Response.Status = con.StatusSuccess;
                                Response.Message = con.MessageSuccess;
                                Response.Data = PFMD;
                            }

                        }
                        else
                        {
                            Response.Status = con.StatusSuccess;
                            Response.Message = con.MessageSuccess;
                            Response.Data = PFMD;
                        }
                    }
                    else
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = PFMD;
                    }
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

        [Route("API/PatientFormData/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(MT_Patient_Form_Data PFMD)
        {
            Db = con.SurgeryCenterDb(PFMD.Slug);
            PatientFormResponse Response = new PatientFormResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                   //{"PFD_TimeZone" , PFMD.PFD_TimeZone},
                   {"PFD_Elements" , PFMD.PFD_Elements},
                   //{"PFD_Modify_Date" , con.ConvertTimeZone(PFMD.PFD_TimeZone,Convert.ToDateTime(PFMD.PFD_Modify_Date))}
                };
                
                DocumentReference docRef = Db.Collection("MT_Patient_Form_Data").Document(PFMD.PFD_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = PFMD;
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

        [Route("API/PatientFormData/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Patient_Form_Data PFMD)
        {
            Db = con.SurgeryCenterDb(PFMD.Slug);
            PatientFormResponse Response = new PatientFormResponse();
            try
            {
                List<MT_Patient_Form_Data> patilist = new List<MT_Patient_Form_Data>();
                Query ObjQuery = Db.Collection("MT_Patient_Form_Data").WhereEqualTo("PFD_Booking_ID", PFMD.PFD_Booking_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Form_Data>());
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

        [Route("API/PatientFormData/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select (MT_Patient_Form_Data PFMD)
        {
            Db = con.SurgeryCenterDb(PFMD.Slug);
            PatientFormResponse Response = new PatientFormResponse();
            try
            {
                MT_Patient_Form_Data PatientFormData = new MT_Patient_Form_Data();
                Query ObjQuery = Db.Collection("MT_Patient_Form_Data").WhereEqualTo("PFD_Unique_ID", PFMD.PFD_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PatientFormData = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Form_Data>();                    
                    Response.Data = PatientFormData;
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

        [Route("API/PatientFormData/GetFormData")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetFormData(MT_Patient_Form_Data PFMD)
        {
            Db = con.SurgeryCenterDb(PFMD.Slug);
            PatientFormResponse Response = new PatientFormResponse();
            try
            {
                MT_Patient_Form_Data PatientFormData = new MT_Patient_Form_Data();
                Query ObjQuery = Db.Collection("MT_Patient_Form_Data").WhereEqualTo("PFD_Booking_ID", PFMD.PFD_Booking_ID).WhereEqualTo("PFD_Form_ID", PFMD.PFD_Form_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PatientFormData = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Form_Data>();
                    Response.Data = PatientFormData;
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
