using MafiatorApp.Enums;
using Xamarin.Forms;

namespace MafiatorApp.Effects.EventArgs
{
    public class TouchActionEventArgs : System.EventArgs
    {
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }

        public long Id { get; }

        public TouchActionType Type { get; }

        public Point Location { get; }

        public bool IsInContact { get; }
    }
}
