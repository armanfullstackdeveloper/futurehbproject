using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic.Helpers;
using Dapper;
using DataModel.Entities;

namespace BusinessLogic.BussinesLogics
{
    public class MemberBL : GenericRepository<Member, long>
    {
        public Member GetSummaryForSession(string userCode)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                Member member = db.Query<Member>("select Id,Name,CityCode,[PicAddress] from [dbo].[Member] where UserCode=@UserCode", new { UserCode = userCode }).SingleOrDefault();
                EnsureCloseConnection(db);
                return member;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userCode);
            }
        }

        public string GetPicAddressByUserCode(string userCode)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                string address = db.Query<string>("select [PicAddress] from [dbo].[Member] WHERE [UserCode]=@userCode", new { userCode }).SingleOrDefault();
                EnsureCloseConnection(db);
                return address;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userCode);
            }
        }

        public new Member SelectOne(long id)
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                Member member = db.Query<Member>("select * from [dbo].[Member] where Id=@code", new { code = id }).SingleOrDefault();
                EnsureCloseConnection(db);
                return member;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, id.ToString());
            }
        }

        public List<Member> SearchByUserCodeAndId(string userCodeOrId) 
        {
            try
            {
                IDbConnection db = EnsureOpenConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@userCodeOrId", userCodeOrId);
                List<Member> members = db.Query<Member>(@"select * from [dbo].[Member] where (@userCodeOrId is null or
                        ([UserCode] like @userCodeOrId or [UserCode] like '%'+@userCodeOrId or [UserCode] like '%'+@userCodeOrId+'%' or 
                        [Id] like @userCodeOrId or [Id] like '%'+@userCodeOrId or [Id] like '%'+@userCodeOrId+'%'))", parameters).ToList();
                EnsureCloseConnection(db);
                return members;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, userCodeOrId);
            }
        }

    }
}
