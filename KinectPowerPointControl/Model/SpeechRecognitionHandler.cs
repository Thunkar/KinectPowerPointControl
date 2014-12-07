using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace KinectPowerPointControl.Model
{
    public class SpeechRecognitionHandler:INotifyPropertyChanged
    {
        public SpeechRecognitionEngine recognizer { get; set; }
        public SpeechSynthesizer synth { get; set; }

        private bool voiceControlActivated;
        public bool VoiceControlActivated
        {
            get
            {
                return voiceControlActivated;
            }
            set
            {
                voiceControlActivated = value;
                NotifyPropertyChanged("VoiceControlActivated");
            }
        }

        private bool active;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                NotifyPropertyChanged("Active");
            }
        }

        public SpeechRecognitionHandler()
        {
            recognizer = new SpeechRecognitionEngine();
            synth = new SpeechSynthesizer();
            Active = false;
        }

        public void InitializeSpeechRecognition(KinectSensor sensor)
        {
            Grammar activate = new Grammar(new GrammarBuilder("Cortana, activa el control por voz"));
            activate.Name = "activate";
            Grammar deactivate = new Grammar(new GrammarBuilder("Cortana, desactiva el control por voz"));
            deactivate.Name = "deactivate";
            recognizer.LoadGrammar(activate);
            recognizer.LoadGrammar(deactivate);
            //Ppt commands
            Grammar show = new Grammar(new GrammarBuilder("Cortana, muestra la ventana"));
            show.Name = "show";
            recognizer.LoadGrammar(show);
            Grammar minimize = new Grammar(new GrammarBuilder("Cortana, minimiza la ventana"));
            minimize.Name = "minimize";
            recognizer.LoadGrammar(minimize);
            Grammar abreppt = new Grammar(new GrammarBuilder("Cortana, abre la presentación"));
            abreppt.Name = "abreppt";
            recognizer.LoadGrammar(abreppt);
            Grammar cierrappt = new Grammar(new GrammarBuilder("Cortana, cierra la presentación"));
            abreppt.Name = "cierrappt";
            recognizer.LoadGrammar(cierrappt);

            recognizer.RequestRecognizerUpdate();
            StartSpeechRecognition(sensor);
        }

        private void StartSpeechRecognition(KinectSensor sensor)
        {

            var audioSource = sensor.AudioSource;
            audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
            var kinectStream = audioSource.Start();
            recognizer.SetInputToAudioStream(
                    kinectStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            Active = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
