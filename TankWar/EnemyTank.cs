using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankWar
{
    public class EnemyTank : Tank
    {
        public EnemyTank(int speed,Color color)
            : base(speed)
        {
            this.Speed = speed;
            this.TankColor = color;
        }

        public EnemyTank(Point location, Size tankSize, int speed,Color color)
            : base(location, tankSize, speed)
        {
            this.Location = location;
            this.TankSize = tankSize;
            this.Speed = speed;
            this.TankColor = color;
        }

        private Color _tankColor;

        public Color TankColor
        {
            get { return _tankColor; }
            set { _tankColor = value; }
        }


        public override void DrawSelf(Graphics g)
        {
            Rectangle recttank = new Rectangle(this.Location, this.TankSize);
            g.FillRectangle(new SolidBrush(this._tankColor), recttank);
            this.TankDirection = (Direction)new Random().Next(1, 8);
        }

        public override void Shoot()
        {
            Bullet bullet = null;
            if (TankDirection == Direction.STOP)
            {
                bullet = new Bullet(new Point(Location.X + 20, Location.Y + 20), new Size(), 20, LastDirection, BulletType.EnemyBullet);
            }
            else
            {
                bullet = new Bullet(new Point(Location.X + 20, Location.Y + 20), new Size(), 20, TankDirection, BulletType.EnemyBullet);
            }
            this.Bullets.Add(bullet);
        }
    }
}
