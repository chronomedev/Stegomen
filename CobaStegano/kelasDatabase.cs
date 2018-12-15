using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

/// <summary>
/// Kelas untuk Menghandle database
/// </summary>
namespace CobaStegano
{
    class kelasDatabase
    {
        private SqlConnection koneksi;
        public kelasDatabase(SqlConnection koneksi_parameter)
        {
            koneksi = koneksi_parameter;
        }

        public String ekstrakIndeks(int id_pesan)
        {
            String ekstrak = "";
            SqlCommand komenSQL = new SqlCommand("select indeks_bitpixel_perkarakter from mspesan where id_pesan ="+id_pesan+";", koneksi);
            koneksi.Open();
            SqlDataReader baca = komenSQL.ExecuteReader();
            while (baca.Read())
            {
                ekstrak = (baca["indeks_bitpixel_perkarakter"].ToString());
            }
            koneksi.Close();
            return ekstrak;
        }

        public void insertDatabase(String parameter)
        {
            SqlCommand komenSQL = new SqlCommand("insert into mspesan(indeks_bitpixel_perkarakter, penerima)values('" + parameter + "', 'ehhh');", koneksi);
            koneksi.Open();
            komenSQL.ExecuteNonQuery();
            koneksi.Close();
        }

        public int getLastMessageID()
        {
            String ekstrak = null;
            SqlCommand komenSQL = new SqlCommand("select top 1 id_pesan from mspesan order by id_pesan desc;", koneksi);
            koneksi.Open();
            SqlDataReader baca = komenSQL.ExecuteReader();
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
