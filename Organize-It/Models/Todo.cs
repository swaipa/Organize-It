using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organize_It.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public bool IsDoing { get; set; }
        public bool IsDone { get; set; }
        public virtual IdentityUser User { get; set; } // Nichlas: need the current users name and id
    }
}
