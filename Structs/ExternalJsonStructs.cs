using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace ImpostersOrdeal
{
    public static class ExternalJsonStructs
    {
        public class Starter
        {
            public int monsNo;
            public int formNo;
            public int level;
            public int itemNo;
        }

        public class HoneyTreeZone
        {
            public List<HoneyTreeSlot> slots;
        }

        public class HoneyTreeSlot
        {
            public int maxlv;
            public int minlv;
            public int monsNo;
            public int formNo;

            public double GetAvgLevel()
            {
                return (minlv + maxlv) / 2.0;
            }
        }

        public class TMLearnset
        {
            public uint set01;
            public uint set02;
            public uint set03;
            public uint set04;
            public uint set05;
            public uint set06;
            public uint set07;
            public uint set08;

            public bool[] GetTMCompatibility()
            {
                bool[] tmCompatibility = new bool[256];
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i] = (set01 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 32] = (set02 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 64] = (set03 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 96] = (set04 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 128] = (set05 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 160] = (set06 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 192] = (set07 & ((uint)1 << i)) != 0;
                for (int i = 0; i < 32; i++)
                    tmCompatibility[i + 224] = (set08 & ((uint)1 << i)) != 0;
                return tmCompatibility;
            }

            public void SetTMCompatibility(bool[] tmCompatibility)
            {
                set01 = 0;
                for (int i = 0; i < 32; i++)
                    set01 |= tmCompatibility[i] ? (uint)1 << i : 0;
                set02 = 0;
                for (int i = 0; i < 32; i++)
                    set02 |= tmCompatibility[i + 32] ? (uint)1 << i : 0;
                set03 = 0;
                for (int i = 0; i < 32; i++)
                    set03 |= tmCompatibility[i + 64] ? (uint)1 << i : 0;
                set04 = 0;
                for (int i = 0; i < 32; i++)
                    set04 |= tmCompatibility[i + 96] ? (uint)1 << i : 0;
                set05 = 0;
                for (int i = 0; i < 32; i++)
                    set05 |= tmCompatibility[i + 128] ? (uint)1 << i : 0;
                set06 = 0;
                for (int i = 0; i < 32; i++)
                    set06 |= tmCompatibility[i + 160] ? (uint)1 << i : 0;
                set07 = 0;
                for (int i = 0; i < 32; i++)
                    set07 |= tmCompatibility[i + 192] ? (uint)1 << i : 0;
                set08 = 0;
                for (int i = 0; i < 32; i++)
                    set08 |= tmCompatibility[i + 224] ? (uint)1 << i : 0;
            }
        }
    }
}
