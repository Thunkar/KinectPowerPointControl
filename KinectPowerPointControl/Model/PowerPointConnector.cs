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
            presentation.SlideShowSettings.Application.Activate();
        }

        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
