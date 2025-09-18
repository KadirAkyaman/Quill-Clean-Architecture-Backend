using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Infrastructure.Options
{
    public class DatabaseOptions
    {
        public const string SectionName = "ConnectionStrings";
        public const string ConnectionStringName = "DefaultConnection";
        public string DefaultConnection { get; set; } = string.Empty;
    }
}