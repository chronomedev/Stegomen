using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobaStegano
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        Image loadedImage;
        
        
        Double textsize;

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
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialokFile = new OpenFileDialog();

            dialokFile.Filter = "Image Files (*.jpg)|*.jpg|Recomended!!! (*.png)|*.png";
            if (dialokFile.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialokFile.FileName.ToString();
                pictureBox1.ImageLocation = textBox1.Text;
                loadedImage = Image.FromFile(textBox1.Text);
                //loadedTrueBitmap = new Bitmap(loadedImage);
                //DecryptedImage = Image.FromFile(textBox1.Text);
                //DecryptedBitmap = new Bitmap(DecryptedImage);
                textsize = (8.0 * ((loadedImage.Height * (loadedImage.Width / 3) * 3) / 3 - 1)) / 1024;
                Console.WriteLine("Textsize =" + textsize);
                Bitmap testing_testinggambar = new Bitmap(textBox1.Text);

                //Buat debugging consolenya
                Console.WriteLine("Extension gambar PNG = " + AmbilExtensionPNG(textBox1.Text));

                //Color pixel = testing_testinggambar.GetPixel();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("kasih dulu passwordnya untuk kemanan pesan!");

            } else if (textBox1.Text == "" || textBox1.Text == null)
            {

                MessageBox.Show("Tentukan Path gambar terlebih dahulu");

            } else 
            {
                string messagetext = kasihPassword(textBox2.Text, textBox3.Text);
                double textlength = System.Text.ASCIIEncoding.ASCII.GetByteCount(messagetext);
                Console.WriteLine("Textlength:" + textlength);
                double textlen = textlength / 1024;

                if (textsize < textlen)
                {
                    MessageBox.Show("Image cannot save text more than" + textsize + "KB");
                }
                else
                {
                    Bitmap img;
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
                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            Color pixel = img.GetPixel(i, j);
                            //Console.WriteLine("value pixel:" + pixel);
                            if (i < 1 && j < textBox2.TextLength)
                            {
                                Console.WriteLine("R= [" + i + "][" + j + "]=" + pixel.R);
                                Console.WriteLine("G= [" + i + "][" + j + "]=" + pixel.G);
                                Console.WriteLine("B= [" + i + "][" + j + "]=" + pixel.B);
                                char letter = Convert.ToChar(textBox2.Text.Substring(j, 1));
                                int value = Convert.ToInt32(letter);
                                Console.WriteLine("letter :" + letter + " value :" + value);
                                img.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, value));

                            }
                            if (i == img.Width - 1 && j == img.Height - 1)
                            {
                                img.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, textBox2.TextLength));
                            }

                        }

                    }
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
                    img.Dispose();
                    File.Delete(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\temp.png");
                    textBox2.Text = "";
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text =="" || textBox1.Text == null)
            {
                MessageBox.Show("Pilih dahulu path gambarnya");
            } else if(textBox3.Text =="" || textBox3.Text == null)
            {

                MessageBox.Show("Isi passwordnya terlebih dahulu!");

            } else 
            {
                Bitmap img = new Bitmap(textBox1.Text);
                String msg = "";
                Color lastpixel = img.GetPixel(img.Width - 1, img.Height - 1);
                int msglength = lastpixel.B;
                for(int i = 0; i < img.Width; i++)
                {
                    for(int z = 0; z< img.Height; z++)
                    {
                        Color pixel = img.GetPixel(i, z);

                        if (i < 1 && z < msglength)
                        {
                            Console.WriteLine("proses decode");
                            int value = pixel.B;
                            char c = Convert.ToChar(value);
                            String letter = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(c) });
                            Console.WriteLine("letter: " + letter + "Value : " + value);
                            msg = msg + letter;
                            textBox2.Text = msg;
                        }
                    }
                }
                if(textBox2.Text == "")
                {
                    MessageBox.Show("Gak ada pesan disampaikan");
                } else
                {
                    MessageBox.Show("Berhasil!");
                }
            }
        }

        public String fungsiUnchiper(String pesan_masukan)
        {
            int indeks_alfabet = -1;
            String pesan_nonchiper = "";
            char[] alfabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z'};

            char[] karakterSwitch = { 'z', 'y', 'x', 'w', 'v', 'u', 't', 's', 'r', 'q',
                'p', 'o', 'n', 'm', 'l', 'k', 'j', 'i', 'h', 'g',
                'f', 'e', 'd', 'c', 'b', 'a' };
            for (int i = 0; i < pesan_masukan.Length; i++)
            {

                char tampung = Convert.ToChar(pesan_masukan.Substring(i, 1));
                for (int z = 0; z < alfabet.Length; z++)
                {
                    Console.WriteLine("Panjang pesan_masukan: " + pesan_masukan.Length);
                    if (tampung == karakterSwitch[z])
                    {
                        indeks_alfabet = z;
                        break;
                    }

                }
                if (indeks_alfabet != (-1))
                {
                    pesan_nonchiper = pesan_nonchiper + alfabet[indeks_alfabet];
                }
                else
                {
                    pesan_nonchiper = pesan_nonchiper + tampung;
                }
                indeks_alfabet = -1;
            }
            return pesan_nonchiper;
        }


        public String fungsiChiperText(String pesan_masukan)
        {
            int indeks_alfabet = -1;
            String pesan_chiper ="";
            char[] alfabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z'};

            char[] karakterSwitch = { 'z', 'y', 'x', 'w', 'v', 'u', 't', 's', 'r', 'q',
                'p', 'o', 'n', 'm', 'l', 'k', 'j', 'i', 'h', 'g',
                'f', 'e', 'd', 'c', 'b', 'a' };
            for(int i = 0; i < pesan_masukan.Length; i++)
            {
                
                char tampung = Convert.ToChar(pesan_masukan.Substring(i, 1));
                for(int z = 0; z<alfabet.Length; z++)
                {
                    Console.WriteLine("Panjang pesan_masukan: " + pesan_masukan.Length);
                    if (tampung == alfabet[z])
                    {
                        indeks_alfabet = z;
                        break;
                    }
                   
                }
                if(indeks_alfabet != (-1))
                {
                    pesan_chiper = pesan_chiper + karakterSwitch[indeks_alfabet];
                } else
                {
                    pesan_chiper = pesan_chiper + tampung;
                }
                indeks_alfabet = -1;
            }
            
            return pesan_chiper;

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
                all_chiper = all_chiper + fungsiChiperText(pecahSpasi[i]) + " ";
            }
            MessageBox.Show("Hasil: " + all_chiper);
            Console.WriteLine(all_chiper);
        }
    }
    }

