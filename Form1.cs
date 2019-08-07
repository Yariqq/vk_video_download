using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace vk_youtube_kyrsach
{
    public struct info
    {
        public string title;
        public string link;
        public info(string title, string link)
        {
            this.title = title;
            this.link = link;
        }
    }
    public partial class Form1 : Form
    {
        VkApi vkApi = new VkApi();
        public Form1()
        {
            InitializeComponent();
        }

        private void EnterBut_Click(object sender, EventArgs e)
        {
            if (LoginCB.Text == "" && PasswordTB.Text == "")
                MessageBox.Show("Вы не ввели логин и пароль");
            else if (PasswordTB.Text == "")
                MessageBox.Show("Вы не ввели пароль");
            else if (LoginCB.Text == "")
                MessageBox.Show("Вы не ввели логин");
            else
            {
                EnterBut.Enabled = false;
                ForgetBut.Enabled = false;
                string F = System.Windows.Forms.Application.StartupPath + @"\Out_users.txt";
                string s;
                int k, j, incCount = 0;
                FileStream fFile = new FileStream(F, FileMode.Open);
                StreamReader sf = new StreamReader(fFile);
                fFile.Seek(0, SeekOrigin.Begin);
                s = sf.ReadLine();
                k = 0;
                j = 0;
                if (s != null)
                {
                    do
                    {
                        if (k == 0)
                        {
                            if (s == LoginCB.Text)
                                j++;
                            k++;
                        }
                        else
                        {
                            if (sf.ReadLine() == LoginCB.Text)
                                j++;
                        }
                    } while (!sf.EndOfStream);
                }
                sf.Close();
                fFile.Close();

                var authorize = new ApiAuthParams();
                authorize.Login = LoginCB.Text;
                authorize.Password = PasswordTB.Text;
                authorize.ApplicationId = 6899366;
                authorize.Settings = Settings.Video;

                try
                {
                    vkApi.Authorize(authorize);
                    if (RememberCB.Checked == true)
                        DataBaseInsert(LoginCB.Text, PasswordTB.Text);
                }
                catch
                {
                    MessageBox.Show("Неверный логин и/или пароль, либо нет доступа в интернет");
                    incCount++;
                    EnterBut.Enabled = true;
                }

                if (incCount == 0)
                {
                    if (j == 0)
                    {
                        FileStream aFile = new FileStream(F, FileMode.OpenOrCreate);
                        StreamWriter sw = new StreamWriter(aFile);
                        aFile.Seek(0, SeekOrigin.End);
                        sw.WriteLine(LoginCB.Text);
                        sw.Close();
                        aFile.Close();
                    }

                    var videos = vkApi.Video.Get(new VideoGetParams
                    {
                        OwnerId = vkApi.UserId
                    });

                    List<info> titles = new List<info>();

                    for (int i = 0; i < videos.Count; i++)
                    {
                        titles.Add(new info(videos[i].Title, videos[i].Player.AbsoluteUri));
                    }
                    if (titles.Count == 0)
                    {
                        DataBaseDelete(LoginCB.Text);
                        DialogResult dialogResult = MessageBox.Show("У вас нет сохраненных видеозаписей. Добавить?", "Warning", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Process.Start("https://vk.com/video");
                            EnterBut.Enabled = true;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            EnterBut.Enabled = true;
                        }
                    }
                    else
                    {
                        this.Hide();
                        Form2 frm = new Form2(titles);
                        frm.ShowDialog();
                    }
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            int i;
            string s;
            string F = System.Windows.Forms.Application.StartupPath + @"\Out_users.txt";
            FileStream aFile = new FileStream(F, FileMode.Open);
            StreamReader sw = new StreamReader(aFile);
            aFile.Seek(0, SeekOrigin.Begin);
            s = sw.ReadLine();
            i = 0;
            if (s != null)
            {
                do
                {
                    if (i == 0)
                    {
                        LoginCB.Items.Add(s);
                        i++;
                    }
                    else
                        LoginCB.Items.Add(sw.ReadLine());
                } while (!sw.EndOfStream);
            }
            sw.Close();
            aFile.Close();
        }

        public void DataBaseRead(string log)
        {
            string connect = "server=localhost;user=root;database=users;password=butterflyknife558831yaYA;";
            MySqlConnection conn = new MySqlConnection(connect);
            try
            {
                conn.Open();
                string sql = "SELECT password FROM users_data WHERE login = '" + log + "'";
                MySqlCommand command = new MySqlCommand(sql, conn);
                try
                {
                    string pass = command.ExecuteScalar().ToString();
                    PasswordTB.Text = pass;
                    RememberCB.Checked = false;
                    RememberCB.Enabled = false;

                    ForgetBut.Enabled = true;
                }
                catch
                {
                    ForgetBut.Enabled = false;
                    RememberCB.Enabled = true;
                    PasswordTB.Clear();
                }

                conn.Close();
            }
            catch
            {
                
            }
        }

        public void DataBaseInsert(string log, string pass)
        {
            string connect = "server=localhost;user=root;database=users;password=butterflyknife558831yaYA;";
            MySqlConnection conn = new MySqlConnection(connect);
            try
            {
                conn.Open();
                string sql = "INSERT INTO users_data (login, password) VALUES ('" + log + "', '" + pass + "')";
                MySqlCommand command = new MySqlCommand(sql, conn);

                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {

            }
        }

        public void DataBaseDelete(string log)
        {
            string connect = "server=localhost;user=root;database=users;password=butterflyknife558831yaYA;";
            MySqlConnection conn = new MySqlConnection(connect);
            try
            {
                conn.Open();
                string sql = "DELETE FROM `users_data` WHERE (`login` = '" + log + "')";
                MySqlCommand command = new MySqlCommand(sql, conn);

                command.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {

            }
        }

        private void LoginCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBaseRead(LoginCB.Text);
        }

        private void ForgetBut_Click(object sender, EventArgs e)
        {
            ForgetBut.Enabled = false;
            DataBaseDelete(LoginCB.Text);
            MessageBox.Show("Ваши данные успешно удалены");
        }
    }
}
