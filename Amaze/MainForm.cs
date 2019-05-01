using System;
using System.Windows.Forms;

namespace Amaze
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			maze = new Maze(100, 100);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}

		readonly Maze maze;

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			maze.Clear();
			var i = 0;
			maze.Initialize(() =>
			{
				if (i % 500 == 0)
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
			maze.Draw(e.Graphics, 10);
		}
	}
}
