using System.Numerics;
using System.Transactions;

namespace DGD203_BurakBisneli_Midterm;

public static class Tools
{
    
    public static int GetChoice(string question, string[] options)
    {
        Console.WriteLine("---------------------------------------");
        Console.WriteLine(question);
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.WriteLine("---------------------------------------");
        int choice = int.Parse(Console.ReadLine() ?? string.Empty);
        return choice;
    }
    
    public static void WaitSeconds(int seconds)
    {
        Thread.Sleep(seconds * 1000); // I used ChatGPT for this line. It forces the program wait.
    }

    public static void LoadingScreen(int lenght)
    {
        Console.Write("[");
        
        Console.Write(new string(' ', lenght)); // I used ChatGPT for this 3 line. I didn't know how to make some space and write "]" this. 
        Console.Write("]");
        Console.SetCursorPosition(1, Console.CursorTop);
        
        for (int i = 0; i < lenght; i++)
        {
            Console.Write("=");
            Thread.Sleep(50);
        }
        Console.WriteLine();
    }
    
}