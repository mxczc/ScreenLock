
namespace LockControl
{
    partial class ScreenLock
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_bg = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnl_bg
            // 
            this.pnl_bg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_bg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_bg.Location = new System.Drawing.Point(0, 0);
            this.pnl_bg.Name = "pnl_bg";
            this.pnl_bg.Size = new System.Drawing.Size(270, 270);
            this.pnl_bg.TabIndex = 5;
            this.pnl_bg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawMouseDown);
            this.pnl_bg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawMouseMove);
            this.pnl_bg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawMouseUp);
            // 
            // ScreenLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_bg);
            this.Name = "ScreenLock";
            this.Size = new System.Drawing.Size(270, 270);
            this.Load += new System.EventHandler(this.ScreenLock_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_bg;
    }
}
