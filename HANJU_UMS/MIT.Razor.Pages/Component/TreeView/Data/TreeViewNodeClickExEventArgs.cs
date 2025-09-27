using DevExpress.Blazor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.TreeView.Data
{
    /// <summary>
    /// 트리에서 노드 클릭 이벤트 호출시 전달하는 데이터 클래스
    /// </summary>
    public class TreeViewNodeClickExEventArgs
    {
        /// <summary>
        /// 트리뷰에서 넘겨 받은 원본 전달 이벤트 클래스
        /// </summary>
        public TreeViewNodeClickEventArgs? e { get; set; }
        /// <summary>
        /// 노트 클릭시 DataTable에 해당하는 로우 정보
        /// </summary>
        public DataRow? itemRow { get; set; }
    }
}
