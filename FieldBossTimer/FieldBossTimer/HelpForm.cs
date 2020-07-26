using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FieldBossTimer
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            helpText.Text = 
@"This is a simple application to keep track of field boss timers

Data
------------------------------------------
- This app uses 2 files. (1) bossTimers.csv (2) userData.csv
- (1) is generated during the first launch
- (2) is generated on the first save
- They are both in the same directory as this.exe

Boss Time Data
------------------------------------------
- The times are read from file(1)
- The format is <Boss Name>,<Time In Seconds> without the angle brackets
- Update this file to show the bosses in the drop down menus
- After you update the file and save it, you can reload the app under File > Reload

Save Data
------------------------------------------
- You can save which boss is currently set on every channel
- File(2) is used to set the default boss selection per channel in future loads
- You can also manually edit this file with the format of <Channel Number>,<Boss Name> without the angle brackets

Convenience
------------------------------------------
- There are convenience buttons on the menu bar to change all channels at once
- Start All, Stop All, Reset All, and the top selection to update all the channels
- Some keyboard shortcuts are available by holding Alt and pressing the underlined letter. E.g.Pressing Alt +R triggers Reset All
- The app was designed around reading data from file (1) but you can also double click individual timers to change them on the fly for one time use

Settings
------------------------------------------
- You can update the.config file to set if this app should flash when a timer is done, and if it pop ups the save result
- Open the file in a text editor and change the ""useFlash"" or ""popupSaveResult"" value to ""true"" or ""false""";
        }
    }
}
