using board;
using chess;
using System;
using System.Collections.Generic;

namespace Chess
{
    class Screen
    {
        public static void PrintMatch(ChessMatch match)
        {
            PrintBoard(match.BoardOfMatch);

            Console.WriteLine();

            PrintTakenPieces(match);

            Console.WriteLine();

            Console.WriteLine("Turn: " + match.Turn);
            Console.WriteLine("Waiting For: " + match.ActualPlayer);

            if (!match.Ended)
            {
                if (match.Check)
                {
                    Console.WriteLine(match.ActualPlayer + ": You are under Check!");
                }
            } else
            {
                Console.WriteLine("Checkmate!");
                Console.WriteLine("Winner: " + match.ActualPlayer);
            }
        }

        public static void PrintTakenPieces(ChessMatch match)
        {
            ConsoleColor aux = Console.ForegroundColor;
            Console.WriteLine("Taken Pieces: ");

            Console.Write("White: ");

            Console.ForegroundColor = ConsoleColor.Red;

            PrintArray(match.CapturedPiecesOfTeam(Color.White));

            Console.ForegroundColor = aux;

            Console.Write("Black: ");

            Console.ForegroundColor = ConsoleColor.Cyan;

            PrintArray(match.CapturedPiecesOfTeam(Color.Black));

            Console.ForegroundColor = aux;
        }

        public static void PrintArray(HashSet<Piece> array)
        {
            Console.Write("{");
            foreach(Piece piece in array)
            {
                Console.Write(piece + ",");
            }
            Console.WriteLine("}");
        }

        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    PrintPiece(board.UniquePiece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }

        public static void PrintBoard(Board board, bool[,] possiblePositions)
        {
            ConsoleColor originalBackground = Console.BackgroundColor;
            ConsoleColor newBackground = ConsoleColor.DarkGray;

            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    if (possiblePositions[i, j])
                    {
                        Console.BackgroundColor = newBackground;
                    } else
                    {
                        Console.BackgroundColor = originalBackground;
                    }
                    PrintPiece(board.UniquePiece(i, j));
                    Console.BackgroundColor = originalBackground;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
            Console.BackgroundColor = originalBackground;
        }

        public static ChessPosition ReadChessPosition()
        {
            string s = Console.ReadLine();
            char column = s[0];
            int line = int.Parse(s[1] + "");
            return new ChessPosition(column, line);
        }

        public static void PrintPiece(Piece piece)
        {
            ConsoleColor aux = Console.ForegroundColor;
            if (piece == null)
            {
                Console.Write("- ");
            } else
            {
                if (piece.Color == Color.White)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}
