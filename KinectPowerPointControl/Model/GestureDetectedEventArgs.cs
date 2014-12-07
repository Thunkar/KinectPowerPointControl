using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPowerPointControl.Model
{

    public delegate void GestureDetectedEventHandler(object source, GestureDetectedEventArgs e);
    public enum GestureType { SlideForwards, SlideBackwards }


    public class GestureDetectedEventArgs : EventArgs
    {
        private GestureType EventInfo;

        public GestureDetectedEventArgs(GestureType Gesture)
        {
            EventInfo = Gesture;
        }
        public GestureType GetInfo()
        {
            return EventInfo;
        }
    }
}
