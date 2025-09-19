namespace JinRestApi.Models;

public class Company
{
    public int Id { get; set; }

    /// <summary>고객사 명</summary>
    public string CompanyName { get; set; } = string.Empty;
   
    /// <summary>고객사 대표자</summary>
    public string CeoName { get; set; } = string.Empty;


}

