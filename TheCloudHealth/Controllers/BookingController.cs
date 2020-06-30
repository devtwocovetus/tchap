using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TheCloudHealth.Lib;
using TheCloudHealth.Models;

namespace TheCloudHealth.Controllers
{
    public class BookingController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        ICreatePDF ObjPDF;
        IEmailer ObjEmailer;
        string UniqueID = "";        
        public BookingController() {
            con = new ConnectionClass();
            ObjPDF = new CreatePDF();
            ObjEmailer = new Emailer();
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
                PMD.PB_Unique_ID = UniqueID;
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
                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PMD);
                if (Result != null)
                {
                    MT_System_EmailTemplates sysemail = new MT_System_EmailTemplates();
                    Email email = new Email();
                    ObjQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Active", true).WhereEqualTo("SET_Is_Deleted", false).WhereEqualTo("SET_Name", "Appointment confirmation");
                    ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {
                        sysemail = ObjQuerySnap.Documents[0].ConvertTo<MT_System_EmailTemplates>();

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
                        PatiEmail.Add(PMD.PB_Patient_Email);
                        PatiName.Add(PMD.PB_Patient_Name);
                        email.To_Email = PatiEmail;
                        email.To_Name = PatiName;
                        email.From_Name = sysemail.SET_From_Name;
                        email.From_Email = sysemail.SET_From_Email;
                        email.HtmlContent = (sysemail.SET_Header == null ? "" : sysemail.SET_Header)
                            + "<br/>"
                            + "<br/>"
                            + sysemail.SET_Message.ToString().Replace("[FirstName]", PMD.PB_Patient_Name)
                            .Replace("[Dr Name]", PMD.PB_Surgical_Procedure_Information == null ? "" : PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_Name)
                            .Replace("[mm/dd/yy]", PMD.PB_Booking_Date.ToString("mm/dd/yy"))
                            .Replace("[hr:mn]", PMD.PB_Booking_Time.ToString())
                            .Replace("[FacilityName]", PMD.PB_Booking_Surgery_Center_Name)
                            + "<br/>"
                            + "<br/>"
                            + sysemail.SET_Footer.ToString().Replace("[FacilityName]", PMD.PB_Booking_Surgery_Center_Name);
                        email.Subject = "Appointment has been scheduled";
                        var message = await ObjEmailer.Send(email);
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess + " Email Status : " + message;
                    }
                    else
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                    }
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

