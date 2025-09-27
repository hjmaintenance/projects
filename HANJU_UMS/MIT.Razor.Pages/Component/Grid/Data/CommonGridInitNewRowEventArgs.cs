using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Grid.Data
{
    /// <summary>
    /// 그리드 새로운 Row 추가 전에 이벤트 호출시 전달하는 데이터 클래스
    /// </summary>
    public class CommonGridInitNewRowEventArgs
    {
        /// <summary>
        /// 새로운 로우 정보 셋팅
        /// </summary>
        public DataRow? Row { get; set; }
        /// <summary>
        /// 새로운 로우 추가 취소
        /// </summary>
        public bool Cancel { get; set; } = false;

    }
}
