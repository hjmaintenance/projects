using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Component.DataEdits.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Grid.Data
{
    /// <summary>
    /// CommonGrid DataColumn에 셋팅될 RepositoryItem Type
    /// </summary>
    public enum RepositoryItemType
    {
        None = 0,
        TextBox,
        SpinEdit,
        ImageComboBox,
        DateEdit,
        CheckBox,

    }

    /// <summary>
    /// CommonGrid에 셋팅할 DataColumn 셋팅 데이터 클래스
    /// </summary>
    public class CommonGridDataColumnAttribute
    {
        /// <summary>ㅇ
        /// DataColumn에 Value 필드이름 셋팅
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// 그리드 DataColumn에 보여질 Caption 셋팅
        /// </summary>
        public string? Caption { get; set; }
        /// <summary>
        /// 그리드 DataColumn 크기 셋팅
        /// </summary>
        public int Width { get; set; } = 100;
        /// <summary>
        /// 그리드 DataColumn에 보여질 DisplayFormat 셋팅
        /// </summary>
        public string? DisplayFormat { get; set; } = string.Empty;

        /// <summary>
        /// 그리드 DataColumn에 대한 Sort 기능 유무
        /// </summary>
        public bool AllowSort { get; set; } = true;
        /// <summary>
        /// 그리드 DataColumn에 대한 컬럼 보이기/숨기기 
        /// </summary>
        public bool Visible { get; set; } = true;
        /// <summary>
        /// 그리드 DataColumn에 대한 고정 컬럼 셋팅
        /// </summary>
        public GridColumnFixedPosition GridColumnFixedPosition { get; set; } = GridColumnFixedPosition.None;
        /// <summary>
        /// 그리드 DataColumn에 대한 Row 정렬 셋팅
        /// </summary>
        public GridTextAlignment TextAlignment { get; set; } = GridTextAlignment.Center;
        /// <summary>
        /// 그리드 DataColumn에 대한 Header 정렬 셋팅
        /// </summary>
        public GridTextAlignment CaptionAlignment { get; set; } = GridTextAlignment.Center;

        /// <summary>
        /// RepositoryItem 정보 셋팅
        /// </summary>
        public CommonGridRepositoryItemAttribute RepositoryItemAttribute { get; init; } = new CommonGridRepositoryItemAttribute();
    }

    /// <summary>
    /// RepositoryItem 정보 클래스
    /// </summary>
    public class CommonGridRepositoryItemAttribute
    {
        /// <summary>
        /// PrimaryKey 설정 여부
        /// PrimaryKey 설정시 SAVE_YN에서 'N'인 데이터는 편집 가능하고 'Y'인데이터는 편집 불가능
        /// </summary>
        public bool IsPrimaryKey { get; set; } = false;
        /// <summary>
        /// 편집 가능 유무
        /// </summary>
        public bool AllowEdit { get; set; } = true;
        /// <summary>
        /// 읽기 전용 설정
        /// </summary>
        public bool ReadOnly { get; set; } = false;
        /// <summary>
        /// 편집 가능 유무 (Editor모양 유지)
        /// </summary>
        public bool Enable { get; set; } = true;
        /// <summary>
        /// 이외 파라메터 값 추가 셋팅
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// 적용할 RepositoryItem에 type 셋팅
        /// 예) ObjectType = typeof(RepositoryITemCheckBox)
        /// </summary>
        public Type? ObjectType { get; set; }
        /// <summary>
        /// 적용할 ReporitoryItem의 기본 계열 타입
        /// </summary>
        public RepositoryItemType RepositoryItemType { get; set; } = RepositoryItemType.None;

        /// <summary>
        /// RepositoryItemTextBox 계열에 대한 추가 셋팅
        /// </summary>
        public CommonGridRepositoryItemTextBoxAttribute TextBoxAttribute { get; set; } = new CommonGridRepositoryItemTextBoxAttribute();
        /// <summary>
        /// RepositoryItemComboBox 계열에 대한 추가 셋팅
        /// </summary>
        public CommonGridRepositoryItemImageComboBoxAttribute ComboBoxAttribute { get; init; } = new CommonGridRepositoryItemImageComboBoxAttribute();
        /// <summary>
        /// RepositoryItemCheckBox 계열에 대한 추가 셋팅
        /// </summary>
        public CommonGridRepositoryItemCheckBoxAttribute CheckBoxAttribute { get; init; } = new CommonGridRepositoryItemCheckBoxAttribute();
        /// <summary>
        /// RepositoryItemDateEdit 계열에 대한 추가 셋팅
        /// </summary>
        public CommonGridRepositoryItemDateEditAttribute DateEditAttribute { get; init; } = new CommonGridRepositoryItemDateEditAttribute();
    }

    /// <summary>
    /// RepositoryItemTextBox 계열에 대한 추가 셋팅 클래스
    /// </summary>
    public class CommonGridRepositoryItemTextBoxAttribute
    {
        /// <summary>
        /// 패스워드 유무 설정
        /// </summary>
        public bool IsPassword { get; set; } = false;
    }

    /// <summary>
    /// RepositoryItemComboBox 계열에 대한 추가 셋팅 클래스
    /// </summary>
    public class CommonGridRepositoryItemImageComboBoxAttribute
    {
        /// <summary>
        /// DisplayFieldName 멤버 설정
        /// </summary>
        public string? DisplayFieldName { get; set; }
        /// <summary>
        /// ValueFieldName 멤버 설정
        /// </summary>
        public string? ValueFieldName { get; set; }
        /// <summary>
        /// ImageFieldName 멤버 설정
        /// </summary>
        public string? ImageFieldName { get; set; }
        /// <summary>
        /// 첫번째 로우에 빈값 추가 유무
        /// </summary>
        public bool IsShowEmptyRow { get; set; } = false;
        /// <summary>
        /// 빈값 명칭
        /// </summary>
        public string EmptyRowName { get; set; } = "전체";
        /// <summary>
        /// 값 변경시 호출 이벤트 셋팅
        /// </summary>
        public EventCallback<ImageComboBoxData> ValueChanged { get; set; }
        /// <summary>
        /// ComboBox Item에 적용할 데이터 테이블
        /// </summary>
        public DataTable? DataSource { get; set; }
    }

    /// <summary>
    /// RepositoryItemCheckBox 계열에 대한 추가 셋팅 클래스
    /// </summary>
    public class CommonGridRepositoryItemCheckBoxAttribute
    {
        /// <summary>
        /// 체크시 VALUE 값
        /// </summary>
        public string ValueChecked { get; set; } = "Y";
        /// <summary>
        /// 체크해제시 VALUE 값
        /// </summary>
        public string ValueUnchecked { get; set; } = "N";
        /// <summary>
        /// 체크 타입 설정
        /// </summary>
        public CheckType CheckType { get; set; } = CheckType.Checkbox;
    }

    /// <summary>
    /// RepositoryItemDateEdit 계열에 대한 추가 셋팅 클래스
    /// </summary>
    public class CommonGridRepositoryItemDateEditAttribute
    {
        /// <summary>
        /// DisplayFormat 설정
        /// 기본 포멧은 = yyyy-MM-dd
        /// </summary>
        public string DisplayFormat { get; set; } = "yyyy-MM-dd";
    }

}
