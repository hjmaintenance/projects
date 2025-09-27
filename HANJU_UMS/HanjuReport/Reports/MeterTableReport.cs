using DevExpress.XtraRichEdit.Import.Doc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HanjuReport.Reports {
    public partial class MeterTableReport : DevExpress.XtraReports.UI.XtraReport {
        public MeterTableReport() {
            InitializeComponent();





        }

    public void SetTitle(string title, string subtitle) {
      label1.Text = title;
      label3.Text = subtitle;
    }


    }
}