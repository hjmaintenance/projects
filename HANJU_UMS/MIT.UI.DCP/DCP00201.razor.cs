using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.DataEdits.Data;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.Grid.RepositoryItem;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.UI.LIB.DataEdits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.DCP
{
    public class DCP00201Base : CommonUIComponentBase
    {
        protected CommonCustSearchLookup? cbo_CUST_CODE { get; set; }
        protected MsgIDImageComboBox? cbo_MSG_ID { get; set; }

        protected CommonDateEdit? dte_FR_DT { get; set; }
        protected CommonDateEdit? dte_TO_DT { get; set; }

        protected CommonGrid? Grd1 { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await InitControls();

                await Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        protected async Task InitControls()
        {
            if (cbo_CUST_CODE != null)
            {
                await cbo_CUST_CODE.LoadData();
                cbo_CUST_CODE.EditValue = this.UserData?.CUST_CODE;
            }
                
            if (cbo_MSG_ID != null)
                await cbo_MSG_ID.LoadData();

            await InvokeAsync(StateHasChanged);
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await Search();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [사용자 정의 버튼 ]



        #endregion [사용자 정의 버튼 ]


        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                var colInfoDT = await DBSearchColumnInfo();

                Grd1.Columns.Clear();

                if (colInfoDT != null)
                {
                    foreach (DataRow row in colInfoDT.Rows)
                    {
                        var column = new CommonGridDataColumnAttribute();
                        column.FieldName = row["COL_NAME"].ToStringTrim();
                        column.Caption = row["COL_DP_NAME"].ToStringTrim();
                        column.RepositoryItemAttribute.ObjectType = typeof(RepositoryItem);

                        Grd1.Columns.Add(column);
                    }
                }

                var datatable = await DBSearch();

                Grd1.DataSource = datatable;

                await Grd1.PostEditorAsync();

                Grd1.Grid?.AutoFitColumnWidths();
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
        }

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00201_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "FR_DT", dte_FR_DT?.EditValue },
                    { "TO_DT", dte_TO_DT?.EditValue },
                    { "CUST_CODE", cbo_CUST_CODE?.EditValue },
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                });
            return datatable;
        }
        
        private async Task<DataTable?> DBSearchColumnInfo()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00201_GRD1_COL_INFO_SELECT", new Dictionary<string, object?>()
                {
                    
                    { "CUST_CODE", cbo_CUST_CODE?.EditValue },
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                });
            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

