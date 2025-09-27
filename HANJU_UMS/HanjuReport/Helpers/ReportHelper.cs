using DevExpress.DataAccess.Native.Excel;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using HanjuReport.Service;
using MIT.ServiceModel;
using System;
using System.Data;
using System.Drawing;

namespace HanjuReport.Helpers {
    public class ReportHelper {
    public static void CreateReportXlsx(XtraReport report, RptInfo ri) {
      report.Margins.Left = 0;
      report.Margins.Top = 0;
      report.Margins.Right = 0;
      report.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
      var cols = ri.Columns;
      var cols2 = ri.Columns2;
      DataTable dt = ri.Data as DataTable;
      report.Landscape = ri.Landscape == "Y" ? true : false;
      report.DataSource = dt;

      /*string colfname = "";
      string coltname = "";

      foreach (var c in cols) {
        if (c.Key != "CHK") {
          colfname += c.Key + ",";
          coltname += c.Value + ",";
        }
      }*/
      //Console.WriteLine($"colfname cnt: {colfname.Length}");
      //Console.WriteLine($"coltname cnt: {coltname.Length}");
      //Console.WriteLine($"Columns cnt: {cols.Count}");
      //Console.WriteLine($"Columns2 cnt: {cols2.Where(c => c.Value.Kind=="Data").Count()}");
      //
      float startHeight = 23;
      //string[] fields = colfname.Split(',', StringSplitOptions.None);
      //string[] titles = coltname.Split(',', StringSplitOptions.None);
      string[] titles = cols2.Where(c=> c.Key != "CHK" && c.Value.Kind == "Data").Select(c => c.Value.ColName).ToArray();
      string[] fields = cols2.Where(c => c.Key != "CHK" && c.Value.Kind == "Data").Select(c => c.Value.FieldName).ToArray();

            /*Console.WriteLine($"fields cnt: {fields.Length}");
            Console.WriteLine($"titles cnt: {titles.Length}");

          Console.WriteLine("fields :");
        foreach (var field in fields)
        {
            Console.Write($"{field}, ");
        }
            Console.WriteLine("");
            Console.WriteLine("titles :");
            foreach (var title in titles)
        {
            Console.Write($"{title}, ");
        }
            Console.WriteLine("");*/

            //DevExpress.XtraReports.UI.PageBand

            PageHeaderBand pageHeader = new PageHeaderBand() { HeightF = startHeight, Name = "pageHeaderBand" };
      //pageHeader.Controls.Add(ttl);

      // 
      int colCountTB = titles.Length; // 전체 셀갯수로 변경 // 9;                                     // 한줄 cell 갯수
      int rowCountTB = 1; // (int)(fields.Length / colCountTB) + 1; // 헤더 행 갯수

      int cellHeight = 35;                                    // cell 높이

      int reportMarginsTop = 50;                              // 레포트 상단 여백
      int reportMarginsLeft = 0;                             // 레포트 좌측 여백
      int reportMarginsRight = 00;                            // 레포트 우측 여백

      float tableBorderWidth = 1F;

      int tableWidth = 100 * titles.Length;  //  (int)(report.PageWidth - reportMarginsLeft - reportMarginsRight);
      int cellWidth = 100; // 50으로 고정 // (tableWidth - 4) / colCountTB; // 4는 경계선 cell 두께
      //if (cellWidth < 50) {
      //  cellWidth = 50;
      //}


      ReportHeaderBand pageTitle = new ReportHeaderBand() { };
      pageTitle.BackColor = Color.FromArgb(5, 5, 5);

      // title
      XRLabel ttl = new XRLabel();

      ttl.Text = ri.Title;
      //ttl.WidthF = tableWidth;
      //ttl.HeightF = 100;
      ttl.Font = new DevExpress.Drawing.DXFont("Arial", 25.64F, DevExpress.Drawing.DXFontStyle.Bold | DevExpress.Drawing.DXFontStyle.Underline);
      ttl.AutoWidth = true;
      ttl.LocationFloat = new DevExpress.Utils.PointFloat(0f, 0F);
      ttl.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      ttl.SizeF = new System.Drawing.SizeF(tableWidth, 26F);
      //ttl.StylePriority.UseFont = false;
      //ttl.StylePriority.UseTextAlignment = false;
      ttl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;



      XRLabel sub_ttl = new XRLabel();

      sub_ttl.Text = ri.SubTitle;
      sub_ttl.AutoWidth = true;
      sub_ttl.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
      sub_ttl.LocationFloat = new DevExpress.Utils.PointFloat(0, 26F);
      sub_ttl.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      sub_ttl.SizeF = new System.Drawing.SizeF(tableWidth, 12F);
      sub_ttl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      // 
      // xrLabel3
      // 

      XRLabel username = new XRLabel();


      username.Text = ri.UName;
      username.AutoWidth = true;
      username.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
      username.LocationFloat = new DevExpress.Utils.PointFloat(0, 38F);
      username.Name = "xrLabel3";
      username.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      username.SizeF = new System.Drawing.SizeF(tableWidth, 12F);
      username.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
      // 
      // xrLabel4
      // 

      XRLabel date1 = new XRLabel();


      date1.Text = ri.Date1;
      date1.AutoWidth = true;
      date1.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
      date1.LocationFloat = new DevExpress.Utils.PointFloat(0, 50F);
      date1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      date1.SizeF = new System.Drawing.SizeF(tableWidth, 15F);
      date1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;



      // contents Panel
      XRPanel contentsPanel = new XRPanel();
      contentsPanel.SizeF = new System.Drawing.SizeF(tableWidth, 56);
      contentsPanel.LocationF = new DevExpress.Utils.PointFloat(reportMarginsLeft, reportMarginsTop);

      contentsPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            ttl,sub_ttl,username,date1 });

