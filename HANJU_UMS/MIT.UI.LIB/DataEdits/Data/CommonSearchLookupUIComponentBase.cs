using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits.Data;
using MIT.Razor.Pages.Component.DataEdits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DevExpress.Data.Filtering.Helpers;

namespace MIT.UI.LIB.DataEdits.Data
{
    public abstract class CommonSearchLookupUIComponentBase<Titem> : CommonUIComponentBase where Titem : ImageComboBoxData
    {
        [Parameter]
        public bool IsShowEmptyRow
        {
            get { return _isShowEmptyRow; }
            set
            {
                _isShowEmptyRow = value;
                StateHasChanged();
            }
        }
        [Parameter]
        public string EmptyRowName
        {
            get { return _emptyRowName; }
            set
            {
                _emptyRowName = value;
                StateHasChanged();
            }
        }
        [Parameter]
        public bool IsFirstRowSelected
        {
            get { return _isFirstRowSelected; }
            set
            {
                _isFirstRowSelected = value;
                StateHasChanged();
            }
        }


        protected CommonSearchLookup<Titem>? ComboBox;

        public ImageComboBoxData? SelectedItem
        {
            get { return ComboBox?.SelectedItem; }
        }

        public object? EditValue
        {
            get
            {
                return ComboBox?.EditValue;
            }
            set
            {
                if (ComboBox != null)
                    ComboBox.EditValue = value;
                StateHasChanged();
            }
        }

        private bool _isShowEmptyRow = true;
        private string _emptyRowName = "전체";
        private bool _isFirstRowSelected = true;

        public abstract Task LoadData();

        protected virtual void OnValueChanged(ImageComboBoxData? item)
        {
            if (ComboBox != null)
                ComboBox.EditValue = item?.Value;
            StateHasChanged();
        }
    }
}
