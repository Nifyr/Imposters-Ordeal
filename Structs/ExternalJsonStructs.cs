using System.Collections.Generic;

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
    }
}
