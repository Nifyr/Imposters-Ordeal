using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ImpostersOrdeal.GlobalData;
using static ImpostersOrdeal.GameDataTypes;

namespace ImpostersOrdeal
{
    public partial class GlobalMetadataEditorForm : Form
    {
        GlobalMetadata gm;
        ImageDefinition id;
        TypeDefinition td;
        FieldDefinition fd;

        public GlobalMetadataEditorForm()
        {
            gm = gameData.globalMetadata;
            InitializeComponent();

            imageListBox.DataSource = GetValidChildren(gm.images);
            id = (ImageDefinition)imageListBox.SelectedItem;
            typeListBox.DataSource = GetValidChildren(id.types);
            td = (TypeDefinition)typeListBox.SelectedItem;
            fieldListBox.DataSource = GetValidChildren(td.fields, false);
            fd = (FieldDefinition)fieldListBox.SelectedItem;
            RefreshByteDisplay();

            ActivateControls();
        }

        private static List<IGMObject> GetValidChildren(IEnumerable<IGMObject> t, bool sort = true)
        {
            List<IGMObject> l = t.Where(o => o.HasDefault()).ToList();
            if (sort)
                l.Sort((o1, o2) => o1.ToString().CompareTo(o2.ToString()));
            return l;
        }

        private void ImageChanged(object sender, EventArgs e)
        {
            id = (ImageDefinition)imageListBox.SelectedItem;
            typeListBox.DataSource = GetValidChildren(id.types);
            typeListBox.SelectedIndex = 0;
        }

        private void TypeChanged(object sender, EventArgs e)
        {
            td = (TypeDefinition)typeListBox.SelectedItem;
            fieldListBox.DataSource = GetValidChildren(td.fields, false);
            fieldListBox.SelectedIndex = 0;
        }

        private void FieldChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            fd = (FieldDefinition)fieldListBox.SelectedItem;
            RefreshByteDisplay();

            ActivateControls();
        }

        private void RTBChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            SetBytes(fd.defautValue, ParseBytes(defaultValueRichTextBox.Text, fd.defautValue.length));
            RefreshByteDisplay();

