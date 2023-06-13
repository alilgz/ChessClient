using ChessClient.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessClient.resources
{
    public class PNGReader
    {
        //chess-39310

        private static Dictionary<int, ImageBrush>  pieces = null;

        public static ImageBrush getChessSkin(Figure f) => getChessSkin((int)f);
        public static ImageBrush getChessSkin(int number)
        {
            if (pieces == null)
            {

                pieces = new Dictionary<int, ImageBrush>();

                for (int i=0; i<=13; i++)
                {
                    try
                    {
                        BitmapImage theImage = new BitmapImage();
                        theImage.BeginInit();
                        theImage.UriSource = new Uri($"resources\\chess-{i}.png", UriKind.Relative);
                        theImage.EndInit();

                        pieces.Add(i, new ImageBrush(theImage)
                        {
                            Stretch = Stretch.Uniform
                        });
                    }
                    catch
                    {
                        pieces.Add(i, null);
                    }
                }
            }

            return pieces[number];
        }

    }
}
