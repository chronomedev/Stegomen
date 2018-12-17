using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;

namespace CobaStegano
{
    public partial class Form1 : Form
    {
        MySqlConnection koneksi;
        kelasDatabase dbHandler;
        libraryFungsi library;
        Image loadedImage;
        Double textsize;
        String id_user_masuk;

        String id_penerima = null;
        ///////////////////Fungsi/////////////////////////////////////////////////////////////
        public Form1(String id_user)
        {
            id_user_masuk = id_user;
            String alamat = "Server=den1.mysql1.gear.host;Database=stegomendb;Uid=stegomendb;Pwd=stegomen!;";
            koneksi = new MySqlConnection(alamat);
            koneksi.Open();
            koneksi.Close();
            Console.WriteLine("koneksi sukses");
            dbHandler = new kelasDatabase(koneksi);
            library = new libraryFungsi();
            InitializeComponent();      
        }


        public String kasihPassword(String password, String pesan)
        {
            String append = password + "|" + pesan;
            return append;
        }

        public Boolean AmbilExtensionPNG(String PathImage)
        { 
            String[] pecah = PathImage.Split('.');
            int highIndex = pecah.Length - 1;
            if(pecah[highIndex].ToLower() == "png")
            {
                return true;

            } else
            {
                return false;
            }
            
        }
        public void KonvertPNG(String pathImageAsal, String pathImageDestination)
        {
        
            Bitmap gambarKonvert = new Bitmap(pathImageAsal);
            gambarKonvert.Save(pathImageDestination+"\\temp.png", ImageFormat.Png);
        }
        ////////////Events///////////////////////////////////////////////////////////////////////////////////
        

        //Import file gambar
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialokFile = new OpenFileDialog();

