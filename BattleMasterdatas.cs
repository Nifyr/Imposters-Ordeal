using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImpostersOrdeal.GlobalData;
using static ImpostersOrdeal.GameDataTypes;

namespace ImpostersOrdeal
{
    public static class BattleMasterdatas
	{
		public class MotionTimingData : ICloneable, IComparable<MotionTimingData>
		{
			public int MonsNo;
			public int FormNo;
			public int Sex;
			public int Buturi01;
			public int Buturi02;
			public int Buturi03;
			public int Tokusyu01;
			public int Tokusyu02;
			public int Tokusyu03;
			public int BodyBlow;
			public int Punch;
			public int Kick;
			public int Tail;
			public int Bite;
			public int Peck;
			public int Radial;
			public int Cry;
			public int Dust;
			public int Shot;
			public int Guard;
			public int LandingFall;
			public int LandingFallEase;
			public object Clone()
			{
				return this.MemberwiseClone();
			}

            public int CompareTo(MotionTimingData other)
            {
                int i = MonsNo.CompareTo(other.MonsNo);
				if (i == 0)
					i = FormNo.CompareTo(other.FormNo);
				if (i == 0)
					i = Sex.CompareTo(other.Sex);
				return i;
			}
        }
	}
}