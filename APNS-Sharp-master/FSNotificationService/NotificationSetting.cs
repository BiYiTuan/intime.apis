using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    class NotificationSetting : ConfigurationSection
    {
        public static NotificationSetting Current
        {
            get {
               return  (NotificationSetting)ConfigurationManager.GetSection("notificationSetting");
            }
        }
        public static NotificationElement CurrentElement
        {
            get {
                return (NotificationElement)Current.Notifications[Current.UseNotification];
            }
        }
        [ConfigurationProperty("useNotification")]
        public string UseNotification
        {
            get { return (String)base["useNotification"]; }
            set { base["useNotification"] = value; }
        }
        [ConfigurationProperty("defaultMessage")]
        public string DefaultMessage
        {
            get { return (String)base["defaultMessage"]; }
            set { base["defaultMessage"] = value; }
        }


        [ConfigurationProperty("concurrent")]
        public int Concurrent
        {
            get { return (int)base["concurrent"]; }
            set { base["concurrent"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public NotificationElementCollection Notifications
        {
            get { return (NotificationElementCollection)base[""]; }
        }
    }
    public class NotificationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new NotificationElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NotificationElement)element).Key;
        }
        public NotificationElement this[int index]
        {
            get { return (NotificationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new NotificationElement this[string name]
        {
            get { return (NotificationElement)BaseGet(name); }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemove(name);
                }
                BaseAdd(value);
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "notification";
            }
        }
    }

    public class NotificationElement : ConfigurationElement
    {

        [ConfigurationProperty("key", IsRequired = false, IsKey = true)]
        public string Key
        {
            get { return (String)base["key"]; }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("p12File", IsRequired = true)]
        public string P12File
        {
            get { return (String)base["p12File"]; }
            set { base["p12File"] = value; }
        }

        [ConfigurationProperty("p12Pass", IsRequired = false)]
        public string P12Pass
        {
            get { return (string)base["p12Pass"]; }
            set { base["p12Pass"] = value; }
        }

        [ConfigurationProperty("isSandBox", IsRequired = false)]
        public bool IsSandBox
        {
            get { return (bool)base["isSandBox"]; }
            set { base["isSandBox"] = value; }
        }

    }

}
