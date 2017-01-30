using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Boundary.Controllers;
using Boundary.Controllers.Ordinary;
using Boundary.Helper;
using Boundary.Helper.StaticValue;
using BusinessLogic.BussinesLogics;
using BusinessLogic.BussinesLogics.RelatedToStoreBL;
using DataModel.Entities;
using DataModel.Enums;
using DataModel.Models.DataModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace Boundary.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId, Func<UserManager<AppUser>> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ShopFinderUserManager shopFinderUserManager = new ShopFinderUserManager();
            AppUser user = await shopFinderUserManager.FindAsync(context.UserName, context.Password);
            if (user != null && !user.IsActive)
            {
                context.SetError("invalid_grant", "وضعیت کاربری شما غیر فعال میباشد");
                return;
            }
            if (user == null)
            {
                context.SetError("invalid_grant", "نام کاربری یا رمز عبور اشتباه است");
                return;
            }

            user.Role=new RoleBL().SelectOne((int)user.RoleCode);
            //admin ha ba mobile kari nadarand!
            if (user.Role == null || user.Role.Id <= 0 || user.RoleCode==ERole.Admin || user.RoleCode==ERole.SuperAdmin)
            {
                context.SetError(StaticString.Message_UnSuccessFull);
                return;
            }            

            ClaimsIdentity oAuthIdentity = await shopFinderUserManager.CreateIdentityAsync(user,
                context.Options.AuthenticationType);

            #region add information to user's role

            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name ?? string.Empty));

            #endregion

            ClaimsIdentity cookiesIdentity = await shopFinderUserManager.CreateIdentityAsync(user,
                CookieAuthenticationDefaults.AuthenticationType);
            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);

            //myCode
            ticket.Properties.Dictionary.Add("Role", user.Role.Name);
            if (user.Role.Name == StaticString.Role_Seller)
            {
                StoreSessionDataModel store = new StoreBL().GetSummaryForSession(user.Id);
                if (store != null)
                {
                    ticket.Properties.Dictionary.Add("StoreCode", store.StoreCode.ToString());
                    ticket.Properties.Dictionary.Add("Status", store.Status.ToString());
                }
            }
            else if (user.Role.Name == StaticString.Role_Member)
            {
                Member member = new MemberBL().GetSummaryForSession(user.Id);
                if (member != null)
                {
                    ticket.Properties.Dictionary.Add("MemberCode", member.Id.ToString());
                }
            }

            context.Request.Context.Authentication.SignIn(cookiesIdentity);   
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }

}