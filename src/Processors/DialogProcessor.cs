using Draw.src.Model;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
    /// <summary>
    /// Класът, който ще бъде използван при управляване на диалога.
    /// </summary>
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Избран елемент.
        /// </summary>
        private List<Shape> selection = new List<Shape>();
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        
        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
        public bool IsDragging {
            get { return isDragging; }
            set { isDragging = value; }
        }

        /// <summary>
        /// Последна позиция на мишката при "влачене".
        /// Използва се за определяне на вектора на транслация.
        /// </summary>
        private PointF lastLocation;
        public PointF LastLocation {
            get { return lastLocation; }
            set { lastLocation = value; }
        }

        #endregion

        /// <summary>
        /// Добавя примитив - правоъгълник на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomRectangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
            rect.BorderBold = 1;
            rect.FillColor = Color.White;
            rect.BorderColor = Color.Black;


            ShapeList.Add(rect);
        }
        public void AddRandomMail()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            

            Mail mail = new Mail(new Rectangle(x, y, 250, 150));
            mail.BorderBold = 1;
            mail.FillColor = Color.White;
            mail.BorderColor = Color.Black;


            ShapeList.Add(mail);
        }

        public void AddRandomElips()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            ElipseShape elipse = new ElipseShape(new Rectangle(x, y, 250, 125));
            elipse.FillColor = Color.White;
            elipse.BorderBold = 1;
            elipse.BorderColor = Color.Black;


            ShapeList.Add(elipse);
        }
        public void AddRandomCircle()
        {
            Random random = new Random();
            int x = random.Next(50, 500);
            int y = random.Next(50, 300);

            CircleShape circle = new CircleShape(new Rectangle(x, y, 200, 100));
            circle.FillColor = Color.White;
            circle.BorderColor = Color.Black;
            circle.BorderBold = 1;

            ShapeList.Add(circle);
        }


        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--) {
                if (ShapeList[i].Contains(point)) {

                    return ShapeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF p)
        {
            if (selection != null) {
                foreach (var item in Selection)
                {
                    item.Move(p.X - lastLocation.X, p.Y - lastLocation.Y);
                    item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);

                }
                lastLocation = p;
            }
        }

        internal void SetSelectedFieldColor(Color color)
        {
            if (Selection != null)
            {
                foreach (var item in Selection)
                {
                    item.GroupFillColor(color);
                    item.FillColor = color;
                }
            }
        }

        public void GroupSelected()
        {
            if (Selection.Count < 2) return;

            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;
            foreach (var item in Selection)
            {
                if (minX > item.Location.X)
                {
                    minX = item.Location.X;
                }
                if (minY > item.Location.Y)
                {
                    minY = item.Location.Y;
                }
                if (maxX < item.Location.X + item.Width)
                {
                    maxX = item.Location.X + item.Width;
                }
                if (maxY < item.Location.Y + item.Height)
                {
                    maxY = item.Location.Y + item.Height;
                }
            }
            var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
            group.SubItem = Selection;
            foreach (var item in Selection)
            {
                ShapeList.Remove(item);
            }

            Selection = new List<Shape>();
            Selection.Add(group);
            ShapeList.Add(group);

        }
        public void Write(object obj,string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream= new FileStream(filename + ".bin",
                                  FileMode.Create);

            formatter.Serialize(stream, obj);
            stream.Close();
        }
        public object Read(string filename)
        {
            object obj;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);

            obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        internal void ChangeSizeNegative()
        {

            foreach (var item in Selection)

            {

                if (item.GetType().Equals(typeof(GroupShape)))
                {

                    item.Width -= 10;
                }
                else
                {
                    item.Width -= 10;
                    item.GroupChangeSizeWIdth(item.Width);
                }


                if (item.GetType().Equals(typeof(GroupShape)))
                {
                    item.Height -= 10;
                }
                else
                {
                    item.Height -= 10;
                    item.GroupChangeSizeHeight(item.Height);
                }
            }
        }
        internal void ChangeSizePositive()
        {

            foreach (var item in Selection)

            {

                if (!item.GetType().Equals(typeof(GroupShape)))
                {

                    item.Width += 10;
                }
                else
                {
                    item.Width += 10;
                    item.GroupChangeSizeWIdth(item.Width);
                }


                if (!item.GetType().Equals(typeof(GroupShape)))

                {
                    item.Height += 10;
                }
                else
                {
                    item.Width += 10;
                    item.GroupChangeSizeHeight(item.Height);
                }
            }
        }

        internal void SetBorderBoldPositive()
        {
            int borderBoldIncrease = 2;
            foreach (var item in Selection)

            {
                item.GroupBorderBoldPositive(item.BorderBold+borderBoldIncrease);
                item.BorderBold = item.BorderBold + borderBoldIncrease;
            }
        }
        internal void SetBorderBoldNegative()
        {
            int borderBoldIncrease = 2;
            foreach (var item in Selection)

            {
                item.GroupBorderBoldPositive(item.BorderBold - borderBoldIncrease);
                item.BorderBold = item.BorderBold - borderBoldIncrease;
            }
        }
        internal void Delete()
        {
            foreach (var item in Selection)
            {
                ShapeList.Remove(item);

            }
            Selection.Clear();
        }
        public override void Draw(Graphics grfx)
        {
            base.Draw(grfx);

            foreach (var item in Selection)
            {
                if (item.GetType() == typeof(CircleShape))
                {

                    grfx.DrawRectangle(Pens.Black, item.Location.X - 3 - (item.BorderBold / 2),
                        item.Location.Y - 3 - (item.BorderBold / 2), item.Width + 6 + (item.BorderBold),
                        item.Height * 2 + 6 + (item.BorderBold));

                }
                else
                {
                    item.RotateShape(grfx);
                    grfx.DrawRectangle(Pens.Black, item.Location.X - 3 - (item.BorderBold / 2),
                        item.Location.Y - 3 - (item.BorderBold / 2), item.Width + 6 + (item.BorderBold),
                        item.Height + 6 + (item.BorderBold));
                    grfx.ResetTransform();
                }
            }
        }
        public void Rotate()
        {
            float angleChange = 45;
            if (Selection.Count != 0)
            {
                
                    foreach (var item in Selection)
                {
                    if (item.GetType() != typeof(CircleShape))
                    {

                        item.GroupRotate();
                        item.Angle = item.Angle + angleChange;
                    }
                    else
                    {
                    }
                }
            }
        }
       
}
}
