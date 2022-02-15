using board;
using chess;
using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChessMatch match = new ChessMatch();

                while (!match.Ended)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintMatch(match);

                        Console.WriteLine();

                        Console.Write("Origin: ");
                        Position origin = Screen.ReadChessPosition().ToPosition();

                        match.ValidateOriginPosition(origin);

                        bool[,] possiblePositions = match.BoardOfMatch.UniquePiece(origin).PossibleMoviments();

                        Console.Clear();
                        Screen.PrintBoard(match.BoardOfMatch, possiblePositions);

                        Console.WriteLine();
                        Console.Write("Destiny: ");
                        Position destiny = Screen.ReadChessPosition().ToPosition();
                        match.ValidateDestinyPosition(origin, destiny);

                        match.PerformPlay(origin, destiny);
                    } catch (BoardException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Screen.PrintMatch(match);
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
