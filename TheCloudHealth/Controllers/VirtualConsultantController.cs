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
    public class VirtualConsultantController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        ICreatePDF ObjPDF;
        string UniqueID = "";
        public VirtualConsultantController()
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

        [Route("API/VirtualConsultant/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                VCMD.VCB_Unique_ID = UniqueID;
                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking");//.WhereEqualTo("VCB_Surgery_Physician_Center_ID", VCMD.VCB_Surgery_Physician_Center_ID).WhereEqualTo("VCB_Office_Type", VCMD.VCB_Office_Type);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    VCMD.VCB_Booking_No = "VCBN000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    VCMD.VCB_Booking_No = "VCBN000" + "1";
                }
                VCMD.VCB_Booking_Date = con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Booking_Date) , 1);
                VCMD.VCB_Create_Date = con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Create_Date));
                VCMD.VCB_Modify_Date = con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Modify_Date));
                DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(VCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = VCMD;
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

        [Route("API/VirtualConsultant/VCListForPO")]
        [HttpPost]
        public async Task<HttpResponseMessage> VCListForPO(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                List<MT_Virtual_Consultant_Booking> patilist = new List<MT_Virtual_Consultant_Booking>();
                Query ObjQuery;
                if (VCMD.VCB_Booking_Physician_Office_ID == "0")
                {
                    ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Booking_Physician_Office_ID", VCMD.VCB_Booking_Physician_Office_ID);
                }                

                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (VCMD.VCB_Booking_Physician_Office_ID != null)
                        {
                            if (Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status != "Draft" && Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status != "Cancelled" && Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status != "Complete")
                            {
                                patilist.Add(Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>());
                            }
                        }
                        else if (VCMD.VCB_Booking_Physician_Office_ID == "0")
                        {
                            patilist.Add(Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>());
                        }                        
                    }

                    Response.DataList = patilist.OrderBy(o => o.VCB_Booking_Date).ToList();
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

        [Route("API/VirtualConsultant/VCListForPatient")]
        [HttpPost]
        public async Task<HttpResponseMessage> VCListForPatient(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                List<MT_Virtual_Consultant_Booking> patilist = new List<MT_Virtual_Consultant_Booking>();
                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Patient_ID", VCMD.VCB_Patient_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status != "Cancelled")
                        {
                            patilist.Add(Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>());
                        }
                    }

                    Response.DataList = patilist.OrderBy(o => o.VCB_Booking_Date).ToList();
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

        [Route("API/VirtualConsultant/VCHistoryListForPO")]
        [HttpPost]
        public async Task<HttpResponseMessage> VCHistoryListForPO(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                List<MT_Virtual_Consultant_Booking> patilist = new List<MT_Virtual_Consultant_Booking>();
                Query ObjQuery;
                if (VCMD.VCB_Booking_Physician_Office_ID == "0")
                {
                    ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false);
                }
                else
                {
                    ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Booking_Physician_Office_ID", VCMD.VCB_Booking_Physician_Office_ID);
                }

                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (VCMD.VCB_Booking_Physician_Office_ID != null)
                        {
                            if (Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status == "Cancelled" || Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status == "Complete")
                            {
                                patilist.Add(Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>());
                            }
                        }
                        else if (VCMD.VCB_Booking_Physician_Office_ID == "0")
                        {
                            patilist.Add(Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>());
                        }
                    }

                    Response.DataList = patilist.OrderBy(o => o.VCB_Booking_Date).ToList();
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

        [Route("API/VirtualConsultant/VCCancelledListForPatient")]
        [HttpPost]
        public async Task<HttpResponseMessage> VCCancelledListForPatient(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                List<MT_Virtual_Consultant_Booking> patilist = new List<MT_Virtual_Consultant_Booking>();
                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Patient_ID", VCMD.VCB_Patient_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        if (Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>().VCB_Status == "Cancelled")
                        {
                            patilist.Add(Docsnapshot.ConvertTo<MT_Virtual_Consultant_Booking>());
                        }
                    }

                    Response.DataList = patilist.OrderBy(o => o.VCB_Booking_Date).ToList();
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

        [Route("API/VirtualConsultant/AddVCBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddVCBooking(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                //Main section 
                UniqueID = con.GetUniqueKey();
                VCMD.VCB_Unique_ID = UniqueID;
                List<Alerts> AlrtList = new List<Alerts>();
                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    VCMD.VCB_Booking_No = "VCBN000" + ObjQuerySnap.Documents.Count + 1.ToString();
                }
                else
                {
                    VCMD.VCB_Booking_No = "VCBN000" + "1";
                }
                VCMD.VCB_Booking_Date = con.ConvertTimeZone("", Convert.ToDateTime(VCMD.VCB_Booking_Date),1);
                VCMD.VCB_Create_Date = con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Create_Date));
                VCMD.VCB_Modify_Date = con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Modify_Date));
                ////VCMD.VCB_Is_Deleted = false;
                //Main section Edit                

                if (VCMD.VCB_Approved != null)
                {
                    foreach (Reason rea in VCMD.VCB_Approved)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (VCMD.VCB_Draft != null)
                {
                    foreach (Reason rea in VCMD.VCB_Draft)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                if (VCMD.VCB_Complete != null)
                {
                    foreach (Reason rea in VCMD.VCB_Complete)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }
                if (VCMD.VCB_Cancelled != null)
                {
                    foreach (Reason rea in VCMD.VCB_Cancelled)
                    {
                        rea.RR_Create_Date = con.ConvertTimeZone(rea.RR_TimeZone, Convert.ToDateTime(rea.RR_Create_Date));
                    }
                }

                DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(VCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = VCMD;
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

        [Route("API/VirtualConsultant/EditVCBooking")]
        [HttpPost]
        public async Task<HttpResponseMessage> EditVCBooking(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                MT_Virtual_Consultant_Booking booking = new MT_Virtual_Consultant_Booking();
                Query Qty = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Unique_ID", VCMD.VCB_Unique_ID);
                QuerySnapshot ObjBooking = await Qty.GetSnapshotAsync();
                if (ObjBooking != null)
                {
                    booking = ObjBooking.Documents[0].ConvertTo<MT_Virtual_Consultant_Booking>();
                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"VCB_Patient_Name", VCMD.VCB_Patient_Name},
                        {"VCB_Patient_Last_Name", VCMD.VCB_Patient_Last_Name},
                        {"VCB_Booking_Date", con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Booking_Date))},
                        {"VCB_Booking_Time", VCMD.VCB_Booking_Time},
                        {"VCB_Doctor_ID", VCMD.VCB_Doctor_ID},
                        {"VCB_Doctor_Name", VCMD.VCB_Doctor_Name},
                        {"VCB_Status", VCMD.VCB_Status},
                        {"VCB_Modify_Date", con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Modify_Date))},
                        {"VCB_TimeZone", VCMD.VCB_TimeZone},
                    };

                    
                    //Main section 
                    DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(VCMD.VCB_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = VCMD;
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

        public MT_Virtual_Consultant_Booking GetImages()
        {
            MT_Virtual_Consultant_Booking Surcenter = new MT_Virtual_Consultant_Booking();
            try
            {
                List<Doc_Uploaded> DocUplodList = new List<Doc_Uploaded>();

                //string[] DBPath;
                int i = 0;
                var httpRequest = HttpContext.Current.Request;
                var postedData = httpRequest.Form[1];
                string str = postedData.Substring(1, postedData.Length - 2);
                JObject jobject = JObject.Parse(str);
                Surcenter = JsonConvert.DeserializeObject<MT_Virtual_Consultant_Booking>(jobject.ToString());

                var Doclist = httpRequest.Form[0];
                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Doclist);

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        int hasheddate = DateTime.Now.GetHashCode();

                        var myKey = dict.FirstOrDefault(x => x.Value == postedFile.FileName.ToString()).Key;

                        string changed_name = myKey + DateTime.Now.ToString("yyyyMMddHHmmss") + postedFile.FileName.Substring(postedFile.FileName.IndexOf('.'), (postedFile.FileName.Length - postedFile.FileName.IndexOf('.')));

                        var filePath = HttpContext.Current.Server.MapPath("~/Images/" + changed_name);

                        postedFile.SaveAs(filePath); // save the file to a folder "Images" in the root of your app

                        //changed_name = @"~\Images\" + changed_name; //store this complete path to database
                        Doc_Uploaded DocUpld = new Doc_Uploaded();
                        DocUpld.DU_Unique_ID = con.GetUniqueKey();
                        DocUpld.DU_Attachment_Type = myKey;
                        DocUpld.DU_Doc_Path = @"~\Images\" + changed_name;
                        DocUpld.DU_Doc_Name = changed_name.Substring(0, 10);
                        DocUpld.DU_Created_By = Surcenter.VCB_Created_By;
                        DocUpld.DU_User_Name = Surcenter.VCB_User_Name;
                        DocUpld.DU_Create_Date = con.ConvertTimeZone(Surcenter.VCB_TimeZone, Convert.ToDateTime(Surcenter.VCB_Modify_Date));
                        DocUpld.DU_Modify_Date = con.ConvertTimeZone(Surcenter.VCB_TimeZone, Convert.ToDateTime(Surcenter.VCB_Modify_Date));
                        DocUpld.DU_Is_Active = true;
                        DocUpld.DU_Is_Deleted = false;
                        DocUpld.DU_TimeZone = Surcenter.VCB_TimeZone;

                        DocUplodList.Add(DocUpld);
                        i++;
                    }
                    Surcenter.VCB_Doc_Uploaded_List = DocUplodList;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Surcenter;
        }

        [Route("API/VirtualConsultant/UploadDocuments")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadDocuments()
        {
            MT_Virtual_Consultant_Booking VCMD = GetImages();
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                List<Doc_Uploaded> DocList = new List<Doc_Uploaded>();
                MT_Virtual_Consultant_Booking Booking = new MT_Virtual_Consultant_Booking();
                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Unique_ID", VCMD.VCB_Unique_ID);//.WhereEqualTo("VCB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Virtual_Consultant_Booking>();
                    if (Booking.VCB_Doc_Uploaded_List != null)
                    {
                        foreach (Doc_Uploaded DU in Booking.VCB_Doc_Uploaded_List)
                        {
                            DocList.Add(DU);
                        }
                    }
                }

                if (VCMD.VCB_Doc_Uploaded_List != null)
                {
                    foreach (Doc_Uploaded DU in VCMD.VCB_Doc_Uploaded_List)
                    {
                        DocList.Add(DU);
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"VCB_TimeZone" , VCMD.VCB_TimeZone},
                        {"VCB_Doc_Uploaded_List" , DocList},
                        {"VCB_Modify_Date" , con.ConvertTimeZone(VCMD.VCB_TimeZone,Convert.ToDateTime(VCMD.VCB_Modify_Date))}
                    };
                DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(VCMD.VCB_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = VCMD;
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

        [Route("API/VirtualConsultant/AssignNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> AssignNotification(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                MT_Notifications Notification = new MT_Notifications();
                MT_Virtual_Consultant_Booking Booking = new MT_Virtual_Consultant_Booking();
                List<MT_Notifications> NotiList = new List<MT_Notifications>();
                Query Qry = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Unique_ID", VCMD.VCB_Unique_ID).WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await Qry.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Virtual_Consultant_Booking>();
                    if (VCMD.VCB_Notifications_Array != null && Booking.VCB_Notifications != null)
                    {
                        foreach (MT_Notifications noti in Booking.VCB_Notifications)
                        {
                            NotiList.Add(noti);
                        }
                    }
                }

                if (VCMD.VCB_Notifications_Array != null)
                {
                    foreach (string str in VCMD.VCB_Notifications_Array)
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
                        {"VCB_Modify_Date", con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Modify_Date))},
                        {"VCB_TimeZone", VCMD.VCB_TimeZone},
                        {"VCB_Notifications", NotiList}
                    };


                //Main section 
                DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(VCMD.VCB_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = VCMD;
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

        [Route("API/VirtualConsultant/DeleteAssignedNotification")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteAssignedNotification(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                MT_Notifications Notification = new MT_Notifications();
                MT_Virtual_Consultant_Booking Booking = new MT_Virtual_Consultant_Booking();
                List<MT_Notifications> NotiList = new List<MT_Notifications>();
                Query Qry = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Unique_ID", VCMD.VCB_Unique_ID).WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await Qry.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Booking = ObjQuerySnap.Documents[0].ConvertTo<MT_Virtual_Consultant_Booking>();
                    if (VCMD.VCB_Notifications_Array != null && Booking.VCB_Notifications != null)
                    {
                        foreach (MT_Notifications noti in Booking.VCB_Notifications)
                        {
                            if (VCMD.VCB_Notifications_Array.Contains<string>(noti.NFT_Unique_ID) == false)
                            {
                                NotiList.Add(noti);
                            }
                        }
                    }
                }

                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"VCB_Modify_Date", con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Modify_Date))},
                        {"VCB_TimeZone", VCMD.VCB_TimeZone},
                        {"VCB_Notifications", NotiList}
                    };


                //Main section 
                DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(VCMD.VCB_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = VCMD;
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

        [Route("API/VirtualConsultant/Select")]
        [HttpPost]
        public async Task<HttpResponseMessage> Select(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                MT_Virtual_Consultant_Booking vc = new MT_Virtual_Consultant_Booking();
                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Is_Deleted", false).WhereEqualTo("VCB_Unique_ID", VCMD.VCB_Unique_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    vc = ObjQuerySnap.Documents[0].ConvertTo<MT_Virtual_Consultant_Booking>();
                    Response.Data = vc;
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

        [Route("API/VirtualConsultant/UpdateStatus")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateStatus(MT_Virtual_Consultant_Booking VCMD)
        {
            Db = con.SurgeryCenterDb(VCMD.Slug);
            VCBookingResponse Response = new VCBookingResponse();
            try
            {
                Incident_Details InciDetails = new Incident_Details();
                MT_Virtual_Consultant_Booking BookingInfo = new MT_Virtual_Consultant_Booking();
                List<Reason> ApprovedList = new List<Reason>();
                List<Reason> DraftList = new List<Reason>();
                List<Reason> CompleteList = new List<Reason>();
                List<Reason> CancelledList = new List<Reason>();

                Query ObjQuery = Db.Collection("MT_Virtual_Consultant_Booking").WhereEqualTo("VCB_Unique_ID", VCMD.VCB_Unique_ID).WhereEqualTo("VCB_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Dictionary<string, object> initialData;
                    BookingInfo = ObjQuerySnap.Documents[0].ConvertTo<MT_Virtual_Consultant_Booking>();

                    if (BookingInfo.VCB_Approved != null)
                    {
                        foreach (Reason RR in BookingInfo.VCB_Approved)
                        {
                            ApprovedList.Add(RR);
                        }
                    }

                    if (VCMD.VCB_Approved != null)
                    {
                        foreach (Reason RR in VCMD.VCB_Approved)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            ApprovedList.Add(RR);
                        }
                    }

                    if (BookingInfo.VCB_Draft != null)
                    {
                        foreach (Reason RR in BookingInfo.VCB_Draft)
                        {
                            DraftList.Add(RR);
                        }
                    }

                    if (VCMD.VCB_Draft != null)
                    {
                        foreach (Reason RR in VCMD.VCB_Draft)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            DraftList.Add(RR);
                        }
                    }

                    if (BookingInfo.VCB_Complete != null)
                    {
                        foreach (Reason RR in BookingInfo.VCB_Complete)
                        {
                            CompleteList.Add(RR);
                        }
                    }

                    if (VCMD.VCB_Complete != null)
                    {
                        foreach (Reason RR in VCMD.VCB_Complete)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            CompleteList.Add(RR);
                        }
                    }

                    if (BookingInfo.VCB_Cancelled != null)
                    {
                        foreach (Reason RR in BookingInfo.VCB_Cancelled)
                        {
                            CancelledList.Add(RR);
                        }
                    }

                    if (VCMD.VCB_Cancelled != null)
                    {
                        foreach (Reason RR in VCMD.VCB_Cancelled)
                        {
                            RR.RR_Create_Date = con.ConvertTimeZone(RR.RR_TimeZone, Convert.ToDateTime(RR.RR_Create_Date));
                            CancelledList.Add(RR);
                        }
                    }

                    initialData = new Dictionary<string, object>
                    {
                        {"VCB_Modify_Date", con.ConvertTimeZone(VCMD.VCB_TimeZone, Convert.ToDateTime(VCMD.VCB_Modify_Date))},
                        {"VCB_Cancelled", CancelledList},
                        {"VCB_Approved", ApprovedList},
                        {"VCB_Draft", DraftList},
                        {"VCB_Complete", CompleteList},
                        {"VCB_Status",VCMD.VCB_Status },
                        {"VCB_TimeZone",VCMD.VCB_TimeZone}
                    };


                    DocumentReference docRef = Db.Collection("MT_Virtual_Consultant_Booking").Document(BookingInfo.VCB_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = VCMD;
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
    }
}
