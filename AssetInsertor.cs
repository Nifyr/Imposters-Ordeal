using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.Distributions;

namespace ImpostersOrdeal
{
    class AssetInsertor
    {
        enum AssetClassID
        {
            GameObject = 1,
            Material = 21,
            Texture2D = 28,
            Mesh = 43,
            AnimationClip = 74,
            AssetBundle = 142,
        }

        class InsertRequest
        {
            public int baseMonsNo;
            public int monsNo;
            public int baseFormNo;
            public int formNo;
            public int gender;
        };

        private const String basePath = "\\Pokemon Database\\pokemons\\";

        private static AssetInsertor instance;
        private Random rnd;
        private Dictionary<String, String> CABNames;
        private List<InsertRequest> insertRequests;
        private AssetInsertor()
        {
            rnd = new();
            CABNames = new();
            insertRequests = new();
        }
        public static AssetInsertor getInstance()
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
        public string genCABName()
        {
            string[] values = new string[4];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = rnd.Next().ToString("x2");
            }

            return String.Join("", values);
        }

        public int genUniqueID(int monsNo, int formNo, int gender, bool isRare)
        {
            int uniqueID = (monsNo * 10000) + (formNo * 100) + (gender * 10);
            if (isRare)
            {
                uniqueID += 1;
            }

            return uniqueID;
        }

