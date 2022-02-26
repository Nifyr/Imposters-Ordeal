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
    public partial class EvolutionEditorForm : Form
    {
        private readonly PokemonEditorForm pef;
        private readonly Pokemon p;
        private EvolutionPath m;

        private readonly string[] evolutionMethods = new string[]
        {
            "", "On LvUp: high friendship", "On LvUp: high friendship & is day", "On LvUp: high friendship & is night",
            "On LvUp: Lv ≥ LvReq", "On Trade", "On Trade: holds item", "Karrablast/Shelmet Trade",
            "On UseItem", "On LvUp: Lv ≥ LvReq & Atk > Def", "On LvUp: Lv ≥ LvReq & Def > Atk", "On LvUp: Lv ≥ LvReq & Atk = Def",
            "On LvUp: Lv ≥ LvReq & rng(0-9) ≤ 4", "On LvUp: Lv ≥ LvReq & rng(0-9) > 4", "On LvUp: Lv ≥ LvReq → Get Shedinja", "SPECIAL_NUKENIN",
            "On LvUp: high beauty", "On UseItem: is male", "On UseItem: is female", "On LvUp: Lv ≥ LvReq & holds item & is day",
            "On LvUp: Lv ≥ LvReq & holds item & is night", "On LvUp: has move", "On LvUp: Pokémon in party", "On LvUp: Lv ≥ LvReq & is male",
            "On LvUp: Lv ≥ LvReq & is female", "On LvUp: is by magnetic field", "On LvUp: is by moss rock", "On LvUp: is by ice rock",
            "On LvUp: Lv ≥ LvReq & device upside down", "On LvUp: high friendship & has move of type", "On LvUp: Lv ≥ LvReq & Dark Pokémon in party", "On LvUp: Lv ≥ LvReq & is raining",
            "On LvUp: Lv ≥ LvReq & is day", "On LvUp: Lv ≥ LvReq & is night", "On LvUp: Lv ≥ LvReq & is female → set form to 1", "FRIENDLY",
            "On LvUp: Lv ≥ LvReq & is game version", "On LvUp: Lv ≥ LvReq & is game version & is day", "On LvUp: Lv ≥ LvReq & is game version & is night", "On LvUp: is by summit",
            "On LvUp: Lv ≥ LvReq & is dusk", "On LvUp: Lv ≥ LvReq & is outside region", "On UseItem: is outside region", "Galarian Farfetch'd Evolution",
            "Galarian Yamask Evolution", "Milcery Evolution", "On LvUp: Lv ≥ LvReq & has amped nature", "On LvUp: Lv ≥ LvReq & has low-key nature"
        };

        private readonly bool[] lvReqMethods = new bool[]
        {
            false, false, false, false, true, false, false, false, false, true, true, true, true, true, true, true,
            false, false, false, true, true, false, false, true, true, false, false, false, true, false, true, true,
            true, true, true, false, true, true, true, false, true, true, false, false, false, false, true, true
        };

        private readonly EvolutionParamType[] paramTypes = new EvolutionParamType[]
        {
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.Item, EvolutionParamType.None,
            EvolutionParamType.Item, EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.Item, EvolutionParamType.Item, EvolutionParamType.Item,
            EvolutionParamType.Item, EvolutionParamType.Move, EvolutionParamType.Pokemon, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.Typing, EvolutionParamType.None, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None,
            EvolutionParamType.GameVersion, EvolutionParamType.GameVersion, EvolutionParamType.GameVersion, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.Item, EvolutionParamType.None,
            EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None, EvolutionParamType.None,
        };

        private enum EvolutionParamType
        {
            None, Item, Move, Pokemon, Typing, GameVersion
        }

        public EvolutionEditorForm(PokemonEditorForm pef)
        {
            this.pef = pef;
            p = pef.p;
            InitializeComponent();
            Text = "Evolution Editor: " + p.GetName();

            destinationDexIDColumn.DataSource = gameData.dexEntries.Select(d => d.GetName()).ToArray();
            methodColumn.DataSource = evolutionMethods;

            p.evolutionPaths.ForEach(e => dataGridView.Rows.Add(new object[] { gameData.GetPokemon(e.destDexID, e.destFormID).GetName(), evolutionMethods[e.method] }));

            RefreshEvolutionPathDisplay();

            ActivateControls();
        }

        private void FocusEvoPathChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            if (dataGridView.CurrentCell == null || dataGridView.CurrentCell.RowIndex >= p.evolutionPaths.Count)
                m = null;
            else
                m = p.evolutionPaths[dataGridView.CurrentCell.RowIndex];

            RefreshEvolutionPathDisplay();
            ActivateControls();
        }

        private void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DeactivateControls();
            if (m == null)
            {
                m = new();
                p.evolutionPaths.Add(m);
            }
            if (e.ColumnIndex == 0)
            {
                m.destDexID = (ushort)pef.pokemon.IndexOf((string)dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                m.destFormID = 0;
            }
            if (e.ColumnIndex == 1)
            {
                int evoMethod = evolutionMethods.ToList().IndexOf((string)dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                m.method = evoMethod == -1 ? (ushort)0 : (ushort)evoMethod;
                m.parameter = 0;
            }

            RefreshEvolutionPathDisplay();
            ActivateControls();
        }

        private void UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            p.evolutionPaths.RemoveAt(e.Row.Index);
        }

        private void RefreshEvolutionPathDisplay()
        {
            if (m == null)
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                formIDComboBox.Visible = false;
                lvReqNumericUpDown.Visible = false;
                evoParamComboBox.Visible = false;
                return;
            }


            label1.Visible = true;
            formIDComboBox.Visible = true;
            formIDComboBox.DataSource = gameData.dexEntries[m.destDexID].forms.Select((p, i) => i.ToString()).ToArray();
            formIDComboBox.SelectedIndex = m.destFormID;

            if (lvReqMethods[m.method])
            {
                label2.Visible = true;
                lvReqNumericUpDown.Visible = true;
                lvReqNumericUpDown.Value = m.level;
            }
            else
            {
                label2.Visible = false;
                lvReqNumericUpDown.Visible = false;
            }

            switch (paramTypes[m.method])
            {
                case EvolutionParamType.None:
                    label3.Visible = false;
                    evoParamComboBox.Visible = false;
                    break;

                case EvolutionParamType.Item:
                    label3.Visible = true;
                    label3.Text = "Item";
                    evoParamComboBox.Visible = true;
                    evoParamComboBox.DataSource = pef.items.ToArray();
                    evoParamComboBox.SelectedIndex = m.parameter;
                    break;

                case EvolutionParamType.Move:
                    label3.Visible = true;
                    label3.Text = "Move";
                    evoParamComboBox.Visible = true;
                    evoParamComboBox.DataSource = pef.moves.ToArray();
                    evoParamComboBox.SelectedIndex = m.parameter;
                    break;

                case EvolutionParamType.Pokemon:
                    label3.Visible = true;
                    label3.Text = "Pokémon";
                    evoParamComboBox.Visible = true;
                    evoParamComboBox.DataSource = pef.pokemon.ToArray();
                    evoParamComboBox.SelectedIndex = m.parameter;
                    break;

                case EvolutionParamType.Typing:
                    label3.Visible = true;
                    label3.Text = "Type";
                    evoParamComboBox.Visible = true;
                    evoParamComboBox.DataSource = pef.typings.ToArray();
                    evoParamComboBox.SelectedIndex = m.parameter;
                    break;

                case EvolutionParamType.GameVersion:
                    label3.Visible = true;
                    label3.Text = "Game Version (unsupported)";
                    evoParamComboBox.Visible = true;
                    evoParamComboBox.DataSource = new string[] { "0" };
                    evoParamComboBox.SelectedIndex = 0;
                    break;
            }
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            if (formIDComboBox.Visible)
                m.destFormID = (ushort)formIDComboBox.SelectedIndex;
            if (lvReqNumericUpDown.Visible)
                m.level = (ushort)lvReqNumericUpDown.Value;
            if (evoParamComboBox.Visible)
                m.parameter = (ushort)evoParamComboBox.SelectedIndex;
        }

        private void ActivateControls()
        {
            dataGridView.CellEnter += FocusEvoPathChanged;
            dataGridView.CellEndEdit += CellEndEdit;
            dataGridView.UserDeletingRow += UserDeletingRow;
            formIDComboBox.SelectedIndexChanged += CommitEdit;
            lvReqNumericUpDown.ValueChanged += CommitEdit;
            evoParamComboBox.SelectedIndexChanged += CommitEdit;
        }

        private void DeactivateControls()
        {
            dataGridView.CellEnter -= FocusEvoPathChanged;
            dataGridView.CellEndEdit -= CellEndEdit;
            dataGridView.UserDeletingRow -= UserDeletingRow;
            formIDComboBox.SelectedIndexChanged -= CommitEdit;
            lvReqNumericUpDown.ValueChanged -= CommitEdit;
            evoParamComboBox.SelectedIndexChanged -= CommitEdit;
        }
    }
}
