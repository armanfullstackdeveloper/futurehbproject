using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Boundary.com.arianpal.merchant;
using Boundary.Helper.StaticValue;
using BusinessLogic.Helpers;
using DataModel.Enums;

namespace Boundary.Helper
{
    public class HelperFunction : HelperFunctionInBL
    {
        public static string PaymentRequestResultMessage(byte code)
        {
            string message = string.Empty;

            switch (code)
            {
                case (byte)ResultValues.GatewayInvalidInfo:
                    message = "کاربر گرامی رمز عبور یا کد کاربری شما صحیح نیست";
                    break;
                case (byte)ResultValues.UserNotActive:
                    message = "کاربر غیر فعال است ";
                    break;
                case (byte)ResultValues.GatewayUnverify:
                    message = "کاربر گرامی درگاه شما غیر فعال می باشد";
                    break;
                case (byte)ResultValues.GatewayIsExpired:
                    message = "کاربر گرامی درگاه شما فاقداعتبار می باشد";
                    break;
                case (byte)ResultValues.GatewayIsBlocked:
                    message = "کاربر گرامی درگاه شما مسدود می باشد";
                    break;
                case (byte)ResultValues.Failed:
                    message = StaticString.Message_UnSuccessFull;
                    break;
                case (byte)ResultValues.Ready:
                    message = "هیچ عملیاتی انجام نشده است";
                    break;
                case (byte)ResultValues.InvalidServerIP:
                    message = "آی پی سرور نامعتبر است";
                    break;
                case (byte)EPaymentRequestResultValues.MinimumTransactionFailed:
                    message = string.Format("مبلق واریزی نمیتواند کمتر از {0} تومان باشد", StaticNemberic.MinimumTarakoneshValue);
                    break;
            }

            return message;
        }

        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public static void UpdateSiteMap(string link)
        {
            try
            {
                string address = System.Web.HttpContext.Current.Server.MapPath("~") + @"sitemap.xml";
                XDocument xDocument = XDocument.Load(address);
                XElement root = xDocument.Root;
                var lastRow = root.Elements().First();
                var xml = new XElement("url",
                    new XElement("loc", link),
                    new XElement("lastmod", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    new XElement("changefreq", "daily"),
                    new XElement("priority", "0.9"));
                lastRow.AddBeforeSelf(xml);
                var node = xDocument.Root.Elements().First();
                if (node.Name.NamespaceName == "")
                {
                    node.Attributes("xmlns").Remove();
                    node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                }
                foreach (XElement element in node.Elements())
                {
                    if (element.Name.NamespaceName == "")
                    {
                        element.Attributes("xmlns").Remove();
                        element.Name = element.Parent.Name.Namespace + element.Name.LocalName;
                    }
                }
                xDocument.Save(address
);
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}