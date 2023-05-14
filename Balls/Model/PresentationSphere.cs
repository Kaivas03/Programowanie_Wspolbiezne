using Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    //This class copes with visual representation of spheres.
    public class PresentationSphere : ISphere
    {
        //Meant for string representation of color we pick.
        public string Color { get; set; }
        public double Diameter { get; }
        public int Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private double top;
        private double left;
        public double Top
        {
            get { return top; }
            set
            {
                if (top == value) return;
                top = value;
                RaisePropertyChanged();
            }
        }
        public double Left
        {
            get { return left; }
            set
            {
                if (left == value) return;
                left = value;
                RaisePropertyChanged();
            }
        }



        //Constructor for visual representation of the sphere. Default color is 'red'.
        public PresentationSphere(int id, double top, double left, double radius, string c = "red")
        {
            this.Id = id;
            this.Color = c;
            this.top = top;
            this.left = left;
            this.Diameter = 2 * radius;
        }

        public void Move(double poitionX, double positionY)
        {
            Left = poitionX;
            Top = positionY;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}