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
using System.ServiceModel.Channels;
using System.Web;

namespace TheCloudHealth.Controllers
{
    public class LogController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        string IPAddress = "";
        public LogController()
        {
            con = new ConnectionClass();
            Db = con.Db();
        }

        public HttpResponseMessage ConvertToJSON(object objectToConvert)
        {
            var jObject = JsonConvert.SerializeObject(objectToConvert);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jObject, Encoding.UTF8, "application/json");
            return response;
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        [Route("API/Log/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> Create(MT_Log_Book LBMD)
        {
            Db = con.SurgeryCenterDb(LBMD.Slug);
            LogResponse Response = new LogResponse();
            try
            {
                UniqueID = con.GetUniqueKey();
                LBMD.Unique_ID = UniqueID;
                LBMD.Ip_Address = GetClientIp();
                LBMD.Operation_Time = con.ConvertTimeZone(LBMD.TimeZone, Convert.ToDateTime(LBMD.Operation_Time));
                DocumentReference docRef = Db.Collection("MT_Log_Book").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(LBMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = LBMD;
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

        [Route("API/Log/List")]
        [HttpPost]
        public async Task<HttpResponseMessage> List(MT_Log_Book LBMD)
        {
            Db = con.SurgeryCenterDb(LBMD.Slug);
            LogResponse Response = new LogResponse();
            try
            {
                List<MT_Log_Book> PMList = new List<MT_Log_Book>();
                Query ObjQuery = Db.Collection("MT_Log_Book");
                QuerySnapshot ObjQuerySnap = await ObjQuery.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in ObjQuerySnap.Documents)
                    {
                        PMList.Add(Docsnap.ConvertTo<MT_Log_Book>());
                    }

                    Response.DataList = PMList;
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
