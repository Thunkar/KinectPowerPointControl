using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KinectPowerPointControl.Model
{
    public class PowerPointConnector:INotifyPropertyChanged
    {
        private string file;
        public string File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
                NotifyPropertyChanged("File");
            }
        }
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int show);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
 
        private const int SW_SHOW = 5;



        private Presentation presentation;
        private Microsoft.Office.Interop.PowerPoint.Application powerPointApp;


        public void OpenPresentation()
        {
            if (File == null || file == string.Empty) return;
            powerPointApp = new Microsoft.Office.Interop.PowerPoint.Application();
            powerPointApp.Visible = MsoTriState.msoTrue;
            Presentations Presentations = powerPointApp.Presentations; 
            presentation = Presentations.Open(File, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue);
            presentation.SlideShowSettings.Run();
        }

        public void ClosePresentation()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();


                presentation.Close();
                Marshal.ReleaseComObject(presentation);

                powerPointApp.Quit();
                Marshal.ReleaseComObject(powerPointApp);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


        public void Activate()
        {
            if(presentation != null)
            {
                presentation.SlideShowWindow.Activate();
                IntPtr handle = (IntPtr)presentation.SlideShowWindow.HWND;
                ShowWindow(handle, SW_SHOW);
                SetForegroundWindow(handle);
                SwitchToThisWindow(handle, true);
            }
                
        }

        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
