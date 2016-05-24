using Amuse.Serializes;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TpNet;
using DAL = Prolliance.Membership.DataPersistence;

namespace Prolliance.Membership.Install.Lib
{
    internal class SettingContext
    {
        public SettingContext()
        {
            SettingGroups = new List<InstallSettingGroup>();
            Settings = new Dictionary<string, string>();
        }
        public List<InstallSettingGroup> SettingGroups { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
    public class InstallContext
    {
        private static readonly string CONFIG_FILE = "Amuse.config";
        private static readonly string LOG_FILE = "install.log";
        private static readonly string APP_KEY = AppSettings.Name;
        private static readonly string APP_NAME = AppSettings.Name;
        private static readonly string DEFAULT_ROLE_CODE = "Administrators";
        private static readonly string DEFAULT_ROLE_NAME = "管理员";
        private static readonly string DEFAULT_ORG_CODE = "default";
        private static readonly string DEFAULT_ORG_NAME = "默认组织";

        private static SettingContext settingContext = new SettingContext();
        private static JsonSerializer serializer = new JsonSerializer();
        public static string AppPhysicsPath { get; set; }

        public static List<InstallSettingGroup> SettingGroups
        {
            get { return settingContext.SettingGroups; }
            set { settingContext.SettingGroups = value; }
        }
        public static Dictionary<string, string> Settings
        {
            get { return settingContext.Settings; }
            set { settingContext.Settings = value; }
        }

        public static void AddGroup(InstallSettingGroup group)
        {
            SettingGroups.Remove(SettingGroups.FirstOrDefault(g => g.Name == group.Name));
            SettingGroups.Add(group);
        }

        public static void Clear()
        {
            SettingGroups.Clear();
            Settings.Clear();
        }

        private static string InstallLogFile
        {
            get
            {
                return string.Format("{0}\\bin\\{1}", AppPhysicsPath, LOG_FILE);
            }
        }

        private static bool? _Installed = null;
        public static bool Installed
        {
            get
            {
                if (_Installed == null)
                    _Installed = File.Exists(InstallLogFile);
                return Convert.ToBoolean(_Installed);
            }
            set
            {
                _Installed = value;
            }
        }
        public static void Save()
        {
            string configFile = string.Format("{0}\\{1}", AppPhysicsPath, CONFIG_FILE);
            string templateBuffer = ResHelper.ReadTextFromRes(typeof(InstallContext).Assembly, "Prolliance.Membership.Install.Lib.Amuse.tmpl");
            string configBuffer = Tp.Parse(templateBuffer, settingContext);
            File.WriteAllText(configFile, configBuffer);
            WriteInstallLog(serializer.Serialize(settingContext));
        }
        private static void WriteInstallLog(string log)
        {
            File.WriteAllText(InstallLogFile, log);
        }
        private static string ReadInstallLog()
        {
            return File.ReadAllText(InstallLogFile);
        }
        public static void CreateDataRepo()
        {
            DAL.DataRepoController.CreateDataRepo();
        }
        public static void InitData(string manifest)
        {
            settingContext = serializer.Deserialize<SettingContext>(ReadInstallLog());
            //添加自身应用
            App app = App.GetApp(APP_KEY);
            if (app == null)
            {
                app = App.Create();
                app.Key = APP_KEY;
                app.Name = APP_NAME;
            }
            app.IsActive = true;
            app.Save();
            //添加默认角色
            Role role = Role.GetRole(DEFAULT_ROLE_CODE);
            if (role == null)
            {
                role = Role.Create();
                role.Code = DEFAULT_ROLE_CODE;
                role.Name = DEFAULT_ROLE_NAME;
            }
            role.IsActive = true;
            role.Save();
            //添加默认用户
            User user = User.GetUser(Settings["account"]);
            if (user == null)
            {
                user = User.Create();
                user.Account = Settings["account"];
                user.Name = user.Account;
            }
            user.IsActive = true;
            user.Save();
            user.GiveRole(role);
            try
            {
                user.SetPassword(Settings["password"]);
            }
            catch { }
            //添加默认组织
            Organization org = Organization.GetOrganization(DEFAULT_ORG_CODE);
            if (org == null)
            {
                org = Organization.Create();
                org.Code = DEFAULT_ORG_CODE;
                org.Name = DEFAULT_ORG_NAME;
            }
            org.IsActive = true;
            org.Save();
            org.AddUser(user);
            //导入初始权限清单
            app.ImportManifestText(manifest);
            //将初始权限授给默认角色
            List<Operation> operationList = Operation.GetOperationList()
                .Where(op => op.AppId == app.Id)
                .ToList();
            foreach (Operation operation in operationList)
            {
                role.GivePermission(operation);
            }
            //写入安装日志
            WriteInstallLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
