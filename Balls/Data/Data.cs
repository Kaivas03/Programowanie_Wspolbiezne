using System;
using System.Collections.Generic;
using System.Threading;

namespace DataLayer
{
    public abstract class DataAPI : IObserver<Sphere>, IObservable<Sphere>
    {
        public abstract int AddSphere();
        public abstract int GetHeight();
        public abstract int GetWidth();
        public abstract double GetSpherePositionX(int id);
        public abstract double GetSpherePositionY(int id);
        public abstract void SetSphereSpeed(int id, double speed);
        public abstract double GetSphereSpeed(int id);
        public abstract void SetSphereMovement(int id, double x, double y);
        public abstract void SwitchDirectionForSphere(int id, bool isX);
        public abstract double[] GetSphereDirections(int id);
        public abstract int GetSphereRadius(int id);
        public abstract double GetSphereMass(int id);
        public abstract int GetSpheresCount();
        public abstract void RandomisePozitions(int width, int height);
        public abstract void StartMovingSphere(int id);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(Sphere sphere);
        public abstract IDisposable Subscribe(IObserver<Sphere> observer);

        public static DataLayer CreateDataLayer(int width, int height)
        {
            return new DataLayer(width, height);
        }

        public static DataLayer CreateDataLayer(Field field)
        {
            return new DataLayer(field);
        }

    }

    public class DataLayer : DataAPI
    {
        private readonly Field field;
        private IDisposable unsubscriber;
        private readonly IList<IObserver<Sphere>> observers;
        public bool IsMoving { get; set; } = false;

        public DataLayer(int width, int height)
        {
            this.field = new Field(width, height);
            observers = new List<IObserver<Sphere>>();
        }

        public DataLayer(Field field)
        {
            this.field = field;
            observers = new List<IObserver<Sphere>>();
        }

        public override int GetSpheresCount()
        {
            return field.SphereList.Count;
        }

        public override int GetSphereRadius(int id)
        {
            return GetSphere(id).R;
        }

        public override double GetSphereSpeed(int id)
        {
            return GetSphere(id).Speed;
        }

        public override double GetSphereMass(int id)
        {
            return GetSphere(id).M;
        }

        public override double[] GetSphereDirections(int id)
        {
            double[] table = new double[2];
            table[0] = GetSphere(id).Direction_X;
            table[1] = GetSphere(id).Direction_Y;
            return table;
        }

        private Sphere GetSphere(int id)
        {
            return field.getSphere(id);
        }

        private List<Sphere> GetSpheres()
        {
            return field.SphereList;
        }

        public override void StartMovingSphere(int id)
        {
            IsMoving = true;
            GetSphere(id).StartMovingThread();
        }

        public override void SetSphereMovement(int id, double x, double y)
        {
            GetSphere(id).Direction_X = x;
            GetSphere(id).Direction_Y = y;
        }

        public override void SwitchDirectionForSphere(int id, bool isX)
        {
            if (isX)
                GetSphere(id).AlternateDirection('x');
            else
                GetSphere(id).AlternateDirection('y');
        }

        public override void SetSphereSpeed(int id, double speed)
        {
            GetSphere(id).Speed = speed;
        }

        public override int GetHeight()
        {
            return field.Height;
        }

        public override int GetWidth()
        {
            return field.Width;
        }

        public override void RandomisePozitions(int width, int height)
        {
            foreach (Sphere sphere in GetSpheres())
            {
                sphere.PickRandomPosition(width, height);
            }
        }

        public override double GetSpherePositionX(int id)
        {
            return GetSphere(id).X;
        }

        public override double GetSpherePositionY(int id)
        {
            return GetSphere(id).Y;
        }

        public override int AddSphere()
        {
            int i = field.AddSphere();
            Subscribe(GetSphere(i));
            return i;
        }

        #region Observer
        public override void OnCompleted()
        {
            unsubscriber.Dispose();
        }

        public override void OnError(Exception error)
        {
            throw error;
        }

        public override void OnNext(Sphere Sphere)
        {

            foreach (IObserver<Sphere> observer in observers)
            {
                observer.OnNext(Sphere);
            }
        }

        public virtual void Subscribe(IObservable<Sphere> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        #endregion

        #region provider

        public override IDisposable Subscribe(IObserver<Sphere> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly IList<IObserver<Sphere>> observers;
            private readonly IObserver<Sphere> observer;

            public Unsubscriber
            (IList<IObserver<Sphere>> observers, IObserver<Sphere> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observer != null && observers.Contains(observer))
                    observers.Remove(observer);
            }
        }

        #endregion


    }
}