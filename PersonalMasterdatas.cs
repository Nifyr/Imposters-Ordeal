using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImpostersOrdeal.GlobalData;
using static ImpostersOrdeal.GameDataTypes;
using System.Numerics;

namespace ImpostersOrdeal
{
    static public class PersonalMasterdatas
    {
        public class AddPersonalTable : ICloneable
        {
            public bool valid_flag;
            public ushort monsno;
            public ushort formno;
            public bool isEnableSynchronize;
            public byte escape;
            public bool isDisableReverce;
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        };
    }
}