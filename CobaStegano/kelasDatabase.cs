using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
using System.Threading.Tasks;

/// <summary>
/// Kelas untuk Menghandle database
/// </summary>
namespace CobaStegano
{
    class kelasDatabase
    {
        private MySqlConnection koneksi;
        

        public kelasDatabase(MySqlConnection koneksi_parameter)
        {
            koneksi = koneksi_parameter;
        }


        public String ambilIdPenerimaByPesan(int id_pesan)
        {
            String id_penerima = "";
            MySqlCommand komenSql = new MySqlCommand("select user_id_line_profile_penerima from mspesan where id_pesan = " + id_pesan + ";", koneksi);
            koneksi.Open();
            MySqlDataReader baca = komenSql.ExecuteReader();
            while (baca.Read())
            {
                id_penerima = baca["user_id_line_profile_penerima"].ToString();
            }
            koneksi.Close();
            return id_penerima;
        }

        public String ekstrakUserName(String id_user_pengirim)
        {
            String displayAmbil = null;
            MySqlCommand komenSQL = new MySqlCommand("select nama_user from msuser where user_id ='" + id_user_pengirim +"';",  koneksi);
            koneksi.Open();
            MySqlDataReader baca = komenSQL.ExecuteReader();
            while (baca.Read())
            {
                displayAmbil = baca["nama_user"].ToString();
            }
            koneksi.Close();
            return displayAmbil;
        }

        public String ekstrakIndeks(int id_pesan)
        {
            String ekstrak = "";
            MySqlCommand komenMySql = new MySqlCommand("select indeks_bitpixel_perkarakter from mspesan where id_pesan ="+id_pesan+";", koneksi);
            koneksi.Open();
            MySqlDataReader baca = komenMySql.ExecuteReader();
            while (baca.Read())
            {
                ekstrak = (baca["indeks_bitpixel_perkarakter"].ToString());
            }
            koneksi.Close();
            return ekstrak;
        }

        // public String cekStatusMsg(int id_pesan)
        //    {
        //        String status = "";
        //        MySqlCommand komenMySql = new MySqlCommand("select status from mspesan where id_pesan =" + id_pesan + ";", koneksi);
        //        koneksi.Open();
        //        MySqlDataReader baca = komenMySql.ExecuteReader();
        //        while (baca.Read())
        //        {
        //            status = baca["status"].ToString();
        //        }

        //        return status;
        //    }


        public void updateStatusMsg(int id_pesan)
        {
            MySqlCommand komenMySql = new MySqlCommand("update mspesan set status = 'dibaca' where id_pesan =" + id_pesan + ";", koneksi);
            koneksi.Open();
            komenMySql.ExecuteNonQuery();
            koneksi.Close();
        }


        //Ambil user id buat login
        public String ambilIdUser(String id_line)
        {
            String id_database_line = null;
            MySqlCommand komenSQL = new MySqlCommand("select user_id from msuser where user_id_line_profile ='" + id_line + "';", koneksi);
            koneksi.Open();
            MySqlDataReader baca = komenSQL.ExecuteReader();
            while (baca.Read()) {
                id_database_line = baca["user_id"].ToString();

            }
            koneksi.Close();
            return id_database_line;
        }

        public void insertDatabase(String parameter, String id_user_penerima)
        {
            MySqlCommand komenMySql = new MySqlCommand("insert into mspesan(indeks_bitpixel_perkarakter, user_id_line_profile_penerima, status)values('" + parameter + "','"+ id_user_penerima+ "', 'enkrip');", koneksi);
            //komenMySql.CommandTimeout = 60;
            koneksi.Open();
            komenMySql.ExecuteNonQuery();
            koneksi.Close();
        }

        public int getLastMessageID()
        {
            String ekstrak = null;
            MySqlCommand komenMySql = new MySqlCommand("select id_pesan from mspesan order by id_pesan desc LIMIT 1;", koneksi);
            koneksi.Open();
            MySqlDataReader baca = komenMySql.ExecuteReader();
            while (baca.Read())
            {
                ekstrak = baca["id_pesan"].ToString();
            }
            int ekstrak_no = Convert.ToInt32(ekstrak);
            koneksi.Close();
            return ekstrak_no;
        }
    }
}
