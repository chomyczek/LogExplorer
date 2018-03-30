using System.Collections.Generic;
using System.Windows.Media;

namespace LogExplorer.Models
{
    public class LogOverview
    {
        #region Public Properties

        public string DirPath => Log.DirPath;

        public string DurationString => Log.DurationString;

        public List<Log> History { get; set; }

        public bool IsSelected
        {
            get { return Log.IsSelected; }
            set { Log.IsSelected = value; }
        }

        public string LogPath => Log.LogPath;

        public string Name => Log.Name;

        public string Result => Log.Result;

        public Brush ResultColor => Log.ResultColor;

        public string StartTimeString => Log.StartTimeString;

        private Log Log => History[0];

        #endregion
    }
}