using System;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TheCloudHealth.Models;
using TheCloudHealth.Lib;

namespace TheCloudHealth.Lib
{
    public class CreatePDF : ICreatePDF
    {
        Constant ObjConstant;
        string PDFHeader = "Patient Infomation";
        public CreatePDF()
        {
            ObjConstant = new Constant();
        }
        public string CreateBookingPDF(MT_PatientInfomation ObjPatientInformation,string Path)
        {
            using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.None))

            using (Document doc = new Document(PageSize.LETTER, 0f, 0f, 0f, 0f))

            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.SetMargins(ObjConstant.PageLeftMargin, ObjConstant.PageRightMargin, ObjConstant.PageTopMargin, ObjConstant.PageBottomMargin);
                doc.Open();

                //Document : Header
                DocumentHeader(doc, PDFHeader, ObjConstant.fontDocHeader, ObjConstant.fontDocSubHeader);

                Demographics(doc, ObjPatientInformation, ObjConstant.fontH1, ObjConstant.fonttext);

                doc.Close();
                doc.Dispose();
                GC.SuppressFinalize(doc);
            }
            return "";
            
        }

        public void DocumentHeader(Document doc, String Header, Font hd, Font txt)
        {
            PdfPTable tableout = new PdfPTable(1);
            //tableout.HorizontalAlignment = Left;
            PdfPCell cell = new PdfPCell(new Phrase(Header, hd));
            cell.Colspan = 6;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            tableout.SpacingAfter = 10f;
            tableout.AddCell(cell);
            doc.Add(tableout);
        }

        public void Demographics(Document doc, MT_PatientInfomation ObjPatientInfor, Font hd, Font txt)
        {
            try
            {
                PdfPTable tableout = new PdfPTable(1);
                //tableout.HorizontalAlignment = align.;
                tableout.SpacingAfter = 5f;
                PdfPCell cell = new PdfPCell(new Phrase("Demographics", FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cell.Colspan = 4;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                tableout.AddCell(cell);
                doc.Add(tableout);

                PdfPTable tablein = new PdfPTable(4);
                //tablein.HorizontalAlignment = Left;
                tablein.SpacingAfter = 10f;
                int[] intTblWidth = { 20, 30, 20, 30 };
                tablein.SetWidths(intTblWidth);
                //First Row

                //First Columns
                AppendCell(tablein, "Name:", hd, 0, true, false, false, true);
                //Second Columns
                AppendCell(tablein, ObjPatientInfor.Patient_First_Name + ' ' + ObjPatientInfor.Patient_Middle_Name + ' ' + ObjPatientInfor.Patient_Last_Name, txt, 0, true, false, false, false);
                //Third Columns
                AppendCell(tablein, "Patient ID:", hd, 0, true, false, false, false);
                //Fourth Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Code, txt, 0, true, true, false, false);


                //Second Row
                //First Columns
                AppendCell(tablein, "Birth Date:", hd, 0, false, false, false, true);
                //Second Columns
                AppendCell(tablein, ObjPatientInfor.Patient_DOB.ToString(), txt, 0, false, false, false, false);
                //Third Columns
                AppendCell(tablein, "Gender:", hd, 0, false, false, false, false);
                //Fourth Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Sex, txt, 0, false, true, false, false);

                //Third Row
                //First Columns
                AppendCell(tablein, "Marital Status:", hd, 0, false, false, false, true);
                //Second Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Marital_Status, txt, 0, false, false, false, false);
                //Third Columns
                AppendCell(tablein, "Religious:", hd, 0, false, false, false, false);
                //Fourth Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Religion, txt, 0, false, true, false, false);


                //Fourth Row
                //First Columns
                AppendCell(tablein, "Race:", hd, 0, false, false, false, true);
                //Second Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Race, txt, 0, false, false, false, false);
                //Third Columns
                AppendCell(tablein, "Ethnic:", hd, 0, false, false, false, false);
                //Fourth Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Ethinicity, txt, 0, false, true, false, false);


                //Fifth Row
                //First Columns
                AppendCell(tablein, "Contact Information:", hd, 0, false, false, false, true);
                //Second Columns
                AppendCell(tablein, "Tel (Primary Home )" + ObjPatientInfor.Patient_Primary_No + "\n"
                    + "Tel (Emergency Contact )" + ObjPatientInfor.Patient_Emergency_No + "\n"
                    + "Tel (Mobile Contact)" + ObjPatientInfor.Patient_Secondary_No + "\n"
                    + "Tel (Office Contact)" + ObjPatientInfor.Patient_Work_No + "\n"
                    + "Tel (Spouse Contact)" + ObjPatientInfor.Patient_Spouse_No + "\n"
                    + "Email " + ObjPatientInfor.Patient_Email
                    , txt, 0, false, false, false, false);
                //Third Columns
                AppendCell(tablein, "Primary Home:", hd, 0, false, false, false, false);
                //Fourth Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Address1 + "\n"
                    + ObjPatientInfor.Patient_Address2
                    + ObjPatientInfor.Patient_City + "," + ObjPatientInfor.Patient_State + " " + ObjPatientInfor.Patient_Zipcode
                    //+ " " + ObjPatientInfor.c
                    , txt, 0, false, true, false, false);




                //Sixth Row
                //First Columns
                AppendCell(tablein, "Language:", hd, 0, false, false, true, true);
                //Second Columns
                AppendCell(tablein, ObjPatientInfor.Patient_Language, txt, 0, false, false, true, false);
                //Third Columns
                AppendCell(tablein, "", hd, 0, false, false, true, false);
                //Fourth Columns
                AppendCell(tablein, "", txt, 0, false, true, true, false);

                doc.Add(tablein);
                Console.WriteLine("tablein Width : " + tablein.TotalHeight.ToString());

            }
            catch (Exception ex)
            {
                
            }
        }


        public void AppendCell(PdfPTable tblObj, string Value, Font fontObj, int ColSpan, Boolean TopBorder = false, Boolean RightBorder = false, Boolean BottomBorder = false, Boolean LeftBorder = false)
        {
            try
            {
                PdfPCell cell = new PdfPCell(new Phrase(Value, fontObj));
                cell.Colspan = ColSpan;
                cell.BorderColor = BaseColor.LIGHT_GRAY;

                if (TopBorder && RightBorder && BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                }
                else if (TopBorder && RightBorder && BottomBorder && !LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                }
                else if (TopBorder && RightBorder && !BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                }
                else if (TopBorder && !RightBorder && BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                }
                else if (!TopBorder && RightBorder && BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                }
                else if (!TopBorder && !RightBorder && BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                }
                else if (TopBorder && !RightBorder && !BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
                }
                else if (TopBorder && RightBorder && !BottomBorder && !LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
                }
                else if (!TopBorder && RightBorder && BottomBorder && !LeftBorder)
                {
                    cell.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                }
                else if (!TopBorder && !RightBorder && !BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.LEFT_BORDER;
                }
                else if (TopBorder && !RightBorder && !BottomBorder && !LeftBorder)
                {
                    cell.Border = Rectangle.TOP_BORDER;
                }
                else if (!TopBorder && RightBorder && !BottomBorder && !LeftBorder)
                {
                    cell.Border = Rectangle.RIGHT_BORDER;
                }
                else if (!TopBorder && !RightBorder && BottomBorder && !LeftBorder)
                {
                    cell.Border = Rectangle.BOTTOM_BORDER;
                }
                else if (!TopBorder && !RightBorder && !BottomBorder && LeftBorder)
                {
                    cell.Border = Rectangle.LEFT_BORDER;
                }
                else
                {
                    cell.Border = Rectangle.NO_BORDER;
                }
                tblObj.AddCell(cell);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception Message From PrintPDF : " + de);
            }
        }
    }
}