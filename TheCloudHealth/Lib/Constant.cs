using iTextSharp.text;
using System;
using System.IO;
using System.Web;

namespace TheCloudHealth.Lib
{
    public class Constant
    {
        public float PageLeftMargin = 0f, PageRightMargin = 0f, PageTopMargin = 40f, PageBottomMargin = 30f;
        public Font fontDocHeader = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //Document Sub Header Font
        public Font fontDocSubHeader = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //Table Header Font
        public Font fontH1 = FontFactory.GetFont("Arial", 8.5f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //Table Inner Text font
        public Font fonttext = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        public string Createfolder()
        {
            //string folderName = HttpContext.Current.Server.MapPath("~/Images/");//ConfigurationManager.AppSettings["outputPath"].ToString() + @"\";
            string pathString = HttpContext.Current.Server.MapPath("~/Images/");// Path.Combine(folderName, "PDF");
            try
            {
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                return pathString;
            }
            catch (Exception de)
            {
                //UpdateLog["@Exceptionmsg"] = de.Message;
                //UpdateLog["@Exceptionflag"] = true;
                //dl.UpdateDataFromProcedure("HCI_UpdateLog", UpdateLog, ConnectionStr);
            }
            return pathString;
        }
    }
}