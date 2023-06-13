using ChessClient.Game;
using ChessClient.resources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        int header_POS_Y = 65;
        int header_POS_X = 65;
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

            for (int i = 0; i < 8; i++)
            {
                int a = (int)'A';
                a += i;

                Label lab1Up = new Label()
                {
                    Width = 26,
                    Height = 26,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderBrush = Brushes.Black,
                    Content = ((char)a).ToString(),
                    BorderThickness = new Thickness(0)
                };

                Label lab1Down = new Label()
                {
                    Width = 26,
                    Height = 26,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderBrush = Brushes.Black,
                    Content = ((char)a).ToString(),
                    BorderThickness = new Thickness(0)
                };

                Canvas1.Children.Add(lab1Up);
                Canvas1.Children.Add(lab1Down);

                Canvas.SetLeft(lab1Up, table_POS_X + i * CELL_WIDTH + lab1Up.Width / 2);
                Canvas.SetTop(lab1Up, table_POS_Y - lab1Up.Height);

                Canvas.SetLeft(lab1Down, table_POS_X + i * CELL_WIDTH + lab1Down.Width / 2);
                Canvas.SetTop(lab1Down, table_POS_Y + 8 * CELL_HEIGHT);


                Label lab2Left = new Label()
                {
                    Width = 26,
                    Height = 26,
                    BorderBrush = Brushes.Black,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = (8 - i).ToString(),
                    BorderThickness = new Thickness(0)
                };
                Label lab2Right = new Label()
                {
                    Width = 26,
                    Height = 26,
                    BorderBrush = Brushes.Black,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = (8 - i).ToString(),
                    BorderThickness = new Thickness(0)
                };
                Canvas1.Children.Add(lab2Left);
                Canvas1.Children.Add(lab2Right);

                Canvas.SetLeft(lab2Left, header_POS_X + lab2Left.Width / 2);
                Canvas.SetTop(lab2Left, table_POS_Y + i * CELL_HEIGHT + lab2Left.Height / 2);
                Canvas.SetLeft(lab2Right, table_POS_X + 8 * CELL_WIDTH);
                Canvas.SetTop(lab2Right, table_POS_Y + i * CELL_HEIGHT + lab2Right.Height / 2);
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

        public void WinMessage(GameStage gs)
        {
            //PaintMap();
            var text1 = gs == GameStage.WhiteWon ? "White" : "Black";
            var text2 = "WON! ";
            var color = gs == GameStage.BlackWon ? ChessColor.Black : ChessColor.White;

            for (int i = 0; i < 5; i++)
            {
                PaintTextFigure(2 + i, 3, color, text1[i].ToString());
                PaintTextFigure(2 + i, 4, color, text2[i].ToString());
            }

        }


        public void TieMessage()
        {
            var text1 = "TIE";
            for (int i = 0; i < 3; i++)
            {
                PaintTextFigure(2 + i, 2, Color.FromRgb(100, 0, 0), text1[i].ToString());
                PaintTextFigure(3 + i, 3, Color.FromRgb(0, 100, 0), text1[i].ToString());
                PaintTextFigure(4 + i, 4, Color.FromRgb(0, 0, 100), text1[i].ToString());
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
        private void PaintTextFigure(int x, int y, ChessColor c, string text) => PaintTextFigure(x, y, c == ChessColor.Black ? Color.FromRgb(1, 1, 1) : Color.FromRgb(220, 220, 220), text);

        private void PaintTextFigure(int x, int y, Color c, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 42;
            textBlock.FontWeight = FontWeight.FromOpenTypeWeight(600);
            textBlock.Text = text;

            textBlock.Foreground = new SolidColorBrush(c);
            Canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, table_POS_X + x * CELL_WIDTH + figure_offset_x);
            Canvas.SetTop(textBlock, table_POS_Y + y * CELL_HEIGHT + figure_offset_y);
        }

        private void PaintPicture(int x, int y, ChessColor c, ImageBrush brush)
        {
            Rectangle myRectangle = new Rectangle();
            myRectangle.Width = 50;
            myRectangle.Height = 50;
            myRectangle.Fill = brush;
            Canvas1.Children.Add(myRectangle);
            Canvas.SetLeft(myRectangle, table_POS_X + x * CELL_WIDTH);
            Canvas.SetTop(myRectangle, table_POS_Y + y * CELL_HEIGHT);

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

            switch (chessGame.CurrentStage)
            {
                case GameStage.WhiteWon:
                case GameStage.BlackWon:
                    WinMessage(chessGame.CurrentStage);
                    break;
                case GameStage.Tie:
                    TieMessage();
                    break;
                default:
                    break;
            }

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
