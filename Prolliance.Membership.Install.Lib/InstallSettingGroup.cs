using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Install.Lib
{
    public class InstallSettingGroup
    {
        public InstallSettingGroup()
        {
            this.Items = new List<InstallSettingItem>();
        }

        public List<InstallSettingItem> Items { get; set; }

        public void AddItem(InstallSettingItem item)
        {
            this.Items.Remove(this.Items.FirstOrDefault(i => i.Name == item.Name));
            this.Items.Add(item);
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }
}
