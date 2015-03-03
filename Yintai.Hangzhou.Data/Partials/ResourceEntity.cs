using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class ResourceEntity
    {
         public string AbsoluteUrl {
            get {
                if (string.IsNullOrEmpty(Name))
                    return string.Empty;
                return Path.Combine(Domain, Name);
            }
        }
       
      
      
    }
}
