using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits
{
    public class CommonCheckBoxBase : CommonUIComponentBase
    {
        [Parameter]
        public string ValueChecked { get; set; } = "Y";
        [Parameter]
        public string ValueUnchecked { get; set; } = "N";
        [Parameter]
        public CheckType CheckType { get; set; } = CheckType.Checkbox;

        [Parameter]
        public bool Enabled
        {
            get { return _enable; }
            set { _enable = value; StateHasChanged(); }
        }
        [Parameter]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; StateHasChanged(); }
        }

        [Parameter]
        public string? EditValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                StateHasChanged();
            }
        }

        [Parameter]
        public EventCallback<string> CheckedChanged { get; set; }

        protected string? _value;

        protected bool _enable = true;
        protected bool _readOnly = false;

        protected void OnCheckedChanged(string value)
        {
            _value = value;

            CheckedChanged.InvokeAsync(_value);
            StateHasChanged();
        }
    }
}
