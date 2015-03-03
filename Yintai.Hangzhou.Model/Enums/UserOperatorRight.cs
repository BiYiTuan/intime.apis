using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    [Flags]
    public enum UserOperatorRight
    {
        GiftCard = 1,
        SystemProduct = 2,
        SelfProduct = 4
    }
}
