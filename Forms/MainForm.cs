using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.Distributions;

namespace ImpostersOrdeal
{
    /// <summary>
    ///  Root form from which the rest of the application is accessible.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public class RandomizerSetupConfig
        {
            public (IDistribution[], List<string>, int) evolutionDestinationPokemon;
            public (IDistribution[], int) evolutionLevel;
            public (IDistribution[], int) baseStats;
            public (IDistribution[], List<string>, int) pokemonTyping;
            public double doubleTypingP;
            public double tmCompatibilityP;
            public double tmCompatibilityTypeBiasP;
            public (IDistribution[], List<string>, int) wildHeldItems;
            public (IDistribution[], List<string>, int) growthRate;
            public (IDistribution[], List<string>, int) abilities;
            public (IDistribution[], int) catchRate;
            public (IDistribution[], int) evYields;
            public (IDistribution[], int) initialFriendship;
            public (IDistribution[], int) expYield;
            public (IDistribution[], List<string>, int) eggMoves;
            public double eggMoveTypeBiasP;
            public (IDistribution[], int) eggMoveCount;
            public (IDistribution[], List<string>, int) levelUpMoves;
            public double levelUpMoveTypeBiasP;
            public (IDistribution[], int) levelUpMoveLevels;
            public (IDistribution[], int) levelUpMoveCount;

            public (IDistribution[], List<string>, int) moveTyping;
            public (IDistribution[], List<string>, int) damageCategory;
            public (IDistribution[], List<string>, int) tmMoves;
            public (IDistribution[], int) movePower;
            public (IDistribution[], int) moveAccuracy;
            public (IDistribution[], int) movePp;
            public (IDistribution[], int) itemPrices;
            public (IDistribution[], List<string>, int) pickupItems;
            public (IDistribution[], List<string>, int) shopItems;

            public (IDistribution[], List<string>, int) wildPokemon;
            public (IDistribution[], int) wildPokemonLevels;
            public (IDistribution[], List<string>, int) trainerItems;
            public (IDistribution[], int) trainerItemCount;
            public (IDistribution[], List<string>, int) trainerPokemonSpecies;
            public (IDistribution[], List<string>, int) trainerPokemonMoves;
            public double trainerPokemonMoveTypeBiasP;
            public (IDistribution[], int) trainerPokemonCount;
            public (IDistribution[], int) trainerPokemonLevels;
            public (IDistribution[], List<string>, int) trainerPokemonHeldItems;
            public double trainerPokemonShinyP;
            public (IDistribution[], List<string>, int) trainerPokemonNatures;
            public (IDistribution[], List<string>, int) trainerPokemonAbilities;
            public (IDistribution[], int) trainerPokemonIvs;
            public (IDistribution[], int) trainerPokemonEvs;

            public (IDistribution[], List<string>, int) scriptedPokemon;
            public (IDistribution[], List<string>, int) scriptedItems;
            public double levelCoefficient;

            public IDistribution evolutionLogicTypingCorrelationDistribution;
            public IDistribution evolutionMoveCount;
        }

        public class NumericDistributionControl : GroupBox
        {
            public IDistribution[] distributions = new IDistribution[]
            { 
                new UniformConstant(100, 0, 100),
                new UniformRelative(100, -25, 25),
                new UniformProportional(100, 0.5, 1.5),
                new NormalConstant(100, 50, 25),
                new NormalRelative(100, 25),
                new NormalProportional(100, 0.25)
            }; // Just some example data for testing purposes, don't worry about it ;)
            public int idx;

            public IDistribution Get()
            {
                return distributions[idx];
            }

            public void SetCurrent(IDistribution d)
            {
                distributions[idx] = d;
            }

            public void Initialize((IDistribution[], int) config)
            {
                this.distributions = config.Item1;
                this.idx = config.Item2;
                UpdateTextBox();
            }

            public void UpdateTextBox()
            {
                List<Control> l = new();
                for (int i = 0; i < Controls.Count; i++)
                    l.Add(Controls[i]);
                l.Find(c => c.Name.Contains("textBox")).Text = Get().GetString();
            }
        }

        public class ItemDistributionControl : Button
        {
            public IDistribution[] distributions = new IDistribution[]
            {
                new Empirical(100, (new int[] { 1, 0, 2, 0, 3, 0, 4 }).ToList()),
                new UniformSelection(100, (new bool[] { true, false, true, false, true, false, true }).ToList())
            };
            public List<string> itemNames = new(new string[] { "Item0", "Item1", "Item2", "Item3", "Item4", "Item5", "Item6" });
            public int idx;

            public IDistribution Get()
            {
                return distributions[idx];
            }

            public void SetCurrent(IDistribution d)
            {
                distributions[idx] = d;
            }

            public void Initialize((IDistribution[], List<string>, int) config)
            {
                this.distributions = config.Item1;
                this.itemNames = config.Item2;
                this.idx = config.Item3;
            }
        }

        public RandomizerSetupConfig rsc;
        private Flavor flavor;
        private Thread loadingDisplay;
        private LoadingForm loadingForm;
        private Randomizer randomizer;
        bool randomizeClicked = false;

