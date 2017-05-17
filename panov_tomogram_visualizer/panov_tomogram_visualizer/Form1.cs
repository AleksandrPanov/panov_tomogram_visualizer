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
        Bin bin;
        View view;
        Cube cube;
        bool isLoaded = false;
        bool needReload = false;
        bool quads = true;
        bool isCube = false;
        int curLayer = 0;
        int FrameCount;
        int xRot = 0;
        int yRot = 0;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps = {0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }
        public Form1()
        {
            InitializeComponent();
            bin = new Bin();
            view = new View();
            cube = new Cube();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                string s = d.FileName;
                bin.readBIN(s);
                view.SetupView(glControl1.Width, glControl1.Height);
                isLoaded = true;
                glControl1.Invalidate();
                trackBar1.Maximum = Bin.Z - 1;
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                if (quads)
                    view.DrawQuads(curLayer);               
                else if (isCube)
                {
                    GL.Rotate(yRot, 0.0, 1.0, 0.0);
                    GL.Rotate(xRot, 1.0, 0.0, 0.0);
                    cube.DrawCube();                  
                   // yRot = 0;
                  //  xRot = 0;
                }
                else
                { 
                    if (needReload)
                    {
                        view.generateTextureImage(curLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            curLayer = trackBar1.Value;
            needReload = true;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

       void glControl1_Load(object sender, EventArgs e)
        {
               
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void четырёхугольникамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quads = true;
        }

        private void текстуройToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quads = false;
            needReload = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            needReload = true;         
            view.min = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            needReload = true;
            view.width = trackBar3.Value;
        }

        private void нарисоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isCube = true;
            quads = false;
            needReload = false;
        }

        private void скрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isCube = false;
            quads = true;
            needReload = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xRot = (xRot + 45) % 360;
        }
    }
}
