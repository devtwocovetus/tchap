using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCloudHealth.Models;

namespace TheCloudHealth.Lib
{
    interface ICreatePDF
    {
        string CreateBookingPDF(MT_PatientInfomation ObjPatientInformation, string Path);
    }
}
