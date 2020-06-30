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
    public class PhysicianOfficeController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        string UniqueSettingID = "";
        public PhysicianOfficeController()
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


        [Route("API/PhysicianOffice/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                List<MT_Specilities> SpelitiesList = new List<MT_Specilities>();
                UniqueID = con.GetUniqueKey();
                POMD.PhyO_Unique_ID = UniqueID;
                POMD.PhyO_Create_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Create_Date));
                POMD.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));

                if (POMD.PhyO_ContactSetting != null)
                {
                    foreach (MT_PhyO_Contact_Setting conset in POMD.PhyO_ContactSetting)
                    {
                        conset.PCS_Unique_ID = con.GetUniqueKey();
                    }
                }
                DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(POMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = POMD;
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

        [Route("API/PhysicianOffice/CheckSiteURLs")]
        [HttpPost]
        public async Task<Boolean> CheckSiteURLs(SiteURL POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            Boolean result = true;
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                Query SelectQuery = Db.Collection("MT_Physician_Office");
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in SelectSnap.Documents)
                    {
                        Surcenter = Docsnap.ConvertTo<MT_Physician_Office>();
                        if (Surcenter.PhyO_SiteURL != null)
                        {
                            if (Surcenter.PhyO_SiteURL.Site_URL.ToLower() == POMD.Site_URL.ToLower())
                            {
                                result = false;
                            }
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        [Route("API/PhysicianOffice/AddSiteURLs")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSiteURLs(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    Surcenter.PhyO_SiteURL = POMD.PhyO_SiteURL;

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = POMD;
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

        [Route("API/PhysicianOffice/AddAppearances")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddAppearances(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                Appearance Appear = new Appearance();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    if (POMD.PhyO_Appearance.App_NavigationColorDark_Hax != null && POMD.PhyO_Appearance.App_NavigationColorDark_Hax != "")
                    {
                        Appear.App_NavigationColorDark_Hax = POMD.PhyO_Appearance.App_NavigationColorDark_Hax;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorDark_R != null && POMD.PhyO_Appearance.App_NavigationColorDark_R != "")
                    {
                        Appear.App_NavigationColorDark_R = POMD.PhyO_Appearance.App_NavigationColorDark_R;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorDark_G != null && POMD.PhyO_Appearance.App_NavigationColorDark_G != "")
                    {
                        Appear.App_NavigationColorDark_G = POMD.PhyO_Appearance.App_NavigationColorDark_G;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorDark_B != null && POMD.PhyO_Appearance.App_NavigationColorDark_B != "")
                    {
                        Appear.App_NavigationColorDark_B = POMD.PhyO_Appearance.App_NavigationColorDark_B;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorDark_A != null && POMD.PhyO_Appearance.App_NavigationColorDark_A != "")
                    {
                        Appear.App_NavigationColorDark_A = POMD.PhyO_Appearance.App_NavigationColorDark_A;

                    }

                    if (POMD.PhyO_Appearance.App_NavigationColorLight_Hax != null && POMD.PhyO_Appearance.App_NavigationColorLight_Hax != "")
                    {
                        Appear.App_NavigationColorLight_Hax = POMD.PhyO_Appearance.App_NavigationColorLight_Hax;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorLight_R != null && POMD.PhyO_Appearance.App_NavigationColorLight_R != "")
                    {
                        Appear.App_NavigationColorLight_R = POMD.PhyO_Appearance.App_NavigationColorLight_R;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorLight_G != null && POMD.PhyO_Appearance.App_NavigationColorLight_G != "")
                    {
                        Appear.App_NavigationColorLight_G = POMD.PhyO_Appearance.App_NavigationColorLight_G;
                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorLight_B != null && POMD.PhyO_Appearance.App_NavigationColorLight_B != "")
                    {
                        Appear.App_NavigationColorLight_B = POMD.PhyO_Appearance.App_NavigationColorLight_B;

                    }
                    if (POMD.PhyO_Appearance.App_NavigationColorLight_A != null && POMD.PhyO_Appearance.App_NavigationColorLight_A != "")
                    {
                        Appear.App_NavigationColorLight_A = POMD.PhyO_Appearance.App_NavigationColorLight_A;
                    }


                    if (POMD.PhyO_Appearance.App_Hyperlinktext_Hax != null && POMD.PhyO_Appearance.App_Hyperlinktext_Hax != "")
                    {
                        Appear.App_Hyperlinktext_Hax = POMD.PhyO_Appearance.App_Hyperlinktext_Hax;
                    }
                    if (POMD.PhyO_Appearance.App_Hyperlinktext_R != null && POMD.PhyO_Appearance.App_Hyperlinktext_R != "")
                    {
                        Appear.App_Hyperlinktext_R = POMD.PhyO_Appearance.App_Hyperlinktext_R;
                    }
                    if (POMD.PhyO_Appearance.App_Hyperlinktext_G != null && POMD.PhyO_Appearance.App_Hyperlinktext_G != "")
                    {
                        Appear.App_Hyperlinktext_G = POMD.PhyO_Appearance.App_Hyperlinktext_G;
                    }
                    if (POMD.PhyO_Appearance.App_Hyperlinktext_B != null && POMD.PhyO_Appearance.App_Hyperlinktext_B != "")
                    {
                        Appear.App_Hyperlinktext_B = POMD.PhyO_Appearance.App_Hyperlinktext_B;
                    }
                    if (POMD.PhyO_Appearance.App_Hyperlinktext_A != null && POMD.PhyO_Appearance.App_Hyperlinktext_A != "")
                    {
                        Appear.App_Hyperlinktext_A = POMD.PhyO_Appearance.App_Hyperlinktext_A;
                    }


                    if (POMD.PhyO_Appearance.App_ButtonBackground_Hax != null && POMD.PhyO_Appearance.App_ButtonBackground_Hax != "")
                    {
                        Appear.App_ButtonBackground_Hax = POMD.PhyO_Appearance.App_ButtonBackground_Hax;

                    }
                    if (POMD.PhyO_Appearance.App_ButtonBackground_R != null && POMD.PhyO_Appearance.App_ButtonBackground_R != "")
                    {
                        Appear.App_ButtonBackground_R = POMD.PhyO_Appearance.App_ButtonBackground_R;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonBackground_G != null && POMD.PhyO_Appearance.App_ButtonBackground_G != "")
                    {
                        Appear.App_ButtonBackground_G = POMD.PhyO_Appearance.App_ButtonBackground_G;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonBackground_B != null && POMD.PhyO_Appearance.App_ButtonBackground_B != "")
                    {
                        Appear.App_ButtonBackground_B = POMD.PhyO_Appearance.App_ButtonBackground_B;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonBackground_A != null && POMD.PhyO_Appearance.App_ButtonBackground_A != "")
                    {
                        Appear.App_ButtonBackground_A = POMD.PhyO_Appearance.App_ButtonBackground_A;
                    }


                    if (POMD.PhyO_Appearance.App_ButtonText_Hax != null && POMD.PhyO_Appearance.App_ButtonText_Hax != "")
                    {
                        Appear.App_ButtonText_Hax = POMD.PhyO_Appearance.App_ButtonText_Hax;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonText_R != null && POMD.PhyO_Appearance.App_ButtonText_R != "")
                    {
                        Appear.App_ButtonText_R = POMD.PhyO_Appearance.App_ButtonText_R;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonText_G != null && POMD.PhyO_Appearance.App_ButtonText_G != "")
                    {
                        Appear.App_ButtonText_G = POMD.PhyO_Appearance.App_ButtonText_G;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonText_B != null && POMD.PhyO_Appearance.App_ButtonText_B != "")
                    {
                        Appear.App_ButtonText_B = POMD.PhyO_Appearance.App_ButtonText_B;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonText_A != null && POMD.PhyO_Appearance.App_ButtonText_A != "")
                    {
                        Appear.App_ButtonText_A = POMD.PhyO_Appearance.App_ButtonText_A;
                    }


                    if (POMD.PhyO_Appearance.App_ButtonMouseHover_Hax != null && POMD.PhyO_Appearance.App_ButtonMouseHover_Hax != "")
                    {
                        Appear.App_ButtonMouseHover_Hax = POMD.PhyO_Appearance.App_ButtonMouseHover_Hax;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonMouseHover_R != null && POMD.PhyO_Appearance.App_ButtonMouseHover_R != "")
                    {
                        Appear.App_ButtonMouseHover_R = POMD.PhyO_Appearance.App_ButtonMouseHover_R;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonMouseHover_G != null && POMD.PhyO_Appearance.App_ButtonMouseHover_G != "")
                    {
                        Appear.App_ButtonMouseHover_G = POMD.PhyO_Appearance.App_ButtonMouseHover_G;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonMouseHover_B != null && POMD.PhyO_Appearance.App_ButtonMouseHover_B != "")
                    {
                        Appear.App_ButtonMouseHover_B = POMD.PhyO_Appearance.App_ButtonMouseHover_B;
                    }
                    if (POMD.PhyO_Appearance.App_ButtonMouseHover_A != null && POMD.PhyO_Appearance.App_ButtonMouseHover_A != "")
                    {
                        Appear.App_ButtonMouseHover_A = POMD.PhyO_Appearance.App_ButtonMouseHover_A;
                    }


                    if (POMD.PhyO_Appearance.App_HyperlinkHoverText_Hax != null && POMD.PhyO_Appearance.App_HyperlinkHoverText_Hax != "")
                    {
                        Appear.App_HyperlinkHoverText_Hax = POMD.PhyO_Appearance.App_HyperlinkHoverText_Hax;

                    }
                    if (POMD.PhyO_Appearance.App_HyperlinkHoverText_R != null && POMD.PhyO_Appearance.App_HyperlinkHoverText_R != "")
                    {
                        Appear.App_HyperlinkHoverText_R = POMD.PhyO_Appearance.App_HyperlinkHoverText_R;
                    }
                    if (POMD.PhyO_Appearance.App_HyperlinkHoverText_G != null && POMD.PhyO_Appearance.App_HyperlinkHoverText_G != "")
                    {
                        Appear.App_HyperlinkHoverText_G = POMD.PhyO_Appearance.App_HyperlinkHoverText_G;

                    }
                    if (POMD.PhyO_Appearance.App_HyperlinkHoverText_B != null && POMD.PhyO_Appearance.App_HyperlinkHoverText_B != "")
                    {
                        Appear.App_HyperlinkHoverText_B = POMD.PhyO_Appearance.App_HyperlinkHoverText_B;

                    }
                    if (POMD.PhyO_Appearance.App_HyperlinkHoverText_A != null && POMD.PhyO_Appearance.App_HyperlinkHoverText_A != "")
                    {
                        Appear.App_HyperlinkHoverText_A = POMD.PhyO_Appearance.App_HyperlinkHoverText_A;
                    }


                    if (POMD.PhyO_Appearance.App_Title1Color_Hax != null && POMD.PhyO_Appearance.App_Title1Color_Hax != "")
                    {
                        Appear.App_Title1Color_Hax = POMD.PhyO_Appearance.App_Title1Color_Hax;
                    }
                    if (POMD.PhyO_Appearance.App_Title1Color_R != null && POMD.PhyO_Appearance.App_Title1Color_R != "")
                    {
                        Appear.App_Title1Color_R = POMD.PhyO_Appearance.App_Title1Color_R;
                    }
                    if (POMD.PhyO_Appearance.App_Title1Color_G != null && POMD.PhyO_Appearance.App_Title1Color_G != "")
                    {
                        Appear.App_Title1Color_G = POMD.PhyO_Appearance.App_Title1Color_G;
                    }
                    if (POMD.PhyO_Appearance.App_Title1Color_B != null && POMD.PhyO_Appearance.App_Title1Color_B != "")
                    {
                        Appear.App_Title1Color_B = POMD.PhyO_Appearance.App_Title1Color_B;
                    }
                    if (POMD.PhyO_Appearance.App_Title1Color_A != null && POMD.PhyO_Appearance.App_Title1Color_A != "")
                    {
                        Appear.App_Title1Color_A = POMD.PhyO_Appearance.App_Title1Color_A;
                    }


                    if (POMD.PhyO_Appearance.App_Title2Color_Hax != null && POMD.PhyO_Appearance.App_Title2Color_Hax != "")
                    {
                        Appear.App_Title2Color_Hax = POMD.PhyO_Appearance.App_Title2Color_Hax;
                    }
                    if (POMD.PhyO_Appearance.App_Title2Color_R != null && POMD.PhyO_Appearance.App_Title2Color_R != "")
                    {
                        Appear.App_Title2Color_R = POMD.PhyO_Appearance.App_Title2Color_R;
                    }
                    if (POMD.PhyO_Appearance.App_Title2Color_G != null && POMD.PhyO_Appearance.App_Title2Color_G != "")
                    {
                        Appear.App_Title2Color_G = POMD.PhyO_Appearance.App_Title2Color_G;
                    }
                    if (POMD.PhyO_Appearance.App_Title2Color_B != null && POMD.PhyO_Appearance.App_Title2Color_B != "")
                    {
                        Appear.App_Title2Color_B = POMD.PhyO_Appearance.App_Title2Color_B;
                    }
                    if (POMD.PhyO_Appearance.App_Title2Color_A != null && POMD.PhyO_Appearance.App_Title2Color_A != "")
                    {
                        Appear.App_Title2Color_A = POMD.PhyO_Appearance.App_Title2Color_A;
                    }


                    if (POMD.PhyO_Appearance.App_LoginBackgroundColor_Hax != null && POMD.PhyO_Appearance.App_LoginBackgroundColor_Hax != "")
                    {
                        Appear.App_LoginBackgroundColor_Hax = POMD.PhyO_Appearance.App_LoginBackgroundColor_Hax;

                    }
                    if (POMD.PhyO_Appearance.App_LoginBackgroundColor_R != null && POMD.PhyO_Appearance.App_LoginBackgroundColor_R != "")
                    {
                        Appear.App_LoginBackgroundColor_R = POMD.PhyO_Appearance.App_LoginBackgroundColor_R;
                    }
                    if (POMD.PhyO_Appearance.App_LoginBackgroundColor_G != null && POMD.PhyO_Appearance.App_LoginBackgroundColor_G != "")
                    {
                        Appear.App_LoginBackgroundColor_G = POMD.PhyO_Appearance.App_LoginBackgroundColor_G;
                    }
                    if (POMD.PhyO_Appearance.App_LoginBackgroundColor_B != null && POMD.PhyO_Appearance.App_LoginBackgroundColor_B != "")
                    {
                        Appear.App_LoginBackgroundColor_B = POMD.PhyO_Appearance.App_LoginBackgroundColor_B;
                    }
                    if (POMD.PhyO_Appearance.App_LoginBackgroundColor_A != null && POMD.PhyO_Appearance.App_LoginBackgroundColor_A != "")
                    {
                        Appear.App_LoginBackgroundColor_A = POMD.PhyO_Appearance.App_LoginBackgroundColor_A;
                    }


                    if (POMD.PhyO_Appearance.App_LoginTextColor_Hax != null && POMD.PhyO_Appearance.App_LoginTextColor_Hax != "")
                    {
                        Appear.App_LoginTextColor_Hax = POMD.PhyO_Appearance.App_LoginTextColor_Hax;
                    }
                    if (POMD.PhyO_Appearance.App_LoginTextColor_R != null && POMD.PhyO_Appearance.App_LoginTextColor_R != "")
                    {
                        Appear.App_LoginTextColor_R = POMD.PhyO_Appearance.App_LoginTextColor_R;
                    }
                    if (POMD.PhyO_Appearance.App_LoginTextColor_G != null && POMD.PhyO_Appearance.App_LoginTextColor_G != "")
                    {
                        Appear.App_LoginTextColor_G = POMD.PhyO_Appearance.App_LoginTextColor_G;
                    }
                    if (POMD.PhyO_Appearance.App_LoginTextColor_B != null && POMD.PhyO_Appearance.App_LoginTextColor_B != "")
                    {
                        Appear.App_LoginTextColor_B = POMD.PhyO_Appearance.App_LoginTextColor_B;
                    }
                    if (POMD.PhyO_Appearance.App_LoginTextColor_A != null && POMD.PhyO_Appearance.App_LoginTextColor_A != "")
                    {
                        Appear.App_LoginTextColor_A = POMD.PhyO_Appearance.App_LoginTextColor_A;
                    }

                    if (POMD.PhyO_Appearance.App_LoginButtonColor_Hax != null && POMD.PhyO_Appearance.App_LoginButtonColor_Hax != "")
                    {
                        Appear.App_LoginButtonColor_Hax = POMD.PhyO_Appearance.App_LoginButtonColor_Hax;

                    }
                    if (POMD.PhyO_Appearance.App_LoginButtonColor_R != null && POMD.PhyO_Appearance.App_LoginButtonColor_R != "")
                    {
                        Appear.App_LoginButtonColor_R = POMD.PhyO_Appearance.App_LoginButtonColor_R;
                    }
                    if (POMD.PhyO_Appearance.App_LoginButtonColor_G != null && POMD.PhyO_Appearance.App_LoginButtonColor_G != "")
                    {
                        Appear.App_LoginButtonColor_G = POMD.PhyO_Appearance.App_LoginButtonColor_G;
                    }
                    if (POMD.PhyO_Appearance.App_LoginButtonColor_B != null && POMD.PhyO_Appearance.App_LoginButtonColor_B != "")
                    {
                        Appear.App_LoginButtonColor_B = POMD.PhyO_Appearance.App_LoginButtonColor_B;
                    }
                    if (POMD.PhyO_Appearance.App_LoginButtonColor_A != null && POMD.PhyO_Appearance.App_LoginButtonColor_A != "")
                    {
                        Appear.App_LoginButtonColor_A = POMD.PhyO_Appearance.App_LoginButtonColor_A;
                    }
                    Surcenter.PhyO_Appearance = Appear;

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = Surcenter;
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


        public MT_Physician_Office GetImages()
        {
            MT_Physician_Office Surcenter = new MT_Physician_Office();
            try
            {
                Dictionary<string, string> prefix =
              new Dictionary<string, string>(){
                                  {"file1", "POLogo_"},
                                  {"file2", "PONavi_"},
                                {"file3", "POFav_"} };
                var httpRequest = HttpContext.Current.Request;
                var postedData = httpRequest.Form[0];
                string str = postedData.Substring(1, postedData.Length - 2);
                JObject jobject = JObject.Parse(str);
                Surcenter = JsonConvert.DeserializeObject<MT_Physician_Office>(jobject.ToString());
                Logo logo = new Logo();

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        int hasheddate = DateTime.Now.GetHashCode();

                        //Good to use an updated name always, since many can use the same file name to upload.
                        string changed_name = prefix[file].ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + postedFile.FileName.Substring(postedFile.FileName.IndexOf('.'), (postedFile.FileName.Length - postedFile.FileName.IndexOf('.')));

                        var filePath = HttpContext.Current.Server.MapPath("~/images/" + changed_name);
                        postedFile.SaveAs(filePath); // save the file to a folder "Images" in the root of your app

                        changed_name = @"~/images/" + changed_name; //store this complete path to database
                        if (file == "file1")
                        {
                            logo.Logo_Login_Image = changed_name;
                        }
                        else if (file == "file2")
                        {
                            logo.Logo_Navigation_Image = changed_name;
                        }
                        else if (file == "file3")
                        {
                            logo.Logo_Fav_Image = changed_name;
                        }
                    }
                    Surcenter.PhyO_Logo = logo;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Surcenter;
        }


        [Route("API/PhysicianOffice/AddLogos")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddLogos()
        {
            MT_Physician_Office POMD = GetImages();
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                Logo logo = new Logo();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    if (POMD.PhyO_Logo != null)
                    {
                        if (POMD.PhyO_Logo.Logo_Login_Image != null && POMD.PhyO_Logo.Logo_Login_Image != "")
                        {
                            logo.Logo_Login_Image = POMD.PhyO_Logo.Logo_Login_Image;
                        }
                        else
                        {
                            if (Surcenter.PhyO_Logo != null)
                            {
                                logo.Logo_Login_Image = Surcenter.PhyO_Logo.Logo_Login_Image;
                            }
                            
                        }
                    }
                    if (POMD.PhyO_Logo != null)
                    {
                        if (POMD.PhyO_Logo.Logo_Navigation_Image != null && POMD.PhyO_Logo.Logo_Navigation_Image != "")
                        {
                            logo.Logo_Navigation_Image = POMD.PhyO_Logo.Logo_Navigation_Image;
                        }
                        else
                        {
                            if (Surcenter.PhyO_Logo != null)
                            {
                                logo.Logo_Navigation_Image = Surcenter.PhyO_Logo.Logo_Navigation_Image;
                            }
                        }
                    }
                    if (POMD.PhyO_Logo != null)
                    {
                        if (POMD.PhyO_Logo.Logo_Fav_Image != null && POMD.PhyO_Logo.Logo_Fav_Image != "")
                        {
                            logo.Logo_Fav_Image = POMD.PhyO_Logo.Logo_Fav_Image;
                        }
                        else
                        {
                            if (Surcenter.PhyO_Logo != null)
                            {
                                logo.Logo_Fav_Image = Surcenter.PhyO_Logo.Logo_Fav_Image;
                            }
                        }
                    }

                    if (POMD.PhyO_Logo != null)
                    {
                        Surcenter.PhyO_Logo = logo;
                    }

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = Surcenter;
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

        public MT_Physician_Office GetSliderImages()
        {
            string[] prefix = new string[] { "POSliderBI_", "POSliderI_" };
            string[] DBPath = new string[2];
            int i = 0;
            var httpRequest = HttpContext.Current.Request;
            var postedDataSurgC = httpRequest.Form[0];
            var postedDataSlider = httpRequest.Form[1];
            string strSurgC = postedDataSurgC.Substring(1, postedDataSurgC.Length - 2);
            string strSlider = postedDataSlider.Substring(1, postedDataSlider.Length - 2);

            JObject jobject = JObject.Parse(strSurgC);
            MT_Physician_Office Surcenter = JsonConvert.DeserializeObject<MT_Physician_Office>(jobject.ToString());

            jobject = JObject.Parse(strSlider);
            Slider slider = JsonConvert.DeserializeObject<Slider>(jobject.ToString());
            slider.Slider_Unique_ID = con.GetUniqueKey();
            slider.Slider_Creted_By = Surcenter.PhyO_Created_By;
            slider.Slider_User_Name = Surcenter.PhyO_User_Name;
            slider.Slider_Create_Date = con.ConvertTimeZone(Surcenter.PhyO_TimeZone, Convert.ToDateTime(Surcenter.PhyO_Modify_Date));
            slider.Slider_Modify_Date = con.ConvertTimeZone(Surcenter.PhyO_TimeZone, Convert.ToDateTime(Surcenter.PhyO_Modify_Date));
            slider.Slider_Is_Active = true;
            slider.Slider_Is_Deleted = false;

            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    int hasheddate = DateTime.Now.GetHashCode();

                    //Good to use an updated name always, since many can use the same file name to upload.
                    string changed_name = prefix[i].ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + postedFile.FileName.Substring(postedFile.FileName.IndexOf('.'), (postedFile.FileName.Length - postedFile.FileName.IndexOf('.')));

                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + changed_name);
                    postedFile.SaveAs(filePath); // save the file to a folder "Images" in the root of your app

                    changed_name = @"~\Images\" + changed_name; //store this complete path to database
                    DBPath[i] = changed_name;
                    i++;
                }
                if (DBPath[0] != null)
                {
                    slider.Slider_Background_Image = DBPath[0].ToString();
                }

                if (DBPath[1] != null)
                {
                    slider.Slider_Slider_Image = DBPath[1].ToString();
                }
                Surcenter.PhyO_Slider = slider;
            }
            return Surcenter;
        }

        [Route("API/PhysicianOffice/AddSliders")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSliders()
        {
            MT_Physician_Office POMD = GetSliderImages();
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                List<Slider> SliderList = new List<Slider>();
                Query SelectQuery = con.Db().Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    if (Surcenter.PhyO_SliderList != null)
                    {
                        foreach (Slider slid in Surcenter.PhyO_SliderList)
                        {
                            SliderList.Add(slid);
                        }
                    }
                    SliderList.Add(POMD.PhyO_Slider);
                    Surcenter.PhyO_SliderList = SliderList;

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = POMD;
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

        [Route("API/PhysicianOffice/ActiveSliders")]
        [HttpPost]
        public async Task<HttpResponseMessage> ActiveSliders(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                List<Slider> SliderList = new List<Slider>();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    if (Surcenter.PhyO_SliderList != null)
                    {
                        foreach (Slider slid in Surcenter.PhyO_SliderList)
                        {
                            if (slid.Slider_Unique_ID == POMD.PhyO_Slider.Slider_Unique_ID)
                            {
                                slid.Slider_Is_Active = POMD.PhyO_Slider.Slider_Is_Active;
                                slid.Slider_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                            }
                            SliderList.Add(slid);
                        }
                    }
                    Surcenter.PhyO_SliderList = SliderList;

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = POMD;
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

        [Route("API/PhysicianOffice/DeleteSliders")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteSliders(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                List<Slider> SliderList = new List<Slider>();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    if (Surcenter.PhyO_SliderList != null)
                    {
                        foreach (Slider slid in Surcenter.PhyO_SliderList)
                        {
                            if (slid.Slider_Unique_ID != POMD.PhyO_Slider.Slider_Unique_ID)
                            {
                                SliderList.Add(slid);
                            }
                            
                        }
                    }

                    Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        {"PhyO_SliderList", SliderList},
                        {"PhyO_Modify_Date", con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date))},
                        {"PhyO_TimeZone", POMD.PhyO_TimeZone}
                    };

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.UpdateAsync(initialData);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = POMD;
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


        [Route("API/PhysicianOffice/AddFooters")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddFooters(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                Footer footer = new Footer();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    footer = POMD.PhyO_Footer;
                    Surcenter.PhyO_Footer = footer;

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = POMD;
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


        [Route("API/PhysicianOffice/AddMiscellaneouss")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddMiscellaneouss(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office Surcenter = new MT_Physician_Office();
                Query SelectQuery = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Surcenter.PhyO_Modify_Date = con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date));
                    Surcenter.PhyO_Miscellaneous = POMD.PhyO_Miscellaneous;

                    DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = POMD;
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

        [Route("API/PhysicianOffice/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                    {
                        { "PhyO_Name", POMD.PhyO_Name },
                        { "PhyO_DBA_Name", POMD.PhyO_DBA_Name },
                        { "PhyO_Address", POMD.PhyO_Address },
                        { "PhyO_City", POMD.PhyO_City },
                        { "PhyO_State", POMD.PhyO_State },
                        { "PhyO_Country", POMD.PhyO_Country },
                        { "PhyO_Zip", POMD.PhyO_Zip },
                        { "PhyO_Landline", POMD.PhyO_Landline },
                        { "PhyO_FaxNo", POMD.PhyO_FaxNo },
                        { "PhyO_AlternateNo", POMD.PhyO_AlternateNo },
                        { "PhyO_MobileNo", POMD.PhyO_MobileNo },
                        { "PhyO_Helpline", POMD.PhyO_Helpline },
                        { "PhyO_Website_URL", POMD.PhyO_Website_URL },
                        { "PhyO_Modify_Date",con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date))},
                        { "PhyO_TimeZone",POMD.PhyO_TimeZone}
                    };

                DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = POMD;
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

        [Route("API/PhysicianOffice/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "PhyO_Modify_Date",con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date))},
                    { "PhyO_Is_Active",POMD.PhyO_Is_Active}
                };
                DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = POMD;
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

        [Route("API/PhysicianOffice/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                Dictionary<string, object> initialData = new Dictionary<string, object>
                {
                    { "PhyO_Modify_Date",con.ConvertTimeZone(POMD.PhyO_TimeZone, Convert.ToDateTime(POMD.PhyO_Modify_Date))},
                    { "PhyO_Is_Deleted",POMD.PhyO_Is_Deleted}
                };
                DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = POMD;
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

        [Route("API/PhysicianOffice/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = POMD;
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

        [Route("API/PhysicianOffice/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                MT_Physician_Office PO = new MT_Physician_Office();
                Query docRef = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID).WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    PO = ObjQuerySnap.Documents[0].ConvertTo<MT_Physician_Office>();
                    Response.Data = PO;
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

        [Route("API/PhysicianOffice/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                List<MT_Physician_Office> AnesList = new List<MT_Physician_Office>();
                Query docRef = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Physician_Office>());
                    }
                    Response.DataList = AnesList.OrderByDescending(o => o.PhyO_Modify_Date).ToList();
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


        [Route("API/PhysicianOffice/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                List<MT_Physician_Office> AnesList = new List<MT_Physician_Office>();
                Query docRef = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", false).WhereEqualTo("PhyO_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Physician_Office>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.PhyO_Name).ToList();
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


        [Route("API/PhysicianOffice/GetPoListFilterWithSC")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetPoListFilterWithSC(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                List<MT_Physician_Office> AnesList = new List<MT_Physician_Office>();
                Query docRef = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", false).WhereEqualTo("PhyO_Surgery_Center_ID", POMD.PhyO_Surgery_Center_ID);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Physician_Office>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.PhyO_Name).ToList();
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

        [Route("API/PhysicianOffice/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {
                List<MT_Physician_Office> AnesList = new List<MT_Physician_Office>();
                Query docRef = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Physician_Office>());
                    }
                    Response.DataList = AnesList;
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

        [Route("API/PhysicianOffice/AddSetting")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> AddSetting(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {

                MT_Physician_Office Equip = new MT_Physician_Office();
                List<MT_PhyO_Contact_Setting> settinglist = new List<MT_PhyO_Contact_Setting>();
                Query docRef1 = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", false).WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID);
                QuerySnapshot ObjQuerySnap1 = await docRef1.GetSnapshotAsync();
                if (ObjQuerySnap1 != null)
                {
                    Equip = ObjQuerySnap1.Documents[0].ConvertTo<MT_Physician_Office>();
                    if (POMD.PhyO_ContactSetting != null)
                    {
                        foreach (MT_PhyO_Contact_Setting SCS in POMD.PhyO_ContactSetting)
                        {
                            UniqueSettingID = con.GetUniqueKey();
                            SCS.PCS_Unique_ID = UniqueSettingID;
                            settinglist.Add(SCS);
                        }
                    }
                    if (Equip.PhyO_ContactSetting != null)
                    {
                        foreach (MT_PhyO_Contact_Setting SCSetting in Equip.PhyO_ContactSetting)
                        {
                            settinglist.Add(SCSetting);
                        }
                    }

                    Equip.PhyO_ContactSetting = settinglist;
                    DocumentReference docRef2 = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef2.SetAsync(Equip, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = Equip;
                    }
                    else
                    {
                        Response.Status = con.StatusNotUpdate;
                        Response.Message = con.MessageNotUpdate;
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

        [Route("API/PhysicianOffice/DeleteSetting")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> DeleteSetting(MT_Physician_Office POMD)
        {
            Db = con.SurgeryCenterDb(POMD.Slug);
            PhysicianOfficeResponse Response = new PhysicianOfficeResponse();
            try
            {

                MT_Physician_Office Equip = new MT_Physician_Office();
                List<MT_PhyO_Contact_Setting> settinglist = new List<MT_PhyO_Contact_Setting>();
                Query docRef1 = Db.Collection("MT_Physician_Office").WhereEqualTo("PhyO_Is_Deleted", false).WhereEqualTo("PhyO_Unique_ID", POMD.PhyO_Unique_ID);
                QuerySnapshot ObjQuerySnap1 = await docRef1.GetSnapshotAsync();
                if (ObjQuerySnap1 != null)
                {
                    Equip = ObjQuerySnap1.Documents[0].ConvertTo<MT_Physician_Office>();
                    if (Equip.PhyO_ContactSetting != null)
                    {
                        foreach (MT_PhyO_Contact_Setting SCSetting in Equip.PhyO_ContactSetting)
                        {
                            if (SCSetting.PCS_Unique_ID != POMD.PhyO_ContactSetting[0].PCS_Unique_ID)
                                settinglist.Add(SCSetting);
                        }
                    }

                    Equip.PhyO_ContactSetting = settinglist;
                    DocumentReference docRef2 = Db.Collection("MT_Physician_Office").Document(POMD.PhyO_Unique_ID);
                    WriteResult Result = await docRef2.SetAsync(Equip, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = Equip;
                    }
                    else
                    {
                        Response.Status = con.StatusNotUpdate;
                        Response.Message = con.MessageNotUpdate;
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
