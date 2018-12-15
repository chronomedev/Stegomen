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

        public void insertDatabase(String parameter)
        {
            MySqlCommand komenMySql = new MySqlCommand("insert into mspesan(indeks_bitpixel_perkarakter, user_id_line_profile_penerima, status)values('" + parameter + "', 'dieindonesianer', 'enkrip');", koneksi);
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
