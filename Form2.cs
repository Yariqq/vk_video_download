using System;
using System.Collections.Generic;
using System.Windows.Forms;using System.Diagnostics;
using System.Net;

namespace vk_youtube_kyrsach
{
    public partial class Form2 : Form
    {
        WebClient client = new WebClient();
        List<info> dta = new List<info>();
        static string[] value = null;
        static int j, k, count, sch, difQualCount = 0;

        WebBrowser wb = new WebBrowser();
        public Form2(List<info> data)
        {
            dta = data;
            InitializeComponent();
            if (data.Count != 0)
            {
                for (int index = 0; index < data.Count; index++)
                {
                    CBVideoList.Items.Add(data[index].title);
                }
            }
        }

        private void butDownload_Click(object sender, EventArgs e)
        {
            if (CBVideoList.CheckedItems.Count == 0)
                MessageBox.Show("Вы не выбрали видео");
            else
            {
                string splitLink;
                if (dta[CBVideoList.SelectedIndex].link[12] == 'y')
                {
                    splitLink = dta[CBVideoList.SelectedIndex].link.Insert(12, "ss");
                    Process.Start(splitLink);
                }
                else
                {
                    string HTML = client.DownloadString(dta[CBVideoList.SelectedIndex].link);
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(HTML);
                    var tdList = htmlDoc.DocumentNode.SelectNodes("//source");
                    if (tdList != null)
                    {
                        string[] dot = null;

                        if (radioButton1.Checked)
                        {
                            for (int i = 1; i < tdList.Count; i++)
                            {
                                value = tdList[i].Attributes["src"].Value.Split('?');
                                dot = value[0].Split('.');
                                string qual = dot[dot.Length - 2];
                                if (qual + 'p' == radioButton1.Text)
                                    break;
                            }
                        }

                        if (radioButton2.Checked)
                        {
                            for (int i = 1; i < tdList.Count; i++)
                            {
                                value = tdList[i].Attributes["src"].Value.Split('?');
                                dot = value[0].Split('.');
                                string qual = dot[dot.Length - 2];
                                if (qual + 'p' == radioButton2.Text)
                                    break;
                            }
                        }
                        if (radioButton3.Checked)
                        {
                            for (int i = 1; i < tdList.Count; i++)
                            {
                                value = tdList[i].Attributes["src"].Value.Split('?');
                                dot = value[0].Split('.');
                                string qual = dot[dot.Length - 2];
                                if (qual + 'p' == radioButton3.Text)
                                    break;
                            }
                        }
                        if (radioButton4.Checked)
                        {
                            for (int i = 1; i < tdList.Count; i++)
                            {
                                value = tdList[i].Attributes["src"].Value.Split('?');
                                dot = value[0].Split('.');
                                string qual = dot[dot.Length - 2];
                                if (qual + 'p' == radioButton4.Text)
                                    break;
                            }
                        }
                        if (radioButton5.Checked)
                        {
                            for (int i = 1; i < tdList.Count; i++)
                            {
                                value = tdList[i].Attributes["src"].Value.Split('?');
                                dot = value[0].Split('.');
                                string qual = dot[dot.Length - 2];
                                if (qual + 'p' == radioButton5.Text)
                                    break;
                            }
                        }
                        if (radioButton6.Checked)
                        {
                            for (int i = 1; i < tdList.Count; i++)
                            {
                                value = tdList[i].Attributes["src"].Value.Split('?');
                                dot = value[0].Split('.');
                                string qual = dot[dot.Length - 2];
                                if (qual + 'p' == radioButton6.Text)
                                    break;
                            }
                        }
                        if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false && radioButton5.Checked == false && radioButton6.Checked == false)
                            MessageBox.Show("Вы не выбрали качество");
                        else
                        { 
                            difQualCount++;
                            j = 0;
                            Uri uri = new Uri(value[0]);
                            SaveFileDialog SFD = new SaveFileDialog();
                            SFD.DefaultExt = "mp4";
                            SFD.InitialDirectory = @"D:\";
                            SFD.RestoreDirectory = true;
                            SFD.FileName = dta[CBVideoList.SelectedIndex].title;

                            if (SFD.ShowDialog() == DialogResult.OK)
                            {
                                client.DownloadFileAsync(uri, SFD.FileName);
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                ButCancel.Enabled = true;
                            }
                        }
                    }
                    else
                        MessageBox.Show("Видео доступно только для просмотра");
                }
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            if (count == 0)
            {
                this.Close();
                count++;
            }
            else
            {
                this.Show();
                butDownload.Focus();
            }
        }

        private void butView_Click_1(object sender, EventArgs e)
        {
            if (CBVideoList.CheckedItems.Count == 0)
                MessageBox.Show("Вы не выбрали видео");
            else
                Process.Start(dta[CBVideoList.SelectedIndex].link);
        }

        private void RBEnabled()
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
        }

        private void CBVideoList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (CBVideoList.CheckedItems.Count > 1)
            {
                for (int i = 0; i < CBVideoList.Items.Count; i++)
                    CBVideoList.SetItemChecked(i, false);
                CBVideoList.SetItemChecked(CBVideoList.SelectedIndex, true);
            }
            if (CBVideoList.CheckedItems.Count == 0)
            {
                RBEnabled();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Сменить пользователя?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                this.Hide();
                Form1 frm = new Form1();
                frm.ShowDialog();
            }
        }

        private void ButCancel_Click_1(object sender, EventArgs e)
        {
            client.CancelAsync();
            progressBar1.Value = 0;
            ButCancel.Enabled = false;
            k++;
            difQualCount = 0;
            MessageBox.Show("Загрузка отменена");
        }

        private void CBVideoList_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {
                butDownload.Enabled = true;
                j = 0;
                k = 0;
                difQualCount = 0;
                RBEnabled();

                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;

                string HTML = client.DownloadString(dta[CBVideoList.SelectedIndex].link);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(HTML);
                var tdList = htmlDoc.DocumentNode.SelectNodes("//source");
                string[] dot = null;
                try
                {
                    for (int i = 1; i < tdList.Count; i++)
                    {
                        if (tdList[i].Attributes["src"] != null)
                        {
                            value = tdList[i].Attributes["src"].Value.Split('?');
                            dot = value[0].Split('.');
                            string qual = dot[dot.Length - 2];

                            if (qual + 'p' == radioButton1.Text)
                                radioButton1.Enabled = true;
                            if (qual + 'p' == radioButton2.Text)
                                radioButton2.Enabled = true;
                            if (qual + 'p' == radioButton3.Text)
                                radioButton3.Enabled = true;
                            if (qual + 'p' == radioButton4.Text)
                                radioButton4.Enabled = true;
                            if (qual + 'p' == radioButton5.Text)
                                radioButton5.Enabled = true;
                            if (qual + 'p' == radioButton6.Text)
                                radioButton6.Enabled = true;
                        }
                    }
                }
                catch
                {

                }
        }

        private void Form2_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            if (sch == 0)
            {
                sch++;
            }
            else
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (k == 0 || difQualCount != 0)
            {
                progressBar1.Maximum = 100;
                progressBar1.Value = e.ProgressPercentage;
                if (progressBar1.Value == 100 && j == 0)
                {
                    j++;
                    System.Threading.Thread.Sleep(1000);
                    MessageBox.Show("Загрузка завершена");
                    ButCancel.Enabled = false;
                    progressBar1.Value = 0;
                    k++;
                    difQualCount = 0;
                }
            }
        }
    }
}
