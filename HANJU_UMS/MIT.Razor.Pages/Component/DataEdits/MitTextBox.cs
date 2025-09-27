using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits {
  public class MitTextBox : DxTextBox {

    public MitTextBox() {

      //ComboBoxBaseKeyDownEventArgs

    }


    protected override void OnInitialized() {
      base.OnInitialized();
      //BindValueMode = BindValueMode.OnInput;
    }



  }
}