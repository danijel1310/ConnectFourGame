namespace ConnectFour.Model
{
    public class FourConnectCellModel
    {
        public int RowIndex { get; }
        public int ColIndex { get; }
        public EConnectFourCellContent Content { get; private set; }

        public FourConnectCellModel(int row, int col)
        {
            RowIndex = row;
            ColIndex = col;
            Content = EConnectFourCellContent.Empty;
        }

        public void SetContent(EConnectFourCellContent content)
        {
            Content = content;
        }

        public CellID Identifier
        {
            get { return CellID.Create(RowIndex, ColIndex);}
        }
    }
}