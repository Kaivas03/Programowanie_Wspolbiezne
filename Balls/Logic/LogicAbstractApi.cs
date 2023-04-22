using System;
using System.Collections.Generic;
using Data;


namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract event EventHandler Update;
        public abstract void CreateBallsList(int count);
        public abstract void UpdateBalls();
        public abstract void Start();
        public abstract void Stop();
        public abstract void SetInterval(int ms);
        public abstract int GetX(int i);
        public abstract int GetY(int i);
        public abstract int GetSize(int i);
        public abstract int GetCount { get; }

        public static LogicAbstractApi CreateApi(int width, int height, TimerApi timer = default(TimerApi))
        {
            return new LogicApi(width, height, timer ?? TimerApi.CreateBallTimer());
        }

    }
    internal class LogicApi : LogicAbstractApi
    {
        private readonly TimerApi timer;

        private DataAbstractApi dataLayer;
        public LogicApi(int width, int height, TimerApi WPFTimer)
        {
            dataLayer = DataAbstractApi.CreateApi(width, height);
            timer = WPFTimer;
            SetInterval(30);
            timer.Tick += (sender, args) => UpdateBalls();
        }
        public override void CreateBallsList(int count)
        {
            dataLayer.CreateBallsList(count);
        }

        public override event EventHandler Update { add => timer.Tick += value; remove => timer.Tick -= value; }

        public override int GetX(int i)
        {
            return dataLayer.GetX(i);
        }
        public override int GetCount => dataLayer.GetCount;

        public override int GetY(int i)
        {
            return dataLayer.GetY(i);
        }
        public override int GetSize(int i)
        {
            return dataLayer.GetSize(i);
        }
        public override void UpdateBalls()
        {
            foreach (Ball ball in dataLayer.GetBallsList())
            {
                ball.newPosition(600, 480);
            }
        }

        public override void Start()
        {
            timer.Start();
        }

        public override void Stop()
        {
            timer.Stop();
        }

        public override void SetInterval(int ms)
        {
            timer.Interval = TimeSpan.FromMilliseconds(ms);
        }
    }
}
