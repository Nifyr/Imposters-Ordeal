using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImpostersOrdeal.Distributions;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    /// <summary>
    ///  Responsible for all randomization related logic and execution.
    /// </summary>
    public class Randomizer
    {
        private readonly MainForm m;
        private readonly Random rng;

        public Randomizer(MainForm m)
        {
            this.m = m;
            rng = new();
        }

        /// <summary>
        ///  Randomizes everything in accordance with current configuration.
        /// </summary>
        public void Randomize()
        {
            //FixTPEVIVs();

            if (m.checkBox57.Checked)
                ScaleEvolutionLevels((double)m.numericUpDown8.Value);
            if (m.checkBox58.Checked)
                ScaleLevelUpMoves((double)m.numericUpDown8.Value);
            if (m.checkBox59.Checked)
                ScaleWildEncounters((double)m.numericUpDown8.Value);
            if (m.checkBox60.Checked)
                ScaleTrainerPokemon((double)m.numericUpDown8.Value);

            if (m.checkBox21.Checked)
                RandomizeMoveTyping(m.itemDistributionControl8.Get());
            if (m.checkBox22.Checked)
                RandomizeDamageCategory(m.itemDistributionControl9.Get());
            if (m.checkBox50.Checked)
                RandomizeTMMoves(m.itemDistributionControl18.Get());
            if (m.checkBox25.Checked)
                RandomizePower(m.numericDistributionControl8.Get());
            if (m.checkBox26.Checked)
                RandomizeAccuracy(m.numericDistributionControl10.Get());
            if (m.checkBox27.Checked)
                RandomizePP(m.numericDistributionControl11.Get());
            if (m.checkBox46.Checked)
                RandomizePrices(m.numericDistributionControl18.Get());
            if (m.checkBox47.Checked)
                RandomizePickupItems(m.itemDistributionControl16.Get());
            if (m.checkBox51.Checked)
                RandomizeShopItems(m.itemDistributionControl19.Get(), m.checkBox52.Checked);

            if (m.checkBox1.Checked)
                RandomizeEvolutionDestinations(m.button2.Get(), m.checkBox3.Checked);
            if (m.checkBox2.Checked)
                RandomizeEvolutionLevels(m.groupBox1.Get());
            if (m.checkBox5.Checked || m.checkBox7.Checked)
                RandomizeStats(m.numericDistributionControl1.Get(), m.checkBox5.Checked, m.checkBox7.Checked, m.checkBox6.Checked);
            if (m.checkBox8.Checked)
                RandomizePokemonTyping(m.itemDistributionControl1.Get(), m.checkBox61.Checked, (double)m.numericUpDown1.Value, m.rsc.evolutionLogicTypingCorrelationDistribution);
            if (m.checkBox15.Checked)
                RandomizeTMCompatibility((double)m.numericUpDown2.Value, (double)m.numericUpDown3.Value, m.checkBox16.Checked);
            if (m.checkBox10.Checked)
                RandomizeWildHeldItems(m.itemDistributionControl2.Get());
            if (m.checkBox12.Checked)
                RandomizeGrowthRates(m.itemDistributionControl3.Get());
            if (m.checkBox13.Checked)
                RandomizePersonalAbilites(m.itemDistributionControl4.Get());
            if (m.checkBox4.Checked)
                RandomizeCatchRates(m.numericDistributionControl2.Get());
            if (m.checkBox11.Checked)
                RandomizeInitialFriendship(m.numericDistributionControl4.Get());
            if (m.checkBox9.Checked)
                RandomizeEvYields(m.numericDistributionControl3.Get());
            if (m.checkBox14.Checked)
                RandomizeExpYields(m.numericDistributionControl5.Get());
            if (m.checkBox24.Checked)
                RandomizeEggMoves(m.itemDistributionControl7.Get(), (double)m.numericUpDown5.Value, m.checkBox23.Checked, m.numericDistributionControl9.Get());
            if (m.checkBox17.Checked || m.checkBox20.Checked)
                RandomizeLevelUpMoves(m.checkBox17.Checked, m.itemDistributionControl6.Get(), m.checkBox20.Checked, m.numericDistributionControl7.Get(), (double)m.numericUpDown4.Value, m.checkBox19.Checked, m.numericDistributionControl6.Get(), m.checkBox18.Checked, m.rsc.evolutionMoveCount);

            if (m.checkBox29.Checked || m.checkBox28.Checked)
                RandomizeWildEncounters(m.checkBox29.Checked, m.itemDistributionControl10.Get(), m.checkBox28.Checked, m.numericDistributionControl12.Get(), m.checkBox31.Checked, m.checkBox30.Checked);
            if (m.checkBox32.Checked)
                RandomizeTrainerItems(m.itemDistributionControl11.Get(), m.checkBox33.Checked, m.numericDistributionControl13.Get());
            if (m.checkBox36.Checked)
                RandomizeTrainerPokemonCount(m.numericDistributionControl14.Get());
            if (m.checkBox40.Checked)
                RandomizeTrainerPokemonLevels(m.numericDistributionControl15.Get());
            if (m.checkBox37.Checked)
                RandomizeTrainerPokemonSpecies(m.itemDistributionControl12.Get(), m.checkBox34.Checked, m.checkBox38.Checked, m.checkBox35.Checked);
            if (m.checkBox42.Checked)
                RandomizeTrainerPokemonHeldItems(m.itemDistributionControl15.Get(), m.checkBox43.Checked);
            if (m.checkBox41.Checked)
                RandomizeTrainerPokemonNatures(m.itemDistributionControl13.Get());
            if (m.comboBox1.SelectedIndex != 0)
                RandomizeTrainerPokemonMoves(m.comboBox1.SelectedIndex == 1, m.itemDistributionControl14.Get(), (double)m.numericUpDown7.Value);
            if (m.checkBox39.Checked)
                RandomizeTrainerPokemonShininess((double)m.numericUpDown6.Value);
            if (m.checkBox48.Checked)
                RandomizeTrainerPokemonAbilities(m.checkBox49.Checked, m.itemDistributionControl17.Get());
            if (m.checkBox44.Checked)
                RandomizeTrainerPokemonIVs(m.numericDistributionControl16.Get());
            if (m.checkBox44.Checked)
                RandomizeTrainerPokemonEVs(m.numericDistributionControl17.Get());

            if (m.checkBox63.Checked)
                RandomizeTypeMatchups(m.itemDistributionControl5.Get());
            if (m.checkBox53.Checked)
                RandomizeScriptedPokemon(m.itemDistributionControl20.Get());
            if (m.checkBox54.Checked)
                RandomizeScriptedItems(m.itemDistributionControl21.Get());
            if (m.checkBox55.Checked)
                RandomizeText(m.checkBox56.Checked);

            if (m.checkBox62.Checked)
                RandomizeMusic();
        }

        // Fix in case an older version of Imposter's Ordeal broke someone's iv and ev spreads.
        private static void FixTPEVIVs()
        {
            foreach (Trainer t in gameData.trainers)
            {
                foreach (TrainerPokemon tp in t.trainerPokemon)
                {
                    byte spAtkIV = tp.spDefIV;
                    byte spDefIV = tp.spdIV;
                    byte spdIV = tp.spAtkIV;
                    byte spAtkEV = tp.spDefEV;
                    byte spDefEV = tp.spdEV;
                    byte spdEV = tp.spAtkEV;

                    tp.spAtkIV = spAtkIV;
                    tp.spDefIV = spDefIV;
                    tp.spdIV = spdIV;
                    tp.spAtkEV = spAtkEV;
                    tp.spDefEV = spDefEV;
                    tp.spdEV = spdEV;
                }
            }
        }

        private void RandomizeTypeMatchups(IDistribution distribution)
        {
            int typeCount = 18;
            for (int o = 0; o < typeCount; o++)
                for (int d = 0; d < typeCount; d++)
                    gameData.globalMetadata.SetTypeMatchup(o, d, ToAffinity(distribution.Next(ToAffinityEnum(gameData.globalMetadata.GetTypeMatchup(o, d)))));

            gameData.SetModified(GameDataSet.DataField.GlobalMetadata);
        }

        private static int ToAffinityEnum(byte affinity)
        {
            return affinity switch
            {
                0 => 0,
                2 => 1,
                4 => 2,
                8 => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(affinity)),
            };
        }

        private static byte ToAffinity(int affinityEnum)
        {
            return affinityEnum switch
            {
                0 => 0,
                1 => 2,
                2 => 4,
                3 => 8,
                _ => throw new ArgumentOutOfRangeException(nameof(affinityEnum)),
            };
        }

        private void RandomizeMusic()
        {
            uint[] groupIDs = {
                2944413750, //BGM_BATTLE
                3369806648, //BGM_CONTEST
                1799075776, //BGM_EVENT
                3346466364  //BGM_FIELD
            };
            uint[] ignoreIDs = {
                0,
                748895195,  //NONE
                1454758594, //BA_SILENCE
                595984104,  //B_CON_SILENCE
                4176813980, //EV_SILENCE_100MS
                3652652993, //EV_SILENCE_2000MS
                789285764,  //EV_SILENCE_VS_TRAINER
                191715830,  //FI_SILENCE
                2460189219, //FI_SILENCE_ARC
                1302076938, //SILENCE_BA013
            };

            foreach (Wwise.WwiseObject wo in gameData.audioData.objectsByID.Values)
                if (wo is Wwise.MusicSwitchCntr msc)
                {
                    (Wwise.GameSync gs, int i) argumentIdx = msc.arguments.Select((gs, i) => (gs, i)).FirstOrDefault(p => groupIDs.Contains(p.gs.group));
                    if (argumentIdx.gs == null)
                        continue;
                    List<Wwise.Node> nodes = new() { msc.decisionTree };
                    for (int i = 0; i <= argumentIdx.i; i++)
                        nodes = nodes.SelectMany(n => n.nodes).ToList();
                    nodes = nodes.Where(n => !ignoreIDs.Contains(n.key)).ToList();
                    while (nodes.Any(n => n.childrenCount > 0))
                        nodes = nodes.SelectMany(n => n.nodes).ToList();
                    List<uint> anis = nodes.Select(n => n.audioNodeId).Distinct().ToList();
                    foreach (Wwise.Node n in nodes)
                        n.audioNodeId = GetRandom(anis);
                }
            gameData.SetModified(GameDataSet.DataField.AudioData);
        }

        private void ScaleTrainerPokemon(double coefficient)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                    if (IsWithin(AbsoluteBoundary.Level, trainerPokemon.level))
                        trainerPokemon.level = (byte)Conform(AbsoluteBoundary.Level, (int)(trainerPokemon.level * coefficient));

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void ScaleWildEncounters(double coefficient)
        {
            foreach (EncounterTableFile encounterTableFile in gameData.encounterTableFiles)
                foreach (EncounterTable encounterTable in encounterTableFile.encounterTables)
                {
                    List<Encounter> encounters = new();
                    encounters.AddRange(encounterTable.day);
                    encounters.AddRange(encounterTable.goodRodMons);
                    encounters.AddRange(encounterTable.groundMons);
                    encounters.AddRange(encounterTable.night);
                    encounters.AddRange(encounterTable.oldRodMons);
                    encounters.AddRange(encounterTable.superRodMons);
                    encounters.AddRange(encounterTable.swayGrass);
                    encounters.AddRange(encounterTable.tairyo);
                    encounters.AddRange(encounterTable.waterMons);
                    foreach (Encounter encounter in encounters)
                        if (IsWithin(AbsoluteBoundary.Level, (int)encounter.GetAvgLevel()))
                        {
                            encounter.minLv = Conform(AbsoluteBoundary.Level, (int)(encounter.minLv * coefficient));
                            encounter.maxLv = Conform(AbsoluteBoundary.Level, (int)(encounter.maxLv * coefficient));
                        }
                }

            foreach (UgEncounterLevelSet ugEncounterLevelSet in gameData.ugEncounterLevelSets)
                if (IsWithin(AbsoluteBoundary.Level, (int)ugEncounterLevelSet.GetAvgLevel()))
                {
                    ugEncounterLevelSet.minLv = Conform(AbsoluteBoundary.Level, (int)(ugEncounterLevelSet.minLv * coefficient));
                    ugEncounterLevelSet.maxLv = Conform(AbsoluteBoundary.Level, (int)(ugEncounterLevelSet.maxLv * coefficient));
                }

            gameData.SetModified(GameDataSet.DataField.EncounterTableFiles);
            gameData.SetModified(GameDataSet.DataField.UgEncounterLevelSets);
        }

        private void ScaleLevelUpMoves(double coefficient)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
                foreach (LevelUpMove levelUpMove in pokemon.levelUpMoves)
                    if (IsWithin(AbsoluteBoundary.Level, levelUpMove.level))
                        levelUpMove.level = (ushort)Conform(AbsoluteBoundary.Level, (int)(levelUpMove.level * coefficient));

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void ScaleEvolutionLevels(double coefficient)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                foreach (EvolutionPath evolutionPath in pokemon.evolutionPaths)
                    if (IsWithin(AbsoluteBoundary.Level, evolutionPath.level))
                        evolutionPath.level = (ushort)Conform(AbsoluteBoundary.Level, (int)(evolutionPath.level * coefficient));
                pokemon.pastEvoLvs = (0, 0);
                pokemon.nextEvoLvs = (ushort.MaxValue, ushort.MaxValue);
                pokemon.pastPokemon = new();
                pokemon.nextPokemon = new();
                pokemon.inferiorForms = new();
                pokemon.superiorForms = new();
            }

            DataParser.SetFamilies();
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeText(bool preserveStringLength)
        {
            if (gameData.messageFileSets == null)
                Task.WaitAll(DataParser.ParseAllMessageFiles());

            foreach (MessageFileSet messageFileSet in gameData.messageFileSets)
            {
                List<LabelData> labelDatas = messageFileSet.GetStrings();
                labelDatas.ForEach(l => l.wordDatas.Last().eventID = 7);

                if (!preserveStringLength)
                {
                    Shuffle(labelDatas);
                    messageFileSet.SetStrings(labelDatas);
                    continue;
                }

                List<int>[] indexes = new List<int>[10];
                for (int i = 0; i < indexes.Length; i++)
                    indexes[i] = new();
                int[] targetLists = new int[labelDatas.Count];

                for (int i = 0; i < labelDatas.Count; i++)
                {
                    int currentList;
                    if (labelDatas[i].wordDatas.Count == 1)
                    {
                        if (labelDatas[i].GetString().Length < 3)
                            currentList = 0;
                        else if (labelDatas[i].GetString().Length < 8)
                            currentList = 1;
                        else if (labelDatas[i].GetString().Length < 21)
                            currentList = 2;
                        else if (labelDatas[i].GetString().Length < 55)
                            currentList = 3;
                        else
                            currentList = 4;
                    }
                    else if (labelDatas[i].wordDatas.Count < 3)
                        currentList = 5;
                    else if (labelDatas[i].wordDatas.Count < 8)
                        currentList = 6;
                    else if (labelDatas[i].wordDatas.Count < 21)
                        currentList = 7;
                    else if (labelDatas[i].wordDatas.Count < 55)
                        currentList = 8;
                    else
                        currentList = 9;

                    targetLists[i] = currentList;
                    indexes[currentList].Add(i);
                }

                List<LabelData> oldLabelDatas = new();
                oldLabelDatas.AddRange(labelDatas);

                List<int>[] newIndexes = new List<int>[10];
                for (int i = 0; i < newIndexes.Length; i++)
                {
                    newIndexes[i] = new();
                    newIndexes[i].AddRange(indexes[i]);
                    Shuffle(newIndexes[i]);
                }

                for (int i = 0; i < labelDatas.Count; i++)
                    labelDatas[newIndexes[targetLists[i]][indexes[targetLists[i]].IndexOf(i)]] = oldLabelDatas[i];

                messageFileSet.SetStrings(labelDatas);
            }

            gameData.SetModified(GameDataSet.DataField.MessageFileSets);
        }

        private void RandomizeScriptedItems(IDistribution distribution)
        {
            foreach (EvScript evScript in gameData.evScripts)
                foreach (Script script in evScript.scripts)
                    foreach (Command command in script.commands)
                        if (command.cmdType == 187 && gameData.items[(int)command.args[0].data].IsPurchasable())
                        {
                            command.args[0].argType = 1;
                            command.args[0].data = distribution.Next((int)command.args[0].data);
                        }

            gameData.SetModified(GameDataSet.DataField.EvScripts);
        }

        private void RandomizeScriptedPokemon(IDistribution distribution)
        {
            foreach (EvScript evScript in gameData.evScripts)
                foreach (Script script in evScript.scripts)
                    foreach(Command command in script.commands)
                        if (command.cmdType == 322)
                        {
                            command.args[1].argType = 1;
                            command.args[1].data = distribution.Next((int)command.args[1].data);
                        }

            gameData.SetModified(GameDataSet.DataField.EvScripts);
        }

        private void RandomizeTrainerPokemonEVs(IDistribution distribution)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                {
                    int[] evs = new int[6];
                    int evTotal = trainerPokemon.GetEVs().Sum();
                    if (IsWithin(AbsoluteBoundary.EvTotal, evTotal))
                        evTotal = Conform(AbsoluteBoundary.EvTotal, distribution.Next(evTotal));

                    while (evTotal > 0)
                    {
                        int index = rng.Next(evs.Length);
                        int room = (int)GetBoundaries(AbsoluteBoundary.Ev)[2] - evs[index];
                        int add = rng.Next(Math.Min(room, 1), Math.Min(room, evTotal));
                        evs[index] += add;
                        evTotal -= add;
                    }

                    trainerPokemon.SetEVs(evs);
                }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonIVs(IDistribution distribution)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                    if (IsWithin(AbsoluteBoundary.Iv, (int)trainerPokemon.GetIVs().Average()))
                    {
                        trainerPokemon.hpIV = (byte)Conform(AbsoluteBoundary.Iv, distribution.Next(trainerPokemon.hpIV));
                        trainerPokemon.atkIV = (byte)Conform(AbsoluteBoundary.Iv, distribution.Next(trainerPokemon.atkIV));
                        trainerPokemon.defIV = (byte)Conform(AbsoluteBoundary.Iv, distribution.Next(trainerPokemon.defIV));
                        trainerPokemon.spAtkIV = (byte)Conform(AbsoluteBoundary.Iv, distribution.Next(trainerPokemon.spAtkIV));
                        trainerPokemon.spDefIV = (byte)Conform(AbsoluteBoundary.Iv, distribution.Next(trainerPokemon.spDefIV));
                        trainerPokemon.spdIV = (byte)Conform(AbsoluteBoundary.Iv, distribution.Next(trainerPokemon.spdIV));
                    }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonLevels(IDistribution distribution)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                    if (IsWithin(AbsoluteBoundary.Level, trainerPokemon.level))
                        trainerPokemon.level = (byte)Conform(AbsoluteBoundary.Level, distribution.Next(trainerPokemon.level));

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonCount(IDistribution distribution)
        {
            foreach (Trainer trainer in gameData.trainers)
            {
                List<TrainerPokemon> trainerPokemon = trainer.trainerPokemon;
                int trainerPokemonCount = trainerPokemon.Count;
                if (IsWithin(AbsoluteBoundary.TrainerPokemonCount, trainerPokemonCount))
                    trainerPokemonCount = Conform(AbsoluteBoundary.TrainerPokemonCount, distribution.Next(trainerPokemon.Count));
                while (trainerPokemon.Count < trainerPokemonCount)
                {
                    if (trainerPokemon.Count == 0)
                    {
                        trainerPokemon.Add(new());
                        continue;
                    }

                    trainerPokemon.Add(Copy(GetRandom(trainerPokemon)));
                }
                while (trainerPokemon.Count > trainerPokemonCount)
                    trainerPokemon.RemoveAt(rng.Next(trainerPokemon.Count));
            }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        /// <summary>
        ///  Returns a separate identical instance of a TrainerPokemon object.
        /// </summary>
        private static TrainerPokemon Copy(TrainerPokemon o)
        {
            TrainerPokemon t = new()
            {
                abilityID = o.abilityID,
                atkEV = o.atkEV,
                atkIV = o.atkIV,
                ballID = o.ballID,
                defEV = o.defEV,
                defIV = o.defIV,
                dexID = o.dexID,
                formID = o.formID,
                hpEV = o.hpEV,
                hpIV = o.hpIV,
                isRare = o.isRare,
                itemID = o.itemID,
                level = o.level,
                moveID1 = o.moveID1,
                moveID2 = o.moveID2,
                moveID3 = o.moveID3,
                moveID4 = o.moveID4,
                natureID = o.natureID,
                seal = o.seal,
                sex = o.sex,
                spAtkEV = o.spAtkEV,
                spAtkIV = o.spAtkIV,
                spDefEV = o.spDefEV,
                spDefIV = o.spDefIV,
                spdEV = o.spdEV,
                spdIV = o.spdIV
            };
            return t;
        }
        

        private void RandomizeTrainerPokemonAbilities(bool includeUnobtainable, IDistribution distribution)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                {
                    if (includeUnobtainable)
                    {
                        trainerPokemon.abilityID = (ushort)distribution.Next(trainerPokemon.abilityID);
                        continue;
                    }

                    trainerPokemon.abilityID = (ushort)GetRandom(gameData.GetPokemon(trainerPokemon.dexID, trainerPokemon.formID).GetAbilities());
                }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonShininess(double shinyP)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                    trainerPokemon.isRare = (byte)(P(shinyP) ? 1 : 0);

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonNatures(IDistribution distribution)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                    trainerPokemon.natureID = (byte)distribution.Next(trainerPokemon.natureID);

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonHeldItems(IDistribution distribution, bool levelLogic)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                {
                    if (levelLogic && !P(trainerPokemon.level))
                        continue;

                    trainerPokemon.itemID = (ushort)distribution.Next(trainerPokemon.itemID);
                }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonMoves(bool setToLevelUpMoves, IDistribution distribution, double typeBiasP)
        {
            foreach (Trainer trainer in gameData.trainers)
                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                {
                    List<ushort> moves = trainerPokemon.GetMoves();
                    Pokemon pokemon = gameData.GetPokemon(trainerPokemon.dexID, trainerPokemon.formID);

                    if (setToLevelUpMoves)
                    {
                        moves = pokemon.levelUpMoves.Where(l => l.level <= trainerPokemon.level).TakeLast(4).Select(l => l.moveID).ToList();
                        trainerPokemon.SetMoves(moves);
                        continue;
                    }

                    for (int i = 0; i < moves.Count; i++)
                    {
                        moves[i] = (ushort)distribution.Next(moves[i]);
                        if (P(typeBiasP))
                            while (!pokemon.GetTyping().Contains(gameData.moves[moves[i]].typingID))
                                moves[i] = (ushort)distribution.Next(moves[i]);
                    }

                    trainerPokemon.SetMoves(moves);
                }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerPokemonSpecies(IDistribution distribution, bool legendLogic, bool typeThemes, bool evolveLogic)
        {
            HashSet<int> legendaryDexIDs = gameData.dexEntries.Where(d => d.forms[0].legendary).Select(d => d.dexID).ToHashSet();
            foreach (Trainer trainer in gameData.trainers)
            {
                int typing = trainer.GetTypeTheme();

                foreach (TrainerPokemon trainerPokemon in trainer.trainerPokemon)
                {
                    Pokemon pokemon = GetRandom(gameData.dexEntries[distribution.Next(trainerPokemon.dexID)].forms);
                    bool acceptLegendary = !legendLogic || P(trainerPokemon.level);
                    while (typeThemes && typing != -1 && !pokemon.GetTyping().Contains(typing) || !acceptLegendary && legendaryDexIDs.Contains(pokemon.dexID))
                        pokemon = GetRandom(gameData.dexEntries[distribution.Next(pokemon.dexID)].forms);

                    if (evolveLogic)
                        pokemon = FindStage(pokemon, trainerPokemon.level, false);

                    trainerPokemon.dexID = pokemon.dexID;
                    trainerPokemon.formID = (ushort)pokemon.formID;
                    trainerPokemon.sex = 3;
                }
            }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeTrainerItems(IDistribution itemDistribution, bool randomizeItemCount, IDistribution itemCountdistribution)
        {
            foreach (Trainer trainer in gameData.trainers)
            {
                List<int> items = trainer.GetItems();
                if (randomizeItemCount)
                {
                    int itemCount = itemCountdistribution.Next(items.Count, 0, 4);
                    while (items.Count < itemCount)
                    {
                        if (items.Count == 0)
                        {
                            items.Add(itemDistribution.Next(1));
                            continue;
                        }

                        items.Add(GetRandom(items));
                    }
                    while (items.Count > itemCount)
                        items.RemoveAt(rng.Next(items.Count));
                }

                for (int i = 0; i < items.Count; i++)
                    items[i] = itemDistribution.Next(items[i]);

                trainer.SetItems(items);
                trainer.SetItemFlag();
            }

            gameData.SetModified(GameDataSet.DataField.Trainers);
        }

        private void RandomizeWildEncounters(bool randomizeSpecies, IDistribution speciesDistribution, bool randomizeLevels, IDistribution levelDistribution, bool legendLogic, bool evolveLogic)
        {
            bool randomizeEncounterTableFormIDs = gameData.Uint16EncounterTables();
            bool ugVersionsUnbounded = gameData.UgVersionsUnbounded();
            bool uint16UgTables = gameData.Uint16UgTables();
            bool randomizeUgEncounterTableFormIDs = ugVersionsUnbounded || uint16UgTables;
            foreach (EncounterTableFile encounterTableFile in gameData.encounterTableFiles)
            {
                foreach (EncounterTable encounterTable in encounterTableFile.encounterTables)
                {
                    RandomizeEncounterList(encounterTable.day, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.goodRodMons, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.groundMons, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.night, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.oldRodMons, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.superRodMons, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.swayGrass, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.tairyo, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.waterMons, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);

                    // Unused tables too, why not?
                    RandomizeEncounterList(encounterTable.gbaRuby, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.gbaSapphire, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.gbaEmerald, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.gbaFire, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                    RandomizeEncounterList(encounterTable.gbaLeaf, randomizeSpecies, speciesDistribution, randomizeLevels, levelDistribution, legendLogic, evolveLogic, randomizeEncounterTableFormIDs);
                }
                if (randomizeSpecies)
                {
                    foreach (HoneyTreeEncounter honeyTreeEncounter in encounterTableFile.honeyTreeEnconters)
                    {
                        honeyTreeEncounter.normalDexID = speciesDistribution.Next(honeyTreeEncounter.normalDexID);
                        honeyTreeEncounter.rareDexID = speciesDistribution.Next(honeyTreeEncounter.rareDexID);
                        honeyTreeEncounter.superRareDexID = speciesDistribution.Next(honeyTreeEncounter.superRareDexID);
                    }
                    for (int i = 0; i < encounterTableFile.safariMons.Count; i++)
                        encounterTableFile.safariMons[i] = speciesDistribution.Next(encounterTableFile.safariMons[i]);
                    for (int i = 0; i < encounterTableFile.trophyGardenMons.Count; i++)
                        encounterTableFile.trophyGardenMons[i] = speciesDistribution.Next(encounterTableFile.trophyGardenMons[i]);
                }
            }

            if (randomizeSpecies)
                foreach (UgEncounterFile ugEncounterFile in gameData.ugEncounterFiles)
                    for (int i = 0; i < ugEncounterFile.ugEncounters.Count; i++)
                    {
                        ugEncounterFile.ugEncounters[i].dexID = speciesDistribution.Next((ushort)ugEncounterFile.ugEncounters[i].dexID);
                        if (randomizeUgEncounterTableFormIDs)
                        {
                            ushort formID = (ushort)rng.Next(gameData.dexEntries[(ushort)ugEncounterFile.ugEncounters[i].dexID].forms.Count);
                            if (ugVersionsUnbounded)
                                ugEncounterFile.ugEncounters[i].version = formID;
                            if (uint16UgTables)
                                ugEncounterFile.ugEncounters[i].dexID += formID << 16;
                        }
                    }

            if (randomizeSpecies)
                foreach (UgSpecialEncounter ugSpecialEncounter in gameData.ugSpecialEncounters)
                    ugSpecialEncounter.dexID = speciesDistribution.Next(ugSpecialEncounter.dexID);

            if (randomizeLevels)
                foreach (UgEncounterLevelSet ugEncounterLevelSet in gameData.ugEncounterLevelSets)
                    if (IsWithin(AbsoluteBoundary.Level, (int)ugEncounterLevelSet.GetAvgLevel()))
                    {
                        ugEncounterLevelSet.minLv = Conform(AbsoluteBoundary.Level, levelDistribution.Next(ugEncounterLevelSet.minLv));
                        ugEncounterLevelSet.maxLv = Conform(AbsoluteBoundary.Level, levelDistribution.Next(ugEncounterLevelSet.maxLv));
                    }

            gameData.SetModified(GameDataSet.DataField.EncounterTableFiles);
            gameData.SetModified(GameDataSet.DataField.UgEncounterFiles);
            gameData.SetModified(GameDataSet.DataField.UgEncounterLevelSets);
            gameData.SetModified(GameDataSet.DataField.UgSpecialEncounters);
        }

        /// <summary>
        ///  Randomizes a list of Encounter objects.
        /// </summary>
        private void RandomizeEncounterList(List<Encounter> encounters, bool randomizeSpecies, IDistribution speciesDistribution, bool randomizeLevels, IDistribution levelDistribution, bool legendLogic, bool evolveLogic, bool randomizeFormIDs)
        {
            List<int> legendaryDexIDs = gameData.dexEntries.Where(d => d.forms[0].legendary).Select(d => d.dexID).ToList();
            foreach (Encounter encounter in encounters)
            {
                if (randomizeLevels && IsWithin(AbsoluteBoundary.Level, (int)encounter.GetAvgLevel()))
                {
                    encounter.minLv = Conform(AbsoluteBoundary.Level, levelDistribution.Next(encounter.minLv));
                    encounter.maxLv = Conform(AbsoluteBoundary.Level, levelDistribution.Next(encounter.maxLv));

                    if (encounter.minLv > encounter.maxLv)
                    {
                        (encounter.maxLv, encounter.minLv) = (encounter.minLv, encounter.maxLv);
                    }
                }

                if (randomizeSpecies)
                {
                    encounter.dexID = speciesDistribution.Next((ushort)encounter.dexID);
                    if (randomizeFormIDs)
                        encounter.dexID += rng.Next(gameData.dexEntries[(ushort)encounter.dexID].forms.Count) << 16;
                    if (legendLogic && !P(encounter.GetAvgLevel()))
                        while (legendaryDexIDs.Contains((ushort)encounter.dexID))
                        {
                            encounter.dexID = speciesDistribution.Next((ushort)encounter.dexID);
                            if (randomizeFormIDs)
                                encounter.dexID += rng.Next(gameData.dexEntries[(ushort)encounter.dexID].forms.Count) << 16;
                        }
                }

                if (evolveLogic)
                {
                    if (randomizeFormIDs)
                    {
                        Pokemon p = FindStage(gameData.GetPokemon((ushort)encounter.dexID, encounter.dexID >> 16), (int)encounter.GetAvgLevel(), true);
                        encounter.dexID = p.dexID + (p.formID << 16);
                    }
                    else
                        encounter.dexID = FindStage(gameData.personalEntries[(ushort)encounter.dexID], (int)encounter.GetAvgLevel(), true).dexID;
                }
            }
        }

        /// <summary>
        ///  Finds the evolution stage a certain pokemon is likely to be at for the specified level.
        /// </summary>
        private Pokemon FindStage(Pokemon pokemon, int level, bool wild)
        {
            //(wildLevel, trainerLevel)
            int pastEvoLevel = wild ? pokemon.pastEvoLvs.Item1 : pokemon.pastEvoLvs.Item2;
            if (pastEvoLevel > level)
                return FindStage(GetRandom(pokemon.pastPokemon), level, wild);
            int nextEvoLevel = wild ? pokemon.nextEvoLvs.Item1 : pokemon.nextEvoLvs.Item2;
            if (nextEvoLevel <= level)
                return FindStage(GetRandom(pokemon.nextPokemon), level, wild);
            return pokemon;
        }

        private void RandomizeShopItems(IDistribution distribution, bool preserveRegularMarts)
        {
            if (!preserveRegularMarts)
                foreach (MartItem item in gameData.shopTables.martItems)
                    item.itemID = (ushort)distribution.Next(item.itemID);

            foreach (FixedShopItem item in gameData.shopTables.fixedShopItems)
                item.itemID = (ushort)distribution.Next(item.itemID);

            foreach (BpShopItem item in gameData.shopTables.bpShopItems)
                item.itemID = (ushort)distribution.Next(item.itemID);

            gameData.SetModified(GameDataSet.DataField.ShopTables);
        }

        private void RandomizePickupItems(IDistribution distribution)
        {
            foreach (PickupItem item in gameData.pickupItems)
                item.itemID = (ushort)distribution.Next(item.itemID);

            gameData.SetModified(GameDataSet.DataField.PickupItems);
        }

        private void RandomizePrices(IDistribution distribution)
        {
            foreach (Item item in gameData.items)
                if (IsWithin(AbsoluteBoundary.Price, item.price))
                    item.price = Conform(AbsoluteBoundary.Price, distribution.Next(item.price));

            gameData.SetModified(GameDataSet.DataField.Items);
        }

        private void RandomizePP(IDistribution distribution)
        {
            foreach (Move move in gameData.moves)
                if (IsWithin(AbsoluteBoundary.Pp, move.basePP))
                    move.basePP = (byte)Conform(AbsoluteBoundary.Pp, distribution.Next(move.basePP));

            gameData.SetModified(GameDataSet.DataField.Moves);
        }

        private void RandomizeAccuracy(IDistribution distribution)
        {
            foreach (Move move in gameData.moves)
                if (IsWithin(AbsoluteBoundary.Accuracy, move.hitPer))
                    move.hitPer = (byte)Conform(AbsoluteBoundary.Accuracy, distribution.Next(move.hitPer));

            gameData.SetModified(GameDataSet.DataField.Moves);
        }

        private void RandomizePower(IDistribution distribution)
        {
            foreach (Move move in gameData.moves)
                if (IsWithin(AbsoluteBoundary.Power, move.power))
                    move.power = (byte)Conform(AbsoluteBoundary.Power, distribution.Next(move.power));

            gameData.SetModified(GameDataSet.DataField.Moves);
        }

        private void RandomizeTMMoves(IDistribution distribution)
        {
            foreach (TM tm in gameData.tms)
                tm.moveID = distribution.Next(tm.moveID);

            gameData.SetModified(GameDataSet.DataField.TMs);
        }

        private void RandomizeDamageCategory(IDistribution distribution)
        {
            foreach (Move move in gameData.moves)
                if (move.damageCategoryID != 0)
                    move.damageCategoryID = (byte)distribution.Next(move.damageCategoryID);

            gameData.SetModified(GameDataSet.DataField.Moves);
        }

        private void RandomizeMoveTyping(IDistribution distribution)
        {
            foreach (Move move in gameData.moves)
                move.typingID = (byte)distribution.Next(move.typingID);

            gameData.SetModified(GameDataSet.DataField.Moves);
        }

        private void RandomizeLevelUpMoves(bool randomizeMoves, IDistribution moveDistribution, bool randomizeLevels, IDistribution levelDistribution, double typeBiasP, bool randomizeMoveCount, IDistribution moveCountDistribution, bool sortByPower, IDistribution evolutionMoveCountDistribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                List<LevelUpMove> levelUpMoves = pokemon.levelUpMoves;

                if (!IsWithin(AbsoluteBoundary.LevelUpMoveCount, levelUpMoves.Count))
                    continue;

                if (randomizeMoveCount)
                {
                    int moveCount = levelUpMoves.Count;
                        moveCount = Conform(AbsoluteBoundary.LevelUpMoveCount, moveCountDistribution.Next(levelUpMoves.Count));
                    while (levelUpMoves.Count < moveCount)
                    {
                        if (levelUpMoves.Count == 0)
                        {
                            LevelUpMove l = new()
                            {
                                level = (ushort)Conform(AbsoluteBoundary.Level, levelDistribution.Next(1)),
                                moveID = (ushort)moveDistribution.Next(1)
                            };
                            levelUpMoves.Add(l);
                            continue;
                        }
                        levelUpMoves.Add(Copy(GetRandom(levelUpMoves)));
                    }
                    while (levelUpMoves.Count > moveCount)
                        levelUpMoves.RemoveAt(rng.Next(levelUpMoves.Count));
                }

                if (randomizeMoves)
                    foreach (LevelUpMove move in levelUpMoves)
                    {
                        move.moveID = (ushort)moveDistribution.Next(move.moveID);
                        if (P(typeBiasP))
                            while (!pokemon.GetTyping().Contains(gameData.moves[move.moveID].typingID))
                                move.moveID = (ushort)moveDistribution.Next(move.moveID);
                    }

                if (randomizeLevels)
                {
                    foreach (LevelUpMove move in levelUpMoves)
                        if (IsWithin(AbsoluteBoundary.Level, move.level))
                            move.level = (ushort)Conform(AbsoluteBoundary.Level, levelDistribution.Next(move.level));

                    if (pokemon.pastPokemon.Count > 0)
                    {
                        int evolutionMoveCount = evolutionMoveCountDistribution.Next(0);
                        LevelUpMove[] evolutionMoves = new LevelUpMove[evolutionMoveCount];
                        for (int i = 0; i < evolutionMoveCount; i++)
                            evolutionMoves[i] = GetRandom(levelUpMoves);
                        foreach (LevelUpMove move in evolutionMoves)
                            move.level = 0;
                    }
                }

                LevelUpMove firstMove = GetRandom(levelUpMoves);
                firstMove.level = 1;
                List<LevelUpMove> attacks = levelUpMoves.Where(l => IsWithin(AbsoluteBoundary.Power, gameData.moves[l.moveID].power)).ToList();
                if (attacks.Count > 0)
                {
                    LevelUpMove target = GetRandom(attacks);
                    (target.moveID, firstMove.moveID) = (firstMove.moveID, target.moveID);
                }
                else
                    while (!IsWithin(AbsoluteBoundary.Power, gameData.moves[firstMove.moveID].power))
                        firstMove.moveID = (ushort)GetRandom(gameData.moves).moveID;

                levelUpMoves.Sort((m1, m2) => m1.level - m2.level);

                if (sortByPower)
                {
                    List<LevelUpMove> attackMoves = new();
                    List<ushort> attackMoveLevels = new();
                    for (int i = 0; i < levelUpMoves.Count; i++)
                    {
                        if (levelUpMoves[i].level == 0 || !IsWithin(AbsoluteBoundary.Power, gameData.moves[levelUpMoves[i].moveID].power))
                            continue;
                        attackMoves.Add(levelUpMoves[i]);
                        attackMoveLevels.Add(levelUpMoves[i].level);
                    }
                    attackMoves.Sort((m1, m2) => gameData.moves[m1.moveID].power - gameData.moves[m2.moveID].power);
                    for (int i = 0; i < attackMoves.Count; i++)
                    {
                        attackMoves[i].level = attackMoveLevels[i];
                    }
                    levelUpMoves.Sort((m1, m2) => m1.level - m2.level);
                }
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        /// <summary>
        ///  Returns a separate identical instance of a LevelUpMove object.
        /// </summary>
        private static LevelUpMove Copy(LevelUpMove o)
        {
            LevelUpMove c = new()
            {
                level = o.level,
                moveID = o.moveID
            };
            return c;
        }

        private void RandomizeEggMoves(IDistribution moveDistribution, double typeBiasP, bool randomMoveCount, IDistribution moveCountDistribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                List<ushort> eggMoves = pokemon.eggMoves;

                if (randomMoveCount)
                {
                    int moveCount = eggMoves.Count;
                    if (IsWithin(AbsoluteBoundary.EggMoveCount, moveCount))
                        moveCount = Conform(AbsoluteBoundary.EggMoveCount, moveCountDistribution.Next(eggMoves.Count));
                    while (eggMoves.Count < moveCount)
                    {
                        if (eggMoves.Count == 0)
                        {
                            eggMoves.Add((ushort)moveDistribution.Next(1));
                            continue;
                        }
                        eggMoves.Add(GetRandom(eggMoves));
                    }
                    while (eggMoves.Count > moveCount)
                        eggMoves.RemoveAt(rng.Next(eggMoves.Count));
                }

                for (int i = 0; i < eggMoves.Count; i++)
                {
                    int moveID = moveDistribution.Next(eggMoves[i]);
                    if (P(typeBiasP))
                        while (!pokemon.GetTyping().Contains(gameData.moves[moveID].typingID))
                            moveID = moveDistribution.Next(eggMoves[i]);
                    eggMoves[i] = (ushort)moveID;
                }
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeExpYields(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
                if (IsWithin(AbsoluteBoundary.ExpYield, pokemon.giveExp))
                    pokemon.giveExp = (ushort)Conform(AbsoluteBoundary.ExpYield, distribution.Next(pokemon.giveExp));

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeEvYields(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                int[] evYield = new int[6];
                int evYieldTotal = pokemon.GetEvYield().Sum();
                if (IsWithin(AbsoluteBoundary.EvYieldTotal, evYieldTotal))
                    evYieldTotal = Conform(AbsoluteBoundary.EvYieldTotal,  distribution.Next(evYieldTotal));

                while (evYieldTotal > 0)
                {
                    int index = rng.Next(evYield.Length);
                    int room = (int)GetBoundaries(AbsoluteBoundary.EvYield)[2] - evYield[index];
                    int add = rng.Next(Math.Min(room, 1), Math.Min(room, evYieldTotal));
                    evYield[index] += add;
                    evYieldTotal -= add;
                }

                pokemon.SetEvYield(evYield);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeInitialFriendship(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
                if (IsWithin(AbsoluteBoundary.InitialFriendship, pokemon.initialFriendship))
                    pokemon.initialFriendship = (byte)Conform(AbsoluteBoundary.InitialFriendship, distribution.Next(pokemon.initialFriendship));

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeCatchRates(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
                if (IsWithin(AbsoluteBoundary.CatchRate, pokemon.getRate))
                    pokemon.getRate = (byte)Conform(AbsoluteBoundary.CatchRate, distribution.Next(pokemon.getRate));

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizePersonalAbilites(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                pokemon.abilityID1 = (ushort)distribution.Next(pokemon.abilityID1);
                pokemon.abilityID2 = (ushort)distribution.Next(pokemon.abilityID2);
                pokemon.abilityID3 = (ushort)distribution.Next(pokemon.abilityID3);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeGrowthRates(IDistribution distribution)
        {
            foreach (DexEntry dexEntry in gameData.dexEntries)
            {
                if (dexEntry.GetPastEntries().Count > 0)
                    continue;

                int grow = distribution.Next(GetRandom(dexEntry.forms).grow);

                foreach (Pokemon pokemon in dexEntry.forms)
                    pokemon.grow = (byte)grow;

                foreach (DexEntry nextDexEntry in dexEntry.GetNextEntries())
                    PropagateGrowthRate(nextDexEntry, grow);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        /// <summary>
        ///  Copies growth rate to the rest of family.
        /// </summary>
        private void PropagateGrowthRate(DexEntry dexEntry, int grow)
        {
            foreach (Pokemon pokemon in dexEntry.forms)
                pokemon.grow = (byte)grow;

            foreach (DexEntry nextDexEntry in dexEntry.GetNextEntries())
                PropagateGrowthRate(nextDexEntry, grow);
        }

        private void RandomizeWildHeldItems(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                pokemon.item1 = (ushort)distribution.Next(pokemon.item1);
                pokemon.item2 = (ushort)distribution.Next(pokemon.item2);
                pokemon.item3 = (ushort)distribution.Next(pokemon.item3);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeTMCompatibility(double compatibilityP, double typeBiasP, bool evolveLogic)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                if (pokemon.pastPokemon.Count > 0 && evolveLogic)
                    continue;

                bool[] tmCompatibility = pokemon.GetTMCompatibility();
                for (int i = 0; i < tmCompatibility.Length; i++)
                {
                    TM tm = gameData.tms[i];
                    if (!gameData.items[tm.itemID].IsActive())
                    {
                        tmCompatibility[i] = false;
                        continue;
                    }

                    if (P(typeBiasP))
                    {
                        tmCompatibility[i] = pokemon.GetTyping().Contains(gameData.moves[tm.moveID].typingID);
                        continue;
                    }

                    tmCompatibility[i] = P(compatibilityP);
                }
                pokemon.SetTMCompatibility(tmCompatibility);

                if (evolveLogic)
                    foreach (Pokemon next in pokemon.nextPokemon)
                        PropagateTMCompatibility(next);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        /// <summary>
        ///  Copies TM compatibility to the rest of family.
        /// </summary>
        private void PropagateTMCompatibility(Pokemon pokemon)
        {
            pokemon.SetTMCompatibility(GetRandom(pokemon.pastPokemon).GetTMCompatibility());
            foreach (Pokemon next in pokemon.nextPokemon)
                PropagateTMCompatibility(next);
        }

        private void RandomizePokemonTyping(IDistribution distribution, bool evolveLogic, double doubleTypingP, IDistribution typingCorrelationDistribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                if (pokemon.pastPokemon.Count > 0 && evolveLogic)
                    continue;

                List<int> oldTyping = pokemon.GetTyping();
                List<int> newTyping = new()
                {
                    distribution.Next(oldTyping.First())
                };
                if (P(doubleTypingP))
                    newTyping.Add(distribution.Next(oldTyping.Last()));
                pokemon.SetTyping(newTyping);

                if (evolveLogic)
                    foreach (Pokemon next in pokemon.nextPokemon)
                        RandomizeEvolutionTyping(next, distribution, doubleTypingP, typingCorrelationDistribution);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        /// <summary>
        ///  Randomizes typings of pokemon based on their previous stages' typing.
        /// </summary>
        private void RandomizeEvolutionTyping(Pokemon pokemon, IDistribution distribution, double doubleTypingP, IDistribution typingCorrelationDistribution)
        {
            List<int> oldTyping = pokemon.GetTyping();
            List<int> newTyping = new();
            Analyzer.TypingCorrelation tc = (Analyzer.TypingCorrelation)Enum.ToObject(typeof(Analyzer.TypingCorrelation), typingCorrelationDistribution.Next(0));
            List<int> pastTyping = GetRandom(pokemon.pastPokemon).GetTyping();
            switch (tc)
            {
                case Analyzer.TypingCorrelation.Identical:
                    newTyping.AddRange(pastTyping);
                    break;
                case Analyzer.TypingCorrelation.Addition:
                    newTyping.AddRange(pastTyping);
                    if (newTyping.Count < 2)
                    {
                        int t = newTyping[0];
                        while (t == newTyping[0])
                            t = distribution.Next(oldTyping.Last());
                        newTyping.Add(t);
                    }
                    else
                        newTyping.RemoveAt(rng.Next(newTyping.Count));
                    break;
                case Analyzer.TypingCorrelation.Swap:
                    newTyping.AddRange(pastTyping);
                    int newType = GetRandom(newTyping);
                    while (newTyping.Contains(newType))
                        newType = distribution.Next(GetRandom(oldTyping));
                    newTyping[rng.Next(newTyping.Count)] = newType;
                    break;
                case Analyzer.TypingCorrelation.NoCorrelation:
                    newTyping.Add(distribution.Next(oldTyping.First()));
                    if (P(doubleTypingP))
                        newTyping.Add(distribution.Next(oldTyping.Last()));
                    break;
            }

            pokemon.SetTyping(newTyping);

            foreach (Pokemon next in pokemon.nextPokemon)
                RandomizeEvolutionTyping(next, distribution, doubleTypingP, typingCorrelationDistribution);
        }

        /// <summary>
        ///  Returns a random element in a list.
        /// </summary>
        private T GetRandom<T>(IList<T> l)
        {
            return l[rng.Next(l.Count)];
        }

        /// <summary>
        ///  Randomly returns true a certain percentage of the time.
        /// </summary>
        private bool P(double percentage)
        {
            return rng.NextDouble() * 100 < percentage;
        }

        private void RandomizeStats(IDistribution distribution, bool shuffle, bool randomizeIndividually, bool bstLogic)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                if (shuffle)
                {
                    List<byte> list = pokemon.GetStats().ToList();
                    Shuffle(list);
                    pokemon.SetStats(list.ToArray());
                }
                if (randomizeIndividually)
                {
                    List<byte> list = pokemon.GetStats().ToList();
                    for (int i = 0; i < list.Count; i++)
                        if (IsWithin(AbsoluteBoundary.BaseStat, list[i]))
                            list[i] = (byte)Conform(AbsoluteBoundary.BaseStat, distribution.Next(list[i]));
                    pokemon.SetStats(list.ToArray());
                }
            }

            if (!bstLogic)
                return;

            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                EnsureBSTLogic(pokemon);
            }

            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        /// <summary>
        ///  Adjusts BSTs so that evolutions and surperior forms have a higher BST.
        /// </summary>
        private void EnsureBSTLogic(Pokemon pokemon)
        {
            List<Pokemon> next = new();
            List<Pokemon> past = new();
            next.AddRange(pokemon.nextPokemon);
            next.AddRange(pokemon.superiorForms);
            past.AddRange(pokemon.pastPokemon);
            past.AddRange(pokemon.inferiorForms);

            foreach (Pokemon nextPokemon in next)
            {
                if (pokemon.GetBST() > nextPokemon.GetBST())
                {
                    FixBST(pokemon, nextPokemon);
                    EnsureBSTLogic(nextPokemon);
                }
            }
            foreach (Pokemon pastPokemon in past)
            {
                if (pokemon.GetBST() < pastPokemon.GetBST())
                {
                    FixBST(pastPokemon, pokemon);
                    EnsureBSTLogic(pastPokemon);
                }
            }
        }

        /// <summary>
        ///  Swaps stats until past's BST is lower or equal to next's.
        /// </summary>
        private void FixBST (Pokemon past, Pokemon next)
        {
            List<byte> pastStats = past.GetStats().ToList();
            List<byte> nextStats = next.GetStats().ToList();

            while (pastStats.Sum(b => (int)b) > nextStats.Sum(b => (int)b))
            {
                int index = -1;
                int maxDelta = 0;
                for (int i = 0; i < pastStats.Count; i++)
                {
                    int delta = pastStats[i] - nextStats[i];
                    if (delta > maxDelta)
                    {
                        index = i;
                        maxDelta = delta;
                    }
                }
                (nextStats[index], pastStats[index]) = (pastStats[index], nextStats[index]);
            }

            past.SetStats(pastStats.ToArray());
            next.SetStats(nextStats.ToArray());
        }

        /// <summary>
        ///  Shuffles a list uniformely.
        /// </summary>
        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        private void RandomizeEvolutionLevels(IDistribution distribution)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                foreach (EvolutionPath evolution in pokemon.evolutionPaths)
                    if (evolution.destDexID != pokemon.dexID && IsWithin(AbsoluteBoundary.Level, evolution.level))
                        evolution.level = (ushort)Conform(AbsoluteBoundary.Level, distribution.Next(evolution.level));
                pokemon.pastEvoLvs = (0, 0);
                pokemon.nextEvoLvs = (ushort.MaxValue, ushort.MaxValue);
                pokemon.pastPokemon = new();
                pokemon.nextPokemon = new();
                pokemon.inferiorForms = new();
                pokemon.superiorForms = new();
            }

            DataParser.SetFamilies();
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }

        private void RandomizeEvolutionDestinations(IDistribution distribution, bool bstLogic)
        {
            foreach (Pokemon pokemon in gameData.personalEntries)
            {
                foreach (EvolutionPath evolution in pokemon.evolutionPaths)
                {
                    if (pokemon.dexID == evolution.destDexID)
                        continue;
                    Pokemon dest;
                    do
                    {
                        dest = gameData.personalEntries[distribution.Next(gameData.GetPokemon(evolution.destDexID, evolution.destFormID).personalID)];
                    } while (bstLogic && dest.GetBST() <= pokemon.GetBST());
                    evolution.destDexID = dest.dexID;
                    evolution.destFormID = (ushort)dest.formID;
                }
                pokemon.pastEvoLvs = (0, 0);
                pokemon.nextEvoLvs = (ushort.MaxValue, ushort.MaxValue);
                pokemon.pastPokemon = new();
                pokemon.nextPokemon = new();
                pokemon.inferiorForms = new();
                pokemon.superiorForms = new();
            }

            DataParser.SetFamilies();
            gameData.SetModified(GameDataSet.DataField.PersonalEntries);
        }
    }
}
