using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Events;
using ConnectFour.Model;

namespace ConnectFour.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly IGameLogic _gameLogic;

        private bool _isGameFinished = false;
        public bool IsGameFinished
        {
            get { return _isGameFinished; }
            set
            {
                if (_isGameFinished != value)
                {
                    _isGameFinished = value;
                    OnPropertyChanged(nameof(IsGameFinished));
                }
            }
        }

        private EActivePlayer _winner;

        public EActivePlayer Winner
        {
            get { return _winner; }
            set
            {
                if (_winner != value)
                {
                    _winner = value;
                    OnPropertyChanged(nameof(Winner));
                }
            }
        }

        private EActivePlayer _activePlayer = EActivePlayer.White;

        public EActivePlayer ActivePlayer
        {
            get { return _activePlayer; }
            set
            {
                _activePlayer = value;
                OnPropertyChanged(nameof(ActivePlayer));
            }
        }

        private EConnectFourCellContent _activePlayerCellColor;

        public EConnectFourCellContent ActivePlayerCellColor
        {
            get { return _activePlayerCellColor; }
            set
            {
                _activePlayerCellColor = value;
                OnPropertyChanged(nameof(ActivePlayerCellColor));
            }
        }

        public MainViewModel(IGameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            _gameLogic.CellStatusChanged += _board_CellStatusChanged;
            _gameLogic.GameFinished += _gameLogic_GameFinished;
            SetupCellStatusViewModels(_gameLogic.NumCols, _gameLogic.NumRows);
            _gameLogic.StartGame();
        }

        private readonly List<List<CellStatusViewModel>> _cells = new List<List<CellStatusViewModel>>();

        public List<List<CellStatusViewModel>> Cells
        {
            get { return _cells; }
        }

        public void SetupCellStatusViewModels(int cols, int rows)
        {
            for (int i = 0; i < cols; i++)
            {
                Cells.Add(new List<CellStatusViewModel>());
                for (int j = 0; j < rows; j++)
                {
                    var cell = new CellStatusViewModel(i, j, EConnectFourCellContent.Empty);
                    cell.CellSelected += Cell_CellSelected;
                    Cells[i].Add(cell);
                }
            }
        }

        private void _gameLogic_GameFinished(object sender, GameEndedEventArgs e)
        {
            Winner = e.Winner;
            IsGameFinished = true;
            Task.Delay(3000).ContinueWith((x) => { IsGameFinished = true; });
        }

        private void _board_CellStatusChanged(object sender, CellStatusChangedEventArgs e)
        {
            fetchCell(e.Identifier).SetContent(e.NewContent);
        }

        private CellStatusViewModel fetchCell(CellID identifier)
        {
            return Cells[identifier.Col][identifier.Row];
        }

        private void Cell_CellSelected(object sender, CellID selectedCellId)
        {
            if (_isGameFinished)
            {
                return;
            }

            var cell = fetchCell(selectedCellId);
            if (_gameLogic.IsValidMove(selectedCellId))
            {
                _gameLogic.SetPiece(selectedCellId);
                ActivePlayer = _gameLogic.ActivePlayer;
                ActivePlayerCellColor = playerToColorConverter(_gameLogic.ActivePlayer);
            }
        }

        private EConnectFourCellContent playerToColorConverter(EActivePlayer player)
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

        private RelayCommand _restartGameCommand;

        public RelayCommand RestartGameCommand
        {
            get
            {
                return _restartGameCommand ?? (_restartGameCommand = new RelayCommand(
                    (x) =>
                    {
                        _gameLogic.StartGame();
                        ActivePlayer = EActivePlayer.White;
                        ActivePlayerCellColor = EConnectFourCellContent.White;
                    },
                    (x) => { return true; }));
            }
        }

        private RelayCommand _surrenderGameCommand;

        public RelayCommand SurrenderGameCommand
        {
            get
            {
                return _surrenderGameCommand ?? (_surrenderGameCommand = new RelayCommand(
                    (x) =>
                    {
                        _gameLogic.SurrenderGame();
                        IsGameFinished = true;
                    },
                    (x) => { return true; }));
            }
        }
    }
}