        public void InsertPokemon(int baseMonsNo, int monsNo, int baseFormNo, int formNo, int gender, string formName)
        {
            UpdateUIMasterdatas(baseMonsNo, monsNo, baseFormNo, formNo, gender);
            UpdateAddPersonalTable(baseMonsNo, monsNo, baseFormNo, formNo);
            UpdateMotionTimingData(baseMonsNo, monsNo, baseFormNo, formNo);
            UpdatePokemonInfos(baseMonsNo, monsNo, baseFormNo, formNo, gender);
            UpdateCommonMsbt(baseMonsNo, monsNo, baseFormNo, formNo, formName);
            UpdatePersonalInfos(baseMonsNo, monsNo, baseFormNo, formNo);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.UIMasterdatas);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.AddPersonalTable);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.MotionTimingData);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.PokemonInfo);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.MessageFileSets);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.PersonalEntries);

            InsertRequest insertRequest = new();
            insertRequest.baseMonsNo = baseMonsNo;
            insertRequest.monsNo = monsNo;
            insertRequest.baseFormNo = baseFormNo;
            insertRequest.formNo = formNo;
            insertRequest.gender = gender;
            insertRequests.Add(insertRequest);
        }

        public void processRequests()
        {
            String baseDirectory = Environment.CurrentDirectory + "\\" + FileManager.outputModName + "\\romfs\\Data\\StreamingAssets\\AssetAssistant\\";
            Directory.CreateDirectory(baseDirectory + "Pokemon Database\\pokemons\\common");
            Directory.CreateDirectory(baseDirectory + "Pokemon Database\\pokemons\\battle\\animations");
            Directory.CreateDirectory(baseDirectory + "Pokemon Database\\pokemons\\field\\animations");
            foreach (InsertRequest insertRequest in insertRequests)
            {
                DuplicateAssetBundles(insertRequest.baseMonsNo, insertRequest.monsNo, insertRequest.baseFormNo, insertRequest.formNo);
            }
        }

        public void UpdatePersonalInfos(int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            // Will only really work for adding a new form. Not a new pokemon based on another one
            Pokemon basePokemon = null;
            foreach (Pokemon pokemon in GlobalData.gameData.personalEntries)
            {
                if (pokemon.dexID == baseMonsNo && pokemon.formIndex == baseFormNo)
                {
                    basePokemon = pokemon;
                }
                pokemon.pastPokemon = new();
                pokemon.nextPokemon = new();
                pokemon.inferiorForms = new();
                pokemon.superiorForms = new();
            }
            basePokemon.formMax = 0;
            basePokemon.formIndex = (ushort)GlobalData.gameData.personalEntries.Count;

            Pokemon newPokemon = (Pokemon) basePokemon.Clone();
            newPokemon.personalID = (ushort) GlobalData.gameData.personalEntries.Count;
            newPokemon.dexID = (ushort) monsNo;
            newPokemon.formID = formNo;

            GlobalData.gameData.dexEntries[basePokemon.dexID].forms.Add(newPokemon);
            GlobalData.gameData.personalEntries.Add(newPokemon);
            DataParser.SetFamilies();
        }
        public void UpdateCommonMsbt(int baseMonsNo, int monsNo, int baseFormNo, int formNo, String formName)
        {
            foreach (MessageFileSet msgFileSet in GlobalData.gameData.messageFileSets)
            {
                foreach (MessageFile msgFile in msgFileSet.messageFiles)
                {
                    Console.WriteLine("msgFile");
                    if (msgFile.mName.Equals("english_ss_zkn_form"))
                    {
                        Console.WriteLine("english_ss_zkn_form");
                        LabelData baseLabelData = null;
                        foreach (LabelData labelData in msgFile.labelDatas)
                        {
                            String baseLabelName = string.Format("ZKN_FORM_{0}_{1}", baseMonsNo.ToString("D3"), baseFormNo.ToString("D3"));
                            if (labelData.labelName == baseLabelName)
                            {
                                baseLabelData = labelData;
                            }
                        }
                        LabelData newLabelData = (LabelData) baseLabelData.Clone();
                        newLabelData.labelName = string.Format("ZKN_FORM_{0}_{1}", monsNo.ToString("D3"), formNo.ToString("D3"));
                        newLabelData.labelIndex = msgFile.labelDatas.Count;
                        newLabelData.arrayIndex = msgFile.labelDatas.Count;
                        newLabelData.wordDatas[0].str = formName;

                        msgFile.labelDatas.Add(newLabelData);
                    }

                    if (msgFile.mName.Equals("english_ss_zkn_height"))
                    {
                        LabelData baseLabelData = null;
                        foreach (LabelData labelData in msgFile.labelDatas)
                        {
                            String baseLabelName = string.Format("ZKN_HEIGHT_{0}_{1}", baseMonsNo.ToString("D3"), baseFormNo.ToString("D3"));
                            if (labelData.labelName == baseLabelName)
                            {
                                baseLabelData = labelData;
                            }
                        }
                        LabelData newLabelData = (LabelData)baseLabelData.Clone();
                        newLabelData.labelName = string.Format("ZKN_HEIGHT_{0}_{1}", monsNo.ToString("D3"), formNo.ToString("D3"));
                        newLabelData.labelIndex = msgFile.labelDatas.Count;
                        newLabelData.arrayIndex = msgFile.labelDatas.Count;

                        msgFile.labelDatas.Add(newLabelData);
                    }

                    if (msgFile.mName.Equals("english_ss_zkn_weight"))
                    {
                        LabelData baseLabelData = null;
                        foreach (LabelData labelData in msgFile.labelDatas)
                        {
                            String baseLabelName = string.Format("ZKN_WEIGHT_{0}_{1}", baseMonsNo.ToString("D3"), baseFormNo.ToString("D3"));
                            if (labelData.labelName == baseLabelName)
                            {
                                baseLabelData = labelData;
                            }
                        }
                        LabelData newLabelData = (LabelData)baseLabelData.Clone();
                        newLabelData.labelName = string.Format("ZKN_WEIGHT_{0}_{1}", monsNo.ToString("D3"), formNo.ToString("D3"));
                        newLabelData.labelIndex = msgFile.labelDatas.Count;
                        newLabelData.arrayIndex = msgFile.labelDatas.Count;

                        msgFile.labelDatas.Add(newLabelData);
                    }
                }
            }
        }
        public void UpdatePokemonInfos(int baseMonsNo, int monsNo, int baseFormNo, int formNo, int gender)
        {
            UpdatePokemonInfo(baseMonsNo, monsNo, baseFormNo, formNo, gender, false);
            UpdatePokemonInfo(baseMonsNo, monsNo, baseFormNo, formNo, gender, true);
        }
        public void UpdatePokemonInfo(int baseMonsNo, int monsNo, int baseFormNo, int formNo, int gender, bool isRare)
        {
            String oldPMName = string.Format("pm{0}_{1}", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String newPMName = string.Format("pm{0}_{1}", monsNo.ToString("D4"), formNo.ToString("D2"));
            int uniqueID = genUniqueID(monsNo, formNo, gender, isRare);
            Masterdatas.PokemonInfoCatalog basePokemonInfoCatalog = null;
            foreach (Masterdatas.PokemonInfoCatalog pokemonInfoCatalog in GlobalData.gameData.pokemonInfos)
            {
                if (pokemonInfoCatalog.MonsNo == baseMonsNo && pokemonInfoCatalog.FormNo == baseFormNo && pokemonInfoCatalog.Rare == isRare)
                {
                    basePokemonInfoCatalog = pokemonInfoCatalog;
                    break;
                }
            }

            Masterdatas.PokemonInfoCatalog newPokemonInfoCatalog = (Masterdatas.PokemonInfoCatalog)basePokemonInfoCatalog.Clone();
            newPokemonInfoCatalog.No = monsNo;
            newPokemonInfoCatalog.MonsNo = monsNo;
            newPokemonInfoCatalog.FormNo = formNo;
            newPokemonInfoCatalog.UniqueID = uniqueID;
            newPokemonInfoCatalog.AssetBundleName = basePokemonInfoCatalog.AssetBundleName.Replace(oldPMName, newPMName);
            GlobalData.gameData.pokemonInfos.Add(newPokemonInfoCatalog);
        }

        public void UpdateMotionTimingData(int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            BattleMasterdatas.MotionTimingData baseMotionTimingData = null;
            foreach (BattleMasterdatas.MotionTimingData motionTimingData in GlobalData.gameData.motionTimingData)
            {
                if (motionTimingData.MonsNo == baseMonsNo && motionTimingData.FormNo == baseFormNo)
                {
                    baseMotionTimingData = motionTimingData;
                    break;
                }
            }

            BattleMasterdatas.MotionTimingData newMotionTimingData = (BattleMasterdatas.MotionTimingData)baseMotionTimingData.Clone();
            newMotionTimingData.MonsNo = monsNo;
            newMotionTimingData.FormNo = formNo;
            GlobalData.gameData.motionTimingData.Add(newMotionTimingData);
        }
        public void UpdateAddPersonalTable(int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            PersonalMasterdatas.AddPersonalTable baseAddPersonalTable = null;
            foreach (PersonalMasterdatas.AddPersonalTable addPersonalTable in GlobalData.gameData.addPersonalTables)
            {
                if (addPersonalTable.monsno == baseMonsNo && addPersonalTable.formno == baseFormNo)
                {
                    baseAddPersonalTable = addPersonalTable;
                    break;
                }
            }

            PersonalMasterdatas.AddPersonalTable newAddPersonalTable = (PersonalMasterdatas.AddPersonalTable)baseAddPersonalTable.Clone();
            newAddPersonalTable.monsno = (ushort) monsNo;
            newAddPersonalTable.formno = (ushort) formNo;
            GlobalData.gameData.addPersonalTables.Add(newAddPersonalTable);
        }
        public void UpdateUIMasterdatas(int baseMonsNo, int monsNo, int baseFormNo, int formNo, int gender)
        {
            UpdateUIMasterdatasData(baseMonsNo, monsNo, baseFormNo, formNo, gender, false);
            UpdateUIMasterdatasData(baseMonsNo, monsNo, baseFormNo, formNo, gender, true);
        }

        public void UpdateUIMasterdatasData(int baseMonsNo, int monsNo, int baseFormNo, int formNo, int gender, bool isRare)
        {
            int baseUniqueID = genUniqueID(baseMonsNo, baseFormNo, gender, isRare);
            int uniqueID = genUniqueID(monsNo, formNo, gender, isRare);

            UIMasterdatas.PokemonIcon pokemonIcon = GlobalData.gameData.uiPokemonIcon[baseUniqueID];
            UIMasterdatas.PokemonIcon newPokemonIcon = (UIMasterdatas.PokemonIcon) pokemonIcon.Clone();
            newPokemonIcon.UniqueID = uniqueID;
            GlobalData.gameData.newUIPokemonIcon[uniqueID] = newPokemonIcon;

            UIMasterdatas.AshiatoIcon ashiatoIcon = GlobalData.gameData.uiAshiatoIcon[baseUniqueID];
            UIMasterdatas.AshiatoIcon newAshiatoIcon = (UIMasterdatas.AshiatoIcon)ashiatoIcon.Clone();
            newAshiatoIcon.UniqueID = uniqueID;
            GlobalData.gameData.newUIAshiatoIcon[uniqueID] = newAshiatoIcon;

            UIMasterdatas.PokemonVoice pokemonVoice = GlobalData.gameData.uiPokemonVoice[baseUniqueID];
            UIMasterdatas.PokemonVoice newPokemonVoice = (UIMasterdatas.PokemonVoice)pokemonVoice.Clone();
            newPokemonVoice.UniqueID = uniqueID;
            GlobalData.gameData.newUIPokemonVoice[uniqueID] = newPokemonVoice;

            UIMasterdatas.ZukanDisplay zukanDisplay = GlobalData.gameData.uiZukanDisplay[baseUniqueID];
            UIMasterdatas.ZukanDisplay newZukanDisplay = (UIMasterdatas.ZukanDisplay)zukanDisplay.Clone();
            newZukanDisplay.UniqueID = uniqueID;
            GlobalData.gameData.newUIZukanDisplay[uniqueID] = newZukanDisplay;

            UIMasterdatas.ZukanCompareHeight zukanCompareHeight = GlobalData.gameData.uiZukanCompareHeights[baseUniqueID];
            UIMasterdatas.ZukanCompareHeight newZukanCompareHeight = (UIMasterdatas.ZukanCompareHeight)zukanCompareHeight.Clone();
            newZukanCompareHeight.UniqueID = uniqueID;
            GlobalData.gameData.newUIZukanCompareHeights[uniqueID] = newZukanCompareHeight;
        }

        public void DuplicateAssetBundles(int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            String commonPath = string.Format("common\\pm{0}_{1}", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String common00Path = string.Format("common\\pm{0}_{1}_00", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String common01Path = string.Format("common\\pm{0}_{1}_01", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String battle00Path = string.Format("battle\\pm{0}_{1}_00", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String battle01Path = string.Format("battle\\pm{0}_{1}_01", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String field00Path = string.Format("field\\pm{0}_{1}_00", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String field01Path = string.Format("field\\pm{0}_{1}_01", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String fieldAnimationsPath = string.Format("field\\animations\\pm{0}_{1}", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));

            String newCommonPath = string.Format("common\\pm{0}_{1}", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newCommon00Path = string.Format("common\\pm{0}_{1}_00", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newCommon01Path = string.Format("common\\pm{0}_{1}_01", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newBattle00Path = string.Format("battle\\pm{0}_{1}_00", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newBattle01Path = string.Format("battle\\pm{0}_{1}_01", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newField00Path = string.Format("field\\pm{0}_{1}_00", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newField01Path = string.Format("field\\pm{0}_{1}_01", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newFieldAnimationsPath = string.Format("field\\animations\\pm{0}_{1}", monsNo.ToString("D4"), formNo.ToString("D2"));

            BundleFileInstance commonBfi = DuplicateAssetBundle(commonPath, newCommonPath, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance common00Bfi = DuplicateAssetBundle(common00Path, newCommon00Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance common01Bfi = DuplicateAssetBundle(common01Path, newCommon01Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance battle00Bfi = DuplicateAssetBundle(battle00Path, newBattle00Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance battle01Bfi = DuplicateAssetBundle(battle01Path, newBattle01Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance field00Bfi = DuplicateAssetBundle(field00Path, newField00Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance field01Bfi = DuplicateAssetBundle(field01Path, newField01Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance fieldAnimationsBfi = DuplicateAssetBundle(fieldAnimationsPath, newFieldAnimationsPath, baseMonsNo, monsNo, baseFormNo, formNo);
        }

        public void writeBundle(BundleFileInstance bfi, string ofpath)
        {
            FileStream stream = File.OpenWrite(ofpath);
            AssetsFileWriter afw = new AssetsFileWriter(stream);
            bfi.file.Pack(bfi.file.reader, afw, AssetBundleCompressionType.LZ4);
            afw.Close();
            bfi.file.Close();
            bfi.stream.Dispose();
        }

        public BundleFileInstance DuplicateAssetBundle(String ifpath, String ofpath, int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            String baseDirectory = Environment.CurrentDirectory + "\\" + FileManager.outputModName + "\\romfs\\Data\\StreamingAssets\\AssetAssistant\\";
            ofpath = baseDirectory + "Pokemon Database\\pokemons\\" + ofpath;
            String newCAB = genCABName();
            String oldPMName = string.Format("pm{0}_{1}", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String newPMName = string.Format("pm{0}_{1}", monsNo.ToString("D4"), formNo.ToString("D2"));

            List<AssetsReplacer> ars = new();
            AssetsManager am = GlobalData.fileManager.getAssetsManager();
            am.updateAfterLoad = true;
            BundleFileInstance bfi = GlobalData.fileManager.GetBundleFileInstance(basePath + ifpath);
            bfi.name = bfi.name.Replace(oldPMName, newPMName);
           

            AssetsFileInstance afi = am.LoadAssetsFileFromBundle(bfi, 0);

            String oldCAB = afi.name.Replace("CAB-", "");
            afi.name = afi.name.Replace(oldCAB, newCAB);
            CABNames.Add(oldCAB, newCAB);

            AssetBundleDirectoryInfo06[] dirInf = bfi.file.bundleInf6.dirInf;
            // afi.table.assetFileInfo
            foreach (AssetBundleDirectoryInfo06 iDirInf in dirInf)
            {
                String dirInfName = iDirInf.name;
                if (dirInfName.Contains(".resS"))
                {
                    iDirInf.name = dirInfName.Replace(oldCAB, newCAB);
                }
            }

            for (int i = 0; i < afi.file.dependencies.dependencyCount; i++)
            {
                String assetPath = afi.file.dependencies.dependencies[i].assetPath;
                foreach (String cabName in CABNames.Keys)
                {
                    assetPath = assetPath.Replace(cabName, CABNames[cabName]);
                }
                afi.file.dependencies.dependencies[i].assetPath = assetPath;
            }

            List<AssetTypeValueField> gameObjects = afi.table.GetAssetsOfType((int)AssetClassID.GameObject).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField gameObject;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObject = gameObjects[i];

                string m_Name = gameObject["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.GameObject);

                m_Name = m_Name.Replace(oldPMName, newPMName);
                gameObject["m_Name"].GetValue().Set(m_Name);

                byte[] b = gameObject.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new AssetsReplacerFromMemory(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }

            List<AssetTypeValueField> materials = afi.table.GetAssetsOfType((int)AssetClassID.Material).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField material;

            for (int i = 0; i < materials.Count; i++)
            {
                material = materials[i];

                string m_Name = material["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.Material);

                m_Name = m_Name.Replace(oldPMName, newPMName);
                material["m_Name"].GetValue().Set(m_Name);

                byte[] b = material.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new AssetsReplacerFromMemory(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }

            List<AssetTypeValueField> texture2Ds = afi.table.GetAssetsOfType((int)AssetClassID.Texture2D).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();

            AssetTypeValueField texture2DField;
            for (int i = 0; i < texture2Ds.Count; i++)
            {
                texture2DField = texture2Ds[i];

                string m_Name = texture2DField["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.Texture2D);

                String m_StreamDataPath = texture2DField["m_StreamData"].children[2].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                m_StreamDataPath = m_StreamDataPath.Replace(oldCAB, newCAB);
                texture2DField["m_Name"].GetValue().Set(m_Name);
                texture2DField["m_StreamData"].children[2].GetValue().Set(m_StreamDataPath);

                byte[] b = texture2DField.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new AssetsReplacerFromMemory(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }

            List<AssetTypeValueField> meshes = afi.table.GetAssetsOfType((int)AssetClassID.Mesh).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField mesh;

            for (int i = 0; i < meshes.Count; i++)
            {
                mesh = meshes[i];

                string m_Name = mesh["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.Mesh);

                m_Name = m_Name.Replace(oldPMName, newPMName);
                mesh["m_Name"].GetValue().Set(m_Name);

                byte[] b = mesh.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new AssetsReplacerFromMemory(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }


            List<AssetTypeValueField> animationClips = afi.table.GetAssetsOfType((int)AssetClassID.AnimationClip).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();

            AssetTypeValueField animationClip;
            for (int i = 0; i < animationClips.Count; i++)
            {
                animationClip = animationClips[i];

                string m_Name = animationClip["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.AnimationClip);

                m_Name = m_Name.Replace(oldPMName, newPMName);
                animationClip["m_Name"].GetValue().Set(m_Name);

                byte[] b = animationClip.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new AssetsReplacerFromMemory(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }

            List<AssetTypeValueField> assetBundles = afi.table.GetAssetsOfType((int)AssetClassID.AssetBundle).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField assetBundle;
            for (int i = 0; i < assetBundles.Count; i++)
            {
                assetBundle = assetBundles[i];

                string m_Name = assetBundle["m_Name"].value.AsString();
                string m_AssetBundleName = assetBundle["m_AssetBundleName"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.AssetBundle);

                m_Name = m_Name.Replace(oldPMName, newPMName);
                m_AssetBundleName = m_Name.Replace(oldPMName, newPMName);
                assetBundle["m_Name"].GetValue().Set(m_Name);
                assetBundle["m_AssetBundleName"].GetValue().Set(m_AssetBundleName);

                AssetTypeValueField m_Dependencies = assetBundle["m_Dependencies"][0];
                for (int j = 0; j < m_Dependencies.childrenCount; j++)
                {
                    AssetTypeValueField entry = m_Dependencies.children[j];
                    string key = entry.value.AsString();
                    key = key.Replace(oldPMName, newPMName);
                    entry.GetValue().Set(key);
                }

                AssetTypeValueField m_Container = assetBundle["m_Container"][0];

                for (int j = 0; j < m_Container.childrenCount; j++)
                {
                    AssetTypeValueField entry = m_Container.children[j];
                    string key = entry.children[0].value.AsString();
                    key = key.Replace(oldPMName, newPMName);
                    entry.children[0].GetValue().Set(key);
                }

                byte[] b = assetBundle.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new AssetsReplacerFromMemory(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }

            // TODO: Update AABBData in PokemonPrefabInfo
            // TODO: Update FieldPokemonEntity

            am.UpdateDependencies(afi);

            GlobalData.fileManager.MakeTempBundle(afi, bfi, afi.name, ars, ofpath);

            return bfi;
        }
    }
}