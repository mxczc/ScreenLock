
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WinformExample.Properties;

namespace WinformExample
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void screenLock1_OnUlocked(LockControl.UnlockEventArgs obj)
        {
            MessageBox.Show(obj.Message);
        }
    }
}
