using board;

namespace chess
{
    class Bishop : Piece
    {
        public Bishop(Color color, Board board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "B";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.UniquePiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] PossibleMoviments()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];
            Position pos = new Position(0, 0);

            // Up + Left
            pos.DefineValues(Position.Line - 1, Position.Column - 1);
            while(Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line - 1, pos.Column - 1);
            }

            // Up + Right
            pos.DefineValues(Position.Line - 1, Position.Column + 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line - 1, pos.Column + 1);
            }

            // Down + Right
            pos.DefineValues(Position.Line + 1, Position.Column + 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line + 1, pos.Column + 1);
            }

            // Down + Left
            pos.DefineValues(Position.Line + 1, Position.Column - 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line + 1, pos.Column - 1);
            }

            return mat;

        }
    }
}
