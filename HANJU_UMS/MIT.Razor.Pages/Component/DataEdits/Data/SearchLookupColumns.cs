using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits.Data
{
    /// <summary>
    /// SearchLookup 컬럼 정보
    /// </summary>
    public class SearchLookupColumns
    {
        public string FieldName { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Width { get; set; } = "100px";
        public bool Visible { get; set; } = true;
        //public int VisibleIndex { get; set; };
    }
}
