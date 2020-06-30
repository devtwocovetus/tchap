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
    public class CountController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        string UniqueSettingID = "";
        public CountController()
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

        [Route("API/Count/All")]
        [HttpGet]
        public async Task<HttpResponseMessage> All(string Slug)
        {
            Db = con.SurgeryCenterDb(Slug);
            CResponse Response = new CResponse();
            try
            {
                // Surgery center count
                Query query = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot querysnapshot = await query.GetSnapshotAsync();
                if (querysnapshot != null)
                {
                    Response.SC = querysnapshot.Documents.Count;
                }
                else
                {
                    Response.SC = 0;
                }

                // Physician office count

                query = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", false); ;
                querysnapshot = await query.GetSnapshotAsync();
                if (querysnapshot != null)
                {
                    Response.PO = querysnapshot.Documents.Count;
                }
                else
                {
                    Response.PO = 0;
                }

                // Patient count

                query = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false); ;
                querysnapshot = await query.GetSnapshotAsync();
                if (querysnapshot != null)
                {
                    Response.Patient = querysnapshot.Documents.Count;
                }
                else
                {
                    Response.Patient = 0;
                }

                // Booking count

                query = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false); ;
                querysnapshot = await query.GetSnapshotAsync();
                if (querysnapshot != null)
                {
                    Response.Booking = querysnapshot.Documents.Count;
                }
                else
                {
                    Response.Booking = 0;
                }

                // Complete booking count

                query = Db.Collection("MT_Patient_Booking").WhereEqualTo("PB_Is_Deleted", false).WhereEqualTo("PB_Status", "Mark As Completed");
                querysnapshot = await query.GetSnapshotAsync();
                if (querysnapshot != null)
                {
                    Response.CompleteBooking= querysnapshot.Documents.Count;
                }
                else
                {
                    Response.CompleteBooking = 0;
                }
            }
            catch (Exception ex)
            {
                Response.Status = con.StatusFailed;
                Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
            }
            return ConvertToJSON(Response);
        }

        //[Route("API/Count/CheckEmail")]
        //[HttpPost]
        ////[Authorize(Roles ="SAdmin")]
        //public async Task<HttpResponseMessage> CheckEmail(ComModel CMD)
        //{
        //    Db = con.SurgeryCenterDb(CMD.Slug);
        //    CommodelResponse Response = new CommodelResponse();
        //    Boolean Result = false;
        //    try
        //    {
        //        MT_User User = new MT_User();
        //        Query docRef = Db.Collection("MT_User").WhereEqualTo("UM_Is_Deleted", false).WhereEqualTo("UM_Is_Active", true).WhereEqualTo("UM_Email",CMD.Email);
        //        QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
        //        if (ObjQuerySnap != null)
        //        {
        //            if (ObjQuerySnap.Documents.Count > 0)
        //            {
        //                Response.Where = "In User";
        //                Result = false;
        //            }
        //            else
        //            {
        //                Result = true;
        //            }
        //        }
        //        else
        //        {
        //            Result = true;
        //        }
        //        if (Result == true)
        //        {
        //            docRef = Db.Collection("MT_Staff_Members").WhereEqualTo("Staff_Is_Deleted", false).WhereEqualTo("Staff_Is_Active", true).WhereEqualTo("Staff_Email", CMD.Email);
        //            ObjQuerySnap = await docRef.GetSnapshotAsync();
        //            if (ObjQuerySnap != null)
        //            {
        //                if (ObjQuerySnap.Documents.Count > 0)
        //                {
        //                    Response.Where = "Staff";
        //                    Result = false;
        //                }
        //                else
        //                {
        //                    Result = true;
        //                }
        //            }
        //            else
        //            {
        //                Result = true;
        //            }
        //        }

        //        if (Result == true)
        //        {
        //            docRef = Db.Collection("MT_PatientInfomation").WhereEqualTo("Patient_Is_Deleted", false).WhereEqualTo("Patient_Is_Active", true).WhereEqualTo("Patient_Email", CMD.Email);
        //            ObjQuerySnap = await docRef.GetSnapshotAsync();
        //            if (ObjQuerySnap != null)
        //            {
        //                if (ObjQuerySnap.Documents.Count > 0)
        //                {
        //                    Response.Where = "Patient";
        //                    Result = false;
        //                }
        //                else
        //                {
        //                    Result = true;
        //                }
        //            }
        //            else
        //            {
        //                Result = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Status = con.StatusFailed;
        //        Response.Message = con.MessageFailed + ", Exception : " + ex.Message;
        //    }
        //    return ConvertToJSON(Response);
        //}
    }

    [FirestoreData]
    public class ComModel 
    { 
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }

    //public class CommodelResponse
    //{
    //    public string Where { get; set; }
    //    public Boolean Result { get; set; }
    //    public int Status { get; set; }
    //    public string Message { get; set; }
    //}

    public class CResponse { 
        public int SC { get; set; }
        public int PO { get; set; }
        public int Patient { get; set; }
        public int Booking { get; set; }
        public int CompleteBooking { get; set; }
        public int CancelledBooking { get; set; }
        public int VCComplete { get; set; }
        public int VCCancelled { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }

}