        /// <summary>
        ///  Confirms with user to cancel loading dump.
        /// </summary>
        private static DialogResult RetryLoadDumpDialog()
        {
            return MessageBox.Show("Game dump is required to continue. Load dump?",
                "Load Cancelled", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        ///  Initializes controls using the specified config.
        /// </summary>
        private void SetupConfig(RandomizerSetupConfig rsc)
        {
            button2.Initialize(rsc.evolutionDestinationPokemon);
            groupBox1.Initialize(rsc.evolutionLevel);
            numericDistributionControl1.Initialize(rsc.baseStats);
            itemDistributionControl1.Initialize(rsc.pokemonTyping);
            numericUpDown1.Value = (decimal)rsc.doubleTypingP;
            numericUpDown2.Value = (decimal)rsc.tmCompatibilityP;
            numericUpDown3.Value = (decimal)rsc.tmCompatibilityTypeBiasP;
            itemDistributionControl2.Initialize(rsc.wildHeldItems);
            itemDistributionControl3.Initialize(rsc.growthRate);
            itemDistributionControl4.Initialize(rsc.abilities);
            numericDistributionControl2.Initialize(rsc.catchRate);
            numericDistributionControl3.Initialize(rsc.evYields);
            numericDistributionControl4.Initialize(rsc.initialFriendship);
            numericDistributionControl5.Initialize(rsc.expYield);
            itemDistributionControl7.Initialize(rsc.eggMoves);
            numericUpDown5.Value = (decimal)rsc.eggMoveTypeBiasP;
            numericDistributionControl9.Initialize(rsc.eggMoveCount);
            itemDistributionControl6.Initialize(rsc.levelUpMoves);
            numericUpDown4.Value = (decimal)rsc.levelUpMoveTypeBiasP;
            numericDistributionControl7.Initialize(rsc.levelUpMoveLevels);
            numericDistributionControl6.Initialize(rsc.levelUpMoveCount);
            itemDistributionControl8.Initialize(rsc.moveTyping);
            itemDistributionControl9.Initialize(rsc.damageCategory);
            itemDistributionControl18.Initialize(rsc.tmMoves);
            numericDistributionControl8.Initialize(rsc.movePower);
            numericDistributionControl10.Initialize(rsc.moveAccuracy);
            numericDistributionControl11.Initialize(rsc.movePp);
            numericDistributionControl18.Initialize(rsc.itemPrices);
            itemDistributionControl16.Initialize(rsc.pickupItems);
            itemDistributionControl19.Initialize(rsc.shopItems);
            itemDistributionControl10.Initialize(rsc.wildPokemon);
            numericDistributionControl12.Initialize(rsc.wildPokemonLevels);
            itemDistributionControl11.Initialize(rsc.trainerItems);
            numericDistributionControl13.Initialize(rsc.trainerItemCount);
            itemDistributionControl12.Initialize(rsc.trainerPokemonSpecies);
            itemDistributionControl15.Initialize(rsc.trainerPokemonHeldItems);
            itemDistributionControl13.Initialize(rsc.trainerPokemonNatures);
            numericUpDown7.Value = (decimal)rsc.trainerPokemonMoveTypeBiasP;
            itemDistributionControl14.Initialize(rsc.trainerPokemonMoves);
            numericUpDown6.Value = (decimal)rsc.trainerPokemonShinyP;
            itemDistributionControl17.Initialize(rsc.trainerPokemonAbilities);
            numericDistributionControl14.Initialize(rsc.trainerPokemonCount);
            numericDistributionControl15.Initialize(rsc.trainerPokemonLevels);
            numericDistributionControl16.Initialize(rsc.trainerPokemonIvs);
            numericDistributionControl17.Initialize(rsc.trainerPokemonEvs);
            itemDistributionControl20.Initialize(rsc.scriptedPokemon);
            itemDistributionControl21.Initialize(rsc.scriptedItems);
            numericUpDown8.Value = (decimal)rsc.levelCoefficient;
            this.rsc = rsc;

            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        ///  Starts up the LoadingForm.
        /// </summary>
        private void StartLoadingDisplay()
        {
            loadingForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            flavor = new();
            randomizer = new Randomizer(this);
            GlobalData.fileManager = new();
            GlobalData.Initialize();
            GlobalData.gameData = new();

            //Confirm with user to get dump path. Abort if cancel.
            if (MessageBox.Show("Alright, to start out, could ya get me a dump of the game real quick?\n" +
                "Gimme the folder that's got the \"romfs\" in it.",
                "Load Dump", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel && RetryLoadDumpDialog() == DialogResult.No)
            {
                this.Close();
                return;
            }

            //Load dump
            while (!GlobalData.fileManager.InitializeFromDump())
                if (RetryLoadDumpDialog() == DialogResult.No)
                {
                    this.Close();
                    return;
                }

            loadingForm = new("Ferociously investigating your dump...", flavor.GetSubTask());
            loadingDisplay = new(StartLoadingDisplay);
            loadingDisplay.Start();

            //Sometimes UpdateSubTask would be called before the loadingForm was done setting up, causing me great grief.
            //This is probably not a very good solution to that, but it works! ¯\_(ツ)_/¯
            Thread.Sleep(10);

            DataParser.PrepareAnalysis();

            loadingForm.UpdateSubTask(flavor.GetSubTask());
            SetupConfig(Analyzer.GetSetupConfig());
            loadingForm.Finish();

            absoluteBoundaryDataGridView.DataSource = GlobalData.absoluteBoundaries;
        }

        private void OpenNumericDistributionForm(object sender, EventArgs e)
        {
            NumericDistributionControl ndc = (NumericDistributionControl)((Button)sender).Parent;
            NumericDistributionForm form = new(ndc);
            form.ShowDialog();
            ndc.UpdateTextBox();
        }

        private void NumericDistributionTextBoxChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            NumericDistributionControl ndc = (NumericDistributionControl)tb.Parent;
            IDistribution newDistribution = Parse(tb.Text);
            if (newDistribution == null)
            {
                tb.Text = ndc.Get().GetString();
                return;
            }
            ndc.idx = (int)newDistribution.GetConfig()[0];
            ndc.SetCurrent(newDistribution);
        }

        private void OpenItemDistributionForm(object sender, EventArgs e)
        {
            ItemDistributionControl idc = (ItemDistributionControl)sender;
            ItemDistributionForm form = new(idc);
            form.ShowDialog();
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ShowDataError();
        }

        public static void ShowDataError()
        {
            MessageBox.Show("Yeah, no. That's not gonna fly buster.\nInput some actual valid data please.",
                "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddMod(object sender, EventArgs e)
        {
            if (MessageBox.Show("Fancy! Let's see if we can merge in a mod, shall we?\n" +
                   "Gimme a folder that's got a \"romfs\" or \"exefs\" in it.",
                   "Add Mod", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                return;

            if (GlobalData.fileManager.AddMod())
            {
                loadingForm = new("Some stuff changed...", flavor.GetSubTask());
                loadingDisplay = new(StartLoadingDisplay);
                loadingDisplay.Start();
                Thread.Sleep(10);
                DataParser.PrepareAnalysis();

                loadingForm.UpdateSubTask(flavor.GetSubTask());
                SetupConfig(Analyzer.GetSetupConfig());
                loadingForm.Finish();
            }
        }

        private void Randomize(object sender, EventArgs e)
        {
            //Notify if already randomized.
            if (randomizeClicked && MessageBox.Show("You uh... You already made me randomize the files, and I\n" +
                "wouldn't really recommend doing it multiple times...\n" +
                "Randomize again anyway?",
                   "Again?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            loadingForm = new("Makin' a mess...", flavor.GetThought());
            loadingDisplay = new(StartLoadingDisplay);
            loadingDisplay.Start();
            Thread.Sleep(10);
            randomizer.Randomize();
            loadingForm.Finish();

            randomizeClicked = true;
            MessageBox.Show(
                "Randomization complete!",
                  "All done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Export(object sender, EventArgs e)
        {
            //Notify if not randomized.
            if (!randomizeClicked && MessageBox.Show("I haven't randomized anything yet...\n" +
                "Just thought I'd mention it in case you forgot.\n" +
                "Proceed anyway? I'll still export any changed files for you.",
                   "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            loadingForm = new("Finishing up...", flavor.GetSubTask());
            loadingDisplay = new(StartLoadingDisplay);
            loadingDisplay.Start();
            Thread.Sleep(10);
            DataParser.CommitChanges();

            loadingForm.UpdateSubTask(flavor.GetThought());
            GlobalData.fileManager.ExportMod();
            loadingForm.Finish();

            MessageBox.Show(
                "And that should be it! Your very own mod has\n" +
                "been created! I'll see myself out now.\n" +
                "Oh, and if you wonder where it is, I placed it right\n" +
                "alongside my executable, \"" + FileManager.outputModName + "\".",
                  "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void OpenPokemonEditor(object sender, EventArgs e)
        {
            PokemonEditorForm pef = new();
            pef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.PersonalEntries);
        }

        private void OpenMoveEditor(object sender, EventArgs e)
        {
            MoveEditorForm mef = new();
            mef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.Moves);
        }

        private void OpenTMEditor(object sender, EventArgs e)
        {
            TMEditorForm tmef = new();
            tmef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.TMs);
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.Items);
        }

        private void OpenItemEditor(object sender, EventArgs e)
        {
            ItemEditorForm ief = new();
            ief.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.Items);
        }

        private void OpenPickupEditor(object sender, EventArgs e)
        {
            PickupEditorForm pef = new();
            pef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.PickupItems);
        }

        private void OpenShopEditor(object sender, EventArgs e)
        {
            ShopEditorForm sef = new();
            sef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.ShopTables);
        }

        private void OpenWildEncEditor(object sender, EventArgs e)
        {
            WildEncEditorForm weef = new();
            weef.ShowDialog();
            GlobalData.gameData.SetModified(GlobalData.GameDataSet.DataField.EncounterTableFiles);
        }
    }
}
