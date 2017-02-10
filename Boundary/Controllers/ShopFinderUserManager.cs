using System;
using System.Threading.Tasks;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.Components;
using DataModel.Entities;
using DataModel.Enums;
using Microsoft.AspNet.Identity;

namespace Boundary.Controllers
{
    /// <summary>
    /// chon bayad az IUser inherit mishod dige az User estefade nemitonestim bokonim
    /// </summary>
    public class AppUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ERole RoleCode { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; } 
    }

    #region

    public class ShopFinderUserManager : UserManager<AppUser>
    {
        public ShopFinderUserManager() : base(new CustomUserSore<AppUser>()) { }

        public override Task<AppUser> FindAsync(string userName, string password)
        {
            Task<AppUser> taskInvoke = Task<AppUser>.Factory.StartNew(() =>
            {
                User userFinded = new UserBL().GetByUserNameAndPassword(userName, password);
                if (userFinded == null) return null;
                AppUser appUser = new AppUser()
                {
                    Id = userFinded.Id,
                    UserName = userFinded.UserName,
                    //Password = user.Password, 
                    RoleCode = userFinded.RoleCode,
                    IsActive = userFinded.IsActive,
                    Email = userFinded.Email
                };
                return appUser;
            });
            return taskInvoke;
        }

        public Task<IdentityResult> CreateUser(ref AppUser user, string password,string email)
        {
            try
            {
                User newUser = new User()
                {
                    UserName = user.UserName,
                    Password = password,
                    RoleCode = user.RoleCode,
                    Email = email
                };
                var result = new UserBL().Save(newUser);
                if (result.DbMessage.MessageType != MessageType.Success)
                {
                    return Task.FromResult(IdentityResult.Failed(result.DbMessage.Message));
                }
                user.Id = newUser.Id;
                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception)
            {
                return Task.FromResult(IdentityResult.Failed(StaticString.Message_UnSuccessFull));
            }
        }


    }

    public class CustomUserSore<T> : IUserStore<T> where T : AppUser
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task CreateAsync(T user)
        {
            //Create /Register New User
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            //Delete User
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}