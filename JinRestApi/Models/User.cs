namespace JinRestApi.Models;

public class User
{
    public int Id { get; set; }

    /// <summary>사용자 이름</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>로그인 아이디</summary>
    public string LoginId { get; set; } = string.Empty;

    /// <summary>성별(M/F)</summary>
    public string Sex { get; set; } = "M";

    /// <summary>사진 URL</summary>
    public string Photo { get; set; } = string.Empty;


    /// <summary> 이메일 </summary>
    public string EMail { get; set; } = string.Empty;


    /// <summary> 비밀번호 </summary>
    public string PasswordHash { get; set; } = string.Empty;

}

