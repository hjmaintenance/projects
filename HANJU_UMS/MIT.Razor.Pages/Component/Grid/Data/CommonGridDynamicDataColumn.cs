using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Component.DataEdits.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Grid.Data
{
    /// <summary>
    /// CommonGridDynamic에 셋팅할 DataColumn 셋팅 데이터 클래스
    /// </summary>
    public class CommonGridDynamicDataColumnAttribute  
    {
        /// <summary>
        /// 필드 멤버 셋팅
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// Header Caption 셋팅
        /// </summary>
        public string? Caption { get; set; }
        /// <summary>
        /// 컬럼 크기 셋팅
        /// </summary>
        public int Width { get; set; } = 0;
        /// <summary>
        /// DisplayFormat 셋팅
        /// </summary>
        public string? DisplayFormat { get; set; }

        /// <summary>
        /// 정렬 가능 유무 셋팅
        /// </summary>
        public bool AllowSort { get; set; } = true;
        /// <summary>
        /// 보이기/숨기기 셋팅
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 컬럼 고정 타입 셋팅
        /// </summary>
        public GridColumnFixedPosition GridColumnFixedPosition { get; set; } = GridColumnFixedPosition.None;
        /// <summary>
        /// Row 데이터 정렬 방햘 셋팅
        /// </summary>
        public GridTextAlignment TextAlignment { get; set; } = GridTextAlignment.Center;
        /// <summary>
        /// Header Caption 정렬 방햘 셋팅
        /// </summary>
        public GridTextAlignment CaptionAlignment { get; set; } = GridTextAlignment.Center;

    }

    /// <summary>
    /// CommonGrid DataColumn에 셋팅될 Width Type
    /// </summary>
    public enum WidthType
    {
        Px = 0,
        Rate = 1
    }

}
