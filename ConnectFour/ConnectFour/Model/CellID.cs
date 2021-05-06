namespace ConnectFour.Model
{
    public class CellID
    {
        private int _row;
        public int Row
        {
            get { return _row; }
        }

        private int _col;
        public int Col
        {
            get { return _col; }
        }

        private CellID(int row, int col)
        {
            _row = row;
            _col = col;
        }
        public static CellID Create(int row, int col)
        {
            return new CellID(row, col);
        }

        public override bool Equals(object other)
        {
            var otherCasted = other as CellID;
            if (otherCasted == null)
            {
                return false;
            }
            return otherCasted.Row == this.Row && otherCasted.Col == this.Col;
        }

        public override int GetHashCode()
        {
            return ("" + Row + Col).GetHashCode(); // Create a string and re-use the HashCode
        }
    }
}