using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BDSP_Randomizer.GameDataTypes;
using static BDSP_Randomizer.GlobalData;

namespace BDSP_Randomizer
{
    public partial class MoveEditorForm : Form
    {
        private List<Move> moves;
        private Dictionary<int, string> statusEffects;
        private List<string> moveSequences;
        private List<string> typings;
        private List<string> damageCategoies;
        private Move m;

        private string[] moveCategories = new string[]
        {
            "Deal Damage",
            "Status Affliction",
            "Stat Change",
            "Heal",
            "Damage + Status",
            "Status + Stat±",
            "Damage + Stat± (target)",
            "Damage + Stat± (self)",
            "Damage + Heal",
            "One Hit KO",
            "Field Effect",
            "Field Effect (target)",
            "Switch Out (target)",
            "Special"
        };

        private string[] targeting = new string[]
        {
            "Ally or Foe",
            "Self or Ally",
            "Ally Only",
            "Foe Only",
            "Allies and Foes",
            "All Foes",
            "Self and Allies",
            "Self Only",
            "Everyone",
            "Self (random)",
            "Everyone (field)",
            "All Foes (field)",
            "Self and Allies (field)",
            "Self (autotarget)"
        };

        private string[] stats = new string[]
        {
            "None",
            "Attack",
            "Defense",
            "Special Attack",
            "Special Defense",
            "Speed",
            "Accuracy",
            "Evasiveness",
            "All Stats"
        };

        public MoveEditorForm()
        {
            statusEffects = new();
            statusEffects[0] = "None";
            statusEffects[1] = "Paralysis";
            statusEffects[2] = "Sleep";
            statusEffects[3] = "Freeze";
            statusEffects[4] = "Burn";
            statusEffects[5] = "Poison";
            statusEffects[6] = "Confusion";
            statusEffects[7] = "Infatuation";
            statusEffects[8] = "Bound";
            statusEffects[9] = "Nightmare";
            statusEffects[12] = "Torment";
            statusEffects[13] = "Disable";
            statusEffects[14] = "Yawn";
            statusEffects[15] = "Heal Block";
            statusEffects[17] = "Identified";
            statusEffects[18] = "Leech Seed";
            statusEffects[19] = "Embargo";
            statusEffects[20] = "Perish Song";
            statusEffects[21] = "Rooted";
            statusEffects[24] = "Throat Chop";
            statusEffects[42] = "Tar Shot";
            statusEffects[65535] = "Special";

            InitializeComponent();

            moves = new();
            moves.AddRange(gameData.moves);
            moves.Sort((m1, m2) => m1.GetName().CompareTo(m2.GetName()));
            typings = gameData.typings.Select(t => t.GetName()).ToList();
            damageCategoies = gameData.damageCategories.Select(d => d.GetName()).ToList();

            //moves.Sort((m1, m2) => m1.rankEffType1 - m2.rankEffType1);

            listBox.DataSource = moves.Select(m => m.GetName()).ToArray();
            listBox.SelectedIndex = 0;
            m = moves[0];

            typingComboBox.DataSource = typings.ToArray();
            damageCategoryComboBox.DataSource = damageCategoies.ToArray();

            statusEffectComboBox.DataSource = statusEffects.Values.ToArray();

            statTypeComboBox1.DataSource = stats.ToList();
            statTypeComboBox2.DataSource = stats.ToList();
            statTypeComboBox3.DataSource = stats.ToList();

            moveCategoryComboBox.DataSource = moveCategories;
            targetingComboBox.DataSource = targeting;

            RefreshMoveDisplay();
            ActivateControls();
        }

        private void MoveChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            m = moves[listBox.SelectedIndex];
            RefreshMoveDisplay();

            ActivateControls();
        }

