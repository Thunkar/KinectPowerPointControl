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

        private Presentation presentation;
        private Microsoft.Office.Interop.PowerPoint.Application ppApp;


        public void OpenPresentation()
        {
            if (File == null || file == string.Empty) return;
            ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
            ppApp.Visible = MsoTriState.msoTrue;
            Presentations ppPresens = ppApp.Presentations; 
            presentation = ppPresens.Open(File, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue);
            Slides objSlides = presentation.Slides;
            Microsoft.Office.Interop.PowerPoint.SlideShowWindows objSSWs; 
            Microsoft.Office.Interop.PowerPoint.SlideShowSettings objSSS;
            //Run the Slide show
            objSSS = presentation.SlideShowSettings;
            objSSS.Run();
            objSSWs = ppApp.SlideShowWindows;
            Maximize();
        }

        public void ClosePresentation()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();


                presentation.Close();
                Marshal.ReleaseComObject(presentation);

                ppApp.Quit();
                Marshal.ReleaseComObject(ppApp);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void Maximize()
        {
            presentation.SlideShowWindow.Activate();
            presentation.SlideShowWindow.Application.WindowState = Microsoft.Office.Interop.PowerPoint.PpWindowState.ppWindowMaximized;
            presentation.SlideShowSettings.Application.WindowState = Microsoft.Office.Interop.PowerPoint.PpWindowState.ppWindowMaximized;
        }

        public void Activate()
        {
            presentation.SlideShowWindow.Activate();
        }

        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
