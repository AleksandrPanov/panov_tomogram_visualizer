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
    class Cube
    {
        double [,]cubeVertexArray = new double[8,3];
        int[,] cubeIndexArray = new int[6, 4];
        int [] VBOtexture = new int[6];//коды текстуры
        int[,] coord = new int[6,2];
        double dx = 50;
        double dy = 50;
        double size = 250;//размер кубмка

        public int min = 0;
        public int width = 2000;

        Bitmap []textureImage = new Bitmap[4];
        public Cube()
        {
            {
                cubeVertexArray[0, 0] = 0.0 + dx;
                cubeVertexArray[0, 1] = 0.0 + dy;
                cubeVertexArray[0, 2] = size;

                cubeVertexArray[1, 0] = 0.0 + dx;
                cubeVertexArray[1, 1] = size + dy;
                cubeVertexArray[1, 2] = size;

                cubeVertexArray[2, 0] = size + dx;
                cubeVertexArray[2, 1] = size + dy;
                cubeVertexArray[2, 2] = size;

                cubeVertexArray[3, 0] = size + dx;
                cubeVertexArray[3, 1] = 0.0 + dy;
                cubeVertexArray[3, 2] = size;

                cubeVertexArray[4, 0] = 0.0 + dx;
                cubeVertexArray[4, 1] = 0.0 + dy;
                cubeVertexArray[4, 2] = 0.0;

                cubeVertexArray[5, 0] = 0.0 + dx;
                cubeVertexArray[5, 1] = size + dy;
                cubeVertexArray[5, 2] = 0.0;

                cubeVertexArray[6, 0] = size + dx;
                cubeVertexArray[6, 1] = size + dy;
                cubeVertexArray[6, 2] = 0.0;

                cubeVertexArray[7, 0] = size + dx;
                cubeVertexArray[7, 1] = 0.0 + dy;
                cubeVertexArray[7, 2] = 0.0;
            }
            {
                cubeIndexArray[0,0] = 0;
                cubeIndexArray[0,1] = 3;
                cubeIndexArray[0,2] = 2;
                cubeIndexArray[0,3] = 1;

                cubeIndexArray[1,0] = 0;
                cubeIndexArray[1,1] = 1;
                cubeIndexArray[1,2] = 5;
                cubeIndexArray[1,3] = 4;

                cubeIndexArray[2,0] = 7;
                cubeIndexArray[2,1] = 4;
                cubeIndexArray[2,2] = 5;
                cubeIndexArray[2,3] = 6;

                cubeIndexArray[3,0] = 3;
                cubeIndexArray[3,1] = 7;
                cubeIndexArray[3,2] = 6;
                cubeIndexArray[3,3] = 2;

                cubeIndexArray[4,0] = 1;
                cubeIndexArray[4,1] = 2;
                cubeIndexArray[4,2] = 6;
                cubeIndexArray[4,3] = 5;

                cubeIndexArray[5,0] = 0;
                cubeIndexArray[5,1] = 4;
                cubeIndexArray[5,2] = 7;
                cubeIndexArray[5,3] = 3;
            }
        }
        public void DrawCube()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Color[] col = new Color[3];
            col[0] = Color.White;
            col[1] = Color.Red;
            col[2] = Color.Yellow;

           for (int i = 0; i < 6; i++)
            {
               GL.Begin(BeginMode.Quads);

                int k = cubeIndexArray[i, 0];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                GL.Color3(col[i % 3]);

                k = cubeIndexArray[i, 1];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                GL.Color3(col[i % 3]);

                k = cubeIndexArray[i, 2];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                GL.Color3(col[i % 3]);

                k = cubeIndexArray[i, 3];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                GL.Color3(col[i % 3]);

                /* for (int j = 0; j < 4; j++)
                 {
                     int k = cubeIndexArray[i, j];
                     GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                     GL.Color3(col[i % 3]);               
                 }*/
                GL.End();
            }
        }
     /*   public void Load2DTextures()
        {
            GL.GenTextures(6, VBOtexture);

            //делаем для 1 грани
             GL.BindTexture(TextureTarget.Texture2D, VBOtexture[0]);
            System.Drawing.Imaging.BitmapData data = textureImage[0].LockBits(
                new Rectangle(0, 0, (int)size, (int)size),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);

            textureImage[0].UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            //делаем для 2 грани...


            ErrorCode Er = GL.GetError();
            string str = Er.ToString();
        }
        public void SetupView(int width, int height)
        {

            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
            GL.Viewport(0, 0, width, height);
        }
     /*   public void DrawQuads(int layerNum)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);

            for (int x = 0; x < Bin.X - 1; x++)
                for (int y = 0; y < Bin.Y - 1; y++)
                {
                    short val;
                    //1st vertex
                    val = Bin.ar[x + y * Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x, y);

                    //2nd vertex
                    val = Bin.ar[x + (y + 1) * Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x, y + 1);

                    //3th vertex
                    val = Bin.ar[x + 1 + (1 + y) * Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x + 1, y + 1);

                    //4th vertex
                    val = Bin.ar[x + 1 + y * Bin.X + layerNum * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(val));
                    GL.Vertex2(x + 1, y);
                }

            GL.End();
        } */
       /* public void generateTextureImage(int layerNum)
        {
            for (int i = 0; i < 6; i++)
            textureImage[i] = new Bitmap((int)size, (int)size);

            //0 грань
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                {
                    int pixNum = i + coord[0,0]  + (j + coord[0,1]) * Bin.X + layerNum * Bin.X * Bin.Y;
                    textureImage[0].SetPixel(i, j, TransferFunction(Bin.ar[pixNum]));
                }
        }
        public void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture[0]);

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
            int max = min + width;
            int newVal = clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }*/
    }
}
