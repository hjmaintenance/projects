/*
* 작성자명 : 김지수
* 작성일자 : 25-02-21
* 최종수정 : 25-03-13
* 화면명 : 메인화면(한주 사용자)
* 프로시저 :P_HMI_MAIN_ENTIRE_SEARCH, P_HMI_MAIN_ELEC_SEARCH, P_HMI_MAIN_STM_SEARCH, 
*           P_HMI_MAIN_6BLR_SEARCH, P_HMI_MAIN_7BLR_SEARCH,P_HMI_MAIN_8BLR_SEARCH,
*           P_HMI_MAIN_9BLR_SEARCH, P_HMI_MAIN_10BLR_SEARCH, P_HMI_MAIN_CCPP_SEARCH,
*/
using DevExpress.Blazor;
using DevExpress.Blazor.Tabs.Internal;
using DevExpress.Pdf.ContentGeneration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Service;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MIT.UI.UMS
{
    public class Mlabel
    {
        public string id { get; set; } = "";
        public object data { get; set; } = "임시";
        public string xx { get; set; } = "0";
        public string yy { get; set; } = "0";
        public string color { get; set; } = "pink";
        public string name { get; set; } = "장비이름";
        public MitCard.CardUtilityType utility { get; set; } = MitCard.CardUtilityType.Power;
        public string unit { get; set; } = "QWER";
    }
    public class Mtab
    {
        public bool _isExpend  = false;
        public bool IsExpend
        {
            get {
                return _isExpend;
            }
            set
            {
                _isExpend = value;
                if (value)
                {
                    DBSearch.Invoke(this);
                }
            }
        }

        public string title = "";
        public string imgURL = "";
        public string sp_name = "";
        public Func<Mtab, Task<DataTable?>>? DBSearch { get; set; }
        public List<Mlabel> mlabelList = new List<Mlabel>();
    }
    public class UM1010MA1Base : CommonUIComponentBase, IDisposable
    {
        readonly string[] tabs = new string[] {
			@"
            전체공정
            |/images/haju_factory/main_n1.png
            |P_HMI_MAIN_ENTIRE_SEARCH
            |a02,316,16.8,red,수전량,kW,PW
            |a03,384,14.4,red,전기공급량,kW,PW
            |a04,334,65,red,발전량,kW,PW
            |a01,239.4,35.6,red,증기 생산량,T/H,ST
            |a05,375.2,89.6,red,증기 공급량,T/H,ST
            |a06,375.2,115,red,잉여증기 수급량,T/H,ST
            |a07,345.2,187.4,red,순수 공급량,T/D,WA
            |a08,145,194.4,red,여과수 공급량,T/D,WA
            ",
			"""
            전기
            |/images/haju_factory/main_n2.png
            |P_HMI_MAIN_ELEC_SEARCH
            |b01,130,19,red,한주T/L 수전량,kW,PW
            |b02,65.6,138.6,red,#1 GTG 발전량,kW,PW
            |b03,65.6,144.6,red,G1 발전량,kW,PW
            |b04,106.4,141,red,G2 발전량,kW,PW
            |b05,81,182.6,red,1공장 공급량,kW,PW
            |b06,270,19,red,석지T/L 수전량,kW,PW
            |b07,206.2,142.4,red,#2 GTG 발전량,kW,PW
            |b08,245.8,134,red,G3 발전량,kW,PW
            |b09,245.8,139.8,red,G4 발전량,kW,PW
            |b10,245.8,145.6,red,G5 발전량,kW,PW
            |b11,183,182.6,red,2공장 공급량,kW,PW
            |b12,226,182.6,red,3공장 공급량,kW,PW
            |b13,270,182.6,red,2-3공장 합계,kW,PW
            |b14,377,36,red,수전량,kW,PW
            |b15,377,48.2,red,발전량,kW,PW
            |b16,378,65,red,공급량,kW,PW
            """,
            """
            증기
            |/images/haju_factory/main_n3.png
            |P_HMI_MAIN_STM_SELECT01
            |c01,46,49,red,#1 HP F,T/H,ST
            |c02,65,49,red,#2 HP F,T/H,ST
            |c03,111,37.6,red,#6 BLR,T/H,ST
            |c04,155,37.4,red,#7 BLR,T/H,ST
            |c05,189.2,37.4,red,#8 BLR,T/H,ST
            |c06,234.8,40.4,red,#9 BLR,T/H,ST
            |c07,264.6,40.4,red,#10 BLR,T/H,ST
            |c08,88.4,68.2,red,#2 ST,T/H,ST
            |c09,40,75.4,red,MS F,T/H,ST
            |c10,24.4,93.2,red,#1 LP F,T/H,ST
            |c11,24.4,104.4,red,#2 LP F,T/H,ST
            |c12,66,114,red,COP DISCH F,T/H,ST
            |c13,40.4,134.6,red,BLD STEAM F,T/H,ST
            |c14,64,161.6,red,EXT STEAM F,T/H,ST
            |c15,154,58.8,red,#3 ST,T/H,ST
            |c16,133,96.6,red,SK,T/H,ST
            |c17,106,96.4,red,C17????,T/H,ST
            |c18,102.6,156,red,C18????,T/H,ST
            |c19,165.4,154.4,red,C19????,T/H,ST
            |c20,201.6,56.6,red,C20????,T/H,ST
            |c21,252,59,red,C21????,T/H,ST
            |c22,216,89.4,red,C22????,T/H,ST
            |c23,236,89.4,red,C23????,T/H,ST
            |c24,187,89.4,red,C24????,T/H,ST
            |c25,174.4,125.8,red,C25????,T/H,ST
            |c26,207,130.4,red,C26????,T/H,ST
            |c27,271,99.4,red,C27????,T/H,ST
            |c28,267.6,130.2,red,C28????,T/H,ST
            |c29,263,162.8,red,C29????,T/H,ST
            |c30,376,29,red,증기 생산량,T/H,ST
            |c31,376,40,red,증기 공급량,T/H,ST
            |c32,385,49.4,red,원증기 공급량,T/H,ST
            |c33,385,60.6,red,증기 고압 공급량,T/H,ST
            |c34,385,71.6,red,증기 중압 공급량,T/H,ST
            |c35,385,82.6,red,증기 저압 공급량,T/H,ST
            |c36,385,93.6,red,증기 공급량 소계,T/H,ST
            |c37,385,104.6,red ,증기 제염 공급량,T/H,ST
            |c38,376,141,red,잉여증기 수급량,T/H,ST
            |c39,385,152.6,red,잉여증기 고압 수급량,T/H,ST
            |c40,385,163.6,red,잉여증기 중압 수급량,T/H,ST
            |c41,385,174.6,red,잉여증기 저압 수급량,T/H,ST
            |c42,385,185.6,red,잉여증기 원증기 수급량,T/H,ST
            """,
            """
            #6BLR
            |/images/haju_factory/main_n4.png
            |P_HMI_MAIN_6BLR_SEARCH
            |d01,75,17.6,red,STEAM,T/H,ST
            |d02,148,17.6,red,Coal,T/H,ST
            |d03,221.6,17.6,red,LNG,Nm3/h,ST
            |d04,131,35.2,red,SH2 압력 D04????,kg/cm²,ST
            |d05,137.6,43.2,red,SH2 무슨 비율?,%,ST
            |d06,36,85,red,D06???,mbar,ST
            |d07,36.4,97.2,red,D07???,Nm³/s,ST
            |d08,36,110.2,red,D08???,mbar,ST
            |d09,36.4,122.2,red,D09???,Nm³/s,ST
            |d10,36,137.6,red,D08???,mbar,ST
            |d11,36.4,151.6,red,D11???,Nm³/s,ST
            |d12,33.6,158,red,D12???,°C,ST
            |d13,33.6,163.6,red,D13???,°C,ST
            |d14,33.6,169,red,D14???,°C,ST
            |d15,33.6,174.4,red,D15???,°C,ST
            |d16,33.6,179.8,red,D16???,°C,ST
            |d17,50,163.4,red,D17???,°C,ST
            |d18,50,169,red,D18???,°C,ST
            |d19,50,174.2,red,D19???,°C,ST
            |d20,50,179.6,red,D20???,°C,ST
            |d21,90.6,67.6,red,D21???,°C,ST
            |d22,90.6,74,red,D22???,°C,ST
            |d23,90.6,80.2,red,D23???,°C,ST
            |d24,90.6,86.4,red,D24???,%,ST
            |d25,90.6,111.2,red,D25???,%,ST
            |d26,90.6,139.2,red,D26???,%,ST
            |d27,131,85.4,red,D27???,mbar,ST
            |d28,131,94,red,D28???,mbar,ST
            |d29,131,102,red,D29???,mbar,ST
            |d30,131,110.8,red,D30???,mbar,ST
            |d31,131,122.6,red,D31???,mbar,ST
            |d32,181.6,56.8,red,D32???,°C,ST
            |d33,181.6,63,red,D33???,°C,ST
            |d35,209.8,48,red,D35???,°C,ST
            |d36,181.6,84.6,red,D36???,°C,ST
            |d37,181.6,77.8,red,D37???,°C,ST
            |d38,181.6,97,red,D38???,°C,ST
            |d39,115.6,152.4,red,D39???,mbar,ST
            |d40,115.6,158.8,red,D40???,mbar,ST
            |d41,115.6,164.6,red,D41???,mbar,ST
            |d42,168,152,red,D42???,AMP,ST
            |d43,195,152,red,D43???,AMP,ST
            |d44,376,15.6,red,SO2,ppm,ST
            |d45,376,27.6,red,NOX,ppm,ST
            |d46,320,15.8,red,FLY ASH SILO,%,ST
            |d47,318,27.4,red,LIMESTONE BIN,TON,ST
            |d48,376,39,red,DUST,units,ST
            |d49,378,50.6,red,O₂,%,ST
            |d50,285.6,65,red,D50???,%,ST
            |d51,282.8,71.8,red,D51???,ppm,ST
            |d52,282.8,78.8,red,D52???,ppm,ST
            |d53,309,88.4,red,D53???,°C,ST
            |d54,304,95,red,D54???,mbar,ST
            |d55,350,116,red,D55???,%,ST
            |d56,366,116,red,D56???,AMP,ST
            |d57,349,129.8,red,BFWP-A,A,ST
            |d58,349,136.4,red,B,A,ST
            |d59,347,143,red,P,bar,ST
            |d60,349,149.6,red,T,°C,ST
            |d61,347.4,156.6,red,F,T/H,ST
            |d62,304,163,red,D62???,AMP,ST
            |d63,266,178,red,D63???,AMP,ST
            |d64,326,163,red,D64???,%,ST
            |d65,349,163,red,D65???,°C,ST
            |d66,293.6,190.2,red,D66???,%,ST
            |d67,315.4,190.2,red,D67???,°C,ST
            |d68,370.6,171.4,red,D68???,Nm³/s,ST
            |d69,370.6,186,red,D69???,Nm³/s,ST
            |d70,266.4,96.4,red,D70???,°C,ST
            |d71,181.6,104.6,red,D71???,°C,ST
            """,
            """
            #7BLR
            |/images/haju_factory/main_n5.png
            |P_HMI_MAIN_7BLR_SEARCH
            |e01,75,17.4,red,STEAM,T/H,ST
            |e02,148.4,17.4,red,Coal,T/H,ST
            |e03,224,17.4,red,LNG,L/H,ST
            |e05,26.6,61.4,red,E05???,Nm³/s,ST
            |e06,26.6,81.4,red,E06???,Nm³/s,ST
            |e07,87,42.8,red,E07???,%,ST
            |e08,81.6,49.6,red,E08???,mmH₂O,ST
            |e09,87,62.2,red,E09???,%,ST
            |e10,81.6,68.8,red,E10???,mmH₂O,ST
            |e11,87,81.4,red,E11???,%,ST
            |e12,81.6,88,red,E12???,mmH₂O,ST
            |e13,44,104,red,E13???,Nm³/s,ST
            |e14,55,138.6,red,E14???,°C,ST
            |e15,55,145,red,E15???,°C,ST
            |e16,55,151.2,red,E16???,°C,ST
            |e17,55,157.8,red,E17???,°C,ST
            |e18,55,164,red,E18???,°C,ST
            |e19,55,170.6,red,AVG,°C,ST
            |e20,115,146,red,A E20???,mmH₂O,ST
            |e21,115,152.6,red,B E21???,mmH₂O,ST
            |e22,115,159,red,C E22???,mmH₂O,ST
            |e23,131,34.4,red,E23???,kg/cm²,ST
            |e24,137,42.4,red,E24???,%,ST
            |e25,128.4,86.4,red,SH2 A E25???,mmH₂O,ST
            |e26,128.4,94,red,SH2 B E26???,mmH₂O,ST
            |e27,128.4,101.6,red,SH2 C E27???,mmH₂O,ST
            |e28,128.4,110,red,SH2 D E28???,mmH₂O,ST
            |e29,128.4,125.4,red,SH2 ?? E29???,mmH₂O,ST
            |e30,93,185,red,E30???,mmH₂O,ST
            |e31,181.6,56.4,red,E31???,°C,ST
            |e32,181.6,63,red,E32???,°C,ST
            |e33,181.6,77.6,red,E33???,°C,ST
            |e34,181.6,84.4,red,E34???,°C,ST
            |e35,181.6,97,red,E35???,°C,ST
            |e36,181.6,104.6,red,E36???,°C,ST
            |e37,186,139,red,E37???,kg/cm²G,ST
            |e38,174,162,red,E38???,A,ST
            |e39,174,168.4,red,E39???,A,ST
            |e40,209.6,47.8,red,E40???,mmH₂O,ST
            |e41,280,88,red,E41???,%,ST
            |e42,280,94.4,red,E42???,°C,ST
            |e43,264,122.6,red,E43???,A,ST
            |e44,264,129,red,E44???,A,ST
            |e45,260.6,135.6,red,E45???,kg/cm²,ST
            |e46,261.4,141.6,red,E46???,T/H,ST
            |e47,246,177.6,red,E47???,°C,ST
            |e48,246,184.6,red,E48???,%,ST
            |e49,268,184.6,red,E49???,A,ST
            |e50,319,94.4,red,E50???,mmH₂O,ST
            |e51,328,117,red,E51???,%,ST
            |e52,312,152.4,red,E52???,%,ST
            |e53,331,152.4,red,E53???,A,ST
            |e54,350,152.4,red,E54???,°C,ST
            |e55,362,168,red,E55???,Nm³/s,ST
            |e56,362,182.6,red,E56???,Nm³/s,ST
            |e57,376,16,red,SOX,ppm,ST
            |e58,376,27.8,red,NOX,ppm,ST
            |e59,376,39.6,red,DUST,ppm,ST
            |e60,376,51,red,O₂,ppm,ST
            """,
            """
            #8BLR
            |/images/haju_factory/main_n6.png
            |P_HMI_MAIN_8BLR_SEARCH
            |f01,76,17.6,red,Steam,T/H,ST
            |f02,149.4,17.6,red,Coal,T/H,ST
            |f03,224,17.6,red,LNG,L/H,ST
            |f04,321.6,16.2,red,FLY ASH SILO,%,ST
            |f05,319.6,27.6,red,LIMESTONE BIN,TON,ST
            |f06,376,16.2,red,SOX,ppm,ST
            |f07,376,27.6,red,NOX,ppm,ST
            |f08,376,39.6,red,DUST,ppm,ST
            |f09,376,51.4,red,O₂,ppm,ST
            |f10,27,61.4,red,F10???,Nm³/s,ST
            |f11,27,81.4,red,F11???,Nm³/s,ST
            |f12,86.4,43,red,F12???,%,ST
            |f13,81.6,49.4,red,F13???,mmH₂O,ST
            |f14,86.4,62.2,red,F14???,%,ST
            |f15,81.6,68.6,red,F15???,mmH₂O,ST
            |f16,86.4,81.6,red,F16???,%,ST
            |f17,81.6,88,red,F17???,mmH₂O,ST
            |f18,44,104,red,F18???,Nm³/s,ST
            |f19,55,138.6,red,F19???,°C,ST
            |f20,55,145.2,red,F20???,°C,ST
            |f21,55,151.6,red,F21???,°C,ST
            |f22,55,157.8,red,F22???,°C,ST
            |f23,55,164,red,F23???,°C,ST
            |f24,55,170.6,red,AVG,°C,ST
            |f25,115,146,red,A F25???,mmH₂O,ST
            |f26,115,152.4,red,B F26???,mmH₂O,ST
            |f27,115,159,red,C F27???,mmH₂O,ST
            |f28,75.2,184.8,red,F28???,°C,ST
            |f29,93,184.8,red,F29???,mmH₂O,ST
            |f30,130.8,34,red,F30???,kg/cm²,ST
            |f31,137,42.4,red,F31???,%,ST
            |f32,127.4,86,red,SH2 A F32???,mmH₂O,ST
            |f33,127.4,94,red,SH2 B F33???,mmH₂O,ST
            |f34,127.4,102,red,SH2 C F34???,mmH₂O,ST
            |f35,128,125,red,SH2 F35???,mmH₂O,ST
            |f36,181.6,56.6,red,F36???,°C,ST
            |f37,181.6,63,red,F37???,°C,ST
            |f38,181.6,84.6,red,F38???,°C,ST
            |f39,181.6,77.6,red,F39???,°C,ST
            |f40,181.6,104.6,red,F40???,°C,ST
            |f41,181.6,97,red,F41???,°C,ST
            |f42,155,141,red,F42???,kg/cm²G,ST
            |f43,158,182.6,red,F43???,°C,ST
            |f44,218,38.4,red,F44???,mmH₂O,ST
            |f45,209.6,48,red,F45???,°C,ST
            |f46,279.6,88,red,F46???,%,ST
            |f47,279.6,94.4,red,F47???,°C,ST
            |f48,222,104,red,F48???,°C,ST
            |f49,264,122.6,red,BFWP-A,A,ST
            |f50,264,129.6,red,B,A,ST
            |f51,261,135.6,red,P,kg/cm²,ST
            |f52,261.6,141.6,red,F,T/H,ST
            |f53,236,169.6,red,F53???,mmH₂O,ST
            |f54,260,169.6,red,F54???,A,ST
            |f55,197,184.4,red,F55???,A,ST
            |f56,247,184.6,red,F56???,%,ST
            |f57,319,94,red,F57???,mmH₂O,ST
            |f58,328,116.6,red,F58???,%,ST
            |f59,350,116.6,red,F59???,A,ST
            |f60,316,161.2,red,F60???,%,ST
            |f61,335.6,161.2,red,F61???,°C,ST
            |f62,266,184.6,red,F62???,°C,ST
            |f63,362,175,red,F63???,Nm³/s,ST
            |f64,362,182,red,F64???,Nm³/s,ST
            |f65,189,133,red,F65???,%,ST
            """,
			"""
            #9BLR
            |/images/haju_factory/main_n7.png
            |P_HMI_MAIN_9BLR_SEARCH
            |g01,76,17.6,red,Steam,T/H,ST
            |g02,148,17.6,red,LNG,M3/H,ST
            |g03,226,17.6,red,LPG,KG/H,ST
            |g04,376,15.6,red,NOX,ppm,ST
            |g05,378.6,27,red,O₂,%,ST
            |g06,369,39,red,FLOW,kNm³/s,ST
            |g07,378.6,50.6,red,TEMP,°C,ST
            |g08,66,76,red,G08???,mmH₂O,ST
            |g09,66,84,red,G09???,mmH₂O,ST
            |g10,66,92.2,red,G10???,mmH₂O,ST
            |g11,66,100.6,red,G11???,mmH₂O,ST
            |g12,51,133.4,red,G12???,mmH₂O,ST
            |g13,112,54,red,G13???,%,ST
            |g14,108.6,60.2,red,G14???,kg/cm²,ST
            |g15,92,71.2,red,G15???,°C,ST
            |g16,92,82.8,red,G16???,°C,ST
            |g17,92,94,red,G17???,°C,ST
            |g18,130,110,red,G18???,%,ST
            |g19,125,116.2,red,G19???,mmH₂O,ST
            |g20,130,122.4,red,G20???,°C,ST
            |g21,125.6,145,red,G21???,mmH₂O,ST
            |g22,130.2,151.2,red,G22???,°C,ST
            |g23,123.6,157.6,red,G23???,kNm³/h,ST
            |g24,178,117.6,red,G24???,mmH₂O,ST
            |g25,183,123.8,red,G25???,°C,ST
            |g26,179.6,144.6,red,G26???,°C,ST
            |g27,227.4,91.4,red,G27???,A,ST
            |g28,270,83,red,G28???,rpm,ST
            |g29,274,89,red,G29???,°C,ST
            |g30,263.6,140,red,G30???,mmH₂O,ST
            |g31,268,145.8,red,G31???,°C,ST
            |g32,292,162,red,G32???,A,ST
            |g33,313,162,red,G33???,%,ST
            """,
			"""
            #10BLR
            |/images/haju_factory/main_n8.png
            |P_HMI_MAIN_10BLR_SEARCH
            |h01,76,17.6,red,Steam,T/H,ST
            |h02,148,17.6,red,LNG,M3/H,ST
            |h03,226,17.6,red,LPG,KG/H,ST
            |h04,376,15.6,red,NOX,ppm,ST
            |h05,378.6,27,red,O₂,%,ST
            |h06,369,39,red,FLOW,kNm³/h,ST
            |h07,378.6,50.6,red,TEMP,°C,ST
            |h08,66,76,red,H08???,mmH₂O,ST
            |h09,66,84,red,H09???,mmH₂O,ST
            |h10,66,92.2,red,H10???,mmH₂O,ST
            |h11,66,100.6,red,H11???,mmH₂O,ST
            |h12,51.6,133.8,red,H12???,mmH₂O,ST
            |h13,112,54,red,H13???,%,ST
            |h14,108.6,60.2,red,H14???,kg/cm²,ST
            |h15,92,71.4,red,H15???,°C,ST
            |h16,92,83,red,H16???,°C,ST
            |h17,92,94.2,red,H17???,°C,ST
            |h18,130,110,red,H18???,%,ST
            |h19,125,116.2,red,H19???,mmH₂O,ST
            |h20,130,122.4,red,H20???,°C,ST
            |h21,125,145,red,H21???,mmH₂O,ST
            |h22,130,151.2,red,H22???,°C,ST
            |h23,123.6,157.6,red,H23???,kNm³/h,ST
            |h24,183,123.8,red,H24???,°C,ST
            |h25,228,91,red,H25???,A,ST
            |h26,271,83,red,H26???,rpm,ST
            |h27,274,89,red,H27???,°C,ST
            |h28,179.6,144.6,red,H28???,°C,ST
            |h29,263.6,140,red,H29???,mmH₂O,ST
            |h30,268,145.8,red,H30???,°C,ST
            |h31,292,162,red,H31???,A,ST
            |h32,312,162,red,H32???,%,ST
            |h33,178.4,117.6,red,H33???,mmH₂O,ST
            """,
            """
            #CCPP
            |/images/haju_factory/main_n9.png
            |P_HMI_MAIN_CCPP_SEARCH
            |k01,68,24.2,red,MS BF,T/H,ST
            |k02,87.8,24.2,red,MS FF,T/H,ST
            |k03,39,36,red,KOGAS FUEL F,T/H,ST
            |k04,39,42.4,red,KOGAS FUEL P,barg,ST
            |k05,109,34.6,red,#1HRSG HP F,T/H,ST
            |k06,109,41,red,#1HRSG HP P,barg,ST
            |k07,36.6,59.2,red,#1HRSG FUEL F,T/H,ST
            |k08,25,76,red,#1 GTG,MW,PW
            |k09,100,62.4,red,#1HRSG EXH.P,barg,ST
            |k10,102,73.8,red,#1HRSG EXH.T,°C,ST
            |k11,42,89.8,red,#1HRSG VGV POS,%,ST
            |k12,69,89.8,red,#1HRSG SPEED,rpm,ST
            |k13,109,113.4,red,#2HRSG HP F,T/H,ST
            |k14,109,120,red,#2HRSG HP P,barg,ST
            |k15,111,126.4,red,#2HRSG HP T,°C,ST
            |k16,37,147.4,red,#2HRSG FUEL F,T/H,ST
            |k17,25,164,red,#2 GTG,MW,PW
            |k18,42,177,red,#2HRSG VGV POS,%,ST
            |k19,70,177,red,SPEED,rpm,ST
            |k20,101,151.4,red,#2HRSG EXH.P,barg,ST
            |k21,102,162.8,red,#2HRSG EXH.T,°C,ST
            |k22,157,22.4,red,#1HRSG LP F,T/H,ST
            |k23,157,29,red,#1HRSG LP P,barg,ST
            |k24,159,35.6,red,#1HRSG LP T,°C,ST
            |k25,156,48.2,red,#1HRSG HP DRUM L,%,ST
            |k26,196,48.2,red,#1HRSG LP DRUM L,%,ST
            |k27,228.6,36.4,red,#1HRSG LP FW F,T/H,ST
            |k28,228.6,42.8,red,#1HRSG LP FW P,barg,ST
            |k29,276.4,25.8,red,#1HRSG NOX,ppm,ST
            |k30,276.4,32.4,red,#1HRSG O₂,ppm,ST
            |k31,276.4,39,red,#1HRSG FLOW,T/H,ST
            |k32,278.4,45.6,red,#1HRSG TEMP,°C,ST
            |k33,215.6,92,red,#1HRSG HP FW F,T/H,ST
            |k34,215.6,98.6,red,#1HRSG HP FW P,barg,ST
            |k35,157,105.4,red,#2HRSG LP F,T/H,ST
            |k36,157,112,red,#2HRSG LP P,barg,ST
            |k37,159,118.4,red,#2HRSG LP T,°C,ST
            |k38,156.2,137,red,#2HRSG HP DRUM L,%,ST
            |k39,196,137,red,#2HRSG LP DRUM L,%,ST
            |k40,228.6,126.6,red,#2HRSG LPFW F,T/H,ST
            |k41,228.6,133.2,red,#2HRSG LP FW P,barg,ST
            |k42,276.4,115.4,red,#2HRSG NOX,ppm,ST
            |k43,276.4,121.8,red,#2HRSG O₂,ppm,ST
            |k44,281.4,128.4,red,#2HRSG FLOW,T/H,ST
            |k45,278.4,135.4,red,#2HRSG TEMP,°C,ST
            |k46,215.6,180.8,red,#2HRSG HP FW F,T/H,ST
            |k47,215.6,187,red,#2HRSG HP FW p,barg,ST
            |k48,364,13.2,red,#2HRSG MS F,T/H,ST
            |k49,364,19.6,red,#2HRSG MS P,barg,ST
            |k50,366,26.2,red,#2HRSG MS T,°C,ST
            |k51,390,33.4,red,#1 STG,MW,PW
            |k52,324.6,49,red,BLD STEAM F,T/H,ST
            |k53,331,75.4,red,EXT STEAM F,T/H,ST
            |k54,378,73.4,red,#1 STEAM F,T/H,ST
            |k55,378,98,red,#2 STEAM F,T/H,ST
            |k56,336,112.8,red,COND HW L,%,ST
            |k57,334,123,red,COND P,barg,ST
            |k58,316.4,154,red,D/A FW F,T/H,ST
            |k59,111,47.8,red,#1HRSG HP T,°C,ST
            |k60,345,160.6,red,COP DISCH F,T/H,ST
            |k61,316.4,160.6,red,D/A FW P,barg,ST
            """
        };
        protected List<Mtab> mTabList = new List<Mtab>();
        protected string?DATE{ get; set; }
        protected string?COM_ID{ get; set; }


        // 타이머 관련 변수
        static private Timer? _refreshTimer;
        private readonly int _refreshInterval = 30; // 30초

        protected override void OnInitialized()
        {
            initTabList();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                InitControls();

                // 
                await btn_Search();

                // 타이머 설정 & 시작
                if(_refreshTimer == null)
                {

                
                _refreshTimer = new Timer(async _ => {

                  if (IsActive || string.IsNullOrEmpty(appData.ActiveTabMenuID) ) {
                    // 확장된 탭들 데이터 갱신
                    await ReloadExpandedTabs();

                    // UI 수동 갱신
                    InvokeAsync(StateHasChanged);
                  }

                }, null, TimeSpan.FromSeconds(_refreshInterval), TimeSpan.FromSeconds(_refreshInterval));
                }
            }
        }

        public void Dispose()
        {
            // 타이머 정리
            if (_refreshTimer != null)
            {
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        private void initTabList()
        {
            mTabList.Clear();
            mTabList = tabs.Select(tab => {
                var tabInfo = tab.Split('|', StringSplitOptions.RemoveEmptyEntries);
                return new Mtab
                {
                    title = tabInfo[0].Trim(),
                    imgURL = tabInfo[1].Trim(),
                    sp_name = tabInfo[2].Trim(),
                    mlabelList = tabInfo.Skip(3)
                                        .Select(label => {
                                            var s = label.Split(',');
                                            return new Mlabel
                                            {
                                                id = s[0].Trim(),
                                                xx = s[1].Trim(),
                                                yy = s[2].Trim(),
                                                color = s[3].Trim(),
                                                name = s.Length > 4 ? s[4].Trim() : "nonono",
                                                unit = s.Length > 5 ? s[5].Trim() : "mmmmmmmmm",
                                                utility = s.Length > 6 ? s[6].Trim() switch
                                                {
                                                    "PW" => MitCard.CardUtilityType.Power,
                                                    "ST" => MitCard.CardUtilityType.Steam,
                                                    "WA" => MitCard.CardUtilityType.Water,
                                                    _ => MitCard.CardUtilityType.None,
                                                } : MitCard.CardUtilityType.None,
                                            };
                                        }).ToList()
                };
            }).ToList();

            //
            Func<Mtab, Task<DataTable?>>[] DBSearchArray = new Func<Mtab, Task<DataTable?>>[]{
                DBSearchEntire,
                DBSearchElec,
                DBSearchSteam,
                DBSearch6BLR,
                DBSearch7BLR,
                DBSearch8BLR,
                DBSearch9BLR,
                DBSearch10BLR,
                DBSearchCCPP,
            };
            for (int i = 0; i < mTabList.Count; ++i)
            {
                mTabList[i].DBSearch = DBSearchArray[i];
            }






        }
       
        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            
        }

        protected async void OnTabChanged(int tabindex)
        {
            for (int i = 0; i < mTabList.Count; ++i)
            {
                mTabList[i].IsExpend = (i == tabindex);
            }
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await btn_Search();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 기능 ]

        protected async Task btn_Search()
        {
            //await Search();
            await ReloadExpandedTabs();
        }

        #endregion [ 사용자 버튼 기능 ]

        #region [ 사용자 이벤트 함수 ]

        protected void OnInputSearchParameter(Dictionary<string, object?> parameters)
        {
            
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]



        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        //private async Task<DataTable?> DBSearchFW_A()
        //{
        //    if (QueryService == null)
        //        return null;

        //    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLISTWATFW_SELECT01", new Dictionary<string, object?>()
        //        {
        //            {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
        //            {"COM_ID", '%' },
        //        });

        //    return datatable;
        //}
        //private async Task<DataTable?> DBSearchDW_A()
        //{
        //    if (QueryService == null)
        //        return null;

        //    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLISTWATDW_SELECT01", new Dictionary<string, object?>()
        //        {
        //            {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
        //            {"COM_ID", '%' },
        //        });

        //    return datatable;
        //}
        //private async Task<DataTable?> DBSearchElec_A()
        //{
        //    if (QueryService == null)
        //        return null;

        //    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLISTELC_SELECT01", new Dictionary<string, object?>()
        //        {
        //            {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
        //            {"COM_ID", '%' },
        //        });

        //    return datatable;
        //}
        //private async Task<DataTable?> DBSearchSteam_A()
        //{
        //    if (QueryService == null)
        //        return null;

        //    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLISTSTM_SELECT01", new Dictionary<string, object?>()
        //        {
        //            {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
        //            {"COM_ID", '%' },
        //        });

        //    return datatable;
        //}

        public bool isLoading { get; set; } = false;
        private async Task<DataTable?> DBSearchEntire(Mtab mtab)
        {

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;


                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            return null;
        }

        private async Task<DataTable?> DBSearchElec(Mtab mtab)
        {

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;

                await LoadData(mtab);
            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            return null;
        }

        private async Task<DataTable?> DBSearchSteam(Mtab mtab)
        {

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            return null;
        }
        private async Task<DataTable?> DBSearch6BLR(Mtab mtab)
        {

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

            return null;
        }
        private async Task<DataTable?> DBSearch7BLR(Mtab mtab)
        {

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                //Console.WriteLine($"isLoading {isLoading}");
                StateHasChanged();
            }

            return null;
        }
        private async Task<DataTable?> DBSearch8BLR(Mtab mtab)
        {
            //Console.WriteLine("DBSearch8BLR");

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;
                //Console.WriteLine($"isLoading {isLoading}");

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                //Console.WriteLine($"isLoading {isLoading}");
                StateHasChanged();
            }

            return null;
        }
        private async Task<DataTable?> DBSearch9BLR(Mtab mtab)
        {

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;
                //Console.WriteLine($"isLoading {isLoading}");

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                //Console.WriteLine($"isLoading {isLoading}");
                StateHasChanged();
            }

            return null;
        }
        private async Task<DataTable?> DBSearch10BLR(Mtab mtab)
        {
            //Console.WriteLine("DBSearch10BLR");

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                //Console.WriteLine($"isLoading {isLoading}");
                StateHasChanged();
            }

            return null;
        }
        private async Task<DataTable?> DBSearchCCPP(Mtab mtab)
        {

            //Console.WriteLine("DBSearchCCPP");

            try
            {
                // 현재탬 빼고 모두 닫기
                foreach (Mtab tab in mTabList)
                {
                    if (tab == mtab) continue;
                    tab._isExpend = false;
                }

                isLoading = true;
                //Console.WriteLine($"isLoading {isLoading}");

                await LoadData(mtab);

            }
            catch (Exception ex)
            {

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
                //Console.WriteLine($"isLoading {isLoading}");
                StateHasChanged();
            }

            return null;
        }
        async Task ReloadExpandedTabs()
        {
            bool isReloaded = false;
            foreach (var mTab in mTabList)
            {
                if (mTab.IsExpend)
                {
                    //Console.WriteLine($"{mTab.title} 탭 리로드");
                    await LoadData(mTab);
                    isReloaded = true;
                }
            }

            // pc모드에서는 모두 접힌 경우가 없기때문에 추가함.
            // 모두 접힌 상태이면, 첫번째 펼친것으로 상태변경
            if (!isReloaded && !IsMobileMode)
            {
                mTabList[0].IsExpend = true;
            }


            // 모바일 모드일때만 동작
            if (IsMobileMode) { 
                // 모바일로 새로 띄운 창 body갱신
                await JSRuntime.InvokeVoidAsync("update_new_window_body", null);
            }

        }

        async Task LoadData(Mtab mtab)
        {

            if (QueryService == null) return;

            var dt = await QueryService.ExecuteDatatableAsync(mtab.sp_name, new Dictionary<string, object?>() { { "COM_ID", '%' } });

            if (dt == null) return;

            for (int j = 0; j < mtab.mlabelList.Count; ++j)
            {
                Mlabel mlabel = mtab.mlabelList[j];
                string columnName = mlabel.id;
                mlabel.data = dt.Columns.Contains(columnName) ? dt.Rows[0][columnName] : "칼럼x";
            }
        }
        #endregion [ 데이터 정의 메소드 ]
    }
}
