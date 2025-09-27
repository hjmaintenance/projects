using DevExpress.Drawing.Internal.Fonts.Interop;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Popup
{

    /// <summary>
    /// 공통 팝업창 셋팅 클래스
    /// </summary>
    public class CommonPopupSettings
    {
        #region [ 유저가 셋팅 하는 부분 ]
        /// <summary>
        /// 팝업 타이틀
        /// </summary>
        public string? HeaderText { get; set; }
        /// <summary>
        /// 팝업 가로 크기
        /// </summary>
        public int Width { get; set; } = 400;
        /// <summary>
        /// 팝업 세로 크기
        /// </summary>
        public int Height { get; set; } = 300;
        /// <summary>
        /// 팝업 최대 가로 크기
        /// </summary>
        public int MaxWidth { get; set; } = 1600;
        /// <summary>
        /// 팝업 최대 세로 크기
        /// </summary>
        public int MaxHeight { get; set; } = 1024;
        /// <summary>
        /// 팝업 최소 가로 크기
        /// </summary>
        public int MinWidth { get; set; } = 400;
        /// <summary>
        /// 팝업 최소 세로 크기
        /// </summary>
        public int MinHeight { get; set; } = 100;
        /// <summary>
        /// 팝업으로 전달할 파라메터 셋팅
        /// </summary>
        public object? Parameter { get; set; }
        /// <summary>
        /// 팝업 오픈할 Razor 페이지 Type 셋팅
        /// </summary>
        public Type? FormComponentType { get; set; }
        /// <summary>
        /// 팝업이 닫힐때 호출 되는 이벤트 콜백
        /// </summary>
        public EventCallback<PopupResult> CloseCallback;

        #endregion [ 유저가 셋팅 하는 부분 ]

        
    }

    /// <summary>
    /// 공통 팝업창에서 사용하는 Setting 정보
    /// </summary>
    public class CommonPopupSystemSettings : CommonPopupSettings
    {
        /// <summary>
        /// Scroll width 영역 셋팅
        /// </summary>
        public string? BodyScollWidth { get; set; } = "400px";
        /// <summary>
        /// Scroll height 영역 셋팅
        /// </summary>
        public string? BodyScollHeight { get; set; } = "300px";

        //public PopupResult PopupResult { get; init; } = new PopupResult();
        /// <summary>
        /// 팝업창 닫힐때 Callback 함수 호출
        /// </summary>
        /// <param name="result"></param>
        public void StartCloseCallback(PopupResult result)
        {
            CloseCallback.InvokeAsync(result);
        }

        public CommonPopupSystemSettings(CommonPopupSettings settings)
        {
            HeaderText = settings.HeaderText;
            Width = settings.Width;
            Height = settings.Height;
            MaxWidth = settings.MaxWidth;
            MaxHeight = settings.MaxHeight;
            MinWidth = settings.MinWidth;
            MinHeight = settings.MinHeight;
            Parameter = settings.Parameter;
            FormComponentType = settings.FormComponentType;
            CloseCallback = settings.CloseCallback;
    }
    }
}
