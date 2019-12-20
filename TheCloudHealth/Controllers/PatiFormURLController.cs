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
    public class PatiFormURLController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public PatiFormURLController()
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

        [Route("API/PatiFormURL/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Patient_Forms_URLs PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PFURLResponse Response = new PFURLResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                PMD.PFU_Unique_ID = UniqueID;
                PMD.PFU_Actual_URL = "http://tchdev.thecloudhealth.com/#/in/" + PMD.PFU_Booking_ID + "/" + PMD.PFU_Form_ID;
                PMD.PFU_Dummy_URL = "http://tchdev.thecloudhealth.com/#/r/" + con.GetUrlToken();
                PMD.PFU_Is_Active = true;

                DocumentReference docRef = Db.Collection("MT_Patient_Forms_URLs").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(PMD);
                if (Result != null)
                {

                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    PMD.PFU_Actual_URL = "";
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

        [Route("API/PatiFormURL/GetDummyURL")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetDummyURL(MT_Patient_Forms_URLs PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PFURLResponse Response = new PFURLResponse();
            try
            {
                MT_Patient_Booking Pbooking = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Forms_URLs").WhereEqualTo("PFU_Is_Active", true).WhereEqualTo("PFU_Booking_ID", PMD.PFU_Booking_ID);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Response.DURL = ObjQuerySnap.Documents[0].ConvertTo<MT_Patient_Forms_URLs>().PFU_Dummy_URL;
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

        [Route("API/PatiFormURL/GetActualURL")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetActualURL(MT_Patient_Forms_URLs PMD)
        {
            Db = con.SurgeryCenterDb(PMD.Slug);
            PFURLResponse Response = new PFURLResponse();
            try
            {
                MT_Patient_Booking Pbooking = new MT_Patient_Booking();
                Query ObjQuery = Db.Collection("MT_Patient_Forms_URLs").WhereEqualTo("PFU_Is_Active", true).WhereEqualTo("PFU_Dummy_URL", PMD.PFU_Dummy_URL);
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    MT_Patient_Forms_URLs PFU = new MT_Patient_Forms_URLs();
                    foreach (DocumentSnapshot docsnap in ObjQuerySnap.Documents)
                    {
                        PFU = docsnap.ConvertTo<MT_Patient_Forms_URLs>();
                    }

                    Response.AURL = PFU.PFU_Actual_URL;
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
