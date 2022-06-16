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
		public class PokemonIcon : ICloneable
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
        }

		public class AshiatoIcon : ICloneable
		{
			public int UniqueID;
			public string SideIconAssetName;
			public string BothIconAssetName;

            public object Clone()
            {
				return this.MemberwiseClone();
			}
        }

		public class PokemonVoice : ICloneable
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
        }

		public class ZukanDisplay : ICloneable
		{
			public int UniqueID;
			public Vector3 MoveLimit;
			public Vector3 ModelOffset;
			public Vector2 ModelRotationAngle;

            public object Clone()
            {
				return this.MemberwiseClone();
			}
        }
		public class ZukanCompareHeight : ICloneable
		{
			public int UniqueID;
			public float PlayerScaleFactor;
			public Vector3 PlayerOffset; 
			public Vector2 PlayerRotationAngle;

            public object Clone()
            {
				return this.MemberwiseClone();
			}
        }
	}
}