using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Data {
  /// <summary>
  /// 유저 관련 정보 클래스
  /// </summary>
  public class User {
    /// <summary>
    /// 유저아이디
    /// </summary>
    public string? USER_ID { get; set; }
    /// <summary>
    /// 유저 권한 아이디
    /// </summary>
    public string? ROLE_GRP_ID { get; set; }
    /// <summary>
    /// 유저명
    /// </summary>
    public string? USER_NAME { get; set; }
    /// <summary>
    /// 유저 영문명
    /// </summary>
    public string? USER_NAME_ENG { get; set; }
    /// <summary>
    /// 유저 사원번호
    /// </summary>
    public string? EMP_NO { get; set; }
    /// <summary>
    /// 유저 회사코드
    /// </summary>
    public string? CUST_CODE { get; set; }
    /// <summary>
    /// 유저 부서코드
    /// </summary>
    public string? DEPT_CODE { get; set; }
    /// <summary>
    /// 유저 사무실 번호
    /// </summary>
    public string? OFFICE_NUM { get; set; }
    /// <summary>
    /// 유저 핸드폰 번호
    /// </summary>
    public string? PHONE_NUM { get; set; }
    /// <summary>
    /// 유저 주소1
    /// </summary>
    public string? ADDRESS_1 { get; set; }
    /// <summary>
    /// 유저 주소2
    /// </summary>
    public string? ADDRESS_2 { get; set; }
    /// <summary>
    /// 유저 도시
    /// </summary>
    public string? COUNTRY { get; set; }
    /// <summary>
    /// 생성시간
    /// </summary>
    public DateTime CREATETIME { get; set; }
    /// <summary>
    /// AccessToken
    /// </summary>
    public string? Token { get; set; }
    /// <summary>
    /// AccessTokenID
    /// </summary>
    public string? ACCESS_TOKEN_ID { get; set; }
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string? REFRESH_TOKEN { get; set; }
    /// <summary> 사용자 테마 </summary>
    public string? Theme { get; set; } = "Mit";
    /// <summary> 사용자 화면크기 </summary>
    public string? Size { get; set; } = "Small";

    /// <summary> 사용자 사진 </summary>
    public string? Photo { get; set; }

    public string? USER_TYPE { get; set; }
  }
}
