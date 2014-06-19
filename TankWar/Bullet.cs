using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TankWar
{
    public class Bullet
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public Bullet(int speed, Direction direction)
        {
            this.Speed = speed;
            this.direction = direction;
        }

        public Bullet(Point location, Size bulletSize, int speed, Direction direction)
        {
            this._location = location;
            this._bulletSize = bulletSize;
            this._speed = speed;
            this.direction = direction;
        }

        #region Property 属性

        private Point _location;

        public Point Location
        {
            get { return _location.IsEmpty ? new Point(0, 0) : _location; }
            set { _location = value; }
        }

        private Size _bulletSize;

        public Size BulletSize
        {
            get { return _bulletSize.IsEmpty ? new Size(10, 10) : _bulletSize; }
            set { _bulletSize = value; }
        }

        private int _speed;

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private Direction direction;

        public Direction BulletDirection
        {
            get { return direction; }
            set { direction = value; }
        }


        #endregion

        #region Private 字段


        #endregion

        #region Function 方法

        public void DrawSelf(Graphics g)
        {
            Rectangle rect = new Rectangle(this.Location, this.BulletSize);
            g.FillEllipse(new SolidBrush(Color.Yellow), rect);
            BulletRun();
        }

        public void BulletRun()
        {
            switch (BulletDirection)
            {
                case Direction.UP:
                    _location.Y -= Speed;                   
                    break;
                case Direction.RIGHT:
                    _location.X += Speed;                    
                    break;
                case Direction.DOWN:
                    _location.Y += Speed;                   
                    break;
                case Direction.LEFT:
                    _location.X -= Speed;                  
                    break;
                case Direction.LUP:
                    _location.Y -= Speed;
                    _location.X -= Speed;                   
                    break;
                case Direction.RUP:
                    _location.Y -= Speed;
                    _location.X += Speed;                   
                    break;
                case Direction.LDOWN:
                    _location.Y += Speed;
                    _location.X -= Speed;                   
                    break;
                case Direction.RDOWN:
                    _location.Y += Speed;
                    _location.X += Speed;                   
                    break;
                case Direction.STOP:
                    break;
                default:
                    break;
            }
        }
        #endregion

    }

}
