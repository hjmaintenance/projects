namespace HanjuReport.Reports
{
    partial class Test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
      this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
      this.Detail = new DevExpress.XtraReports.UI.DetailBand();
      this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
      this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
      this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
      this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
      this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
      this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
      this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      // 
      // TopMargin
      // 
      this.TopMargin.Name = "TopMargin";
      // 
      // BottomMargin
      // 
      this.BottomMargin.Name = "BottomMargin";
      // 
      // Detail
      // 
      this.Detail.HeightF = 125.7084F;
      this.Detail.Name = "Detail";
      // 
      // xrLabel1
      // 
      this.xrLabel1.AutoWidth = true;
      this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 25.64F, DevExpress.Drawing.DXFontStyle.Underline);
      this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
      this.xrLabel1.Name = "xrLabel1";
      this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel1.SizeF = new System.Drawing.SizeF(609.7917F, 42.79169F);
      this.xrLabel1.StylePriority.UseFont = false;
      this.xrLabel1.StylePriority.UseTextAlignment = false;
      this.xrLabel1.Text = "한주 유틸리티 모니터링\r\n출력 서버\r\n";
      this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      // 
      // ReportHeader
      // 
      this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1});
      this.ReportHeader.HeightF = 228.125F;
      this.ReportHeader.Name = "ReportHeader";
      // 
      // PageHeader
      // 
      this.PageHeader.Name = "PageHeader";
      // 
      // PageFooter
      // 
      this.PageFooter.Name = "PageFooter";
      // 
      // xrLabel2
      // 
      this.xrLabel2.AutoWidth = true;
      this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
      this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 42.79168F);
      this.xrLabel2.Name = "xrLabel2";
      this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel2.SizeF = new System.Drawing.SizeF(609.7917F, 26.12503F);
      this.xrLabel2.StylePriority.UseFont = false;
      this.xrLabel2.StylePriority.UseTextAlignment = false;
      this.xrLabel2.Text = "한주 유틸리티 모니터링\r\n출력 서버\r\n";
      this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      // 
      // xrLabel3
      // 
      this.xrLabel3.AutoWidth = true;
      this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
      this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 86.12502F);
      this.xrLabel3.Name = "xrLabel3";
      this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel3.SizeF = new System.Drawing.SizeF(609.7917F, 26.12503F);
      this.xrLabel3.StylePriority.UseFont = false;
      this.xrLabel3.StylePriority.UseTextAlignment = false;
      this.xrLabel3.Text = "홍길동";
      this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
      // 
      // xrLabel4
      // 
      this.xrLabel4.AutoWidth = true;
      this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
      this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 112.25F);
      this.xrLabel4.Name = "xrLabel4";
      this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      this.xrLabel4.SizeF = new System.Drawing.SizeF(609.7917F, 26.12503F);
      this.xrLabel4.StylePriority.UseFont = false;
      this.xrLabel4.StylePriority.UseTextAlignment = false;
      this.xrLabel4.Text = "한주 유틸리티 모니터링\r\n출력 서버\r\n";
      this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
      // 
      // TestReport
      // 
      this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.PageHeader,
            this.PageFooter});
      this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
      this.Version = "24.2";
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
    private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
    private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
    private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    private DevExpress.XtraReports.UI.XRLabel xrLabel3;
    private DevExpress.XtraReports.UI.XRLabel xrLabel2;
  }
}
