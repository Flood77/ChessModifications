using board;
using System;
using System.Collections.Generic;

namespace chess
{
    class ChessMatch
    {
        public Board BoardOfMatch { get; private set; }
        public int Turn { get; private set; }
        public Color ActualPlayer { get; private set; }
        public bool Ended { get; private set; }
        public bool Check { get; private set; }
        public Piece VulnerableEnPassant { get; private set; }

        private HashSet<Piece> Pieces;
        private HashSet<Piece> CapturedPieces;

        public ChessMatch(int mode = 0)
        {
            BoardOfMatch = new Board(8, 8);
            Turn = 1;
            ActualPlayer = Color.White;
            Ended = false;
            Check = false;
            VulnerableEnPassant = null;
            Pieces = new HashSet<Piece>();
            CapturedPieces = new HashSet<Piece>();
            if(mode == 0)
            {
                PutPieces();
            }
            else if(mode == 1)
            {
                PutPiecesSpecial();
            }
        }

        public Piece ExecuteMoviment(Position origin, Position destiny)
        {
            Piece p = BoardOfMatch.RemovePiece(origin);
            p.IncrementManyMoves();
            Piece takenPiece = BoardOfMatch.RemovePiece(destiny);
            BoardOfMatch.PutPiece(p, destiny);
            if(takenPiece != null)
            {
                CapturedPieces.Add(takenPiece);
            }

            // #SpecialPlay

            // Castle Kingside
            if(p is King && destiny.Column == origin.Column + 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column + 3);
                Position destinyOfRook = new Position(origin.Line, origin.Column + 1);

                Piece rook = BoardOfMatch.RemovePiece(originOfRook);
                rook.IncrementManyMoves();
                BoardOfMatch.PutPiece(rook, destinyOfRook);
            }

            // Castle Queenside
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column - 4);
                Position destinyOfRook = new Position(origin.Line, origin.Column - 1);

