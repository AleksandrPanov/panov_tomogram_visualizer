using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panov_tomogram_visualizer
{
    class Bin
    {
        public static int X, Y, Z;
        public static short[] ar;
        public Bin() { }

        public void readBIN(string path)
        {
            if (File.Exists(path))
            {
                BinaryReader r =
                    new BinaryReader(File.Open(path, FileMode.Open));

                X = r.ReadInt32();
                Y = r.ReadInt32();
                Z = r.ReadInt32();

                int arSzie = X * Y * Z;
                ar = new short[arSzie];
                for (int i = 0; i < arSzie; i++)
                    ar[i] = r.ReadInt16();
            }
        }
    }
}
