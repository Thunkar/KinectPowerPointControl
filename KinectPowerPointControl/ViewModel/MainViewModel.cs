using KinectPowerPointControl.Model;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KinectPowerPointControl.ViewModel
{
    public class MainViewModel
    {
        public static MainViewModel Current { get; set; }
        public KinectHandler KinectHandler { get; set; }
        public KinectGestureProcessor KinectGestureProcessor { get; set; }

        public PowerPointConnector PowerPointConnector { get; set; }
        public SpeechRecognitionHandler SpeechRecognitionHandler { get; set; }

        public Thread Initializer;

        public MainViewModel()
        {
            Current = this;
            KinectHandler = new KinectHandler();
            KinectGestureProcessor = new KinectGestureProcessor();
            KinectGestureProcessor.GestureDetected += KinectGestureProcessor_GestureDetected;
            PowerPointConnector = new PowerPointConnector();
            SpeechRecognitionHandler = new SpeechRecognitionHandler();
            Initializer = new Thread(new ThreadStart(Setup));
            
        }


        private void Setup()
        {
            InitializeKinect();
            SpeechRecognitionHandler.InitializeSpeechRecognition(KinectHandler.Sensor);
            SpeechRecognitionHandler.recognizer.SpeechRecognized += recognizer_SpeechRecognized;
        }


        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "Cortana, activa el control por voz" && e.Result.Confidence >= 0.7)
            {
                SpeechRecognitionHandler.synth.SpeakAsync("Control por voz activado");
                SpeechRecognitionHandler.VoiceControlActivated = true;
            }
            if (e.Result.Text == "Cortana, desactiva el control por voz" && e.Result.Confidence >= 0.65)
            {
                SpeechRecognitionHandler.synth.SpeakAsync("Control por voz desactivado");
                SpeechRecognitionHandler.VoiceControlActivated = false;
            }
            if (SpeechRecognitionHandler.VoiceControlActivated == true && e.Result.Confidence >= 0.65) voiceControlHandler(e.Result.Text);
        }

        public void voiceControlHandler(String command)
        {
            if (command == "Cortana, abre la presentación")
            {
                SpeechRecognitionHandler.synth.SpeakAsync("Abriendo la presentación");
                OpenPresentation();
                PowerPointConnector.Activate();
            }
            if (command == "Cortana, cierra la presentación")
            {
                PowerPointConnector.ClosePresentation();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ShowWindow();
                }));
                SpeechRecognitionHandler.synth.SpeakAsync("Cerrando presentación");
            }
            if (command == "Cortana, muestra la ventana")
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ShowWindow();
                }));
                PowerPointConnector.Activate();
                SpeechRecognitionHandler.synth.SpeakAsync("Mostrando ventana");
            }
            if (command == "Cortana, minimiza la ventana")
            {
                PowerPointConnector.Activate();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    HideWindow();
                }));
                SpeechRecognitionHandler.synth.SpeakAsync("Minimizando ventana");
            }
        }


        private void ShowWindow()
        {
            Application.Current.MainWindow.Topmost = true;
            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
        }

        private void HideWindow()
        {
            Application.Current.MainWindow.Topmost = false;
            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        public void OpenPresentation()
        {
            if (PowerPointConnector.File == null)
                Browse();
            PowerPointConnector.OpenPresentation();
        }

        public void Browse()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "Locate your presentation";
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            PowerPointConnector.File = System.IO.Path.GetFullPath(dialog.FileName);
        }

        public void ClosePresentation()
        {
            PowerPointConnector.ClosePresentation();
        }


        void KinectGestureProcessor_GestureDetected(object source, GestureDetectedEventArgs e)
        {
            if (e.GetInfo().Equals(GestureType.SlideForwards))
            {
                System.Windows.Forms.SendKeys.SendWait("{Right}");
            }
            else
            {
                System.Windows.Forms.SendKeys.SendWait("{Left}");
            }
        }


        public void InitializeKinect()
        {
            KinectHandler.Busy = true;
            KinectHandler.InitializeSensor();
            KinectGestureProcessor.EllipseSize = 20;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                KinectHandler.Sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);
                KinectHandler.Sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);
                KinectHandler.Sensor.DepthFrameReady += Sensor_DepthFrameReady;
                KinectHandler.interactionStream = new InteractionStream(KinectHandler.Sensor, new DummyInteractionClient());
                KinectHandler.interactionStream.InteractionFrameReady += interactionStream_InteractionFrameReady;
            }));
            KinectHandler.Busy = false;
        }

        void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame == null)
                    return;
                KinectHandler.interactionStream.ProcessDepth(depthFrame.GetRawPixelData(), depthFrame.Timestamp);
            }
        }

        void interactionStream_InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            using (InteractionFrame frame = e.OpenInteractionFrame())
            {
                if (frame != null)
                {
                    if (KinectHandler.userInfos == null)
                    {
                        KinectHandler.userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength];
                    }

                    frame.CopyInteractionDataTo(KinectHandler.userInfos);
                }
                else
                {
                    return;
                }
            }



            foreach (UserInfo userInfo in KinectHandler.userInfos)
            {
                foreach (InteractionHandPointer handPointer in userInfo.HandPointers)
                {
                    switch (handPointer.HandEventType)
                    {
                        case InteractionHandEventType.Grip:
                            switch (handPointer.HandType)
                            {
                                case InteractionHandType.Left:
                                    KinectGestureProcessor.LeftGrip = true;
                                    break;
                                case InteractionHandType.Right:
                                    KinectGestureProcessor.RightGrip = true;
                                    break;
                            }
                            break;

                        case InteractionHandEventType.GripRelease:
                            switch (handPointer.HandType)
                            {
                                case InteractionHandType.Left:
                                    KinectGestureProcessor.LeftGrip = false;
                                    break;
                                case InteractionHandType.Right:
                                    KinectGestureProcessor.RightGrip = false;
                                    break;
                            }
                            break;
                    }
                }

            }
        }


        public void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            byte[] colorBytes = null;
            using (var image = e.OpenColorImageFrame())
            {
                if (image == null)
                    return;
                if (colorBytes == null || colorBytes.Length != image.PixelDataLength)
                {
                    colorBytes = new byte[image.PixelDataLength];
                }

                image.CopyPixelDataTo(colorBytes);

                //You could use PixelFormats.Bgr32 below to ignore the alpha,
                //or if you need to set the alpha you would loop through the bytes 
                //as in this loop below
                int length = colorBytes.Length;
                for (int i = 0; i < length; i += 4)
                {
                    colorBytes[i + 3] = 255;
                }

                BitmapSource source = BitmapSource.Create(image.Width,
                    image.Height,
                    96,
                    96,
                    PixelFormats.Bgra32,
                    null,
                    colorBytes,
                    image.Width * image.BytesPerPixel);
                KinectHandler.ImageFromKinect = source;
            }
        }

        public void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = KinectHandler.Skeletons;
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;
                if (skeletons == null || skeletons.Length != skeletonFrame.SkeletonArrayLength)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }

                skeletonFrame.CopySkeletonDataTo(skeletons);

                KinectHandler.interactionStream.ProcessSkeleton(skeletons, KinectHandler.Sensor.AccelerometerGetCurrentReading(), skeletonFrame.Timestamp);
            }

            KinectHandler.closestSkeleton = skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked)
                                                .OrderBy(s => s.Position.Z * Math.Abs(s.Position.X))
                                                .FirstOrDefault();

            if (KinectHandler.closestSkeleton == null)
                return;

            Joint head = KinectHandler.closestSkeleton.Joints[JointType.Head];
            Joint rightHand = KinectHandler.closestSkeleton.Joints[JointType.HandRight];
            Joint leftHand = KinectHandler.closestSkeleton.Joints[JointType.HandLeft];


            if (head.TrackingState == JointTrackingState.NotTracked ||
                rightHand.TrackingState == JointTrackingState.NotTracked ||
                leftHand.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }
            else
            {
                KinectGestureProcessor.HeadPosition = head.Position;
                KinectGestureProcessor.LeftHandPosition = leftHand.Position;
                KinectGestureProcessor.RightHandPosition = rightHand.Position;
                if(rightHand.Position.Y < head.Position.Y - 0.45)
                {
                    KinectGestureProcessor.RightUp = false;
                }
                else
                {
                    KinectGestureProcessor.RightUp = true;
                }

                if (leftHand.Position.Y < head.Position.Y - 0.45)
                {
                    KinectGestureProcessor.LeftUp = false;
                }
                else
                {
                    KinectGestureProcessor.LeftUp = true;
                }

                if (rightHand.Position.X > head.Position.X + 0.45)
                {
                    KinectGestureProcessor.RightSeparated = true;
                }
                else
                {
                    KinectGestureProcessor.RightSeparated = false;
                }

                if (leftHand.Position.X < head.Position.X - 0.45)
                {
                    KinectGestureProcessor.LeftSeparated = true;
                }
                else
                {
                    KinectGestureProcessor.LeftSeparated = false;
                }
                KinectGestureProcessor.UpdateState();
            }
        }
    }
}
