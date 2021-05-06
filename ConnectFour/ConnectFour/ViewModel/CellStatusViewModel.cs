using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConnectFour.Model;

namespace ConnectFour.ViewModel
{
    public class CellStatusViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event EventHandler<CellID> CellSelected = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private EConnectFourCellContent _cellFilling = EConnectFourCellContent.Empty;

        public EConnectFourCellContent CellFilling
        {
            get { return _cellFilling; }
            private set
            {
                if (value != _cellFilling)
                {
                    _cellFilling = value;
                    OnPropertyChanged(nameof(CellFilling));
                }
            }
        }

        private RelayCommand _cellSelectedCommand;

        public RelayCommand CellSelectedCommand
        {
            get
            {
                return _cellSelectedCommand ?? (_cellSelectedCommand = new RelayCommand(
                    (x) =>
                    {
                        CellSelected(this, CellID.Create(_row, _col));
                    },
                    (x) => { return true; }));
            }
        }

        public void SetContent(EConnectFourCellContent content)
        {
            _cellFilling= content;
        }

        private int _row;
        private int _col;

        public CellID Identifier
        {
            get { return  CellID.Create(_row, _col);}
        }

        public CellStatusViewModel(int row, int col, EConnectFourCellContent content)
        {
            _row = row;
            _col = col;
            _cellFilling = content;
        }
    }
}