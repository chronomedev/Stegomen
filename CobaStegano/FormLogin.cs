using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CobaStegano
{
    public partial class FormLogin : Form
    {

        MySqlConnection koneksi;
        kelasDatabase dbHandler;
        Form1 login;
        public FormLogin()
        {
            InitializeComponent();
            koneksi = new MySqlConnection("Server=den1.mysql1.gear.host;Database=stegomendb;Uid=stegomendb;Pwd=stegomen!;");
            dbHandler = new kelasDatabase(koneksi);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String id_user_masuk = dbHandler.ambilIdUser(textBox1.Text);
            if(id_user_masuk == "" || id_user_masuk == null)
            {
                MessageBox.Show("ID line tidak ditemukan. Bila belum ada, daftar ID anda di bot Stegomen Line");
            } else
            {
                login = new Form1(id_user_masuk);
                this.Hide();
                if (login.ShowDialog() == DialogResult.None)
                {
                    this.Show();
                }

            }
        }
    }
}