      // background panel
      XRPanel backPanel = new XRPanel();
      backPanel.SizeF = new System.Drawing.SizeF(tableWidth, 56 + reportMarginsTop);
      backPanel.LocationF = new DevExpress.Utils.PointFloat(0, 0);
      backPanel.BackColor = Color.FromArgb(240, 240, 240);
      backPanel.Controls.AddRange(new XRControl[] { contentsPanel });

      pageTitle.Controls.AddRange(new XRControl[] { backPanel });


      // header table


      XRTable headerTable = XRTable.CreateTable(
                            new Rectangle(0,    // rect X
                                          0,          // rect Y
                                          tableWidth, // width
                                          cellHeight * rowCountTB),        // height
                                          1,          // table row count
                                          0);         // table column count
      headerTable.LocationF = new DevExpress.Utils.PointFloat(reportMarginsLeft, 0);
      //headerTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
      headerTable.BackColor = Color.FromArgb(230, 230, 230);
      headerTable.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
      headerTable.BeginInit();

      ////첫번째 row는 상단줄긋기용
      //XRTableRow firstRow = headerTable.Rows.FirstRow;
      //SetTableTopBorder(firstRow, tableWidth, tableBorderWidth, cellWidth, colCountTB);

      // 받은 Columns2 출력(디버깅용)
      //foreach (var kvp in cols2)
      //{
      //    var meta = kvp.Value;
      //    Console.WriteLine($"FieldName: {meta.FieldName}, ColName: {meta.ColName}, Kind: {meta.Kind}, " +
      //                        $"ParentId: {meta.ParentId}, Order: {meta.Order}, Depth: {meta.Depth}, " +
      //                        $"ColSpan: {meta.ColSpan}, RowSpan: {meta.RowSpan}");
      //}
      
      // 밴드 컬럼이 포함된 Columns2 그리기
      int headerRows = cols2.Values.Max(m => m.Depth) + 1;
      var rows = new XRTableRow[headerRows];
      
      // 헤더 테이블에 row 등록
      for (int r = 0; r < headerRows; r++)
      {
        rows[r] = r == 0 ? headerTable.Rows.FirstRow : new XRTableRow();
        headerTable.Rows.Add(rows[r]);
      }

      // 재귀로 모든 헤더 셀 Row에 추가.
      void AddCells(string? parentId, int depth)
      {
        foreach (var col in cols2.Values.Where(m => m.ParentId == parentId && m.FieldName != "CHK")
                                            .OrderBy(m => m.Order))
        {
            XRTableCell cell = CreateHeaderTableCell(cellWidth * col.ColSpan, col.ColName);
            cell.Borders = DevExpress.XtraPrinting.BorderSide.All;
            cell.BorderWidth = 0.1F;

            // row 셀병합
            if (col.RowSpan > 1)
            {
                cell.RowSpan = col.RowSpan;

                // ── 아래 행들에 더미 셀 삽입 ───────────────────
                for (int i = 1; i < col.RowSpan; i++)
                {
                    var dummy = new XRTableCell { WidthF = cell.WidthF };
                    rows[depth + i].Cells.Add(dummy);
                }
            }

            rows[depth].Cells.Add(cell);

            if (col.Kind == "Band")
              AddCells(col.FieldName, depth + 1);   // 자식 밴드/데이터 재귀
        }
      }
      AddCells(null, 0);


