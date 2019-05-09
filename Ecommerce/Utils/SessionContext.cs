using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Ecommerce.Utils
{
    public static class SessionContext
    {
        public static void SetAuthenticationToken(bool isPersistant, User userData)
        {
            string data = null;
            if (userData != null)
                data = new JavaScriptSerializer().Serialize(new User {
                    UserID = userData.UserID,
                    Email = userData.Email,
                    Name = userData.Name,
                    Roles = userData.Roles
                });

            //Create Form Authentication ticket
            //Parameter(1) = Ticket version
            //Parameter(2) = User ID
            //Parameter(3) = Ticket Current Date and Time
            //Parameter(4) = Ticket Expiry
            //Parameter(5) = Remember me check
            //Parameter(6) = User Data
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, userData.UserID.ToString(), DateTime.Now, DateTime.Now.AddHours(2), isPersistant, data);

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static User GetUserData()
        {
            User userData = null;

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    userData = new JavaScriptSerializer().Deserialize(ticket.UserData, typeof(User)) as User;
                }
            }
            catch (Exception)
            {
            }

            return userData;
        }
    }

}