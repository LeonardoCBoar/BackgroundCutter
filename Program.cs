using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;


namespace BackgroundCutter
{
    class Program
    {
        static void Main(string[] args)
        {
            

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //itera pelo diretório de entrada, executando o algoritimo em todas as imagens la
            string [] images = System.IO.Directory.GetFiles("Input\\");
            foreach (var path in images)
            {
                Bitmap image = new Bitmap(path);
                //Image a = bielAlgo(image);
                Image a = boarAlgo(image);
                a.Save("Output\\"+path.Split('\\')[1].Split('.')[0]+".png", ImageFormat.Png);
            }
            stopwatch.Stop();
            Console.WriteLine($"Tempo de Execução: {stopwatch.Elapsed.TotalSeconds} segundos");

        }


  


        static Bitmap bielAlgo(Bitmap rawImage)

        {

            Console.WriteLine("Setup......");
            Bitmap newImage = new Bitmap(rawImage.Width, rawImage.Height);
            for (int i = 0; i < rawImage.Width; i++)
            {//deixa o bitmap totalmente branco
                for (int j = 0; j < rawImage.Height; j++)
                {
                    newImage.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                }
            }

            //todos os pixels da borda sendo convertidos para transparente caso sejam brancos
            for (int i = 0; i < rawImage.Width; i++)
            {
                if (isWhitePixel(rawImage.GetPixel(i, 0))) newImage.SetPixel(i, 0, Color.Transparent);
                else newImage.SetPixel(i,0,rawImage.GetPixel(i, 0));
                if (isWhitePixel(rawImage.GetPixel(i, rawImage.Height - 1))) newImage.SetPixel(i, newImage.Height - 1, Color.Transparent);
                else newImage.SetPixel(i, rawImage.Height - 1, rawImage.GetPixel(i, rawImage.Height - 1));
            }
            for (int j = 0; j < rawImage.Height; j++)
            {
                if (isWhitePixel(rawImage.GetPixel(0, j))) newImage.SetPixel(0, j, Color.Transparent);
                else newImage.SetPixel(0, j, rawImage.GetPixel(0, j));
                if (isWhitePixel(rawImage.GetPixel(rawImage.Width - 1, j))) newImage.SetPixel(rawImage.Width - 1, j, Color.Transparent);
                else newImage.SetPixel(rawImage.Width - 1, j, rawImage.GetPixel(rawImage.Width - 1, j));
            }

            Console.WriteLine("Cutting Background......");
            //3 loops são necesários para cortar o fundo branco
            for (int i = 1; i < rawImage.Width-1; i++)
            {/*esquerda->direita
               cima->baixo*/
                for (int j = 1; j < rawImage.Height-1; j++)
                {
                    if (newImage.GetPixel(i, j) == Color.FromArgb(0, 255, 255, 255)) continue;
                    var originalColor = rawImage.GetPixel(i, j);
                    if (isWhitePixel(originalColor))
                    {
                        if (newImage.GetPixel(i - 1, j) == Color.FromArgb(0, 255, 255, 255)){
                            newImage.SetPixel(i, j, Color.Transparent);
                        }
                        else if (newImage.GetPixel(i , j-1) == Color.FromArgb(0, 255, 255, 255))
                        {
                            newImage.SetPixel(i, j, Color.Transparent);
                        }

                        else
                        {
                            //Console.WriteLine("x:" + i.ToString() + "y:" + j.ToString());
                            newImage.SetPixel(i, j, Color.White);
                        }
                    }
                    else newImage.SetPixel(i, j, originalColor);
                    //Console.WriteLine(originalColor);

                }
            }
            for (int i = rawImage.Width - 2; i >0; i--)
            {/*direita->esquerda
               baixo->cima*/
                for (int j = rawImage.Height - 2; j >0; j--)
                {
                    if (newImage.GetPixel(i, j) == Color.FromArgb(0, 255, 255, 255)) continue;
                    var originalColor = rawImage.GetPixel(i, j);
                        if (isWhitePixel(originalColor))
                    {
                        if (newImage.GetPixel(i + 1, j) == Color.FromArgb(0, 255, 255, 255))
                        {
                            newImage.SetPixel(i, j, Color.Transparent);
                        }
                        else if (newImage.GetPixel(i, j + 1) == Color.FromArgb(0, 255, 255, 255))
                        {
                            newImage.SetPixel(i, j, Color.Transparent);
                        }

                        else
                        {
                            newImage.SetPixel(i, j, Color.White);
                        }
                    }
                    else newImage.SetPixel(i, j, originalColor);

                }
            }
            
            for (int i = 1; i < rawImage.Width - 1; i++)
            {//esquerda->direita
               //cima->baixo
                for (int j = 1; j < rawImage.Height - 1; j++)
                {

                    if (newImage.GetPixel(i, j) == Color.FromArgb(0, 255, 255, 255)) continue;
                    var originalColor = rawImage.GetPixel(i, j);
                    if (isWhitePixel(originalColor))
                    {
                        if (newImage.GetPixel(i - 1, j) == Color.FromArgb(0, 255, 255, 255))
                        {
                            newImage.SetPixel(i, j, Color.Transparent);
                        }
                        else if (newImage.GetPixel(i, j - 1) == Color.FromArgb(0, 255, 255, 255))
                        {
                            newImage.SetPixel(i, j, Color.Transparent);
                        }

                        else
                        {
                            //Console.WriteLine("x:" + i.ToString() + "y:" + j.ToString());
                            newImage.SetPixel(i, j, Color.White);
                        }
                    }
                    else newImage.SetPixel(i, j, originalColor);
                    //Console.WriteLine(originalColor);

                }
            }

            Console.WriteLine("Finishing......");
            return newImage;
        }

