using DataLayer;
using System;
using System.Collections.Generic;

namespace Logic
{
    public abstract class LogicAPI : IObservable<int>, IObserver<Sphere>
    {
        //Abstract methods of our API.
        public abstract double GetSphereX(int id);
        public abstract double GetSphereY(int id);
        public abstract double GetSphereRadius(int id);
        public abstract int CountSpheres();
        public abstract void PickRandomPositions(int width, int height);
        public int[] Size { get; set; }
        public abstract void MoveAll();
        public abstract int AddObject();
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(Sphere sphere);
        public abstract IDisposable Subscribe(IObserver<int> observer);

        //We create bussiness logic.
        public static LogicAPI CreateLayer(DataAPI data = default)
        {
            //If null, then we go with default option (for object it's null).
            return new BusinessLogic(data ?? DataAPI.CreateDataLayer(350, 350));
        }

        //This class implements our abstract API.
        private class BusinessLogic : LogicAPI
        {
            private readonly DataAPI DataLayer;
            private Data.Logger logger;
            private IDisposable unsubscriber;
            private IList<IObserver<int>> observers;

            //Constructor creates a field object and requiers DataAPI object or NULL(defauklt, when Data layer is not used).
            public BusinessLogic(DataAPI api)
            {
                DataLayer = api;
                Subscribe(DataLayer);
                observers = new List<IObserver<int>>();

                //We create logger when creating the logic of this application.
                logger = new Data.Logger();
                logger.StartLogging();

                int[] table = new int[2];
                //Size is asigned based on width and height stored in data layer.
                table[0] = DataLayer.GetWidth();
                table[1] = DataLayer.GetHeight();
                Size = table;
            }

            //Move all spheres using method from API.
            public override void MoveAll()
            {
                for (int i = 0; i < DataLayer.GetSpheresCount(); i++)
                {
                    DataLayer.StartMovingSphere(i);
                }
            }

            //Adding new sphere to our field.
            public override int AddObject()
            {
                return DataLayer.AddSphere();
            }

            //Uses method from sphere class to randomise position of every sphere in the field.
            public override void PickRandomPositions(int width, int height)
            {
                DataLayer.RandomisePozitions(width, height);
            }

            //Switches chosen direction of a sphere to oposite.
            public void SwitchDirections(int id, bool isX)
            {
                DataLayer.SwitchDirectionForSphere(id, isX);
                NotifyObservers(id);
            }

            //Getters for sphere's position.
            public override double GetSphereX(int id)
            {
                return DataLayer.GetSpherePositionX(id);
            }
            public override double GetSphereY(int id)
            {
                return DataLayer.GetSpherePositionY(id);
            }

            //Getter for sphere's radius.
            public override double GetSphereRadius(int id)
            {
                return DataLayer.GetSphereRadius(id);
            }

            //Tells us how many spheres are stored.
            public override int CountSpheres()
            {
                return DataLayer.GetSpheresCount();
            }

            //Checks for border colision of a sphere and, if necesseary, switches one of it's directions.
            private void CheckBoundries(int id)
            {
                double Position_X = DataLayer.GetSpherePositionX(id);
                double Position_Y = DataLayer.GetSpherePositionY(id);
                double[] Directions = DataLayer.GetSphereDirections(id);
                int Radius = DataLayer.GetSphereRadius(id);
                if (Position_X + Radius * 2 + Directions[0] > Size[0] || Position_X + Directions[0] < 0)
                    SwitchDirections(id, true);
                if (Position_Y + Radius * 2 + Directions[1] > Size[1] || Position_Y + Directions[1] < 0)
                    SwitchDirections(id, false);

            }

