// See https://aka.ms/new-console-template for more information

float addition(float a, float b)
{
    return a + b;
}

float subtraction(float a, float b)
{ 
    return a - b;
}

float multiplication(float a, float b)
{
    return a * b;
}

float division(float a, float b)
{
    return a / b;
}

bool isZero(float a)
{
    if (a == 0) return true;
    else return false;
}

int main()
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
            return 0;
        case "2":
            Console.WriteLine("Dzialanie: " + first + " - " + second + "\nTwoj wynik to: " + subtraction(first, second));
            return 0;
        case "3":
            Console.WriteLine("Dzialanie: " + first + " * " + second + "\nTwoj wynik to: " + multiplication(first, second));
            return 0;
        case "4":
            if (isZero(second) == true)
            {
                Console.WriteLine("Nie mozna dzielic przez zero!\nKoncze program.");
                return 1;
            }
            Console.WriteLine("Dzialanie: " + first + " / " + second + "\nTwoj wynik to: " + division(first, second));
            return 0;
        default:
            Console.WriteLine("Zly wybor.\nKoncze program.");
            return 1;
    }
}

main();

