using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TheCloudHealth.Models;

namespace TheCloudHealth.Lib
{
    public class ConnectionClass
    {

        #region Declare Variables

        public int StatusFailed = 0;
        public string MessageFailed = "Failed";
        public int StatusSuccess = 1;
        public string MessageSuccess = "OK";
        public int StatusNotInsert = 2;
        public string MessageNotInsert = "Not Inserted";
        public int StatusNotUpdate = 3;
        public string MessageNotUpdate = "Not Updated";
        public int StatusNotDeleted = 4;
        public string MessageNotDeleted = "Not Updated";
        public int StatusWC = 5;
        public string MessageWC = "Wrong Credentials";
        public int StatusDNE = 6;
        public string MessageDNE = "Data Not Exist";
        public int StatusDAE = 7;
        public string MessageDAE = "Data Already Exist";

        public int StatusAE = 8;
        public string MessageAE = "Already Exist";

        

        //

        public string WaystarURL = "https://eligibilityapi.zirmed.com/1.0/Rest/Gateway/GatewayAsync.ashx";
        public string WaystarUID = "A3f7b777Yac024BTwlr8TGD0MoaWKkNK7+wu9TmcTV0=";//RTE185505
        public string WaystarPWord = "9duLgV8aBLbjyNn0NePWtH8kgwyhmTVM7+wu9TmcTV0=";//demriz05
        public string WaystarDFormat = "SF1";        
        public string WaystarRPType = "271";
        public string WaystarRSType = "HTML";
        public string WaystarTData = "ABC CLINIC|HUMANA|H333224444|JOHN|DOE|9/5/1988|S|||30||61101||1|1P|||123456789||HPI-1234567893";

        public string BaseURL = "";
        ICryptoEngine ICryptoData;

        public Random a = new Random(); // replace from new Random(DateTime.Now.Ticks.GetHashCode());
                                        // Since similar code is done in default constructor internally
        public FirestoreDb fireStoreDb;
        public FirestoreDb fireStoreDbLog;
        private static Random random = new Random();

        #endregion  Declare Variables
        public ConnectionClass() 
        {
            BaseURL = ConfigurationManager.AppSettings["BaseURL"].ToString();
            ICryptoData = new CryptoEngineData();
        }



        #region Credential For TheCloudHealthCare Database
        public FirestoreDb Db()
        {
            string fileName = "thecloudhealthcare-e25078639941.json";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            string projectId = "thecloudhealthcare";
            return fireStoreDb = FirestoreDb.Create(projectId);
        }

        #endregion  Credential For TheCloudHealthCare Database

        #region Credential For Google Cloud Storage

        //public FirestoreDb GCS()
        //{
        //    string fileName = "thecloudhealthcare-e25078639941.json";
        //    string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);
        //    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
        //    string projectId = "thecloudhealthcare";

        //    //return StorageClient.Create("GOOGLE_APPLICATION_CREDENTIALS");
        //    return fireStoreDb = FirestoreDb.Create(projectId);
        //}

        #endregion Credential For Google Cloud Storage



        public FirestoreDb SurgeryCenterDb(string ProjectID)
        {

            string fileName = "";// = "thecloudhealthcare-e25078639941.json";
            string filepath = "";// = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);
            string projectId = "";// = "thecloudhealthcare";
            if (ProjectID == "oakbrooksurgical-d3aca")
            {
                fileName = "oakbrooksurgical-d3aca-71ded5e2bcff.json";
                filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
                projectId = "oakbrooksurgical-d3aca";
                fireStoreDb = FirestoreDb.Create(projectId);
            }
            else
            {
                fileName = "thecloudhealthcare-e25078639941.json";
                filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
                projectId = "thecloudhealthcare";
                fireStoreDb = FirestoreDb.Create(projectId);
            }


            return fireStoreDb;
        }

        //#region Credential For TheCloudHealthCare Log Database
        //public FirestoreDb DbLog()
        //{
        //    string fileName = "thecloudhealthlog-381287e88e82.json";
        //    string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ConfigFile\", fileName);            
        //    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
        //    string projectId = "TheCloudHealthLog";
        //    return fireStoreDbLog = FirestoreDb.Create(projectId);
        //}

        //#endregion  Credential For TheCloudHealthCare Log Database
        public string GetUniqueKey()
        {
            var ticks = new DateTime(1990, 1, 1).Ticks;
            var ans = DateTime.Now.Ticks - ticks;
            var uniqueId = ans.ToString("x");
            return RandomString(4) + RandomString(4) + uniqueId.ToString();
        }

        public string GetUrlToken()
        {
            var ticks = new DateTime(1990, 1, 1).Ticks;
            var ans = DateTime.Now.Ticks - ticks;
            var uniqueId = ans.ToString("x");
            return RandomString(4) + uniqueId.ToString();
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZAabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public DateTime ConvertTimeZone(string timezone,DateTime CurrentDate)
        {
            if (timezone == "")
            {
                timezone = "US/Eastern (GMT-04:00)";
            }

            Double hours = 0;
            Double minutes = 0;
            DateTime SelectedTime;
            int index = timezone.IndexOf('+') + 1;
            if (index > 0)
            {
                int len = timezone.Length - 1;
                string Extrahours = timezone.Substring(index, 5);
                hours = Convert.ToDouble(Extrahours.Substring(0, 2));
                minutes = Convert.ToDouble(Extrahours.Substring(Extrahours.IndexOf(':') + 1, 2));
            }
            else
            {
                index = timezone.IndexOf('-') + 1;
                if (index > 0)
                {
                    int len = timezone.Length - 1;
                    string Extrahours = timezone.Substring(index, 5);
                    hours = Convert.ToDouble("-" + Extrahours.Substring(0, 2));
                    minutes = Convert.ToDouble("-" + Extrahours.Substring(Extrahours.IndexOf(':') + 1, 2));

                }
            }
            return SelectedTime = DateTime.SpecifyKind(CurrentDate.AddHours(hours).AddMinutes(minutes), DateTimeKind.Utc);
        }
        public string DecryptData(string EncrypString)
        {
            return ICryptoData.Decrypt(ICryptoData.Decrypt(EncrypString, "sblw-3hn8-sqoy19"), "sblw-3hn8-sqoy19");
        }
        public string EncryptData(string EncrypString)
        {
            return ICryptoData.Decrypt(ICryptoData.Decrypt(EncrypString, "sblw-3hn8-sqoy19"), "sblw-3hn8-sqoy19");
        }
        public string CreateFile(string textstring,string FileName)
        {
            string strreturn = "";
            if (!File.Exists(textstring))
            {
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(FileName), FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(textstring);
                    }
                }
                strreturn = FileName;
            }
            else
            {
                strreturn = "File Already Exist";
            }
            return strreturn;
        }
        public async Task<HttpResponseMessage> VerifyInsureanceAsync(string apiUrl)
        {
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;

                response = await client.GetAsync(apiUrl);
            }
            return response;
        }
        [Obsolete]
        public string HTMLToPDF(string HTMLString, string FileName)
        {
            string Pathstring = HttpContext.Current.Server.MapPath(FileName);
            try
            {

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    string Finalstring = "";
                    int Findex = HTMLString.IndexOf("<head>");
                    int Lindex = HTMLString.IndexOf("</head>");
                    int len = HTMLString.Length;
                    Finalstring = HTMLString.Substring(0, Findex);
                    Finalstring = Finalstring + HTMLString.Substring(Lindex + 7, len - Lindex - 7);
                    StringReader sr = new StringReader(Finalstring);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(Pathstring, FileMode.Create, FileAccess.Write));
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    stream.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return Pathstring;
        }
    }
}