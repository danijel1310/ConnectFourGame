using System;
using ConnectFour.Model;

namespace ConnectFour.Events
{
    public class GameEndedEventArgs : EventArgs
    {
        public EActivePlayer Winner { get; }

        public GameEndedEventArgs(EActivePlayer winner)
        {
            Winner = winner;
        }

    }
}