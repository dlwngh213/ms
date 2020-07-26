using System;
using System.Windows.Forms;

namespace FieldBossTimer
{
    public partial class TimeInputForm : Form
    {
        public TimeInputForm()
        {
            InitializeComponent();
        }

        public long Time 
        {
            get
            {
                var values = dateTimePicker1.Text.Split(':');
                var hours = int.Parse(values[0]);
                var minutes = int.Parse(values[1]);
                var seconds = int.Parse(values[2]);

                return (hours * 60 + minutes) * 60 + seconds;
            }
        }

        private void TimeInputForm_Load(object sender, EventArgs e)
        {
            okButton.DialogResult = DialogResult.OK;
            dateTimePicker1.CustomFormat = "HH:mm:ss";
            dateTimePicker1.Value = DateTime.Today;
        }
    }
}
