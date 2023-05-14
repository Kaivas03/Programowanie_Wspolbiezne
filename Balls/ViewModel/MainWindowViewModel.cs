using Model;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class MainWindowViewModel : ViewModelBase

    {
        //PRIVATE
        //Connection with Model layer.
        private readonly ModelAPI ModelLayer;
        //Wil help us determine when the simulation is started and decide what actions we can take (start/stop).
        private bool isReadyToBegin = false;
        private bool didntStartYet = true;

        //Content of the box represented by string.
        private string box_content;
        //We have two concurrent tasks and twoe threads to deal with them.

        //PUBLIC

        //Collection of observable spheres.
        public ObservableCollection<ISphere> Spheres { get; set; }

        //ICommand for all buttons the user can interact with.
        public ICommand PrepareSimulationAction { get; set; }
        public ICommand BeginSimulationAction { get; set; }

        //How many spheres will be placed.
        public int HowMany { get; set; }

        ////This property works with box_content field. It hands ofer that fields value and allows us to modify it and notify about it being changed.
        public string TextBox
        {
            get { return box_content; }
            set { box_content = value; RaisePropertyChanged("TextBox"); }
        }

        //This property works with notStarted field. It hands ofer that fields value and allows us to modify it and notify about it being changed.
        public bool IsReadyToBegin
        {
            get { return isReadyToBegin; }
            set { isReadyToBegin = value; RaisePropertyChanged("isReadyToBegin"); }
        }

        //While this makes it impossible to restart simulation after it started.
        public bool DidntStartYet
        {
            get { return didntStartYet; }
            set { didntStartYet = value; RaisePropertyChanged("didntStartYet"); }
        }

        //Simply checks out what we have in the text box and makes sure, that the simulation won't start as long, as we don't have
        //appropriate number in there.
        public int ReadBox()
        {   
            //Try parse returns true if parsed text is indeed an integer. We also check if we are bellow the upper boundry.
            if (Int32.TryParse(box_content, out int value) && Int32.Parse(TextBox) <= 20 && Int32.Parse(TextBox) > 0)
            {
                return Int32.Parse(TextBox);
            }
            else
            {
                //Return 0 when conditions are not met. 0 shows us, that wrong value has been inserted, hence we won't move forwartd as long as it stands.
                return 0;
            }
        }

        //Class constructor.
        public MainWindowViewModel()
        {
            PrepareSimulationAction = new RelayCommand(() => PrepareSimulation());
            BeginSimulationAction = new RelayCommand(() => BeginSimulation());

            //We create new instance of model api and assign it as this object's Model layer representation.
            ModelLayer = ModelAPI.CreateApi();
            Spheres = new ObservableCollection<ISphere>();
            IDisposable observer = ModelLayer.Subscribe<ISphere>(x => Spheres.Add(x));
            for (int i = 0; i < ModelLayer.PresentedSpheres.Count; i++)
            {
                Spheres.Add(ModelLayer.PresentedSpheres[i]);
            }
            //We assign relay commands and tell them which methods shall be activated in case of user interacting with one of the buttons.
        }

        private void BeginSimulation()
        {
            //We finally begin the simulation.
            ModelLayer.MoveSpheres();
            IsReadyToBegin = false;
        }

        //What happens when user clicks START button.
        private void PrepareSimulation()
        {
            //We check out how many balls are required.
            int howMany = ReadBox();
            //The ReadBox method will return 0 if something is wrong with value placed in the box. Only if the value is right can we
            //start the simulation.
            if (howMany != 0)
            {
                //We create required amount of spheres in our model layer.
                ModelLayer.AddNewSpheres(howMany);
                //We get rid of current content and refill the collection anew.
                Spheres.Clear();
                for (int i = 0; i < ModelLayer.PresentedSpheres.Count; i++)
                {
                    Spheres.Add(ModelLayer.PresentedSpheres[i]);
                }
                //And we let sombody start the simulation.
                ModelLayer.PickRandomPositions(350, 350);
                IsReadyToBegin = true;
                DidntStartYet = false;
            }
        }

    }
}