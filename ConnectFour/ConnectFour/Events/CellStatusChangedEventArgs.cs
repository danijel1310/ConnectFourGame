using System;
using ConnectFour.Model;

namespace ConnectFour.Events
{
    public class CellStatusChangedEventArgs : EventArgs
    {
        public CellID Identifier { get; }

        public EConnectFourCellContent NewContent { get; }

        public EConnectFourCellContent OldContent { get; }

        public CellStatusChangedEventArgs(CellID identifier, EConnectFourCellContent newContent,
            EConnectFourCellContent oldContent)
        {
            Identifier = identifier;
            NewContent = newContent;
            OldContent = oldContent;
        }

    }
}