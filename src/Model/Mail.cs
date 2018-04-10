﻿using System;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    [Serializable]
    public class Mail : Shape
    {

        #region Constructor
        public Mail(RectangleF rect) : base(rect)
        {
        }

        public Mail(Mail mail) : base(mail)
        {
        }

        #endregion


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
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                return true;
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }


        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        /// 
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            base.RotateShape(grfx);

            Point[] dot = { new Point((int)Rectangle.X, (int)Rectangle.Y),
                new Point((int)Rectangle.X + (int)Rectangle.Width / 2, (int)Rectangle.Y + (int)Rectangle.Height / 2),
                new Point((int)Rectangle.X + (int)Rectangle.Width, (int)Rectangle.Y) };

            grfx.FillRectangle(new SolidBrush( FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawRectangle(new Pen(BorderColor, BorderBold), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawLines(new Pen(BorderColor, BorderBold), dot);
            grfx.DrawLine(new Pen(BorderColor, BorderBold), new Point((int)Rectangle.X + (int)Rectangle.Width / 2, (int)Rectangle.Y + (int)Rectangle.Height / 2),
                new Point((int)Rectangle.X + (int)Rectangle.Width / 2, (int)Rectangle.Y + (int)Rectangle.Height));

            grfx.ResetTransform();
        }

    }
}
