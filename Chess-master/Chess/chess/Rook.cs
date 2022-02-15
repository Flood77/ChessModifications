using board;

namespace chess
{
    class Rook : Piece
    {
        public Rook(Color color, Board board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "R";
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

            // Up
            pos.DefineValues(Position.Line - 1, Position.Column);
            while(Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.Line -= 1;
            }

            // Down
            pos.DefineValues(Position.Line + 1, Position.Column);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.Line += 1;
            }

            // Right
            pos.DefineValues(Position.Line, Position.Column + 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column += 1;
            }

            // Left
            pos.DefineValues(Position.Line, Position.Column - 1);
            while (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.UniquePiece(pos) != null && Board.UniquePiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column -= 1;
            }

            return mat;
        }
    }
}
