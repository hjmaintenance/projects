using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits.Data;
using MIT.Razor.Pages.Component.DataEdits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DevExpress.Data.Filtering.Helpers;

namespace MIT.UI.LIB.DataEdits.Data
{
    public class CustImageComboBoxData : ImageComboBoxData
    {
        public string CUST_CODE { get; set; } = string.Empty;
        public string CUST_DIV { get; set; } = string.Empty;
        public string CUST_NAME { get; set; } = string.Empty;
    }
}
