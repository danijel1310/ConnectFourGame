using ConnectFour.Events;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConnectFour.Model
{
    public class ClassicFourConnectGrid : IFourConnectGrid
    {
        private Dictionary<CellID, FourConnectCellModel> _cells;

        public int NumRows
        {
            get { return 6; }
        }

        public int NumCols
        {
            get { return 7; }
        }

        public event EventHandler<CellStatusChangedEventArgs> CellStatusChanged = delegate { };

        public FourConnectCellModel GetCell(CellID cellId)
        {
            return _cells[cellId];
        }

        private CellID keyFor(int row, int col)
        {
            return CellID.Create(row, col);
        }


        public void SetPiece(int row, int col, EConnectFourCellContent content)
        {
            if (IsValidMove(keyFor(row, col)))
            {
                SetPiece(getLowestEmptyRowOfCol(col), content);
            }
        }
        
        public void SetPiece(CellID target, EConnectFourCellContent content)
        {
            EConnectFourCellContent oldContent = _cells[target].Content;

            _cells[target].SetContent(content);
            CellStatusChanged(this, new CellStatusChangedEventArgs(target, content, oldContent));
        }

        public void InitializeBoard()
        {
            _cells = new Dictionary<CellID, FourConnectCellModel>();
            for (int col = 0; col < NumCols; col++)
            {
                for (int row = 0; row < NumRows; row++)
                {
                    _cells.Add(keyFor(row, col), new FourConnectCellModel(row, col));
                }
            }

            clearBoard();
        }

        private void clearBoard()
        {
            for (int col = 0; col < NumCols; col++)
            {
                for (int row = 0; row < NumRows; row++)
                {
                    SetPiece(row, col, EConnectFourCellContent.Empty);
                }
            }
        }

        private CellID getLowestEmptyRowOfCol(int col)
        {
            for (int row = NumRows - 1; row >= 0; row--)
            {
                var cellid = CellID.Create(row, col);
                if (_cells[cellid].Content == EConnectFourCellContent.Empty)
                {
                    return cellid;
                }
            }

            throw new ColFullException(col);
        }

        public bool IsValidMove(CellID target)
        {
            try
            {
                if (getLowestEmptyRowOfCol(target.Col).GetType().Name == "CellID")
                {
                    return true;
                }
            }
            catch (ColFullException ex)
            {
                return false;
            }

            return false;
        }
    }

    [Serializable]
    internal class ColFullException : Exception
    {
        private int _col;

        public ColFullException()
        {
        }

        public ColFullException(int col)
        {
            this._col = col;
        }

        public ColFullException(string message) : base(message)
        {
        }

        public ColFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ColFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}