using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.MessageBox
{
    /// <summary>
    /// 메시지 박스 팝업 닫힐때 결과 타입
    /// </summary>
    public enum CommonMsgResult
    {
        NONE,
        OK,
        Cancel,
        Yes,
        No
    }

    /// <summary>
    /// 메시지 박스 버튼 타입
    /// </summary>
    public enum CommonMsgButtons
    {
        OK,
        OKCancel,
        YesNo
    }

    /// <summary>
    /// 메시지 박스 아이콘 타입
    /// </summary>
    public enum CommonMsgBoxIcon
    {
        Success,
        Info,
        Warning,
        Error,
    }

    /// <summary>
    /// 메시지 박스 팝업창 추가/삭제/전체삭제 선택 타입
    /// </summary>
    public enum CommonMsgProgressType
    {
        Add,
        Remove,
        AllRemove
    }

    /// <summary>
    /// 메시지 박스 추가시 정보 데이터 셋팅 클래스
    /// </summary>
    public class CommonMessageBoxItem
    {
        
        /// <summary>
        /// 아이콘 타입
        /// </summary>
        public CommonMsgBoxIcon Type { get; set; } = CommonMsgBoxIcon.Info;
        /// <summary>
        /// 버튼 타입
        /// </summary>
        public CommonMsgButtons MsgButtons { get; set; } = CommonMsgButtons.OK;
        /// <summary>
        /// 메시지 박스에 출력할 메시지
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// 메시지 박스 헤더
        /// </summary>
        public string Header { get; set; } = CommonMessage.ALRIM;
        /// <summary>
        /// 메시지 박스에 자동 닫힘 시간 설정
        /// 1000 = 1초
        /// </summary>
        public int CloseTime { get; set; } = 0;
        /// <summary>
        /// 메시지 박스 팝업창 닫힐때 Callback 호출 이벤트
        /// </summary>
        public EventCallback<CommonMsgResult> CloseCallBack { get; set; }
    }
}
