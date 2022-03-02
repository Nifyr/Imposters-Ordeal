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
    public partial class PickupEditorForm : Form
    {
        List<PickupItem> pickupItems;
        List<string> items;

        public PickupEditorForm()
        {
            pickupItems = gameData.pickupItems;
            items = gameData.items.Select(i => i.GetName()).ToList();
            InitializeComponent();

            ItemColumn.DataSource = items.ToArray();
            Lv10Column.ValueType = typeof(int);
            Lv20Column.ValueType = typeof(int);
            Lv30Column.ValueType = typeof(int);
            Lv40Column.ValueType = typeof(int);
            Lv50Column.ValueType = typeof(int);
            Lv60Column.ValueType = typeof(int);
            Lv70Column.ValueType = typeof(int);
            Lv80Column.ValueType = typeof(int);
            Lv90Column.ValueType = typeof(int);
            Lv100Column.ValueType = typeof(int);

            foreach (PickupItem p in pickupItems)
                dataGridView.Rows.Add(items[p.itemID], (int)p.ratios[0],
                    (int)p.ratios[1], (int)p.ratios[2], (int)p.ratios[3],
                    (int)p.ratios[4], (int)p.ratios[5], (int)p.ratios[6],
                    (int)p.ratios[7], (int)p.ratios[8], (int)p.ratios[9]);

            ActivateControls();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            pickupItems = new();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                PickupItem p = new();
                p.ratios = new();
                p.itemID = (ushort)items.IndexOf((string)row.Cells[0].Value);
                for (int i = 0; i < 10; i++)
                    p.ratios.Add((byte)(int)row.Cells[1 + i].Value);
                pickupItems.Add(p);
            }
            gameData.pickupItems = pickupItems;
        }

        private void ActivateControls()
        {
            dataGridView.CellEndEdit += CommitEdit;
        }

        private void DeactivateControls()
        {
            dataGridView.CellEndEdit -= CommitEdit;
        }
    }
}
