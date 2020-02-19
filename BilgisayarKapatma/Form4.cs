using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace BilgisayarKapatma
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                if (textBox1.Text == Settings1.Default.sifre)
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        Settings1.Default.sifre = MD5_Sifrele(textBox2.Text);
                        Settings1.Default.Save();
                        lbl_durum.Text = "Şifre değiştirildi!";
                        lbl_durum.ForeColor = Color.Green;
                    }
                    else
                    {
                        lbl_durum.Text = "Girdiğiniz şifreler aynı değil!";
                        lbl_durum.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lbl_durum.Text = "Eski şifre yanlış!";
                    lbl_durum.ForeColor = Color.Red;
                }
            }
            else
            {
                lbl_durum.Text = "Boş alan bırakmayınız!";
                lbl_durum.ForeColor = Color.Red;
            }
        }

        public static string MD5_Sifrele(string metin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.PasswordChar = '\0';
            }
            else
            {
                textBox1.PasswordChar = '●';
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '●';
                textBox3.PasswordChar = '●';
            }
        }
    }
}
