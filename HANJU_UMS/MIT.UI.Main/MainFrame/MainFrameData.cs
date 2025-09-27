using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.Main.MainFrame
{
    /// <summary>
    /// 메인메뉴에서 선택한 프로그램 정보 셋팅 클래스
    /// </summary>
    public class MainFrameData
    {
        /// <summary>
        /// 메인메뉴에서 선택한 프로그램 화면의 Razor Page 클래스 타입 셋팅
        /// </summary>
        public Type? ClassType { get; set; }
        /// <summary>
        /// 메인메뉴에서 선택한 프로그램 화면의 프로그램 프로젝트 Assembly명
        /// 예) MIT.UI.COM
        /// </summary>
        public string? PGM_PATH { get; set; }
        /// <summary>
        /// 메인메뉴에서 선택한 프로그램 화면의 프로그램 프로젝트 Assembly 포함 전체 경로 및 클래스 이름
        /// 예) MIT.UI.COM.COM00101
        /// </summary>
        public string? PGM_CLASS { get; set; }
        /// <summary>
        /// 메인메뉴에서 선택한 프로그램 화면의 프로그램 아이디
        /// </summary>
        public string? PGM_ID { get; set; }
        /// <summary>
        /// 메인메뉴에서 선택한 메인메뉴 아이디
        /// </summary>
        public string? MENU_ID { get; set; }
        /// <summary>
        /// 메인메뉴에서 선택한 메인메뉴명
        /// </summary>
        public string? MENU_NAME { get; set; }

        public RenderFragment? PagRF { get; set; }
  }

    public class MainFrameDataButtonData : MainFrameData
    {
        /// <summary>
        /// 메인UI 화면에서 공통 버튼에 대한 버튼 정보 및 버튼 클릭시 이벤트 호출
        /// </summary>
        public MainCommonButtonData MainCommonButtonData { get; set; } = new MainCommonButtonData();

        public MainFrameDataButtonData(MainFrameData mainFrameData, MainCommonButtonData buttonData) 
        { 
            this.ClassType = mainFrameData.ClassType;
            this.PGM_PATH = mainFrameData.PGM_PATH;
            this.PGM_CLASS = mainFrameData.PGM_CLASS;
            this.PGM_ID = mainFrameData.PGM_ID;
            this.MENU_ID = mainFrameData.MENU_ID;
            this.MENU_NAME = mainFrameData.MENU_NAME;
            this.MainCommonButtonData = buttonData;
        }
    }




}
