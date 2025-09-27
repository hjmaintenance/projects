using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.LoadingPanel
{
    /// <summary>
    /// LoadingPanel.razor에서 사용
    /// 로딩 패널 추가 프로세스 인터페이스
    /// </summary>
    public interface ILoadingPanelSystem
    {
        /// <summary>
        /// 로딩 Razor 페이지에서 사용
        /// 유저 사용금지
        /// </summary>
        event Action<bool>? OnLoadingPanelProgressAsync;
    }

    /// <summary>
    /// 로딩 패널 관련 서비스 인터페이스 
    /// </summary>
    public interface ILoadingPanelService
    {
        /// <summary>
        /// 로딩 패널 보이기
        /// </summary>
        void Show();
        /// <summary>
        /// 로딩 패널 닫기
        /// </summary>
        void Close();
    }

    /// <summary>
    /// 로딩 패널 관련 서비스 클래스
    /// </summary>
    public class LoadingPanelService : ILoadingPanelService, ILoadingPanelSystem
    {
        /// <summary>
        /// 로딩 Razor 페이지에서 사용
        /// 유저 사용금지
        /// </summary>
        public event Action<bool>? OnLoadingPanelProgressAsync;

        /// <summary>
        /// 로딩 패널 보이기
        /// </summary>
        public void Show() 
        {
            OnLoadingPanelProgressAsync?.Invoke(true);
        }

        /// <summary>
        /// 로딩 패널 닫기
        /// </summary>
        public void Close()
        {
            OnLoadingPanelProgressAsync?.Invoke(false);
        }

    }
}
