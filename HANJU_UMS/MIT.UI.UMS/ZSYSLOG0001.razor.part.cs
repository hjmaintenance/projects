    
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-10
* 최종수정 : 25-02-10
* 프로시저 : p_log_search
*/ 
namespace MIT.UI.UMS
{
    public partial class ZSYSLOG0001Base : CommonUIComponentBase
    {

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await btn_Search();
        }

        protected override async Task Btn_Common_Save_Click()
        {
            await btn_Save();
        }

        protected override async Task Btn_Common_Delete_Click()
        {
            await btn_Delete();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 기능 ]

        protected async Task btn_Search()
        {
            await Search();
        }

        protected async Task btn_Save()
        {
            if(Grd1 == null || Grd1.Grid == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            if (!Grd1.IsCheckedNeedColumns())
            {
                MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
                return;
            }

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
        }

        protected async Task SaveCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Save())
            {
                MessageBoxService?.Show("저장하였습니다.");
                await btn_Search();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        protected async Task btn_Delete()
        {
            if (Grd1 == null || Grd1.Grid == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
        }

        protected async Task DeleteCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Delete())
            {
                MessageBoxService?.Show("삭제하였습니다.");
                await btn_Search();
            }
            else
            {
                MessageBoxService?.Show("삭제에 실패하였습니다.");
            }
        }


        protected async Task btn_Grd1_Row_Add()
        {
            if (Grd1 == null || Grd1.Grid == null)
                return;

            Grd1.AddNewRow();

            await Grd1.PostEditorAsync();
        }

    #endregion [ 사용자 버튼 기능 ]

    #region [ 사용자 이벤트 함수 ]

    #endregion [ 사용자 이벤트 함수 ]

    #region [ 사용자 정의 메소드 ]



    public bool IsReadOnly { get; set; } = true;


    private async Task Search()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                
                var datasource = await DBSearch();


        Grd1.DataSource = datasource;


        await Grd1.PostEditorAsync();
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                CloseLoadingPanel();
            }


      StateHasChanged();
    }

        private async Task<bool> Save()
        {
            try
            {
                ShowLoadingPanel();
                
                var checkedRows = Grd1?.GetCheckedRows();
                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBSave(checkedRows.CopyToDataTable());

                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> Delete()
        {
            try
            {
                ShowLoadingPanel();

                var checkedRows = Grd1?.GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBDelete(checkedRows.CopyToDataTable());

                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("p_log_search", new Dictionary<string, object?>()
                {
                    {"SRCH", SRCH.ToStringTrim() },
                    {"IsSuccess", IsSuccess.ToStringTrim() },
                    {"Danger",                     Danger.ToStringTrim() },
                    {"s_dt", s_dt.EditValue },
                    {"E_DT", E_DT.EditValue },


                });

            ChartDt1 = datatable;
            return datatable;
        }

        private async Task DBSave(DataTable datatable)
        {
            if (QueryService == null)
                return;

            await QueryService.ExecuteNonQuery("p_log_search_SAVE", datatable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBDelete(DataTable datatable)
        {
            if (QueryService == null)
                return;
            
            await QueryService.ExecuteNonQuery("p_log_search_DELETE", datatable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