      // 정해진 컬럼 갯수를 초과하면 한줄씩 추가
      /*for (int i = 0; i < rowCountTB; i++) {
        XRTableRow tableRow = new XRTableRow();
        tableRow.Width = tableWidth;
        tableRow.HeightF = (float)cellHeight;

        // 첫번째 셀은 테이블 경계선 긋기용
        //XRTableCell leftLineCell = CreateTableLeftBorder(tableBorderWidth);
        //tableRow.Cells.Add(leftLineCell);

        // 한줄에 colCountTB만큼 cell추가
        for (int j = i * colCountTB; j < ((i + 1) * colCountTB); j++) {
          // 마지막 행은 cell 갯수가 부족하면 빈 cell로 채우기
          XRTableCell cell =
              (j < fields.Length) ? CreateHeaderTableCell(cellWidth, titles[j]) : CreateHeaderTableCell(cellWidth, "");

          // 마지막 cell오른쪽은 border를 없애줘야함
          //if (j < ((i + 1) * colCountTB) - 1)
            cell.Borders = DevExpress.XtraPrinting.BorderSide.All;
          //if (i < rowCountTB - 1)
          //  cell.Borders |= DevExpress.XtraPrinting.BorderSide.Bottom;
          cell.BorderWidth = 0.1F;
          //cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;

          tableRow.Cells.Add(cell);
        }

        // 마지막 셀은 테이블 경계선 긋기용
        //XRTableCell RightLineCell = CreateTableRightBorder(tableBorderWidth);
        //tableRow.Cells.Add(RightLineCell);

        headerTable.Rows.Add(tableRow);
      }*/

      // 마지막 행은 하단줄긋기용
      //XRTableRow lastRow = new XRTableRow();
      //SetTableBottomBorder(lastRow, tableWidth, tableBorderWidth, cellWidth, colCountTB);
      //headerTable.Rows.Add(lastRow);

      //
      headerTable.EndInit();
      headerTable.AdjustSize();
      pageHeader.Controls.Add(headerTable);

      DetailBand detail = new DetailBand() { HeightF = cellHeight * rowCountTB, Name = "detailBand" };

      XRTable detailTable = XRTable.CreateTable(
                      new Rectangle(reportMarginsLeft,    // rect X
                                      0,          // rect Y
                                      tableWidth, // width
                                      cellHeight * rowCountTB),        // height
                                      1,          // table row count
                                      0);         // table column count

      detailTable.Width = tableWidth;
      detailTable.BeginInit();

      //
      for (int i = 0; i < rowCountTB; i++) {
        XRTableRow tableRow = (i == 0) ? detailTable.Rows.FirstRow : new XRTableRow();
        tableRow.Width = tableWidth;
        tableRow.HeightF = (float)cellHeight;

        // 첫번째 셀은 테이블 경계선 긋기용
        //XRTableCell leftLineCell = CreateTableLeftBorder(tableBorderWidth);
        //tableRow.Cells.Add(leftLineCell);

        // 한줄에 colCountTB만큼 cell추가
        // 마지막 행은 cell 갯수가 부족하면 빈 cell로 채우기
        for (int j = i * colCountTB; j < ((i + 1) * colCountTB); j++) {
          XRTableCell cell =
              (j < fields.Length) ? CreateDetailTableCell(fields[j], cellWidth) : CreateDetailTableCell("", cellWidth);
          // 마지막 cell오른쪽은 border를 없애줘야함
          //if (j < ((i + 1) * colCountTB) - 1)
          cell.Borders = DevExpress.XtraPrinting.BorderSide.All;
          //if (i < rowCountTB - 1)
          //  cell.Borders |= DevExpress.XtraPrinting.BorderSide.Bottom;
          cell.BorderWidth = 0.1F;
          //cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
          tableRow.Cells.Add(cell);

        }

        // 마지막 셀은 테이블 경계선 긋기용
        //XRTableCell RightLineCell = new XRTableCell();
        //RightLineCell.Width = 2;
        //RightLineCell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        //RightLineCell.BorderWidth = tableBorderWidth;
        //tableRow.Cells.Add(RightLineCell);

        if (i > 0) detailTable.Rows.Add(tableRow);
      }

