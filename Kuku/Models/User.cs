using System;
using Microsoft.AspNetCore.Identity;

namespace Kuku.Models
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateTime { get; set; }
        public bool PermisionLevel { get; set; }
        public string Email { get; set; }
    }
}