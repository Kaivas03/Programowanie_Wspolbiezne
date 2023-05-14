using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Model
{
    public abstract class ModelAPI : IObserver<int>, IObservable<ISphere>
    {
        internal Logic.LogicAPI LogicLayer;
        public List<PresentationSphere> PresentedSpheres;
        public int R { get; set; }
        public int[] Size { get; set; }
        public static ModelAPI CreateApi()
        {
            return new ModelLayer();
        }

        public abstract void MoveSpheres();
        public abstract void AddSpheres(int howMany);
        public abstract void AddNewSpheres(int howMany);
        public abstract void PickRandomPositions(int width, int height);
        public abstract void Subscribe(IObservable<int> provider);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);
        public abstract IDisposable Subscribe(IObserver<ISphere> observer);
    }

    public interface ISphere : INotifyPropertyChanged
    {
        double Top { get; }
        double Left { get; }
        double Diameter { get; }
    }

    public class SphereChangeEventArgs : EventArgs
    {
        public ISphere Sphere { get; set; }
    }

    public interface INotifyBallChanged
    {
        // Occurs when a property value changes.In this case: Sphere.
        event EventHandler<SphereChangeEventArgs> BallChanged;
    }

    internal class ModelLayer : ModelAPI
    {
        private IDisposable unsubscriber;
        public event EventHandler<SphereChangeEventArgs> SphereChanged;
        private IObservable<EventPattern<SphereChangeEventArgs>> eventObservable = null;

        //We start with construction of the whole ModelAPI.
        public ModelLayer()
        {
            eventObservable = Observable.FromEventPattern<SphereChangeEventArgs>(this, "SphereChanged");
            //How big will the spheres be.
            R = 5;
            //If null, then we go with default option (for object it's null). We also copy board size from logic.
            LogicLayer = LogicLayer ?? Logic.LogicAPI.CreateLayer();
            Size = new int[2] { LogicLayer.Size[0], LogicLayer.Size[1] };
            //Initializing empty list for spheres stored in logical layer.
            PresentedSpheres = new List<PresentationSphere>();
            //We create presentation of spheres from logic layer and place them in the list.
            AddSpheres(LogicLayer.CountSpheres());
            Subscribe(LogicLayer);
        }

        //Method for adding random amount of spheres to the presentation layer.
        public override void AddSpheres(int howMany)
        {
            for (int i = 0; i < LogicLayer.CountSpheres(); i++)
            {
                AddPresentedSphere(i);
            }
        }

        public override void AddNewSpheres(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                AddPresentedSphere(LogicLayer.AddObject());
            }

            foreach (PresentationSphere sphere in PresentedSpheres)
            {
                SphereChanged?.Invoke(this, new SphereChangeEventArgs() { Sphere = sphere });
            }
        }

        public void AddPresentedSphere(int id)
        {
            double[] position = new double[2];
            position[0] = LogicLayer.GetSphereX(id);
            position[1] = LogicLayer.GetSphereY(id);
            double radius = LogicLayer.GetSphereRadius(id);
            Random rnd = new Random();
            PresentedSpheres.Add(new PresentationSphere(id, position[0], position[1], radius, "red"));
        }


        //Triggering movement in logical layer every 15ms.
        public override void MoveSpheres()
        {
            LogicLayer.MoveAll();
        }

        public void UpdateSphere(int id)
        {
            double[] position = new double[2];
            position[0] = LogicLayer.GetSphereX(id);
            position[1] = LogicLayer.GetSphereY(id);
            PresentedSpheres[id].Left = position[0];
            PresentedSpheres[id].Top = position[1];
        }

        //Ordering logic layer to randomise all positions for spheres.
        public override void PickRandomPositions(int width, int height)
        {
            LogicLayer.PickRandomPositions(width, height);
        }

        #region observer

        public override void Subscribe(IObservable<int> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public override void OnCompleted()
        {
            unsubscriber.Dispose();
        }

        public override void OnError(Exception error)
        {
            throw error;
        }

        public override void OnNext(int id)
        {
            UpdateSphere(id);
        }

        #endregion

        #region provider

        public override IDisposable Subscribe(IObserver<ISphere> observer)
        {
            return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Sphere), ex => observer.OnError(ex), () => observer.OnCompleted());
        }

        #endregion
    }
}