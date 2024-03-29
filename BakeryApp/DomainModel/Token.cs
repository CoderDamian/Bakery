﻿namespace DomainModel
{
    public class Token
    {
        public string Value { get; set; } = string.Empty;
        public string Refresh { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; } // Expiry time refresh token
    }
}
