using DevExpress.Blazor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Common;
public class AppData {

  private SizeMode _sizemode = SizeMode.Small;
  public SizeMode Sizemode {
    get { return _sizemode; }
    set {
      _sizemode = value;
      NotifyDataChanged();
    }
  }

  public event Action OnChange;

  // 변경 될 때 마다 자식 컴포넌트에 전파
  private void NotifyDataChanged() => OnChange?.Invoke();


  public IDictionary<string, DataTable> GlobalDic { get; set; } = new Dictionary<string, DataTable>();

  public int ActiveTabIndex { get; set; } = 0;
  public string ActiveTabMenuID { get; set; }




}