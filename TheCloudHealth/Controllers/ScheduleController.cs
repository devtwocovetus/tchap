using Google.Cloud.Firestore;
using System;
using TheCloudHealth.Models;
using System.Threading.Tasks;
using TheCloudHealth.Lib;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace TheCloudHealth.Controllers
{
    public class ScheduleController : ApiController
    {
        ConnectionClass con;
        ITwilioSMS SMSObj;
        ITwilioVoice VoiceObj;
        ITwilioWhatsApp WhatsappObj;
        IEmailer EmailObj;
        FirestoreDb Db;
        string Email = "";
        string Phone = "";
        string Name = "";
        DateTime BookingDate;
        DateTime NoticationSendingDay;
        int Days = 0;
        int i,j;

        public ScheduleController()
        {
            con = new ConnectionClass();
            SMSObj = new TwilioSMS();
            VoiceObj = new TwilioVoice();
            WhatsappObj = new TwilioWhatsApp();
            EmailObj = new Emailer();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        [Route("API/Schedule/BookingSchedule")]
        [HttpGet]
        public async Task<HttpResponseMessage> BookingSchedule(string Slug)
        {
            Db = con.SurgeryCenterDb(Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                MT_PatientInfomation Patient = new MT_PatientInfomation();
                Query ObjQuery = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null && ObjQuerySnap.Documents.Count > 0)
                {
                    foreach (DocumentSnapshot docsnap in ObjQuerySnap.Documents)
                    {
                        Booking = docsnap.ConvertTo<MT_Patient_Booking>();
                        if (Booking.PB_Notifications != null)
                        {
                            if (Booking.PB_Patient_ID != null && Booking.PB_Patient_ID != "")
                            {
                                Query PatientQuery = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Active", true).WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Unique_ID", Booking.PB_Patient_ID);
                                QuerySnapshot ObjPatientQuerySnap = await PatientQuery.GetSnapshotAsync();
                                if (ObjPatientQuerySnap != null)
                                {
                                    Patient = ObjPatientQuerySnap.Documents[0].ConvertTo<MT_PatientInfomation>();
                                    Email = Patient.Patient_Email;
                                    Phone = Patient.Patient_Primary_No;
                                    Name = Patient.Patient_First_Name + " " + Patient.Patient_Middle_Name + " " + Patient.Patient_Last_Name;
                                    BookingDate = Booking.PB_Booking_Date;
                                    foreach (MT_Notifications noti in Booking.PB_Notifications)
                                    {
                                        if (noti.NFT_Actions != null)
                                        {
                                            foreach (Notification_Action action in noti.NFT_Actions)
                                            {
                                                if (action.NFA_Status == "Pending")
                                                {
                                                    if (action.NFA_DayOrWeek == 0)
                                                    {
                                                        if (action.NFA_Be_Af == 0)
                                                        {
                                                            Days = (-1) * action.NFA_Days;
                                                        }
                                                        else if (action.NFA_Be_Af == 1)
                                                        {
                                                            Days = action.NFA_Days;
                                                        }
                                                    }
                                                    else if (action.NFA_DayOrWeek == 1)
                                                    {
                                                        if (action.NFA_Be_Af == 0)
                                                        {
                                                            Days = (-7) * action.NFA_Days;
                                                        }
                                                        else if (action.NFA_Be_Af == 1)
                                                        {
                                                            Days = 7 * action.NFA_Days;
                                                        }
                                                    }
                                                    NoticationSendingDay = BookingDate.AddDays(Days);
                                                    if (DateTime.Now.Date.ToString("MM/dd/yyyy") == NoticationSendingDay.Date.ToString("MM/dd/yyyy"))
                                                    {
                                                        TimeSpan NotificationTiming = TimeSpan.Parse(action.NFA_Timing);
                                                        TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
                                                        if (TimeSpan.Compare(timeOfDay, NotificationTiming) >=0)
                                                        {
                                                            switch (action.NFA_Action_Type)
                                                            {
                                                                case "SMS":
                                                                    Sms sms = new Sms();
                                                                    sms.Receiver_Contact_No = Phone;
                                                                    sms.Message_Body = action.NFA_Message;
                                                                    sms.Message_Date = con.ConvertTimeZone(action.NFA_TimeZone, Convert.ToDateTime(DateTime.Now));
                                                                    sms.Message_Title = action.NFA_Action_Title;
                                                                    SMSObj.SendSMS(sms);
                                                                    Response = await UpdateAction(Booking.PB_Unique_ID, action.NFA_Unique_ID, Slug);
                                                                    break;
                                                                case "Email":
                                                                    Email email = new Email();
                                                                    email.To_Name = Name;
                                                                    email.To_Email = Email;
                                                                    email.PlainTextContent = "";
                                                                    email.HtmlContent = action.NFA_Message;
                                                                    await EmailObj.Send(email);
                                                                    Response = await UpdateAction(Booking.PB_Unique_ID, action.NFA_Unique_ID, Slug);
                                                                    break;
                                                                case "Whatsapp":
                                                                    Whatsapp whatsapp = new Whatsapp();
                                                                    whatsapp.Receiver_Contact_No = Phone;
                                                                    whatsapp.Message_Title = action.NFA_Action_Title;
                                                                    whatsapp.Message_Date = con.ConvertTimeZone(action.NFA_TimeZone, Convert.ToDateTime(DateTime.Now));
                                                                    whatsapp.Message_Body = action.NFA_Message;
                                                                    WhatsappObj.SendMessageWithWhatsapp(whatsapp);
                                                                    Response = await UpdateAction(Booking.PB_Unique_ID, action.NFA_Unique_ID, Slug);
                                                                    break;
                                                                case "Voice":
                                                                    Voice voice = new Voice();
                                                                    voice.Voice_Call_Date = con.ConvertTimeZone(action.NFA_TimeZone, Convert.ToDateTime(DateTime.Now));
                                                                    voice.Voice_Receiver_Name = Patient.Patient_First_Name;
                                                                    voice.Voice_Call_Body = action.NFA_Message;
                                                                    voice.Voice_Receiver_Contact_No = Phone;
                                                                    VoiceObj.SetVoiceCall(voice);
                                                                    Response = await UpdateAction(Booking.PB_Unique_ID, action.NFA_Unique_ID, Slug);
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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

        private async Task<BookingResponse> UpdateAction(string BookingID,string ActionID,string Slug)
        {
            Db = con.SurgeryCenterDb(Slug);
            BookingResponse Response = new BookingResponse();
            try
            {
                MT_Patient_Booking Booking = new MT_Patient_Booking();
                List<MT_Notifications> notilist = new List<MT_Notifications>();
                List<Notification_Action> Actionlist = new List<Notification_Action>();
                Query QueryObj = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Unique_ID", BookingID);
                QuerySnapshot ObjQuerySnap = await QueryObj.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Booking>();
                    if (Booking.PB_Notifications != null && Booking.PB_Notifications.Count > 0)
                    {
                        foreach (MT_Notifications noti in Booking.PB_Notifications)
                        {
                            if (noti.NFT_Actions != null && noti.NFT_Actions.Count > 0)
                            {
                                foreach (Notification_Action action in noti.NFT_Actions)
                                {
                                    if (action.NFA_Unique_ID == ActionID)
                                    {
                                        action.NFA_Status = "Sent";
                                        Actionlist.Add(action);
                                    }
                                    else
                                    {
                                        Actionlist.Add(action);
                                    }
                                }
                                noti.NFT_Actions = Actionlist;
                            }
                            notilist.Add(noti);
                        }
                    }

                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PB_Notifications", notilist}
                    };
                    DocumentReference documentRef = Db.Collection("MT_Patient_Booking").Document(BookingID);
                    WriteResult Result = await documentRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                    }
                    else
                    {
                        Response.Status = con.StatusNotUpdate;
                        Response.Message = con.MessageNotUpdate;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return Response;
        }

    }
}
