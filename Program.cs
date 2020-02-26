using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.IO;
using Newtonsoft.Json;


namespace TextToWavExe
{
    class Program
    {
        static SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
        static List<string> InstalledVoices { get; set; }
        static List<string> InstalledCultures { get; set; }
        static void Main(string[] args)
        {
            Initialize();
            var voiceOverDoc = LoadInput();
            SpeakToWave(voiceOverDoc, 0);
        }

        static VoiceOverDoc LoadInput()
        {
            string projpath;
            if (System.Diagnostics.Debugger.IsAttached)
                projpath = new Uri(Path.Combine(new string[] { System.AppDomain.CurrentDomain.BaseDirectory, "..\\.." })).AbsolutePath;
            else
                projpath = System.AppDomain.CurrentDomain.BaseDirectory;
            var jsonString = System.IO.File.ReadAllText(projpath + "/test-file.json");
            var voiceOverDoc = JsonConvert.DeserializeObject<VoiceOverDoc>(jsonString);
            return voiceOverDoc;
        }
        static void Initialize()
        {
            InstalledVoices = new List<string>();
            InstalledCultures = new List<string>();
            var Voices = SpeechSynthesizer.GetInstalledVoices();
            foreach (var v in Voices)
            {
                InstalledVoices.Add(v.VoiceInfo.Name);
                InstalledCultures.Add(v.VoiceInfo.Culture.Name);
            }
        }

        static void SpeakToWave(VoiceOverDoc voiceOverDoc, int TrackId)
        {
            try
            {
                Track Track = voiceOverDoc.Tracks[TrackId];
                string Language = "en";
                string SSML = "";
                SSML = "<?xml version=\"1.0\"?> ";
                SSML += "<speak version=\"1.0\" ";
                SSML += "xml:lang=\"" + Language + "\"> ";

                foreach (Topic Topic in Track.Topics)
                {
                    foreach (Content Content in Topic.Contents)
                    {
                        var Text = Content.Narration;

                        string Volume = Content.Volume.ToString();

                        string Pitch = "";
                        if (Content.Pitch == 0) Pitch = "x-low";
                        if (Content.Pitch == 1) Pitch = "low";
                        if (Content.Pitch == 2) Pitch = "medium";
                        if (Content.Pitch == 3) Pitch = "high";
                        if (Content.Pitch == 4) Pitch = "x-high";

                        double dSpeed = Content.Speed;
                        dSpeed /= 100;
                        var Speed = dSpeed.ToString();

                        // add pauses to speech
                        Text = Text.Replace(",,", "<break time=\"500ms\"/>");

                        SSML += "<voice name=\"" + Content.Voice + "\" xml:lang=\"" + Language + "\">";
                        SSML += "<prosody volume=\"" + Volume + "\" pitch=\"" + Pitch + "\" rate=\"" + Speed + "\" >";
                        SSML += Text;
                        SSML += "</prosody>";
                        SSML += "</voice> ";
                    }
                }







                SSML += "</speak>";

                SpeechSynthesizer.SetOutputToWaveFile("Test.wav", new SpeechAudioFormatInfo(44100, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                //SpeechSynthesizer.SelectVoice(VoiceOverDoc.Voice);
                SpeechSynthesizer.SpeakSsml(SSML);
                SpeechSynthesizer.SetOutputToDefaultAudioDevice();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

    }
}
