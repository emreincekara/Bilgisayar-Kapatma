using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace BilgisayarKapatma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void Item_Ekle()
        {
            comboBox1.Items.AddRange(new object[] { "Bilgisayarı Kapat", "Yeniden Başlat", "Oturumu Kapat", "Hazırda Beklet", "Uyku" });
            comboBox1.SelectedIndex = 0;
            for (int i = 0; i <= 100; i++)
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.SelectedIndex = 0;
            for (int i = 0; i <= 59; i++)
            {
                comboBox3.Items.Add(i);
                comboBox4.Items.Add(i);
            }
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Item_Ekle();
            KeyboardHook.CreateHook(KeyReader);
            toolStripStatusLabel2.Text = string.Empty;
            lbl_saat.Text = DateTime.Now.ToLongTimeString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            if (comboBox1.SelectedItem.ToString() == "Bilgisayarı Kapat")
            {
                label9.Text = "Bilgisayar Kapatılacak";
                Kapatma_Sekli = "-f -s -t 0";
                uyku_modu = false;
            }
            if (comboBox1.SelectedItem.ToString() == "Yeniden Başlat")
            {
                label9.Text = "Yeniden Başlatılacak";
                Kapatma_Sekli = "-f -r -t 0";
                uyku_modu = false;
            }
            if (comboBox1.SelectedItem.ToString() == "Oturumu Kapat")
            {
                label9.Text = "Oturum Kapatılacak";
                Kapatma_Sekli = "-f -l -t 0";
                uyku_modu = false;
            }
            if (comboBox1.SelectedItem.ToString() == "Hazırda Beklet")
            {
                label9.Text = "Hazırda Bekletilecek";
                Kapatma_Sekli = "-f -h -t 0";
                uyku_modu = false;
            }
            if (comboBox1.SelectedItem.ToString() == "Uyku")
            {
                label9.Text = "Uyku Modu";
                uyku_modu = true;
                Kapatma_Sekli = "";
            }
        }

        bool uyku_modu = false;

        string sec_saat = "";
        string sec_dakika = "";
        string sec_saniye = "";

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1) return;
            sec_saat = comboBox2.SelectedItem.ToString();
            saat = sec_saat;
            timer1.Start();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == -1) return;
            sec_dakika = comboBox3.SelectedItem.ToString();
            dakika = sec_dakika;
            timer1.Start();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == -1) return;
            sec_saniye = comboBox4.SelectedItem.ToString();
            saniye = sec_saniye;
            timer1.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false;
            }
        }

        int toplam_sure = 0;

        bool kapanma_kontrol = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (sec_saat != "0" || sec_dakika != "0" || sec_saniye != "0")
            {
                if (Convert.ToInt32(sec_saat) <= 100 && Convert.ToInt32(sec_dakika) <= 59 && Convert.ToInt32(sec_saniye) <= 59)
                {
                    button1.Enabled = false;
                    button2.Enabled = true;
                    groupBox1.Enabled = false;
                    groupBox4.Visible = true;
                    this.ClientSize = new System.Drawing.Size(265, 456);
                    toplam_sure = (Convert.ToInt32(sec_saat) * 3600) + (Convert.ToInt32(sec_dakika) * 60) + Convert.ToInt32(sec_saniye);
                    progressBar1.Maximum = toplam_sure;
                    kalan_sure = toplam_sure;
                    lbl_durum.Text = "Başlatıldı!";
                    lbl_durum.ForeColor = Color.Green;
                    zamanlayici.Start();
                    kapanma_kontrol = true;
                }
                else
                {
                    MessageBox.Show("Hatalı zaman dilimi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("İşlem için gerekli olan süreyi seçmediniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            groupBox1.Enabled = true;
            zamanlayici.Stop();
            gecen_sure = 0;
            kalan_sure = 0;
            progressbar_suresi = 0;
            progressBar1.Value = 0;
            toplam_sure = 0;
            label12.Text = "...";
            label14.Text = "...";
            lbl_durum.Text = "Durduruldu!";
            lbl_durum.ForeColor = Color.Red;
            kapanma_kontrol = false;
        }

        string Kapatma_Sekli = "";

        public void Pc_Kapatma()
        {
            if (uyku_modu == true)
            {
                Application.SetSuspendState(PowerState.Suspend, false, false);
                button2.PerformClick();
                return;
            }
            if (Kapatma_Sekli != "")
            {
                Process islem = new Process();
                islem.StartInfo = new ProcessStartInfo("shutdown", Kapatma_Sekli);
                islem.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                islem.Start();
                //Process.Start("shutdown", Kapatma_Sekli);
            }
        }

        int gecen_sure = 0;
        int kalan_sure = 0;
        int progressbar_suresi = 0;

        private void zamanlayici_Tick(object sender, EventArgs e)
        {
            label12.Text = gecen_sure++.ToString();
            label14.Text = kalan_sure--.ToString();
            if (progressbar_suresi < toplam_sure)
            {
                progressBar1.Value = progressbar_suresi;
                progressbar_suresi++;
            }
            else if (progressbar_suresi == toplam_sure)
            {
                zamanlayici.Stop();
                progressBar1.Value = progressbar_suresi;
                lbl_durum.Text = label9.Text + "!";
                lbl_durum.ForeColor = Color.Green;
                Pc_Kapatma();
            }
        }

        private void çocukKilidiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kapanma_kontrol == true)
            {
                DialogResult soru = MessageBox.Show("Program kendini gizleyerek arka planda çalışacaktır." + "\n" + "Programı görünür yapmak için " + Settings1.Default.kisayol + " kısayol tuşunu kullanabilirsiniz." + "\n" + "Gizlemek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (soru == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Visible = false;
                    this.ShowInTaskbar = false;
                    RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies", true);
                    rkey.CreateSubKey("System", RegistryKeyPermissionCheck.Default);
                    rkey.Close();
                    RegistryKey rkey2 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
                    rkey2.SetValue("DisableTaskMgr", 1);
                    rkey2.Close();
                    sifre_kontrol = false;
                    sifre_kontrolu.Start();
                }
            }
            else
            {
                MessageBox.Show("Bu özelliği kullanabilmek için öncelikle programı başlatmalısınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void KeyReader(IntPtr wParam, IntPtr lParam)
        {
            int key = Marshal.ReadInt32(lParam);
            KeyboardHook.VK vk = (KeyboardHook.VK)key;
            String temp = "";

            switch (vk)
            {
                case KeyboardHook.VK.VK_F1: temp = "<-F1->";
                    break;
                case KeyboardHook.VK.VK_F2: temp = "<-F2->";
                    break;
                case KeyboardHook.VK.VK_F3: temp = "<-F3->";
                    break;
                case KeyboardHook.VK.VK_F4: temp = "<-F4->";
                    break;
                case KeyboardHook.VK.VK_F5: temp = "<-F5->";
                    break;
                case KeyboardHook.VK.VK_F6: temp = "<-F6->";
                    break;
                case KeyboardHook.VK.VK_F7: temp = "<-F7->";
                    break;
                case KeyboardHook.VK.VK_F8: temp = "<-F8->";
                    break;
                case KeyboardHook.VK.VK_F9: temp = "<-F9->";
                    break;
                case KeyboardHook.VK.VK_F10: temp = "<-F10->";
                    break;
                case KeyboardHook.VK.VK_F11: temp = "<-F11->";
                    break;
                case KeyboardHook.VK.VK_F12: temp = "<-F12->";
                    break;
                case KeyboardHook.VK.VK_NUMLOCK: temp = "<-numlock->";
                    break;
                case KeyboardHook.VK.VK_SCROLL: temp = "<-scroll>";
                    break;
                case KeyboardHook.VK.VK_LSHIFT: temp = "<-left shift->";
                    break;
                case KeyboardHook.VK.VK_RSHIFT: temp = "<-right shift->";
                    break;
                case KeyboardHook.VK.VK_LCONTROL: temp = "<-left control->";
                    break;
                case KeyboardHook.VK.VK_RCONTROL: temp = "<-right control->";
                    break;
                case KeyboardHook.VK.VK_SEPERATOR: temp = "|";
                    break;
                case KeyboardHook.VK.VK_SUBTRACT: temp = "-";
                    break;
                case KeyboardHook.VK.VK_ADD: temp = "+";
                    break;
                case KeyboardHook.VK.VK_DECIMAL: temp = ".";
                    break;
                case KeyboardHook.VK.VK_DIVIDE: temp = "/";
                    break;
                case KeyboardHook.VK.VK_NUMPAD0: temp = "0";
                    break;
                case KeyboardHook.VK.VK_NUMPAD1: temp = "1";
                    break;
                case KeyboardHook.VK.VK_NUMPAD2: temp = "2";
                    break;
                case KeyboardHook.VK.VK_NUMPAD3: temp = "3";
                    break;
                case KeyboardHook.VK.VK_NUMPAD4: temp = "4";
                    break;
                case KeyboardHook.VK.VK_NUMPAD5: temp = "5";
                    break;
                case KeyboardHook.VK.VK_NUMPAD6: temp = "6";
                    break;
                case KeyboardHook.VK.VK_NUMPAD7: temp = "7";
                    break;
                case KeyboardHook.VK.VK_NUMPAD8: temp = "8";
                    break;
                case KeyboardHook.VK.VK_NUMPAD9: temp = "9";
                    break;
                case KeyboardHook.VK.VK_Q: temp = "q";
                    break;
                case KeyboardHook.VK.VK_W: temp = "w";
                    break;
                case KeyboardHook.VK.VK_E: temp = "e";
                    break;
                case KeyboardHook.VK.VK_R: temp = "r";
                    break;
                case KeyboardHook.VK.VK_T: temp = "t";
                    break;
                case KeyboardHook.VK.VK_Y: temp = "y";
                    break;
                case KeyboardHook.VK.VK_U: temp = "u";
                    break;
                case KeyboardHook.VK.VK_I: temp = "i";
                    break;
                case KeyboardHook.VK.VK_O: temp = "o";
                    break;
                case KeyboardHook.VK.VK_P: temp = "p";
                    break;
                case KeyboardHook.VK.VK_A: temp = "a";
                    break;
                case KeyboardHook.VK.VK_S: temp = "s";
                    break;
                case KeyboardHook.VK.VK_D: temp = "d";
                    break;
                case KeyboardHook.VK.VK_F: temp = "f";
                    break;
                case KeyboardHook.VK.VK_G: temp = "g";
                    break;
                case KeyboardHook.VK.VK_H: temp = "h";
                    break;
                case KeyboardHook.VK.VK_J: temp = "j";
                    break;
                case KeyboardHook.VK.VK_K: temp = "k";
                    break;
                case KeyboardHook.VK.VK_L: temp = "l";
                    break;
                case KeyboardHook.VK.VK_Z: temp = "z";
                    break;
                case KeyboardHook.VK.VK_X: temp = "x";
                    break;
                case KeyboardHook.VK.VK_C: temp = "c";
                    break;
                case KeyboardHook.VK.VK_V: temp = "v";
                    break;
                case KeyboardHook.VK.VK_B: temp = "b";
                    break;
                case KeyboardHook.VK.VK_N: temp = "n";
                    break;
                case KeyboardHook.VK.VK_M: temp = "m";
                    break;
                case KeyboardHook.VK.VK_0: temp = "0";
                    break;
                case KeyboardHook.VK.VK_1: temp = "1";
                    break;
                case KeyboardHook.VK.VK_2: temp = "2";
                    break;
                case KeyboardHook.VK.VK_3: temp = "3";
                    break;
                case KeyboardHook.VK.VK_4: temp = "4";
                    break;
                case KeyboardHook.VK.VK_5: temp = "5";
                    break;
                case KeyboardHook.VK.VK_6: temp = "6";
                    break;
                case KeyboardHook.VK.VK_7: temp = "7";
                    break;
                case KeyboardHook.VK.VK_8: temp = "8";
                    break;
                case KeyboardHook.VK.VK_9: temp = "9";
                    break;
                case KeyboardHook.VK.VK_SNAPSHOT: temp = "<-print screen->";
                    break;
                case KeyboardHook.VK.VK_INSERT: temp = "<-insert->";
                    break;
                case KeyboardHook.VK.VK_DELETE: temp = "<-delete->";
                    break;
                case KeyboardHook.VK.VK_BACK: temp = "<-backspace->";
                    break;
                case KeyboardHook.VK.VK_TAB: temp = "<-tab->";
                    break;
                case KeyboardHook.VK.VK_RETURN: temp = "<-enter->";
                    break;
                case KeyboardHook.VK.VK_PAUSE: temp = "<-pause->";
                    break;
                case KeyboardHook.VK.VK_CAPITAL: temp = "<-caps lock->";
                    break;
                case KeyboardHook.VK.VK_ESCAPE: temp = "<-esc->";
                    break;
                case KeyboardHook.VK.VK_SPACE: temp = " "; //was <-space->
                    break;
                case KeyboardHook.VK.VK_PRIOR: temp = "<-page up->";
                    break;
                case KeyboardHook.VK.VK_NEXT: temp = "<-page down->";
                    break;
                case KeyboardHook.VK.VK_END: temp = "<-end->";
                    break;
                case KeyboardHook.VK.VK_HOME: temp = "<-home->";
                    break;
                case KeyboardHook.VK.VK_LEFT: temp = "<-arrow left->";
                    break;
                case KeyboardHook.VK.VK_UP: temp = "<-arrow up->";
                    break;
                case KeyboardHook.VK.VK_RIGHT: temp = "<-arrow right->";
                    break;
                case KeyboardHook.VK.VK_DOWN: temp = "<-arrow down->";
                    break;
                default: break;
            }
            if (temp == Settings1.Default.kisayol)
            {
                if (form_kontrol == false && this.Visible == false)
                {
                    new Form3().ShowDialog();
                }                
            }
        }

        public static bool form_kontrol = false;
        public static bool sifre_kontrol = false;

        public void Goster()
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies", true);
            rkey.CreateSubKey("System", RegistryKeyPermissionCheck.Default);
            rkey.Close();
            RegistryKey rkey2 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
            rkey2.SetValue("DisableTaskMgr", 0);
            rkey2.Close();
        }

        private void programAyarlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void sifre_kontrolu_Tick(object sender, EventArgs e)
        {
            if (sifre_kontrol == true)
            {
                Goster();
                sifre_kontrolu.Stop();
            }
        }

        private void kısayolTuşuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form5().ShowDialog();
        }

        private void programŞifresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form4().ShowDialog();
        }

        string saat;
        string dakika;
        string saniye;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sec_saat) <= 9)
            {
                saat = "0" + sec_saat;
            }
            else
            {
                saat = sec_saat;
            }
            if (Convert.ToInt32(sec_dakika) <= 9)
            {
                dakika = "0" + sec_dakika;
            }
            else
            {
                dakika = sec_dakika;
            }
            if (Convert.ToInt32(sec_saniye) <= 9)
            {
                saniye = "0" + sec_saniye;
            }
            else
            {
                saniye = sec_saniye;
            }
            label10.Text = saat + " : " + dakika + " : " + saniye;
            timer1.Stop();
        }

        private void saat_tmr_Tick(object sender, EventArgs e)
        {
            lbl_saat.Text = DateTime.Now.ToLongTimeString();
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("  Bu program Emre İncekara tarafından yapılmıştır." + "\n\n\t" + "                 © 2013", "Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
