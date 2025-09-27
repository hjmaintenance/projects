using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.TreeView.Data
{
    /// <summary>
    /// 트리리스트에서 DataSource에 적용되는 데이터
    /// Datatable에서 넘겨받은 데이터를 가공하여 트리리스트에 적용하기 위한 클래스
    /// </summary>
    public class TreeViewNodeData
    {
        /// <summary>
        /// 노드 체크 상태
        /// </summary>
        public bool Checked { get; set; } = false;
        /// <summary>
        /// 노드 고유 부모 아이디
        /// 시스템에서 자동셋팅
        /// </summary>
        public string ParentGUID { get; set; } = string.Empty;
        /// <summary>
        /// 노드 고유 아이디
        /// </summary>
        public string GUID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 해당 노드에 대한 실제 DataTable의 DAtaRow 데이터
        /// </summary>
        public DataRow? Row { get; set; }


    public string TName { get; set; } = string.Empty;
    public string TValue { get; set; } = string.Empty;
    public string PValue { get; set; } = string.Empty;

    

    /// <summary>
    /// 현재 노드의 자식 노드 정보
    /// </summary>
    public ObservableCollection<TreeViewNodeData> Child = new ObservableCollection<TreeViewNodeData>();
    }
}
