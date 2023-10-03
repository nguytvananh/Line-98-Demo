using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
namespace Lines
{
	public partial class MainForm : XtraForm
	{
		GamePanel panel;
		string currentFileName = "";

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			panel = new GamePanel(this);
			this.Controls.Add(panel);
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panel.newGame();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}

		private void endGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			panel.setGameOver();
		}


		private void highScoresToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new HighScoresForm().ShowDialog(this);
		}

		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmAbout().ShowDialog(this);
		}
	}
}