      // 마지막 행은 하단줄긋기용
      //XRTableRow detailLastRow = new XRTableRow();
      //SetTableBottomBorder(detailLastRow, tableWidth, tableBorderWidth, cellWidth, colCountTB);
      //detailTable.Rows.Add(detailLastRow);


      detailTable.Font = new Font("맑은 고딕", 10);
      detailTable.EndInit();
      detailTable.AdjustSize();
      detail.Controls.Add(detailTable);










      // report footer 
      ReportFooterBand reportFooter = new ReportFooterBand() { HeightF = 0, Name = "reportFooterBand" };

      // 한주
      XRPageInfo pageInfo1 = new XRPageInfo();
      pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(reportMarginsLeft, 10F);
      pageInfo1.Name = "pageInfo1";
      pageInfo1.TextFormatString = "주식회사 한주";
      pageInfo1.SizeF = new System.Drawing.SizeF(tableWidth / 3, startHeight);
      pageInfo1.Font = new Font("맑은 고딕", 10);


      // 날짜
      XRPageInfo pageInfo3 = new XRPageInfo();
      pageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(reportMarginsLeft + (tableWidth * 2 / 3), 10F);
      pageInfo3.Name = "pageInfo3";
      pageInfo3.SizeF = new System.Drawing.SizeF(tableWidth / 3, startHeight);
      pageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
      pageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
      pageInfo3.Font = new Font("맑은 고딕", 10);

      reportFooter.Controls.AddRange(new XRControl[] { pageInfo1, pageInfo3 });






      // bottom margin band
      BottomMarginBand bottomMargin = new BottomMarginBand() { Name = "BottomMargin" };

      // 페이지수
      //XRPageInfo pageInfo = new XRPageInfo();
      //pageInfo.LocationFloat = new DevExpress.Utils.PointFloat(reportMarginsLeft + (tableWidth / 3), 10F);
      //pageInfo.Name = "pageInfo2";
      //pageInfo.TextFormatString = "Page {0} / {1}";
      //pageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      //pageInfo.SizeF = new System.Drawing.SizeF(tableWidth / 3, startHeight);



      //bottomMargin.Controls.AddRange(new XRControl[] { pageInfo });



