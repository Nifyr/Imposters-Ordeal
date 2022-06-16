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
    public static class Masterdatas
    {
        public enum Size
        {
            XS = 0,
            S = 1,
            M = 2,
            L = 3,
            XL = 4,
        }

        public enum MoveType
        {
            GROUND = 0,
            FLY = 1,
            RUN_FLY = 2,
        }
        public class PokemonInfoCatalog : ICloneable
        {
            public int UniqueID;
            public int No;
            public int SinnohNo;
            public int MonsNo;
            public int FormNo;
            public byte Sex;
            public bool Rare;
            public string AssetBundleName;
            public float BattleScale;
            public float ContestScale;
            public Size ContestSize;
            public float FieldScale;
            public float FieldChikaScale;
            public float StatueScale;
            public float FieldWalkingScale;
            public float FieldFureaiScale;
            public float MenuScale;
            public string ModelMotion;
            public Vector3 ModelOffset;
            public Vector3 ModelRotationAngle;
            public float DistributionScale;
            public string DistributionModelMotion;
            public Vector3 DistributionModelOffset;
            public Vector3 DistributionModelRotationAngle;
            public float VoiceScale;
            public string VoiceModelMotion;
            public Vector3 VoiceModelOffset;
            public Vector3 VoiceModelRotationAngle;
            public Vector3 CenterPointOffset;
            public Vector2 RotationLimitAngle;
            public float StatusScale; 
            public string StatusModelMotion;
            public Vector3 StatusModelOffset;
            public Vector3 StatusModelRotationAngle;
            public float BoxScale;
            public string BoxModelMotion;
            public Vector3 BoxModelOffset;
            public Vector3 BoxModelRotationAngle;
            public float CompareScale;
            public string CompareModelMotion;
            public Vector3 CompareModelOffset;
            public Vector3 CompareModelRotationAngle;
            public float BrakeStart;
            public float BrakeEnd;
            public float WalkSpeed;
            public float RunSpeed;
            public float WalkStart;
            public float RunStart; 
            public float BodySize;
            public float AppearLimit;
            public MoveType MoveType; 
            public bool GroundEffect;
            public bool Waitmoving;
            public int BattleAjustHeight;
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
    }
}