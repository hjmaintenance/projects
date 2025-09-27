using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExpress.Data.ODataLinq;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.Grid.RepositoryItem;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.UI.LIB.DataEdits;
using MIT.UI.LIB.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.COM
{
    public class COM00101Base : CommonUIComponentBase
    {
        protected CommonTextBox? Txt_GRP_CODE { get; set; }
        protected CommonTextBox? Txt_GRP_NAME { get; set; }
        protected UseYNImageComboBox? Cbo_USE_YN { get; set; }

        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await InitControls();
                
                await Btn_Common_Search_Click();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        protected async Task InitControls()
        {
            // 필수 입력 그리드 컬럼 셋팅
            Grd1?.SetNeedColumns(new string[] {
                "CODE", "CODE_NAME"
            });

            // 콤보 박스 데이터 로드
            Cbo_USE_YN?.LoadData();

            // Render
            await InvokeAsync(StateHasChanged);
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        /// <summary>
        /// 공통 조회 버튼 이벤트
        /// </summary>
        /// <returns></returns>
        protected override async Task Btn_Common_Search_Click()
        {
            if (Grd1 != null)
                await Grd1.PerformSearchButtonClick();

            if (Grd2 != null)
                await Grd2.PerformSearchButtonClick();
        }

        /// <summary>
        /// 공통 저장 버튼 이벤트
        /// </summary>
        /// <returns></returns>
        protected override async Task Btn_Common_Save_Click()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            if (!Grd1.IsCheckedRows() && !Grd2.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            if (!Grd1.IsCheckedNeedColumns() && !Grd2.IsCheckedNeedColumns())
            {
                MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
                return;
            }

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
        }

        /// <summary>
        /// 저장 메시지 박스가 닫힐때 Close 이벤트 처리
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected async Task SaveCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            bool isSuccess = true;

            if (Grd1 != null)
                isSuccess = await Grd1.PerformSaveButtonClick();

            if (Grd2 != null)
                isSuccess = await Grd2.PerformSaveButtonClick();

            if (isSuccess)
            {
                MessageBoxService?.Show("저장하였습니다.");
                await Btn_Common_Search_Click();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        /// <summary>
        /// 공통 삭제 버튼 이벤트
        /// </summary>
        /// <returns></returns>
        protected override async Task Btn_Common_Delete_Click()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            if (!Grd1.IsCheckedRows() && !Grd2.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
        }

        /// <summary>
        /// 삭제 메시지 박스가 닫힐때 Close 이벤트 처리
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected async Task DeleteCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            bool isSuccess = true;

            if (Grd1 != null)
                isSuccess = await Grd1.PerformDeleteButtonClick();

            if (Grd2 != null)
                isSuccess = await Grd2.PerformDeleteButtonClick();

            if (isSuccess)
            {
                MessageBoxService?.Show("삭제하였습니다.");
                await Btn_Common_Search_Click();
            }
            else
            {
                MessageBoxService?.Show("삭제에 실패하였습니다.");
            }
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 이벤트 ]

        #endregion [ 사용자 버튼 이벤트 ]

        #region [ 사용자 이벤트 함수 ]

        /// <summary>
        /// 그리드1 조회 전에 조회에 필요한 DB 파라메터 값 셋팅
        /// </summary>
        /// <param name="parameters"></param>
        protected void Grd1_OnInputSearchParameter(Dictionary<string, object?> parameters)
        {
            parameters.Add("CODE", Txt_GRP_CODE?.Text);
            parameters.Add("CODE_NAME", Txt_GRP_NAME?.Text);
            parameters.Add("USE_YN", Cbo_USE_YN?.EditValue);
        }

        /// <summary>
        /// 그리드2 조회 전에 조회에 필요한 DB 파라메터 값 셋팅
        /// </summary>
        /// <param name="parameters"></param>
        protected void Grd2_OnInputSearchParameter(Dictionary<string, object?> parameters)
        {
            var CODE = Grd1?.GetFocusedRowCellValue("CODE").ToStringTrim();

            parameters.Add("CODE", CODE);
        }

        /// <summary>
        /// 그리드에 새로운 행 추가전에 값 셋팅 추가 또는 새로운행 추가 취소
        /// 취소는 Cancel 에 true 값 셋팅
        /// </summary>
        /// <param name="e"></param>
        protected void Grd1_InitNewRowChanging(CommonGridInitNewRowEventArgs e)
        {
            if (e.Row == null || Grd1 == null)
            {
                e.Cancel = true;
                return;
            }

            e.Row["USE_YN"] = "Y";
        }

        /// <summary>
        /// 그리드에 새로운 행 추가전에 값 셋팅 추가 또는 새로운행 추가 취소
        /// 취소는 Cancel 에 true 값 셋팅
        /// </summary>
        /// <param name="e"></param>
        protected void Grd2_InitNewRowChanging(CommonGridInitNewRowEventArgs e)
        {
            var CODE = Grd1?.GetFocusedRowCellValue("CODE");
            var SAVE_YN = Grd1?.GetFocusedRowCellValue("SAVE_YN");

            if (e.Row == null || Grd1 == null || string.IsNullOrEmpty(CODE.ToStringTrim()) || SAVE_YN.ToStringTrim().Equals("N"))
            {
                e.Cancel = true;
                return;
            }

            e.Row["PARENT_CODE"] = CODE;
            e.Row["USE_YN"] = "Y";
        }

        /// <summary>
        /// 그리드 클릭시 포커스 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task Grd1_FocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (Grd2 != null)
                await Grd2.PerformSearchButtonClick();
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드]

        #endregion [ 사용자 정의 메소드]

        #region [ 데이터 정의 메소드 ]

        #endregion [ 데이터 정의 메소드 ]
    }
}

