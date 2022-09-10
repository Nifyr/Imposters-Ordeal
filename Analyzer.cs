using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImpostersOrdeal.GlobalData;
using static ImpostersOrdeal.Distributions;
using static ImpostersOrdeal.GameDataTypes;

namespace ImpostersOrdeal
{
    /// <summary>
    ///  Responsible for figuring out a good starting configuration given the loaded files.
    /// </summary>
    public static class Analyzer
    {
        /// <summary>
        ///  Generates RandomizerSetupConfig through statistical analysis of gamefiles.
        /// </summary>
        public static MainForm.RandomizerSetupConfig GetSetupConfig()
        {
            MainForm.RandomizerSetupConfig randomizerSetupConfig = new();

            //Evolution Destinations
            int[] instances = new int[gameData.personalEntries.Count];
            List<INamedEntity> entities = gameData.personalEntries.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                List<Pokemon> nextPokemon = gameData.personalEntries[personalID].nextPokemon;
                for (int evoPath = 0; evoPath < nextPokemon.Count; evoPath++)
                    instances[nextPokemon[evoPath].GetID()]++;
            }
            randomizerSetupConfig.evolutionDestinationPokemon = ToItemDistributionConfig(instances, entities);

            //Evolution Levels
            List<int> observations = new();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                for (int item = 0; item < gameData.personalEntries[personalID].evolutionPaths.Count; item++)
                    if (IsWithin(AbsoluteBoundary.Level, gameData.personalEntries[personalID].evolutionPaths[item].level))
                        observations.Add(gameData.personalEntries[personalID].evolutionPaths[item].level);
            }
            randomizerSetupConfig.evolutionLevel = ToNumericDistributionConfig(observations);

