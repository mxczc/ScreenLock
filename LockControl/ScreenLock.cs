using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace LockControl
{
    public partial class ScreenLock : UserControl
    {
        public ScreenLock()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 返回解锁结果事件
        /// </summary>
        public event Action<UnlockEventArgs> OnUlocked;
        /// <summary>
        /// 解锁密码
        /// </summary>
        public string Password { get; set; } = "12345678";
        //九宫格的九个点
        private List<Point> InitialPoints = new List<Point>();
        //解锁点列表
        private List<Point> UnLockPoints = new List<Point>();
        //绘制九宫格圆心点的画笔
        private Pen DrawPen = new Pen(Color.FromArgb(200, Color.Green), 5);
        //是否开始绘制解锁（鼠标左键按下时开始）
        private bool _isDraw = false;
        //是否选中九宫格中的其中一个点
        private bool _isSelected = false;
        //临时存储密码字符串
        public string PasswordStr = string.Empty;

        #region 控件事件
        /// <summary>
        /// 鼠标点击左键且移动时开始绘制解锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraw)
            {
                //如果还未确定九宫格的点则实时改变最后一个点的坐标
                if (!_isSelected)
                {
                    UnLockPoints[UnLockPoints.Count - 1] = e.Location;

                }
                else//如果确定了则再添加一个改变点
                {
                    UnLockPoints.Add(e.Location);
                    _isSelected = false;
                }
                AddPoint(e.Location);

            }
            ShowAreaAndLine();
            //GC.Collect();
        }
        /// <summary>
        /// 鼠标左键弹起时结束绘制九宫格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawMouseUp(object sender, MouseEventArgs e)
        {
            if (_isDraw)
            {
                //停止绘制标示
                _isDraw = false;
                //清除绘制的九宫格点
                UnLockPoints.Clear();
                //显示密码（可用来判断九宫格密码）
                if (PasswordStr.Length < 4)
                {
                    OnUlocked?.Invoke(new UnlockEventArgs() { IsSuc = false, Message = "九宫格密码不能小于4个点", Password = PasswordStr });
                }
                else
                {
                    if (PasswordStr == Password.Replace(" ", ""))
                    {
                        OnUlocked?.Invoke(new UnlockEventArgs() { IsSuc = true, Message = "解锁成功", Password = PasswordStr });
                    }
                    else
                    {
                        OnUlocked?.Invoke(new UnlockEventArgs() { IsSuc = false, Message = "解锁密码错误", Password = PasswordStr });
                    }
                }
                PasswordStr = "";
                //绘制九宫格（此处相当于初始化九宫格）
                ShowAreaAndLine();
            }
        }
        /// <summary>
        /// 鼠标左键点击时触发开始绘制九宫格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawMouseDown(object sender, MouseEventArgs e)
        {
            //开始绘制标记
            _isDraw = true;
            //添加第一个点击的点
            UnLockPoints.Add(e.Location);
            //显示九宫格解锁图案
            ShowAreaAndLine();
        }
        private void ScreenLock_Load(object sender, EventArgs e)
        {
            CreadtPoint();
            CreateCanvas();
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化九宫格的九个点
        /// </summary>
        private void CreadtPoint()
        {
            InitialPoints.Clear();
            int width = pnl_bg.Width / 3;
            int height = pnl_bg.Height / 3;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    InitialPoints.Add(new Point(width * y + width / 2, height * x + height / 2));
                }
            }
        }
        private void CreateCanvas()
        {
            //添加一块画布
            Bitmap backBit = new Bitmap(pnl_bg.Width, pnl_bg.Height);
            Graphics g = Graphics.FromImage(backBit);
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清除Graphics
            g.Clear(pnl_bg.BackColor);
            //绘制画布
            g.DrawImage(backBit, new Rectangle(0, 0, pnl_bg.Width, pnl_bg.Height),
                new Rectangle(0, 0, pnl_bg.Width, pnl_bg.Height), GraphicsUnit.Pixel);
            //绘制九宫格的点
            for (int i = 0; i < InitialPoints.Count; i++)
            {
                g.DrawImage(LockControl.Resource.Shape, new Point(InitialPoints[i].X - 20, InitialPoints[i].Y - 20));
            }
            pnl_bg.BackgroundImage = backBit;
        }

        /// <summary>
        /// 判断传入的点是否九宫格中的点
        /// </summary>
        /// <param name="p">点</param>
        private void AddPoint(Point p)
        {
            for (int i = 0; i < InitialPoints.Count; i++)
            {
                //遍历九宫格的点，鼠标经过的点是否在其中一个九宫格的点的范围内
                if ((p.X > InitialPoints[i].X - 20 && p.X < InitialPoints[i].X + 20) && (p.Y > InitialPoints[i].Y - 20 && p.Y < InitialPoints[i].Y + 20))
                {
                    //判断九宫格的点是否已经选择过，若选择过则不处理
                    if (!(PasswordStr.IndexOf((i + 1).ToString()) > -1))
                    {
                        //若尚未选择过该点则添加
                        UnLockPoints[UnLockPoints.Count - 1] = (InitialPoints[i]);
                        //追加密码字符串
                        PasswordStr = PasswordStr + (i + 1);
                        _isSelected = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 绘制九宫格解锁图案
        /// </summary>
        private void ShowAreaAndLine()
        {
            try
            {
                //添加一块画布
                Bitmap backBit = new Bitmap(pnl_bg.Width, pnl_bg.Height);
                Graphics g = Graphics.FromImage(backBit);
                g.SmoothingMode = SmoothingMode.HighQuality;
                //清除Graphics
                g.Clear(pnl_bg.BackColor);
                //绘制画布
                g.DrawImage(backBit, new Rectangle(0, 0, pnl_bg.Width, pnl_bg.Height),
                    new Rectangle(0, 0, pnl_bg.Width, pnl_bg.Height), GraphicsUnit.Pixel);
                //绘制九宫格的点
                for (int i = 0; i < InitialPoints.Count; i++)
                {
                    g.DrawImage(LockControl.Resource.Shape, new Point(InitialPoints[i].X - 20, InitialPoints[i].Y - 20));
                }
                //绘制选中的解锁点
                int pointIndex = 1;
                Point pointEnd;
                Point pointStart = new Point(0, 0);
                for (int i = 0; i < UnLockPoints.Count; i++)
                {
                    //绘制选中点背景
                    if (i < UnLockPoints.Count - 1 || _isSelected)
                    {
                        //高亮绘制选中点
                        g.DrawImage(LockControl.Resource.Shape, new Point(UnLockPoints[i].X - 20, UnLockPoints[i].Y - 20));
                    }
                    pointEnd = UnLockPoints[i];
                    //绘制两点连接的直线
                    if (pointIndex > 1)
                    {
                        g.DrawLine(DrawPen, pointStart, pointEnd);
                    }
                    //绘制小圆，覆盖两条线段的连接处
                    if (i < UnLockPoints.Count - 1 || _isSelected)
                    {
                        //绘制区域（）
                        Rectangle rg = new Rectangle(UnLockPoints[i].X - 6, UnLockPoints[i].Y - 6, 11, 11);
                        g.DrawEllipse(DrawPen, rg);
                        //画空心圆
                        Brush bru = new SolidBrush(Color.FromArgb(255, Color.Green));
                        g.FillEllipse(bru, rg);
                        //填充空心圆，实心圆

                    }
                    pointStart = UnLockPoints[i];
                    pointIndex++;

                }
                g.Dispose();
                pnl_bg.CreateGraphics().DrawImage(backBit, 0, 0);
                backBit.Dispose();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        #endregion
    }

    /// <summary>
    /// 解锁结果信息
    /// </summary>
    public class UnlockEventArgs : EventArgs
    {
        //是否解锁成功
        public bool IsSuc { get; set; }
        //消息
        public string Message { get; set; }
        //解锁密码
        public string Password { get; set; }
    }
}
