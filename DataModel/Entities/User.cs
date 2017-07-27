using System;
using DataModel.Enums;

namespace DataModel.Entities
{
    public class User : EntityBase<User>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ERole RoleCode { get; set; } 
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string TelegramId { get; set; }
        public ERegisterBy? RegisterBy { get; set; }
        public int? RegisterDate { get; set; }
        public string HashCode { get; set; }

        public Role Role { get; set; }
    }
}
