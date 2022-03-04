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
    public partial class ShopEditorForm : Form
    {
        List<MartItem> martItems;
        List<FixedShopItem> fixedShopItems;
        List<BpShopItem> bpShopItems;
        List<string> items;

        public ShopEditorForm()
        {
            martItems = gameData.shopTables.martItems;
            fixedShopItems = gameData.shopTables.fixedShopItems;
            bpShopItems = gameData.shopTables.bpShopItems;
            items = gameData.items.Select(i => i.GetName()).ToList();

            InitializeComponent();

            commonItemColumn.DataSource = items.ToArray();
            badgeCountColumn.ValueType = typeof(int);
            zoneIDColumn.ValueType = typeof(int);
            fixedItemColumn.DataSource = items.ToArray();
            shopColumn.ValueType = typeof(int);
            bpItemColumn.DataSource = items.ToArray();
            npcColumn.ValueType = typeof(int);

            foreach (MartItem m in martItems)
                martDataGridView.Rows.Add(new object[] { items[m.itemID], m.badgeNum, m.zoneID });

            foreach (FixedShopItem f in fixedShopItems)
                fixedShopDataGridView.Rows.Add(new object[] { items[f.itemID], f.shopID });

            foreach (BpShopItem b in bpShopItems)
                bpShopDataGridView.Rows.Add(new object[] { items[b.itemID], b.npcID });

            ActivateControls();
        }

        private void CommitEdit(object sender, EventArgs e)
        {
            martItems = new();
            foreach (DataGridViewRow row in martDataGridView.Rows)
            {
                if (row.Cells[0].Value == null ||
                    row.Cells[1].Value == null ||
                    row.Cells[2].Value == null ||
                    items.IndexOf((string)row.Cells[0].Value) == 0)
                    continue;
                MartItem m = new();
                m.itemID = (ushort)items.IndexOf((string)row.Cells[0].Value);
                m.badgeNum = (int)row.Cells[1].Value;
                m.zoneID = (int)row.Cells[2].Value;
                martItems.Add(m);
            }
            gameData.shopTables.martItems = martItems;

            fixedShopItems = new();
            foreach (DataGridViewRow row in fixedShopDataGridView.Rows)
            {
                if (row.Cells[0].Value == null ||
                    row.Cells[1].Value == null ||
                    items.IndexOf((string)row.Cells[0].Value) == 0)
                    continue;
                FixedShopItem f = new();
                f.itemID = (ushort)items.IndexOf((string)row.Cells[0].Value);
                f.shopID = (int)row.Cells[1].Value;
                fixedShopItems.Add(f);
            }
            gameData.shopTables.fixedShopItems = fixedShopItems;

            bpShopItems = new();
            foreach (DataGridViewRow row in bpShopDataGridView.Rows)
            {
                if (row.Cells[0].Value == null ||
                    row.Cells[1].Value == null ||
                    items.IndexOf((string)row.Cells[0].Value) == 0)
                    continue;
                BpShopItem b = new();
                b.itemID = (ushort)items.IndexOf((string)row.Cells[0].Value);
                b.npcID = (int)row.Cells[1].Value;
                bpShopItems.Add(b);
            }
            gameData.shopTables.bpShopItems = bpShopItems;
        }

        private void ActivateControls()
        {
            martDataGridView.CellEndEdit += CommitEdit;
            martDataGridView.UserDeletedRow += CommitEdit;
            fixedShopDataGridView.CellEndEdit += CommitEdit;
            fixedShopDataGridView.UserDeletedRow += CommitEdit;
            bpShopDataGridView.CellEndEdit += CommitEdit;
            bpShopDataGridView.UserDeletedRow += CommitEdit;
        }

        private void DeactivateControls()
        {
            martDataGridView.CellEndEdit -= CommitEdit;
            martDataGridView.UserDeletedRow -= CommitEdit;
            fixedShopDataGridView.CellEndEdit -= CommitEdit;
            fixedShopDataGridView.UserDeletedRow -= CommitEdit;
            bpShopDataGridView.CellEndEdit -= CommitEdit;
            bpShopDataGridView.UserDeletedRow -= CommitEdit;
        }

        private void MartDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { items[0], 0, -1 });
        }

        private void FixedShopDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { items[0], 0 });
        }

        private void BpShopDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.SetValues(new object[] { items[0], 0 });
        }

        private void DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MainForm.ShowDataError();
        }
    }
}
