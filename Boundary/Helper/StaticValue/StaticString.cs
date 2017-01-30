namespace Boundary.Helper.StaticValue 
{
    public class StaticString
    {
        #region message

        public static string Message_SuccessFull
        {
            get { return "عملیات با موفقیت انجام شد"; }
        }

        public static string Message_UnSuccessFull
        {
            get { return "عملیات با خطا مواجه شد"; }
        }

        public static string Message_WrongAccess
        {
            get { return "شما اجازه دسترسی به این بخش را ندارید"; }
        }

        public static string Message_InvalidInputs
        {
            get { return "ورودی(ها) نامعتبر می باشد"; }
        }

        public static string Message_SuspendedStoreStatus
        {
            get { return "وضعیت فروشگاه شما در حالت معلق قرار دارد، در این حالت امکان افزودن محصول جدید وجود ندارد"; }
        }

        #endregion


        #region action name

        public static string Action_Login
        {
            get { return "Login"; }
        }

        public static string Controller_ForLogin
        {
            get { return "Account"; }
        }

        public static string Action_Error
        {
            get { return "Error"; }
        }

        public static string Controller_ForError
        {
            get { return "Home"; }
        }

        public static string Action_OrderPaymentReturnUrl
        {
            get { return "Order/PaymentResult"; }
        }

        #endregion


        #region session name

        public static string Session_ShoppingBag { get { return "HBShoppingBag"; } }

        #endregion


        #region Role name

        public const string Role_SuperAdmin = "SuperAdmin";
        public const string Role_Admin ="Admin";
        public const string Role_Seller = "Seller";
        public const string Role_Member = "Member";

        #endregion

    }
}