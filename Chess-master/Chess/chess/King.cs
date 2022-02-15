using board;

namespace chess
{
    class King : Piece
    {
        private ChessMatch Match;

        public King(Color color, Board board, ChessMatch match) : base(color, board)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "K";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.UniquePiece(pos);
            return p == null || p.Color != Color;
        }

        private bool testRookToCastle(Position pos)
        {
            Piece p = Board.UniquePiece(pos);
            return p != null && p is Rook && p.Color == Color && ManyMoves == 0;
        }

        public override bool[,] PossibleMoviments()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];

            Position pos = new Position(0, 0);

            // Up
            pos.DefineValues(Position.Line - 1, Position.Column);
            if(Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Up + Right
            pos.DefineValues(Position.Line - 1, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Right
            pos.DefineValues(Position.Line, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Right + Down
            pos.DefineValues(Position.Line + 1, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Down
            pos.DefineValues(Position.Line + 1, Position.Column);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Down + Left
            pos.DefineValues(Position.Line + 1, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Left
            pos.DefineValues(Position.Line, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Left + Up
            pos.DefineValues(Position.Line - 1, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // #SpecialPlay
            if (ManyMoves == 0 && !Match.Check)
            {
                // Castle Kingside - roque pequeno
                Position positionOfRook = new Position(Position.Line, Position.Column + 3);
                if(testRookToCastle(positionOfRook))
                {
                    Position p1 = new Position(Position.Line, Position.Column + 1);
                    Position p2 = new Position(Position.Line, Position.Column + 2);

                    if (Board.UniquePiece(p1) == null && Board.UniquePiece(p2) == null)
                    {
                        mat[Position.Line, Position.Column + 2] = true;
                    }
                    {

                    }
                }

                // Castle Queenside - roque grande
                Position positionOfRookQueenSide = new Position(Position.Line, Position.Column - 4);
                if (testRookToCastle(positionOfRookQueenSide))
                {
                    Position p1 = new Position(Position.Line, Position.Column - 1);
                    Position p2 = new Position(Position.Line, Position.Column - 2);
                    Position p3 = new Position(Position.Line, Position.Column - 3);

                    if (Board.UniquePiece(p1) == null && Board.UniquePiece(p2) == null && Board.UniquePiece(p3) == null)
                    {
                        mat[Position.Line, Position.Column - 2] = true;
                    }
                    {

                    }
                }
            }

            return mat;
        }
    }
}