            //With power of maths finds distance between current sphere and other spheres. If distance is lower or equal
            //to sum of this and other sphere's radians - they collide!
            private void CheckCollisions(int id)
            {
                for (int i = 0; i < DataLayer.GetSpheresCount(); i++)
                {
                    if (i == id) continue;
                    double distance = Math.Sqrt(Math.Pow((DataLayer.GetSpherePositionX(id) + DataLayer.GetSphereDirections(id)[0]) - (DataLayer.GetSpherePositionX(i)
                                    + DataLayer.GetSphereDirections(i)[0]), 2) + Math.Pow((DataLayer.GetSpherePositionY(id) + DataLayer.GetSphereDirections(id)[1])
                                    - (DataLayer.GetSpherePositionY(i) + DataLayer.GetSphereDirections(i)[1]), 2));

                    if (Math.Abs(distance) <= DataLayer.GetSphereRadius(id) + DataLayer.GetSphereRadius(i))
                    {
                        //When collision happens we assign new movement as a result of executing NewMovement() method.
                        double[] newMovement = NewMovement(id, i);
                        DataLayer.SetSphereDirections(id, newMovement[0], newMovement[1]);
                        DataLayer.SetSphereDirections(i, newMovement[2], newMovement[3]);
                    }
                }


            }


            #region observer

            public virtual void Subscribe(IObservable<Sphere> provider)
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

            public override void OnNext(Sphere Sphere)
            {
                //We can succesfully use OnNext method to always creaty entry in the log whenever a ball is about
                //to move.
                logger.AddLog(DataLayer.PrepareLog(Sphere.Id));
                //For every sphere we check for border collision, collision with other balls and then notify observers.
                CheckBoundries(Sphere.Id);
                CheckCollisions(Sphere.Id);
                NotifyObservers(Sphere.Id);
            }

            //When notified, we go with next ball.
            public void NotifyObservers(int id)
            {
                foreach (var observer in observers)
                {
                    if (observer != null)
                    {
                        observer.OnNext(id);
                    }
                }
                System.Threading.Thread.Sleep(1);

            }

            //VERY POWERFULL MATHS for calculating new vector of movement for two spheres that collided. It takes speed, position of spheres and mass into account.
            public double[] NewMovement(int id, int id2)
            {
                double mass = DataLayer.GetSphereMass(id);
                double otherMass = DataLayer.GetSphereMass(id2);

                double[] velocity = new double[2] { DataLayer.GetSphereDirections(id)[0], DataLayer.GetSphereDirections(id)[1] };
                double[] position = new double[2] { DataLayer.GetSpherePositionX(id), DataLayer.GetSpherePositionY(id) };

                double[] velocityOther = new double[2] { DataLayer.GetSphereDirections(id2)[0], DataLayer.GetSphereDirections(id2)[1] };
                double[] positionOther = new double[2] { DataLayer.GetSpherePositionX(id2), DataLayer.GetSpherePositionY(id2) };

                double fDistance = (double)Math.Sqrt((position[0] - positionOther[0]) * (position[0] - positionOther[0])
                       + (position[1] - positionOther[1]) * (position[1] - positionOther[1]));

                double nx = (positionOther[0] - position[0]) / fDistance;
                double ny = (positionOther[1] - position[1]) / fDistance;

                double tx = -ny;
                double ty = nx;

                // Dot Product Tangent
                double dpTan1 = velocity[0] * tx + velocity[1] * ty;
                double dpTan2 = velocityOther[0] * tx + velocityOther[1] * ty;

                // Dot Product Normal
                double dpNorm1 = velocity[0] * nx + velocity[1] * ny;
                double dpNorm2 = velocityOther[0] * nx + velocityOther[1] * ny;

                // Conservation of momentum in 1D
                double m1 = (dpNorm1 * (mass - otherMass) + 2.0f * otherMass * dpNorm2) / (mass + otherMass);
                double m2 = (dpNorm2 * (otherMass - mass) + 2.0f * mass * dpNorm1) / (mass + otherMass);

                double[] newMovements = new double[4]
                {
                    tx * dpTan1 + nx * m1,
                    ty * dpTan1 + ny * m1,
                    tx * dpTan2 + nx * m2,
                    ty * dpTan2 + ny * m2
                };

                return newMovements;
            }

            #endregion

            #region provider

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);
                return new Unsubscriber(observers, observer);
            }

            private class Unsubscriber : IDisposable
            {
                private IList<IObserver<int>> _observers;
                private IObserver<int> _observer;

                public Unsubscriber
                (IList<IObserver<int>> observers, IObserver<int> observer)
                {
                    _observers = observers;
                    _observer = observer;
                }

                public void Dispose()
                {
                    if (_observer != null && _observers.Contains(_observer))
                        _observers.Remove(_observer);
                }
            }

            #endregion
        }
    }
}