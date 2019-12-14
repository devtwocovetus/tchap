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
using System.Globalization;
using System.Linq;

namespace TheCloudHealth.Controllers
{
    public class EquipmentsController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        public EquipmentsController()
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

        [Route("API/Equipments/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {



                UniqueID = con.GetUniqueKey();
                EMD.Equip_Unique_ID = UniqueID;
                EMD.Equip_Create_Date = con.ConvertTimeZone(EMD.Equip_TimeZone, Convert.ToDateTime(EMD.Equip_Create_Date));
                EMD.Equip_Modify_Date = con.ConvertTimeZone(EMD.Equip_TimeZone, Convert.ToDateTime(EMD.Equip_Modify_Date));

                DocumentReference docRef = Db.Collection("MT_Equipment").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(EMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
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

        [Route("API/Equipments/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Equip_Type", EMD.Equip_Type },
                    { "Equip_Name", EMD.Equip_Name },
                    { "Equip_Description", EMD.Equip_Description },
                    { "Equip_Modify_Date",con.ConvertTimeZone(EMD.Equip_TimeZone,Convert.ToDateTime(EMD.Equip_Modify_Date)) },
                    { "Equip_Surgery_Physician_Id",EMD.Equip_Surgery_Physician_Id}
                };

                DocumentReference docRef = Db.Collection("MT_Equipment").Document(EMD.Equip_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
                }
                else
                {
                    Response.Status = con.StatusNotUpdate;
                    Response.Message = con.MessageNotUpdate;
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

        [Route("API/Equipments/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Equip_Modify_Date",con.ConvertTimeZone(EMD.Equip_TimeZone,Convert.ToDateTime(EMD.Equip_Modify_Date)) },
                    { "Equip_Is_Active",EMD.Equip_Is_Active}
                };

                DocumentReference docRef = Db.Collection("MT_Equipment").Document(EMD.Equip_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
                }
                else
                {
                    Response.Status = con.StatusNotUpdate;
                    Response.Message = con.MessageNotUpdate;
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


        [Route("API/Equipments/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "Equip_Modify_Date",con.ConvertTimeZone(EMD.Equip_TimeZone,Convert.ToDateTime(EMD.Equip_Modify_Date)) },
                    { "Equip_Is_Deleted",EMD.Equip_Is_Deleted}
                };

                DocumentReference docRef = Db.Collection("MT_Equipment").Document(EMD.Equip_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = EMD;
                }
                else
                {
                    Response.Status = con.StatusNotUpdate;
                    Response.Message = con.MessageNotUpdate;
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

        [Route("API/Equipments/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Equipment").Document(EMD.Equip_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = null;
                }
                else
                {
                    Response.Status = con.StatusNotDeleted;
                    Response.Message = con.MessageNotDeleted;
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


        [Route("API/Equipments/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                MT_Equipment Equip = new MT_Equipment();
                Query docRef = Db.Collection("MT_Equipment").WhereEqualTo("Equip_Unique_ID", EMD.Equip_Unique_ID).WhereEqualTo("Equip_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    Equip = ObjQuerySnap.Documents[0].ConvertTo<MT_Equipment>();
                    Response.Data = Equip;
                }
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

        [Route("API/Equipments/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                List<MT_Equipment> AnesList = new List<MT_Equipment>();
                Query docRef = Db.Collection("MT_Equipment").WhereEqualTo("Equip_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Equipment>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Equip_Name).ToList();
                }
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

        [Route("API/Equipments/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                List<MT_Equipment> AnesList = new List<MT_Equipment>();
                Query docRef = Db.Collection("MT_Equipment").WhereEqualTo("Equip_Is_Deleted", false).WhereEqualTo("Equip_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Equipment>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Equip_Name).ToList();
                }
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



        [Route("API/Equipments/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Equipment EMD)
        {
            Db = con.SurgeryCenterDb(EMD.Slug);
            EquipmentResponse Response = new EquipmentResponse();
            try
            {
                List<MT_Equipment> AnesList = new List<MT_Equipment>();
                Query docRef = Db.Collection("MT_Equipment").WhereEqualTo("Equip_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Equipment>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.Equip_Name).ToList();
                }
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
    }
}
