namespace MIT.Razor.Pages.Component.MessageBox
{
    public class CommonMessage
    {
        //메세지박스 헤더 문구
        public const string ALRIM = "알림";
        public const string ERROR = "오류";

        //저장 메세지
        public const string SAVE_EMPTY_DATA = "저장할 대상이 없습니다.";
        public const string SAVE_PK_DATA = "필수 입력값이 누락되었습니다.";
        public const string SAVE_CHECK = "저장하시겠습니까?";
        public const string SAVE_SUCCESS = "저장이 완료되었습니다.";
        public const string SAVE_FAIL = "저장에 실패하였습니다.";

        //삭제 메세지
        public const string DELETE_EMPTY_DATA = "삭제할 대상이 없습니다.";
        public const string DELETE_CHECK = "삭제하시겠습니까?";
        public const string DELETE_CHECK2 = "하위 정보도 같이 삭제됩니다. 정말 삭제 하시겠습니까?";
        public const string DELETE_SUCCESS = "삭제가 완료되었습니다.";
        public const string DELETE_FAIL = "삭제에 실패하였습니다.";
    }
}
