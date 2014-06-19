using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TankWar
{
    public enum Direction { STOP, UP, LUP, RUP, DOWN, LDOWN, RDOWN, LEFT, RIGHT }
    public class Tank
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Tank(int speed)
        {
            this.Speed = speed;
        }

        public Tank(Point location, Size tankSize, int speed)
        {
            this.Location = location;
            this.TankSize = tankSize;
            this.Speed = speed;
        }

        #region Property 属性

        private Point _location;

        public Point Location
        {
            get { return _location.IsEmpty ? new Point(0, 0) : _location; }
            set { _location = value; }
        }

        private Size _tankSize;

        public Size TankSize
        {
            get { return _tankSize.IsEmpty ? new Size(50, 50) : _tankSize; }
            set { _tankSize = value; }
        }

        private int _speed;

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private Direction direction;

        public Direction TankDirection
        {
            get { return direction; }
            set { direction = value; }
        }

        private Direction lastDirection;

        public Direction LastDirection
        {
            get { return lastDirection == Direction.STOP ? lastDirection = Direction.RIGHT : lastDirection; }
            set { lastDirection = value; }
        }


        private List<Bullet> _bullets = new List<Bullet>();

        public List<Bullet> Bullets
        {
            get { return _bullets; }
            set { _bullets = value; }
        }


        #endregion

        #region Private 字段

        private bool up, left, right, down = false;

        #endregion

        #region Public Function 方法

        public void DrawSelf(Graphics g)
        {
            Rectangle recttank = new Rectangle(this.Location, this.TankSize);
            g.FillRectangle(new SolidBrush(Color.Red), recttank); 
        }

        public void TankRun()
        {
            switch (direction)
            {
                case Direction.UP:
                    _location.Y -= Speed;
                    if (_location.Y <= 0)
                        _location.Y = TankMainForm.ScreenHeight;
                    break;
                case Direction.RIGHT:
                    _location.X += Speed;
                    if (_location.X >= TankMainForm.ScreenWidth)
                        _location.X = 0;
                    break;
                case Direction.DOWN:
                    _location.Y += Speed;
                    if (_location.Y >= TankMainForm.ScreenHeight)
                        _location.Y = 0;
                    break;
                case Direction.LEFT:
                    _location.X -= Speed;
                    if (_location.X <= 0)
                        _location.X = TankMainForm.ScreenWidth;
                    break;
                case Direction.LUP:
                    _location.Y -= Speed;
                    _location.X -= Speed;
                    if (_location.Y <= 0)
                        _location.Y = TankMainForm.ScreenHeight;
                    if (_location.X <= 0)
                        _location.X = TankMainForm.ScreenWidth;
                    break;
                case Direction.RUP:
                    _location.Y -= Speed;
                    _location.X += Speed;
                    if (_location.Y <= 0)
                        _location.Y = TankMainForm.ScreenHeight;
                    if (_location.X >= TankMainForm.ScreenWidth)
                        _location.X = 0;
                    break;
                case Direction.LDOWN:
                    _location.Y += Speed;
                    _location.X -= Speed;
                    if (_location.Y >= TankMainForm.ScreenHeight)
                        _location.Y = 0;
                    if (_location.X <= 0)
                        _location.X = TankMainForm.ScreenWidth;
                    break;
                case Direction.RDOWN:
                    _location.Y += Speed;
                    _location.X += Speed;
                    if (_location.Y >= TankMainForm.ScreenHeight)
                        _location.Y = 0;
                    if (_location.X >= TankMainForm.ScreenWidth)
                        _location.X = 0;
                    break;
                case Direction.STOP:
                    break;
                default:
                    break;
            }
        }

        public void KeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    up = true;
                    break;
                case Keys.Right:
                    right = true;
                    break;
                case Keys.Down:
                    down = true;
                    break;
                case Keys.Left:
                    left = true;
                    break;
                default:
                    break;
            }
            getDirection();
        }

        public void KeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    up = false;
                    break;
                case Keys.Right:
                    right = false;
                    break;
                case Keys.Down:
                    down = false;
                    break;
                case Keys.Left:
                    left = false;
                    break;
                default:
                    break;
            }
            getDirection();
        }

        public void Shoot()
        {
            Bullet bullet = null;
            if (TankDirection == Direction.STOP)
            {
                bullet = new Bullet(new Point(Location.X + 20, Location.Y + 20), new Size(), 20, LastDirection);
            }
            else
            {
                bullet = new Bullet(new Point(Location.X + 20, Location.Y + 20), new Size(), 20, TankDirection);
            }
            this._bullets.Add(bullet);
        }

        #endregion

        #region Private Function 私有方法

        private void getDirection()
        {
            if (up && !right && !down && !left)
            {
                direction = Direction.UP;
                lastDirection = direction;
            }
            else if (!up && !right && down && !left)
            {
                direction = Direction.DOWN;
                lastDirection = direction;
            }
            else if (!up && !right && !down && left)
            {
                direction = Direction.LEFT;
                lastDirection = direction;
            }
            else if (!up && right && !down && !left)
            {
                direction = Direction.RIGHT;
                lastDirection = direction;
            }
            else if (up && !right && !down && left)
            {
                direction = Direction.LUP;
                lastDirection = direction;
            }
            else if (up && right && !down && !left)
            {
                direction = Direction.RUP;
                lastDirection = direction;
            }
            else if (!up && !right && down && left)
            {
                direction = Direction.LDOWN;
                lastDirection = direction;
            }
            else if (!up && right && down && !left)
            {
                direction = Direction.RDOWN;
                lastDirection = direction;
            }
            else if (!up && !right && !down && !left)
            {
                direction = Direction.STOP;
            }

        }

        #endregion

    }
}
