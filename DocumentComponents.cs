using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextToWavExe
{
    #region class VoiceOverDoc
    public class VoiceOverDoc
    {
        public List<Track> Tracks { get; set; }
        public string AudioPath { get; set; }
        public string Voice { get; set; }
        public int Volume { get; set; }
        public int Speed { get; set; }
        public int Pitch { get; set; }
        public bool AutoOpen { get; set; }
        public bool SaveDefaults { get; set; }
        public bool SaveWaveFiles { get; set; }
        public bool SpeakPerSelection { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }

        public VoiceOverDoc(string name) {
            Name = name;
        }
    }
    #endregion

    #region class Track
    public class Track
    {
        public string Name { get; set; }
        public List<Topic> Topics { get; set; }

        public Track()
        {
            Topics = new List<Topic>();
        }    
    }
    #endregion

    #region class Topic
    public class Topic
    {
        public string Name { get; set; }
        public Content Content { get; set; }
        public List <Content> Contents { get; set; }
        public Topic()
        {
            Content = new Content();
            Contents = new List<Content>();
        }     
    }
    #endregion

    #region class Content
    public class Content
    {
        public string Narration { get; set; }
        public string Subtitles { get; set; }
        public string Voice { get; set; }
        public int Volume { get; set; }
        public int Speed { get; set; }
        public int Pitch { get; set; }

        public Content()
        {
            Narration = "";
            Subtitles = "";
            Volume = -1;
            Speed = -1;
            Pitch = -1;
        }        
    }
    #endregion
}
