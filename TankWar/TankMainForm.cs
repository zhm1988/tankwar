using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TankWar
{
    public partial class TankMainForm : Form
    {
        public const int ScreenWidth = 800; //窗体宽度
        public const int ScreenHeight = 600; //窗体高度        
        Tank HeroTank = new Tank(5);
        Tank enemy1;       
        public TankMainForm()
        {
            InitializeComponent();
            //设置双缓冲 解决闪烁问题
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(updateui));
            thread.Start();
        }

        #region EVENT

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TankMainForm_Load(object sender, EventArgs e)
        {
            InitMainForm();
            Random r = new Random();
            enemy1 = new Tank(new Point(r.Next(0, 800), r.Next(0, 600)), new Size(), 50);
        }
        /// <summary>
        /// 窗体重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TankMainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Bitmap bitmap = new Bitmap(ScreenWidth, ScreenHeight);
            Graphics bufferGraphics = Graphics.FromImage(bitmap);
            bufferGraphics.Clear(this.BackColor);
            Rectangle rect = new Rectangle(0, 0, ScreenWidth, ScreenHeight);
            bufferGraphics.FillRectangle(new SolidBrush(Color.Green), rect);
            DrawTank(bufferGraphics);
            DrawEnemyTank(bufferGraphics);
            g.DrawImage(bitmap, 0, 0);
            bufferGraphics.Dispose();
            bitmap.Dispose();
            //this.Invalidate(rect);
            Thread.Sleep(50);
           
        }
        /// <summary>
        /// 窗体按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TankMainForm_KeyDown(object sender, KeyEventArgs e)
        {
            HeroTank.KeyDown(e);
            if (e.KeyCode == Keys.A)
            {
                HeroTank.Shoot();
            }
        }

        private void TankMainForm_KeyUp(object sender, KeyEventArgs e)
        {
            HeroTank.KeyUp(e);
        }

        #endregion

        #region 私有方法

        //初始化窗体
        private void InitMainForm()
        {
            this.Width = ScreenWidth;
            this.Height = ScreenHeight;
            this.BackColor = Color.Green;
            this.MaximizeBox = false;
        }

        //画出HeroTank
        private void DrawTank(Graphics g)
        {
            HeroTank.DrawSelf(g);
            if (HeroTank.Bullets != null && HeroTank.Bullets.Count > 0)
            {
                foreach (var item in HeroTank.Bullets)
                {
                    item.DrawSelf(g);
                }
            }            
        }

        private void DrawEnemyTank(Graphics g) 
        {  
            enemy1.DrawSelf(g);
            enemy1.TankDirection = Direction.RIGHT;
            if (enemy1.Bullets != null && enemy1.Bullets.Count > 0)
            {
                foreach (var item in enemy1.Bullets)
                {
                    item.DrawSelf(g);
                }
            }            
           //enemy1.Shoot();
        }

        private void updateui()
        {
            while (true)
            {
                Thread.Sleep(50);
                this.Refresh();
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

    }
}
