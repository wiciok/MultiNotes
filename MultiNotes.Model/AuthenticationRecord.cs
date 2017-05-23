using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.Model
{
    public class AuthenticationRecord
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
