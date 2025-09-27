using DevExpress.Charts.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages;
  public class CommCode {
    public string Name { get; set; }
    public string Value { get; set; }
    public string Desc { get; set; }
  public Dictionary<string, string> Others { get; set; } = new Dictionary<string, string>();
}



  public class TrendCode: CommCode {

  public enum TrendTypeEnum {    Real, Day, Month, CurMonth, CurMonthRe  }
  public enum GAUGE_TYPE {    PW, WA, ST  }
  public DateTime CompreDate { get; set; } = DateTime.Now.AddDays(-1);
  public TrendTypeEnum TrendType { get; set; } = TrendTypeEnum.Real;
  public string TrendTypeName { get {

      return TrendType switch {
        TrendTypeEnum.Real => "실사용량(KW)",
        TrendTypeEnum.Day => "일사용량(KW)",
        TrendTypeEnum.Month => "월사용량(KW)",
        TrendTypeEnum.CurMonth => "당월PEAK(KW)",
        TrendTypeEnum.CurMonthRe => "당월역율(%)",
        _ => "실사용량(KW)"
      };

    } }
  public string ComId { get; set; }
  public GAUGE_TYPE GaugeType { get; set; } = GAUGE_TYPE.PW;
  public string GaugeTypeName { get {
      return GaugeType switch {
        GAUGE_TYPE.PW => "전기",
        GAUGE_TYPE.WA => "융수",
        GAUGE_TYPE.ST => "증기",
        _ => "전기"
      };
    }
  } 
  public string FdrId { get; set; }
}


