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
        int[] VBOtexture = {0,1,2,3,4,5,6};//коды текстуры
        int[,] coord = new int[6,2];
        int dx = 106;
        int dy = 145;
        int dz = 1;
        int size = 150;//размер кубика
        int sizeZ = 50;

        public int min = 0;
        public int width = 1;

        Bitmap []textureImage = new Bitmap[6];
        public Cube()
        {
            textureImage[0] = new Bitmap(size, size);
            textureImage[1] = new Bitmap(sizeZ, size);
            textureImage[2] = new Bitmap(size, size);
            textureImage[3] = new Bitmap(sizeZ, size);
            textureImage[4] = new Bitmap(sizeZ, size);
            textureImage[5] = new Bitmap(sizeZ, size);
           // for (int i = 0; i < 6; i++)
             //   textureImage[i] = new Bitmap(size,size);
            {
                cubeVertexArray[0, 0] = 0.0;
                cubeVertexArray[0, 1] = 0.0;
                cubeVertexArray[0, 2] = sizeZ;

                cubeVertexArray[1, 0] = 0.0;
                cubeVertexArray[1, 1] = size;
                cubeVertexArray[1, 2] = sizeZ;

                cubeVertexArray[2, 0] = size;
                cubeVertexArray[2, 1] = size;
                cubeVertexArray[2, 2] = sizeZ;

                cubeVertexArray[3, 0] = size;
                cubeVertexArray[3, 1] = 0.0;
                cubeVertexArray[3, 2] = sizeZ;

                cubeVertexArray[4, 0] = 0.0 ;
                cubeVertexArray[4, 1] = 0.0 ;
                cubeVertexArray[4, 2] = 0.0 ;

                cubeVertexArray[5, 0] = 0.0;
                cubeVertexArray[5, 1] = size;
                cubeVertexArray[5, 2] = 0.0;

                cubeVertexArray[6, 0] = size;
                cubeVertexArray[6, 1] = size;
                cubeVertexArray[6, 2] = 0.0;

                cubeVertexArray[7, 0] = size;
                cubeVertexArray[7, 1] = 0.0;
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

                cubeIndexArray[2,0] = 4;
                cubeIndexArray[2,1] = 7;
                cubeIndexArray[2,2] = 6;
                cubeIndexArray[2,3] = 5;

                cubeIndexArray[3,0] = 7;
                cubeIndexArray[3,1] = 3;
                cubeIndexArray[3,2] = 2;
                cubeIndexArray[3,3] = 6;

                cubeIndexArray[4,0] = 1;
                cubeIndexArray[4,1] = 2;
                cubeIndexArray[4,2] = 6;
                cubeIndexArray[4,3] = 5;

                cubeIndexArray[5,0] = 0;
                cubeIndexArray[5,1] = 3;
                cubeIndexArray[5,2] = 7;
                cubeIndexArray[5, 3] = 4;
            }     
        }
        public void GenerateFaceCube()
        {
            //1грань
            int count = 0;
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    int pixNum = i + dx + (j + dy) * Bin.X + (dz + sizeZ) * Bin.X * Bin.Y;
                    Color n = TransferFunction(Bin.ar[pixNum]);
                    if (n.R != 0)
                        count++;
                    textureImage[0].SetPixel(i, j, n);
                }
            }
            //2грань
            for (int z = 0; z < sizeZ; z++)
            {
                    for (int j = 0; j < size; ++j)
                    {
                        int pixNum = dx + (j + dy) * Bin.X + (dz + z) * Bin.X * Bin.Y;
                        textureImage[1].SetPixel(z, j, TransferFunction(Bin.ar[pixNum]));
                    }
            }
            //3грань
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    int pixNum = i + dx + (j + dy) * Bin.X + (dz) * Bin.X * Bin.Y;
                    textureImage[2].SetPixel(i, j, TransferFunction(Bin.ar[pixNum]));
                }
            }

            //4грань
            for (int z = 0; z < sizeZ; z++)
            {
                for (int j = 0; j < size; ++j)
                {
                    int pixNum = dx + size + (j + dy) * Bin.X + (dz + z) * Bin.X * Bin.Y;
                    textureImage[3].SetPixel(z, j, TransferFunction(Bin.ar[pixNum]));
                }
            }
            //5грань
            for (int z = 0; z < sizeZ; z++)
            {
                for (int i = 0; i < size; ++i)
                {
                    int pixNum = dx + i + (size + dy) * Bin.X + (dz + z) * Bin.X * Bin.Y;
                    textureImage[4].SetPixel(z, i, TransferFunction(Bin.ar[pixNum]));
                }
            }
            //6грань
            for (int z = 0; z < sizeZ; z++)
            {
                for (int i = 0; i < size; ++i)
                {
                    int pixNum = dx + i + (dy) * Bin.X + (dz + z) * Bin.X * Bin.Y;
                    textureImage[5].SetPixel(z, i, TransferFunction(Bin.ar[pixNum]));
                }
            }
        }
        public void LoadTex()
        {
            for (int i = 0; i < 6; i++)
            {
                GL.BindTexture(TextureTarget.Texture2D, VBOtexture[i]);
                System.Drawing.Imaging.BitmapData data = textureImage[i].LockBits(
                    new Rectangle(0, 0, textureImage[i].Width, textureImage[i].Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, data.Scan0);

                textureImage[i].UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);

                ErrorCode Er = GL.GetError();
                string str = Er.ToString();
            }
}
        public void DrawTexturesCube()
        {           
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            for (int i = 0; i < 6; i++)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, VBOtexture[i]);
                GL.Begin(BeginMode.Quads);
                GL.Color3(Color.White);

                GL.TexCoord2(0f, 0f);
                int k = cubeIndexArray[i, 0];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);

                GL.TexCoord2(0f, 1f);
                k = cubeIndexArray[i, 1];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);

                GL.TexCoord2(1f, 1f);
                k = cubeIndexArray[i, 2];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);

                GL.TexCoord2(1f, 0f);
                k = cubeIndexArray[i, 3];
                GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);

                GL.End();
                GL.Disable(EnableCap.Texture2D);
            }

            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 2*size, 0);
            GL.Vertex3(2 * size, 2 * size, 0);
            GL.Vertex3(2 * size, 0, 0);
            GL.End();


            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(2 * size, 0, 0);
            GL.Vertex3( 2*size, 0, 2 * size);
            GL.Vertex3( 2*size,  2*size,  2*size);
            GL.Vertex3( 2*size,  2*size, 0);
            GL.End();


            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0,  2*size);
            GL.Vertex3( 2*size, 0,  2*size);
            GL.Vertex3( 2*size,  2*size,  2*size);
            GL.Vertex3(0,  2*size,  2*size);
            GL.End();


            GL.Begin(BeginMode.LineLoop);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0,  2*size);
            GL.Vertex3(0,  2*size,  2*size);
            GL.Vertex3(0,  2*size, 0);
            GL.End();

            GL.Color3(Color.Black);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(60, 0, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 60, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 60);
            GL.End();

        }
        public void DrawCube()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Color[] col = new Color[6];
            col[0] = Color.White;
            col[1] = Color.Red;
            col[2] = Color.Yellow;
            col[3] = Color.Green;
            col[4] = Color.Blue;
            col[5] = Color.Orange;            
            for (int i = 0; i < 6; i++)
             {
                GL.Begin(BeginMode.Quads);
                 int k = cubeIndexArray[i, 0];
                 GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                 GL.Color3(col[i]);

                 k = cubeIndexArray[i, 1];
                 GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                 GL.Color3(col[i]);

                 k = cubeIndexArray[i, 2];
                 GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                 GL.Color3(col[i]);

                 k = cubeIndexArray[i, 3];
                 GL.Vertex3(cubeVertexArray[k, 0], cubeVertexArray[k, 1], cubeVertexArray[k, 2]);
                 GL.Color3(col[i]);
                GL.End();
            }
           
        }
        public Color TransferFunction(short value)
        {
            int max = min + width;
            int newVal = clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }
        public int clamp(int value, int min, int max) // clamp = сцеплять
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
