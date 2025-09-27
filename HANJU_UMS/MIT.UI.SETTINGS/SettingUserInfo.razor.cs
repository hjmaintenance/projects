using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.SETTINGS
{
    public class SettingUserInfoBase : CommonUIComponentBase
    {
        protected string? OfficeNum { get; set; }
        protected string? Phone { get; set; }
        protected string? Address1 { get; set; }
        protected string? Address2 { get; set; }

        protected string? Password { get; set; }
        protected string? NewPassword { get; set; }
        protected string? ConfirmPassword { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await Search();
            }
        }


        protected void OnUserInfoChanged()
        {
            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveUserInfoChangedCallback);
        }

        protected async Task SaveUserInfoChangedCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await SaveUserInfoChanged())
            {
                MessageBoxService?.Show("저장하였습니다.");
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        protected void OnUserPasswordChanged()
        {
            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveUserPasswordChangedCallback);
        }

        protected async Task SaveUserPasswordChangedCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await SaveUserPasswordChanged())
            {
                MessageBoxService?.Show("저장하였습니다.");
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        #region [ 사용자 정의 메소드]

        private async Task Search()
        {
            try
            {
                ShowLoadingPanel();
                var datatable = await DBSearch();

                if (datatable == null || datatable.Rows.Count == 0)
                    return;

                var data = datatable.Rows[0];

                OfficeNum = data["OFFICE_NUM"].ToStringTrim();
                Phone = data["PHONE_NUM"].ToStringTrim();
                Address1 = data["ADDRESS_1"].ToStringTrim();
                Address2 = data["ADDRESS_2"].ToStringTrim();

                StateHasChanged();
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

        protected async Task<bool> SaveUserInfoChanged()
        {
            try
            {
                ShowLoadingPanel();

                await DBSaveUserInfoChanged();

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

        protected async Task<bool> SaveUserPasswordChanged()
        {
            try
            {
                ShowLoadingPanel();

                if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPassword))
                {
                    throw new Exception("신규 및 확인 비밀번호를 입력하세요.");
                }

                if (!NewPassword.Equals(ConfirmPassword))
                {
                    throw new Exception("신규와 확인 비밀번호가 같지않습니다.");
                }

                string result =  await DBSaveUserPasswordChanged();

                if (result.Equals("0"))
                    throw new Exception("기존 비밀번호가 다릅니다.");

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

        #endregion [ 사용자 정의 메소드]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_USER_INFO_SELECT", new Dictionary<string, object?>()
                {
                    { "USER_ID", USER_ID },
                });


            return datatable;
        }

        private async Task DBSaveUserInfoChanged()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_SETTING_USER_INFO_SML_SAVE", new Dictionary<string, object?>()
                {
                    { "USER_ID", USER_ID },
                    { "OFFICE_NUM", OfficeNum.ToStringTrim() },
                    { "PHONE_NUM", Phone.ToStringTrim() },
                    { "ADDRESS_1", Address1.ToStringTrim() },
                    { "ADDRESS_2", Address2.ToStringTrim() },
                    { "REG_ID", USER_ID },
                });
        }

        private async Task<string> DBSaveUserPasswordChanged()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_USER_PWD_SML_SAVE", new Dictionary<string, object?>()
                {
                    { "USER_ID", USER_ID },
                    { "PASSWORD", EncryptHelper.EncryptSHA512(Password.ToStringTrim()) },
                    { "PASSWORD_NEW", EncryptHelper.EncryptSHA512(NewPassword.ToStringTrim()) },
                    { "REG_ID", USER_ID },
                });

            return datatable == null ? string.Empty : datatable.Rows[0]["RESULT"].ToStringTrim();
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
