using DevExpress.XtraRichEdit.Import.Doc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HanjuReport.Reports {
    public partial class OperationStatusReport : DevExpress.XtraReports.UI.XtraReport {
        public OperationStatusReport() {
            InitializeComponent();





        }

    public void SetTitle(string title, string subtitle) {
      label1.Text = title;
      label3.Text = subtitle;
    }


    }
}