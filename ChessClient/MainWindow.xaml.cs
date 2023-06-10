using ChessClient.Game;
using ChessClient.resources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Figure = ChessClient.Game.Figure;

namespace ChessClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChessGame chessGame;
        int table_POS_X = 100;
        int table_POS_Y = 100;
        int header_POS_y = 65;
        int header_POS_x = 65;
        int CELL_WIDTH = 52;
        int CELL_HEIGHT = 52;
        int figure_offset_x = 26;
        int figure_offset_y = 26;

        static readonly Color WhiteCellColor = Color.FromRgb(235, 235, 208);
        static readonly Color BlackCellColor = Color.FromRgb(119, 149, 86);

        public MainWindow()
        {
            InitializeComponent();
            PaintMap();
        }


        void PaintMap()
        {
            var white = new SolidColorBrush(WhiteCellColor);
            var black = new SolidColorBrush(BlackCellColor);
            List<Rectangle> cells = new List<Rectangle>();

            for (int i = 0; i < 8; i++)
            {
                Label lab1 = new Label();
                lab1.Width = 25;
                lab1.Height = 25;
                lab1.VerticalAlignment = VerticalAlignment.Center;
                lab1.BorderBrush = Brushes.Black;
                lab1.Content = (i + 1).ToString();
                Canvas1.Children.Add(lab1);
                Canvas.SetLeft(lab1, table_POS_X + i * CELL_WIDTH);
                Canvas.SetTop(lab1, header_POS_y);

                Label lab2 = new Label();
                lab2.Width = 25;
                lab2.Height = 25;
                lab2.VerticalAlignment = VerticalAlignment.Center;
                int a = (int)'A';
                a += i;
                lab2.Content = ((char)a).ToString();
                Canvas1.Children.Add(lab2);
                Canvas.SetLeft(lab2, header_POS_x);
                Canvas.SetTop(lab2, table_POS_Y + i * CELL_HEIGHT);
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var el = new Rectangle();
                    el.Width = 50;
                    el.Height = 50;
                    el.VerticalAlignment = VerticalAlignment.Top;
                    el.Fill = (i + j) % 2 == 1 ? white : black;
                    el.StrokeThickness = 1;
                    Canvas1.Children.Add(el);
                    Canvas.SetLeft(el, table_POS_X + i * CELL_WIDTH);
                    Canvas.SetTop(el, table_POS_Y + j * CELL_HEIGHT);
                }
        }

        public void RefreshBoard()
        {
            PaintMap();

            if (chessGame == null)
                return;
            // else we can paint current figures position 
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PaintFigure(i, j, chessGame.map.map[i, j]);
                    PaintFigure(i, j, chessGame.possibleMoves.map[i, j]);
                }
            }

        }

        private void PaintFigure(int i, int j, Figure figure)
        {
            switch (figure)
            {
                case Figure.bRook:
                case Figure.wRook:
                case Figure.wBishop:
                case Figure.bBishop:
                case Figure.bKnight:
                case Figure.wQueen:
                case Figure.bQueen:
                case Figure.wKing:
                case Figure.wKnight:
                case Figure.bKing:
                case Figure.wPawn:
                case Figure.bPawn:
                    PaintPicture(i, j, ChessColor.Black, PNGReader.getChessSkin(figure));
                    break;
                case Figure.moveMarker:
                    PaintMarker(i, j, ChessColor.Empty);
                    break;
                default:
                    break;
            }

        }

        private void PaintKing(int x, int y, ChessColor c) => PaintTextFigure(x, y, c, "K"); 
        private void PaintQueen(int x, int y, ChessColor c) => PaintTextFigure(x, y, c, "Q");
        private void PaintBishop(int x, int y, ChessColor c) => PaintTextFigure(x, y, c, "B");
        private void PaintKnight(int x, int y, ChessColor c) => PaintTextFigure(x, y, c, "N");
        private void PaintRook(int x, int y, ChessColor c) => PaintTextFigure(x, y, c, "R");
        private void PaintPawn(int x, int y, ChessColor c) => PaintTextFigure(x, y, c, "P");
        private void PaintTextFigure(int x, int y, ChessColor c, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 32;
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(c == ChessColor.Black ? Color.FromRgb(11, 11, 11) : Color.FromRgb(200, 200, 200));
            Canvas1.Children.Add(textBlock);

            Canvas.SetLeft(textBlock, table_POS_X + x * CELL_WIDTH);
            Canvas.SetTop(textBlock, table_POS_Y + y * CELL_HEIGHT);

        }

        private void PaintPicture(int x, int y, ChessColor c, ImageBrush brush)
        {
            Rectangle myRectangle = new Rectangle();
            myRectangle.Width = 50;
            myRectangle.Height = 50;
            myRectangle.Fill = brush;
            Canvas1.Children.Add(myRectangle);
            Canvas.SetLeft(myRectangle, table_POS_X + x * CELL_WIDTH );
            Canvas.SetTop(myRectangle, table_POS_Y + y * CELL_HEIGHT );

        }
        private void PaintMarker(int x, int y, ChessColor c)
        {
            var rb = new Rectangle();

            rb.Width = 5;
            rb.Height = 5;
            rb.Fill = new SolidColorBrush(Color.FromRgb(1, 200, 200));
            Canvas1.Children.Add(rb);
            Canvas.SetLeft(rb, table_POS_X + x * CELL_WIDTH + CELL_WIDTH / 2 - 5 / 2);
            Canvas.SetTop(rb, table_POS_Y + y * CELL_HEIGHT + CELL_WIDTH / 2 - 5 / 2);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            chessGame = new ChessGame();
            chessGame.Restart();
            RefreshBoard();
        }

        public delegate void PaintCallback();

        private void cavas_Mouse1Down(object sender, MouseButtonEventArgs e)
        {
            if (chessGame == null)
                return;

            var pos = Mouse.GetPosition(sender as Canvas);
            var chessCell = ConvertToChessCoordinates(pos);
            chessGame.OnClick(chessCell, out var refresh);
            if (refresh)
                RefreshBoard();
        }
        private Position ConvertToChessCoordinates(Point pos)
        {
            Position result = new Position();
            result.x = (int)Math.Floor((pos.X - table_POS_X) / CELL_WIDTH);
            result.y = (int)Math.Floor((pos.Y - table_POS_Y) / CELL_HEIGHT);
            return result;
        }


        private void DrawChessKing(int x, int y, ChessColor c)
        {

            Path kingBody = new Path();
            //            kingBody.Data = Geometry.Parse("M0 0 L100 0 Q150 50 100 100 L0 100 Q50 50 0 0z");
            kingBody.Data = Geometry.Parse("M5 50 L10 0 L40 0 L45 50z");
            kingBody.Fill = c == ChessColor.Black ? Brushes.Black : Brushes.White;

            PathGeometry kingCrown = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(5, 0);
            figure.Segments.Add(new LineSegment(new Point(25, 20), true));
            figure.Segments.Add(new LineSegment(new Point(45, 0), true));
            kingCrown.Figures.Add(figure);

            Path kingHead = new Path();
            kingHead.Data = kingCrown;
            kingBody.Fill = c == ChessColor.Black ? Brushes.Black : Brushes.White;
            kingHead.Margin = new Thickness(0, -15, 0, 0);


            Canvas1.Children.Add(kingBody);
            //Canvas1.Children.Add(kingHead);
            Canvas.SetLeft(kingBody, table_POS_X + x * CELL_WIDTH + 2);
            Canvas.SetTop(kingBody, table_POS_Y + y * CELL_HEIGHT + 2);

            //Canvas.SetLeft(kingHead, table_POS_X + x * CELL_WIDTH + CELL_WIDTH / 2 );
            //Canvas.SetTop(kingHead, table_POS_Y + y * CELL_HEIGHT + CELL_WIDTH / 2 );


        }

    }
}
