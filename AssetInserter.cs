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
using System.Text.RegularExpressions;

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

        private const string basePath = "\\Pokemon Database\\";

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

        public void InsertPokemon(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, string formName)
        {
            List<(int, int)> uniqueIDs = GetUniqueIDs(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdatePokemonInfo(uniqueIDs, out List<(string, string)> assetBundleNames);
            UpdateDprBin(assetBundleNames, out Dictionary<string, string> assetBundlePaths);
            UpdateUIMasterdatas(uniqueIDs);
            UpdateAddPersonalTable(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdateMotionTimingData(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdatePersonalInfos(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdateCommonMsbt(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, formName);
            DuplicateAssetBundles(assetBundlePaths, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            gameData.SetModified(GameDataSet.DataField.UIMasterdatas);
            gameData.SetModified(GameDataSet.DataField.AddPersonalTable);
            gameData.SetModified(GameDataSet.DataField.MotionTimingData);
            gameData.SetModified(GameDataSet.DataField.PokemonInfo);
            gameData.SetModified(GameDataSet.DataField.MessageFileSets);
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
            gameData.SetModified(GameDataSet.DataField.DprBin);
        }

        private List<(int, int)> GetUniqueIDs(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            (int, int) uniqueIDRange = (GenUniqueID(srcMonsNo, srcFormNo, 0, false), GenUniqueID(srcMonsNo, srcFormNo, 9, true));
            List<Masterdatas.PokemonInfoCatalog> pics = gameData.pokemonInfos.Where(pic => pic.UniqueID >= uniqueIDRange.Item1 && pic.UniqueID <= uniqueIDRange.Item2).ToList();
            List<(int, int)> uniqueIDPairs = new();
            foreach (Masterdatas.PokemonInfoCatalog pic in pics)
                uniqueIDPairs.Add((pic.UniqueID, GenUniqueID(dstMonsNo, dstFormNo, pic.Sex, pic.Rare)));
            return uniqueIDPairs;
        }

        public void UpdateDprBin(List<(string, string)> assetBundleNames, out Dictionary<string, string> assetBundlePaths)
        {
            AssetBundleDownloadManifest abdm = gameData.dprBin;

            assetBundlePaths = new();
            Dictionary<string, string> assetBundlePathsWithDependencies = new();

            List<AssetBundleRecord> recordsToCopy = new();
            foreach ((string, string) assetBundleNamePair in assetBundleNames)
            {
                List<AssetBundleRecord> recordsToAdd = abdm.GetAssetBundleRecordsWithDependencies("pokemons/battle/" + assetBundleNamePair.Item1, true).ToList();
                recordsToAdd.AddRange(abdm.GetAssetBundleRecordsWithDependencies("pokemons/field/" + assetBundleNamePair.Item1, true));
                foreach (AssetBundleRecord record in recordsToAdd)
                    if (record.projectName == "Pokemon Database" && !assetBundlePaths.ContainsKey(record.assetBundleName) && !assetBundlePathsWithDependencies.ContainsKey(record.assetBundleName))
                    {
                        recordsToCopy.Add(record);
                        string newAssetBundlePath = Path.Combine(Path.GetDirectoryName(record.assetBundleName), assetBundleNamePair.Item2).Replace('\\', '/');
                        while (assetBundlePaths.Values.Contains(newAssetBundlePath) || assetBundlePathsWithDependencies.Values.Contains(newAssetBundlePath))
                            newAssetBundlePath = IncrementName(newAssetBundlePath);
                        if (record.allDependencies.Length == 0)
                            assetBundlePaths[record.assetBundleName] = newAssetBundlePath;
                        else
                            assetBundlePathsWithDependencies[record.assetBundleName] = newAssetBundlePath;
                    }
            }
            recordsToCopy.Sort((r1, r2) => r1.allDependencies.Length - r2.allDependencies.Length);
            foreach (KeyValuePair<string, string> pair in assetBundlePathsWithDependencies)
                assetBundlePaths.Add(pair.Key, pair.Value);

            foreach (AssetBundleRecord record in recordsToCopy)
            {
                AssetBundleRecord newRecord = (AssetBundleRecord)record.Clone();
                newRecord.assetBundleName = assetBundlePaths[newRecord.assetBundleName];

                for (int i = 0; i < newRecord.assetPaths.Length; i++)
                    newRecord.assetPaths[i] = UpdateAssetPath(newRecord.assetPaths[i], Path.GetFileName(newRecord.assetBundleName));

                for (int i = 0; i < newRecord.allDependencies.Length; i++)
                    if (assetBundlePaths.ContainsKey(newRecord.allDependencies[i]))
                        newRecord.allDependencies[i] = assetBundlePaths[newRecord.allDependencies[i]];
                abdm.Add(newRecord);
            }
        }

        private string IncrementName(string assetBundlePath)
        {
            if (Regex.Matches(assetBundlePath, @"_\d+$").Count == 0)
                return assetBundlePath + "_0";
            return Regex.Replace(assetBundlePath, @"_\d+$", m => "_" + (int.Parse(m.Value[1..]) + 1));
        }

        private string UpdateAssetPath(string assetPath, string assetBundleName)
        {
            string name = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath)).Replace('\\', '/');
            string ext = Path.GetExtension(assetPath);
            if (ext == null)
                ext = "";
            if (Regex.Matches(name, @"pm\d+$").Count == 0)
                return name + assetBundleName + ext;
            return Regex.Replace(name, @"pm\d+$", m => assetBundleName) + ext;
        }

        public void UpdatePersonalInfos(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
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

            Pokemon newPokemon = (Pokemon)srcPokemon.Clone();
            newPokemon.dexID = (ushort) dstMonsNo;
            newPokemon.formID = dstFormNo;
            newPokemon.personalID = (ushort)GetPersonalInsertPos(newPokemon);

            PersonalInsert(newPokemon);
            if (gameData.dexEntries.Count == newPokemon.dexID)
            {
                gameData.dexEntries.Add(new());
                gameData.dexEntries[newPokemon.dexID].dexID = newPokemon.dexID;
                gameData.dexEntries[newPokemon.dexID].forms = new();
                gameData.dexEntries[newPokemon.dexID].name = newPokemon.name;
            }
            gameData.dexEntries[newPokemon.dexID].forms.Add(newPokemon);
            foreach (Pokemon p in gameData.dexEntries[newPokemon.dexID].forms)
            {
                p.formMax = (byte)(dstFormNo + 1);
                if (gameData.dexEntries[newPokemon.dexID].forms.Count > 1)
                    p.formIndex = gameData.dexEntries[newPokemon.dexID].forms[1].personalID;
            }
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

        public void UpdatePokemonInfo(List<(int, int)> uniqueIDs, out List<(string, string)> assetBundleNames)
        {
            assetBundleNames = new();

            foreach ((int, int) uniqueIDPair in uniqueIDs)
            {
                Masterdatas.PokemonInfoCatalog srcPIC = GetPIC(uniqueIDPair.Item1);
                
                Masterdatas.PokemonInfoCatalog dstPIC = (Masterdatas.PokemonInfoCatalog)srcPIC.Clone();
                gameData.pokemonInfos.Add(dstPIC);
                dstPIC.No = uniqueIDPair.Item2 / 10000;
                dstPIC.MonsNo = dstPIC.No;
                dstPIC.FormNo = uniqueIDPair.Item2 % 10000 / 100;
                dstPIC.UniqueID = uniqueIDPair.Item2;

                if (assetBundleNames.Any(p => p.Item1 == srcPIC.AssetBundleName))
                    dstPIC.AssetBundleName = assetBundleNames.Find(p => p.Item1 == srcPIC.AssetBundleName).Item2;
                else
                {
                    dstPIC.AssetBundleName = "pm" + uniqueIDPair.Item2.ToString();
                    assetBundleNames.Add((srcPIC.AssetBundleName, dstPIC.AssetBundleName));
                }
            }
        }

        private Masterdatas.PokemonInfoCatalog GetPIC(int uniqueID)
        {
            return gameData.pokemonInfos.Find(pic => pic.UniqueID == uniqueID);
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

        public void UpdateUIMasterdatas(List<(int, int)> uniqueIDs)
        {
            foreach ((int, int) uniqueIDPair in uniqueIDs)
            {
                int srcUniqueID = uniqueIDPair.Item1;
                int dstUniqueID = uniqueIDPair.Item2;

                UIMasterdatas.PokemonIcon pokemonIcon = gameData.uiPokemonIcon.Find(o => o.UniqueID == srcUniqueID);
                UIMasterdatas.PokemonIcon newPokemonIcon = (UIMasterdatas.PokemonIcon)pokemonIcon.Clone();
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
        }

        public void DuplicateAssetBundles(Dictionary<string, string> assetBundlePaths, int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            foreach (KeyValuePair<string, string> item in assetBundlePaths)
                DuplicateAssetBundle(item.Key, item.Value, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
        }

        public BundleFileInstance DuplicateAssetBundle(string ifpath, string ofpath, int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            string baseDirectory = Environment.CurrentDirectory + "\\" + FileManager.outputModName + "\\romfs\\Data\\StreamingAssets\\AssetAssistant\\";
            ofpath = "romfs\\Data\\StreamingAssets\\AssetAssistant\\Pokemon Database\\" + ofpath;
            string newCAB = GenCABName();
            string oldPMName = string.Format("pm{0}_{1}", srcMonsNo.ToString("D4"), srcFormNo.ToString("D2"));
            string newPMName = string.Format("pm{0}_{1}", dstMonsNo.ToString("D4"), dstFormNo.ToString("D2"));

            List<AssetsReplacer> ars = new();
            AssetsManager am = fileManager.getAssetsManager();
            am.updateAfterLoad = true;
            BundleFileInstance bfi = fileManager.GetBundleFileInstance(basePath + ifpath.Replace('/', '\\'));
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

            fileManager.MakeTempBundle("romfs\\Data\\StreamingAssets\\AssetAssistant\\Pokemon Database\\" + ifpath.Replace('/', '\\'), ofpath.Replace('/', '\\'), ars, "CAB-" + newCAB);

            return bfi;
        }
    }
}