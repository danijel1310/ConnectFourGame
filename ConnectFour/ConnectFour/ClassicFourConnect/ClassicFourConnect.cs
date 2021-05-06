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

        private EConnectFourCellContent playerToContent(EActivePlayer player)
        {
            if (player == EActivePlayer.Black)
            {
                return EConnectFourCellContent.Black;
            }
            else
            {
                return EConnectFourCellContent.White;
            }
        }

        public void SetPiece(CellID target)
        {
            if (!IsValidMove(target))
            {
                return;
            }

            var targetCell = _board.GetCell(target);
            var cellid = _board.SetPiece(target.Row, target.Col, playerToContent(ActivePlayer));

            if (isGameFinished(cellid))
            {
                GameFinished(this, new GameEndedEventArgs(ActivePlayer));
            }

            nextTurn();
        }

        private bool isGameFinished(CellID target)
        {
            return (horizontalWin() || verticalWin() || diagonalWin(target));
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

        private bool horizontalWin()
        {

            var win = false;
            for (int row = _board.NumRows - 1; row >= 0; row--)
            {
                for (int col = 0; col < _board.NumCols; col++)
                {
                    if (_board.GetCell(CellID.Create(row, col)).Content == playerToContent(ActivePlayer) && (col + 3 <= _board.NumCols))
                    {

                        win = true;
                        for (int tempCol = col + 1; tempCol < _board.NumCols && tempCol < col + 4; tempCol++)
                        {
                            if (_board.GetCell(CellID.Create(row, tempCol)).Content ==
                                playerToContent(ActivePlayer))
                            {
                                win = true;
                            }
                            else
                            {
                                win = false;
                                break;
                            }
                        }

                        if (win)
                        {
                            return win;
                        }
                    }
                    win = false;
                }
            }

            return win;
        }

        private bool diagonalWin(CellID currentField)
        {
            return true;
        }

        private bool verticalWin()
        {
            var win = false;
            for (int col = 0; col < _board.NumCols; col++)
            {
                for (int row = _board.NumRows - 1; row >= 0; row--)
                {
                    if (_board.GetCell(CellID.Create(row, col)).Content == playerToContent(ActivePlayer) && row - 3 >= 0)
                    {
                        win = true;
                        for (int tempRow = row - 1; tempRow <= 0 && tempRow > row - 4; tempRow--)
                        {
                            if (_board.GetCell(CellID.Create(tempRow, col)).Content == playerToContent(ActivePlayer))
                            {
                                win = true;
                            }
                            else
                            {
                                win = false;
                                break;
                            }
                        }

                        if (win)
                        {
                            return win;
                        }
                    }

                    win = false;
                }
            }

            return win;
        }
    }
}