namespace MIT.WebService.Attributes
{
    /// <summary>
    /// 권한 인증 패스
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
