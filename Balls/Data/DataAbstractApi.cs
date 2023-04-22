using System;
using System.Collections.Generic;

namespace Data
{
    public abstract class DataAbstractApi
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        internal abstract List<Ball> balls { get; }
        public abstract void CreateBallsList(int count);
        public abstract List<Ball> GetBallsList();
        public abstract int GetX(int i);
        public abstract int GetY(int i);
        public abstract int GetSize(int i);
        public abstract int GetCount { get; }
        public static DataAbstractApi CreateApi(int width, int height)
        {
            return new DataApi(width, height);
        }
    }

    internal class DataApi : DataAbstractApi
    {
        public override int Width { get; }
        public override int Height { get; }
        internal override List<Ball> balls { get; }
        public DataApi(int width, int height)
        {
            Width = width;
            Height = height;
            balls = new List<Ball>();
        }
        public override void CreateBallsList(int count)
        {
            Random random = new Random();
            if (count > 0)
            {
                for (uint i = 0; i < count; i++)
                {
                    int radius = random.Next(20, 40);
                    int x = random.Next(radius, Width - radius);
                    int y = random.Next(radius, Height - radius);
                    int newX = random.Next(radius);
                    int newY = random.Next(radius);
                    Ball ball = new Ball(radius, x, y, newX, newY);
                    balls.Add(ball);
                }
            }
            if (count < 0)
            {
                for (int i = count; i < 0; i++)
                {
                    if (balls.Count > 0)
                    {
                        balls.Remove(balls[balls.Count - 1]);
                    };
                }
            }

        }
        public override List<Ball> GetBallsList()
        {
            return balls;
        }
        public override int GetX(int i)
        {
            return balls[i].X;
        }
        public override int GetY(int i)
        {
            return balls[i].Y;
        }
        public override int GetSize(int i)
        {
            return balls[i].Size;
        }
        public override int GetCount => balls.Count;
    }
}
