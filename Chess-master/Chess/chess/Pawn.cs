using board;
using System.Linq.Expressions;

namespace chess
{
    class Pawn : Piece
    {
        private ChessMatch Match;

        public Pawn(Color color, Board board, ChessMatch match) : base(color, board)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool IsThereEnemy(Position position)
        {
            Piece p = Board.UniquePiece(position);
            return p != null && p.Color != Color;
        }

        private bool Free(Position position)
        {
            return Board.UniquePiece(position) == null;
        }

        public override bool[,] PossibleMoviments()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];
            Position pos = new Position(0, 0);

            if (Color == Color.White)
            {
                pos.DefineValues(Position.Line - 1, Position.Column);
                if (Board.IsPositionValid(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 2, Position.Column);
                Position p2 = new Position(Position.Line - 1, Position.Column);
                if (Board.IsPositionValid(p2) && Free(p2)&& Board.IsPositionValid(pos) && Free(pos) && ManyMoves == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 1, Position.Column - 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 1, Position.Column + 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                // #SpecialPlay
                // EnPassant
                if (Position.Line == 3)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.IsPositionValid(left) && IsThereEnemy(left) && Board.UniquePiece(left) == Match.VulnerableEnPassant)
                    {
                        mat[left.Line - 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.IsPositionValid(right) && IsThereEnemy(right) && Board.UniquePiece(right) == Match.VulnerableEnPassant)
                    {
                        mat[right.Line - 1, right.Column] = true;
                    }
                }
            }
            else
            {
                pos.DefineValues(Position.Line + 1, Position.Column);
                if (Board.IsPositionValid(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                Position p2 = new Position(Position.Line + 1, Position.Column);
                pos.DefineValues(Position.Line + 2, Position.Column);
                if (Board.IsPositionValid(p2) && Free(p2) && Board.IsPositionValid(pos) && Free(pos) && ManyMoves == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 1, Position.Column + 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 1, Position.Column - 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                // #SpecialPlay
                // EnPassant
                if (Position.Line == 4)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.IsPositionValid(left) && IsThereEnemy(left) && Board.UniquePiece(left) == Match.VulnerableEnPassant)
                    {
                        mat[left.Line + 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.IsPositionValid(right) && IsThereEnemy(right) && Board.UniquePiece(right) == Match.VulnerableEnPassant)
                    {
                        mat[right.Line + 1, right.Column] = true;
                    }
                }

            }
            return mat;
        }

    }
}
