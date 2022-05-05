using Logic;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;



namespace Model
{
    public abstract class ModelAbstractApi
    {
        public abstract int width { get; }
        public abstract int height { get; }
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }
        public abstract void CreateEllipses(int ballVal);
        public abstract void Move();

        public abstract void Stop();


        public static ModelAbstractApi CreateApi(int Weight, int Height)
        {
            return new ModelApi(Weight, Height);
        }
    }
    internal class ModelApi : ModelAbstractApi
    {
        public override int width { get; }
        public override int height { get; }

        private LogicAbstractApi LogicLayer;
        public override List<Ellipse> ellipseCollection { get; }
        public override Canvas Canvas { get; set; }
        public ModelApi(int Width, int Height)
        {

            width = Width;
            height = Height;
            LogicLayer = LogicAbstractApi.CreateApi(width, height);
            ellipseCollection = new List<Ellipse>();
            Canvas = new Canvas();
            Canvas.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.VerticalAlignment = VerticalAlignment.Top;
            Canvas.Width = width;
            Canvas.Height = height;
            Canvas.Background = new SolidColorBrush(Color.FromRgb(241, 237, 237));
            LogicLayer.Update += (sender, args) => Move();
        }
        public override void CreateEllipses(int ballVal)
        {
            LogicLayer.CreateBallsList(ballVal);

            for (int i = LogicLayer.GetCount - ballVal; i < LogicLayer.GetCount; i++)
            {
                Ellipse ellipse = new Ellipse { Width = LogicLayer.GetSize(i), Height = LogicLayer.GetSize(i), Fill = Brushes.Black };
                Canvas.SetLeft(ellipse, LogicLayer.GetX(i));
                Canvas.SetTop(ellipse, LogicLayer.GetY(i));
                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
            LogicLayer.Start();

        }

        public override void Move()
        {
            for (int i = 0; i < LogicLayer.GetCount; i++)
            {
                Canvas.SetLeft(ellipseCollection[i], LogicLayer.GetX(i));
                Canvas.SetTop(ellipseCollection[i], LogicLayer.GetY(i));
            }
            for (int i = LogicLayer.GetCount; i < ellipseCollection.Count; i++)
            {
                Canvas.Children.Remove(ellipseCollection[ellipseCollection.Count - 1]);
                ellipseCollection.Remove(ellipseCollection[ellipseCollection.Count - 1]);
            }
        }

        public override void Stop()
        {
            LogicLayer.Stop();
        }
    }

}
