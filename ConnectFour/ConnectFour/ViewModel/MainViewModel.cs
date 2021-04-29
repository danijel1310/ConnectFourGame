using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public MainViewModel()
        {
            SetupCellStatusViewModels();
        }

        private readonly List<List<CellStatusViewModel>> _cells = new List<List<CellStatusViewModel>>();

        public List<List<CellStatusViewModel>> Cells
        {
            get { return _cells; }
        }

        public void SetupCellStatusViewModels()
        {
            int rows = 6;
            int cols = 7;
            for (int i = 0; i < cols; i++)
            {
                Cells.Add(new List<CellStatusViewModel>());
                for (int j = 0; j < rows; j++)
                {
                    var cell = new CellStatusViewModel(i, j);
                    //TODO: set listener on COL selected 
                    Cells[i].Add(cell);
                }
            }
        }
    }
}
