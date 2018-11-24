using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Amaze
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			maze = new Maze(25, 50);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}

		Maze maze;

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			maze.Clear();
			int i = 0;
			maze.Initialize(() =>
			{
				if (i % 1 == 0)
				{
					Invalidate();
					Application.DoEvents();
				}
				i++;
			});
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			maze.Draw(e.Graphics, 35);
		}
	}
}
