using DevExpress.Drawing.Internal.Fonts.Interop;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Popup
{
    /// <summary>
    /// CommonPopup.razor에서만 사용
    /// 팝업 페이지에서 팝업 추가/닫기 호출 이벤트 인터페이스
    /// </summary>
    public interface ICommonPopupSystemService
    {
        /// <summary>
        /// 공통 팝업 Razor 페이지 에서 사용
        /// 유저 사용금지
        /// </summary>
        event Action<CommonPopupSettings> OnOpenPopup;

        /// <summary>
        /// 공통 팝업 Razor 페이지 에서 사용
        /// 유저 사용금지
        /// </summary>
        event Action<CommonPopupSettings, PopupResult> OnClosePopup;

        /// <summary>
        /// 팝업창 닫기
        /// 공통 팝업 클래스에서 처리중
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="result"></param>
        void Close(CommonPopupSettings settings, PopupResult result);
    }

    /// <summary>
    /// 공통 팝업 관련 인터페이스 서비스
    /// </summary>
    public interface ICommonPopupService
    {
        /// <summary>
        /// 팝업창 열기 및 팝업 정보 셋팅
        /// </summary>
        /// <param name="settings"></param>
        void Show(CommonPopupSettings settings);

        /// <summary>
        /// 팝업창 열기 및 팝업 정보 셋팅
        /// </summary>
        /// <param name="FormComponentType"></param>
        /// <param name="HeaderText"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="MaxWidth"></param>
        /// <param name="MaxHeight"></param>
        /// <param name="MinWidth"></param>
        /// <param name="MinHeight"></param>
        /// <param name="Parameter"></param>
        /// <param name="CloseCallback"></param>
        void Show(Type? FormComponentType
            , string? HeaderText = ""
            , int Width = 400
            , int Height = 300
            , int MaxWidth = 1600
            , int MaxHeight = 1024
            , int MinWidth = 100
            , int MinHeight = 100
            , object? Parameter = null
            , Func<PopupResult, Task>? CloseCallback = null);

        
    }

    /// <summary>
    /// 공통 팝업 관련 클래스 서비스
    /// </summary>
    public class CommonPopupService : ICommonPopupService, ICommonPopupSystemService
    {
        /// <summary>
        /// 공통 팝업 Razor 페이지 에서 사용
        /// 유저 사용금지
        /// </summary>
        public event Action<CommonPopupSettings>? OnOpenPopup;
        /// <summary>
        /// 공통 팝업 Razor 페이지 에서 사용
        /// 유저 사용금지
        /// </summary>
        public event Action<CommonPopupSettings, PopupResult>? OnClosePopup;

        /// <summary>
        /// 팝업창 열기 및 팝업 정보 셋팅
        /// </summary>
        /// <param name="settings"></param>
        public void Show(CommonPopupSettings settings)
        {
            OnOpenPopup?.Invoke(settings);
        }

        /// <summary>
        /// 팝업창 열기 및 팝업 정보 셋팅
        /// </summary>
        /// <param name="FormComponentType"></param>
        /// <param name="HeaderText"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="MaxWidth"></param>
        /// <param name="MaxHeight"></param>
        /// <param name="MinWidth"></param>
        /// <param name="MinHeight"></param>
        /// <param name="Parameter"></param>
        /// <param name="CloseCallback"></param>
        public void Show(Type? FormComponentType
            , string? HeaderText = ""
            , int Width = 400
            , int Height = 300
            , int MaxWidth = 1600
            , int MaxHeight = 1024
            , int MinWidth = 400
            , int MinHeight = 100
            , object? Parameter = null
            , Func<PopupResult, Task>? CloseCallback = null)
        {
            var settings = new CommonPopupSettings();
            settings.HeaderText = HeaderText;
            settings.Width = Width;
            settings.Height = Height;
            settings.MaxWidth = MaxWidth;
            settings.MaxHeight = MaxHeight;
            settings.MinWidth = MinWidth;
            settings.MinHeight = MinHeight;
            settings.Parameter = Parameter;
            settings.FormComponentType = FormComponentType;
            if (CloseCallback != null)
                settings.CloseCallback = EventCallback.Factory.Create(this, CloseCallback);

            OnOpenPopup?.Invoke(settings);
        }

        /// <summary>
        /// 팝업창 닫기
        /// 공통 팝업 클래스에서 처리중
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="result"></param>
        public void Close(CommonPopupSettings settings, PopupResult result)
        {
            OnClosePopup?.Invoke(settings, result);
        }
    }
}