                Piece rook = BoardOfMatch.RemovePiece(originOfRook);
                rook.IncrementManyMoves();
                BoardOfMatch.PutPiece(rook, destinyOfRook);
            }

            // EnPassant
            if(p is Pawn)
            {
                if (origin.Column != destiny.Column && takenPiece == null)
                {
                    Position posPawn;
                    if(p.Color == Color.White)
                    {
                        posPawn = new Position(destiny.Line + 1, destiny.Column);
                    } else
                    {
                        posPawn = new Position(destiny.Line - 1, destiny.Column);
                    }
                    takenPiece = BoardOfMatch.RemovePiece(posPawn);
                    CapturedPieces.Add(takenPiece);
                }
            }
            return takenPiece;
        }

        public void UndoMoviment(Position origin, Position destiny, Piece takenPiece)
        {
            Piece p = BoardOfMatch.RemovePiece(destiny);
            p.DecrementManyMoves();
            if (takenPiece != null)
            {
                BoardOfMatch.PutPiece(takenPiece, destiny);
                CapturedPieces.Remove(takenPiece);
            }
            BoardOfMatch.PutPiece(p, origin);


            // #SpecialMove
            // Castle Kingside
            if (p is King && destiny.Column == origin.Column + 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column + 3);
                Position destinyOfRook = new Position(origin.Line, origin.Column + 1);

                Piece rook = BoardOfMatch.RemovePiece(destinyOfRook);
                rook.DecrementManyMoves();
                BoardOfMatch.PutPiece(rook, originOfRook);
            }

            // Castle Queenside
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column - 4);
                Position destinyOfRook = new Position(origin.Line, origin.Column - 1);

                Piece rook = BoardOfMatch.RemovePiece(destinyOfRook);
                rook.IncrementManyMoves();
                BoardOfMatch.PutPiece(rook, originOfRook);
            }

            // EnPassant
            if (p is Pawn)
            {
                if (origin.Column != destiny.Column && takenPiece == VulnerableEnPassant)
                {
                    Piece pawn = BoardOfMatch.RemovePiece(destiny);
                    Position posPawn;
                    if (p.Color == Color.White)
                    {
                        posPawn = new Position(3, destiny.Column);
                    }
                    else
                    {
                        posPawn = new Position(4, destiny.Column);
                    }
                    BoardOfMatch.PutPiece(pawn, posPawn);
                }
            }
        }

        public void PerformPlay(Position origin, Position destiny)
        {
            Piece takenPiece = ExecuteMoviment(origin, destiny);

            if(IsTheKingInCheck(ActualPlayer))
            {
                UndoMoviment(origin, destiny, takenPiece);
                throw new BoardException("[ERROR] Can't put yourself in check");
            }

            Piece p = BoardOfMatch.UniquePiece(destiny);

            // #SpecialPlay upgrade
            if (p is Pawn)
            {
                if((p.Color == Color.White && destiny.Line == 0) || (p.Color == Color.Black && destiny.Line == 7))
                {
                    p = BoardOfMatch.RemovePiece(destiny);
                    Pieces.Remove(p);

                    Piece queen = new Queen(p.Color, BoardOfMatch);
                    BoardOfMatch.PutPiece(queen, destiny);

                    Pieces.Add(queen);
                }
            }

            if (IsTheKingInCheck(AdversaryColor(ActualPlayer))) {
                Check = true;
            } else
            {
                Check = false;
            }

            if (IsCheckmate(AdversaryColor(ActualPlayer)))
            {
                Ended = true;
            } else
            {
                Turn++;
                ChangePlayer();
            }

            // #SpecialPlay
            // EnPassant
            if(p is Pawn && (destiny.Line == origin.Line - 2 || destiny.Line == origin.Line + 2))
            {
                VulnerableEnPassant = p;
            } else
            {
                VulnerableEnPassant = null;
            }
        }

        public void ValidateOriginPosition(Position pos)
        {
            if (BoardOfMatch.UniquePiece(pos) == null)
            {
                throw new BoardException("[ERROR] There isn't any piece in this position");
            }
            if (ActualPlayer != BoardOfMatch.UniquePiece(pos).Color)
            {
                throw new BoardException("[ERROR] Please choose a piece of the right team");
            }
            if (!BoardOfMatch.UniquePiece(pos).IsTherePossibleMoves())
            {
                throw new BoardException("[ERROR] There isn't any possible moves for that piece");
            }
        }

        public void ValidateDestinyPosition(Position origin, Position destiny)
        {
            if (!BoardOfMatch.UniquePiece(origin).PossibleMove(destiny))
            {
                throw new BoardException("[ERROR] Invalid destiny position");
            }
        }

        private void ChangePlayer()
        {
            if (ActualPlayer == Color.White)
            {
                ActualPlayer = Color.Black;
            } else {
                ActualPlayer = Color.White;
            }
        }
        
        public HashSet<Piece> CapturedPiecesOfTeam (Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();

            foreach(Piece piece in CapturedPieces)
            {
                if(piece.Color == color)
                {
                    aux.Add(piece);
                }
            }

            return aux;
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();

            foreach (Piece piece in Pieces)
            {
                if (piece.Color == color)
                {
                    aux.Add(piece);
                }
            }

            aux.ExceptWith(CapturedPiecesOfTeam(color));

            return aux;
        }

        private Color AdversaryColor(Color color)
        {
            if(color == Color.White)
            {
                return Color.Black;
            } else
            {
                return Color.White;
            }
        }

        private Piece IfKingExisits(Color color)
        {
            foreach(Piece piece in InGamePieces(color))
            {
                if (piece is King)
                {
                    return piece;
                }
            }
            return null;
        }

        public bool IsTheKingInCheck(Color color)
        {
            Piece king = IfKingExisits(color);
            if (king == null)
            {
                throw new BoardException("[ERROR] King of color: " + color + " not found");
            }
            foreach(Piece piece in InGamePieces(AdversaryColor(color)))
            {
                bool[,] mat = piece.PossibleMoviments();
                if (mat[king.Position.Line, king.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsCheckmate(Color color)
        {
            if (!IsTheKingInCheck(color))
            {
                return false;
            }
            foreach(Piece piece in InGamePieces(color))
            {
                bool[,] mat = piece.PossibleMoviments();
                for(int i = 0; i < BoardOfMatch.Lines; i++)
                {
                    for(int j = 0; j < BoardOfMatch.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = piece.Position;
                            Position destiny = new Position(i, j);
                            Piece takenPiece = ExecuteMoviment(origin, destiny);
                            bool isInCheck = IsTheKingInCheck(color);
                            UndoMoviment(origin, destiny, takenPiece);
                            if (!isInCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void PutNewPiece(char column, int line, Piece piece)
        {
            BoardOfMatch.PutPiece(piece, new ChessPosition(column, line).ToPosition());
            Pieces.Add(piece);
        }

        private void PutPieces()
        {
            PutNewPiece('a', 1, new Rook(Color.White, BoardOfMatch));
            PutNewPiece('b', 1, new Horse(Color.White, BoardOfMatch));
            PutNewPiece('c', 1, new Bishop(Color.White, BoardOfMatch));
            PutNewPiece('d', 1, new Queen(Color.White, BoardOfMatch));
            PutNewPiece('e', 1, new King(Color.White, BoardOfMatch, this));
            PutNewPiece('f', 1, new Bishop(Color.White, BoardOfMatch));
            PutNewPiece('g', 1, new Horse(Color.White, BoardOfMatch));
            PutNewPiece('h', 1, new Rook(Color.White, BoardOfMatch));
            PutNewPiece('a', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('b', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('c', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('d', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('e', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('f', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('g', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('h', 2, new Pawn(Color.White, BoardOfMatch, this));

            PutNewPiece('a', 8, new Rook(Color.Black, BoardOfMatch));
            PutNewPiece('b', 8, new Horse(Color.Black, BoardOfMatch));
            PutNewPiece('c', 8, new Bishop(Color.Black, BoardOfMatch));
            PutNewPiece('d', 8, new Queen(Color.Black, BoardOfMatch));
            PutNewPiece('e', 8, new King(Color.Black, BoardOfMatch, this));
            PutNewPiece('f', 8, new Bishop(Color.Black, BoardOfMatch));
            PutNewPiece('g', 8, new Horse(Color.Black, BoardOfMatch));
            PutNewPiece('h', 8, new Rook(Color.Black, BoardOfMatch));
            PutNewPiece('a', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('b', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('c', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('d', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('e', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('f', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('g', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('h', 7, new Pawn(Color.Black, BoardOfMatch, this));
        }

        private void PutPiecesSpecial()
        {
            List<char> whiteSpaces = new List<char>() { 'a','b','c','d','e','f','g','h' };
            List<char> blackSpaces = new List<char>() { 'a','b','c','d','e','f','g','h' };
            Random rand = new Random();
            int place = 0;

            //place black space bishop
            place = rand.Next(4) * 2;
            PutNewPiece(whiteSpaces[place], 1, new Bishop(Color.White, BoardOfMatch));
            whiteSpaces.RemoveAt(place);

            //place white space bishop
            place = rand.Next(4);
            place = rand.Next(4);
            char placement = ' ';
            switch (place)
            {
                case 0:
                    placement = 'b';
                    break;
                case 1:
                    placement = 'd';
                    break;
                case 2:
                    placement = 'f';
                    break;
                case 3:
                    placement = 'h';
                    break;
            }
            PutNewPiece(placement, 1, new Bishop(Color.White, BoardOfMatch));
            whiteSpaces.Remove(placement);

            //place Knights Randomly
            place = rand.Next(whiteSpaces.Count);
            PutNewPiece(whiteSpaces[place], 1, new Horse(Color.White, BoardOfMatch));
            whiteSpaces.RemoveAt(place);
            place = rand.Next(whiteSpaces.Count);
            PutNewPiece(whiteSpaces[place], 1, new Horse(Color.White, BoardOfMatch));
            whiteSpaces.RemoveAt(place);

            //place Queen Randomly
            place = rand.Next(whiteSpaces.Count);
            PutNewPiece(whiteSpaces[place], 1, new Queen(Color.White, BoardOfMatch));
            whiteSpaces.RemoveAt(place);

            //place King and Rooks based on remaining spots
            PutNewPiece(whiteSpaces[0], 1, new Rook(Color.White, BoardOfMatch));
            PutNewPiece(whiteSpaces[1], 1, new King(Color.White, BoardOfMatch, this));
            PutNewPiece(whiteSpaces[2], 1, new Rook(Color.White, BoardOfMatch));

            //place white pawns
            PutNewPiece('a', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('b', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('c', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('d', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('e', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('f', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('g', 2, new Pawn(Color.White, BoardOfMatch, this));
            PutNewPiece('h', 2, new Pawn(Color.White, BoardOfMatch, this));

            //place white space bishop
            place = rand.Next(4) * 2;
            PutNewPiece(blackSpaces[place], 8, new Bishop(Color.Black, BoardOfMatch));
            blackSpaces.RemoveAt(place);

            //place black space bishop
            place = rand.Next(4);
            place = rand.Next(4);
            switch (place)
            {
                case 0:
                    placement = 'b';
                    break;
                case 1:
                    placement = 'd';
                    break;
                case 2:
                    placement = 'f';
                    break;
                case 3:
                    placement = 'h';
                    break;
            }
            PutNewPiece(placement, 8, new Bishop(Color.Black, BoardOfMatch));
            blackSpaces.Remove(placement);

            //place Knights Randomly
            place = rand.Next(blackSpaces.Count);
            PutNewPiece(blackSpaces[place], 8, new Horse(Color.Black, BoardOfMatch));
            blackSpaces.RemoveAt(place);
            place = rand.Next(blackSpaces.Count);
            PutNewPiece(blackSpaces[place], 8, new Horse(Color.Black, BoardOfMatch));
            blackSpaces.RemoveAt(place);

            //place Queen Randomly
            place = rand.Next(blackSpaces.Count);
            PutNewPiece(blackSpaces[place], 8, new Queen(Color.Black, BoardOfMatch));
            blackSpaces.RemoveAt(place);

            //place King and Rooks based on remaining spots
            PutNewPiece(blackSpaces[0], 8, new King(Color.Black, BoardOfMatch, this));
            PutNewPiece(blackSpaces[1], 8, new Rook(Color.Black, BoardOfMatch));
            PutNewPiece(blackSpaces[2], 8, new Rook(Color.Black, BoardOfMatch));

            //place black pawns
            PutNewPiece('a', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('b', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('c', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('d', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('e', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('f', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('g', 7, new Pawn(Color.Black, BoardOfMatch, this));
            PutNewPiece('h', 7, new Pawn(Color.Black, BoardOfMatch, this));
        }
    }
}
