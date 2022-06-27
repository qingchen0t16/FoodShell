using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enum
{
    public enum CardBarStatu
    {
        Feasible,   // 可种植
        NeedCD, // 还在CD
        NeedFire,       // 火苗不足
        Disabled    // 两个都不充足，禁用
    }
}
