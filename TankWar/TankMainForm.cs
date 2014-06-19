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
        Thread MainUIThread = null;         //主界面线程
        Thread HeroTankThread = null;       //HeroTank线程
        Thread EnemyTankThread = null;      //EnemyTank线程
        Tank HeroTank = null;
        Tank enemy1 = null;
        List<Tank> Enemys = null;
        public TankMainForm()
        {
            InitializeComponent();
            InitMainForm();
            InitTank();
            //设置双缓冲 解决闪烁问题
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            CheckForIllegalCrossThreadCalls = false;
        }

        //原来底层重绘每次会清除画布，然后再全部重新绘制，这才是导致闪烁最主要的原因
        protected override void WndProc(ref Message m)
        {
            // 禁掉清除背景消息
            if (m.Msg == 0x0014)
                return;
            base.WndProc(ref m);
        }

        #region EVENT

        private void TankMainForm_Load(object sender, EventArgs e)
        {
            MainUIThread = new Thread(new ThreadStart(UpdateUI));
            MainUIThread.Priority = ThreadPriority.Highest;
            MainUIThread.Start();
            MainUIThread.IsBackground = true;
            HeroTankThread = new Thread(new ThreadStart(UpdateHero));
            HeroTankThread.Priority = ThreadPriority.BelowNormal;
            HeroTankThread.Start();
            HeroTankThread.IsBackground = true;
            EnemyTankThread = new Thread(new ThreadStart(UpdateEnemy));
            EnemyTankThread.Priority = ThreadPriority.BelowNormal;
            EnemyTankThread.Start();
            EnemyTankThread.IsBackground = true;
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
                HeroTank.Shoot();
        }

        private void TankMainForm_KeyUp(object sender, KeyEventArgs e)
        {
            HeroTank.KeyUp(e);
        }       

        private void TankMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                MainUIThread.Join(1);
                MainUIThread.Abort();
                HeroTankThread.Join(1);
                HeroTankThread.Abort();
                EnemyTankThread.Join(1);
                EnemyTankThread.Abort();               
            }
            catch
            {

            }
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
        //初始化Tank
        private void InitTank()
        {
            HeroTank = new Tank(5);
            Random r = new Random();
            enemy1 = new Tank(new Point(r.Next(0, 800), r.Next(0, 600)), new Size(), 50);
            Enemys = new List<Tank>() { new Tank(new Point(r.Next(0, 750), r.Next(0, 550)), new Size(), 50), new Tank(new Point(r.Next(0, 750), r.Next(0, 550)), new Size(), 50) };
        }

        //画出Tank
        private void DrawTank(Graphics g,Tank tank)
        {
            tank.DrawSelf(g);
            if (tank.Bullets != null && tank.Bullets.Count > 0)
            {
                foreach (var item in tank.Bullets)
                {
                    item.DrawSelf(g);
                }
            }
        }
       
        //刷新主界面
        private void UpdateUI()
        {
            while (true)
            {                
                Graphics g = this.CreateGraphics();
                Bitmap bitmap = new Bitmap(ScreenWidth, ScreenHeight);
                Graphics bufferGraphics = Graphics.FromImage(bitmap);
                bufferGraphics.Clear(this.BackColor);
                Rectangle rect = new Rectangle(0, 0, ScreenWidth, ScreenHeight);
                bufferGraphics.FillRectangle(new SolidBrush(Color.Black), rect);
                DrawTank(bufferGraphics,HeroTank);
                DrawTank(bufferGraphics,enemy1);
                foreach (var item in Enemys)
                {
                    DrawTank(bufferGraphics,item);
                    item.TankDirection = (Direction)new Random().Next(1, 8);
                }
                g.DrawImage(bitmap, 0, 0);
                bufferGraphics.Dispose();
                bitmap.Dispose();
                Thread.Sleep(50);                         
            }
        }
        //刷新Hero
        private void UpdateHero()
        {
            while (true)
            {
                HeroTank.TankRun();
                Thread.Sleep(100);
            }
        }
        //刷新Enemy
        private void UpdateEnemy()
        {
            while (true)
            {
                foreach (var item in Enemys)
                {
                    item.TankRun();                   
                    item.Shoot();
                }
                enemy1.TankRun();
                enemy1.Shoot();
                Thread.Sleep(1000);
            }
        }

        #endregion


    }
}
