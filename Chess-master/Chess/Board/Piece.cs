namespace board
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int ManyMoves { get; protected set; }
        public Board Board { get; protected set; }

        public Piece()
        {

        }

        public Piece(Color color, Board board)
        {
            Position = null;
            Board = board;
            Color = color;
            ManyMoves = 0;
        }

        public void IncrementManyMoves()
        {
            ManyMoves++;
        }

        public void DecrementManyMoves()
        {
            ManyMoves--;
        }
        
        public bool IsTherePossibleMoves()
        {
            bool[,] mat = PossibleMoviments();
            for(int i = 0; i < Board.Lines; i++)
            {
                for(int j = 0; j < Board.Columns; j++)
                {
                    if(mat[i, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool PossibleMove(Position pos)
        {
            return PossibleMoviments()[pos.Line, pos.Column];
        }

        public abstract bool[,] PossibleMoviments();
    }
}
