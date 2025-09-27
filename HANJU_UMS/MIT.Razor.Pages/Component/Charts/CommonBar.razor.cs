using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using System.Data;

namespace MIT.Razor.Pages.Component.Charts
{
    public class CommonBarBase : CommonUIComponentBase
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
        /// 차트에 적용될 데이터 테이블
        /// </summary>
        protected DataTable? _datatable;

        /// <summary>
        /// 차트 데이터 셋팅
        /// </summary>
        public DataTable? DataSource
        {
            get { return _datatable; }
            set
            {
                _datatable = value;
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
