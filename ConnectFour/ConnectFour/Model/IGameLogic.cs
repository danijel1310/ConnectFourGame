using System;
using ConnectFour.Events;

namespace ConnectFour.Model
{
    public enum EActivePlayer
    {
        White,
        Black
    };

    public interface IGameLogic
    {
        int NumRows { get; }

        int NumCols { get; }

        bool IsValidMove(CellID target);

        void SetPiece(CellID target);

        void StartGame();

        void SurrenderGame();

        EActivePlayer ActivePlayer { get; }

        event EventHandler<CellStatusChangedEventArgs> CellStatusChanged;

        event EventHandler<GameEndedEventArgs> GameFinished;
    }
}