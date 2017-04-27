using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace panov_tomogram_visualizer
{
    public partial class Form1 : Form
    {
        Bin b = new Bin();
        public Form1()
        {
            InitializeComponent();
            b.readBIN("E:\\projects\\panov_tomogram_visualizer\\testdata.bin");
        }
    }
}
