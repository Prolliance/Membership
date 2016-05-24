using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Prolliance.Membership.Common
{
    public static class AppSettings
    {
        private const string SERVER_PATH_PLACEHOLDER = "{server-path}";
        private static NameValueCollection _Settings = ConfigurationManager.AppSettings;
        public static string ServerPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        public static string LogPath
        {
            get
            {
                return _Settings["log-path"].Replace(SERVER_PATH_PLACEHOLDER, ServerPath);
            }
        }
        public static int TokenTimeout
        {
            get
            {
                return Convert.ToInt32(_Settings["token-timeout"]);
            }
        }
        public static string Name
        {
            get
            {
                return _Settings["name"];
            }
        }

        public static string Email
        {
            get
            {
                return _Settings["email"];
            }
        }

        public static string ExtensionModel
        {
            get
            {
                return _Settings["ExtensionModel"];
            }
        }

        public static List<string> UserInfoBase
        {
            get
            {
                return _Settings["UserInfoBase"].Split(',').Where(p=>!string.IsNullOrEmpty(p)).ToList();
            }
        }

        public static List<string> OrgInfoBase
        {
            get
            {
                return _Settings["OrgInfoBase"].Split(',').Where(p => !string.IsNullOrEmpty(p)).ToList();
            }
        }

        public static List<string> MembershipNodes
        {
            get
            {
                return _Settings["MembershipNodes"].Split(',').Where(p => !string.IsNullOrEmpty(p)).ToList();
            }
        } 
    }
}
