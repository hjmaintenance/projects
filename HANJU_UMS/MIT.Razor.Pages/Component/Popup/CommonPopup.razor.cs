using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Popup
{
    /// <summary>
    /// 공통 팝업 클래스
    /// </summary>
    public class CommonPopupBase : CommonComponentBase, IDisposable
    {
        /// <summary>
        /// 팝업 정보 리스트
        /// </summary>
        protected List<CommonPopupSystemSettings> _listSettings = new List<CommonPopupSystemSettings>();

        protected override void OnInitialized()
        {
            var popupService = CommonPopupService as ICommonPopupSystemService;

            // 팝업 Show에서 추가된 정보 Add 및 보이기 이벤트 추가
            if (popupService != null)
                popupService.OnOpenPopup += OnOpenPopup;
            // 팝업 Show에서 추가된 정보 close 및 삭제 이벤트 추가
            if (popupService != null)
                popupService.OnClosePopup += OnPopupClosed;
            
            if (NavigationManager != null)
                NavigationManager.LocationChanged += OnLocationChanged;
        }

        public void Dispose()
        {
            var popupService = CommonPopupService as ICommonPopupSystemService;

            // 팝업 Show에서 추가된 정보 Add 및 보이기 이벤트 삭제
            if (popupService != null)
                popupService.OnOpenPopup -= OnOpenPopup;
            // 팝업 Show에서 추가된 정보 close 및 삭제 이벤트 삭제
            if (popupService != null)
                popupService.OnClosePopup -= OnPopupClosed;

            if (NavigationManager != null)
                NavigationManager.LocationChanged -= OnLocationChanged;
        }

        /// <summary>
        /// 로케이션 변경시 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            _listSettings.Clear();
            StateHasChanged();
        }

        /// <summary>
        /// Show 함수 호출 시 팝업 정보 추가 및 셋팅
        /// </summary>
        /// <param name="item"></param>
        private void OnOpenPopup(CommonPopupSettings item)
        {
            var popup = new CommonPopupSystemSettings(item);

            popup.BodyScollWidth = $"{item.Width - 20}px";
            popup.BodyScollHeight = $"{item.Height - 115}px";

            _listSettings.Add(popup);

            StateHasChanged();
        }

        /// <summary>
        /// 팝업에 적용될 Razor Page 정보 셋팅 및 그리기
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        protected RenderFragment? RenderEditForm(CommonPopupSystemSettings setting)
        {
            if (setting.FormComponentType == null)
                return null;
            
            RenderFragment item = s =>
            {
                int seq = 0;
                // 팝업 적용될 Razor Page 적용
                s.OpenComponent(seq++, setting.FormComponentType);
                // Razor Page에 Parameter 전달 변수 셋팅
                s.AddAttribute(seq++, "RecieveParameter", setting.Parameter);
                // 팝업 정보 변수 셋팅
                s.AddAttribute(seq++, "PopupSetting", setting);
                
                s.CloseComponent();
            };
            return item;
        }

        /// <summary>
        /// 팝업이 닫힘때 호출되는 함수
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="result"></param>
        protected void OnPopupClosed(CommonPopupSettings setting, PopupResult result)
        {
            if (setting is CommonPopupSystemSettings sysSettings)
            {
                // 리스트에 일치하는 팝업정보 없을때 리턴
                if (_listSettings.IndexOf(sysSettings) < 0)
                    return;

                // 팝업이 닫기/삭제 되기전에 Callback 함수 호출 및 팝업에서 보낸 결과 데이터 전달 
                sysSettings.StartCloseCallback(result);
                // 팝업 삭제
                _listSettings.Remove(sysSettings);
            }
            
            // 상태 적용
            StateHasChanged();
        }

        // 팝업 사이즈 변경될때 팝업 적용된 Razor Page 크기 변경 적용
        protected void OnResizeCompleted(CommonPopupSystemSettings setting, PopupResizeCompletedEventArgs e)
        {
            setting.BodyScollWidth = $"{e.Size.Width - 20}px";
            setting.BodyScollHeight = $"{e.Size.Height - 115}px";
            
            StateHasChanged();
        }

        
    }
}
