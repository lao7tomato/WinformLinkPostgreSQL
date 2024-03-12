namespace WindowsFormsApp2
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.spatialDockManager1 = new DotSpatial.Controls.SpatialDockManager();
            this.legend1 = new DotSpatial.Controls.Legend();
            this.map1 = new DotSpatial.Controls.Map();
            this.spatialToolStrip1 = new DotSpatial.Controls.SpatialToolStrip();
            this.InPut = new System.Windows.Forms.ToolStripButton();
            this.OutBrowse = new System.Windows.Forms.ToolStripButton();
            this.Test = new System.Windows.Forms.ToolStripButton();
            this.ComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.spatialDockManager1)).BeginInit();
            this.spatialDockManager1.Panel1.SuspendLayout();
            this.spatialDockManager1.Panel2.SuspendLayout();
            this.spatialDockManager1.SuspendLayout();
            this.spatialToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spatialDockManager1
            // 
            this.spatialDockManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spatialDockManager1.Location = new System.Drawing.Point(0, 28);
            this.spatialDockManager1.Name = "spatialDockManager1";
            // 
            // spatialDockManager1.Panel1
            // 
            this.spatialDockManager1.Panel1.Controls.Add(this.legend1);
            // 
            // spatialDockManager1.Panel2
            // 
            this.spatialDockManager1.Panel2.Controls.Add(this.map1);
            this.spatialDockManager1.Size = new System.Drawing.Size(1034, 581);
            this.spatialDockManager1.SplitterDistance = 344;
            this.spatialDockManager1.TabControl1 = null;
            this.spatialDockManager1.TabControl2 = null;
            this.spatialDockManager1.TabIndex = 1;
            // 
            // legend1
            // 
            this.legend1.BackColor = System.Drawing.Color.White;
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 345, 581);
            this.legend1.Dock = System.Windows.Forms.DockStyle.Left;
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 187, 428);
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Location = new System.Drawing.Point(0, 0);
            this.legend1.MinimumSize = new System.Drawing.Size(5, 5);
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = null;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.Size = new System.Drawing.Size(345, 581);
            this.legend1.TabIndex = 0;
            this.legend1.Text = "legend1";
            this.legend1.UseLegendForSelection = true;
            this.legend1.VerticalScrollEnabled = true;
            // 
            // map1
            // 
            this.map1.AllowDrop = true;
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.CollisionDetection = false;
            this.map1.Dock = System.Windows.Forms.DockStyle.Right;
            this.map1.ExtendBuffer = false;
            this.map1.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map1.IsBusy = false;
            this.map1.IsZoomedToMaxExtent = false;
            this.map1.Legend = this.legend1;
            this.map1.Location = new System.Drawing.Point(3, 0);
            this.map1.Name = "map1";
            this.map1.ProgressHandler = null;
            this.map1.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.RedrawLayersWhileResizing = false;
            this.map1.SelectionEnabled = true;
            this.map1.Size = new System.Drawing.Size(683, 581);
            this.map1.TabIndex = 0;
            this.map1.ZoomOutFartherThanMaxExtent = false;
            // 
            // spatialToolStrip1
            // 
            this.spatialToolStrip1.ApplicationManager = null;
            this.spatialToolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.spatialToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InPut,
            this.OutBrowse,
            this.Test,
            this.ComboBox1});
            this.spatialToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.spatialToolStrip1.Map = this.map1;
            this.spatialToolStrip1.Name = "spatialToolStrip1";
            this.spatialToolStrip1.Size = new System.Drawing.Size(1034, 28);
            this.spatialToolStrip1.TabIndex = 0;
            this.spatialToolStrip1.Text = "spatialToolStrip1";
            // 
            // InPut
            // 
            this.InPut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InPut.Image = ((System.Drawing.Image)(resources.GetObject("InPut.Image")));
            this.InPut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InPut.Name = "InPut";
            this.InPut.Size = new System.Drawing.Size(50, 25);
            this.InPut.Text = "InPut";
            this.InPut.Click += new System.EventHandler(this.InPut_Click);
            // 
            // OutBrowse
            // 
            this.OutBrowse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OutBrowse.Image = ((System.Drawing.Image)(resources.GetObject("OutBrowse.Image")));
            this.OutBrowse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OutBrowse.Name = "OutBrowse";
            this.OutBrowse.Size = new System.Drawing.Size(93, 25);
            this.OutBrowse.Text = "OutBrowse";
            this.OutBrowse.Click += new System.EventHandler(this.OutBrowse_Click);
            // 
            // Test
            // 
            this.Test.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Test.Image = ((System.Drawing.Image)(resources.GetObject("Test.Image")));
            this.Test.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(44, 25);
            this.Test.Text = "Test";
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // ComboBox1
            // 
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(121, 28);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 609);
            this.Controls.Add(this.spatialDockManager1);
            this.Controls.Add(this.spatialToolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.spatialDockManager1.Panel1.ResumeLayout(false);
            this.spatialDockManager1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spatialDockManager1)).EndInit();
            this.spatialDockManager1.ResumeLayout(false);
            this.spatialToolStrip1.ResumeLayout(false);
            this.spatialToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DotSpatial.Controls.SpatialDockManager spatialDockManager1;
        private DotSpatial.Controls.Legend legend1;
        private DotSpatial.Controls.Map map1;
        private DotSpatial.Controls.SpatialToolStrip spatialToolStrip1;
        private System.Windows.Forms.ToolStripButton InPut;
        private System.Windows.Forms.ToolStripButton OutBrowse;
        private System.Windows.Forms.ToolStripButton Test;
        private System.Windows.Forms.ToolStripComboBox ComboBox1;
    }
}

