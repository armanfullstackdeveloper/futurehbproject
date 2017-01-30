namespace BusinessLogic.Components
{
    /// <summary>
    /// use this class for manage returned message from procedure
    /// and
    /// customize message in app
    /// </summary>
    public class DbMessage
    {
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
        public DbMessage(int messageId)
        {
            switch (messageId % 10)
            {
                case 1:
                    MessageType = Components.MessageType.Success;
                    break;
                case 2:
                    MessageType = Components.MessageType.Fail;
                    break;
                case 3:
                    MessageType = Components.MessageType.Info;
                    break;
                default:
                    MessageType = Components.MessageType.UnDefined;
                    break;
            }
            FindMessageText(messageId);
        }

        public static MessageType ChechTypeById(int id)
        {
            switch (id % 10)
            {
                case 1:
                    return Components.MessageType.Success;
                case 2:
                    return Components.MessageType.Fail;
                case 3:
                    return Components.MessageType.Info;
                default:
                    return Components.MessageType.UnDefined;
            }
        }
        private void FindMessageText(int id)
        {
            //قسمت مربوط به پیام های موفقیت آمیز
            #region
            if (MessageType == MessageType.Success)
            {
                switch (id)
                {
                    case 111:
                        {
                            Message = "عملیات با موفقیت انجام شد";
                            break;
                        }
                    case 10001:
                    case 10081:
                        {
                            Message = "عملیات ویرایش با موفقیت به پایان رسید";
                            break;
                        }
                    case 10011:
                    case 10091:
                        {
                            Message = "عملیات درج با موفقیت به پایان رسید";
                            break;
                        }
                    case 10021:
                    case 10061:
                    case 10071:
                        {
                            Message = "عملیات حذف با موفقیت انجام شد";
                            break;
                        }
                    case 10031:
                        {
                            Message = "ثبت فروشگاه با موفقیت انجام شد";
                            break;
                        }
                    case 10041:
                        {
                            Message = "ثبت محصول با موفقیت انجام شد";
                            break;
                        }
                    case 10051:
                        {
                            Message = "ویرایش محصول با موفقیت انجام شد";
                            break;
                        }

                }
            }
            #endregion

            //قسمت مربوط به پیام های ناموفق
            #region
            else if (MessageType == Components.MessageType.Fail)
            {
                switch (id)
                {
                    case 222:
                        {
                            Message = "عملیات با خطا مواجه شد";
                            break;
                        }
                    case 20002:
                        Message = "عملیات ویرایش با موفقیت به پایان نرسید";
                        break;
                    case 20012:
                        Message = "عملیات درج با موفقیت به پایان نرسید";
                        break;
                    case 20022:
                        Message = "خطا در اطلاعات فروشنده";
                        break;
                    case 20032:
                        Message = "خطا در اطلاعات فروشگاه";
                        break;
                    case 20042:
                        Message = "خطا در اطلاعات کتگوری";
                        break;
                    case 20052:
                        Message = "خطا در عملیات ثبت محصول";
                        break;
                    case 20062:
                        Message = "خطا در عملیات ویرایش محصول";
                        break;
                    case 20072:
                        Message = "خطا در نوع محصول";
                        break;
                    case 20082:
                        {
                            Message = "شما قادر به حذف این محصول نیستید";
                            break;
                        }
                    case 20092:
                        {
                            Message = "شما قادر به حذف این عکس نیستید";
                            break;
                        }
                    case 20102:
                        {
                            Message = "شما قادر به ویرایش این عکس نیستید";
                            break;
                        }
                    case 20112:
                        {
                            Message = "شما قادر به ذخیره این عکس نیستید";
                            break;
                        }
                }
            }
            #endregion

            //قسمت مربوط به پیام های اطلاعاتی
            #region
            else if (MessageType == Components.MessageType.Info)
            {
                switch (id)
                {
                    case 333:
                        Message = "شما اجازه دسترسی به این بخش را ندارید";
                        break;
                    case 30013:
                        Message = "نام کاربری از قبل موجود میباشد";
                        break;
                    case 30003:
                        Message = "رکوردی برای حذف پیدا نشد";
                        break;
                }
            }
            #endregion
        }
    }

    public enum MessageType
    {
        Fail = 2, Success = 1, Info = 3, UnDefined = 0
    }
}
