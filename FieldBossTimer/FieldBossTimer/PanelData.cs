using System;
using System.Windows.Forms;

namespace FieldBossTimer
{
    public class PanelData
    {
        public ComboBox ComboBox { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Timer Timer { get; set; }
        public Label TimeLabel { get; set; }
        public Button StartButton { get; set; }
        public Button StopButton { get; set; }
        public Button ResetButton { get; set; }
    }
}
