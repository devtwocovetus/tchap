using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCloudHealth.Models;

namespace TheCloudHealth.Lib
{
    interface IEmailer
    {
        Task<string> Send(Email EML);
    }
}
