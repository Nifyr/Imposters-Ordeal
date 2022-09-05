using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;
using static ImpostersOrdeal.Wwise;
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

        private static AssetInserter instance;
        private readonly Random rnd;
        private readonly Dictionary<string, string> CABNames;
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
                values[i] = rnd.Next().ToString("x8");
            }

            return string.Join("", values);
        }

        public int GenUniqueID(int monsNo, int formNo, int gender, bool isRare)
        {
            return monsNo * 10000 + formNo * 100 + gender * 10 + (isRare ? 1 : 0);
        }

        public void InsertPokemon(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, string speciesName, string formName)
        {
            List<(int, int)> uniqueIDs = GetUniqueIDs(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdatePokemonInfo(uniqueIDs, out List<(string, string)> assetBundleNames);
            UpdateDprBin(assetBundleNames, out Dictionary<string, string> assetBundlePaths);
            UpdateUIMasterdatas(uniqueIDs);
            UpdateAudioBank(uniqueIDs, out (uint, uint) sourceID);
            DuplicateAudioSource(sourceID.Item1, sourceID.Item2);
            UpdateAddPersonalTable(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            UpdateMotionTimingData(uniqueIDs);
            UpdatePersonalInfos(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, speciesName);
            UpdateCommonMsbt(srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, speciesName, formName);
            DuplicateAssetBundles(assetBundlePaths, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo);
            gameData.SetModified(GameDataSet.DataField.UIMasterdatas);
            gameData.SetModified(GameDataSet.DataField.AddPersonalTable);
            gameData.SetModified(GameDataSet.DataField.MotionTimingData);
            gameData.SetModified(GameDataSet.DataField.PokemonInfo);
            gameData.SetModified(GameDataSet.DataField.MessageFileSets);
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
            gameData.SetModified(GameDataSet.DataField.DprBin);
            gameData.SetModified(GameDataSet.DataField.AudioData);
        }

        private static void DuplicateAudioSource(uint srcID, uint dstID) => fileManager.DuplicateAudioSource(srcID, dstID);

        private void UpdateAudioBank(List<(int, int)> uniqueIDs, out (uint, uint) sourceID)
        {
            (int, int) uniqueID = uniqueIDs.First();
            Dictionary<uint, WwiseObject> lookup = gameData.audioData.objectsByID;
            HircChunk hc = (HircChunk)gameData.audioData.banks.First().chunks.Find(c => c is HircChunk);

            Event e = (Event)lookup[FNV132(GetWwiseEvents(uniqueID.Item1).First())];
            ActionPlay ap = (ActionPlay)lookup[e.actionIDs.First()];
            WwiseObject wo = lookup[ap.idExt];
            Sound s;
            if (wo is Sound s0)
                s = s0;
            else
            {
                SwitchCntr sc = (SwitchCntr)wo;
                s = (Sound)lookup[sc.children.childIDs.First()];
            }
            sourceID = (s.bankSourceData.mediaInformation.sourceID, NextUInt32(gameData.audioData));

            Sound newS = (Sound)s.Clone();
            newS.id = NextUInt32(gameData.audioData);
            newS.nodeBaseParams.directParentID = NextUInt32(gameData.audioData);
            newS.bankSourceData.mediaInformation.sourceID = sourceID.Item2;
            gameData.audioData.objectsByID[newS.id] = newS;
            hc.loadedItem.Add(newS);

            if (uniqueID.Item1 / 10000 != 25 && uniqueID.Item1 / 10000 != 133)
            {
                ActorMixer am = (ActorMixer)lookup[s.nodeBaseParams.directParentID];
                ActorMixer amParent = (ActorMixer)lookup[am.nodeBaseParams.directParentID];
                ActorMixer newAM = (ActorMixer)am.Clone();
                newAM.id = newS.nodeBaseParams.directParentID;
                newAM.children.childIDs = new() { newS.id };
                amParent.children.childIDs.Add(newAM.id);
                gameData.audioData.objectsByID[newAM.id] = newAM;
                hc.loadedItem.Add(newAM);
            }
            else
                newS.nodeBaseParams.directParentID = 0;

            ActionPlay newAP = (ActionPlay)ap.Clone();
            newAP.id = NextUInt32(gameData.audioData);
            newAP.idExt = newS.id;
            gameData.audioData.objectsByID[newAP.id] = newAP;
            hc.loadedItem.Add(newAP);

            List<uint> newEventIDs = GetWwiseEvents(uniqueID.Item2).Select(s => FNV132(s)).ToList();
            foreach (uint newEventID in newEventIDs)
            {
                Event newE = (Event)e.Clone();
                newE.id = newEventID;
                newE.actionIDs = new();
                newE.actionIDs.Add(newAP.id);
                gameData.audioData.objectsByID[newE.id] = newE;
                hc.loadedItem.Add(newE);
            }
        }

        private uint NextUInt32(WwiseData wd)
        {
            uint output;
            do
            {
                byte[] uintBytes = new byte[4];
                rnd.NextBytes(uintBytes);
                output = BitConverter.ToUInt32(uintBytes);
            } while (wd.objectsByID.ContainsKey(output));
            return output;
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

            List<AssetBundleRecord> recordsToClone = new();
            foreach ((string, string) assetBundleNamePair in assetBundleNames)
            {
                List<AssetBundleRecord> recordsToAdd = abdm.GetAssetBundleRecordsWithDependencies("pokemons/battle/" + assetBundleNamePair.Item1, true).ToList();
                recordsToAdd.AddRange(abdm.GetAssetBundleRecordsWithDependencies("pokemons/field/" + assetBundleNamePair.Item1, true));
                foreach (AssetBundleRecord record in recordsToAdd)
                    if (record.projectName == "Pokemon Database" &&
                        !assetBundlePaths.ContainsKey(record.assetBundleName) &&
                        !assetBundlePathsWithDependencies.ContainsKey(record.assetBundleName))
                    {
                        recordsToClone.Add(record);
                        string newAssetBundlePath = Path.Combine(Path.GetDirectoryName(record.assetBundleName),
                            GenAssetBundleName(assetBundleNamePair.Item2, Path.GetFileName(record.assetBundleName).Length)).Replace('\\', '/');

                        while (assetBundlePaths.Values.Contains(newAssetBundlePath) || assetBundlePathsWithDependencies.Values.Contains(newAssetBundlePath) ||
                            assetBundlePaths.Keys.Contains(newAssetBundlePath) || assetBundlePathsWithDependencies.Keys.Contains(newAssetBundlePath))
                            newAssetBundlePath = Path.Combine(Path.GetDirectoryName(record.assetBundleName),
                                GenAssetBundleName(int.Parse(assetBundleNamePair.Item2.Replace("pm", "").Replace("_", "")) + 2,
                                Path.GetFileName(record.assetBundleName).Length)).Replace('\\', '/');


                        if (record.allDependencies.Length == 0)
                            assetBundlePaths[record.assetBundleName] = newAssetBundlePath;
                        else
                            assetBundlePathsWithDependencies[record.assetBundleName] = newAssetBundlePath;
                    }
            }
            recordsToClone.Sort((r1, r2) => r1.allDependencies.Length - r2.allDependencies.Length);
            foreach (KeyValuePair<string, string> pair in assetBundlePathsWithDependencies)
                assetBundlePaths.Add(pair.Key, pair.Value);

            foreach (AssetBundleRecord record in recordsToClone)
            {
                AssetBundleRecord newRecord = (AssetBundleRecord)record.Clone();
                newRecord.assetBundleName = assetBundlePaths[newRecord.assetBundleName];

                for (int i = 0; i < newRecord.assetPaths.Length; i++)
                    newRecord.assetPaths[i] = ReplacePM(newRecord.assetPaths[i], Path.GetFileName(newRecord.assetBundleName));

                // What a surprise, gender variations in the same bundle! (sometimes)
                // Let's just get rid of the duplicates. Probably didn't need them anyway...
                newRecord.assetPaths = newRecord.assetPaths.Distinct().ToArray();

                for (int i = 0; i < newRecord.allDependencies.Length; i++)
                    if (assetBundlePaths.ContainsKey(newRecord.allDependencies[i]))
                        newRecord.allDependencies[i] = assetBundlePaths[newRecord.allDependencies[i]];

                abdm.Add(newRecord);
            }
        }

        private string ReplacePM(string target, string reference)
        {
            string name6 = reference[..6];
            string name9 = reference[..9];
            string name12 = null;
            if (reference.Length >= 12)
                name12 = reference[..12];
            target = Regex.Replace(target, @"pm\d{4}", name6);
            target = Regex.Replace(target, @"pm\d{4}_\d{2}", name9);
            if (name12 != null)
                target = Regex.Replace(target, @"pm\d{4}_\d{2}_\d{2}", name12);
            return target;
        }

        public void UpdatePersonalInfos(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, string speciesName)
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
                newPokemon.name = speciesName;
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

        public void UpdateCommonMsbt(int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, string speciesName, string formName)
        {
            bool newSpecies = dstFormNo == 0;
            foreach (MessageFileSet msgFileSet in gameData.messageFileSets)
            {
                foreach (MessageFile msgFile in msgFileSet.messageFiles)
                {
                    if (newSpecies && (msgFile.mName.Contains("dlp_whitelist_monsname") || msgFile.mName.Contains("ss_monsname")))
                    {
                        string srcLabelName = "MONSNAME_" + srcMonsNo.ToString("D3");
                        string dstLabelName = "MONSNAME_" + dstMonsNo.ToString("D3");
                        LabelData srcLabelData = msgFile.labelDatas.Find(mf => mf.labelName == srcLabelName);
                        LabelData newLabelData = (LabelData)srcLabelData.Clone();
                        newLabelData.labelName = dstLabelName;
                        newLabelData.labelIndex = dstMonsNo;
                        newLabelData.arrayIndex = newLabelData.labelIndex;
                        newLabelData.wordDatas[0].str = speciesName;

                        if (dstMonsNo < msgFile.labelDatas.Count)
                            msgFile.labelDatas[dstMonsNo] = newLabelData;
                        else
                            msgFile.labelDatas.Add(newLabelData);
                    }

                    if (newSpecies && msgFile.mName.Contains("dp_pokedex_diamond"))
                    {
                        string srcLabelName = "DP_pokedex_diamond_" + srcMonsNo.ToString("D3");
                        string dstLabelName = "DP_pokedex_diamond_" + dstMonsNo.ToString("D3");
                        LabelData srcLabelData = msgFile.labelDatas.Find(mf => mf.labelName == srcLabelName);
                        LabelData newLabelData = (LabelData)srcLabelData.Clone();
                        newLabelData.labelName = dstLabelName;
                        newLabelData.labelIndex = dstMonsNo;
                        newLabelData.arrayIndex = newLabelData.labelIndex;

                        msgFile.labelDatas.Add(newLabelData);
                    }

                    if (newSpecies && msgFile.mName.Contains("dp_pokedex_pearl"))
                    {
                        string srcLabelName = "DP_pokedex_pearl_" + srcMonsNo.ToString("D3");
                        string dstLabelName = "DP_pokedex_pearl_" + dstMonsNo.ToString("D3");
                        LabelData srcLabelData = msgFile.labelDatas.Find(mf => mf.labelName == srcLabelName);
                        LabelData newLabelData = (LabelData)srcLabelData.Clone();
                        newLabelData.labelName = dstLabelName;
                        newLabelData.labelIndex = dstMonsNo;
                        newLabelData.arrayIndex = newLabelData.labelIndex;

                        msgFile.labelDatas.Add(newLabelData);
                    }

                    if (newSpecies && msgFile.mName.Contains("ss_zkn_type"))
                    {
                        string srcLabelName = "ZKN_TYPE_" + srcMonsNo.ToString("D3");
                        string dstLabelName = "ZKN_TYPE_" + dstMonsNo.ToString("D3");
                        LabelData srcLabelData = msgFile.labelDatas.Find(mf => mf.labelName == srcLabelName);
                        LabelData newLabelData = (LabelData)srcLabelData.Clone();
                        newLabelData.labelName = dstLabelName;
                        newLabelData.labelIndex = dstMonsNo;
                        newLabelData.arrayIndex = newLabelData.labelIndex;

                        msgFile.labelDatas.Add(newLabelData);
                    }

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
                Masterdatas.PokemonInfoCatalog srcPIC = gameData.pokemonInfos.Find(pic => pic.UniqueID == uniqueIDPair.Item1);
                
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
                    dstPIC.AssetBundleName = GenAssetBundleName(uniqueIDPair.Item2, srcPIC.AssetBundleName.Length, true);
                    assetBundleNames.Add((srcPIC.AssetBundleName, dstPIC.AssetBundleName));
                }
            }
        }

        private static string GenAssetBundleName(string oldFileName, int newLength) =>
            GenAssetBundleName(int.Parse(oldFileName.Replace("pm", "").Replace("_", "")), newLength);

        private static string GenAssetBundleName(int uniqueID, int length, bool adjustFormID = false)
        {
            //I've developed a strong distaste for this naming scheme lately. 
            if (adjustFormID)
                uniqueID += 1100;
            return length switch
            {
                9 => "pm" + (uniqueID / 10000).ToString("D4") +
                "_" + (uniqueID / 100 % 100).ToString("D2"),
                12 => "pm" + (uniqueID / 10000).ToString("D4") +
                "_" + (uniqueID / 100 % 100).ToString("D2") +
                "_" + (uniqueID % 100).ToString("D2"),
                _ => throw new NotImplementedException("Assetbundle file name of length " + length + " unsupported."),
            };
        }

        public void UpdateMotionTimingData(List<(int src, int dst)> uniqueIDs)
        {
            foreach ((int src, int dst) pair in uniqueIDs)
            {
                int srcMonsNo = pair.src / 10000;
                int srcFormNo = pair.src / 100 % 100;
                int srcSex = pair.src / 10 % 10;
                int dstMonsNo = pair.dst / 10000;
                int dstFormNo = pair.dst / 100 % 100;
                int dstSex = pair.dst / 10 % 10;
                if (gameData.motionTimingData.Any(o => o.MonsNo == dstMonsNo && o.FormNo == dstFormNo && o.Sex == dstSex))
                    continue;
                BattleMasterdatas.MotionTimingData baseMotionTimingData = null;
                foreach (BattleMasterdatas.MotionTimingData motionTimingData in gameData.motionTimingData)
                {
                    if (motionTimingData.MonsNo == srcMonsNo && motionTimingData.FormNo == srcFormNo && motionTimingData.Sex == srcSex)
                    {
                        baseMotionTimingData = motionTimingData;
                        break;
                    }
                }

                BattleMasterdatas.MotionTimingData newMotionTimingData = (BattleMasterdatas.MotionTimingData)baseMotionTimingData.Clone();
                newMotionTimingData.MonsNo = dstMonsNo;
                newMotionTimingData.FormNo = dstFormNo;
                newMotionTimingData.Sex = dstSex;
                gameData.motionTimingData.Add(newMotionTimingData);
            }
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

                UIMasterdatas.PokemonIcon pokemonIcon = gameData.uiPokemonIcon.Find(o => o.uniqueID == srcUniqueID);
                UIMasterdatas.PokemonIcon newPokemonIcon = (UIMasterdatas.PokemonIcon)pokemonIcon.Clone();
                newPokemonIcon.uniqueID = dstUniqueID;
                gameData.uiPokemonIcon.Add(newPokemonIcon);

                UIMasterdatas.AshiatoIcon ashiatoIcon = gameData.uiAshiatoIcon.Find(o => o.uniqueID == srcUniqueID);
                UIMasterdatas.AshiatoIcon newAshiatoIcon = (UIMasterdatas.AshiatoIcon)ashiatoIcon.Clone();
                newAshiatoIcon.uniqueID = dstUniqueID;
                gameData.uiAshiatoIcon.Add(newAshiatoIcon);

                UIMasterdatas.PokemonVoice pokemonVoice = gameData.uiPokemonVoice.Find(o => o.uniqueID == srcUniqueID);
                UIMasterdatas.PokemonVoice newPokemonVoice = (UIMasterdatas.PokemonVoice)pokemonVoice.Clone();
                newPokemonVoice.wwiseEvent = GetWwiseEvents(dstUniqueID).First();
                newPokemonVoice.uniqueID = dstUniqueID;
                gameData.uiPokemonVoice.Add(newPokemonVoice);

                UIMasterdatas.ZukanDisplay zukanDisplay = gameData.uiZukanDisplay.Find(o => o.uniqueID == srcUniqueID);
                UIMasterdatas.ZukanDisplay newZukanDisplay = (UIMasterdatas.ZukanDisplay)zukanDisplay.Clone();
                newZukanDisplay.uniqueID = dstUniqueID;
                gameData.uiZukanDisplay.Add(newZukanDisplay);

                UIMasterdatas.ZukanCompareHeight zukanCompareHeight = gameData.uiZukanCompareHeights.Find(o => o.uniqueID == srcUniqueID);
                UIMasterdatas.ZukanCompareHeight newZukanCompareHeight = (UIMasterdatas.ZukanCompareHeight)zukanCompareHeight.Clone();
                newZukanCompareHeight.uniqueID = dstUniqueID;
                gameData.uiZukanCompareHeights.Add(newZukanCompareHeight);

                if (gameData.uiSearchPokeIconSex[0].monsNo == dstUniqueID / 10000)
                {
                    gameData.uiSearchPokeIconSex[0].monsNo++;
                    UIMasterdatas.SearchPokeIconSex searchPokeIconSex = gameData.uiSearchPokeIconSex.Find(o => o.monsNo == srcUniqueID / 10000);
                    UIMasterdatas.SearchPokeIconSex newSearchPokeIconSex = (UIMasterdatas.SearchPokeIconSex)searchPokeIconSex.Clone();
                    newSearchPokeIconSex.monsNo = dstUniqueID / 10000;
                    gameData.uiSearchPokeIconSex.Add(newSearchPokeIconSex);
                }
            }
        }

        private static List<string> GetWwiseEvents(int uniqueID)
        {
            List<string> events = new();
            string[] prefixes = new string[]
            {
                "PLAY_PV", "PLAY_PV_BTL", "PLAY_PV_EV"
            };
            foreach (string p in prefixes)
                for (int i = 0; i < 5; i++)
                    events.Add(p + "_" + (uniqueID / 10000).ToString("D3") + "_" +
                        (uniqueID / 100 % 100).ToString("D2") + "_" + i.ToString("D2"));
            return events;
        }

        private void DuplicateAssetBundles(Dictionary<string, string> assetBundlePaths, int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo)
        {
            CloneMode c = CloneMode.Unk;
            foreach (KeyValuePair<string, string> item in assetBundlePaths)
                c = DuplicateAssetBundle(item.Key, item.Value, srcMonsNo, dstMonsNo, srcFormNo, dstFormNo, c);
        }

        private enum CloneMode
        {
            Unk, Mod, Dump
        }

        private CloneMode DuplicateAssetBundle(string srcPath, string dstPath, int srcMonsNo, int dstMonsNo, int srcFormNo, int dstFormNo, CloneMode c)
        {
            string baseDirectory = Environment.CurrentDirectory + "\\" + FileManager.outputModName + "\\romfs\\Data\\StreamingAssets\\AssetAssistant\\";
            string newCAB = GenCABName();
            string refPM = Path.GetFileNameWithoutExtension(dstPath);

            List<AssetsReplacer> ars = new();
            AssetsManager am = fileManager.getAssetsManager();
            am.updateAfterLoad = true;
            BundleFileInstance bfi = null;
            switch (c)
            {
                case CloneMode.Unk:
                    bfi = fileManager.TryGetPokemonBundleFileInstance(dstPath.Replace('/', '\\'));
                    if (bfi == null)
                    {
                        bfi = fileManager.GetPokemonBundleFileInstance(srcPath.Replace('/', '\\'));
                        c = CloneMode.Dump;
                    }
                    else
                    {
                        srcPath = dstPath;
                        c = CloneMode.Mod;
                    }
                    break;
                case CloneMode.Mod:
                    srcPath = dstPath;
                    bfi = fileManager.GetPokemonBundleFileInstance(srcPath.Replace('/', '\\'));
                    break;
                case CloneMode.Dump:
                    bfi = fileManager.GetPokemonBundleFileInstance(srcPath.Replace('/', '\\'));
                    break;
            }

            bfi.name = ReplacePM(bfi.name, refPM);

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
                m_Name = ReplacePM(m_Name, refPM);
                m_StreamDataPath = m_StreamDataPath.Replace(oldCAB, newCAB);
                texture2DField["m_Name"].GetValue().Set(m_Name);
                texture2DField["m_StreamData"].children[2].GetValue().Set(m_StreamDataPath);

                byte[] b = texture2DField.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }
            /*
            List<AssetTypeValueField> gameObjects = afi.table.GetAssetsOfType((int)AssetClassID.GameObject).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField gameObject;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObject = gameObjects[i];

                string m_Name = gameObject["m_Name"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.GameObject);

                m_Name = ReplacePM(m_Name, refPM);
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

                m_Name = ReplacePM(m_Name, refPM);
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

                m_Name = ReplacePM(m_Name, refPM);
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

                m_Name = ReplacePM(m_Name, refPM);
                animationClip["m_Name"].GetValue().Set(m_Name);

                byte[] b = animationClip.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }
            */
            List<AssetTypeValueField> assetBundles = afi.table.GetAssetsOfType((int)AssetClassID.AssetBundle).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField assetBundle;
            for (int i = 0; i < assetBundles.Count; i++)
            {
                assetBundle = assetBundles[i];

                string m_Name = assetBundle["m_Name"].value.AsString();
                string m_AssetBundleName = assetBundle["m_AssetBundleName"].value.AsString();
                AssetFileInfoEx afie = afi.table.GetAssetInfo(m_Name, (int)AssetClassID.AssetBundle);

                m_Name = ReplacePM(m_Name, refPM);
                m_AssetBundleName = ReplacePM(m_AssetBundleName, refPM);
                assetBundle["m_Name"].GetValue().Set(m_Name);
                assetBundle["m_AssetBundleName"].GetValue().Set(m_AssetBundleName);

                AssetTypeValueField m_Dependencies = assetBundle["m_Dependencies"][0];
                for (int j = 0; j < m_Dependencies.childrenCount; j++)
                {
                    AssetTypeValueField entry = m_Dependencies.children[j];
                    string key = entry.value.AsString();
                    key = ReplacePM(key, refPM);
                    entry.GetValue().Set(key);
                }

                AssetTypeValueField m_Container = assetBundle["m_Container"][0];

                for (int j = 0; j < m_Container.childrenCount; j++)
                {
                    AssetTypeValueField entry = m_Container.children[j];
                    string key = entry.children[0].value.AsString();
                    key = ReplacePM(key, refPM);
                    entry.children[0].GetValue().Set(key);
                }

                byte[] b = assetBundle.WriteToByteArray();
                AssetsReplacerFromMemory arfm = new(0, afie.index, (int)afie.curFileType, AssetHelper.GetScriptIndex(afi.file, afie), b);
                ars.Add(arfm);
            }
            
            // TODO: Update AABBData in PokemonPrefabInfo
            // TODO: Update FieldPokemonEntity
            
            am.UpdateDependencies(afi);
            
            if (c == CloneMode.Mod)
                srcPath = dstPath;

            fileManager.MakeTempBundle("romfs\\Data\\StreamingAssets\\AssetAssistant\\Pokemon Database\\" + srcPath.Replace('/', '\\'),
                "romfs\\Data\\StreamingAssets\\AssetAssistant\\Pokemon Database\\" + dstPath.Replace('/', '\\'), ars, "CAB-" + newCAB);

            return c;
        }
    }
}