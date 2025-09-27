using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using System.Data;

namespace MIT.Razor.Pages.Component.Charts
{
    public class MitChartLineBase : CommonUIComponentBase
    {
        /// <summary>
        /// 커스텀 툴팁
        /// </summary>
        [Parameter]
        public RenderFragment? ToolTipTemplate { get; set; }

        /// <summary>
        /// 커스텀 범례
        /// </summary>
        [Parameter]
        public RenderFragment? LegendTemplate { get; set; }

        /// <summary>
        /// 차트 클래스 정보
        /// </summary>
        public DxChart? Chart { get; protected set; }

        /// <summary>
        /// 차트 제목
        /// </summary>
        [Parameter]
        public string ChartTitle { get; set; } = string.Empty;

        /// <summary>
        /// 차트 서브 제목
        /// </summary>
        [Parameter]
        public string ChartSubTitle { get; set; } = string.Empty;

        /// <summary>
        /// 차트 가로 사이즈 조절 자동으로 선택
        /// </summary>
        [Parameter]
        public bool IsLayoutAuto { get; set; } = true;
        /// <summary>
        /// 차트 CSS 스타일 
        /// </summary>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// 차트 너비
        /// </summary>
        [Parameter]
        public string WidthStr { get; set; } = "100%";

        /// <summary>
        /// 차트 높이
        /// </summary>
        [Parameter]
        public string HeightStr { get; set; } = string.Empty;

        /// <summary>
        /// 툴팁 보이기
        /// </summary>
        [Parameter]
        public bool IsShowToolTip { get; set; } = false;

        /// <summary>
        /// 범례 보이기
        /// </summary>
        [Parameter]
        public bool IsShowLegend { get; set; } = false;
        /// <summary>
        /// 포인트 라벨 보이기
        /// </summary>
        [Parameter]
        public bool IsShowLineSeries { get; set; } = false;
        /// <summary>
        /// x축 라벨 보이기
        /// </summary>
        [Parameter]
        public bool IsShowArgumentAxisLabel { get; set; } = true;
        /// <summary>
        /// 스크롤 보이기
        /// </summary>
        [Parameter]
        public bool IsScrollVisible { get; set; } = true;


    [Parameter]
    public string XName { get; set; } = "";

    [Parameter]
    public string YName { get; set; } = "";

    [Parameter]
    public string YNames { get; set; } = "";

    [Parameter]
    public string YTitle { get; set; } = "";


    [Parameter]
    public string YTitles { get; set; } = "";




    /// <summary>
    /// 차트에 적용될 데이터 테이블
    /// </summary>
    protected DataTable? _datatable = new DataTable();

        /// <summary>
        /// 차트 데이터 셋팅
        /// </summary>
        [Parameter]
        public DataTable? DataSource
        {
            get { return _datatable; }
            set
            {
                if (value != null) _datatable = value;
                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            StateHasChanged();
        }

    }
}
