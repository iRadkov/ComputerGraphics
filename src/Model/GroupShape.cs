using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    [Serializable]
    public class GroupShape : Shape
    {
        #region Constructor

        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion
        protected List<Shape> subItem = new List<Shape>();
        public List<Shape> SubItem
        {
            get { return subItem; }
            set { subItem = value; }
        }

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                foreach (var item in SubItem)
                {
                    if (item.Contains(point)) return true;
                }
                return true;
            }
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.RotateShape(grfx);
            base.DrawSelf(grfx);
            grfx.ResetTransform();
            foreach (var item in SubItem)
            {
                base.RotateShape(grfx);
                item.DrawSelf(grfx);
                grfx.ResetTransform();
            }
        }
        public override void Move(float dx, float dy)
        {
            base.Move(dx, dy);
            foreach (var item in SubItem)
            {
                item.Move(dx * 2, dy * 2);
            }
        }
        public override void GroupFillColor(Color color)
        {
            base.GroupFillColor(color);
            foreach (var item in SubItem)
            {
                item.FillColor = color;
            }
        }
        public override void GroupBorderColor(Color color)
        {
            base.GroupBorderColor(color);
            foreach (var item in SubItem)
            {
                item.BorderColor = color;
            }
        }
        public override void GroupTranslate(PointF point)
        {
            base.GroupTranslate(point);
            foreach (var item in SubItem)
            {
                item.Location = new PointF(this.Location.X + (item.Location.X - point.X), this.Location.Y - (point.Y - item.Location.Y));
            }
        }
        public override void GroupBorderBoldNegative(float borderBold)
        {
            base.GroupBorderBoldNegative(borderBold);
            foreach (var item in SubItem)
            {
                item.BorderBold = borderBold;
            }
        }
        public override void GroupBorderBoldPositive(float borderBold)
        {
            base.GroupBorderBoldPositive(borderBold);
            foreach (var item in SubItem)
            {
                item.BorderBold = borderBold;
            }
        }
        public override void GroupChangeSizeWIdth(float width)
        {
            base.GroupChangeSizeWIdth(width);
            float maxX = float.NegativeInfinity;
            float minX = float.PositiveInfinity;
            foreach (var item in SubItem)
            {
                item.Width = width;
                if (minX > item.Location.X)
                {
                    minX = item.Location.X;
                }
                if (maxX < item.Location.X + item.Width)
                {
                    maxX = item.Location.X + item.Width;
                }

            }
            this.Rectangle = new RectangleF(minX, this.Rectangle.Y, maxX - minX, this.Rectangle.Height);
        }
        public override void GroupChangeSizeHeight(float height)
        {
            base.GroupChangeSizeHeight(height);
            float maxY = float.NegativeInfinity;
            float minY = float.PositiveInfinity;
            foreach (var item in SubItem)
            {
                item.Height = height;
                if (minY > item.Location.Y)
                {
                    minY = item.Location.Y;
                }
                if (maxY < item.Location.Y + item.Height)
                {
                    maxY = item.Location.Y + item.Height;
                }

            }
            this.Rectangle = new RectangleF(this.Rectangle.X, minY, this.Rectangle.Width, maxY - minY);

        }
        public override void GroupRotate()
        {
            base.GroupRotate();
            foreach (var item in SubItem)
            {
                item.Angle = 45;
            }
        }


    }
}
