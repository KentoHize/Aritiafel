using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aritiafel.Items
{
    public class ArSettingGroup : SortedSet<ArSetting>
    {
        class CompareArSetting : IComparer<ArSetting>
        {
            public int Compare(ArSetting a, ArSetting b)
            {
                if (string.IsNullOrEmpty(a.Section) && string.IsNullOrEmpty(b.Section))
                    return a.Key.CompareTo(b.Key);
                else if (string.IsNullOrEmpty(a.Section) && !string.IsNullOrEmpty(b.Section))
                    return -1;
                else if (!string.IsNullOrEmpty(a.Section) && string.IsNullOrEmpty(b.Section))
                    return 1;

                int result = a.Section.CompareTo(b.Section);
                if (result != 0)
                    return result;
                return a.Key.CompareTo(b.Key);
            }
        }

        public ArSettingGroup()
            : base(new CompareArSetting())
        { }

        public void Add<T>(string key, T value, string section = null, string description = null)
        {
            ArSetting arSetting = new ArSetting(key, value, section, description);
            Add(arSetting);
        }

        public object GetValue(string key)
            => this.FirstOrDefault(m => m.Key == key).Value;

        public T GetValue<T>(string key)
            => (T)this.FirstOrDefault(m => m.Key == key).Value;

        public void SetValue(string key, object value)
        {
            this.FirstOrDefault(m => m.Key == key).Value = value;
        }

        public void Remove(string key)
        {
            foreach (ArSetting ars in this)
            {
                if (ars.Key == key)
                {
                    Remove(ars);
                    return;
                }
            }
            throw new KeyNotFoundException(key);
        }
    }
}
