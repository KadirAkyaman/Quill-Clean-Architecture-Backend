using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
    }
}