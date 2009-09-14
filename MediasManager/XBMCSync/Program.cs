using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBMCSync
{
    class Program
    {
        static void Main(string[] args)
        {

             Console.WriteLine(Hash(@"D:\Perso\Films2\Les 4 Fantastiques et le Surfer d'Argent\Les 4 Fantastiques et le Surfer d'Argent.tbn".ToLower()));
             Console.WriteLine(Hash(@"D:\Perso\Films2\Les 4 Fantastiques et le Surfer d'Argent\Les 4 Fantastiques et le Surfer d'Argent.avi".ToLower()));
             Console.WriteLine(Hash(@"D:\Perso\Films2\Les 4 Fantastiques et le Surfer d'Argent\Les 4 Fantastiques et le Surfer d'Argent".ToLower()));
             Console.WriteLine(Hash(@"Les 4 Fantastiques et le Surfer d'Argent.avi".ToLower()));
             Console.WriteLine(Hash(@"Les 4 Fantastiques et le Surfer d'Argent.tbn"));
 
    

            Console.ReadLine();
        }

        public static string Hash(string input)
        {
            byte[] bytes;
            uint m_crc = 0xffffffff;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            bytes = encoding.GetBytes(input.ToLower());
            foreach (byte myByte in bytes)
            {
                m_crc ^= ((uint)(myByte) << 24);
                for (int i = 0; i < 8; i++)
                {
                    if ((System.Convert.ToUInt32(m_crc) & 0x80000000) == 0x80000000)
                    {
                        m_crc = (m_crc << 1) ^ 0x04C11DB7;
                    }
                    else
                    {
                        m_crc <<= 1;
                    }
                }
            }
            return String.Format("{0:x8}", m_crc);
        }
    }
}
