using board;
using chess;
using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game();
            string gamemode;

            Console.WriteLine("Welcome to Chess");
            Console.WriteLine("Please choose a game mode");
            Console.WriteLine("Select s for standard chess");
            Console.WriteLine("or c for Chess960");
            gamemode = Console.ReadLine();
            
            bool correct = false;
            do {
                switch (gamemode.ToLower())
                {
                    case "s":
                        g.Standard();
                        correct = true;
                        break;
                    case "c":
                        g.Chess960();
                        correct = true;
                        break;
                    default:
                        Console.WriteLine("I'm sorry but that was not a correct input, try again");
                        gamemode = Console.ReadLine();
                        break;
                }
            } while (!correct);
            Console.ReadLine();
        }
    }
}
