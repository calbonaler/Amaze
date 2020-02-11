using System;
using System.Windows.Forms;

namespace Amaze
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			m_CellSize = 2;
		}

		int m_CellSize;
		Maze m_Maze;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			tstCellSize.Text = m_CellSize.ToString();
		}

		private async void TsbGenerate_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(tstRowCount.Text, out var rowCount) || rowCount <= 0)
			{
				tslStatus.Text = "Row Count: Validation failed";
				return;
			}
			if (!int.TryParse(tstColumnCount.Text, out var columnCount) || columnCount <= 0)
			{
				tslStatus.Text = "Column Count: Validation failed";
				return;
			}
			if (m_Maze != null && rowCount == m_Maze.RowCount && columnCount == m_Maze.ColumnCount)
				m_Maze.Clear();
			else
				m_Maze = new Maze(rowCount, columnCount);
			picViewer.Invalidate();
			var previousProgress = 0;
			await m_Maze.GenerateAsync(new Progress<int>(progress =>
			{
				tslStatus.Text = "Generation: " + progress + "% completed";
				if (progress - previousProgress >= 10)
				{
					picViewer.Invalidate();
					previousProgress = progress;
				}
			}));
			picViewer.Invalidate();
		}

		private void TsbSolve_Click(object sender, EventArgs e)
		{
			var result = false;
			if (m_Maze != null)
			{
				result = m_Maze.Solve(0, 0, m_Maze.RowCount - 1, m_Maze.ColumnCount - 1);
				picViewer.Invalidate();
			}
			if (result)
				tslStatus.Text = "Solved successfully!";
			else
				tslStatus.Text = "Solving failed";
		}

		private void PicViewer_Paint(object sender, PaintEventArgs e)
		{
			m_Maze?.Draw(e.Graphics, m_CellSize);
		}

		private void TstCellSize_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
				TstCellSize_Leave(sender, e);
		}

		private void TstCellSize_Leave(object sender, EventArgs e)
		{
			if (int.TryParse(tstCellSize.Text, out var cellSize) && cellSize > 1)
			{
				m_CellSize = cellSize;
				picViewer.Invalidate();
				tslStatus.Text = "Cell Size: Updated successfully!";
			}
			else
			{
				tstCellSize.Text = m_CellSize.ToString();
				tslStatus.Text = "Cell Size: Validation failed";
			}
		}
	}
}