        private void RefreshMoveDisplay()
        {
            moveDisplayTextBox.Text = FormatNumber(m.GetID(), 3) + " - " + m.GetName();
            isValidCheckBox.Checked = m.isValid == 1;
            typingComboBox.SelectedIndex = m.typingID;
            damageCategoryComboBox.SelectedIndex = m.damageCategoryID;
            numericUpDown1.Value = m.power;
            numericUpDown2.Value = m.hitPer;
            numericUpDown3.Value = m.basePP;

            statusEffectComboBox.SelectedItem = statusEffects[m.sickID];
            numericUpDown12.Value = m.sickPer;
            numericUpDown13.Value = m.sickCont;
            numericUpDown14.Value = m.sickTurnMin;
            numericUpDown15.Value = m.sickTurnMax;

            statTypeComboBox1.SelectedIndex = m.rankEffType1;
            statTypeComboBox2.SelectedIndex = m.rankEffType2;
            statTypeComboBox3.SelectedIndex = m.rankEffType3;
            numericUpDown16.Value = m.rankEffValue1;
            numericUpDown18.Value = m.rankEffValue2;
            numericUpDown20.Value = m.rankEffValue3;
            numericUpDown17.Value = m.rankEffPer1;
            numericUpDown19.Value = m.rankEffPer2;
            numericUpDown21.Value = m.rankEffPer3;

            moveCategoryComboBox.SelectedIndex = m.category;
            numericUpDown4.Value = m.priority;
            numericUpDown5.Value = m.hitCountMin;
            numericUpDown6.Value = m.hitCountMax;
            numericUpDown7.Value = m.criticalRank;
            numericUpDown8.Value = m.shrinkPer;
            numericUpDown9.Value = m.aiSeqNo;
            numericUpDown10.Value = m.damageRecoverRatio;
            numericUpDown11.Value = m.hpRecoverRatio;
            targetingComboBox.SelectedIndex = m.target;

            bool[] flags = m.GetFlags();
            checkBox1.Checked = flags[0];
            checkBox2.Checked = flags[1];
            checkBox3.Checked = flags[2];
            checkBox4.Checked = flags[3];
            checkBox5.Checked = flags[4];
            checkBox6.Checked = flags[5];
            checkBox7.Checked = flags[6];
            checkBox8.Checked = flags[7];
            checkBox9.Checked = flags[8];
            checkBox10.Checked = flags[9];
            checkBox11.Checked = flags[10];
            checkBox12.Checked = flags[11];
            checkBox13.Checked = flags[12];
            checkBox14.Checked = flags[13];
            checkBox15.Checked = flags[14];
            checkBox16.Checked = flags[15];
            checkBox17.Checked = flags[16];
            checkBox18.Checked = flags[17];
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            m.isValid = isValidCheckBox.Checked ? (byte)1 : (byte)0;
            m.typingID = (byte)typingComboBox.SelectedIndex;
            m.damageCategoryID = (byte)damageCategoryComboBox.SelectedIndex;
            m.power = (byte)numericUpDown1.Value;
            m.hitPer = (byte)numericUpDown2.Value;
            m.basePP = (byte)numericUpDown3.Value;

            m.sickID = (ushort)statusEffects.Keys.ToArray()[statusEffectComboBox.SelectedIndex];
            m.sickPer = (byte)numericUpDown12.Value;
            m.sickCont = (byte)numericUpDown13.Value;
            m.sickTurnMin = (byte)numericUpDown14.Value;
            m.sickTurnMax = (byte)numericUpDown15.Value;

            m.rankEffType1 = (byte)statTypeComboBox1.SelectedIndex;
            m.rankEffType2 = (byte)statTypeComboBox2.SelectedIndex;
            m.rankEffType3 = (byte)statTypeComboBox3.SelectedIndex;
            m.rankEffValue1 = (sbyte)numericUpDown16.Value;
            m.rankEffValue2 = (sbyte)numericUpDown18.Value;
            m.rankEffValue3 = (sbyte)numericUpDown20.Value;
            m.rankEffPer1 = (byte)numericUpDown17.Value;
            m.rankEffPer2 = (byte)numericUpDown19.Value;
            m.rankEffPer3 = (byte)numericUpDown21.Value;

            m.category = (byte)moveCategoryComboBox.SelectedIndex;
            m.priority = (sbyte)numericUpDown4.Value;
            m.hitCountMin = (byte)numericUpDown5.Value;
            m.hitCountMax = (byte)numericUpDown6.Value;
            m.criticalRank = (byte)numericUpDown7.Value;
            m.shrinkPer = (byte)numericUpDown8.Value;
            m.aiSeqNo = (ushort)numericUpDown9.Value;
            m.damageRecoverRatio = (sbyte)numericUpDown10.Value;
            m.hpRecoverRatio = (sbyte)numericUpDown11.Value;
            m.target = (byte)targetingComboBox.SelectedIndex;

            bool[] flags = new bool[32];
            flags[0] = checkBox1.Checked;
            flags[1] = checkBox2.Checked;
            flags[2] = checkBox3.Checked;
            flags[3] = checkBox4.Checked;
            flags[4] = checkBox5.Checked;
            flags[5] = checkBox6.Checked;
            flags[6] = checkBox7.Checked;
            flags[7] = checkBox8.Checked;
            flags[8] = checkBox9.Checked;
            flags[9] = checkBox10.Checked;
            flags[10] = checkBox11.Checked;
            flags[11] = checkBox12.Checked;
            flags[12] = checkBox13.Checked;
            flags[13] = checkBox14.Checked;
            flags[14] = checkBox15.Checked;
            flags[15] = checkBox16.Checked;
            flags[16] = checkBox17.Checked;
            flags[17] = checkBox18.Checked;
            m.SetFlags(flags);
        }

