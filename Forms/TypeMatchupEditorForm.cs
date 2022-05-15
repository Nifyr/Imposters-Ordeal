using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GameDataTypes;
using static ImpostersOrdeal.GlobalData;

namespace ImpostersOrdeal
{
    public partial class TypeMatchupEditorForm : Form
    {
        private GlobalMetadata gm;
        private int typeWidth;
        private const int TypeCount = 18;

        private static readonly uint[] affinityColours =
        {
            0xFF2E3436,
            0, // unused
            0xFFA40000,
            0, // unused
            0xFFFFFFFF,
            0, 0, 0, // unused
            0xFF4E9A06
        };

        public TypeMatchupEditorForm()
        {
            gm = gameData.globalMetadata;
            InitializeComponent();

            typeWidth = pictureBox.Width / TypeCount;

            PopulateChart();
        }

        private byte[] GetAffinities()
        {
            byte[] affinities = new byte[TypeCount * TypeCount];
            for (int y = 0; y < TypeCount; y++)
                for (int x = 0; x < TypeCount; x++)
                    affinities[y * TypeCount + x] = gm.GetTypeMatchup(y, x);
            return affinities;
        }

        // Borrowing this part from pk3DS
        private void PopulateChart()
        {
            pictureBox.Image = GetBitmap(typeWidth, TypeCount, GetAffinities());
        }

        private static Bitmap GetBitmap(int itemsize, int itemsPerRow, byte[] vals)
        {
            // set up image
            var width = itemsize * itemsPerRow;
            var height = itemsize * vals.Length / itemsPerRow;
            byte[] bmpData = new byte[4 * width * height];

            // loop over area
            for (int i = 0; i < vals.Length; i++)
            {
                int X = i % itemsPerRow;
                int Y = i / itemsPerRow;

                // Plop into image
                byte[] itemColor = BitConverter.GetBytes(affinityColours[vals[i]]);
                for (int x = 0; x < itemsize * itemsize; x++)
                {
                    Buffer.BlockCopy(itemColor, 0, bmpData, (((Y * itemsize) + (x % itemsize)) * width * 4) + (((X * itemsize) + (x / itemsize)) * 4), 4);
                }
            }
            // slap on a grid
            byte[] gridColor = BitConverter.GetBytes(0x17000000);
            for (int i = 0; i < width * height; i++)
            {
                if (i % itemsize == 0 || i / (itemsize * itemsPerRow) % itemsize == 0)
                    Buffer.BlockCopy(gridColor, 0, bmpData, (i / (itemsize * itemsPerRow) * width * 4) + (i % (itemsize * itemsPerRow) * 4), 4);
            }

            // assemble image
            Bitmap b = new(width, height, PixelFormat.Format32bppArgb);
            BitmapData bData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(bmpData, 0, bData.Scan0, bmpData.Length);
            b.UnlockBits(bData);
            return b;
        }

        public void GetCoordinate(Control sender, MouseEventArgs e, out int X, out int Y)
        {
            X = e.X / typeWidth;
            Y = e.Y / typeWidth;
            if (e.X == sender.Width - 1 - 2) // tweak because the furthest pixel is unused for transparent effect, and 2 px are used for border
                X--;
            if (e.Y == sender.Height - 1 - 2)
                Y--;
        }

        private void PictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            GetCoordinate((PictureBox)sender, e, out int X, out int Y);
            if (X >= TypeCount || Y >= TypeCount)
                return;

            gm.SetTypeMatchup(Y, X, ToggleEffectiveness(gm.GetTypeMatchup(Y, X), e.Button == MouseButtons.Left));

            PopulateChart();
        }

        public static byte ToggleEffectiveness(byte currentValue, bool increase)
        {
            byte[] vals = { 0, 2, 4, 8 };
            int curIndex = Array.IndexOf(vals, currentValue);
            if (curIndex < 0)
                return currentValue;

            uint shift = (uint)(curIndex + (increase ? 1 : -1));
            var newIndex = shift % vals.Length;
            return vals[newIndex];
        }

        private void PictureBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBoxMouseClick(sender, e);
        }
    }
}
