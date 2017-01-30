using DataModel.Entities;
using DataModel.Enums;

namespace Boundary.Helper.StaticValue
{
    public class StaticRole
    {
        public static string GetRoleName(ERole role)
        {
            switch (role)
            {
                case ERole.Admin:
                    return StaticString.Role_Admin;
                case ERole.Seller:
                    return StaticString.Role_Seller;
                case ERole.Member:
                    return StaticString.Role_Member;
                case ERole.SuperAdmin:
                    return StaticString.Role_SuperAdmin;
                default:
                    return null;
            }
        }

        public static Role GetRole(ERole role) 
        {
            switch (role)
            {
                case ERole.Admin:
                    return Admin;
                case ERole.Seller:
                    return Seller;
                case ERole.Member:
                    return Member;
                case ERole.SuperAdmin:
                    return SuperAdmin;
                default:
                    return null;
            }
        }

        public static Role Member
        {
            get
            {
                return new Role()
                {
                    Name = StaticString.Role_Member,
                    Id = ERole.Member
                };
            }
        }

        public static Role Seller
        {
            get
            {
                return new Role()
                {
                    Name = StaticString.Role_Seller,
                    Id = ERole.Seller
                };
            }
        }

        public static Role Admin
        {
            get
            {
                return new Role()
                {
                    Name = StaticString.Role_Admin,
                    Id = ERole.Admin
                };
            }
        }

        public static Role SuperAdmin
        {
            get
            {
                return new Role()
                {
                    Name = StaticString.Role_SuperAdmin,
                    Id = ERole.SuperAdmin
                };
            }
        }
    }
}