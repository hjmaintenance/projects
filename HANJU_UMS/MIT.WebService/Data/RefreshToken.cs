namespace MIT.WebService.Data
{
    /// <summary>
    /// Refresh Token 정보 클래스
    /// </summary>
    public class RefreshToken
    {
        public string? USER_ID { get; set; }
        public string? ACCESS_TOKEN_ID { get; set; }
        public string? REFRESH_TOKEN { get; set; }
        public DateTime EXPIRES_DATE { get; set; }
        public string? CREATE_IP { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string? REVOKE_IP { get; set; }
        public DateTime? REVOKE_DATE { get; set; }
        public bool IsExpired => DateTime.Now >= EXPIRES_DATE;
        public bool IsRevoked => REVOKE_DATE != null;
        public bool IsActive => !IsRevoked && !IsExpired;

    }
}
