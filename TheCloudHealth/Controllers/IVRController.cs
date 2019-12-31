using System.Web.Mvc;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;

namespace TheCloudHealth.Controllers
{
    public class IVRController : TwilioController
    {
        // GET: IVR
        public ActionResult Index()
        {
            return View();
        }

        [Route("IVR/Welcome")]
        [HttpGet]
        public TwiMLResult Welcome()
        {
            var response = new VoiceResponse();
            var gather = new Gather(action: Url.ActionUri("Show", "Menu"), numDigits: 1);
            gather.Say("Thank you for calling the E.T. Phone Home Service - the " +
                       "adventurous alien's first choice in intergalactic travel. " +
                       "Press 1 for directions, press 2 to make a call.");
            response.Append(gather);

            return TwiML(response);
        }


        public void DownloadPDF()
        {
            string HTMLContent = "Hello <b>World</b>";
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(HTMLContent));
            Response.End();
        }

        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }
    }
}