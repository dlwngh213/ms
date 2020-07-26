using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FieldBossTimer
{
    public partial class MainForm : Form
    {
        private static readonly string _panelName = "panel";
        private static readonly string _comboBoxName = "comboBox";
        private static readonly string _timeLabelName = "timeLabel";
        private static readonly string _noneItemText = "(None)";

        private static readonly Color _defaultTimeLabelColor = SystemColors.ActiveBorder;
        private static readonly Color _finishedTimeLabelColor = Color.DarkRed;

        private readonly List<PanelData> _panelData = new List<PanelData>();
        private readonly List<ComboBox> _comboBoxes = new List<ComboBox>();
        private readonly List<Timer> _timers = new List<Timer>();
        private Dictionary<string, int> _bossTimers;
        private bool _useFlash = false;
        private bool _popupSaveResult = true;

        public List<string> BossOptions { get; set; }

        public MainForm()
        {
            bool.TryParse(ConfigurationManager.AppSettings["useFlash"], out _useFlash);
            bool.TryParse(ConfigurationManager.AppSettings["popupSaveResult"], out _popupSaveResult);
            InitializeComponent();
            MapTimers();
            MapControls();
            LoadBossData();
            LoadUserData();
        }

        private void MapTimers()
        {
            _timers.Add(timer1);
            _timers.Add(timer2);
            _timers.Add(timer3);
            _timers.Add(timer4);
            _timers.Add(timer5);
            _timers.Add(timer6);
            _timers.Add(timer7);
            _timers.Add(timer8);
            _timers.Add(timer9);
            _timers.Add(timer10);
            _timers.Add(timer11);
            _timers.Add(timer12);
            _timers.Add(timer13);
            _timers.Add(timer14);
            _timers.Add(timer15);
            _timers.Add(timer16);
            _timers.Add(timer17);
            _timers.Add(timer18);
            _timers.Add(timer19);
            _timers.Add(timer20);
            _timers.Add(timer21);
            _timers.Add(timer22);
            _timers.Add(timer23);
            _timers.Add(timer24);
            _timers.Add(timer25);
            _timers.Add(timer26);
            _timers.Add(timer27);
            _timers.Add(timer28);
            _timers.Add(timer29);
            _timers.Add(timer30);
            _timers.ForEach(x => x.Tick += OnTimerTick);
        }

        private void MapControls()
        {
            var panels = Controls.OfType<Panel>();
            toolStripComboBox1.SelectedIndexChanged += OnToolStripComboBoxChanged;

            foreach (var panel in panels)
            {
                var index = int.Parse(panel.Name.Replace(_panelName, ""));
                var comboBox = panel.Controls.OfType<ComboBox>().FirstOrDefault();
                var timeLabel = panel.Controls.OfType<Label>().Where(x => x.Name.StartsWith(_timeLabelName)).FirstOrDefault();
                var buttons = panel.Controls.OfType<Button>();

                comboBox.SelectedValueChanged += OnComboBoxSelectionChanged;
                _comboBoxes.Add(comboBox);

                _panelData.Add(new PanelData
                {
                    ComboBox = comboBox,
                    ResetButton = buttons.Where(x => x.Name.StartsWith("reset", StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                    StartButton = buttons.Where(x => x.Name.StartsWith("start", StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                    StopButton = buttons.Where(x => x.Name.StartsWith("stop", StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                    TimeLabel = timeLabel,
                    Timer = _timers[index - 1],
                    TimeSpan = TimeSpan.Zero
                });
            }
        }

        private void OnToolStripComboBoxChanged(object sender, EventArgs e)
        {
            var selectedItem = (string)toolStripComboBox1.SelectedItem ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(selectedItem))
                _comboBoxes.ForEach(x => x.SelectedItem = selectedItem);

            channelLabel1.Focus();
        }

        private void OnComboBoxSelectionChanged(object sender, EventArgs e)
        {
            var panelData = _panelData.Where(x => x.ComboBox == sender).FirstOrDefault();

            if (panelData == null)
                return;

            panelData.Timer.Stop();
            panelData.TimeSpan = GetBossTimeSpan(panelData.ComboBox);

            UpdateTimeLabel(panelData, false);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var panelData = _panelData.Where(x => x.Timer == sender).FirstOrDefault();

            if (panelData == null)
                return;

            panelData.TimeSpan = panelData.TimeSpan.Subtract(new TimeSpan(0, 0, 1));
            UpdateTimeLabel(panelData, true);
        }

        private void UpdateTimeLabel(PanelData panelData, bool colorRedOnZero)
        {
            if (panelData.TimeSpan == TimeSpan.Zero && colorRedOnZero || panelData.TimeSpan < TimeSpan.Zero)
            {
                panelData.TimeLabel.BackColor = _finishedTimeLabelColor;

                if (_useFlash && Form.ActiveForm != this)
                    this.FlashNotification();

            }

            panelData.TimeLabel.Text = panelData.TimeSpan.ToString();
        }

        private void LoadBossData()
        {
            _bossTimers = FileUtil.LoadBossTimersData();

            if (!_bossTimers.Any())
            {
                FileUtil.GenerateSampleBossTimersFile();
                _bossTimers = FileUtil.LoadBossTimersData();
                MessageBox.Show($"\nThe file {FileUtil.BossTimersFile} was generated with sample lines in the same directory as this .exe." +
                    $"\nUpdate the file with your own values and then reload (Alt + F + R) to have those options in the drop down items." +
                    $"\nSee the help menu (Alt + H) for more details.", "Data Load Result", MessageBoxButtons.OK);
            }

            SetComboBoxData();
        }

        private void SetComboBoxData()
        {
            var bosses = _bossTimers.Keys.ToArray();
            Array.Sort(bosses);

            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.Add(_noneItemText);
            toolStripComboBox1.Items.AddRange(bosses);

            foreach (var comboBox in _comboBoxes)
            {
                comboBox.Items.Clear();
                comboBox.Items.Add(_noneItemText);
                comboBox.Items.AddRange(bosses);
                comboBox.SelectedItem = comboBox.Items[0];
            }
        }

        private void LoadUserData()
        {
            var userData = FileUtil.LoadUserSaveData();

            if (!userData.Any())
                return;

            foreach (var kvp in userData)
            {
                var comboBox = _comboBoxes.Find(x => x.Name.Equals(_comboBoxName + kvp.Key));
                comboBox.SelectedItem = kvp.Value;
            }
        }

        private void SaveUserData()
        {
            var data = new List<List<string>>();

            foreach (var comboBox in _comboBoxes)
            {
                var index = comboBox.Name.Replace(_comboBoxName, "");
                var value = (string)comboBox.SelectedItem;

                if (!value?.Equals(_noneItemText) ?? false)
                    data.Add(new List<string> { index, value });
            }

            string message = "Successfully saved";
            MessageBoxIcon icon = MessageBoxIcon.None;

            try
            {
                FileUtil.SaveCurrentValues(data);
            }
            catch (Exception e)
            {
                message = $"Error while saving...\n{e.Message}";
                icon = MessageBoxIcon.Error;
            }
            finally
            {
                if (_popupSaveResult)
                    MessageBox.Show(message, "Save Result", MessageBoxButtons.OK, icon);
            }
        }

        private TimeSpan GetBossTimeSpan(ComboBox comboBox)
        {
            var selectedItem = (string)comboBox.SelectedItem ?? string.Empty;

            if (_bossTimers.TryGetValue(selectedItem, out int time))
                return new TimeSpan(time * TimeSpan.TicksPerSecond);
            else
                return TimeSpan.Zero;
        }

        private void StartAllTimers()
        {
            _panelData.ForEach(x =>
            {
                if (!((string)x.ComboBox.SelectedItem ?? string.Empty).Equals(_noneItemText))
                    x.Timer.Start();
            });
        }

        private void StopAllTimers()
        {
            _timers.ForEach(x => x.Stop());
        }

        private void ResetAllTimeSpans()
        {
            _panelData.ForEach(x => ResetTimeSpan(x));
        }

        private void ResetTimeSpan(PanelData panelData)
        {
            panelData.Timer.Stop();
            panelData.TimeLabel.BackColor = _defaultTimeLabelColor;
            panelData.TimeSpan = GetBossTimeSpan(panelData.ComboBox);
            UpdateTimeLabel(panelData, false);
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetAllTimeSpans();
            LoadBossData();
            LoadUserData();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveUserData();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HelpForm().Show();
        }

        private void startAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartAllTimers();
        }

        private void stopAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAllTimers();
        }

        private void resetAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAllTimers();
            ResetAllTimeSpans();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            var panelData = _panelData.Where(x => x.StartButton == sender).FirstOrDefault();

            if (panelData != null)
                panelData.Timer.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            var panelData = _panelData.Where(x => x.StopButton == sender).FirstOrDefault();

            if (panelData != null)
                panelData.Timer.Stop();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            var panelData = _panelData.Where(x => x.ResetButton == sender).FirstOrDefault();

            if (panelData != null)
                ResetTimeSpan(panelData);
        }

        private void timeLabel_DoubleClick(object sender, EventArgs e)
        {
            var panelData = _panelData.Where(x => x.TimeLabel == sender).FirstOrDefault();

            using (var timeForm = new TimeInputForm())
            {
                var result = timeForm.ShowDialog();

                if (result == DialogResult.OK && timeForm.Time > 0)
                {
                    panelData.Timer.Stop();
                    panelData.TimeSpan = new TimeSpan(timeForm.Time * TimeSpan.TicksPerSecond);
                    UpdateTimeLabel(panelData, false);
                }
            }
        }
    }
}
