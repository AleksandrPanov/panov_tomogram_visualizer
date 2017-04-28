using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace panov_tomogram_visualizer
{
    class View
    {
        public void SetupView(int width, int height)
        {

              GL.ShadeModel(ShadingModel.Smooth);
              GL.MatrixMode(MatrixMode.Projection);
              GL.LoadIdentity();
              GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
              GL.Viewport(0,0,width, height); 
        }
        public void DrawQuads(int layerNum)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);

            for (int x = 0; x < Bin.X - 1; x++)
                for(int y = 0; y < Bin.Y - 1; y++)
                {
                    short val;
                    //1st vertex
                    val = Bin.ar[x + y * Bin.X + layerNum * Bin.X*Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x, y);

                    //2nd vertex
                    val = Bin.ar[x + (y + 1)* Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x, y + 1);

                    //3th vertex
                    val = Bin.ar[x + 1 +(1 + y) * Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x + 1, y + 1);

                    //4th vertex
                    val = Bin.ar[x + 1 + y * Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x + 1, y);
                }

            GL.End();
        }
        public int clamp(int value, int min, int max) // clamp = сцеплять
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public Color TransferFunction(short value)
        {
            int min = 0;
            int max = 2000;
            int newVal = clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }     
    }
}