      //
      report.Bands.AddRange(new Band[] { detail, pageHeader, pageTitle, reportFooter, bottomMargin });
    }

    public static void CreateReport(XtraReport report, RptInfo ri) {
      report.Margins.Left = 0;
      report.Margins.Top = 0;
      report.Margins.Right = 0;
      report.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
      var cols = ri.Columns;
      DataTable dt = ri.Data as DataTable;
      report.Landscape = ri.Landscape == "Y" ? true : false;
      report.DataSource = dt;
      string colfname = "";
      string coltname = "";
      foreach (var c in cols) {
        if (c.Key != "CHK") {
          colfname += c.Key + ",";
          coltname += c.Value + ",";
        }
      }
      float startHeight = 23;
      string[] fields = colfname.Split(',', StringSplitOptions.RemoveEmptyEntries);
      string[] titles = coltname.Split(',', StringSplitOptions.RemoveEmptyEntries);

      //DevExpress.XtraReports.UI.PageBand

      PageHeaderBand pageHeader = new PageHeaderBand() { HeightF = startHeight, Name = "pageHeaderBand" };
      //pageHeader.Controls.Add(ttl);

      // 
      int colCountTB = 9;                                     // 한줄 cell 갯수
      int rowCountTB = (int)(fields.Length / colCountTB) + 1; // 헤더 행 갯수

      int cellHeight = 30;                                    // cell 높이

      int reportMarginsTop = 50;                              // 레포트 상단 여백
      int reportMarginsLeft = 0;                             // 레포트 좌측 여백
      int reportMarginsRight = 00;                            // 레포트 우측 여백

      float tableBorderWidth = 1F;

      int tableWidth = (int)(report.PageWidth - reportMarginsLeft - reportMarginsRight);
      int cellWidth = (tableWidth - 4) / colCountTB; // 4는 경계선 cell 두께
      if (cellWidth < 50) {
        cellWidth = 50;
      }


      ReportHeaderBand pageTitle = new ReportHeaderBand() { };
      pageTitle.BackColor = Color.FromArgb(5, 5, 5);

      // title
      XRLabel ttl = new XRLabel();

      ttl.Text = ri.Title;
      //ttl.WidthF = tableWidth;
      //ttl.HeightF = 100;
      ttl.Font = new DevExpress.Drawing.DXFont("Arial", 25.64F, DevExpress.Drawing.DXFontStyle.Bold | DevExpress.Drawing.DXFontStyle.Underline);
      ttl.AutoWidth = true;
      ttl.LocationFloat = new DevExpress.Utils.PointFloat(0f, 0F);
      ttl.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      ttl.SizeF = new System.Drawing.SizeF(tableWidth, 26F);
      //ttl.StylePriority.UseFont = false;
      //ttl.StylePriority.UseTextAlignment = false;
      ttl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;



      XRLabel sub_ttl = new XRLabel();

      sub_ttl.Text = ri.SubTitle;
      sub_ttl.AutoWidth = true;
      sub_ttl.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
      sub_ttl.LocationFloat = new DevExpress.Utils.PointFloat(0, 26F);
      sub_ttl.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      sub_ttl.SizeF = new System.Drawing.SizeF(tableWidth, 12F);
      sub_ttl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      // 
      // xrLabel3
      // 

      XRLabel username = new XRLabel();


      username.Text = ri.UName;
      username.AutoWidth = true;
      username.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
      username.LocationFloat = new DevExpress.Utils.PointFloat(0, 38F);
      username.Name = "xrLabel3";
      username.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      username.SizeF = new System.Drawing.SizeF(tableWidth, 12F);
      username.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
      // 
      // xrLabel4
      // 

      XRLabel date1 = new XRLabel();


      date1.Text = ri.Date1;
      date1.AutoWidth = true;
      date1.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
      date1.LocationFloat = new DevExpress.Utils.PointFloat(0, 50F);
      date1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
      date1.SizeF = new System.Drawing.SizeF(tableWidth, 15F);
      date1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;



      // contents Panel
      XRPanel contentsPanel = new XRPanel();
      contentsPanel.SizeF = new System.Drawing.SizeF(tableWidth, 56);
      contentsPanel.LocationF = new DevExpress.Utils.PointFloat(reportMarginsLeft, reportMarginsTop);

      contentsPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            ttl,sub_ttl,username,date1 });

      // background panel
      XRPanel backPanel = new XRPanel();
      backPanel.SizeF = new System.Drawing.SizeF(report.PageWidth, 56 + reportMarginsTop);
      backPanel.LocationF = new DevExpress.Utils.PointFloat(0, 0);
      backPanel.BackColor = Color.FromArgb(240, 240, 240);
      backPanel.Controls.AddRange(new XRControl[] { contentsPanel });

      pageTitle.Controls.AddRange(new XRControl[] { backPanel });


      // header table


      XRTable headerTable = XRTable.CreateTable(
                            new Rectangle(0,    // rect X
                                          0,          // rect Y
                                          tableWidth, // width
                                          cellHeight * rowCountTB),        // height
                                          1,          // table row count
                                          0);         // table column count
      headerTable.LocationF = new DevExpress.Utils.PointFloat(reportMarginsLeft, 0);
      //headerTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
      headerTable.BackColor = Color.FromArgb(230, 230, 230);
      headerTable.Font = new Font("Verdana", 10, FontStyle.Bold);
      headerTable.BeginInit();

      //첫번째 row는 상단줄긋기용
      XRTableRow firstRow = headerTable.Rows.FirstRow;
      SetTableTopBorder(firstRow, tableWidth, tableBorderWidth, cellWidth, colCountTB);

      // 정해진 컬럼 갯수를 초과하면 한줄씩 추가
      for (int i = 0; i < rowCountTB; i++) {
        XRTableRow tableRow = new XRTableRow();
        tableRow.Width = tableWidth;
        tableRow.HeightF = (float)cellHeight;

        // 첫번째 셀은 테이블 경계선 긋기용
        XRTableCell leftLineCell = CreateTableLeftBorder(tableBorderWidth);
        tableRow.Cells.Add(leftLineCell);

        // 한줄에 colCountTB만큼 cell추가
        for (int j = i * colCountTB; j < ((i + 1) * colCountTB); j++) {
          // 마지막 행은 cell 갯수가 부족하면 빈 cell로 채우기
          XRTableCell cell =
              (j < fields.Length) ? CreateHeaderTableCell(cellWidth, titles[j]) : CreateHeaderTableCell(cellWidth, "");

          // 마지막 cell오른쪽은 border를 없애줘야함
          if (j < ((i + 1) * colCountTB) - 1)
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
          if (i < rowCountTB - 1)
            cell.Borders |= DevExpress.XtraPrinting.BorderSide.Bottom;
          cell.BorderWidth = 0.1F;
          //cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;

          tableRow.Cells.Add(cell);
        }

        // 마지막 셀은 테이블 경계선 긋기용
        XRTableCell RightLineCell = CreateTableRightBorder(tableBorderWidth);
        tableRow.Cells.Add(RightLineCell);

        headerTable.Rows.Add(tableRow);
      }

      // 마지막 행은 하단줄긋기용
      XRTableRow lastRow = new XRTableRow();
      SetTableBottomBorder(lastRow, tableWidth, tableBorderWidth, cellWidth, colCountTB);
      headerTable.Rows.Add(lastRow);

      //
      headerTable.EndInit();
      headerTable.AdjustSize();
      pageHeader.Controls.Add(headerTable);

      DetailBand detail = new DetailBand() { HeightF = cellHeight * rowCountTB, Name = "detailBand" };

      XRTable detailTable = XRTable.CreateTable(
                      new Rectangle(reportMarginsLeft,    // rect X
                                      0,          // rect Y
                                      tableWidth, // width
                                      cellHeight * rowCountTB),        // height
                                      1,          // table row count
                                      0);         // table column count

      detailTable.Width = tableWidth;
      detailTable.BeginInit();

      //
      for (int i = 0; i < rowCountTB; i++) {
        XRTableRow tableRow = (i == 0) ? detailTable.Rows.FirstRow : new XRTableRow();
        tableRow.Width = tableWidth;
        tableRow.HeightF = (float)cellHeight;

        // 첫번째 셀은 테이블 경계선 긋기용
        XRTableCell leftLineCell = CreateTableLeftBorder(tableBorderWidth);
        tableRow.Cells.Add(leftLineCell);

        // 한줄에 colCountTB만큼 cell추가
        // 마지막 행은 cell 갯수가 부족하면 빈 cell로 채우기
        for (int j = i * colCountTB; j < ((i + 1) * colCountTB); j++) {
          XRTableCell cell =
              (j < fields.Length) ? CreateDetailTableCell(fields[j], cellWidth) : CreateDetailTableCell("", cellWidth);
          // 마지막 cell오른쪽은 border를 없애줘야함
          if (j < ((i + 1) * colCountTB) - 1)
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
          if (i < rowCountTB - 1)
            cell.Borders |= DevExpress.XtraPrinting.BorderSide.Bottom;
          cell.BorderWidth = 0.1F;
          //cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
          tableRow.Cells.Add(cell);

        }

        // 마지막 셀은 테이블 경계선 긋기용
        XRTableCell RightLineCell = new XRTableCell();
        RightLineCell.Width = 2;
        RightLineCell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        RightLineCell.BorderWidth = tableBorderWidth;
        tableRow.Cells.Add(RightLineCell);

        if (i > 0) detailTable.Rows.Add(tableRow);
      }

      // 마지막 행은 하단줄긋기용
      XRTableRow detailLastRow = new XRTableRow();
      SetTableBottomBorder(detailLastRow, tableWidth, tableBorderWidth, cellWidth, colCountTB);
      detailTable.Rows.Add(detailLastRow);


      detailTable.Font = new Font("Verdana", 8F);
      detailTable.EndInit();
      detailTable.AdjustSize();
      detail.Controls.Add(detailTable);










      // report footer 
      ReportFooterBand reportFooter = new ReportFooterBand() { HeightF = 0, Name = "reportFooterBand" };

      // 한주
      XRPageInfo pageInfo1 = new XRPageInfo();
      pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(reportMarginsLeft, 10F);
      pageInfo1.Name = "pageInfo1";
      pageInfo1.TextFormatString = "주식회사 한주";
      pageInfo1.SizeF = new System.Drawing.SizeF(tableWidth / 3, startHeight);


      // 날짜
      XRPageInfo pageInfo3 = new XRPageInfo();
      pageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(reportMarginsLeft + (tableWidth * 2 / 3), 10F);
      pageInfo3.Name = "pageInfo3";
      pageInfo3.SizeF = new System.Drawing.SizeF(tableWidth / 3, startHeight);
      pageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
      pageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;

      reportFooter.Controls.AddRange(new XRControl[] { pageInfo1, pageInfo3 });






      // bottom margin band
      BottomMarginBand bottomMargin = new BottomMarginBand() { Name = "BottomMargin" };

      // 페이지수
      XRPageInfo pageInfo = new XRPageInfo();
      pageInfo.LocationFloat = new DevExpress.Utils.PointFloat(reportMarginsLeft + (tableWidth / 3), 10F);
      pageInfo.Name = "pageInfo2";
      pageInfo.TextFormatString = "Page {0} / {1}";
      pageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
      pageInfo.SizeF = new System.Drawing.SizeF(tableWidth / 3, startHeight);



      bottomMargin.Controls.AddRange(new XRControl[] { pageInfo });



      //
      report.Bands.AddRange(new Band[] { detail, pageHeader, pageTitle, reportFooter, bottomMargin });
    }


    public static XRTableCell CreateHeaderTableCell(int cellWidth, string text)
        {
            XRTableCell cell = new XRTableCell();
            cell.Width = cellWidth;
            cell.Text = text;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            return cell;
        }

        public static XRTableCell CreateDetailTableCell(string field, int cellWidth)
        {
            XRTableCell cell = new XRTableCell();
            if (!string.IsNullOrEmpty(field))
            {
                ExpressionBinding binding = new ExpressionBinding("BeforePrint", "Text", String.Format("[{0}]", field));
                cell.ExpressionBindings.Add(binding);
            }
            else { cell.Text = ""; }
            cell.Width = cellWidth;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            if (field.Contains("Date"))
                cell.TextFormatString = "{0:MM/dd/yyyy}";

            return cell;
        }
        public static void SetTableTopBorder(XRTableRow borderRow, int tableWidth, float tableBorderWidth, int cellWidth, int colCountTB)
        {
            borderRow.Width = tableWidth;
            borderRow.HeightF = 2;
            borderRow.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            borderRow.BorderWidth = tableBorderWidth;
            for (int i = 0; i < colCountTB; ++i)
            {
                // 마지막 행은 cell 갯수가 부족하면 빈 cell로 채우기
                XRTableCell cell = new XRTableCell();
                cell.Width = cellWidth;
                if (i == 0) cell.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left;
                else if (i == colCountTB - 1) cell.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right;
                borderRow.Cells.Add(cell);
            }
        }
        public static void SetTableBottomBorder(XRTableRow borderRow, int tableWidth, float tableBorderWidth, int cellWidth, int colCountTB)
        {
            borderRow.Width = tableWidth;
            borderRow.HeightF = 2;
            borderRow.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            borderRow.BorderWidth = tableBorderWidth;
            for (int i = 0; i < colCountTB; ++i)
            {
                // 마지막 행은 cell 갯수가 부족하면 빈 cell로 채우기
                XRTableCell cell = new XRTableCell();
                cell.Width = cellWidth;
                if (i == 0) cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left;
                else if (i == colCountTB - 1) cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right;
                borderRow.Cells.Add(cell);
            }
        }
        public static XRTableCell CreateTableLeftBorder(float tableBorderWidth)
        {
            XRTableCell leftLineCell =  new XRTableCell();
            leftLineCell.Width = 2;
            leftLineCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            leftLineCell.BorderWidth = tableBorderWidth;
            return leftLineCell;
        }
        public static XRTableCell CreateTableRightBorder(float tableBorderWidth)
        {
            XRTableCell rightLineCell = new XRTableCell();
            rightLineCell.Width = 2;
            rightLineCell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            rightLineCell.BorderWidth = tableBorderWidth;
            return rightLineCell;
        }
    }
}
