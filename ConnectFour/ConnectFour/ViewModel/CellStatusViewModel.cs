using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConnectFour.Model;

namespace ConnectFour.ViewModel
{
    public class CellStatusViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CellStatusViewModel(int col, int row)
        {
            _col = col;
            _row = row;
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

        private int _row;

        private int _col;
    }
}