        static Bitmap boarAlgo(Bitmap rawImage)
        {

            Bitmap newImage = new Bitmap(rawImage.Width, rawImage.Height);
            for (int i = 0; i < rawImage.Width; i++)
            {//deixa o bitmap totalmente branco
                for (int j = 0; j < rawImage.Height; j++)
                {
                    newImage.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                }
            }


            for (int i = 0; i < rawImage.Width; i++)
            {
                cutDown(rawImage.Height,i);
                cutUp(rawImage.Height,i);

            }
            for (int j = 0; j < rawImage.Height; j++)
            {
                cutRight(rawImage.Width,j);
                cutLeft(rawImage.Width, j);

            }


            for (int i = 0; i < rawImage.Width; i++)
            {
                for (int j = 0; j < rawImage.Height; j++)
                {
                    if (newImage.GetPixel(i, j) != Color.FromArgb(0, 255, 255, 255))
                    {
                        newImage.SetPixel(i, j, rawImage.GetPixel(i, j));
                    }
                }
            }
            return newImage;


            void cutDown(int size, int x)
            {
                int j = 0;
                while (j < size)
                {
                    if (isWhitePixel(rawImage.GetPixel(x, j)))
                    {
                        newImage.SetPixel(x, j, Color.Transparent);
                        j++;
                    }
                    else {
                        if(isWhitePixel(rawImage.GetPixel(x+1,j)))
                        cutRight(size, j,x);

                        break;
                    }

                }
            }

            void cutUp(int size, int x)
            {
                int j = size - 1;
                while (j > -1)
                {
                    if (isWhitePixel(rawImage.GetPixel(x, j)))
                    {
                        newImage.SetPixel(x, j, Color.Transparent);
                        j--;
                    }
                    else break;

                }
            }

            void cutRight(int size, int y,int x=0)
            {
                int i = x;
                while (i <size)
                {
                    if (isWhitePixel(rawImage.GetPixel(i, y)))
                    {
                        newImage.SetPixel(i, y, Color.Transparent);
                        i++;
                    }
                    else break;

                }
            }

            void cutLeft(int size, int y)
            {
                int i = size - 1;
                while (i >-1)
                {
                    if (isWhitePixel(rawImage.GetPixel(i, y)))
                    {
                        newImage.SetPixel(i, y, Color.Transparent);
                        i--;
                    }
                    else break;

                }
            }
        }

        

        static bool isWhitePixel(Color col)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //checagem  se um pixel é aparentemente branco
            //tira a diferença entre os valores da cor, se ela for menor que o offset, o pixel é branco
            int offset = 20;
            int high  = new int[] { col.A, col.R, col.G, col.B }.Max();
            int low = new int[] { col.A, col.R, col.G, col.B }.Min();
            stopwatch.Stop();
            //Console.WriteLine($"Tempo de Execução: {stopwatch.Elapsed.TotalSeconds} segundos");
            if ((high - low) > offset) return false;
            else return true;



        }
    }
}
