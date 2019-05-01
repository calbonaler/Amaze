namespace Amaze
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tslRowCount = new System.Windows.Forms.ToolStripLabel();
			this.tstRowCount = new System.Windows.Forms.ToolStripTextBox();
			this.tslColumnCount = new System.Windows.Forms.ToolStripLabel();
			this.tstColumnCount = new System.Windows.Forms.ToolStripTextBox();
			this.tslCellSize = new System.Windows.Forms.ToolStripLabel();
			this.tstCellSize = new System.Windows.Forms.ToolStripTextBox();
			this.tsbGenerate = new System.Windows.Forms.ToolStripButton();
			this.tsbSolve = new System.Windows.Forms.ToolStripButton();
			this.tslStatus = new System.Windows.Forms.ToolStripLabel();
			this.picViewer = new System.Windows.Forms.PictureBox();
			this.tsMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslRowCount,
            this.tstRowCount,
            this.tslColumnCount,
            this.tstColumnCount,
            this.tslCellSize,
            this.tstCellSize,
            this.tsbGenerate,
            this.tsbSolve,
            this.tslStatus});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(437, 25);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tslRowCount
			// 
			this.tslRowCount.Name = "tslRowCount";
			this.tslRowCount.Size = new System.Drawing.Size(38, 22);
			this.tslRowCount.Text = "Rows:";
			// 
			// tstRowCount
			// 
			this.tstRowCount.Name = "tstRowCount";
			this.tstRowCount.Size = new System.Drawing.Size(50, 25);
			// 
			// tslColumnCount
			// 
			this.tslColumnCount.Name = "tslColumnCount";
			this.tslColumnCount.Size = new System.Drawing.Size(56, 22);
			this.tslColumnCount.Text = "Columns:";
			// 
			// tstColumnCount
			// 
			this.tstColumnCount.Name = "tstColumnCount";
			this.tstColumnCount.Size = new System.Drawing.Size(50, 25);
			// 
			// tslCellSize
			// 
			this.tslCellSize.Name = "tslCellSize";
			this.tslCellSize.Size = new System.Drawing.Size(52, 22);
			this.tslCellSize.Text = "Cell Size:";
			// 
			// tstCellSize
			// 
			this.tstCellSize.Name = "tstCellSize";
			this.tstCellSize.Size = new System.Drawing.Size(50, 25);
			this.tstCellSize.Leave += new System.EventHandler(this.TstCellSize_Leave);
			this.tstCellSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TstCellSize_KeyPress);
			// 
			// tsbGenerate
			// 
			this.tsbGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGenerate.Name = "tsbGenerate";
			this.tsbGenerate.Size = new System.Drawing.Size(58, 22);
			this.tsbGenerate.Text = "Generate";
			this.tsbGenerate.Click += new System.EventHandler(this.TsbGenerate_Click);
			// 
			// tsbSolve
			// 
			this.tsbSolve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbSolve.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSolve.Name = "tsbSolve";
			this.tsbSolve.Size = new System.Drawing.Size(39, 22);
			this.tsbSolve.Text = "Solve";
			this.tsbSolve.Click += new System.EventHandler(this.TsbSolve_Click);
			// 
			// tslStatus
			// 
			this.tslStatus.Name = "tslStatus";
			this.tslStatus.Size = new System.Drawing.Size(0, 22);
			// 
			// picViewer
			// 
			this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picViewer.Location = new System.Drawing.Point(0, 25);
			this.picViewer.Name = "picViewer";
			this.picViewer.Size = new System.Drawing.Size(437, 307);
			this.picViewer.TabIndex = 1;
			this.picViewer.TabStop = false;
			this.picViewer.Paint += new System.Windows.Forms.PaintEventHandler(this.PicViewer_Paint);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(437, 332);
			this.Controls.Add(this.picViewer);
			this.Controls.Add(this.tsMain);
			this.Name = "MainForm";
			this.Text = "Amaze";
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton tsbGenerate;
		private System.Windows.Forms.ToolStripButton tsbSolve;
		private System.Windows.Forms.ToolStripLabel tslStatus;
		private System.Windows.Forms.PictureBox picViewer;
		private System.Windows.Forms.ToolStripLabel tslRowCount;
		private System.Windows.Forms.ToolStripTextBox tstRowCount;
		private System.Windows.Forms.ToolStripLabel tslColumnCount;
		private System.Windows.Forms.ToolStripTextBox tstColumnCount;
		private System.Windows.Forms.ToolStripLabel tslCellSize;
		private System.Windows.Forms.ToolStripTextBox tstCellSize;
	}
}

