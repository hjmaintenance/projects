using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel {

  public class RptInfo {
    public string Title { get; set; }
    public DataTable Data { get; set; }
    public Dictionary<string, string> Columns { get; set; }
    public Dictionary<string, RptCol> Columns2 { get; set; } // xlsx에서만 사용
    public string SubTitle { get; set; }
    public string UName { get; set; }
    public string Date1 { get; set; }
    public string Date2 { get; set; }
    public string Landscape { get; set; }
    public string FileName { get; set; }
    public string ExportType { get; set; }
  }


  /// header 여러개를 어떻게 받아 올지 고민해서 완성 하자
  public record RptCol (
    string FieldName,   // 고유키, DataColumn은 FieldName, Band는 "B_"+ 순서
    string ColName,     // 헤더 텍스트
    string Kind,        // "Data" | "Band"
    string? ParentId,   // 상위 컬럼이 없으면 null
    int Order,          // 같은 레벨 내 좌→우 순서
    int Depth,          // 0 = 최상위
    int ColSpan,        // 병합할 열 수
    int RowSpan        // 병합할 행 수
  );


  public class DataPoint {
    public string SeriesName { get; set; }
    public DateTime GAUGE_DATETIME { get; set; }
    public double R_USEQTY { get; set; }
    //public double PEAK_VAL { get; set; }
    public bool IsPeak { get; set; } = false;

    public DataPoint(DateTime dtime, double qty, bool ispeak = false) {
      //(GAUGE_DATETIME, R_USEQTY) = (dtime, qty);
      (GAUGE_DATETIME, R_USEQTY, IsPeak) = (dtime, qty, ispeak);
    }

  }



  public class PeakPoint {
    public DateTime Sdt { get; set; }
    public DateTime Edt { get; set; }

    public double PEAK_VAL { get; set; }

    public PeakPoint(DateTime sdt, DateTime edt, double peak) {
      //(GAUGE_DATETIME, R_USEQTY) = (dtime, qty);
      (Sdt, Edt, PEAK_VAL) = (sdt, edt, peak);
    }

  }



  public class DataPoints {
    public bool IsVisible { get; set; } = true;
    public string? RName { get; set; }
    public List<DPoint>? DPoints { get; set; } = new List<DPoint>();
  }


  public class DPoint {
    public DateTime? Xtime { get; set; }
    public double? Qty1 { get; set; }
    public double? Qty2 { get; set; }
    public double? R_USEQTY { get; set; }
    public double? Press { get; set; }
    public double? Temper { get; set; }
    public int ViewType { get; set; } = 0;
    public double? ViewQty {
      get {
        if (ViewType == 0) {
          return R_USEQTY;
        }
        else if (ViewType == 1) {
          return Press;
        }
        else if (ViewType == 2) {
          return Temper;
        }
        else {
          return R_USEQTY;
        }
      }
    }
    public DPoint() {

    }
    public DPoint(DateTime xtime, double qty1, double qty2 = 0) {
      (Xtime, Qty1, Qty2) = (xtime, qty1, qty2);
    }
    public DPoint(DateTime xtime, double useqty, double press, double temper, int viewType) {
      (Xtime, R_USEQTY, Press, Temper, ViewType) = (xtime, useqty, press, temper, viewType);
    }
  }


}
