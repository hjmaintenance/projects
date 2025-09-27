using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.MessageBox
{

    /// <summary>
    /// CommonMesageBox.razor 페이지에서 사용
    /// 메시지 박스 추가 프로세스
    /// </summary>
    public interface ICommonMessageBoxSystem
    {
        /// <summary>
        /// 메시지 박스 Razor 페이지에서 사용
        /// 메시지 박스 추가 프로세서 호출 이벤트
        /// 유저 사용 금지
        /// </summary>
        event Action<CommonMsgProgressType, CommonMessageBoxItem?> OnMessageProgressAsync;
    }

    /// <summary>
    /// 메시지 박스 관련 서비스 인터페이스
    /// </summary>
    public interface ICommonMessageBoxService
    {

        /// <summary>
        /// 메시지 박스 팝업창 열기
        /// </summary>
        /// <param name="message"></param>
        /// <param name="closeTime"></param>
        /// <param name="buttons"></param>
        /// <param name="type"></param>
        /// <param name="CloseCallBack"></param>
        void Show(string message, int closeTime = 0, CommonMsgButtons buttons = CommonMsgButtons.OK, CommonMsgBoxIcon type = CommonMsgBoxIcon.Info, Func<CommonMsgResult, Task>? CloseCallBack = null, string Header = CommonMessage.ALRIM);
        /// <summary>
        /// 팝업창 전체 닫기
        /// </summary>
        public void Clear();
    }

    /// <summary>
    /// 메시지 박스 관련 서비스 클래스
    /// </summary>
    public class CommonMessageBoxService : ICommonMessageBoxService, ICommonMessageBoxSystem
    {
        /// <summary>
        /// 메시지 박스 Razor 페이지에서 사용
        /// 메시지 박스 추가 프로세서 호출 이벤트
        /// 유저 사용 금지
        /// </summary>
        public event Action<CommonMsgProgressType, CommonMessageBoxItem?>? OnMessageProgressAsync;

        /// <summary>
        /// 메시지 박스 팝업창 열기
        /// </summary>
        /// <param name="message"></param>
        /// <param name="closeTime"></param>
        /// <param name="buttons"></param>
        /// <param name="type"></param>
        /// <param name="CloseCallBack"></param>
        public void Show(string message, 
            int closeTime = 0,
            CommonMsgButtons buttons = CommonMsgButtons.OK,
            CommonMsgBoxIcon type = CommonMsgBoxIcon.Info,
            //EventCallback<MsgResult> CloseCallBack = default,
            Func<CommonMsgResult, Task>? CloseCallBack = null,
            string Header = CommonMessage.ERROR)
        {
            var msg = new CommonMessageBoxItem();
            msg.Message = message;
            msg.CloseTime = closeTime;
            msg.Type = type;
            msg.MsgButtons = buttons;
            msg.CloseCallBack = CloseCallBack == null ? default : EventCallback.Factory.Create(this, CloseCallBack);
            msg.Header = Header;
            //msg.CloseCallBack = CloseCallBack;
            OnMessageProgressAsync?.Invoke(CommonMsgProgressType.Add, msg);
        }
        /// <summary>
        /// 팝업창 전체 닫기
        /// </summary>
        public void Clear()
        {
            OnMessageProgressAsync?.Invoke(CommonMsgProgressType.AllRemove, null);
        }


    }
}
