using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessLogic.BussinesLogics;
using DataModel.Entities;
using DataModel.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DataModel.Entities.User;

namespace TelegramApi.Controllers
{
    public class BotController : ApiController
    {
        /// <summary>
        /// متدی برای پاسخگویی به آپدیت های ربات
        /// </summary>
        public async Task<IHttpActionResult> Post(Update update)
        {
            string token = Resource.General.Bot_Token;
            TelegramBotClient bot = new TelegramBotClient(token);
            var chatType = update.Message.Chat.Type;
            string userChatId = update.Message.From.Username;

            //ربات به آپدیت های گروههای چت پاسخی ندهد
            if (chatType != ChatType.Private)
            {
                return Ok();
            }
            var text = update.Message.Text;

            //اگه قصد لاگین داشت طرف
            if (text != null && text.Contains("signin"))
            {
                var user = new UserBL().GetByTelegramId(userChatId);
#if DEBUG
                string hostName = "http://localhost:5330";
#else
                    string hostName = Resource.General.HostName_Main; 
#endif

                if (user == null)
                {
                    //create user
                    string hashCode = CreateMember(userChatId);
                    string registerPageAddress = hostName + "/signup/telegram?hashCode=" + hashCode;
                    await bot.SendTextMessageAsync(userChatId,
                        $"لطفا از طریق آدرس زیر اطلاعات خود را تکمیل کنید: {registerPageAddress}");
                }
                else
                {
                    string hashCode = Guid.NewGuid().ToString();
                    user.HashCode = hashCode;
                    new UserBL().Update(user);

                    string loginPageAddress = hostName + "/signin/telegram?hashCode=" + hashCode;
                    await bot.SendTextMessageAsync(userChatId,
                        $"لطفا از طریق آدرس زیر وارد شوید: {loginPageAddress}");
                }
            }


            return null;
        }

        //اگه نال یا خالی باشه یعنی موفقیت آمیز نبوده و در غیر این صورت همون هش کد تولید شده
        private string CreateMember(string chatId)
        {
            try
            {
                string hashCode = Guid.NewGuid().ToString();
                var user = new UserBL().Save(new User()
                {
                    TelegramId = chatId,
                    RegisterBy = ERegisterBy.Telegram,
                    //IsActive = true,
                    //RoleCode = ERole.Member,
                    RegisterDate = PersianDateTime.Now.ToInt(),
                    HashCode = hashCode
                }).Obj;

                Member member = new Member()
                {
                    UserCode = user.Id,
                };

                var cutomerRegisterResult = new MemberBL().Insert(member);

                if (cutomerRegisterResult > 0)
                {
                    //user.RoleCode = ERole.Member;
                    //user.IsActive = true;
                    //new UserBL().Update(user);

                    return hashCode;
                }
                new UserBL().DeleteById(user.Id);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        [NonAction]
        public void GetUserActions()
        {
            string token = Resource.General.Bot_Token;
            TelegramBotClient bot = new TelegramBotClient(token);

            while (true)
            {
                try
                {
                    Update[] u = bot.GetUpdatesAsync(0, 0).Result;
                    Update up = u[u.Length - 1];
                    if (up.CallbackQuery != null)
                    {
                        var v = up.CallbackQuery.Data;
                    }
                    Thread.Yield();
                    //Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    //
                }
            }
        }
    }
}
