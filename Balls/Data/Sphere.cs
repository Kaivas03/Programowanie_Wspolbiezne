using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer
{
    //This class defines all the properties of spheres, that we are going to use and some methods connected to them.
    public class Sphere : IObservable<Sphere>
    {
        //Properties:
        private Random randomiser = new Random();
        public int Id { get; set; }
        public int R { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Direction_X { get; set; }
        public double Direction_Y { get; set; }
        public double Speed { get; set; }
        public double M { get; set; }
        internal readonly IList<IObserver<Sphere>> observers;
        private Task SphereThread;

        //Constructor assigning to a sphere it's position in 2D and radius (how big it is).
        public Sphere(int Id, double x = 10, double y = 10, int r = 5)
        {
            this.Id = Id;
            M = 10;
            R = r;
            X = x;
            Y = y;
            observers = new List<IObserver<Sphere>>();
            PickRandomDirection();
        }

        //Function that allows us to pick random starting positions for the spheres.
        public void PickRandomPosition(int width, int height)
        {
            //We use max width and hight - R*4, beacuse we want the balls to start with some distance from the edge of our window.
            this.X = this.R * 4 + randomiser.Next(width - this.R * 8);
            this.Y = this.R * 4 + randomiser.Next(height - this.R * 8);
            NotifyObservers();
        }

        public void PickRandomDirection()
        {
            //Changes speed.
            this.Speed = 3;

            //We pick randomly either -1 or 1.
            int X_axis = randomiser.Next(2) == 1 ? 1 : -1;
            int Y_axis = randomiser.Next(2) == 1 ? 1 : -1;

            //Finally we randomise where we are moving to.
            this.Direction_X = (double)(0.0001 * X_axis * (1 + randomiser.Next(10000))) * Speed;
            this.Direction_Y = (double)(0.0001 * Y_axis * (1 + randomiser.Next(10000))) * Speed;

        }

        //This method makes a single sphere change it's position towards the direction.
        public void Move()
        {
            //We move some distance that is increased by given speed.
            lock (this)
            {
                X += Direction_X;
                Y += Direction_Y;
            }


            NotifyObservers();
        }

        //Here we change direction of one axis if the sphere reaches the screen's edge.
        public void AlternateDirection(char whichDirection)
        {
            if (whichDirection == 'x')
                this.Direction_X = -this.Direction_X;
            else if (whichDirection == 'y')
                this.Direction_Y = -this.Direction_Y;

            NotifyObservers();
        }

        public void StartMoving()
        {
            while (true)
            {
                Move();
                NotifyObservers();
            }
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers.ToList())
            {
                if (observer != null)
                {
                    observer.OnNext(this);
                }

            }
        }

        public void StartMovingThread()
        {
            if (SphereThread is null)
            {
                CancellationToken token = new CancellationToken();
                this.SphereThread = new Task(StartMoving, token);
            }
            SphereThread.Start();
        }

        public IDisposable Subscribe(IObserver<Sphere> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<Sphere>> _observers;
            private IObserver<Sphere> _observer;

            public Unsubscriber(IList<IObserver<Sphere>> observers, IObserver<Sphere> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}