            ActivateControls();
        }

        private void UIntChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            int length = GetBytes(fd.defautValue).Length;
            try
            {
                switch (length)
                {
                    case 1:
                        SetBytes(fd.defautValue, new byte[] { byte.Parse(uIntTextBox.Text) });
                        break;
                    case 2:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(ushort.Parse(uIntTextBox.Text)));
                        break;
                    case 4:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(uint.Parse(uIntTextBox.Text)));
                        break;
                    case 8:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(ulong.Parse(uIntTextBox.Text)));
                        break;
                }
            } catch { }
            RefreshByteDisplay();

            ActivateControls();
        }

        private void IntChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            int length = GetBytes(fd.defautValue).Length;
            try
            {
                switch (length)
                {
                    case 1:
                        SetBytes(fd.defautValue, new byte[] { (byte)sbyte.Parse(intTextBox.Text) });
                        break;
                    case 2:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(short.Parse(intTextBox.Text)));
                        break;
                    case 4:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(int.Parse(intTextBox.Text)));
                        break;
                    case 8:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(long.Parse(intTextBox.Text)));
                        break;
                }
            }
            catch { }
            RefreshByteDisplay();

            ActivateControls();
        }

        private void DecimalChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            int length = GetBytes(fd.defautValue).Length;
            try
            {
                switch (length)
                {
                    case 4:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(float.Parse(decimalTextBox.Text)));
                        break;
                    case 8:
                        SetBytes(fd.defautValue, BitConverter.GetBytes(double.Parse(decimalTextBox.Text)));
                        break;
                    case 16:
                        SetBytes(fd.defautValue, decimal.GetBits(decimal.Parse(decimalTextBox.Text)).SelectMany(i => BitConverter.GetBytes(i)).ToArray());
                        break;
                }
            }
            catch { }
            RefreshByteDisplay();

            ActivateControls();
        }

        private void StringChanged(object sender, EventArgs e)
        {
            DeactivateControls();

            int length = GetBytes(fd.defautValue).Length;
            try
            {
                if (length == 2)
                {
                    SetBytes(fd.defautValue, BitConverter.GetBytes(char.Parse(stringTextBox.Text)));
                }
                else
                {
                    byte[] rawText = Encoding.UTF8.GetBytes(stringTextBox.Text);
                    byte[] b = new byte[rawText.Length + 4];
                    Buffer.BlockCopy(BitConverter.GetBytes(length - 4), 0, b, 0, 4);
                    Buffer.BlockCopy(rawText, 0, b, 4, rawText.Length);
                    SetBytes(fd.defautValue, b);
                }
            }
            catch { }
            RefreshByteDisplay();

            ActivateControls();
        }

        private void RefreshByteDisplay()
        {
            byte[] b = GetBytes(fd.defautValue);
            defaultValueRichTextBox.Text = BitConverter.ToString(b).Replace("-", " ");
            switch (b.Length)
            {
                case 1:
                    uIntTextBox.Text = b[0].ToString();
                    uIntTextBox.Enabled = true;
                    intTextBox.Text = ((sbyte)b[0]).ToString();
                    intTextBox.Enabled = true;
                    break;
                case 2:
                    uIntTextBox.Text = BitConverter.ToUInt16(b, 0).ToString();
                    uIntTextBox.Enabled = true;
                    intTextBox.Text = BitConverter.ToInt16(b, 0).ToString();
                    intTextBox.Enabled = true;
                    break;
                case 4:
                    uIntTextBox.Text = BitConverter.ToUInt32(b, 0).ToString();
                    uIntTextBox.Enabled = true;
                    intTextBox.Text = BitConverter.ToInt32(b, 0).ToString();
                    intTextBox.Enabled = true;
                    break;
                case 8:
                    uIntTextBox.Text = BitConverter.ToUInt64(b, 0).ToString();
                    uIntTextBox.Enabled = true;
                    intTextBox.Text = BitConverter.ToInt64(b, 0).ToString();
                    intTextBox.Enabled = true;
                    break;
                default:
                    uIntTextBox.Text = "n/a";
                    uIntTextBox.Enabled = false;
                    intTextBox.Text = "n/a";
                    intTextBox.Enabled = false;
                    break;
            }
            switch (b.Length)
            {
                case 4:
                    decimalTextBox.Text = BitConverter.ToSingle(b, 0).ToString();
                    decimalTextBox.Enabled = true;
                    break;
                case 8:
                    decimalTextBox.Text = BitConverter.ToDouble(b, 0).ToString();
                    decimalTextBox.Enabled = true;
                    break;
                case 16:
                    try
                    {
                        int[] ints = new int[4];
                        for (int i = 0; i < ints.Length; i++)
                            ints[i] = BitConverter.ToInt32(b, i * 4);
                        decimalTextBox.Text = new decimal(ints).ToString();
                    }
                    catch
                    {
                        decimalTextBox.Text = "NaN";
                    }
                    decimalTextBox.Enabled = true;
                    break;
                default:
                    decimalTextBox.Text = "n/a";
                    decimalTextBox.Enabled = false;
                    break;
            }
            try
            {
                if (b.Length == 2)
                    stringTextBox.Text = Encoding.Unicode.GetString(b);
                else
                    stringTextBox.Text = Encoding.UTF8.GetString(b, 4, b.Length - 4);
                stringTextBox.Enabled = true;
            }
            catch
            {
                stringTextBox.Text = "n/a";
                stringTextBox.Enabled = false;
            }
        }

        private static byte[] ParseBytes(string text, int length)
        {
            string[] byteStrings = text.Split(" ");
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
                try
                {
                    bytes[i] = Convert.ToByte(byteStrings[i], 16);
                } catch { }
            return bytes;
        }

        private byte[] GetBytes(FieldDefaultValue fdv)
        {
            byte[] b = new byte[fdv.length];
            Buffer.BlockCopy(gm.buffer, (int)fdv.offset, b, 0, b.Length);
            return b;
        }

        private void SetBytes(FieldDefaultValue fdv, byte[] b)
        {
            Buffer.BlockCopy(b, 0, gm.buffer, (int)fdv.offset, Math.Min(b.Length, fdv.length));
        }

        private void ActivateControls()
        {
            imageListBox.SelectedIndexChanged += ImageChanged;
            typeListBox.SelectedIndexChanged += TypeChanged;
            fieldListBox.SelectedIndexChanged += FieldChanged;
            defaultValueRichTextBox.LostFocus += RTBChanged;
            uIntTextBox.LostFocus += UIntChanged;
            intTextBox.LostFocus += IntChanged;
            decimalTextBox.LostFocus += DecimalChanged;
            stringTextBox.LostFocus += StringChanged;
        }

        private void DeactivateControls()
        {
            imageListBox.SelectedIndexChanged -= ImageChanged;
            typeListBox.SelectedIndexChanged -= TypeChanged;
            fieldListBox.SelectedIndexChanged -= FieldChanged;
            defaultValueRichTextBox.LostFocus -= RTBChanged;
            uIntTextBox.LostFocus -= UIntChanged;
            intTextBox.LostFocus -= IntChanged;
            decimalTextBox.LostFocus -= DecimalChanged;
            stringTextBox.LostFocus -= StringChanged;
        }
    }
}