            dialokFile.Filter = "Image Files (*.jpg)|*.jpg|Recomended!!! (*.png)|*.png";
            if (dialokFile.ShowDialog() == DialogResult.OK)
            {
                loadedImage = Image.FromFile(dialokFile.FileName.ToString());
                if (loadedImage.Height < 100 || loadedImage.Width < 100)
                {
                    MessageBox.Show("Gambar minimal harus lebih dari 100 x 100 pixel");
                } else
                {
                    textBox1.Text = dialokFile.FileName.ToString();
                    pictureBox1.ImageLocation = textBox1.Text;
                    textsize = (8.0 * ((loadedImage.Height * (loadedImage.Width / 3) * 3) / 3 - 1)) / 1024;
                    Console.WriteLine("Textsize =" + textsize);
                    Bitmap testing_testinggambar = new Bitmap(textBox1.Text);
                    //Buat debugging consolenya
                    Console.WriteLine("Extension gambar PNG = " + AmbilExtensionPNG(textBox1.Text));
                }

            }
        }

        
        //////encrypt gambar
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox4.Text == null || textBox4.Text == "")
            {
                MessageBox.Show("Kasih ID penerima dari Line tersebut");
            } else if(textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("kasih dulu passwordnya untuk kemanan pesan!");

            } else if (textBox1.Text == "" || textBox1.Text == null)
            {

                MessageBox.Show("Tentukan Path gambar terlebih dahulu");

            } else 
            {
                Bitmap img;
                string messagetext = kasihPassword(textBox2.Text, textBox3.Text);
                //double textlength = System.Text.ASCIIEncoding.ASCII.GetByteCount(messagetext);
                Console.WriteLine("Text length normal: " + messagetext.Length);
                //Console.WriteLine("Textlength:" + textlength);
                

                if (messagetext.Length-2>loadedImage.Width)
                {
                    MessageBox.Show("Ukuran gambar terlalu besar. Silahkan perpendek teksnya");
                }
                else
                {

                    String tampung_perbit_pixel = "";
                    
                    if (AmbilExtensionPNG(textBox1.Text) == false)
                    {
                        String direktori = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        KonvertPNG(textBox1.Text, direktori);
                        img = new Bitmap(direktori+"\\temp.png");
                        //Buat Debugging 
                        Console.WriteLine("----------Berhasil Convert PNG---------");
                    } else
                    {
                        img = new Bitmap(textBox1.Text);
                        Console.WriteLine("----------TIDAK CONVERT---------");
                    }
                    
                    Console.WriteLine("Panjang textbox 2: " + textBox2.TextLength);
                    int tampung_panjang_karakter = textBox2.TextLength;

                    //Looping per karakter
                    for(int indeks_karakter = 0; indeks_karakter < messagetext.Length; indeks_karakter++)
                    {
                        char karakter = Convert.ToChar(messagetext.Substring(indeks_karakter, 1));
                        int value = Convert.ToInt32(karakter);
                        String char_binary = Convert.ToString(value, 2);
                        Console.WriteLine("VALUE CHAR = " + value+" BINER CHAR = " + char_binary);


                        //looping per 1 bit di LSB masukin pixelnya
                        for(int i = 0; i < char_binary.Length; i++)
                        {
                            Console.WriteLine("LOOP BIT ========" + i);
                            char tampung = Convert.ToChar(char_binary.Substring(i, 1));
                            Color pixel = img.GetPixel(indeks_karakter, i);
                            int pixel_B_value = pixel.B;

                            Console.WriteLine("PIXEL B = " + pixel_B_value);
                            String biner_pixel_B = Convert.ToString(pixel_B_value, 2);
                            Console.WriteLine("BINER_PIXEL = " + biner_pixel_B);
                            String ubahbit = library.ubah_bit(biner_pixel_B, tampung);

                            Console.WriteLine("UBAH BIT = " + ubahbit);
                            int newBinary_pixelB = Convert.ToInt32(ubahbit, 2);
                            Console.WriteLine(newBinary_pixelB);
                            img.SetPixel(indeks_karakter, i, Color.FromArgb(pixel.R, pixel.G, newBinary_pixelB));

                        }
                        //tampung indeks perbit di satu pixel agar nanti waktu decrypt bisa di fetch diambil length nya
                        tampung_perbit_pixel = tampung_perbit_pixel + char_binary.Length + "-";
                        
                    }

                    //insert status dan indeks bit pixel ke database serta id penerimanya 
                    dbHandler.insertDatabase(tampung_perbit_pixel, dbHandler.ambilIdUser(textBox4.Text));
                    Color pixelTerakhir = img.GetPixel(img.Width-1, img.Height-1);

                    //tampung messageID di pixel paling kanan bawah
                    int id_pesan = dbHandler.getLastMessageID();
                    Console.WriteLine("MESSAGE IDNYA DIMASUKIN = " + id_pesan);
                    img.SetPixel(img.Width-1, img.Height-1, Color.FromArgb(pixelTerakhir.R, pixelTerakhir.B, id_pesan));

                    Console.WriteLine("PANJANG PERINDEKS: " + tampung_perbit_pixel);
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "Recommended!!!! (*.png) | *.png|Common Images(*.jpg)|*.jpg";
                    saveFile.InitialDirectory = @"C:\Users\";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        textBox1.Text = saveFile.FileName.ToString();
                        pictureBox1.ImageLocation = textBox1.Text;
                        img.Save(textBox1.Text);
                    }
                    MessageBox.Show("Gambar berhasil Disimpan!");

                    //kirim notif ada yang buat pesan rahasia ke dia
                    if(library.sendToPenerima(dbHandler.ambilIdUser(textBox4.Text), dbHandler.ekstrakUserName(id_user_masuk), id_pesan) != "berhasil")
                    {
                        //MessageBox.Show("Gagal mengirim ke line penerima anda");
                    }
                    img.Dispose();
                    File.Delete(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\temp.png");
                    textBox2.Text = "";
                }
            }
        }

        //Decrypt Gambar
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text =="" || textBox1.Text == null)
            {
                MessageBox.Show("Pilih dahulu path gambarnya");
            } else if(textBox3.Text =="" || textBox3.Text == null)
            {

                MessageBox.Show("Isi passwordnya terlebih dahulu!");
                //Console.WriteLine("ISI INDEKS" + dbHandler.ekstrakIndeks());

            } else 
            {
                Bitmap img = new Bitmap(textBox1.Text);
                Color pixel_msg_ID = img.GetPixel(img.Width - 1, img.Height - 1);
                int messageID = pixel_msg_ID.B;
                String[] ekstrak_indeks_karakter = dbHandler.ekstrakIndeks(messageID).Split('-');
                Console.WriteLine(ekstrak_indeks_karakter[0]);

               
                String msg = "";
                String letter = "";
                for (int i = 0; i < ekstrak_indeks_karakter.Length-1; i++)
                {
                    String getLSBEkstrak = "";
                    String binerCharEkstrak = "";
                    int get_bit_IndexChar = Convert.ToInt32(ekstrak_indeks_karakter[i]);
                    for(int z = 0; z < get_bit_IndexChar; z++)
                    {
                        Color pixel = img.GetPixel(i, z);
                        int value_pixelB = pixel.B;
                        String binerPixel = Convert.ToString(value_pixelB, 2);
                        Console.WriteLine("BINER PIXEL DECRYPT = " + binerPixel);
                        Console.WriteLine("----" + get_bit_IndexChar);
                        //Console.WriteLine("---------" + binerPixel.Substring(getIndexChar - 1, 1));
                        //ambil 1 bit warna blue tiap pixel ke bawah
                        getLSBEkstrak = getLSBEkstrak + binerPixel.Substring(binerPixel.Length-1, 1);

                    }
                    int konvertValue = Convert.ToInt32(getLSBEkstrak, 2);
                    Console.WriteLine("VALUE CHAR EKSTRAK " + konvertValue);
                    char convertExtract = Convert.ToChar(konvertValue);
                    letter = letter + convertExtract.ToString();

                    Console.WriteLine("VALUE DI CONVERT DECRYPT = " + letter);

                    Console.WriteLine("Looping: " + i + " ================" + getLSBEkstrak);
                }

                Color msgID = img.GetPixel(img.Width - 1, img.Height - 1);
                int id_pesan = msgID.B;
                Console.WriteLine("ID PESAN = " + msgID);
                Console.WriteLine("MESSAGE = " + letter);


                //matching dengan password
                String[] pecah = letter.Split('|');

                if (pecah[pecah.Length - 1] == textBox3.Text)
                {
                    dbHandler.updateStatusMsg(id_pesan);
                    textBox2.Text = library.displayMessage(pecah);
                    MessageBox.Show("Pesan berhasil di decode!");
                    Console.WriteLine("ID USER MASUK====" + dbHandler.ekstrakUserName(id_user_masuk));
                    if (library.notifBuka(dbHandler.ekstrakUserName(id_user_masuk),id_pesan, dbHandler.ambilIdPenerimaByPesan(id_pesan)) != "berhasil")
                    {
                        //MessageBox.Show("Gagal mengirim ke line penerima anda");
                    }

                } else
                {
                    MessageBox.Show("Password tidak Sesuai!");
                }
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            String all_chiper = "";
            String[] pecahSpasi = textBox3.Text.Split(' ');
            for(int i= 0; i < pecahSpasi.Length; i++)
            {
                all_chiper = all_chiper + library.fungsiChiperText(pecahSpasi[i]) + " ";
            }
            MessageBox.Show("Hasil: " + all_chiper);
            Console.WriteLine(all_chiper);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            id_penerima = dbHandler.ambilIdUser(textBox4.Text);
            if(id_penerima == null || id_penerima == "")
            {
                MessageBox.Show("ID penerima belum terdaftar di bot Stegomen");
            } else
            {
                //textBox4.Text = id_penerima;
                MessageBox.Show("User tersebut terdaftar!");
            }
        }
    }
    }

