using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BilgisayarKapatma
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (checkBox1.Checked == true)
                {
                    if (key_kontrol != true)
                    {
                        key.SetValue("PCShutdown", "\"" + Application.ExecutablePath + "\"");
                    }
                }
                else
                {
                    if (key_kontrol == true)
                    {
                        key.DeleteValue("PCShutdown");
                    }
                }
                lbl_durum.Text = "Ayarlandı!";
                lbl_durum.ForeColor = Color.Green;
            }
            catch
            {
                lbl_durum.Text = "Başarısız!";
                lbl_durum.ForeColor = Color.Red;
            }            
        }

        bool key_kontrol = false;

        private void Form2_Load(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key.GetValue("PCShutdown") != null)
            {
                checkBox1.Checked = true;
                key_kontrol = true;
            }
            else
            {
                checkBox1.Checked = false;
                key_kontrol = false;
            }
        }
    }
}
