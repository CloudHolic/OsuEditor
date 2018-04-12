using System.ComponentModel;

namespace OsuEditor.Models
{
    public class DiffVersion : INotifyPropertyChanged
    {
        #region string DiffName
        private string _diffName;
        public string DiffName {
            get => _diffName;
            set
            {
                _diffName = value;
                OnPropertyChanged(nameof(DiffName));
            }
        }
        #endregion

        #region string FileName
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        #endregion

        #region bool Activated
        private bool _activated;
        public bool Activated
        {
            get => _activated;
            set
            {
                _activated = value;
                OnPropertyChanged(nameof(Activated));
            }
        }
        #endregion

        public DiffVersion()
        {
            DiffName = string.Empty;
            FileName = string.Empty;
            Activated = false;
        }

        public DiffVersion(string diff, string file, bool activated)
        {
            DiffName = diff;
            FileName = file;
            Activated = activated;
        }

        public DiffVersion(DiffVersion prevVersion)
        {
            DiffName = prevVersion.DiffName;
            FileName = prevVersion.FileName;
            Activated = prevVersion.Activated;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
