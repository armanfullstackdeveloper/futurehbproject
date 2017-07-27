using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using BusinessLogic.Components;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.BussinesLogics
{
    public class UserBL : DapperConfiguration
    {
        private readonly IDbConnection _db;
        public UserBL()
        {
            _db = EnsureOpenConnection();
        }
        public QueryResult<User> Save(User obj)
        {
            try
            {
                using (var txScope = new TransactionScope())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", value: obj.Id, dbType: DbType.String, size: 128, direction: ParameterDirection.InputOutput);
                    parameters.Add("@UserName", obj.UserName);
                    parameters.Add("@Password", obj.Password);
                    parameters.Add("@RoleCode", obj.RoleCode);
                    parameters.Add("@Email", obj.Email);
                    parameters.Add("@RegisterDate", PersianDateTime.Now.Date.ToInt());
                    parameters.Add("@RegisterBy", obj.RegisterBy);
                    parameters.Add("@HashCode", obj.HashCode);
                    parameters.Add("@TelegramId", obj.TelegramId);
                    parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                    _db.Execute("User_Save", parameters, commandType: CommandType.StoredProcedure);
                    obj.Id = parameters.Get<string>("@Id");
                    var procResult = parameters.Get<int>("@ProcResult");
                    txScope.Complete();
                    EnsureCloseConnection(_db);
                    return new QueryResult<User>(obj, null, procResult);
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(obj).ToString());
            }
        }

        public QueryResult<User> Update(User obj)
        {
            try
            {
                using (var txScope = new TransactionScope())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", value: obj.Id, dbType: DbType.String, size: 128);
                    parameters.Add("@UserName", obj.UserName);
                    parameters.Add("@Password", obj.Password);
                    parameters.Add("@RoleCode", obj.RoleCode);
                    parameters.Add("@isActive", obj.IsActive);
                    parameters.Add("@Email", obj.Email);
                    parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                    _db.Execute("User_Update", parameters, commandType: CommandType.StoredProcedure);
                    var procResult = parameters.Get<int>("@ProcResult");
                    txScope.Complete();
                    EnsureCloseConnection(_db);
                    return new QueryResult<User>(obj, null, procResult);
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(obj).ToString());
            }
        }

        public bool DeleteById(string id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", value: id, dbType: DbType.String, size: 128);
                parameters.Add("@ProcResult", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                _db.Execute("User_DeleteByID", parameters, commandType: CommandType.StoredProcedure);
                var procResult = parameters.Get<int>("@ProcResult");
                EnsureCloseConnection(_db);
                if (DbMessage.ChechTypeById(procResult) == MessageType.Success)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, id);
            }
        }

        public User GetById(string id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", value: id, dbType: DbType.String, size: 128);
                User user = _db.Query<User>("User_SelectByID", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return user;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, id);
            }
        }

        public List<User> GetAll(string usernameOrId, bool? active = null, int? pageNumer = 1)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@active", active);
                parameters.Add("@username", usernameOrId);
                parameters.Add("@PageNumber", pageNumer);
                parameters.Add("@RowspPage", StaticNembericInBL.CountOfItemsInAdminPages);
                List<User> users = _db.Query<User>(@"select * from [dbo].[User] where (@active is null or IsActive=@active)
                        and (@username is null or
                        ([UserName] like @username or [UserName] like '%'+@username or [UserName] like '%'+@username+'%' or 
                        [Id] like @username or [Id] like '%'+@username or [Id] like '%'+@username+'%'))
                        order by Id 
                        OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
                        FETCH NEXT @RowspPage ROWS ONLY;", parameters).ToList();
                EnsureCloseConnection(_db);
                return users;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, string.Empty);
            }
        }

        public User GetByUserNameAndPassword(string username, string password)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", username);
                parameters.Add("@Password", password);
                User user = _db.Query<User>("User_SelectByUserNameAndPassword", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return user;
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => username),
                        Value = username
                    },
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => password),
                        Value = password
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }


        public User GetByUserName(string username)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", username);
                User user = _db.Query<User>("User_SelectByUserName", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return user;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, username);
            }
        }

        public User GetByTelegramId(string chatId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@chatId", chatId);
                User user = _db.Query<User>("User_SelectByTelegramId", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return user;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, chatId);
            }
        }

        public User GetByHashCode(string hashCode)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@hashCode", hashCode);
                User user = _db.Query<User>("User_SelectByHashCode", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return user;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, hashCode);
            }
        }


        public List<User> GetByEmail(string email)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@email", email);
                List<User> users = _db.Query<User>("User_SelectByEmail", parameters, commandType: CommandType.StoredProcedure).ToList();
                EnsureCloseConnection(_db);
                return users;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, email);
            }
        }

        public bool IfUsernameExist(string username)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", username);
                int a = _db.Query<int>("User_SelectUserName", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                EnsureCloseConnection(_db);
                return (a > 0);
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, username);
            }
        }
    }
}
