using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPowerPointControl.Model
{
    public class KinectGestureProcessor:INotifyPropertyChanged
    {
        public enum GestureState { Idle, Separated, Grip, Joined, Done}

        private GestureState currentLeftState;
        private GestureState currentRightState;
        public GestureState CurrentLeftState
        {
            get
            {
                return currentLeftState;
            }
            set
            {
                currentLeftState = value;
                NotifyPropertyChanged("CurrentLeftState");
            }
        }
        public GestureState CurrentRightState
        {
            get
            {
                return currentRightState;
            }
            set
            {
                currentRightState = value;
                NotifyPropertyChanged("CurrentRightState");
            }
        }

        private bool leftGrip;
        public bool LeftGrip
        {
            get
            {
                return leftGrip;
            }
            set
            {
                leftGrip = value;
                NotifyPropertyChanged("LeftGrip");
            }
        }
        private bool rightGrip;
        public bool RightGrip
        {
            get
            {
                return rightGrip;
            }
            set
            {
                rightGrip = value;
                NotifyPropertyChanged("RightGrip");
            }
        }

        private bool leftSeparated;
        public bool LeftSeparated
        {
            get
            {
                return leftSeparated;
            }
            set
            {
                leftSeparated = value;
                NotifyPropertyChanged("LeftSeparated");
            }
        }
        private bool rightSeparated;
        public bool RightSeparated
        {
            get
            {
                return rightSeparated;
            }
            set
            {
                rightSeparated = value;
                NotifyPropertyChanged("RightSeparated");
            }
        }

        private bool leftUp;
        public bool LeftUp
        {
            get
            {
                return leftUp;
            }
            set
            {
                leftUp = value;
                NotifyPropertyChanged("LeftUp");
            }
        }
        private bool rightUp;
        public bool RightUp
        {
            get
            {
                return rightUp;
            }
            set
            {
                rightUp = value;
                NotifyPropertyChanged("RightUp");
            }
        }

        private int ellipseSize;
        public int EllipseSize
        {
            get
            {
                return ellipseSize;
            }
            set
            {
                ellipseSize = value;
                NotifyPropertyChanged("EllipseSize");
            }
        }

        private SkeletonPoint headPosition;
        public SkeletonPoint HeadPosition
        {
            get
            {
                return headPosition;
            }
            set
            {
                headPosition = value;
                NotifyPropertyChanged("HeadPosition");
            }
        }

        private SkeletonPoint rightHandPosition;
        public SkeletonPoint RightHandPosition
        {
            get
            {
                return rightHandPosition;
            }
            set
            {
                rightHandPosition = value;
                NotifyPropertyChanged("RightHandPosition");
            }
        }

        private SkeletonPoint leftHandPosition;
        public SkeletonPoint LeftHandPosition
        {
            get
            {
                return leftHandPosition;
            }
            set
            {
                leftHandPosition = value;
                NotifyPropertyChanged("LeftHandPosition");
            }
        }

        private int leftStateCounter = 0;
        private int rightStateCounter = 0;

        private const int THRESHOLD = 5;



        public event GestureDetectedEventHandler GestureDetected;

        public KinectGestureProcessor()
        {
            CurrentLeftState = GestureState.Idle;
            CurrentRightState = GestureState.Idle;
        }

        private void UpdateLeftState()
        {
            if(!LeftUp)
            {
                CurrentLeftState = GestureState.Idle;
                return;
            }
            switch(CurrentLeftState)
            {
                case GestureState.Idle:
                    if(leftStateCounter == THRESHOLD)
                    {
                        CurrentLeftState = GestureState.Separated;
                        leftStateCounter = 0;
                    }
                    else if (LeftSeparated && !LeftGrip)
                    {
                        leftStateCounter++;
                    }
                    else
                    {
                        CurrentLeftState = GestureState.Idle;
                        leftStateCounter = 0;
                    }
                    break;
                case GestureState.Separated:
                    if (LeftSeparated && !LeftGrip)
                    {
                        CurrentLeftState = GestureState.Separated;
                        leftStateCounter = 0;
                        break;
                    }
                    else if(leftStateCounter == THRESHOLD)
                    {
                        CurrentLeftState = GestureState.Grip;
                        leftStateCounter = 0;
                    }
                    else if (LeftGrip && LeftSeparated)
                    {
                        leftStateCounter++;
                    }
                    else
                    {
                        CurrentLeftState = GestureState.Idle;
                        leftStateCounter = 0;
                    }
                    break;
                case GestureState.Grip:
                    if (LeftGrip && LeftSeparated)
                    {
                        CurrentLeftState = GestureState.Grip;
                        leftStateCounter = 0;
                        break;
                    }
                    else if(leftStateCounter == THRESHOLD)
                    {
                        CurrentLeftState = GestureState.Joined;
                        leftStateCounter = 0;
                    }
                    else if (!LeftSeparated && LeftGrip)
                    {
                        leftStateCounter++;
                    }
                    else
                    {
                        CurrentLeftState = GestureState.Idle;
                        leftStateCounter = 0;
                    }
                    break;
                case GestureState.Joined:
                    if (!LeftSeparated && LeftGrip)
                    {
                        CurrentLeftState = GestureState.Joined;
                        leftStateCounter = 0;
                        break;
                    }
                    else if(leftStateCounter == THRESHOLD)
                    {
                        CurrentLeftState = GestureState.Done;
                        if (GestureDetected != null)
                        {
                            GestureDetected(this, new GestureDetectedEventArgs(GestureType.SlideBackwards));
                        }
                        leftStateCounter = 0;
                    }
                    if (!LeftGrip && !LeftSeparated)
                    {
                        leftStateCounter++;
                    }
                    else
                    {
                        CurrentLeftState = GestureState.Idle;
                        leftStateCounter = 0;
                    }
                    break;
                case GestureState.Done:
                    CurrentLeftState = GestureState.Idle;
                    break;
            }
        }

        private void UpdateRightState()
        {
            if (!RightUp)
            {
                CurrentRightState = GestureState.Idle;
                return;
            }
            switch (CurrentRightState)
            {
                case GestureState.Idle:
                    if(rightStateCounter == THRESHOLD)
                    {
                        CurrentRightState = GestureState.Separated;
                        rightStateCounter = 0;
                    }
                    else if (RightSeparated && !RightGrip)
                    {
                        rightStateCounter++;
                    }
                    else
                    {
                        CurrentRightState = GestureState.Idle;
                        rightStateCounter = 0;
                    }
                    break;
                case GestureState.Separated:
                    if (RightSeparated && !RightGrip)
                    {
                        CurrentRightState = GestureState.Separated;
                        rightStateCounter = 0;
                        break;
                    }
                    if(rightStateCounter == THRESHOLD)
                    {
                        CurrentRightState = GestureState.Grip;
                        rightStateCounter = 0;
                    }
                    if (RightSeparated && RightGrip)
                    {
                        rightStateCounter++;
                    }
                    else
                    {
                        CurrentRightState = GestureState.Idle;
                        rightStateCounter = 0;
                    }
                    break;
                case GestureState.Grip:
                    if (RightSeparated && RightGrip)
                    {
                        CurrentRightState = GestureState.Grip;
                        rightStateCounter = 0;
                        break;
                    }
                    if(rightStateCounter == THRESHOLD)
                    {
                        CurrentRightState = GestureState.Joined;
                        rightStateCounter = 0;
                    }
                    else if (!RightSeparated && RightGrip)
                    {
                        rightStateCounter++;
                    }
                    else
                    {
                        CurrentRightState = GestureState.Idle;
                        rightStateCounter = 0;
                    }
                    break;
                case GestureState.Joined:
                    if (!RightSeparated && RightGrip)
                    {
                        CurrentRightState = GestureState.Joined;
                        rightStateCounter = 0;
                        break;
                    }
                    if(rightStateCounter == THRESHOLD)
                    {
                        CurrentRightState = GestureState.Done;
                        if (GestureDetected != null)
                        {
                            GestureDetected(this, new GestureDetectedEventArgs(GestureType.SlideForwards));
                        }
                        rightStateCounter = 0;
                    }
                    if (!RightGrip && !RightSeparated)
                    {
                        rightStateCounter++;
                    }
                    else
                    {
                        CurrentRightState = GestureState.Idle;
                        rightStateCounter = 0;
                    }
                    break;
                case GestureState.Done:
                    CurrentRightState = GestureState.Idle;
                    break;
            }
        }

        public void UpdateState()
        {
            UpdateLeftState();
            UpdateRightState(); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
