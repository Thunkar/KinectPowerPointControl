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

        public GestureState CurrentLeftState;
        public GestureState CurrentRightState;

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



        public event GestureDetectedEventHandler GestureDetected;

        public KinectGestureProcessor()
        {
            CurrentLeftState = GestureState.Idle;
            CurrentRightState = GestureState.Idle;
        }

        private void UpdateLeftState()
        {
            switch(CurrentLeftState)
            {
                case GestureState.Idle:
                    if (LeftSeparated && !LeftGrip)
                        CurrentLeftState = GestureState.Separated;
                    else
                        CurrentLeftState = GestureState.Idle;
                    break;
                case GestureState.Separated:
                    if (LeftSeparated && !LeftGrip)
                    {
                        CurrentLeftState = GestureState.Separated;
                        break;
                    }
                    if (LeftGrip && LeftSeparated)
                        CurrentLeftState = GestureState.Grip;
                    else
                        CurrentLeftState = GestureState.Idle;
                    break;
                case GestureState.Grip:
                    if (LeftGrip && LeftSeparated)
                    {
                        CurrentLeftState = GestureState.Grip;
                        break;
                    }
                    if (!LeftSeparated && LeftGrip)
                        CurrentLeftState = GestureState.Joined;
                    else
                        CurrentLeftState = GestureState.Idle;
                    break;
                case GestureState.Joined:
                    if (!LeftSeparated && LeftGrip)
                    {
                        CurrentLeftState = GestureState.Joined;
                        break;
                    }
                    if (!LeftGrip && !LeftSeparated)
                    {
                        CurrentLeftState = GestureState.Done;
                        if (GestureDetected != null)
                        {
                            GestureDetected(this, new GestureDetectedEventArgs(GestureType.SlideBackwards));
                        }
                    }
                    else
                        CurrentLeftState = GestureState.Idle;
                    break;
                case GestureState.Done:
                    CurrentLeftState = GestureState.Idle;
                    break;
            }
        }

        private void UpdateRightState()
        {
            switch (CurrentRightState)
            {
                case GestureState.Idle:
                    if (RightSeparated && !RightGrip)
                        CurrentRightState = GestureState.Separated;
                    else
                        CurrentRightState = GestureState.Idle;
                    break;
                case GestureState.Separated:
                    if (RightSeparated && !RightGrip)
                    {
                        CurrentRightState = GestureState.Separated;
                        break;
                    }
                    if (RightSeparated && RightGrip)
                        CurrentRightState = GestureState.Grip;
                    else
                        CurrentLeftState = GestureState.Idle;
                    break;
                case GestureState.Grip:
                    if (RightSeparated && RightGrip)
                    {
                        CurrentRightState = GestureState.Grip;
                        break;
                    }
                    if (!RightSeparated && RightGrip)
                        CurrentRightState = GestureState.Joined;
                    else
                        CurrentRightState = GestureState.Idle;
                    break;
                case GestureState.Joined:
                    if (!RightSeparated && RightGrip)
                    {
                        CurrentRightState = GestureState.Joined;
                        break;
                    }
                    if (!RightGrip && !RightSeparated)
                    {
                        CurrentRightState = GestureState.Done;
                        if (GestureDetected != null)
                        {
                            GestureDetected(this, new GestureDetectedEventArgs(GestureType.SlideForwards));
                        }
                    }
                    else
                        CurrentRightState = GestureState.Idle;
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
