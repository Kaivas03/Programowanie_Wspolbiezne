using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public abstract class DataAPI : IObserver<Sphere>, IObservable<Sphere>
    {
        //All the methods of abstract interface implemented below.
        public abstract int AddSphere();
        public abstract int GetHeight();
        public abstract int GetWidth();
        public abstract int GetID(int id);
        public abstract double GetSpherePositionX(int id);
        public abstract double GetSpherePositionY(int id);
        public abstract void SetSphereSpeed(int id, double speed);
        public abstract double GetSphereSpeed(int id);
        public abstract void SetSphereDirections(int id, double x, double y);
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
        public abstract string PrepareLog(int id);
        public abstract IDisposable Subscribe(IObserver<Sphere> observer);

        public static DataLayer CreateDataLayer(int width, int height)
        {
            return new DataLayer(width, height);
        }

    }

    public class DataLayer : DataAPI
    {
        //DataAPI will containa a field of balls and with some particular size.
        private readonly Field field;
        //Unsubscriber and observers.
        private IDisposable unsubscriber;
        private readonly IList<IObserver<Sphere>> observers;

        //Constructor
        public DataLayer(int width, int height)
        {
            //Assigning new field.
            this.field = new Field(width, height);
            //Creating and assigning new list of observers for spheres.
            observers = new List<IObserver<Sphere>>();
        }

        //Returns amount of spheres stored in Data.
        public override int GetSpheresCount()
        {
            return field.SphereList.Count;
        }

        //Returns radius of particular sphere.
        public override int GetSphereRadius(int id)
        {
            return GetSphere(id).R;
        }

        //Returns speed of particular sphere.
        public override double GetSphereSpeed(int id)
        {
            return GetSphere(id).Speed;
        }

        public override int GetID(int id)
        {
            return GetSphere(id).Id;
        }

        //Returns mass of particular sphere.
        public override double GetSphereMass(int id)
        {
            return GetSphere(id).M;
        }

        //Returns directions of particular sphere (how much and where the sphere moves on x and y axis).
        public override double[] GetSphereDirections(int id)
        {
            double[] table = new double[2];
            table[0] = GetSphere(id).Direction_X;
            table[1] = GetSphere(id).Direction_Y;
            return table;
        }

        //Returnsparticular sphere (Onlhy accessible within data api).
        private Sphere GetSphere(int id)
        {
            return field.getSphere(id);
        }

        //One of the methods in chain that eventualy order the sphere to start moving.
        public override void StartMovingSphere(int id)
        {
            GetSphere(id).StartMoving();
        }

        //Setter for direction of the sphere.
        public override void SetSphereDirections(int id, double x, double y)
        {
            GetSphere(id).Direction_X = x;
            GetSphere(id).Direction_Y = y;
        }

        //Can switch sphere'e direction (multiply by -1).
        public override void SwitchDirectionForSphere(int id, bool isX)
        {
            if (isX)
                GetSphere(id).AlternateDirection('x');
            else
                GetSphere(id).AlternateDirection('y');
        }

        //Setter for sphere's speed.
        public override void SetSphereSpeed(int id, double speed)
        {
            GetSphere(id).Speed = speed;
        }

        //Getters for height and width of the field.
        public override int GetHeight()
        {
            return field.Height;
        }
        public override int GetWidth()
        {
            return field.Width;
        }

        //Randomises positions of all the spheres contained by the field.
        public override void RandomisePozitions(int width, int height)
        {
            foreach (Sphere sphere in field.SphereList)
            {
                sphere.PickRandomPosition(width, height);
            }
        }

        //Getters for position of particular sphere.
        public override double GetSpherePositionX(int id)
        {
            return GetSphere(id).X;
        }
        public override double GetSpherePositionY(int id)
        {
            return GetSphere(id).Y;
        }

        //Adds a default sphere to the field.
        public override int AddSphere()
        {
            int i = field.AddSphere();
            Subscribe(GetSphere(i));
            return i;
        }

        //Here we create a string that is to become an entry in our log.
        public override string PrepareLog(int id)
        {
            //This StringBulider is responsible for assembling our string for
            //the logger.
            StringBuilder logMessage = new StringBuilder();

            //Sphere's ID goes first.
            logMessage.Append($"ID: {id}\n");

            //Next it's position (X an Y variables).
            logMessage.Append($"X: {GetSphere(id).X}\n");
            logMessage.Append($"Y: {GetSphere(id).Y}\n");

            //And the movement in each direction it's about to make.
            logMessage.Append($"X movement: {GetSphere(id).Direction_X}\n");
            logMessage.Append($"Y movement: {GetSphere(id).Direction_Y}\n\n");

            return logMessage.ToString();
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