        public MT_Patient_Booking GetImages()
        {
            MT_Patient_Booking Surcenter = new MT_Patient_Booking();
            try
            {
                List<Doc_Uploaded> DocUplodList = new List<Doc_Uploaded>();

                //string[] DBPath;
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

                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone,Convert.ToDateTime(PMD.PB_Modify_Date))},
                        { "PB_TimeZone",PMD.PB_TimeZone},
                        {"PB_Forms", FormsList}
                    };

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
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

        [Route("API/Booking/GetPatientBookingList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetPatientBookingList(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<MT_Patient_Booking> patilist = new List<MT_Patient_Booking>();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID).WhereEqualTo("PB_Patient_ID", PMD.PB_Patient_ID).OrderByDescending("PB_Booking_Date");
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

        [Route("API/Booking/GetPatientBookingStatusList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetPatientBookingStatusList(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<MT_Patient_Booking> BookingList = new List<MT_Patient_Booking>();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID).WhereEqualTo("PB_Patient_ID", PMD.PB_Patient_ID).OrderByDescending("PB_Booking_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        BookingList.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
                    }
                    Response.DataList = BookingList;
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
                            if (Docsnapshot.ConvertTo<MT_Patient_Booking>().PB_Status == "Draft" || Docsnapshot.ConvertTo<MT_Patient_Booking>().PB_Status == "Action Required")
                            {
                                patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
                            }                            
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
                    
                    Response.DataList = patilist.OrderBy(o => o.PB_Surgical_Procedure_Information.SPI_Date).ToList();
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

        [Route("API/Booking/GetQuickBookingList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetQuickBookingList(MT_Patient_Booking PMD)
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
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "QB").WhereEqualTo("PB_Booking_Surgery_Center_ID", PMD.PB_Booking_Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "QB").WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID);
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

        [Route("API/Booking/GetWalkInBookingList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetWalkInBookingList(MT_Patient_Booking PMD)
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
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "WK").WhereEqualTo("PB_Booking_Surgery_Center_ID", PMD.PB_Booking_Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "WK").WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID);
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
        [HttpGet]
        public async Task<HttpResponseMessage> GetBookingListFilterWithPO(string SurgeryPhysicianID, string OfficeType, string Status, string Slug)
        {
            Db = con.SurgeryCenterDb(Slug);
            BookingResponse Response = new BookingResponse();
            Query ObjQuery;
            QuerySnapshot ObjQuerySnap;
            try
            {
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                List<MT_Patient_Booking> patilist = new List<MT_Patient_Booking>();
                if (OfficeType == "P")
                {
                    switch (Status)
                    {
                        case "Total":
                            ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Physician_Office_ID", SurgeryPhysicianID);//.OrderByDescending("PB_Booking_Date");
                            break;
                        default:
                            ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Physician_Office_ID", SurgeryPhysicianID).WhereEqualTo("PB_Status", Status);
                            break;
                    }
                    ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {
                        foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                        {
                            Booking = Docsnapshot.ConvertTo<MT_Patient_Booking>();
                            if (Booking.PB_Status != "Mark As Completed")
                            {
                                patilist.Add(Booking);
                            }
                            
                        }
                        Response.DataList = patilist.OrderBy(o => o.PB_Surgical_Procedure_Information.SPI_Date).ToList();
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                    }
                    else
                    {
                        Response.Status = con.StatusDNE;
                        Response.Message = con.MessageDNE;
                    }
                }
                else if (OfficeType == "S")
                {
                    switch (Status)
                    {
                        case "Total":
                            ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Surgery_Center_ID", SurgeryPhysicianID);//.OrderByDescending("PB_Booking_Date");
                            break;
                        default:
                            ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Surgery_Center_ID", SurgeryPhysicianID).WhereEqualTo("PB_Status", Status);
                            break;
                    }
                    ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {
                        foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                        {
                            Booking = Docsnapshot.ConvertTo<MT_Patient_Booking>();
                            if (Booking.PB_Status != "Draft" && Booking.PB_Status!= "Mark As Completed")
                            {
                                patilist.Add(Docsnapshot.ConvertTo<MT_Patient_Booking>());
                            }
                        }
                        Response.DataList = patilist.OrderBy(o => o.PB_Surgical_Procedure_Information.SPI_Date).ToList();
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                    }
                    else
                    {
                        Response.Status = con.StatusDNE;
                        Response.Message = con.MessageDNE;
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

        [Route("API/Booking/GetCompletedBookingList")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetCompletedBookingList(MT_Patient_Booking PMD)
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
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Surgery_Center_ID", PMD.PB_Booking_Surgery_Center_ID).WhereEqualTo("PB_Status", "Mark As Completed");
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID).WhereEqualTo("PB_Status", "Mark As Completed");
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
                List<Reason> RejectList = new List<Reason>();
                List<Reason> ApprovedList = new List<Reason>();
                List<Reason> DraftList = new List<Reason>();
                List<Reason> IncompleteList = new List<Reason>();
                List<Reason> CompleteList = new List<Reason>();
                List<Reason> NoshowList = new List<Reason>();
                List<Reason> SuspendList = new List<Reason>();
                List<Reason> UnapprovedList = new List<Reason>();

                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID).WhereEqualTo("PB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Dictionary<string, object> initialData;
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();                    
                    if (BookingInfo.PB_Reject != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Reject)
                        {
                            RejectList.Add(RR);
                        }
                    }

                    if (PMD.PB_Reject != null)
                    {
                        foreach (Reason RR in PMD.PB_Reject)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            RejectList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Approved != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Approved)
                        {
                            ApprovedList.Add(RR);
                        }
                    }

                    if (PMD.PB_Approved != null)
                    {
                        foreach (Reason RR in PMD.PB_Approved)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            ApprovedList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Draft != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Draft)
                        {
                            DraftList.Add(RR);
                        }
                    }

                    if (PMD.PB_Draft != null)
                    {
                        foreach (Reason RR in PMD.PB_Draft)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            DraftList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Incomplete != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Incomplete)
                        {
                            IncompleteList.Add(RR);
                        }
                    }

                    if (PMD.PB_Incomplete != null)
                    {
                        foreach (Reason RR in PMD.PB_Incomplete)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            IncompleteList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Complete != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Complete)
                        {
                            CompleteList.Add(RR);
                        }
                    }

                    if (PMD.PB_Complete != null)
                    {
                        foreach (Reason RR in PMD.PB_Complete)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            CompleteList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Noshow != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Noshow)
                        {
                            NoshowList.Add(RR);
                        }
                    }

                    if (PMD.PB_Noshow != null)
                    {
                        foreach (Reason RR in PMD.PB_Noshow)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            NoshowList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Suspended != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Suspended)
                        {
                            SuspendList.Add(RR);
                        }
                    }

                    if (PMD.PB_Suspended != null)
                    {
                        foreach (Reason RR in PMD.PB_Suspended)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            SuspendList.Add(RR);
                        }
                    }

                    if (BookingInfo.PB_Unapproved != null)
                    {
                        foreach (Reason RR in BookingInfo.PB_Unapproved)
                        {
                            UnapprovedList.Add(RR);
                        }
                    }

                    if (PMD.PB_Unapproved != null)
                    {
                        foreach (Reason RR in PMD.PB_Unapproved)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            UnapprovedList.Add(RR);
                        }
                    }



                    initialData = new Dictionary<string, object>
                    {
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_Reject", RejectList},
                        {"PB_Approved", ApprovedList},
                        {"PB_Draft", DraftList},
                        {"PB_Incomplete", IncompleteList},
                        {"PB_Complete", CompleteList},
                        {"PB_Noshow", NoshowList},
                        {"PB_Suspended", SuspendList},
                        {"PB_Unapproved", UnapprovedList},
                        {"PB_Status",PMD.PB_Status },
                        {"PB_TimeZone",PMD.PB_TimeZone}
                    };
                    

                    DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(BookingInfo.PB_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        if (PMD.PB_Status == "Cancelled")
                        {
                            MT_System_EmailTemplates sysemail = new MT_System_EmailTemplates();
                            Email email = new Email();
                            ObjQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Active", true).WhereEqualTo("SET_Is_Deleted", false).WhereEqualTo("SET_Name", "Appointment Cancellation");
                            ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                            if (ObjQuerySnap != null)
                            {
                                sysemail = ObjQuerySnap.Documents[0].ConvertTo<MT_System_EmailTemplates>();

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
                                PatiEmail.Add(PMD.PB_Patient_Email);
                                PatiName.Add(PMD.PB_Patient_Name);
                                email.To_Email = PatiEmail;
                                email.To_Name = PatiName;
                                email.From_Name = sysemail.SET_From_Name;
                                email.From_Email = sysemail.SET_From_Email;
                                email.HtmlContent = (sysemail.SET_Header == null ? "" : sysemail.SET_Header)
                                    + "<br/>"
                                    + "<br/>"
                                    + sysemail.SET_Message.ToString().Replace("[FirstName]", PMD.PB_Patient_Name)
                                    .Replace("[Dr Name]", PMD.PB_Surgical_Procedure_Information == null ? "" : PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_Name)
                                    .Replace("[mm/dd/yy]", PMD.PB_Booking_Date.ToString("mm/dd/yy"))
                                    .Replace("[hr:mn]", PMD.PB_Booking_Time.ToString())
                                    .Replace("[FacilityName]", PMD.PB_Booking_Surgery_Center_Name)
                                    + "<br/>"
                                    + "<br/>"
                                    + sysemail.SET_Footer.ToString().Replace("[FacilityName]", PMD.PB_Booking_Surgery_Center_Name)
                                    .Replace("[Email]",(PMD.PB_Patient_Email==null?"":PMD.PB_Patient_Email));
                                email.Subject = "[FirstName], your appointment has been cancelled".Replace("[FirstName]",PMD.PB_Patient_Name);
                                var message = await ObjEmailer.Send(email);
                                Response.Status = con.StatusSuccess;
                                Response.Message = con.MessageSuccess + " Email Status : " + message;
                            }
                            else
                            {
                                Response.Status = con.StatusSuccess;
                                Response.Message = con.MessageSuccess;
                            }
                        }
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
                List<MT_Patient_Booking> BookingList = new List<MT_Patient_Booking>();
                //Approved Count
                Query ObjQuery;
                QuerySnapshot ObjQuerySnap;
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Approved").WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Approved").WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Approved").WhereEqualTo("PB_Booked_From", "NB");
                }
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Approved = ObjQuerySnap.Count;
                //Action Required

                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Action Required");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Action_Required = ObjQuerySnap.Count;
                //In Review
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Action Required").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Action Required");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.In_Review = ObjQuerySnap.Count;
                //Reject
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Rejected").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Rejected").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Rejected");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Rejected = ObjQuerySnap.Count;
                //Draft
                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Draft").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Draft").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Draft");
                }
                
                ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                Response.Draft = ObjQuerySnap.Count;
                //Total

                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                    ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null && ObjQuerySnap.Documents.Count > 0)
                    {
                        foreach (DocumentSnapshot docsnap in ObjQuerySnap.Documents)
                        {
                            if (docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "WK" && docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "QB" && docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "Draft" && docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "Mark As Completed")
                            {
                                BookingList.Add(docsnap.ConvertTo<MT_Patient_Booking>());
                            }

                        }
                    }
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                    ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null && ObjQuerySnap.Documents.Count > 0)
                    {
                        foreach (DocumentSnapshot docsnap in ObjQuerySnap.Documents)
                        {
                            if (docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "WK" && docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "QB" && docsnap.ConvertTo<MT_Patient_Booking>().PB_Status != "Mark As Completed")
                            {
                                BookingList.Add(docsnap.ConvertTo<MT_Patient_Booking>());
                            }

                        }
                    }
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB");
                }
                
                Response.Total = BookingList.Count();

                //InComplete

                if (Office_Type == "S")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Incomplete").WhereEqualTo("PB_Booking_Surgery_Center_ID", Surgery_Center_ID);
                }
                else if (Office_Type == "P")
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Incomplete").WhereEqualTo("PB_Booking_Physician_Office_ID", Surgery_Center_ID);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booked_From", "NB").WhereEqualTo("PB_Status", "Incomplete");
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
                ////PMD.PB_Is_Deleted = false;
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
                    PMD.PB_Surgical_Procedure_Information.SPI_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date),1);
                    PMD.PB_Surgical_Procedure_Information.SPI_Create_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Create_Date));
                    PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Modify_Date));
                    PMD.PB_Surgical_Procedure_Information.SPI_TimeZone = PMD.PB_Surgical_Procedure_Information.SPI_TimeZone;                    
                    PMD.PB_Booking_Surgery_Center_ID = PMD.PB_Booking_Surgery_Center_ID;
                    PMD.PB_Booking_Surgery_Center_Name = PMD.PB_Booking_Surgery_Center_Name;
                    PMD.PB_TimeZone = PMD.PB_TimeZone;
                    PMD.PB_Booking_Date = con.ConvertTimeZone(PMD.PB_Surgical_Procedure_Information.SPI_TimeZone, Convert.ToDateTime(PMD.PB_Surgical_Procedure_Information.SPI_Date),1);
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


                //reject
                if (PMD.PB_Reject != null)
                {
                    foreach (Reason rea in PMD.PB_Reject)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Approved != null)
                {
                    foreach (Reason rea in PMD.PB_Approved)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Draft != null)
                {
                    foreach (Reason rea in PMD.PB_Draft)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Incomplete != null)
                {
                    foreach (Reason rea in PMD.PB_Incomplete)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Complete != null)
                {
                    foreach (Reason rea in PMD.PB_Complete)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Noshow != null)
                {
                    foreach (Reason rea in PMD.PB_Noshow)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Suspended != null)
                {
                    foreach (Reason rea in PMD.PB_Suspended)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Unapproved != null)
                {
                    foreach (Reason rea in PMD.PB_Unapproved)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (PMD.PB_Notes != null)
                {
                    foreach (Reason rea in PMD.PB_Notes)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PMD);
                if (Result != null)
                {
                    MT_System_EmailTemplates sysemail = new MT_System_EmailTemplates();
                    Email email = new Email();
                    ObjQuery = Db.Collection("MT_System_EmailTemplates").WhereEqualTo("SET_Is_Active", true).WhereEqualTo("SET_Is_Deleted", false).WhereEqualTo("SET_Name", "Appointment confirmation");
                    ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                    if (ObjQuerySnap != null)
                    {
                        sysemail = ObjQuerySnap.Documents[0].ConvertTo<MT_System_EmailTemplates>();

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
                        PatiEmail.Add(PMD.PB_Patient_Email);
                        PatiName.Add(PMD.PB_Patient_Name);
                        email.To_Email = PatiEmail;
                        email.To_Name = PatiName;
                        email.From_Name = sysemail.SET_From_Name;
                        email.From_Email = sysemail.SET_From_Email;
                        email.HtmlContent = (sysemail.SET_Header==null?"": sysemail.SET_Header)
                            + "<br/>"
                            + "<br/>"
                            + sysemail.SET_Message.ToString().Replace("[FirstName]", PMD.PB_Patient_Name)
                            .Replace("[Dr Name]", PMD.PB_Surgical_Procedure_Information==null?"":PMD.PB_Surgical_Procedure_Information.SPI_Surgeon_Name)
                            .Replace("[mm/dd/yy]", PMD.PB_Booking_Date.ToString("mm/dd/yy"))
                            .Replace("[hr:mn]", PMD.PB_Booking_Time.ToString())
                            .Replace("[FacilityName]", PMD.PB_Booking_Surgery_Center_Name)
                            + "<br/>"
                            + "<br/>"
                            + sysemail.SET_Footer.ToString().Replace("[FacilityName]", PMD.PB_Booking_Surgery_Center_Name);
                        email.Subject = "Appointment has been scheduled";
                        var message = await ObjEmailer.Send(email);
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess + " Email Status : " + message;
                    }
                    else
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                    }
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
                        {"PB_Booking_Time", PMD.PB_Booking_Time},
                        {"PB_Booking_Duration", PMD.PB_Booking_Duration},
                        {"PB_Alerts", PMD.PB_Alerts},
                        {"PB_Notes", PMD.PB_Notes},
                        {"PB_Status", PMD.PB_Status},
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_TimeZone", PMD.PB_TimeZone},
                        {"PB_Created_By", PMD.PB_Created_By},
                        {"PB_User_Name", PMD.PB_User_Name},
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

        [Route("API/Booking/AssignNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> AssignNotification(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Notifications Notification = new MT_Notifications();
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                List<MT_Notifications> NotiList = new List<MT_Notifications>();
                Query Qry = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID).WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await Qry.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (PMD.PB_Notifications_Array != null && Booking.PB_Notifications != null)
                    {
                        foreach (MT_Notifications noti in Booking.PB_Notifications)
                        {
                            NotiList.Add(noti);
                        }
                    }
                }

                if (PMD.PB_Notifications_Array != null)
                {
                    foreach (string str in PMD.PB_Notifications_Array)
                    {
                        Query Qry1 = Db.Collection("MT_Notifications").WhereEqualTo("NFT_Unique_ID", str).WhereEqualTo("NFT_Is_Deleted", false).WhereEqualTo("NFT_Is_Active", true);
                        QuerySnapshot ObjQuerySnap1 = await Qry1.GetSnapshotAsync();
                        if (ObjQuerySnap1 != null)
                        {
                            NotiList.Add(ObjQuerySnap1.Documents[0].ConvertTo<MT_Notifications>());
                        }
                    }
                }
                


                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_TimeZone", PMD.PB_TimeZone},
                        {"PB_Notifications", NotiList}
                    };


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

        [Route("API/Booking/DeleteAssignedNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAssignedNotification(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Notifications Notification = new MT_Notifications();
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                List<MT_Notifications> NotiList = new List<MT_Notifications>();
                Query Qry = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", PMD.PB_Unique_ID).WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await Qry.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (PMD.PB_Notifications_Array != null && Booking.PB_Notifications!=null)
                    {
                        foreach (MT_Notifications noti in Booking.PB_Notifications)
                        {
                            if (PMD.PB_Notifications_Array.Contains<string>(noti.NFT_Unique_ID) == false)
                            {
                                NotiList.Add(noti);
                            }
                        }
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_TimeZone", PMD.PB_TimeZone},
                        {"PB_Notifications", NotiList}
                    };

                
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

        [Route("API/Booking/RemoveAllBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> RemoveAllBooking(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                Query Qry = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID);
                QuerySnapshot ObjQuerySnap = await Qry.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot docsnap in ObjQuerySnap.Documents)
                    {
                        Booking = docsnap.ConvertTo<MT_Patient_Booking>();
                        DocumentReference docRef = Db.Collection("MT_Patient_Booking").Document(Booking.PB_Unique_ID);
                        WriteResult Result = await docRef.DeleteAsync();
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

        [Route("API/Booking/MarkAsCompleteBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> MarkAsCompleteBooking(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_PatientInfomation PatientInfo = new MT_PatientInfomation();
                Query Qry = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Unique_ID", PMD.PB_Patient_ID);
                QuerySnapshot ObjQuerySnap1 = await Qry.GetSnapshotAsync();
                if (ObjQuerySnap1 != null)
                {
                    PatientInfo = ObjQuerySnap1.Documents[0].ConvertTo<MT_PatientInfomation>();
                    Dictionary<string, object> PatientData = new Dictionary<string, object>
                    {
                        {"Patient_Unique_ID",PatientInfo.Patient_Unique_ID},
                        {"Patient_Code",PatientInfo.Patient_Code},
                        {"Patient_Prefix",PatientInfo.Patient_Prefix},
                        {"Patient_First_Name",PatientInfo.Patient_First_Name},
                        {"Patient_Middle_Name",PatientInfo.Patient_Middle_Name},
                        {"Patient_Last_Name",PatientInfo.Patient_Last_Name},
                        {"Patient_DOB",con.ConvertTimeZone(PatientInfo.Patient_TimeZone,Convert.ToDateTime(PatientInfo.Patient_DOB))},
                        {"Patient_Sex",PatientInfo.Patient_Sex},
                        {"Patient_SSN",PatientInfo.Patient_SSN},
                        {"Patient_Address1",PatientInfo.Patient_Address1},
                        {"Patient_Address2",PatientInfo.Patient_Address2},
                        {"Patient_City",PatientInfo.Patient_City},
                        {"Patient_State",PatientInfo.Patient_State},
                        {"Patient_Zipcode",PatientInfo.Patient_Zipcode},
                        {"Patient_Primary_No",PatientInfo.Patient_Primary_No},
                        {"Patient_Secondary_No",PatientInfo.Patient_Secondary_No},
                        {"Patient_Spouse_No",PatientInfo.Patient_Spouse_No},
                        {"Patient_Work_No",PatientInfo.Patient_Work_No},
                        {"Patient_Emergency_No",PatientInfo.Patient_Emergency_No},
                        {"Patient_Email",PatientInfo.Patient_Email},
                        {"Patient_Religion",PatientInfo.Patient_Religion},
                        {"Patient_Ethinicity",PatientInfo.Patient_Ethinicity},
                        {"Patient_Race",PatientInfo.Patient_Race},
                        {"Patient_Marital_Status",PatientInfo.Patient_Marital_Status},
                        {"Patient_Nationality",PatientInfo.Patient_Nationality},
                        {"Patient_Language",PatientInfo.Patient_Language},
                        {"Patient_Height_In_Ft",PatientInfo.Patient_Height_In_Ft},
                        {"Patient_Height_In_Inch",PatientInfo.Patient_Height_In_Inch},
                        {"Patient_Weight",PatientInfo.Patient_Weight},
                        {"Patient_Body_Mass_Index",PatientInfo.Patient_Body_Mass_Index},
                        {"Patient_Data",PatientInfo.Patient_Data},
                        {"Patient_Insurance_Type",PatientInfo.Patient_Insurance_Type},
                        {"Patient_Response_Data",PatientInfo.Patient_Response_Data},
                        {"Patient_Surgery_Physician_Center_ID",PatientInfo.Patient_Surgery_Physician_Center_ID},
                        {"Patient_Office_Type",PatientInfo.Patient_Office_Type},
                        {"Patient_Created_By",PatientInfo.Patient_Created_By},
                        {"Patient_User_Name",PatientInfo.Patient_User_Name},
                        {"Patient_Create_Date",con.ConvertTimeZone(PatientInfo.Patient_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Create_Date))},
                        {"Patient_Modify_Date",con.ConvertTimeZone(PatientInfo.Patient_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Modify_Date))},
                        {"Patient_Is_Active",PatientInfo.Patient_Is_Active},
                        {"Patient_Is_Deleted",PatientInfo.Patient_Is_Deleted},
                        {"Patient_TimeZone",PatientInfo.Patient_TimeZone},
                    };

                    if (PatientInfo.Patient_Primary_Insurance_Details != null)
                    {
                        Dictionary<string, object> PrimaryInsuranceData = new Dictionary<string, object> 
                        {
                            {"PPID_Unique_ID", PatientInfo.Patient_Primary_Insurance_Details.PPID_Unique_ID},
                            {"PPID_Relation_To_Patient", PatientInfo.Patient_Primary_Insurance_Details.PPID_Relation_To_Patient},
                            {"PPID_Subscriber_Name", PatientInfo.Patient_Primary_Insurance_Details.PPID_Subscriber_Name},
                            {"PPID_Subscriber_SSN_No", PatientInfo.Patient_Primary_Insurance_Details.PPID_Subscriber_SSN_No},
                            {"PPID_DOB", con.ConvertTimeZone(PatientInfo.Patient_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Primary_Insurance_Details.PPID_DOB))},
                            {"PPID_Policy_No", PatientInfo.Patient_Primary_Insurance_Details.PPID_Policy_No},
                            {"PPID_Primary_Insurance", PatientInfo.Patient_Primary_Insurance_Details.PPID_Primary_Insurance},
                            {"PPID_PO_Box_No", PatientInfo.Patient_Primary_Insurance_Details.PPID_PO_Box_No},
                            {"PPID_Address", PatientInfo.Patient_Primary_Insurance_Details.PPID_Address},
                            {"PPID_Address2", PatientInfo.Patient_Primary_Insurance_Details.PPID_Address2},
                            {"PPID_State", PatientInfo.Patient_Primary_Insurance_Details.PPID_State},
                            {"PPID_City", PatientInfo.Patient_Primary_Insurance_Details.PPID_City},
                            {"PPID_Zip_Code", PatientInfo.Patient_Primary_Insurance_Details.PPID_Zip_Code},
                            {"PPID_DocPath", PatientInfo.Patient_Primary_Insurance_Details.PPID_DocPath},
                            {"PPID_V_Start_Date", con.ConvertTimeZone(PatientInfo.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Primary_Insurance_Details.PPID_V_Start_Date))},
                            {"PPID_V_End_Date", con.ConvertTimeZone(PatientInfo.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Primary_Insurance_Details.PPID_V_End_Date))},
                            {"PPID_Created_By", PatientInfo.Patient_Primary_Insurance_Details.PPID_Created_By                            },
                            {"PPID_User_Name", PatientInfo.Patient_Primary_Insurance_Details.PPID_User_Name},
                            {"PPID_Modify_Date", con.ConvertTimeZone(PatientInfo.Patient_Primary_Insurance_Details.PPID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Primary_Insurance_Details.PPID_Modify_Date))},
                            {"PPID_Is_Active", PatientInfo.Patient_Primary_Insurance_Details.PPID_Is_Active},
                            {"PPID_Is_Deleted", PatientInfo.Patient_Primary_Insurance_Details.PPID_Is_Deleted},
                            {"PPID_TimeZone", PatientInfo.Patient_Primary_Insurance_Details.PPID_TimeZone},
                        };
                        PatientData.Add("Patient_Primary_Insurance_Details", PrimaryInsuranceData);
                    }

                    if (PatientInfo.Patient_Secondary1_Insurance_Details != null)
                    {
                        Dictionary<string, object> Secondary1_InsuranceData = new Dictionary<string, object>
                        {
                            {"PSID_Unique_ID",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Unique_ID},
                            {"PSID_Relation_To_Patient",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Relation_To_Patient},
                            {"PSID_Subscriber_Name",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Subscriber_Name},
                            {"PSID_Subscriber_SSN_No",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Subscriber_SSN_No},
                            {"PSID_DOB",con.ConvertTimeZone(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_DOB))},
                            {"PSID_Policy_No",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Policy_No},
                            {"PSID_Primary_Insurance",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Primary_Insurance},
                            {"PSID_PO_Box_No",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_PO_Box_No},
                            {"PSID_Address",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Address},
                            {"PSID_Address2",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Address2},
                            {"PSID_State",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_State},
                            {"PSID_City",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_City},
                            {"PSID_Zip_Code",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Zip_Code},
                            {"PSID_DocPath",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_DocPath},
                            {"PSID_V_Start_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_V_Start_Date))},
                            {"PSID_V_End_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_V_End_Date))},
                            {"PSID_Trace_Number",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Trace_Number},
                            {"PSID_Created_By",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Created_By},
                            {"PSID_User_Name",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_User_Name},
                            {"PSID_Modify_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Modify_Date))},
                            {"PSID_Is_Active",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Is_Active},
                            {"PSID_Is_Deleted",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_Is_Deleted},
                            {"PSID_TimeZone",PatientInfo.Patient_Secondary1_Insurance_Details.PSID_TimeZone},
                        };
                        PatientData.Add("Patient_Secondary1_Insurance_Details", Secondary1_InsuranceData);
                    }

                    if (PatientInfo.Patient_Secondary2_Insurance_Details != null)
                    {
                        Dictionary<string, object> Secondary2_InsuranceData = new Dictionary<string, object>
                        {
                            {"PSID_Unique_ID",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Unique_ID},
                            {"PSID_Relation_To_Patient",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Relation_To_Patient},
                            {"PSID_Subscriber_Name",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Subscriber_Name},
                            {"PSID_Subscriber_SSN_No",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Subscriber_SSN_No},
                            {"PSID_DOB",con.ConvertTimeZone(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_DOB))},
                            {"PSID_Policy_No",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Policy_No},
                            {"PSID_Primary_Insurance",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Primary_Insurance},
                            {"PSID_PO_Box_No",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_PO_Box_No},
                            {"PSID_Address",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Address},
                            {"PSID_Address2",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Address2},
                            {"PSID_State",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_State},
                            {"PSID_City",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_City},
                            {"PSID_Zip_Code",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Zip_Code},
                            {"PSID_DocPath",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_DocPath},
                            {"PSID_V_Start_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_V_Start_Date))},
                            {"PSID_V_End_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_V_End_Date))},
                            {"PSID_Trace_Number",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Trace_Number},
                            {"PSID_Created_By",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Created_By},
                            {"PSID_User_Name",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_User_Name},                            
                            {"PSID_Modify_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Modify_Date))},
                            {"PSID_Is_Active",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Is_Active},
                            {"PSID_Is_Deleted",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_Is_Deleted},
                            {"PSID_TimeZone",PatientInfo.Patient_Secondary2_Insurance_Details.PSID_TimeZone},
                        };
                        PatientData.Add("Patient_Secondary2_Insurance_Details", Secondary2_InsuranceData);
                    }

                    if (PatientInfo.Patient_Secondary3_Insurance_Details != null)
                    {
                        Dictionary<string, object> Secondary3_InsuranceData = new Dictionary<string, object>
                        {
                            {"PSID_Unique_ID",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Unique_ID},
                            {"PSID_Relation_To_Patient",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Relation_To_Patient},
                            {"PSID_Subscriber_Name",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Subscriber_Name},
                            {"PSID_Subscriber_SSN_No",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Subscriber_SSN_No},
                            {"PSID_DOB",con.ConvertTimeZone(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_DOB))},
                            {"PSID_Policy_No",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Policy_No},
                            {"PSID_Primary_Insurance",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Primary_Insurance},
                            {"PSID_PO_Box_No",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_PO_Box_No},
                            {"PSID_Address",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Address},
                            {"PSID_Address2",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Address2},
                            {"PSID_State",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_State},
                            {"PSID_City",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_City},
                            {"PSID_Zip_Code",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Zip_Code},
                            {"PSID_DocPath",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_DocPath},
                            {"PSID_V_Start_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_V_Start_Date))},
                            {"PSID_V_End_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_TimeZone,Convert.ToDateTime(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_V_End_Date))},
                            {"PSID_Trace_Number",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Trace_Number},
                            {"PSID_Created_By",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Created_By},
                            {"PSID_User_Name",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_User_Name},
                            {"PSID_Modify_Date",con.ConvertTimeZone(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_TimeZone, Convert.ToDateTime(PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Modify_Date))},
                            {"PSID_Is_Active",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Is_Active},
                            {"PSID_Is_Deleted",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_Is_Deleted},
                            {"PSID_TimeZone",PatientInfo.Patient_Secondary3_Insurance_Details.PSID_TimeZone},
                        };
                        PatientData.Add("Patient_Secondary3_Insurance_Details", Secondary3_InsuranceData);
                    }
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Status", "Mark As Completed"},
                        {"PB_Modify_Date", con.ConvertTimeZone(PMD.PB_TimeZone, Convert.ToDateTime(PMD.PB_Modify_Date))},
                        {"PB_TimeZone", PMD.PB_TimeZone},
                        {"PB_Created_By", PMD.PB_Created_By},
                        {"PB_User_Name", PMD.PB_User_Name},
                        {"PB_Patient_Details", PatientData}
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
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        [Route("API/Booking/CheckPatientBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> CheckPatientBooking(MT_Patient_Booking PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                List<MT_Patient_Booking> patilist = new List<MT_Patient_Booking>();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Booking_Physician_Office_ID", PMD.PB_Booking_Physician_Office_ID).WhereEqualTo("PB_Patient_ID", PMD.PB_Patient_ID).OrderByDescending("PB_Booking_Date");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    if (ObjQuerySnap.Documents.Count > 0)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.IsAvailable = true;
                        return ConvertToJSON(Response);
                    }
                    else
                    {
                        ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Patient_ID", PMD.PB_Patient_ID);
                        ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                        if (ObjQuerySnap != null)
                        {
                            if (ObjQuerySnap.Documents.Count > 0)
                            {
                                Response.Status = con.StatusSuccess;
                                Response.Message = con.MessageSuccess;
                                Response.IsAvailable = true;
                                return ConvertToJSON(Response);
                            }
                        }
                        else
                        {
                            Response.Status = con.StatusSuccess;
                            Response.Message = con.MessageSuccess;
                            Response.IsAvailable = false;
                        }
                    }
                }
                else
                {
                    Response.Status = con.StatusDNE;
                    Response.Message = con.MessageDNE;
                    Response.IsAvailable = false;
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
