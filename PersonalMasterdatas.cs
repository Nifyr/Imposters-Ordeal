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
        public class AddPersonalTable : ICloneable, IComparable<AddPersonalTable>
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

            public int CompareTo(AddPersonalTable other)
            {
                if (formno.CompareTo(other.formno) != 0)
                {
                    if (formno == 0)
                        return -1;
                    if (other.formno == 0)
                        return 1;
                }

                int i = monsno.CompareTo(other.monsno);
                if (i == 0)
                    i = formno.CompareTo(other.formno);
                return i;
            }
        }
    }
}