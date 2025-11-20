namespace SmartChess.Models.Chess
{
    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Position FromChessNotation(string notation)
        {
            char file = notation[0];
            char rank = notation[1];
            int x = file - 'a';
            int y = rank - '1';
            return new Position(x, y);
        }

        public string ToChessNotation()
        {
            return $"{(char)('a' + X)}{(char)('1' + Y)}";
        }

        public bool Equals(Position other) => X == other.X && Y == other.Y;
        public override bool Equals(object? obj) => obj is Position other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Position left, Position right) => left.Equals(right);
        public static bool operator !=(Position left, Position right) => !left.Equals(right);
        public override string ToString() => ToChessNotation();
    }
}