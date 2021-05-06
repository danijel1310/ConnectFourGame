using System;
using ConnectFour.Events;

namespace ConnectFour.Model
{
    public interface IFourConnectGrid
    {
        int NumRows { get; }

        int NumCols { get; }

        event EventHandler<CellStatusChangedEventArgs> CellStatusChanged;

        void InitializeBoard();

        FourConnectCellModel GetCell(CellID cellId);

        bool IsValidMove(CellID target);

        void SetPiece(CellID target, EConnectFourCellContent content);

        void SetPiece(int row, int col, EConnectFourCellContent content);
    }
}