        private void ActivateControls()
        {
            listBox.SelectedIndexChanged += MoveChanged;
            isValidCheckBox.CheckedChanged += CommitEdit;
            typingComboBox.SelectedIndexChanged += CommitEdit;
            damageCategoryComboBox.SelectedIndexChanged += CommitEdit;
            numericUpDown1.ValueChanged += CommitEdit;
            numericUpDown2.ValueChanged += CommitEdit;
            numericUpDown3.ValueChanged += CommitEdit;

            statusEffectComboBox.SelectedIndexChanged += CommitEdit;
            numericUpDown12.ValueChanged += CommitEdit;
            numericUpDown13.ValueChanged += CommitEdit;
            numericUpDown14.ValueChanged += CommitEdit;
            numericUpDown15.ValueChanged += CommitEdit;

            statTypeComboBox1.SelectedIndexChanged += CommitEdit;
            statTypeComboBox2.SelectedIndexChanged += CommitEdit;
            statTypeComboBox3.SelectedIndexChanged += CommitEdit;
            numericUpDown16.ValueChanged += CommitEdit;
            numericUpDown18.ValueChanged += CommitEdit;
            numericUpDown20.ValueChanged += CommitEdit;
            numericUpDown17.ValueChanged += CommitEdit;
            numericUpDown19.ValueChanged += CommitEdit;
            numericUpDown21.ValueChanged += CommitEdit;

            moveCategoryComboBox.SelectedIndexChanged += CommitEdit;
            numericUpDown4.ValueChanged += CommitEdit;
            numericUpDown5.ValueChanged += CommitEdit;
            numericUpDown6.ValueChanged += CommitEdit;
            numericUpDown7.ValueChanged += CommitEdit;
            numericUpDown8.ValueChanged += CommitEdit;
            numericUpDown9.ValueChanged += CommitEdit;
            numericUpDown10.ValueChanged += CommitEdit;
            numericUpDown11.ValueChanged += CommitEdit;
            targetingComboBox.SelectedIndexChanged += CommitEdit;

            checkBox1.CheckedChanged += CommitEdit;
            checkBox2.CheckedChanged += CommitEdit;
            checkBox3.CheckedChanged += CommitEdit;
            checkBox4.CheckedChanged += CommitEdit;
            checkBox5.CheckedChanged += CommitEdit;
            checkBox6.CheckedChanged += CommitEdit;
            checkBox7.CheckedChanged += CommitEdit;
            checkBox8.CheckedChanged += CommitEdit;
            checkBox9.CheckedChanged += CommitEdit;
            checkBox10.CheckedChanged += CommitEdit;
            checkBox11.CheckedChanged += CommitEdit;
            checkBox12.CheckedChanged += CommitEdit;
            checkBox13.CheckedChanged += CommitEdit;
            checkBox14.CheckedChanged += CommitEdit;
            checkBox15.CheckedChanged += CommitEdit;
            checkBox16.CheckedChanged += CommitEdit;
            checkBox17.CheckedChanged += CommitEdit;
            checkBox18.CheckedChanged += CommitEdit;
        }

