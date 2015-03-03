using CLAP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.console.onetime
{
    class Program
    {
        static void Main(string[] args)
        {
            UnityBootStrapper.Init();
            Parser.RunConsole<OneTimeCommand>(args);
        }
    }
}
