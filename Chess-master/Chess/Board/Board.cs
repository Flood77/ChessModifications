namespace board
{
    class Board
    {
        public int Lines { get; set; }
        public int Columns { get; set; }
        public Piece[,] Piece;

        public Board(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Piece = new Piece[lines, columns];
        }

        public Piece UniquePiece(int line, int column)
        {
            return Piece[line, column];
        }

        public Piece UniquePiece(Position pos)
        {
            return Piece[pos.Line, pos.Column];
        }

        public bool IsTherePiece(Position pos)
        {
            ValidatePosition(pos);
            return UniquePiece(pos) != null;
        }

        public void PutPiece(Piece p, Position pos)
        {
            if (IsTherePiece(pos))
            {
                throw new BoardException("[ERROR] Already existis a piece on this position");
            }
            Piece[pos.Line, pos.Column] = p;
            p.Position = pos;
        }

        public Piece RemovePiece(Position pos)
        {
            if (UniquePiece(pos) == null)
            {
                return null;
            }
            Piece aux = UniquePiece(pos);
            aux.Position = null;
            Piece[pos.Line, pos.Column] = null;

            return aux;
        }

        public bool IsPositionValid(Position pos)
        {
            if (pos.Line < 0 || pos.Line >= Lines || pos.Column < 0 || pos.Column >= Columns)
            {
                return false;
            }

            return true;
        }

        public void ValidatePosition(Position pos)
        {
            if (!IsPositionValid(pos))
            {
                throw new BoardException("[ERROR] Invalid Position");
            }
        }
    }
}
