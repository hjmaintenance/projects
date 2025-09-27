using MIT.DataUtil.Common;
using DevExpress.XtraPrinting.Native.MarkupText;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Service;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
using MIT.Razor.Pages.Component.Popup;

namespace MIT.Razor.Pages.Component
{
    /// <summary>
    /// 공통 팝업창 관련 부모 클래스
    /// 팝업창에 적용할 Razor Page는 꼭 이클래스를 참조해서 팝업창을 열어야함
    /// </summary>
    public class CommonPopupComponentBase : CommonUIComponentBase
    {
        /// <summary>
        /// 팝업창으로 전달할 파라미터 값 셋팅 변수
        /// </summary>
        [Parameter]
        public object? RecieveParameter { get; set; }
        /// <summary>
        /// 팝업창에서 결과 값 전송할 팝업 상태 값과 파라미터 값 셋팅 변수
        /// </summary>
        public PopupResult Result { get; init; } = new PopupResult();
        /// <summary>
        /// 최초 팝업창에 셋팅된 정보 변수
        /// </summary>
        [Parameter]
        public CommonPopupSettings? PopupSetting { get; set; }

        /// <summary>
        /// 팝업창 닫기 호출 함수
        /// </summary>
        protected void PopupClose()
        {
            if (PopupSetting != null)
                (CommonPopupService as ICommonPopupSystemService)?.Close(PopupSetting, Result);
        }

        /// <summary>
        /// 팝업창 닫기 호출 함수
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="resultParameter"></param>
        protected void PopupClose(PopupResultType resultType = PopupResultType.OK, object? resultParameter = null)
        {
            Result.PopupResultType = resultType;
            Result.SetParameter(resultParameter);

            if (PopupSetting != null)
                (CommonPopupService as ICommonPopupSystemService)?.Close(PopupSetting, Result);
        }
    }
}
