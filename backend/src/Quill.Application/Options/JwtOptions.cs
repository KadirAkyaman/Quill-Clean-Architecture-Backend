using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.Options
{
    public class JwtOptions
    {
        public const string SectionName = "JwtSettings";
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInMinutes { get; set; } 
    }
}