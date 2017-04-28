using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace panov_tomogram_visualizer
{
    class View
    {
        int VBOtexture;
        Bitmap textureImage;
        public void Load2DTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            System.Drawing.Imaging.BitmapData data = textureImage.LockBits(
                new Rectangle(0,0,textureImage.Width, textureImage.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);

            textureImage.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            ErrorCode Er = GL.GetError();
            string str = Er.ToString();
        }
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
        public void generateTextureImage(int layerNum)
        {
            textureImage = new Bitmap(Bin.X, Bin.Y);
            for (int i = 0; i < Bin.X; ++i)
                for (int j = 0; j < Bin.Y; ++j)
                {
                    int pixNum = i + j * Bin.X + layerNum * Bin.X * Bin.Y;
                    textureImage.SetPixel(i, j, TransferFunction(Bin.ar[pixNum])); 
                }
        }
        public void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.White);

            GL.TexCoord2(0f, 0f);
            GL.Vertex2(0, 0);

            GL.TexCoord2(0f, 1f);
            GL.Vertex2(0, Bin.Y);

            GL.TexCoord2(1f, 1f);
            GL.Vertex2(Bin.X, Bin.Y);

            GL.TexCoord2(1f, 0f);
            GL.Vertex2(Bin.X, 0);

            GL.End();

            GL.Disable(EnableCap.Texture2D);
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