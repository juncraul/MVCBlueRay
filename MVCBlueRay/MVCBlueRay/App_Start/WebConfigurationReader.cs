using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MVCBlueRay.App_Start
{
    public static class WebConfigurationReader
    {
        public static string EmailToSendFrom
        {
            get
            {
                return WebConfigurationManager.AppSettings["EmailToSendFrom"] ?? "";
            }
        }

        public static string EmailToSendFromPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["EmailToSendFromPassword"] ?? "";
            }
        }
        public static string TokenTypePasswordReset
        {
            get
            {
                return WebConfigurationManager.AppSettings["TokenTypePasswordReset"] ?? "";
            }
        }
    }
}