            //Base Stats
            observations = new();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                Pokemon p = gameData.personalEntries[personalID];
                if (IsWithin(AbsoluteBoundary.BaseStat, p.basicHp))
                    observations.Add(p.basicHp);
                if (IsWithin(AbsoluteBoundary.BaseStat, p.basicAtk))
                    observations.Add(p.basicAtk);
                if (IsWithin(AbsoluteBoundary.BaseStat, p.basicDef))
                    observations.Add(p.basicDef);
                if (IsWithin(AbsoluteBoundary.BaseStat, p.basicSpAtk))
                    observations.Add(p.basicSpAtk);
                if (IsWithin(AbsoluteBoundary.BaseStat, p.basicSpDef))
                    observations.Add(p.basicSpDef);
                if (IsWithin(AbsoluteBoundary.BaseStat, p.basicSpd))
                    observations.Add(p.basicSpd);
            }
            randomizerSetupConfig.baseStats = ToNumericDistributionConfig(observations);

            //Pokémon Typing
            instances = new int[gameData.typings.Count];
            entities = gameData.typings.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                List<int> typing = gameData.personalEntries[personalID].GetTyping();
                for (int i = 0; i < typing.Count; i++)
                    instances[typing[i]]++;
            }
            randomizerSetupConfig.pokemonTyping = ToItemDistributionConfig(instances, entities);

            //Pokémon Typing
            randomizerSetupConfig.doubleTypingP = GetOccurrencePercent(gameData.personalEntries, p => p.typingID1 != p.typingID2);

            //Typing Evolution Logic
            instances = new int[4];
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                Pokemon p = gameData.personalEntries[personalID];
                for (int path = 0; path < p.nextPokemon.Count; path++)
                    instances[(int)p.CompareTyping(p.nextPokemon[path])]++;
            }
            randomizerSetupConfig.evolutionLogicTypingCorrelationDistribution = new Empirical(100, instances.ToList());

            //TM Compatibility
            instances = new int[gameData.tms.Count];
            entities = gameData.tms.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                bool[] tmCompatibility = gameData.personalEntries[personalID].GetTMCompatibility();
                for (int tmID = 0; tmID < tmCompatibility.Length; tmID++)
                    if (tmCompatibility[tmID])
                        instances[tmID]++;
            }
            List<int> validInstances = new();
            for (int tmID = 0; tmID < instances.Length; tmID++)
                if (gameData.tms[tmID].IsValid())
                    validInstances.Add(instances[tmID]);
            randomizerSetupConfig.tmCompatibilityP = 100 * validInstances.Average() / gameData.personalEntries.Count;

            //TM Type Bias
            List<(Pokemon, Move)> movePairing = gameData.personalEntries.SelectMany(p => p.GetCompatibleTMs().Select(t => (p, gameData.moves[gameData.tms[t].moveID]))).ToList();
            double sameTypePercent = GetOccurrencePercent(movePairing, pm => pm.Item1.GetTyping().Contains(pm.Item2.typingID));
            double unbiasedSameTypePercent = (100 + randomizerSetupConfig.doubleTypingP) / gameData.typings.Count;
            randomizerSetupConfig.tmCompatibilityTypeBiasP = 100 * (sameTypePercent - unbiasedSameTypePercent) / (100 - unbiasedSameTypePercent);
            if (randomizerSetupConfig.tmCompatibilityTypeBiasP < 0)
                randomizerSetupConfig.tmCompatibilityTypeBiasP = 0;

            //Wild Held Items
            instances = new int[gameData.items.Count];
            entities = gameData.items.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                int[] wildHeldItems = gameData.personalEntries[personalID].GetWildHeldItems();
                for (int i = 0; i < wildHeldItems.Length; i++)
                    instances[wildHeldItems[i]]++;
            }
            randomizerSetupConfig.wildHeldItems = ToItemDistributionConfig(instances, entities);

            //Growth Rate
            randomizerSetupConfig.growthRate = GetItemDistributionConfig(gameData.personalEntries, p => p.grow, gameData.growthRates.Select(o => (INamedEntity)o).ToList());

            //Abilities
            instances = new int[gameData.abilities.Count];
            entities = gameData.abilities.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
            {
                int[] abilities = gameData.personalEntries[personalID].GetAbilities();
                for (int i = 0; i < abilities.Length; i++)
                    instances[abilities[i]]++;
            }
            randomizerSetupConfig.abilities = ToItemDistributionConfig(instances, entities);
            
            //Catch Rate
            randomizerSetupConfig.catchRate = GetNumericDistributionConfig(gameData.personalEntries, p => p.getRate, AbsoluteBoundary.CatchRate);

            //Ev Yield
            randomizerSetupConfig.evYields = GetNumericDistributionConfig(gameData.personalEntries, p => p.GetEvYield().Sum(), AbsoluteBoundary.EvYieldTotal);

            //Initial Friendship
            randomizerSetupConfig.initialFriendship = GetNumericDistributionConfig(gameData.personalEntries, p => p.initialFriendship, AbsoluteBoundary.InitialFriendship);

            //Exp Yield
            randomizerSetupConfig.expYield = GetNumericDistributionConfig(gameData.personalEntries, p => p.giveExp, AbsoluteBoundary.ExpYield);

            //Egg Moves
            instances = new int[gameData.moves.Count];
            entities = gameData.moves.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
                for (int i = 0; i < gameData.personalEntries[personalID].eggMoves.Count; i++)
                    instances[gameData.personalEntries[personalID].eggMoves[i]]++;
            randomizerSetupConfig.eggMoves = ToItemDistributionConfig(instances, entities);

            //Egg Move Type Bias
            movePairing = gameData.personalEntries.SelectMany(p => p.eggMoves.Select(i => (p, gameData.moves[i]))).ToList();
            sameTypePercent = GetOccurrencePercent(movePairing, pm => pm.Item1.GetTyping().Contains(pm.Item2.typingID));
            unbiasedSameTypePercent = (100 + randomizerSetupConfig.doubleTypingP) / gameData.typings.Count;
            randomizerSetupConfig.eggMoveTypeBiasP = 100 * (sameTypePercent - unbiasedSameTypePercent) / (100 - unbiasedSameTypePercent);
            if (randomizerSetupConfig.eggMoveTypeBiasP < 0)
                randomizerSetupConfig.eggMoveTypeBiasP = 0;

            //Egg Move Count
            randomizerSetupConfig.eggMoveCount = GetNumericDistributionConfig(gameData.personalEntries, p => p.eggMoves.Count, AbsoluteBoundary.EggMoveCount);

            //Level Up Moves
            instances = new int[gameData.moves.Count];
            entities = gameData.moves.Select(o => (INamedEntity)o).ToList();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
                for (int i = 0; i < gameData.personalEntries[personalID].levelUpMoves.Count; i++)
                    instances[gameData.personalEntries[personalID].levelUpMoves[i].moveID]++;
            randomizerSetupConfig.levelUpMoves = ToItemDistributionConfig(instances, entities);

            //Level Up Move Type Bias
            movePairing = gameData.personalEntries.SelectMany(p => p.levelUpMoves.Select(l => (p, gameData.moves[l.moveID]))).ToList();
            sameTypePercent = GetOccurrencePercent(movePairing, pm => pm.Item1.GetTyping().Contains(pm.Item2.typingID));
            unbiasedSameTypePercent = (100 + randomizerSetupConfig.doubleTypingP) / gameData.typings.Count;
            randomizerSetupConfig.levelUpMoveTypeBiasP = 100 * (sameTypePercent - unbiasedSameTypePercent) / (100 - unbiasedSameTypePercent);
            if (randomizerSetupConfig.levelUpMoveTypeBiasP < 0)
                randomizerSetupConfig.levelUpMoveTypeBiasP = 0;

            //Level Up Move Levels
            observations = new();
            for (int personalID = 1; personalID < gameData.personalEntries.Count; personalID++)
                for (int i = 0; i < gameData.personalEntries[personalID].levelUpMoves.Count; i++)
                    if (IsWithin(AbsoluteBoundary.Level, gameData.personalEntries[personalID].levelUpMoves[i].level))
                        observations.Add(gameData.personalEntries[personalID].levelUpMoves[i].level);
            randomizerSetupConfig.levelUpMoveLevels = ToNumericDistributionConfig(observations);

            //Evolution Move Count
            (IDistribution[], int) emcc = GetNumericDistributionConfig(gameData.personalEntries.Where(p => p.pastPokemon.Count > 0).ToList(), p => p.levelUpMoves.Where(l => l.level == 0).Count(), AbsoluteBoundary.LevelUpMoveCount);
            randomizerSetupConfig.evolutionMoveCount = emcc.Item1[3];

            //Level Up Move Count
            randomizerSetupConfig.levelUpMoveCount = GetNumericDistributionConfig(gameData.personalEntries, p => p.levelUpMoves.Count, AbsoluteBoundary.LevelUpMoveCount);

            //Move Typing
            randomizerSetupConfig.moveTyping = GetItemDistributionConfig(gameData.moves, m => m.typingID, gameData.typings.Select(o => (INamedEntity)o).ToList());

            //Move Damage Category
            randomizerSetupConfig.damageCategory = GetItemDistributionConfig(gameData.moves.Where(m => m.damageCategoryID != 0).ToList(), m => m.damageCategoryID, gameData.damageCategories.Select(o => (INamedEntity)o).ToList());

            //TM Moves
            randomizerSetupConfig.tmMoves = GetItemDistributionConfig(gameData.tms, t => t.moveID, gameData.moves.Select(o => (INamedEntity)o).ToList());

            //Move Power
            randomizerSetupConfig.movePower = GetNumericDistributionConfig(gameData.moves, m => m.power, AbsoluteBoundary.Power);

            //Move Accuracy
            randomizerSetupConfig.moveAccuracy = GetNumericDistributionConfig(gameData.moves, m => m.hitPer, AbsoluteBoundary.Accuracy);

            //Move PP
            randomizerSetupConfig.movePp = GetNumericDistributionConfig(gameData.moves, m => m.basePP, AbsoluteBoundary.Pp);

            //Item Prices
            randomizerSetupConfig.itemPrices = GetNumericDistributionConfig(gameData.items, i => i.price, AbsoluteBoundary.Price);

            //Pickup Items
            randomizerSetupConfig.pickupItems = GetItemDistributionConfig(gameData.pickupItems, p => p.itemID, gameData.items.Select(o => (INamedEntity)o).ToList());

            //Shop Items
            instances = new int[gameData.items.Count];
            entities = gameData.items.Select(o => (INamedEntity)o).ToList();
            for (int i = 0; i < gameData.shopTables.martItems.Count; i++)
                instances[gameData.shopTables.martItems[i].itemID]++;
            for (int i = 0; i < gameData.shopTables.fixedShopItems.Count; i++)
                instances[gameData.shopTables.fixedShopItems[i].itemID]++;
            randomizerSetupConfig.shopItems = ToItemDistributionConfig(instances, entities);

            //Wild Pokémon
            instances = new int[gameData.dexEntries.Count];
            entities = gameData.dexEntries.Select(o => (INamedEntity)o).ToList();
            for (int version = 0; version < gameData.encounterTableFiles.Length; version++)
            {
                for (int zone = 0; zone < gameData.encounterTableFiles[version].encounterTables.Count; zone++)
                {
                    EncounterTable e = gameData.encounterTableFiles[version].encounterTables[zone];
                    for (int i = 0; i < e.day.Count; i++)
                        if ((ushort)e.day[i].dexID > 0)
                            instances[(ushort)e.day[i].dexID]++;
                    for (int i = 0; i < e.goodRodMons.Count; i++)
                        if ((ushort)e.goodRodMons[i].dexID > 0)
                            instances[(ushort)e.goodRodMons[i].dexID]++;
                    for (int i = 0; i < e.groundMons.Count; i++)
                        if ((ushort)e.groundMons[i].dexID > 0)
                            instances[(ushort)e.groundMons[i].dexID]++;
                    for (int i = 0; i < e.night.Count; i++)
                        if ((ushort)e.night[i].dexID > 0)
                            instances[(ushort)e.night[i].dexID]++;
                    for (int i = 0; i < e.oldRodMons.Count; i++)
                        if ((ushort)e.oldRodMons[i].dexID > 0)
                            instances[(ushort)e.oldRodMons[i].dexID]++;
                    for (int i = 0; i < e.superRodMons.Count; i++)
                        if ((ushort)e.superRodMons[i].dexID > 0)
                            instances[(ushort)e.superRodMons[i].dexID]++;
                    for (int i = 0; i < e.swayGrass.Count; i++)
                        if ((ushort)e.swayGrass[i].dexID > 0)
                            instances[(ushort)e.swayGrass[i].dexID]++;
                    for (int i = 0; i < e.tairyo.Count; i++)
                        if ((ushort)e.tairyo[i].dexID > 0)
                            instances[(ushort)e.tairyo[i].dexID]++;
                    for (int i = 0; i < e.waterMons.Count; i++)
                        if ((ushort)e.waterMons[i].dexID > 0)
                            instances[(ushort)e.waterMons[i].dexID]++;
                }

                for (int i = 0; i < gameData.encounterTableFiles[version].trophyGardenMons.Count; i++)
                    if (gameData.encounterTableFiles[version].trophyGardenMons[i] > 0)
                        instances[gameData.encounterTableFiles[version].trophyGardenMons[i]]++;

                for (int i = 0; i < gameData.encounterTableFiles[version].honeyTreeEnconters.Count; i++)
                {
                    if (gameData.encounterTableFiles[version].honeyTreeEnconters[i].normalDexID > 0)
                        instances[gameData.encounterTableFiles[version].honeyTreeEnconters[i].normalDexID]++;
                    if (gameData.encounterTableFiles[version].honeyTreeEnconters[i].rareDexID > 0)
                        instances[gameData.encounterTableFiles[version].honeyTreeEnconters[i].rareDexID]++;
                    if (gameData.encounterTableFiles[version].honeyTreeEnconters[i].superRareDexID > 0)
                        instances[gameData.encounterTableFiles[version].honeyTreeEnconters[i].superRareDexID]++;
                }

                for (int i = 0; i < gameData.encounterTableFiles[version].safariMons.Count; i++)
                    if (gameData.encounterTableFiles[version].safariMons[i] > 0)
                        instances[gameData.encounterTableFiles[version].safariMons[i]]++;

            }
            for (int file = 0; file < gameData.ugEncounterFiles.Count; file++)
                for (int i = 0; i < gameData.ugEncounterFiles[file].ugEncounters.Count; i++)
                    if (gameData.ugEncounterFiles[file].ugEncounters[i].dexID > 0)
                        instances[gameData.ugEncounterFiles[file].ugEncounters[i].dexID]++;
            randomizerSetupConfig.wildPokemon = ToItemDistributionConfig(instances, entities);

            //Wild Pokémon Levels
            observations = new();
            for (int version = 0; version < gameData.encounterTableFiles.Length; version++)
                for (int zone = 0; zone < gameData.encounterTableFiles[version].encounterTables.Count; zone++)
                {
                    EncounterTable e = gameData.encounterTableFiles[version].encounterTables[zone];
                    for (int i = 0; i < e.day.Count; i++)
                        if ((ushort)e.day[i].dexID > 0)
                        {
                            observations.Add(e.day[i].minLv);
                            observations.Add(e.day[i].maxLv);
                        }
                    for (int i = 0; i < e.goodRodMons.Count; i++)
                        if ((ushort)e.goodRodMons[i].dexID > 0)
                        {
                            observations.Add(e.goodRodMons[i].minLv);
                            observations.Add(e.goodRodMons[i].maxLv);
                        }
                    for (int i = 0; i < e.groundMons.Count; i++)
                        if ((ushort)e.groundMons[i].dexID > 0)
                        {
                            observations.Add(e.groundMons[i].minLv);
                            observations.Add(e.groundMons[i].maxLv);
                        }
                    for (int i = 0; i < e.night.Count; i++)
                        if ((ushort)e.night[i].dexID > 0)
                        {
                            observations.Add(e.night[i].minLv);
                            observations.Add(e.night[i].maxLv);
                        }
                    for (int i = 0; i < e.oldRodMons.Count; i++)
                        if ((ushort)e.oldRodMons[i].dexID > 0)
                        {
                            observations.Add(e.oldRodMons[i].minLv);
                            observations.Add(e.oldRodMons[i].maxLv);
                        }
                    for (int i = 0; i < e.superRodMons.Count; i++)
                        if ((ushort)e.superRodMons[i].dexID > 0)
                        {
                            observations.Add(e.superRodMons[i].minLv);
                            observations.Add(e.superRodMons[i].maxLv);
                        }
                    for (int i = 0; i < e.swayGrass.Count; i++)
                        if ((ushort)e.swayGrass[i].dexID > 0)
                        {
                            observations.Add(e.swayGrass[i].minLv);
                            observations.Add(e.swayGrass[i].maxLv);
                        }
                    for (int i = 0; i < e.tairyo.Count; i++)
                        if ((ushort)e.tairyo[i].dexID > 0)
                        {
                            observations.Add(e.tairyo[i].minLv);
                            observations.Add(e.tairyo[i].maxLv);
                        }
                    for (int i = 0; i < e.waterMons.Count; i++)
                        if ((ushort)e.waterMons[i].dexID > 0)
                        {
                            observations.Add(e.waterMons[i].minLv);
                            observations.Add(e.waterMons[i].maxLv);
                        }
                }
            for (int set = 0; set < gameData.ugEncounterLevelSets.Count; set++)
            {
                observations.Add(gameData.ugEncounterLevelSets[set].minLv);
                observations.Add(gameData.ugEncounterLevelSets[set].maxLv);
            }
            randomizerSetupConfig.wildPokemonLevels = ToNumericDistributionConfig(observations);

            //Trainer Items
            instances = new int[gameData.items.Count];
            entities = gameData.items.Select(o => (INamedEntity)o).ToList();
            for (int i = 0; i < gameData.trainers.Count; i++)
            {
                List<int> items = gameData.trainers[i].GetItems();
                for (int item = 0; item < items.Count; item++)
                    instances[items[item]]++;
            }
            randomizerSetupConfig.trainerItems = ToItemDistributionConfig(instances, entities);

            //Trainer Item Count
            randomizerSetupConfig.trainerItemCount = GetNumericDistributionConfig(gameData.trainers, t => t.GetItems().Count);

            //Trainer Pokémon
            List<TrainerPokemon> tps = gameData.trainers.SelectMany(t => t.trainerPokemon).ToList();
            randomizerSetupConfig.trainerPokemonSpecies = GetItemDistributionConfig(tps, p => gameData.GetPokemon(p.dexID, p.formID).personalID, gameData.personalEntries.Select(o =>(INamedEntity)o).ToList());

            //Trainer Pokémon Moves
            instances = new int[gameData.moves.Count];
            entities = gameData.moves.Select(o => (INamedEntity)o).ToList();
            for (int i = 0; i < tps.Count; i++)
            {
                List<ushort> moves = tps[i].GetMoves();
                for (int move = 0; move < moves.Count; move++)
                    instances[moves[move]]++;
            }
            randomizerSetupConfig.trainerPokemonMoves = ToItemDistributionConfig(instances, entities);

            //Trainer Pokémon Move Type Bias
            movePairing = tps.SelectMany(p => p.GetMoves().Select(l => (gameData.GetPokemon(p.dexID, p.formID), gameData.moves[l]))).ToList();
            sameTypePercent = GetOccurrencePercent(movePairing, pm => pm.Item1.GetTyping().Contains(pm.Item2.typingID));
            unbiasedSameTypePercent = (100 + randomizerSetupConfig.doubleTypingP) / gameData.typings.Count;
            randomizerSetupConfig.trainerPokemonMoveTypeBiasP = 100 * (sameTypePercent - unbiasedSameTypePercent) / (100 - unbiasedSameTypePercent);
            if (randomizerSetupConfig.trainerPokemonMoveTypeBiasP < 0)
                randomizerSetupConfig.trainerPokemonMoveTypeBiasP = 0;

            //Trainer Pokémon Count
            randomizerSetupConfig.trainerPokemonCount = GetNumericDistributionConfig(gameData.trainers, t => t.trainerPokemon.Count, AbsoluteBoundary.TrainerPokemonCount);

            //Trainer Pokémon Levels
            randomizerSetupConfig.trainerPokemonLevels = GetNumericDistributionConfig(tps, p => p.level, AbsoluteBoundary.Level);

            //Trainer Pokémon Held Items
            randomizerSetupConfig.trainerPokemonHeldItems = GetItemDistributionConfig(tps.Where(p => p.itemID > 0).ToList(), p => p.itemID, gameData.items.Select(o => (INamedEntity)o).ToList());

            //Trainer Pokémon Shininess
            randomizerSetupConfig.trainerPokemonShinyP = GetOccurrencePercent(tps, p => p.isRare == 1);

            //Trainer Pokémon Natures
            randomizerSetupConfig.trainerPokemonNatures = GetItemDistributionConfig(tps, p => p.natureID, gameData.natures.Select(o => (INamedEntity)o).ToList());

            //Trainer Pokémon Abilities
            randomizerSetupConfig.trainerPokemonAbilities = GetItemDistributionConfig(tps, p => p.abilityID, gameData.abilities.Select(o => (INamedEntity)o).ToList());

            //Trainer Pokémon IVs
            observations = new();
            for (int i = 0; i < tps.Count; i++)
            {
                int[] ivs = tps[i].GetIVs();
                for (int iv = 0; iv < ivs.Length; iv++)
                    if (IsWithin(AbsoluteBoundary.Iv, ivs[iv]))
                        observations.Add(ivs[iv]);
            }
            randomizerSetupConfig.trainerPokemonIvs = ToNumericDistributionConfig(observations);

            //Trainer Pokémon EVs
            randomizerSetupConfig.trainerPokemonEvs = GetNumericDistributionConfig(tps, p => p.GetEVs().Sum(), AbsoluteBoundary.EvTotal);

            //Scripted Pokémon
            List<Command> commands = gameData.evScripts.SelectMany(e => e.scripts.SelectMany(s => s.commands)).ToList();
            randomizerSetupConfig.scriptedPokemon = GetItemDistributionConfig(commands.Where(c => c.cmdType == 322 && c.args[1].argType == 1).ToList(), c => (int)c.args[1].data, gameData.dexEntries.Select(o => (INamedEntity)o).ToList());

            //Scripted Items
            randomizerSetupConfig.scriptedItems = GetItemDistributionConfig(commands.Where(c => c.cmdType == 187 && c.args[0].argType == 1 && gameData.items[(int)c.args[0].data].IsPurchasable()).ToList(), c => (int)c.args[0].data, gameData.items.Select(o => (INamedEntity)o).ToList());

            //Type Matchups
            instances = new int[4];
            int typeCount = 18;
            for (int o = 0; o < typeCount; o++)
                for (int d = 0; d < typeCount; d++)
                    switch(gameData.globalMetadata.GetTypeMatchup(o, d))
                    {
                        case 0:
                            instances[0]++;
                            break;
                        case 2:
                            instances[1]++;
                            break;
                        case 4:
                            instances[2]++;
                            break;
                        case 8:
                            instances[3]++;
                            break;
                    }
            List<string> affinities = new List<string>();
            affinities.Add("0x");
            affinities.Add("1/2x");
            affinities.Add("1x");
            affinities.Add("2x");
            IDistribution[] affinityDistributions = new IDistribution[]
            {
                new Empirical(100, instances.ToList()),
                new UniformSelection(100, instances.Select(i => true).ToList())
            };
            randomizerSetupConfig.typeMatchups = (affinityDistributions, affinities, 0);

            //Level Coefficient
            randomizerSetupConfig.levelCoefficient = 1;

            return randomizerSetupConfig;
        }

        public enum TypingCorrelation
        {
            Identical,
            Addition,
            Swap,
            NoCorrelation
        }

        /// <summary>
        ///  Finds the particular correlation of typings between two Pokémon.
        /// </summary>
        public static TypingCorrelation CompareTyping(this Pokemon p1, Pokemon p2)
        {
            List<int> typing1 = p1.GetTyping();
            List<int> typing2 = p2.GetTyping();
            int matches = 0;
            if (typing1.Contains(typing2[0]))
                matches++;
            if (typing2.Count > 1 && typing1.Contains(typing2[1]))
                matches++;
            if (matches == 0 && !(typing1.Count == 1 && typing2.Count == 1))
                return TypingCorrelation.NoCorrelation;
            if (matches == 2 || typing1.Count == 1 && typing2.Count == 1 && matches == 1)
                return TypingCorrelation.Identical;
            if (typing1.Count != typing2.Count)
                return TypingCorrelation.Addition;
            return TypingCorrelation.Swap;
        }

        /// <summary>
        ///  Calculates the frequency at which a certian predicate is true for items in a list.
        /// </summary>
        private static double GetOccurrencePercent<T>(List<T> objects, Func<T, bool> f)
        {
            int occurrences = objects.Where(f).Count();
            return 100.0 * occurrences / objects.Count;
        }

        /// <summary>
        ///  Generates numeric distribution objects.
        /// </summary>
        private static (IDistribution[], int) GetNumericDistributionConfig<T>(List<T> objects, Func<T, int> f, AbsoluteBoundary ab = AbsoluteBoundary.None)
        {
            List<int> observations = new();
            for (int item = 0; item < objects.Count; item++)
                if (IsWithin(ab, f.Invoke(objects[item])))
                    observations.Add(f.Invoke(objects[item]));
            return ToNumericDistributionConfig(observations);
        }

        /// <summary>
        ///  Generates numeric distribution objects from a list of observations.
        /// </summary>
        private static (IDistribution[], int) ToNumericDistributionConfig(List<int> observations)
        {
            double min = observations.Count > 0 ? observations.Min() : 0;
            double max = observations.Count > 0 ? observations.Max() : 0;
            double uniAvg = (min + max) / 2.0;
            double avg = observations.Count > 0 ? observations.Average() : 0;
            double std = observations.StandardDeviation();

            IDistribution[] distributions = new IDistribution[]
            {
                new UniformConstant(100, min, max),
                new UniformRelative(100, (min-uniAvg)/2, (max-uniAvg)/2),
                new UniformProportional(100, (min+uniAvg)/(2*uniAvg == 0 ? 1 : 2*uniAvg), (max+uniAvg)/(2*uniAvg == 0 ? 1 : 2*uniAvg)),
                new NormalConstant(100, avg, std),
                new NormalRelative(100, std/2),
                new NormalProportional(100, std/(2*avg == 0 ? 1 : 2*avg))
            };
            return (distributions, 4);
        }

        /// <summary>
        ///  Generates item distribution objects.
        /// </summary>
        private static (IDistribution[], List<string>, int) GetItemDistributionConfig<T>(List<T> objects, Func<T, int> f, List<INamedEntity> entities)
        {
            int[] instances = new int[entities.Count];
            for (int item = 0; item < objects.Count; item++)
                instances[f.Invoke(objects[item])]++;
            return ToItemDistributionConfig(instances, entities);
        }

        /// <summary>
        ///  Generates item distribution objects from an array of instances.
        /// </summary>
        private static (IDistribution[], List<string>, int) ToItemDistributionConfig(int[] instances, List<INamedEntity> entities)
        {
            IDistribution[] distributions = new IDistribution[]
            {
                new Empirical(100, instances.ToList()),
                new UniformSelection(100, entities.Select(e => e.IsValid()).ToList())
            };
            List<string> names = entities.Select(t => t.GetName()).ToList();
            return (distributions, names, 0);
        }
    }
}
