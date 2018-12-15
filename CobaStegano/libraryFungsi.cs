using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CobaStegano
{
    class libraryFungsi
    {


        public String ubah_bit(String biner_pixel, char bit_ambil)
        {
            String newBinary = "";
            for (int i = 0; i < biner_pixel.Length; i++)
            {
                if (i == biner_pixel.Length - 1)
                {
                    newBinary = newBinary + bit_ambil;

                }
                else
                {
                    newBinary = newBinary + biner_pixel.Substring(i, 1);
                }
            }

            return newBinary;
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
            String pesan_chiper = "";
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
                    if (tampung == alfabet[z])
                    {
                        indeks_alfabet = z;
                        break;
                    }

                }
                if (indeks_alfabet != (-1))
                {
                    pesan_chiper = pesan_chiper + karakterSwitch[indeks_alfabet];
                }
                else
                {
                    pesan_chiper = pesan_chiper + tampung;
                }
                indeks_alfabet = -1;
            }

            return pesan_chiper;
        }

        public String displayMessage(String[] letter)
        {
            String pesan = "";
            for(int i = 0; i < letter.Length - 1; i++)
            {
                pesan = pesan + letter[i];
            }
            return pesan;
            
        }
    }
}
