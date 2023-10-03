
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lines
{
	public partial class FrmInputName : DevExpress.XtraEditors.XtraForm
	{
		Game g;
		
		public FrmInputName(Game g)
		{
			this.g = g;
			InitializeComponent();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			if (textEdit1.Text == "")
				DevExpress.XtraEditors.XtraMessageBox.Show("Invalid name!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
			{
				Player p = new Player(textEdit1.Text, g.scores, g.hours, g.minutes, g.seconds);
				Score.insert(p);
				this.Close();
				new HighScoresForm().ShowDialog();
			}
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}