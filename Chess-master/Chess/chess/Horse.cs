using board;

namespace chess
{
    class Horse : Piece
    {
        public Horse(Color color, Board board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "H";
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

            pos.DefineValues(Position.Line - 1, Position.Column - 2);
            if(Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line - 2, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line - 2, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line - 1, Position.Column + 2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line + 1, Position.Column + 2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line + 2, Position.Column + 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line + 2, Position.Column - 1);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            pos.DefineValues(Position.Line + 1, Position.Column - 2);
            if (Board.IsPositionValid(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }
            return mat;
        }
    }
}
