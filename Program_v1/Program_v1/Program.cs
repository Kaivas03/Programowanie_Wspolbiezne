// See https://aka.ms/new-console-template for more information

public class Calculator
{
    static float addition(float a, float b)
    {
        return a + b;
    }

    static float subtraction(float a, float b)
    {
        return a - b;
    }

    static float multiplication(float a, float b)
    {
        return a * b;
    }

    static float division(float a, float b)
    {
        return a / b;
    }

    static bool isZero(float a)
    {
        if (a == 0) return true;
        else return false;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Co chcesz zrobic?\n1. Dodawanie\n2. Odejmowanie\n3. Mnozenie\n4. Dzielenie\n");

        string choice = Console.ReadLine();
        Console.WriteLine("Podaj pierwsza liczbe: ");
        float first = float.Parse(Console.ReadLine());
        Console.WriteLine("Podaj druga liczbe: ");
        float second = float.Parse(Console.ReadLine());

        switch (choice)
        {
            case "1":
                Console.WriteLine("Dzialanie: " + first + " + " + second + "\nTwoj wynik to: " + addition(first, second));
                break;
            case "2":
                Console.WriteLine("Dzialanie: " + first + " - " + second + "\nTwoj wynik to: " + subtraction(first, second));
                break;
            case "3":
                Console.WriteLine("Dzialanie: " + first + " * " + second + "\nTwoj wynik to: " + multiplication(first, second));
                break;
            case "4":
                if (isZero(second) == true)
                {
                    Console.WriteLine("Nie mozna dzielic przez zero!\nKoncze program.");
                    break;
                }
                Console.WriteLine("Dzialanie: " + first + " / " + second + "\nTwoj wynik to: " + division(first, second));
                break;
            default:
                Console.WriteLine("Zly wybor.\nKoncze program.");
                break;
        }
    }
}


