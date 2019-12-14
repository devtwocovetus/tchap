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
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace TheCloudHealth.Controllers
{
    public class SurgeryCenterController : ApiController
    {
        ConnectionClass con;
        FirestoreDb Db;
        string UniqueID = "";
        string UniqueSettingID = "";
        public SurgeryCenterController()
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

        [Route("API/SurgeryCenter/Create")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateAsync(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {

                UniqueID = con.GetUniqueKey();
                SCMD.SurgC_Unique_ID = UniqueID;
                SCMD.SurgC_Create_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Create_Date));
                SCMD.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                if (SCMD.SurgC_ContactSetting != null)
                {
                    foreach (MT_SurgC_Contact_Setting conset in SCMD.SurgC_ContactSetting)
                    {
                        conset.SCS_Unique_ID = con.GetUniqueKey();
                    }
                }

                DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(UniqueID);
                WriteResult Result = await docRef.SetAsync(SCMD);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/CheckSiteURL")]
        [HttpPost]
        public async Task<Boolean> CheckSiteURL(SiteURL SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            Boolean result = true;
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                Query SelectQuery = Db.Collection("MT_Surgery_Center");
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    foreach (DocumentSnapshot Docsnap in SelectSnap.Documents)
                    {
                        Surcenter = Docsnap.ConvertTo<MT_Surgery_Center>();
                        if (Surcenter.SurgC_SiteURL != null)
                        {
                            if (Surcenter.SurgC_SiteURL.Site_URL.ToLower() == SCMD.Site_URL.ToLower())
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
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        [Route("API/SurgeryCenter/AddSiteURL")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSiteURL(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    Surcenter.SurgC_SiteURL = SCMD.SurgC_SiteURL;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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


        [Route("API/SurgeryCenter/AddAppearance")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddAppearance(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                Appearance Appear = new Appearance();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));

                    if (SCMD.SurgC_Appearance.App_NavigationColorDark_Hax != null && SCMD.SurgC_Appearance.App_NavigationColorDark_Hax != "")
                    {
                        Appear.App_NavigationColorDark_Hax = SCMD.SurgC_Appearance.App_NavigationColorDark_Hax;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorDark_R != null && SCMD.SurgC_Appearance.App_NavigationColorDark_R != "")
                    {
                        Appear.App_NavigationColorDark_R = SCMD.SurgC_Appearance.App_NavigationColorDark_R;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorDark_G != null && SCMD.SurgC_Appearance.App_NavigationColorDark_G != "")
                    {
                        Appear.App_NavigationColorDark_G = SCMD.SurgC_Appearance.App_NavigationColorDark_G;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorDark_B != null && SCMD.SurgC_Appearance.App_NavigationColorDark_B != "")
                    {
                        Appear.App_NavigationColorDark_B = SCMD.SurgC_Appearance.App_NavigationColorDark_B;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorDark_A != null && SCMD.SurgC_Appearance.App_NavigationColorDark_A != "")
                    {
                        Appear.App_NavigationColorDark_A = SCMD.SurgC_Appearance.App_NavigationColorDark_A;

                    }

                    if (SCMD.SurgC_Appearance.App_NavigationColorLight_Hax != null && SCMD.SurgC_Appearance.App_NavigationColorLight_Hax != "")
                    {
                        Appear.App_NavigationColorLight_Hax = SCMD.SurgC_Appearance.App_NavigationColorLight_Hax;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorLight_R != null && SCMD.SurgC_Appearance.App_NavigationColorLight_R != "")
                    {
                        Appear.App_NavigationColorLight_R = SCMD.SurgC_Appearance.App_NavigationColorLight_R;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorLight_G != null && SCMD.SurgC_Appearance.App_NavigationColorLight_G != "")
                    {
                        Appear.App_NavigationColorLight_G = SCMD.SurgC_Appearance.App_NavigationColorLight_G;
                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorLight_B != null && SCMD.SurgC_Appearance.App_NavigationColorLight_B != "")
                    {
                        Appear.App_NavigationColorLight_B = SCMD.SurgC_Appearance.App_NavigationColorLight_B;

                    }
                    if (SCMD.SurgC_Appearance.App_NavigationColorLight_A != null && SCMD.SurgC_Appearance.App_NavigationColorLight_A != "")
                    {
                        Appear.App_NavigationColorLight_A = SCMD.SurgC_Appearance.App_NavigationColorLight_A;
                    }


                    if (SCMD.SurgC_Appearance.App_Hyperlinktext_Hax != null && SCMD.SurgC_Appearance.App_Hyperlinktext_Hax != "")
                    {
                        Appear.App_Hyperlinktext_Hax = SCMD.SurgC_Appearance.App_Hyperlinktext_Hax;
                    }
                    if (SCMD.SurgC_Appearance.App_Hyperlinktext_R != null && SCMD.SurgC_Appearance.App_Hyperlinktext_R != "")
                    {
                        Appear.App_Hyperlinktext_R = SCMD.SurgC_Appearance.App_Hyperlinktext_R;
                    }
                    if (SCMD.SurgC_Appearance.App_Hyperlinktext_G != null && SCMD.SurgC_Appearance.App_Hyperlinktext_G != "")
                    {
                        Appear.App_Hyperlinktext_G = SCMD.SurgC_Appearance.App_Hyperlinktext_G;
                    }
                    if (SCMD.SurgC_Appearance.App_Hyperlinktext_B != null && SCMD.SurgC_Appearance.App_Hyperlinktext_B != "")
                    {
                        Appear.App_Hyperlinktext_B = SCMD.SurgC_Appearance.App_Hyperlinktext_B;
                    }
                    if (SCMD.SurgC_Appearance.App_Hyperlinktext_A != null && SCMD.SurgC_Appearance.App_Hyperlinktext_A != "")
                    {
                        Appear.App_Hyperlinktext_A = SCMD.SurgC_Appearance.App_Hyperlinktext_A;
                    }


                    if (SCMD.SurgC_Appearance.App_ButtonBackground_Hax != null && SCMD.SurgC_Appearance.App_ButtonBackground_Hax != "")
                    {
                        Appear.App_ButtonBackground_Hax = SCMD.SurgC_Appearance.App_ButtonBackground_Hax;

                    }
                    if (SCMD.SurgC_Appearance.App_ButtonBackground_R != null && SCMD.SurgC_Appearance.App_ButtonBackground_R != "")
                    {
                        Appear.App_ButtonBackground_R = SCMD.SurgC_Appearance.App_ButtonBackground_R;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonBackground_G != null && SCMD.SurgC_Appearance.App_ButtonBackground_G != "")
                    {
                        Appear.App_ButtonBackground_G = SCMD.SurgC_Appearance.App_ButtonBackground_G;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonBackground_B != null && SCMD.SurgC_Appearance.App_ButtonBackground_B != "")
                    {
                        Appear.App_ButtonBackground_B = SCMD.SurgC_Appearance.App_ButtonBackground_B;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonBackground_A != null && SCMD.SurgC_Appearance.App_ButtonBackground_A != "")
                    {
                        Appear.App_ButtonBackground_A = SCMD.SurgC_Appearance.App_ButtonBackground_A;
                    }


                    if (SCMD.SurgC_Appearance.App_ButtonText_Hax != null && SCMD.SurgC_Appearance.App_ButtonText_Hax != "")
                    {
                        Appear.App_ButtonText_Hax = SCMD.SurgC_Appearance.App_ButtonText_Hax;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonText_R != null && SCMD.SurgC_Appearance.App_ButtonText_R != "")
                    {
                        Appear.App_ButtonText_R = SCMD.SurgC_Appearance.App_ButtonText_R;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonText_G != null && SCMD.SurgC_Appearance.App_ButtonText_G != "")
                    {
                        Appear.App_ButtonText_G = SCMD.SurgC_Appearance.App_ButtonText_G;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonText_B != null && SCMD.SurgC_Appearance.App_ButtonText_B != "")
                    {
                        Appear.App_ButtonText_B = SCMD.SurgC_Appearance.App_ButtonText_B;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonText_A != null && SCMD.SurgC_Appearance.App_ButtonText_A != "")
                    {
                        Appear.App_ButtonText_A = SCMD.SurgC_Appearance.App_ButtonText_A;
                    }


                    if (SCMD.SurgC_Appearance.App_ButtonMouseHover_Hax != null && SCMD.SurgC_Appearance.App_ButtonMouseHover_Hax != "")
                    {
                        Appear.App_ButtonMouseHover_Hax = SCMD.SurgC_Appearance.App_ButtonMouseHover_Hax;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonMouseHover_R != null && SCMD.SurgC_Appearance.App_ButtonMouseHover_R != "")
                    {
                        Appear.App_ButtonMouseHover_R = SCMD.SurgC_Appearance.App_ButtonMouseHover_R;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonMouseHover_G != null && SCMD.SurgC_Appearance.App_ButtonMouseHover_G != "")
                    {
                        Appear.App_ButtonMouseHover_G = SCMD.SurgC_Appearance.App_ButtonMouseHover_G;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonMouseHover_B != null && SCMD.SurgC_Appearance.App_ButtonMouseHover_B != "")
                    {
                        Appear.App_ButtonMouseHover_B = SCMD.SurgC_Appearance.App_ButtonMouseHover_B;
                    }
                    if (SCMD.SurgC_Appearance.App_ButtonMouseHover_A != null && SCMD.SurgC_Appearance.App_ButtonMouseHover_A != "")
                    {
                        Appear.App_ButtonMouseHover_A = SCMD.SurgC_Appearance.App_ButtonMouseHover_A;
                    }


                    if (SCMD.SurgC_Appearance.App_HyperlinkHoverText_Hax != null && SCMD.SurgC_Appearance.App_HyperlinkHoverText_Hax != "")
                    {
                        Appear.App_HyperlinkHoverText_Hax = SCMD.SurgC_Appearance.App_HyperlinkHoverText_Hax;

                    }
                    if (SCMD.SurgC_Appearance.App_HyperlinkHoverText_R != null && SCMD.SurgC_Appearance.App_HyperlinkHoverText_R != "")
                    {
                        Appear.App_HyperlinkHoverText_R = SCMD.SurgC_Appearance.App_HyperlinkHoverText_R;
                    }
                    if (SCMD.SurgC_Appearance.App_HyperlinkHoverText_G != null && SCMD.SurgC_Appearance.App_HyperlinkHoverText_G != "")
                    {
                        Appear.App_HyperlinkHoverText_G = SCMD.SurgC_Appearance.App_HyperlinkHoverText_G;

                    }
                    if (SCMD.SurgC_Appearance.App_HyperlinkHoverText_B != null && SCMD.SurgC_Appearance.App_HyperlinkHoverText_B != "")
                    {
                        Appear.App_HyperlinkHoverText_B = SCMD.SurgC_Appearance.App_HyperlinkHoverText_B;

                    }
                    if (SCMD.SurgC_Appearance.App_HyperlinkHoverText_A != null && SCMD.SurgC_Appearance.App_HyperlinkHoverText_A != "")
                    {
                        Appear.App_HyperlinkHoverText_A = SCMD.SurgC_Appearance.App_HyperlinkHoverText_A;
                    }


                    if (SCMD.SurgC_Appearance.App_Title1Color_Hax != null && SCMD.SurgC_Appearance.App_Title1Color_Hax != "")
                    {
                        Appear.App_Title1Color_Hax = SCMD.SurgC_Appearance.App_Title1Color_Hax;
                    }
                    if (SCMD.SurgC_Appearance.App_Title1Color_R != null && SCMD.SurgC_Appearance.App_Title1Color_R != "")
                    {
                        Appear.App_Title1Color_R = SCMD.SurgC_Appearance.App_Title1Color_R;
                    }
                    if (SCMD.SurgC_Appearance.App_Title1Color_G != null && SCMD.SurgC_Appearance.App_Title1Color_G != "")
                    {
                        Appear.App_Title1Color_G = SCMD.SurgC_Appearance.App_Title1Color_G;
                    }
                    if (SCMD.SurgC_Appearance.App_Title1Color_B != null && SCMD.SurgC_Appearance.App_Title1Color_B != "")
                    {
                        Appear.App_Title1Color_B = SCMD.SurgC_Appearance.App_Title1Color_B;
                    }
                    if (SCMD.SurgC_Appearance.App_Title1Color_A != null && SCMD.SurgC_Appearance.App_Title1Color_A != "")
                    {
                        Appear.App_Title1Color_A = SCMD.SurgC_Appearance.App_Title1Color_A;
                    }


                    if (SCMD.SurgC_Appearance.App_Title2Color_Hax != null && SCMD.SurgC_Appearance.App_Title2Color_Hax != "")
                    {
                        Appear.App_Title2Color_Hax = SCMD.SurgC_Appearance.App_Title2Color_Hax;
                    }
                    if (SCMD.SurgC_Appearance.App_Title2Color_R != null && SCMD.SurgC_Appearance.App_Title2Color_R != "")
                    {
                        Appear.App_Title2Color_R = SCMD.SurgC_Appearance.App_Title2Color_R;
                    }
                    if (SCMD.SurgC_Appearance.App_Title2Color_G != null && SCMD.SurgC_Appearance.App_Title2Color_G != "")
                    {
                        Appear.App_Title2Color_G = SCMD.SurgC_Appearance.App_Title2Color_G;
                    }
                    if (SCMD.SurgC_Appearance.App_Title2Color_B != null && SCMD.SurgC_Appearance.App_Title2Color_B != "")
                    {
                        Appear.App_Title2Color_B = SCMD.SurgC_Appearance.App_Title2Color_B;
                    }
                    if (SCMD.SurgC_Appearance.App_Title2Color_A != null && SCMD.SurgC_Appearance.App_Title2Color_A != "")
                    {
                        Appear.App_Title2Color_A = SCMD.SurgC_Appearance.App_Title2Color_A;
                    }


                    if (SCMD.SurgC_Appearance.App_LoginBackgroundColor_Hax != null && SCMD.SurgC_Appearance.App_LoginBackgroundColor_Hax != "")
                    {
                        Appear.App_LoginBackgroundColor_Hax = SCMD.SurgC_Appearance.App_LoginBackgroundColor_Hax;

                    }
                    if (SCMD.SurgC_Appearance.App_LoginBackgroundColor_R != null && SCMD.SurgC_Appearance.App_LoginBackgroundColor_R != "")
                    {
                        Appear.App_LoginBackgroundColor_R = SCMD.SurgC_Appearance.App_LoginBackgroundColor_R;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginBackgroundColor_G != null && SCMD.SurgC_Appearance.App_LoginBackgroundColor_G != "")
                    {
                        Appear.App_LoginBackgroundColor_G = SCMD.SurgC_Appearance.App_LoginBackgroundColor_G;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginBackgroundColor_B != null && SCMD.SurgC_Appearance.App_LoginBackgroundColor_B != "")
                    {
                        Appear.App_LoginBackgroundColor_B = SCMD.SurgC_Appearance.App_LoginBackgroundColor_B;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginBackgroundColor_A != null && SCMD.SurgC_Appearance.App_LoginBackgroundColor_A != "")
                    {
                        Appear.App_LoginBackgroundColor_A = SCMD.SurgC_Appearance.App_LoginBackgroundColor_A;
                    }


                    if (SCMD.SurgC_Appearance.App_LoginTextColor_Hax != null && SCMD.SurgC_Appearance.App_LoginTextColor_Hax != "")
                    {
                        Appear.App_LoginTextColor_Hax = SCMD.SurgC_Appearance.App_LoginTextColor_Hax;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginTextColor_R != null && SCMD.SurgC_Appearance.App_LoginTextColor_R != "")
                    {
                        Appear.App_LoginTextColor_R = SCMD.SurgC_Appearance.App_LoginTextColor_R;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginTextColor_G != null && SCMD.SurgC_Appearance.App_LoginTextColor_G != "")
                    {
                        Appear.App_LoginTextColor_G = SCMD.SurgC_Appearance.App_LoginTextColor_G;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginTextColor_B != null && SCMD.SurgC_Appearance.App_LoginTextColor_B != "")
                    {
                        Appear.App_LoginTextColor_B = SCMD.SurgC_Appearance.App_LoginTextColor_B;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginTextColor_A != null && SCMD.SurgC_Appearance.App_LoginTextColor_A != "")
                    {
                        Appear.App_LoginTextColor_A = SCMD.SurgC_Appearance.App_LoginTextColor_A;
                    }

                    if (SCMD.SurgC_Appearance.App_LoginButtonColor_Hax != null && SCMD.SurgC_Appearance.App_LoginButtonColor_Hax != "")
                    {
                        Appear.App_LoginButtonColor_Hax = SCMD.SurgC_Appearance.App_LoginButtonColor_Hax;

                    }
                    if (SCMD.SurgC_Appearance.App_LoginButtonColor_R != null && SCMD.SurgC_Appearance.App_LoginButtonColor_R != "")
                    {
                        Appear.App_LoginButtonColor_R = SCMD.SurgC_Appearance.App_LoginButtonColor_R;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginButtonColor_G != null && SCMD.SurgC_Appearance.App_LoginButtonColor_G != "")
                    {
                        Appear.App_LoginButtonColor_G = SCMD.SurgC_Appearance.App_LoginButtonColor_G;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginButtonColor_B != null && SCMD.SurgC_Appearance.App_LoginButtonColor_B != "")
                    {
                        Appear.App_LoginButtonColor_B = SCMD.SurgC_Appearance.App_LoginButtonColor_B;
                    }
                    if (SCMD.SurgC_Appearance.App_LoginButtonColor_A != null && SCMD.SurgC_Appearance.App_LoginButtonColor_A != "")
                    {
                        Appear.App_LoginButtonColor_A = SCMD.SurgC_Appearance.App_LoginButtonColor_A;
                    }
                    Surcenter.SurgC_Appearance = Appear;
                    //Appear = SCMD.SurgC_Appearance;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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

        public MT_Surgery_Center GetImages()
        {
            MT_Surgery_Center Surcenter = new MT_Surgery_Center();
            try
            {
                Dictionary<string, string> prefix =
              new Dictionary<string, string>(){
                                  {"file1", "SCLogo_"},
                                  {"file2", "SCNavi_"},
                                {"file3", "SCFav_"} };
                var httpRequest = HttpContext.Current.Request;
                var postedData = httpRequest.Form[0];
                string str = postedData.Substring(1, postedData.Length - 2);
                JObject jobject = JObject.Parse(str);
                Surcenter = JsonConvert.DeserializeObject<MT_Surgery_Center>(jobject.ToString());
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
                    Surcenter.SurgC_Logo = logo;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Surcenter;
        }


        [Route("API/SurgeryCenter/AddLogo")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddLogo()
        {
            MT_Surgery_Center SCMD = GetImages();
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                Logo logo = new Logo();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    if (SCMD.SurgC_Logo != null)
                    {
                        if (SCMD.SurgC_Logo.Logo_Login_Image != null && SCMD.SurgC_Logo.Logo_Login_Image != "")
                        {
                            logo.Logo_Login_Image = SCMD.SurgC_Logo.Logo_Login_Image;
                        }
                        else
                        {
                            logo.Logo_Login_Image = Surcenter.SurgC_Logo.Logo_Login_Image;
                        }
                    }
                    if (SCMD.SurgC_Logo != null)
                    {
                        if (SCMD.SurgC_Logo.Logo_Navigation_Image != null && SCMD.SurgC_Logo.Logo_Navigation_Image != "")
                        {
                            logo.Logo_Navigation_Image = SCMD.SurgC_Logo.Logo_Navigation_Image;
                        }
                        else
                        {
                            logo.Logo_Navigation_Image = Surcenter.SurgC_Logo.Logo_Navigation_Image;
                        }
                    }
                    if (SCMD.SurgC_Logo != null)
                    {
                        if (SCMD.SurgC_Logo.Logo_Fav_Image != null && SCMD.SurgC_Logo.Logo_Fav_Image != "")
                        {
                            logo.Logo_Fav_Image = SCMD.SurgC_Logo.Logo_Fav_Image;
                        }
                        else
                        {
                            logo.Logo_Fav_Image = Surcenter.SurgC_Logo.Logo_Fav_Image;
                        }
                    }
                    if (SCMD.SurgC_Logo != null)
                    {
                        Surcenter.SurgC_Logo = logo;
                    }

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
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



        public MT_Surgery_Center GetSliderImages()
        {
            string[] prefix = new string[] { "SCSliderBI_", "SCSliderI_" };
            string[] DBPath = new string[2];
            int i = 0;
            var httpRequest = HttpContext.Current.Request;
            var postedDataSurgC = httpRequest.Form[0];
            var postedDataSlider = httpRequest.Form[1];
            string strSurgC = postedDataSurgC.Substring(1, postedDataSurgC.Length - 2);
            string strSlider = postedDataSlider.Substring(1, postedDataSlider.Length - 2);

            JObject jobject = JObject.Parse(strSurgC);
            MT_Surgery_Center Surcenter = JsonConvert.DeserializeObject<MT_Surgery_Center>(jobject.ToString());

            jobject = JObject.Parse(strSlider);
            Slider slider = JsonConvert.DeserializeObject<Slider>(jobject.ToString());
            slider.Slider_Unique_ID = con.GetUniqueKey();
            slider.Slider_Creted_By = Surcenter.SurgC_Created_By;
            slider.Slider_User_Name = Surcenter.SurgC_User_Name;
            slider.Slider_Create_Date = con.ConvertTimeZone(Surcenter.SurgC_TimeZone, Convert.ToDateTime(Surcenter.SurgC_Modify_Date));
            slider.Slider_Modify_Date = con.ConvertTimeZone(Surcenter.SurgC_TimeZone, Convert.ToDateTime(Surcenter.SurgC_Modify_Date));
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
                Surcenter.SurgC_Slider = slider;
            }
            return Surcenter;
        }

        [Route("API/SurgeryCenter/AddSlider")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddSlider()
        {
            MT_Surgery_Center SCMD = GetSliderImages();
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                List<Slider> SliderList = new List<Slider>();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    if (Surcenter.SurgC_SliderList != null)
                    {
                        foreach (Slider slid in Surcenter.SurgC_SliderList)
                        {
                            SliderList.Add(slid);
                        }
                    }
                    SliderList.Add(SCMD.SurgC_Slider);
                    Surcenter.SurgC_SliderList = SliderList;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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


        [Route("API/SurgeryCenter/ActiveSlider")]
        [HttpPost]
        public async Task<HttpResponseMessage> ActiveSlider(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                List<Slider> SliderList = new List<Slider>();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    if (Surcenter.SurgC_SliderList != null)
                    {
                        foreach (Slider slid in Surcenter.SurgC_SliderList)
                        {
                            if (slid.Slider_Unique_ID == SCMD.SurgC_Slider.Slider_Unique_ID)
                            {
                                slid.Slider_Is_Active = SCMD.SurgC_Slider.Slider_Is_Active;
                                slid.Slider_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                            }
                            SliderList.Add(slid);
                        }
                    }
                    Surcenter.SurgC_SliderList = SliderList;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/DeleteSlider")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteSlider(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                List<Slider> SliderList = new List<Slider>();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    if (Surcenter.SurgC_SliderList != null)
                    {
                        foreach (Slider slid in Surcenter.SurgC_SliderList)
                        {
                            if (slid.Slider_Unique_ID == SCMD.SurgC_Slider.Slider_Unique_ID)
                            {
                                slid.Slider_Is_Deleted = SCMD.SurgC_Slider.Slider_Is_Deleted;
                                slid.Slider_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                            }
                            SliderList.Add(slid);
                        }
                    }
                    Surcenter.SurgC_SliderList = SliderList;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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


        [Route("API/SurgeryCenter/AddFooter")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddFooter(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                Footer footer = new Footer();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    footer = SCMD.SurgC_Footer;
                    Surcenter.SurgC_Footer = footer;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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


        [Route("API/SurgeryCenter/AddMiscellaneous")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddMiscellaneous(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center Surcenter = new MT_Surgery_Center();
                Miscellaneous miscell = new Miscellaneous();
                Query SelectQuery = con.Db().Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot SelectSnap = await SelectQuery.GetSnapshotAsync();
                if (SelectSnap != null)
                {
                    Surcenter = SelectSnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    Surcenter.SurgC_Modify_Date = con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date));
                    miscell = SCMD.SurgC_Miscellaneous;
                    Surcenter.SurgC_Miscellaneous = miscell;

                    DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                    WriteResult Result = await docRef.SetAsync(Surcenter, SetOptions.Overwrite);
                    if (Result != null)
                    {
                        Response.Status = con.StatusSuccess;
                        Response.Message = con.MessageSuccess;
                        Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                Dictionary<string, object> initialData;
                List<MT_Specilities> SpelitiesList = new List<MT_Specilities>();
                initialData = new Dictionary<string, object>
                    {
                        { "SurgC_Name", SCMD.SurgC_Name },
                        { "SurgC_DBA_Name", SCMD.SurgC_DBA_Name },
                        { "SurgC_Address", SCMD.SurgC_Address },
                        { "SurgC_City", SCMD.SurgC_City },
                        { "SurgC_State", SCMD.SurgC_State },
                        { "SurgC_Country", SCMD.SurgC_Country },
                        { "SurgC_Zip", SCMD.SurgC_Zip },
                        { "SurgC_Landline", SCMD.SurgC_Landline },
                        { "SurgC_FaxNo", SCMD.SurgC_FaxNo },
                        { "SurgC_Email", SCMD.SurgC_Email },
                        { "SurgC_AlternateNo", SCMD.SurgC_AlternateNo },
                        { "SurgC_MobileNo", SCMD.SurgC_MobileNo },
                        { "SurgC_Helpline", SCMD.SurgC_Helpline },
                        { "SurgC_Website_URL", SCMD.SurgC_Website_URL },
                        { "SurgC_Is_Active", SCMD.SurgC_Is_Active },
                        { "SurgC_Is_Deleted", SCMD.SurgC_Is_Deleted },
                        { "SurgC_Specilities", SCMD.SurgC_Specilities },
                        { "SurgC_SpecilitiesList", SCMD.SurgC_SpecilitiesList },
                        { "SurgC_ContactSetting", SCMD.SurgC_ContactSetting },
                        //{ "SurgC_Setting", SCMD.SurgC_Setting },
                        { "SurgC_Modify_Date",con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date))}
                    };
                DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/IsActive")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsActive(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                Dictionary<string, object> initialData;
                initialData = new Dictionary<string, object>
                {
                    { "SurgC_Is_Active", SCMD.SurgC_Is_Active },
                    { "SurgC_Modify_Date",con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date))}
                };
                DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/IsDeleted")]
        [HttpPost]
        public async Task<HttpResponseMessage> IsDeleted(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                Dictionary<string, object> initialData;
                initialData = new Dictionary<string, object>
                {
                    { "SurgC_Is_Deleted", SCMD.SurgC_Is_Deleted },
                    { "SurgC_Modify_Date",con.ConvertTimeZone(SCMD.SurgC_TimeZone, Convert.ToDateTime(SCMD.SurgC_Modify_Date))}
                };
                DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                WriteResult Result = await docRef.UpdateAsync(initialData);
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/Remove")]
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                DocumentReference docRef = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
                WriteResult Result = await docRef.DeleteAsync();
                if (Result != null)
                {
                    Response.Status = con.StatusSuccess;
                    Response.Message = con.MessageSuccess;
                    Response.Data = SCMD;
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

        [Route("API/SurgeryCenter/Select")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Select(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                MT_Surgery_Center SC = new MT_Surgery_Center();
                List<Slider> SliderList = new List<Slider>();
                Query docRef = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    SC = ObjQuerySnap.Documents[0].ConvertTo<MT_Surgery_Center>();
                    if (SC.SurgC_SliderList != null)
                    {
                        foreach (Slider slide in SC.SurgC_SliderList)
                        {
                            if (slide.Slider_Is_Deleted == false)
                            {
                                SliderList.Add(slide);
                            }
                        }
                        SC.SurgC_SliderList = SliderList;
                    }
                    Response.Data = SC;
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

        [Route("API/SurgeryCenter/List")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> List(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                List<MT_Surgery_Center> AnesList = new List<MT_Surgery_Center>();
                Query docRef = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Surgery_Center>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.SurgC_DBA_Name).ToList();
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

        [Route("API/SurgeryCenter/ListDD")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> ListDD(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                List<MT_Surgery_Center> AnesList = new List<MT_Surgery_Center>();
                Query docRef = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", false).WhereEqualTo("SurgC_Is_Active", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Surgery_Center>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.SurgC_DBA_Name).ToList();
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

        [Route("API/SurgeryCenter/GetDeletedList")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> GetDeletedList(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                List<MT_Surgery_Center> AnesList = new List<MT_Surgery_Center>();
                Query docRef = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", true);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot Docsnapshot in ObjQuerySnap.Documents)
                    {
                        AnesList.Add(Docsnapshot.ConvertTo<MT_Surgery_Center>());
                    }
                    Response.DataList = AnesList.OrderBy(o => o.SurgC_DBA_Name).ToList();
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


        [Route("API/SurgeryCenter/AddSetting")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> AddSetting(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {

                MT_Surgery_Center Equip = new MT_Surgery_Center();
                List<MT_SurgC_Contact_Setting> settinglist = new List<MT_SurgC_Contact_Setting>();
                Query docRef1 = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", false).WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID);
                QuerySnapshot ObjQuerySnap1 = await docRef1.GetSnapshotAsync();
                if (ObjQuerySnap1 != null)
                {
                    Equip = ObjQuerySnap1.Documents[0].ConvertTo<MT_Surgery_Center>();
                    if (SCMD.SurgC_ContactSetting != null)
                    {
                        foreach (MT_SurgC_Contact_Setting SCS in SCMD.SurgC_ContactSetting)
                        {
                            UniqueSettingID = con.GetUniqueKey();
                            SCS.SCS_Unique_ID = UniqueSettingID;
                            settinglist.Add(SCS);
                        }
                    }
                    if (Equip.SurgC_ContactSetting != null)
                    {
                        foreach (MT_SurgC_Contact_Setting SCSetting in Equip.SurgC_ContactSetting)
                        {
                            settinglist.Add(SCSetting);
                        }
                    }

                    Equip.SurgC_ContactSetting = settinglist;
                    DocumentReference docRef2 = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
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



        [Route("API/SurgeryCenter/DeleteSetting")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> DeleteSetting(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {

                MT_Surgery_Center Equip = new MT_Surgery_Center();
                List<MT_SurgC_Contact_Setting> settinglist = new List<MT_SurgC_Contact_Setting>();
                Query docRef1 = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", false).WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID);
                QuerySnapshot ObjQuerySnap1 = await docRef1.GetSnapshotAsync();
                if (ObjQuerySnap1 != null)
                {
                    Equip = ObjQuerySnap1.Documents[0].ConvertTo<MT_Surgery_Center>();
                    if (Equip.SurgC_ContactSetting != null)
                    {
                        foreach (MT_SurgC_Contact_Setting SCSetting in Equip.SurgC_ContactSetting)
                        {
                            if (SCSetting.SCS_Unique_ID != SCMD.SurgC_ContactSetting[0].SCS_Unique_ID)
                                settinglist.Add(SCSetting);
                        }
                    }

                    Equip.SurgC_ContactSetting = settinglist;
                    DocumentReference docRef2 = Db.Collection("MT_Surgery_Center").Document(SCMD.SurgC_Unique_ID);
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

        public MT_Surgery_Center GetFile()
        {
            MT_Surgery_Center SurgeryCenter = new MT_Surgery_Center();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                var postedData = httpRequest.Form[0];
                string str = postedData.Substring(1, postedData.Length - 2);
                JObject jobject = JObject.Parse(str);
                SurgeryCenter = JsonConvert.DeserializeObject<MT_Surgery_Center>(jobject.ToString());

                if (httpRequest.Files.Count > 0)
                {
                    MT_DBCollection DBCollection = new MT_DBCollection();

                    var postedFile = httpRequest.Files[0];

                    int hasheddate = DateTime.Now.GetHashCode();

                    var myKey = postedFile.FileName.ToString();

                    string changed_name = myKey;

                    var filePath = HttpContext.Current.Server.MapPath("~/ConfigFile/" + changed_name);

                    postedFile.SaveAs(filePath); // save the file to a folder "Images" in the root of your app

                    changed_name = @"~\ConfigFile\" + changed_name; //store this complete path to database

                    DBCollection.DBC_Unique_ID = con.GetUniqueKey();
                    DBCollection.DBC_Modify_Date = con.ConvertTimeZone(SurgeryCenter.SurgC_TimeZone, Convert.ToDateTime(SurgeryCenter.SurgC_Modify_Date));
                    DBCollection.DBC_Create_Date = con.ConvertTimeZone(SurgeryCenter.SurgC_TimeZone, Convert.ToDateTime(SurgeryCenter.SurgC_Modify_Date));
                    DBCollection.DBC_File_Name = myKey;
                    DBCollection.DBC_File_Path = changed_name;
                    DBCollection.DBC_Project_ID = SurgeryCenter.SurgC_Project_ID;
                    DBCollection.DBC_Created_By = SurgeryCenter.SurgC_Created_By;
                    DBCollection.DBC_User_Name = SurgeryCenter.SurgC_User_Name;
                    DBCollection.DBC_Is_Active = true;
                    DBCollection.DBC_Is_Deleted = false;
                    SurgeryCenter.SurgC_DB_Setting = DBCollection;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return SurgeryCenter;
        }

        [Route("API/SurgeryCenter/DBSetup")]
        [HttpPost]
        //[Authorize(Roles ="SAdmin")]
        public async Task<HttpResponseMessage> DBSetup()
        {
            MT_Surgery_Center SCMD = GetFile();
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {

                MT_Surgery_Center Equip = new MT_Surgery_Center();
                Query docRef1 = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Is_Deleted", false).WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID);
                QuerySnapshot ObjQuerySnap1 = await docRef1.GetSnapshotAsync();
                if (ObjQuerySnap1 != null)
                {
                    Equip = ObjQuerySnap1.Documents[0].ConvertTo<MT_Surgery_Center>();
                    if (SCMD.SurgC_DB_Setting != null)
                    {
                        Equip.SurgC_DB_Setting = SCMD.SurgC_DB_Setting;
                    }

                    DocumentReference docRef2 = Db.Collection("MT_Surgery_Center").Document(Equip.SurgC_Unique_ID);
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

        [Route("API/SurgeryCenter/GetSCListFilterWithPO")]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetSCListFilterWithPO(MT_Surgery_Center SCMD)
        {
            Db = con.SurgeryCenterDb(SCMD.Slug);
            SurgeryCenterResponse Response = new SurgeryCenterResponse();
            try
            {
                List<MT_Surgery_Center> SCList = new List<MT_Surgery_Center>();

                Query docRef = Db.Collection("MT_Surgery_Center").WhereEqualTo("SurgC_Unique_ID", SCMD.SurgC_Unique_ID).WhereEqualTo("SurgC_Is_Deleted", false);
                QuerySnapshot ObjQuerySnap = await docRef.GetSnapshotAsync();
                if (ObjQuerySnap != null)
                {
                    foreach (DocumentSnapshot DocSnap in ObjQuerySnap.Documents)
                    {
                        SCList.Add(DocSnap.ConvertTo<MT_Surgery_Center>());
                    }
                    Response.DataList = SCList;
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
