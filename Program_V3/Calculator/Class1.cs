namespace Calculator
{
    public class Calculator
    {
        public float Addition(float a, float b)
        {
            return a + b;
        }

        public float Subtraction(float a, float b)
        {
            return a - b;
        }

        public float Multiplication(float a, float c)
        {
            return a * c;
        }

        public bool IsZero(float a)
        {
            if (a == 0) return true;
            else return false;
        }

        public float Division(float a, float b)
        {
            if (!IsZero(b)) return a / b;
            else return 1;
        }

    }
}