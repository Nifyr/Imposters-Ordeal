using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;
using SmartPoint.AssetAssistant;

namespace ImpostersOrdeal
{
    class AssetInserter
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

        private const string basePath = "\\Pokemon Database\\pokemons\\";

        private static AssetInserter instance;
        private Random rnd;
        private Dictionary<string, string> CABNames;
        private AssetInserter()
        {
            rnd = new();
            CABNames = new();
        }
        public static AssetInserter GetInstance()
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
        public string GenCABName()
        {
            string[] values = new string[4];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = rnd.Next().ToString("x2");
            }

            return string.Join("", values);
        }

        public int GenUniqueID(int monsNo, int formNo, int gender, bool isRare)
        {
            return monsNo * 10000 + formNo * 100 + gender * 10 + (isRare ? 1 : 0);
        }

        public void InsertPokemon(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, int gender, string formName)
        {
            UpdateUIMasterdatas(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, gender);
            UpdateAddPersonalTable(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdateMotionTimingData(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdatePokemonInfos(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, gender);
            UpdatePersonalInfos(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdateCommonMsbt(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, formName);
            DuplicateAssetBundles(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdateDprBin(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            gameData.SetModified(GameDataSet.DataField.UIMasterdatas);
            gameData.SetModified(GameDataSet.DataField.AddPersonalTable);
            gameData.SetModified(GameDataSet.DataField.MotionTimingData);
            gameData.SetModified(GameDataSet.DataField.PokemonInfo);
            gameData.SetModified(GameDataSet.DataField.MessageFileSets);
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
            gameData.SetModified(GameDataSet.DataField.DprBin);
        }

        public void UpdateDprBin(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            AssetBundleDownloadManifest abdm = gameData.dprBin;

            foreach (AssetBundleRecord record in abdm.records)
            {
                string oldPMName = string.Format("pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
                string newPMName = string.Format("pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
                if (record.assetBundleName.Contains(oldPMName))
                {
                    AssetBundleRecord newRecord = (AssetBundleRecord)record.Clone();
                    newRecord.assetBundleName = newRecord.assetBundleName.Replace(oldPMName, newPMName);

                    for (int i = 0; i < newRecord.assetPaths.Length; i++)
                    {
                        newRecord.assetPaths[i] = newRecord.assetPaths[i].Replace(oldPMName, newPMName);
                    }

                    for (int i = 0; i < newRecord.allDependencies.Length; i++)
                    {
                        newRecord.allDependencies[i] = newRecord.allDependencies[i].Replace(oldPMName, newPMName);
                    }
                    abdm.Add(newRecord.assetBundleName, newRecord);
                }
            }
        }

        public void UpdatePersonalInfos(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            // Will only really work for adding a new form. Not a new pokemon based on another one
            Pokemon srcPokemon = null;
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                if (pokemon.dexID == srcMonsNo && pokemon.formID == srcFormNo)
                {
                    srcPokemon = pokemon;
                }
                pokemon.pastPokemon = new();
                pokemon.nextPokemon = new();
                pokemon.inferiorForms = new();
                pokemon.superiorForms = new();
            }
            srcPokemon.formMax = (byte)(dstFormNo + 1);

            Pokemon newPokemon = (Pokemon)srcPokemon.Clone();
            newPokemon.dexID = (ushort) dstMonsNo;
            newPokemon.formID = dstFormNo;
            newPokemon.personalID = (ushort)GetPersonalInsertPos(newPokemon);

            PersonalInsert(newPokemon);
            gameData.dexEntries[newPokemon.dexID].forms.Add(newPokemon);
            foreach (Pokemon p in gameData.dexEntries[newPokemon.dexID].forms)
                p.formIndex = gameData.dexEntries[newPokemon.dexID].forms[1].personalID;
            DataParser.SetFamilies();
        }

        private static void PersonalInsert(Pokemon insert)
        {
            foreach (Pokemon p in gameData.personalEntries)
            {
                if (p.personalID >= insert.personalID)
                    p.personalID++;
                if (p.formIndex >= insert.personalID)
                    p.formIndex++;
            }

            IEnumerable<LabelData> lds = gameData.messageFileSets.SelectMany(mfs => mfs.messageFiles.Where
            (mf => mf.mName.Contains("ss_zkn_form") || mf.mName.Contains("ss_zkn_height") || mf.mName.Contains("ss_zkn_weight"))
            .SelectMany(mf => mf.labelDatas));
            foreach (LabelData ld in lds)
            {
                if (ld.labelIndex >= insert.personalID)
                    ld.labelIndex++;
                if (ld.arrayIndex >= insert.personalID)
                    ld.arrayIndex++;
            }

            if (insert.personalID == gameData.personalEntries.Count)
                gameData.personalEntries.Add(insert);
            else
                gameData.personalEntries.Insert(insert.personalID, insert);
        }

        private static int GetPersonalInsertPos(Pokemon p)
        {
            for (int i = 0; i < gameData.personalEntries.Count; i++)
                if (p.CompareTo(gameData.personalEntries[i]) < 0)
                    return i;
            return gameData.personalEntries.Count;
        }

        public void UpdateCommonMsbt(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, string formName)
        {
            foreach (MessageFileSet msgFileSet in gameData.messageFileSets)
            {
                foreach (MessageFile msgFile in msgFileSet.messageFiles)
                {
                    if (msgFile.mName.Contains("ss_zkn_form"))
                    {
                        LabelData baseLabelData = null;
                        foreach (LabelData labelData in msgFile.labelDatas)
                        {
                            string baseLabelName = string.Format("ZKN_FORM_{0}_{1}", srcMonsNo.ToString("D3"), srcFormNo.ToString("D3"));
                            if (labelData.labelName == baseLabelName)
                            {
                                baseLabelData = labelData;
                            }
                        }
                        LabelData newLabelData = (LabelData) baseLabelData.Clone();
                        newLabelData.labelName = string.Format("ZKN_FORM_{0}_{1}", dstMonsNo.ToString("D3"), dstFormNo.ToString("D3"));
                        newLabelData.labelIndex = gameData.GetPokemon(dstMonsNo, dstFormNo).personalID;
                        newLabelData.arrayIndex = newLabelData.labelIndex;
                        newLabelData.wordDatas[0].str = formName;

                        msgFile.labelDatas.Insert(newLabelData.arrayIndex, newLabelData);
                    }

                    if (msgFile.mName.Contains("ss_zkn_height"))
                    {
                        LabelData baseLabelData = null;
                        foreach (LabelData labelData in msgFile.labelDatas)
                        {
                            string baseLabelName = string.Format("ZKN_HEIGHT_{0}_{1}", srcMonsNo.ToString("D3"), srcFormNo.ToString("D3"));
                            if (labelData.labelName == baseLabelName)
                            {
                                baseLabelData = labelData;
                            }
                        }
                        LabelData newLabelData = (LabelData)baseLabelData.Clone();
                        newLabelData.labelName = string.Format("ZKN_HEIGHT_{0}_{1}", dstMonsNo.ToString("D3"), dstFormNo.ToString("D3"));
                        newLabelData.labelIndex = gameData.GetPokemon(dstMonsNo, dstFormNo).personalID;
                        newLabelData.arrayIndex = newLabelData.labelIndex;

                        msgFile.labelDatas.Insert(newLabelData.arrayIndex, newLabelData);
                    }

                    if (msgFile.mName.Contains("ss_zkn_weight"))
                    {
                        LabelData baseLabelData = null;
                        foreach (LabelData labelData in msgFile.labelDatas)
                        {
                            string baseLabelName = string.Format("ZKN_WEIGHT_{0}_{1}", srcMonsNo.ToString("D3"), srcFormNo.ToString("D3"));
                            if (labelData.labelName == baseLabelName)
                            {
                                baseLabelData = labelData;
                            }
                        }
                        LabelData newLabelData = (LabelData)baseLabelData.Clone();
                        newLabelData.labelName = string.Format("ZKN_WEIGHT_{0}_{1}", dstMonsNo.ToString("D3"), dstFormNo.ToString("D3"));
                        newLabelData.labelIndex = gameData.GetPokemon(dstMonsNo, dstFormNo).personalID;
                        newLabelData.arrayIndex = newLabelData.labelIndex;

                        msgFile.labelDatas.Insert(newLabelData.arrayIndex, newLabelData);
                    }
                }
            }
        }

        public void UpdatePokemonInfos(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, int gender)
        {
            UpdatePokemonInfo(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, gender, false);
            UpdatePokemonInfo(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, gender, true);
        }

        public void UpdatePokemonInfo(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, int gender, bool isRare)
        {
            string oldPMName = string.Format("pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string newPMName = string.Format("pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            int uniqueID = GenUniqueID(dstMonsNo, dstFormNo, gender, isRare);
            Masterdatas.PokemonInfoCatalog basePokemonInfoCatalog = null;
            foreach (Masterdatas.PokemonInfoCatalog pokemonInfoCatalog in gameData.pokemonInfos)
            {
                if (pokemonInfoCatalog.MonsNo == srcMonsNo && pokemonInfoCatalog.FormNo == srcFormNo && pokemonInfoCatalog.Rare == isRare)
                {
                    basePokemonInfoCatalog = pokemonInfoCatalog;
                    break;
                }
            }

            Masterdatas.PokemonInfoCatalog newPokemonInfoCatalog = (Masterdatas.PokemonInfoCatalog)basePokemonInfoCatalog.Clone();
            newPokemonInfoCatalog.No = dstMonsNo;
            newPokemonInfoCatalog.MonsNo = dstMonsNo;
            newPokemonInfoCatalog.FormNo = dstFormNo;
            newPokemonInfoCatalog.UniqueID = uniqueID;
            newPokemonInfoCatalog.AssetBundleName = basePokemonInfoCatalog.AssetBundleName.Replace(oldPMName, newPMName);
            gameData.pokemonInfos.Add(newPokemonInfoCatalog);
        }

        public void UpdateMotionTimingData(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            BattleMasterdatas.MotionTimingData baseMotionTimingData = null;
            foreach (BattleMasterdatas.MotionTimingData motionTimingData in gameData.motionTimingData)
            {
                if (motionTimingData.MonsNo == srcMonsNo && motionTimingData.FormNo == srcFormNo)
                {
                    baseMotionTimingData = motionTimingData;
                    break;
                }
            }

            BattleMasterdatas.MotionTimingData newMotionTimingData = (BattleMasterdatas.MotionTimingData)baseMotionTimingData.Clone();
            newMotionTimingData.MonsNo = dstMonsNo;
            newMotionTimingData.FormNo = dstFormNo;
            gameData.motionTimingData.Add(newMotionTimingData);
        }
        public void UpdateAddPersonalTable(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            PersonalMasterdatas.AddPersonalTable baseAddPersonalTable = null;
            foreach (PersonalMasterdatas.AddPersonalTable addPersonalTable in gameData.addPersonalTables)
            {
                if (addPersonalTable.monsno == srcMonsNo && addPersonalTable.formno == srcFormNo)
                {
                    baseAddPersonalTable = addPersonalTable;
                    break;
                }
            }

            PersonalMasterdatas.AddPersonalTable newAddPersonalTable = (PersonalMasterdatas.AddPersonalTable)baseAddPersonalTable.Clone();
            newAddPersonalTable.monsno = (ushort) dstMonsNo;
            newAddPersonalTable.formno = (ushort) dstFormNo;
            gameData.addPersonalTables.Add(newAddPersonalTable);
        }
        public void UpdateUIMasterdatas(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, int gender)
        {
            UpdateUIMasterdatasData(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, gender, false);
            UpdateUIMasterdatasData(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, gender, true);
        }

        public void UpdateUIMasterdatasData(int srcMonsNo, int dstmonsNo, int srcFormNo, int dstFormNo, int gender, bool isRare)
        {
            int srcUniqueID = GenUniqueID(srcMonsNo, srcFormNo, gender, isRare);
            int dstUniqueID = GenUniqueID(dstmonsNo, dstFormNo, gender, isRare);

            UIMasterdatas.PokemonIcon pokemonIcon = gameData.uiPokemonIcon.Find(o => o.UniqueID == srcUniqueID);
            UIMasterdatas.PokemonIcon newPokemonIcon = (UIMasterdatas.PokemonIcon) pokemonIcon.Clone();
            newPokemonIcon.UniqueID = dstUniqueID;
            gameData.uiPokemonIcon.Add(newPokemonIcon);

            UIMasterdatas.AshiatoIcon ashiatoIcon = gameData.uiAshiatoIcon.Find(o => o.UniqueID == srcUniqueID);
            UIMasterdatas.AshiatoIcon newAshiatoIcon = (UIMasterdatas.AshiatoIcon)ashiatoIcon.Clone();
            newAshiatoIcon.UniqueID = dstUniqueID;
            gameData.uiAshiatoIcon.Add(newAshiatoIcon);

            UIMasterdatas.PokemonVoice pokemonVoice = gameData.uiPokemonVoice.Find(o => o.UniqueID == srcUniqueID);
            UIMasterdatas.PokemonVoice newPokemonVoice = (UIMasterdatas.PokemonVoice)pokemonVoice.Clone();
            newPokemonVoice.UniqueID = dstUniqueID;
            gameData.uiPokemonVoice.Add(newPokemonVoice);

            UIMasterdatas.ZukanDisplay zukanDisplay = gameData.uiZukanDisplay.Find(o => o.UniqueID == srcUniqueID);
            UIMasterdatas.ZukanDisplay newZukanDisplay = (UIMasterdatas.ZukanDisplay)zukanDisplay.Clone();
            newZukanDisplay.UniqueID = dstUniqueID;
            gameData.uiZukanDisplay.Add(newZukanDisplay);

            UIMasterdatas.ZukanCompareHeight zukanCompareHeight = gameData.uiZukanCompareHeights.Find(o => o.UniqueID == srcUniqueID);
            UIMasterdatas.ZukanCompareHeight newZukanCompareHeight = (UIMasterdatas.ZukanCompareHeight)zukanCompareHeight.Clone();
            newZukanCompareHeight.UniqueID = dstUniqueID;
            gameData.uiZukanCompareHeights.Add(newZukanCompareHeight);
        }

        public void DuplicateAssetBundles(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            string commonPath = string.Format("common\\pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string common00Path = string.Format("common\\pm{0}_{1}_00", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string common01Path = string.Format("common\\pm{0}_{1}_01", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string battle00Path = string.Format("battle\\pm{0}_{1}_00", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string battle01Path = string.Format("battle\\pm{0}_{1}_01", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string field00Path = string.Format("field\\pm{0}_{1}_00", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string field01Path = string.Format("field\\pm{0}_{1}_01", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string fieldAnimationsPath = string.Format("field\\animations\\pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string battleAnimationsPath = string.Format("battle\\animations\\pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));

            string newCommonPath = string.Format("common\\pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newCommon00Path = string.Format("common\\pm{0}_{1}_00", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newCommon01Path = string.Format("common\\pm{0}_{1}_01", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newBattle00Path = string.Format("battle\\pm{0}_{1}_00", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newBattle01Path = string.Format("battle\\pm{0}_{1}_01", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newField00Path = string.Format("field\\pm{0}_{1}_00", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newField01Path = string.Format("field\\pm{0}_{1}_01", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newFieldAnimationsPath = string.Format("field\\animations\\pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));
            string newBattleAnimationsPath = string.Format("battle\\animations\\pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));

            DuplicateAssetBundle(commonPath, newCommonPath, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(common00Path, newCommon00Path, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(common01Path, newCommon01Path, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(fieldAnimationsPath, newFieldAnimationsPath, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(battleAnimationsPath, newBattleAnimationsPath, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(battle00Path, newBattle00Path, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(battle01Path, newBattle01Path, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(field00Path, newField00Path, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            DuplicateAssetBundle(field01Path, newField01Path, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
        }

        public void WriteBundle(BundleFileInstance bfi, string ofpath)
        {
            FileStream stream = File.OpenWrite(ofpath);
            AssetsFileWriter afw = new(stream);
            bfi.file.Pack(bfi.file.reader, afw, AssetBundleCompressionType.LZ4);
            afw.Close();
            bfi.file.Close();
            bfi.stream.Dispose();
        }

        public BundleFileInstance DuplicateAssetBundle(string ifpath, string ofpath, int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            string baseDirectory = Environment.CurrentDirectory + "\\" + FileManager.outputModName + "\\romfs\\Data\\StreamingAssets\\AssetAssistant\\";
            ofpath = "romfs\\Data\\StreamingAssets\\AssetAssistant\\Pokemon Database\\pokemons\\" + ofpath;
            string newCAB = GenCABName();
            string oldPMName = string.Format("pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string newPMName = string.Format("pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));

            List<AssetsReplacer> ars = new();
            AssetsManager am = fileManager.getAssetsManager();
            am.updateAfterLoad = true;
            BundleFileInstance bfi = fileManager.GetBundleFileInstance(basePath + ifpath);
            bfi.name = bfi.name.Replace(oldPMName, newPMName);

            AssetsFileInstance afi = am.LoadAssetsFileFromBundle(bfi, 0);

            string oldCAB = afi.name.Replace("CAB-", "");
            afi.name = afi.name.Replace(oldCAB, newCAB);
            CABNames[oldCAB] = newCAB;

            AssetBundleDirectoryInfo06[] dirInf = bfi.file.bundleInf6.dirInf;
            // afi.table.assetFileInfo
            foreach (AssetBundleDirectoryInfo06 iDirInf in dirInf)
            {
                string dirInfName = iDirInf.name;
                if (dirInfName.Contains(".resS"))
                {
                    iDirInf.name = dirInfName.Replace(oldCAB, newCAB);
                }
            }

            for (int i = 0; i < afi.file.dependencies.dependencyCount; i++)
            {
                string assetPath = afi.file.dependencies.dependencies[i].assetPath;
                foreach (string cabName in CABNames.Keys)
                {
                    assetPath = assetPath.Replace(cabName, CABNames[cabName]);
                }
                afi.file.dependencies.dependencies[i].assetPath = assetPath;
            }

            List<AssetTypeValueField> texture2Ds = afi.table.GetAssetsOfType((int)AssetClassID.Texture2D).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();

            AssetTypeValueField texture2DField;
            for (int i = 0; i < texture2Ds.Count; i++)
            {
                texture2DField = texture2Ds[i];

                string m_Name = texture2DField["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.Texture2D);

                string m_StreamDataPath = texture2DField["m_StreamData"].children[2].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                m_StreamDataPath = m_StreamDataPath.Replace(oldCAB, newCAB);
                texture2DField["m_Name"].GetValue().Set(m_Name);
                texture2DField["m_StreamData"].children[2].GetValue().Set(m_StreamDataPath);

                byte[] b = texture2DField.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
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
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
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
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
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
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
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
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
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
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }

            // TODO: Update AABBData in PokemonPrefabInfo
            // TODO: Update FieldPokemonEntity

            am.UpdateDependencies(afi);

            fileManager.MakeTempBundle("romfs\\Data\\StreamingAssets\\AssetAssistant\\Pokemon Database\\pokemons\\" + ifpath, ofpath, ars, "CAB-" + newCAB);

            return bfi;
        }
    }
}