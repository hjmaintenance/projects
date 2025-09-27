using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Common
{
    /// <summary>
    /// 팝업 닫은 상태 결과 타입
    /// </summary>
    public enum PopupResultType
    { 
        OK,
        Close,
        ETC
    }

    /// <summary>
    /// 공통 팝업이 닫힐때 메인화면으로 결과 파라미터값 전달하는 클래스
    /// </summary>
    public class PopupResult
    {
        /// <summary>
        /// 결과 파라미터 변수
        /// </summary>
        private object? _parameter;
        public object? Parameter
        {
            get => _parameter;
            private set => SetParameter(value);
        }

        /// <summary>
        /// 팝업 닫기 결과 타입 / 셋팅
        /// </summary>
        public PopupResultType PopupResultType { get; set; } = PopupResultType.Close;
        /// <summary>
        /// 팝업 닫기 결과 파라미터 셋팅
        /// </summary>
        /// <param name="parameter"></param>
        public void SetParameter(object? parameter) => _parameter = parameter;
    }
}
