using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BilgisayarKapatma
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                comboBox1.Items.Add("F" + i.ToString());
            }
            for (int i = 0; i <= comboBox1.Items.Count; i++)
            {
                if (Settings1.Default.kisayol == "<-" + comboBox1.Items[i] + "->")
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kisayol_tusu = "";
            switch (comboBox1.SelectedItem.ToString())
            {
                case "F1": kisayol_tusu = "<-F1->";
                    break;
                case "F2": kisayol_tusu = "<-F2->";
                    break;
                case "F3": kisayol_tusu = "<-F3->";
                    break;
                case "F4": kisayol_tusu = "<-F4->";
                    break;
                case "F5": kisayol_tusu = "<-F5->";
                    break;
                case "F6": kisayol_tusu = "<-F6->";
                    break;
                case "F7": kisayol_tusu = "<-F7->";
                    break;
                case "F8": kisayol_tusu = "<-F8->";
                    break;
                case "F9": kisayol_tusu = "<-F9->";
                    break;
                case "F10": kisayol_tusu = "<-F10->";
                    break;
                case "F11": kisayol_tusu = "<-F11->";
                    break;
                case "F12": kisayol_tusu = "<-F12->";
                    break;
                default: break;
            }
            Settings1.Default.kisayol = kisayol_tusu;
            Settings1.Default.Save();
            lbl_durum.Text = "Değiştirildi!";
            lbl_durum.ForeColor = Color.Green;
        }
    }
}
