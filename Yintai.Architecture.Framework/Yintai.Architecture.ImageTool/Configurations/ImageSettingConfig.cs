using System;
using System.Configuration;
using Yintai.Architecture.ImageTool.Core;

namespace Yintai.Architecture.ImageTool.Configurations
{
    public class ImageSettingConfig : ConfigurationSection
    {
        [ConfigurationProperty("folder")]
        public string Folder
        {
            get { return (String)base["folder"]; }
            set { base["folder"] = value; }
        }

        [ConfigurationProperty("backupFolder")]
        public string BackupFolder
        {
            get { return (String)base["backupFolder"]; }
            set { base["backupFolder"] = value; }
        }

        [ConfigurationProperty("imageQuality")]
        public int ImageQuality
        {
            get { return (int)base["imageQuality"]; }
            set { base["imageQuality"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ImageElementCollection ImageCollection
        {
            get { return (ImageElementCollection)base[""]; }
        }
    }

    #region 上传图片配置信息

    public class ImageElement : ConfigurationElement
    {

        [ConfigurationProperty("key", IsRequired = false, IsKey = true)]
        public string Key
        {
            get { return (String)base["key"]; }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("suffix", IsRequired = false)]
        public string Suffix
        {
            get { return (String)base["suffix"]; }
            set { base["suffix"] = value; }
        }

        [ConfigurationProperty("size", IsRequired = false)]
        public int Size
        {
            get { return (Int32)base["size"]; }
            set { base["size"] = value; }
        }

        [ConfigurationProperty("folder", IsRequired = false)]
        public string Folder
        {
            get { return (String)base["folder"]; }
            set { base["folder"] = value; }
        }

        [ConfigurationProperty("rootFolder", IsRequired = false)]
        public string RootFolder
        {
            get { return (String)base["rootFolder"]; }
            set { base["rootFolder"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = false)]
        public int Type
        {
            get { return (Int32)base["type"]; }
            set { base["type"] = value; }
        }


        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ThumbElementCollection ThumbCollection
        {
            get { return (ThumbElementCollection)base[""]; }
        }

        [ConfigurationProperty("asNormalFile", DefaultValue = false)]
        public bool AsNormalFile
        {
            get
            {
                return (bool)base["asNormalFile"];
            }
            set
            {
                base["asNormalFile"] = value;
            }
        }
    }

    public class ImageElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ImageElement)element).Key;
        }
        public ImageElement this[int index]
        {
            get { return (ImageElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ImageElement this[string name]
        {
            get { return (ImageElement)BaseGet(name); }
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
                return "image";
            }
        }
    }

    #endregion

    #region 缩略图配置信息

    public class ThumbElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = false, IsKey = true)]
        public string Key
        {
            get { return (String)base["key"]; }
            set { base["key"] = value; }
        }
        /// <summary>
        /// 缩略图生成模式
        /// </summary>
        [ConfigurationProperty("mode", IsRequired = false)]
        public ThumbMode Mode
        {
            get { return (ThumbMode)base["mode"]; }
            set { base["mode"] = value; }
        }
        /// <summary>
        /// 缩略图的宽
        /// </summary>
        [ConfigurationProperty("width", IsRequired = false)]
        public int Width
        {
            get { return (Int32)base["width"]; }
            set { base["width"] = value; }
        }
        /// <summary>
        /// 缩略图的高
        /// </summary>
        [ConfigurationProperty("height", IsRequired = false)]
        public int Height
        {
            get { return (Int32)base["height"]; }
            set { base["height"] = value; }
        }
    }

    public class ThumbElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ThumbElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ThumbElement)element).Key;
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
                return "thumb";
            }
        }

        public ThumbElement this[int index]
        {
            get { return (ThumbElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new ThumbElement this[string name]
        {
            get { return (ThumbElement)BaseGet(name); }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemove(name);
                }
                BaseAdd(value);
            }
        }
    }

    #endregion
}
