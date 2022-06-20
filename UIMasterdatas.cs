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
	public static class UIMasterdatas
	{
		public class PokemonIcon : ICloneable, IComparable<PokemonIcon>
		{
			public int UniqueID;
			public string AssetBundleName;
			public string AssetName;
			public string AssetBundleNameLarge;
			public string AssetNameLarge;
			public string AssetBundleNameDP;
			public string AssetNameDP;
			public Vector2 HallofFameOffset;

            public object Clone()
            {
				return this.MemberwiseClone();
            }

            public int CompareTo(PokemonIcon other)
            {
                return UniqueID.CompareTo(other.UniqueID);
            }
        }

		public class AshiatoIcon : ICloneable, IComparable<AshiatoIcon>
		{
			public int UniqueID;
			public string SideIconAssetName;
			public string BothIconAssetName;

            public object Clone()
            {
				return this.MemberwiseClone();
			}

			public int CompareTo(AshiatoIcon other)
			{
				return UniqueID.CompareTo(other.UniqueID);
			}
		}

		public class PokemonVoice : ICloneable, IComparable<PokemonVoice>
		{
			public int UniqueID; 
			public string WwiseEvent;
			public string stopEventId;
			public Vector3 CenterPointOffset;
			public bool RotationLimits;
			public Vector2 RotationLimitAngle;

            public object Clone()
            {
				return this.MemberwiseClone();
			}

			public int CompareTo(PokemonVoice other)
			{
				return UniqueID.CompareTo(other.UniqueID);
			}
		}

		public class ZukanDisplay : ICloneable, IComparable<ZukanDisplay>
		{
			public int UniqueID;
			public Vector3 MoveLimit;
			public Vector3 ModelOffset;
			public Vector2 ModelRotationAngle;

            public object Clone()
            {
				return this.MemberwiseClone();
			}

			public int CompareTo(ZukanDisplay other)
			{
				return UniqueID.CompareTo(other.UniqueID);
			}
		}
		public class ZukanCompareHeight : ICloneable, IComparable<ZukanCompareHeight>
		{
			public int UniqueID;
			public float PlayerScaleFactor;
			public Vector3 PlayerOffset; 
			public Vector2 PlayerRotationAngle;

            public object Clone()
            {
				return this.MemberwiseClone();
			}

			public int CompareTo(ZukanCompareHeight other)
			{
				return UniqueID.CompareTo(other.UniqueID);
			}
		}
	}
}