        private void DeactivateControls()
        {
            listBox.SelectedIndexChanged -= MoveChanged;
            isValidCheckBox.CheckedChanged -= CommitEdit;
            typingComboBox.SelectedIndexChanged -= CommitEdit;
            damageCategoryComboBox.SelectedIndexChanged -= CommitEdit;
            numericUpDown1.ValueChanged -= CommitEdit;
            numericUpDown2.ValueChanged -= CommitEdit;
            numericUpDown3.ValueChanged -= CommitEdit;

            statusEffectComboBox.SelectedIndexChanged -= CommitEdit;
            numericUpDown12.ValueChanged -= CommitEdit;
            numericUpDown13.ValueChanged -= CommitEdit;
            numericUpDown14.ValueChanged -= CommitEdit;
            numericUpDown15.ValueChanged -= CommitEdit;

            statTypeComboBox1.SelectedIndexChanged -= CommitEdit;
            statTypeComboBox2.SelectedIndexChanged -= CommitEdit;
            statTypeComboBox3.SelectedIndexChanged -= CommitEdit;
            numericUpDown16.ValueChanged -= CommitEdit;
            numericUpDown18.ValueChanged -= CommitEdit;
            numericUpDown20.ValueChanged -= CommitEdit;
            numericUpDown17.ValueChanged -= CommitEdit;
            numericUpDown19.ValueChanged -= CommitEdit;
            numericUpDown21.ValueChanged -= CommitEdit;

            moveCategoryComboBox.SelectedIndexChanged -= CommitEdit;
            numericUpDown4.ValueChanged -= CommitEdit;
            numericUpDown5.ValueChanged -= CommitEdit;
            numericUpDown6.ValueChanged -= CommitEdit;
            numericUpDown7.ValueChanged -= CommitEdit;
            numericUpDown8.ValueChanged -= CommitEdit;
            numericUpDown9.ValueChanged -= CommitEdit;
            numericUpDown10.ValueChanged -= CommitEdit;
            numericUpDown11.ValueChanged -= CommitEdit;
            targetingComboBox.SelectedIndexChanged -= CommitEdit;

            checkBox1.CheckedChanged -= CommitEdit;
            checkBox2.CheckedChanged -= CommitEdit;
            checkBox3.CheckedChanged -= CommitEdit;
            checkBox4.CheckedChanged -= CommitEdit;
            checkBox5.CheckedChanged -= CommitEdit;
            checkBox6.CheckedChanged -= CommitEdit;
            checkBox7.CheckedChanged -= CommitEdit;
            checkBox8.CheckedChanged -= CommitEdit;
            checkBox9.CheckedChanged -= CommitEdit;
            checkBox10.CheckedChanged -= CommitEdit;
            checkBox11.CheckedChanged -= CommitEdit;
            checkBox12.CheckedChanged -= CommitEdit;
            checkBox13.CheckedChanged -= CommitEdit;
            checkBox14.CheckedChanged -= CommitEdit;
            checkBox15.CheckedChanged -= CommitEdit;
            checkBox16.CheckedChanged -= CommitEdit;
            checkBox17.CheckedChanged -= CommitEdit;
            checkBox18.CheckedChanged -= CommitEdit;
        }

        private string FormatNumber(int n, int digits)
        {
            string output = n.ToString();
            while (output.Length < digits)
                output = '0' + output;
            return output;
        }

        private void OpenAnimationEditor(object sender, EventArgs e)
        {
            if (moveSequences == null)
            {
                moveSequences = new();
                moveSequences.Add("");
                moveSequences.AddRange(fileManager.GetMoveSequences());
            }
            MoveAnimationEditorForm maef = new(m, moveSequences);
            maef.ShowDialog();
        }
    }
}
