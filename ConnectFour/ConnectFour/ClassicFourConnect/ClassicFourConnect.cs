using ConnectFour.Events;
using System;

namespace ConnectFour.Model
{
    public class ClassicFourConnect : IGameLogic
    {
        private EActivePlayer _activePlayer = EActivePlayer.White;
        private IFourConnectGrid _board;

        public event EventHandler<CellStatusChangedEventArgs> CellStatusChanged = delegate { };
        public event EventHandler<GameEndedEventArgs> GameFinished = delegate { };

        public ClassicFourConnect(IFourConnectGrid board)
        {
            _board = board;
            _board.CellStatusChanged += _chessboard_CellStatusChanged;
        }
        private void _chessboard_CellStatusChanged(object sender, CellStatusChangedEventArgs e)
        {
            CellStatusChanged(this, e);
        }

        public int NumRows
        {
            get { return _board.NumRows; }
        }

        public int NumCols
        {
            get { return _board.NumCols; }
        }

        public EActivePlayer ActivePlayer {
            get { return _activePlayer; }
        }


        public bool IsValidMove(CellID target)
        {
            return _board.IsValidMove(target);
        }

        public void StartGame()
        {
            _board.InitializeBoard();
            _activePlayer = EActivePlayer.White;
        }

        public void SurrenderGame()
        {
            if (ActivePlayer == EActivePlayer.Black)
            {
                GameFinished(this, new GameEndedEventArgs(EActivePlayer.White));
            }else if (ActivePlayer == EActivePlayer.White)
            {
                GameFinished(this, new GameEndedEventArgs(EActivePlayer.Black));
            }
        }

        public void SetPiece(CellID target)
        {
            if (!IsValidMove(target))
            {
                throw new InvalidOperationException("Not possible");
            }

            var targetCell = _board.GetCell(target);

            //danijels logic

            nextTurn();
        }

        private void nextTurn()
        {
            if (_activePlayer == EActivePlayer.White)
            {
                _activePlayer = EActivePlayer.Black;
            }
            else
            {
                _activePlayer = EActivePlayer.White;
            }
        }
    }
}