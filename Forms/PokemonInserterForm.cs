using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class PokemonInserterForm : Form
    {
        private List<DexEntry> dexEntries;
        private DexEntry d;
        private InserterMode inserterMode;

        private Dictionary<int, int> formCounts = new();
        private List<List<GenderConfig>> genderConfigs;

        private enum GenderConfig
        {
            Male, Female, Genderless, Normal, Variants
        }

        private enum InserterMode
        {
            Form, Species
        }

        public PokemonInserterForm()
        {
            for (int i = 0; i < 899; i++)
                formCounts[i] = 1;
            formCounts[3] = 3;
            formCounts[6] = 4;
            formCounts[9] = 3;
            formCounts[12] = 2;
            formCounts[15] = 2;
            formCounts[18] = 2;
            formCounts[19] = 2;
            formCounts[20] = 3;
            formCounts[25] = 17;
            formCounts[26] = 2;
            formCounts[27] = 2;
            formCounts[28] = 2;
            formCounts[37] = 2;
            formCounts[38] = 2;
            formCounts[50] = 2;
            formCounts[51] = 2;
            formCounts[52] = 4;
            formCounts[53] = 2;
            formCounts[65] = 2;
            formCounts[68] = 2;
            formCounts[74] = 2;
            formCounts[75] = 2;
            formCounts[76] = 2;
            formCounts[77] = 2;
            formCounts[78] = 2;
            formCounts[79] = 2;
            formCounts[80] = 3;
            formCounts[83] = 2;
            formCounts[88] = 2;
            formCounts[89] = 2;
            formCounts[94] = 3;
            formCounts[99] = 2;
            formCounts[103] = 2;
            formCounts[105] = 3;
            formCounts[110] = 2;
            formCounts[115] = 2;
            formCounts[122] = 2;
            formCounts[127] = 2;
            formCounts[130] = 2;
            formCounts[131] = 2;
            formCounts[133] = 3;
            formCounts[142] = 2;
            formCounts[143] = 2;
            formCounts[144] = 2;
            formCounts[145] = 2;
            formCounts[146] = 2;
            formCounts[150] = 3;
            formCounts[172] = 2;
            formCounts[181] = 2;
            formCounts[199] = 2;
            formCounts[201] = 28;
            formCounts[208] = 2;
            formCounts[212] = 2;
            formCounts[214] = 2;
            formCounts[222] = 2;
            formCounts[229] = 2;
            formCounts[248] = 2;
            formCounts[254] = 2;
            formCounts[257] = 2;
            formCounts[260] = 2;
            formCounts[263] = 2;
            formCounts[264] = 2;
            formCounts[282] = 2;
            formCounts[302] = 2;
            formCounts[303] = 2;
            formCounts[306] = 2;
            formCounts[308] = 2;
            formCounts[310] = 2;
            formCounts[319] = 2;
            formCounts[323] = 2;
            formCounts[334] = 2;
            formCounts[351] = 4;
            formCounts[354] = 2;
            formCounts[359] = 2;
            formCounts[362] = 2;
            formCounts[373] = 2;
            formCounts[376] = 2;
            formCounts[380] = 2;
            formCounts[381] = 2;
            formCounts[382] = 2;
            formCounts[383] = 2;
            formCounts[384] = 2;
            formCounts[386] = 4;
            formCounts[412] = 3;
            formCounts[413] = 3;
            formCounts[421] = 2;
            formCounts[422] = 2;
            formCounts[423] = 2;
            formCounts[428] = 2;
            formCounts[445] = 2;
            formCounts[448] = 2;
            formCounts[460] = 2;
            formCounts[475] = 2;
            formCounts[479] = 6;
            formCounts[487] = 2;
            formCounts[492] = 2;
            formCounts[493] = 18;
            formCounts[531] = 2;
            formCounts[550] = 2;
            formCounts[554] = 2;
            formCounts[555] = 4;
            formCounts[562] = 2;
            formCounts[569] = 2;
            formCounts[585] = 4;
            formCounts[586] = 4;
            formCounts[618] = 2;
            formCounts[641] = 2;
            formCounts[642] = 2;
            formCounts[645] = 2;
            formCounts[646] = 3;
            formCounts[647] = 2;
            formCounts[648] = 2;
            formCounts[649] = 5;
            formCounts[658] = 3;
            formCounts[666] = 20;
            formCounts[669] = 5;
            formCounts[670] = 6;
            formCounts[671] = 5;
            formCounts[676] = 10;
            formCounts[678] = 2;
            formCounts[681] = 2;
            formCounts[710] = 4;
            formCounts[711] = 4;
            formCounts[716] = 2;
            formCounts[718] = 5;
            formCounts[719] = 2;
            formCounts[720] = 2;
            formCounts[735] = 2;
            formCounts[738] = 2;
            formCounts[741] = 4;
            formCounts[743] = 2;
            formCounts[744] = 2;
            formCounts[745] = 3;
            formCounts[746] = 2;
            formCounts[752] = 2;
            formCounts[754] = 2;
            formCounts[758] = 2;
            formCounts[773] = 18;
            formCounts[774] = 14;
            formCounts[777] = 2;
            formCounts[778] = 4;
            formCounts[784] = 2;
            formCounts[800] = 4;
            formCounts[801] = 2;
            formCounts[809] = 2;
            formCounts[812] = 2;
            formCounts[815] = 2;
            formCounts[818] = 2;
            formCounts[823] = 2;
            formCounts[826] = 2;
            formCounts[834] = 2;
            formCounts[839] = 2;
            formCounts[841] = 2;
            formCounts[842] = 2;
            formCounts[844] = 2;
            formCounts[845] = 3;
            formCounts[849] = 4;
            formCounts[851] = 2;
            formCounts[854] = 2;
            formCounts[855] = 2;
            formCounts[858] = 2;
            formCounts[861] = 2;
            formCounts[869] = 64;
            formCounts[875] = 2;
            formCounts[876] = 2;
            formCounts[877] = 2;
            formCounts[879] = 2;
            formCounts[884] = 2;
            formCounts[888] = 2;
            formCounts[889] = 2;
            formCounts[890] = 2;
            formCounts[892] = 4;
            formCounts[893] = 2;
            formCounts[898] = 3;

            dexEntries = gameData.dexEntries;
            inserterMode = InserterMode.Form;

            string output = "genderConfigs = new()\n{\n";
            for (int dexID = 0; dexID < dexEntries.Count; dexID++)
            {
                output += "new() { ";
                for (int formID = 0; formID < dexEntries[dexID].forms.Count; formID++)
                {
                    output += "GenderConfig.";
                    IEnumerable<Masterdatas.PokemonInfoCatalog> pis = gameData.pokemonInfos.Where(pi => pi.MonsNo == dexID && pi.FormNo == formID && !pi.Rare);
                    if (pis.All(pi => pi.Sex == 0))
                        output += "Male";
                    else if (pis.All(pi => pi.Sex == 1))
                        output += "Female";
                    else if (pis.All(pi => pi.Sex == 2))
                        output += "Genderless";
                    else if (pis.Count() == 2 && pis.ElementAt(0).AssetBundleName == pis.ElementAt(1).AssetBundleName)
                        output += "Normal";
                    else if (pis.Count() == 2 && pis.ElementAt(0).AssetBundleName != pis.ElementAt(1).AssetBundleName)
                        output += "Variants";
                    else
                        throw new NotImplementedException();
                    if (formID != dexEntries[dexID].forms.Count - 1)
                        output += ",";
                    output += " ";
                }
                output += "}";
                if (dexID != dexEntries.Count - 1)
                    output += ",";
                output += "\n";
            }
            output += "};";
        

            genderConfigs = new()
{
new() { GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Male, GenderConfig.Male, GenderConfig.Male, GenderConfig.Male, GenderConfig.Male, GenderConfig.Male, GenderConfig.Male, GenderConfig.Male, GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Male },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Variants, GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Male },
new() { GenderConfig.Female },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Female, GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Male },
new() { GenderConfig.Variants },
new() { GenderConfig.Female },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants, GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Genderless },
new() { GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Female },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Variants },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Variants },
new() { GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Male, GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Female },
new() { GenderConfig.Female },
new() { GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Male },
new() { GenderConfig.Male },
new() { GenderConfig.Male, GenderConfig.Male },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Female },
new() { GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female, GenderConfig.Female },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Male, GenderConfig.Female },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Normal },
new() { GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal, GenderConfig.Normal },
new() { GenderConfig.Genderless, GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless },
new() { GenderConfig.Genderless, GenderConfig.Genderless, GenderConfig.Genderless }
            };

            InitializeComponent();

            d = dexEntries[0];
            dexIDComboBox.DataSource = dexEntries.Select(o => o.GetName()).ToArray();
            dexIDComboBox.SelectedIndex = 0;

            newFormRadioButton.Checked = true;
            label2.Text = "Pokémon " + dexEntries.Count + " Name:";

            RefreshDexEntryDisplay();
            RefreshModeDisplay();
            ActivateControls();
        }

        private void DexEntryChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            d = dexEntries[dexIDComboBox.SelectedIndex];
            RefreshDexEntryDisplay();

            ActivateControls();
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            inserterMode = newFormRadioButton.Checked ? InserterMode.Form : InserterMode.Species;
            RefreshModeDisplay();

            ActivateControls();
        }

        private void RefreshDexEntryDisplay()
        {
            formIDComboBox.DataSource = d.forms.Select((o, i) => i).ToArray();
            formIDComboBox.SelectedIndex = 0;
            label3.Text = inserterMode == InserterMode.Form ? "Form " + d.forms.Count + " Name:" : "Form 0 Name:";
        }

        private void RefreshModeDisplay()
        {
            speciesNameTextBox.Enabled = inserterMode == InserterMode.Species;
            label2.Enabled = inserterMode == InserterMode.Species;
            label3.Text = inserterMode == InserterMode.Form ? "Form " + d.forms.Count + " Name:" : "Form 0 Name:";
            if (inserterMode == InserterMode.Species)
                formNameTextBox.Text = "";
            else
                formNameTextBox.Text = "New Form";
        }

        private void ActivateControls()
        {
            dexIDComboBox.SelectedIndexChanged += DexEntryChanged;
            newFormRadioButton.CheckedChanged += ModeChanged;
            newSpeciesRadioButton.CheckedChanged += ModeChanged;
        }

        private void DeactivateControls()
        {
            dexIDComboBox.SelectedIndexChanged -= DexEntryChanged;
            newFormRadioButton.CheckedChanged -= ModeChanged;
            newSpeciesRadioButton.CheckedChanged -= ModeChanged;
        }

        private void AddFormClick(object sender, EventArgs e)
        {
            for (int dexID = 0; dexID < 899; dexID++)
            {
                if (dexEntries.Count == dexID)
                {
                    int srcDexID = 0;
                    int srcFormID = 0;
                    switch (genderConfigs[dexID][0])
                    {
                        case GenderConfig.Male:
                            srcDexID = 475;
                            srcFormID = 0;
                            break;
                        case GenderConfig.Female:
                            srcDexID = 242;
                            srcFormID = 0;
                            break;
                        case GenderConfig.Genderless:
                            srcDexID = 492;
                            srcFormID = 0;
                            break;
                        case GenderConfig.Normal:
                            srcDexID = 395;
                            srcFormID = 0;
                            break;
                        case GenderConfig.Variants:
                            srcDexID = 3;
                            srcFormID = 0;
                            break;
                    }
                    PokemonInserter.GetInstance().InsertPokemon(srcDexID, dexID, srcFormID, 0, "Ditto" + dexID, "Form0");
                }
                for (int formID = 0; formID < formCounts[dexID]; formID++)
                    if (dexEntries[dexID].forms.Count == formID)
                    {
                        int srcDexID = 0;
                        int srcFormID = 0;
                        switch (genderConfigs[dexID][formID])
                        {
                            case GenderConfig.Male:
                                srcDexID = 475;
                                srcFormID = 0;
                                break;
                            case GenderConfig.Female:
                                srcDexID = 242;
                                srcFormID = 0;
                                break;
                            case GenderConfig.Genderless:
                                srcDexID = 492;
                                srcFormID = 0;
                                break;
                            case GenderConfig.Normal:
                                srcDexID = 395;
                                srcFormID = 0;
                                break;
                            case GenderConfig.Variants:
                                srcDexID = 3;
                                srcFormID = 0;
                                break;
                        }
                        PokemonInserter.GetInstance().InsertPokemon(srcDexID, dexID, srcFormID, formID, "Ditto" + dexID, "Form" + formID);
                    }
            }
        }
    }
}
