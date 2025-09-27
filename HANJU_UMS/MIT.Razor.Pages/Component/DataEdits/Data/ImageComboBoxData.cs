using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits.Data
{
    /// <summary>
    /// 이미지 콤보에 Datatable을 ObservableCollection로 변환 하기 위한 정보 클래스
    /// </summary>
    public class ImageComboBoxData
    {
        /// <summary>
        /// 고유 아이디 부여
        /// </summary>
        public string? GUID { get; set; } = Guid.NewGuid().ToString();
        //public string? CHK { get; set; } = "N";
        //public string? ImagePath { get; set; }
        /// <summary>
        /// Datatable 에서 Value 멤버에 대한 Value데이터 값 셋팅
        /// </summary>
        public object? Value { get; set; }
        /// <summary>
        /// 해당 데이터 리스트에서 Datatable의 DataRow에 해당 하는 값 셋팅
        /// </summary>
        public DataRow? Row { get; set; }
    }
}
