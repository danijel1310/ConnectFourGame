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
            //Unmark this if the player at the start is always white
            //_activePlayer = EActivePlayer.White;
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
            return (HorizontalWin() || VerticalWin() || DiagonalWin() );
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

        private bool HorizontalWin()
        {
            for (int row = _board.NumRows - 1; row >= 0; row--)
            {
                int matchCounter = 0;

                for (int col = 0; col < _board.NumCols; col++)
                {
                    if (_board.GetCell(CellID.Create(row, col)).Content == playerToContent(ActivePlayer))
                    {
                        matchCounter++;
                    }
                    else
                    {
                        matchCounter = 0;
                    }

                    if (matchCounter == 4)
                    {
                        Console.WriteLine("HORIZONTALWIN");
                        return true;
                    }
                }
            }
            return false;

        }

        private bool VerticalWin()
        {
            for (int col = 0; col < _board.NumCols; col++)
            {
                int matchCounter = 0;

                for (int row = _board.NumRows - 1; row >= 0; row--)
                {
                    if (_board.GetCell(CellID.Create(row, col)).Content == playerToContent(ActivePlayer))
                    {
                        matchCounter++;
                    }
                    else
                    {
                        matchCounter = 0;
                    }

                    if (matchCounter == 4)
                    {
                        Console.WriteLine("VERTICALWIN");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DiagonalWin()

        /*
         *      4       
         *  _______  5
         *  | O O O  O  O O O
         *3 | O O O  O  O O O
         *  | O O O  O  O O O
         * 
         *  | O O O  O  O O O
         *2 | O O O  O  O O O
         *  | O O O  O  O O O
         * ______
         * 1         6   
         * 
         * 
         */
        {
            //1
            for (int col = 0; col+4 < _board.NumCols; col++) {
                int row = _board.NumRows - 1;
                int counter = 0;
                for(int diagCol = col; row >= 0 && diagCol < _board.NumCols; diagCol++)
                {
                    if (_board.GetCell(CellID.Create(row, diagCol)).Content == playerToContent(ActivePlayer)) {
                        counter++;
                    } else
                    {
                        counter = 0;
                    }
                    if (counter == 4)
                    {
                        Console.WriteLine("DIAGONALWIN 1");
                        return true;
                    }
                    row--;
                }
            }

            //2
            for(int row = _board.NumRows-1; row-3 > 0; row--)
            {
                int col = 0;
                int counter = 0;
                for(int diagRow = row; diagRow >= 0 && col >= 0; diagRow--) {
                    if (_board.GetCell(CellID.Create(diagRow, col)).Content == playerToContent(ActivePlayer))
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    if (counter == 4)
                    {
                        Console.WriteLine("DIAGONALWIN 2");
                        return true;
                    }
                    col++;
                }
            }

            //3
            for (int row = 0; row + 3 < _board.NumRows; row++)
            {
                int col = 0;
                int counter = 0;
                for (int diagRow = row; diagRow < _board.NumRows && col < _board.NumCols; diagRow++)
                {
                    if (_board.GetCell(CellID.Create(diagRow, col)).Content == playerToContent(ActivePlayer))
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    if (counter == 4)
                    {
                        Console.WriteLine("DIAGONALWIN 3");
                        return true;
                    }
                    col++;
                }
            }

            //4
            for (int col = 0; col + 4 < _board.NumCols; col++)
            {
                int row = 0;
                int counter = 0;
                for (int diagCol = col; row < _board.NumRows && diagCol < _board.NumCols; diagCol++)
                {
                    if (_board.GetCell(CellID.Create(row, diagCol)).Content == playerToContent(ActivePlayer))
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                    if (counter == 4)
                    {
                        Console.WriteLine("DIAGONALWIN 4");
                        return true;
                    }
                    row++;
                }
            }

            //5
            var leftOverCol = _board.NumCols / 2;

            var topRow = 0;
            var lastCounter = 0;
            for(int topCol = leftOverCol; topCol < _board.NumCols; topCol++)
            {
                if (_board.GetCell(CellID.Create(topRow, topCol)).Content == playerToContent(ActivePlayer))
                {
                    lastCounter++;
                }
                else
                {
                    lastCounter = 0;
                }
                if (lastCounter == 4)
                {
                    Console.WriteLine("DIAGONALWIN 5");
                    return true;
                }
                    topRow++;
            }

            var bottomRow = _board.NumRows - 1;
            lastCounter = 0;

            for (int bottomCol = leftOverCol; bottomCol < _board.NumCols; bottomCol++)
            {
                if (_board.GetCell(CellID.Create(bottomRow, bottomCol)).Content == playerToContent(ActivePlayer))
                {
                    lastCounter++;
                }
                else
                {
                    lastCounter = 0;
                }
                if (lastCounter == 4)
                {
                    Console.WriteLine("DIAGONALWIN 6");
                    return true;
                }
                bottomRow--;
            }
            return false;
        }

        /*
        private bool horizontalWin()
        {

            var win = false;
            for (int row = _board.NumRows - 1; row >= 0; row--)
            {
                for (int col = 0; col < _board.NumCols; col++)
                {
                    if (_board.GetCell(CellID.Create(row, col)).Content == playerToContent(ActivePlayer) && (col + 3 < _board.NumCols))
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
        
        private bool diagonalWin()
        {
            var win = false;

            //bottom left to top right
            for (int col = 0; col < _board.NumCols - 3; col++)
            {
                for (int row = _board.NumRows - 1; row >= 3; row--)
                {
                    var startContent = _board.GetCell(CellID.Create(row, col)).Content;
                    if (startContent == EConnectFourCellContent.Empty)
                    {
                        break;
                    }else if (startContent == playerToContent(ActivePlayer))
                    {
                        win = true;
                        for (int tempRow = row - 1; tempRow > row - 4 && win; tempRow--)
                        {
                            for (int tempCol = col + 1; tempCol < col + 4 && win; tempCol++)
                            {
                                if (_board.GetCell(CellID.Create(tempRow, tempCol)).Content ==
                                    playerToContent(ActivePlayer))
                                {
                                    win = true;
                                }
                                else
                                {
                                    win = false;
                                }
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

            //top left to bottom right
            for (int col = 0; col < _board.NumCols - 3; col++)
            {
                for (int row = _board.NumRows - 4; row >= 0; row--)
                {
                    var startContent = _board.GetCell(CellID.Create(row, col)).Content;
                    if (startContent == EConnectFourCellContent.Empty)
                    {
                        break;
                    }
                    else if (startContent == playerToContent(ActivePlayer))
                    {
                        win = true;
                        for (int tempRow = row + 1; tempRow < row + 4 && win; tempRow++)
                        {
                            for (int tempCol = col + 1; tempCol < col + 4 && win; tempCol++)
                            {
                                if (_board.GetCell(CellID.Create(tempRow, tempCol)).Content ==
                                    playerToContent(ActivePlayer))
                                {
                                    win = true;
                                }
                                else
                                {
                                    win = false;
                                }
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

        private bool verticalWin()
        {
            return false;
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
        }*/
    }
        
}