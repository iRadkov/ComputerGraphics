using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
  [Serializable]
    public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
     
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked) {
                var sel = dialogProcessor.ContainsPoint(e.Location);

                if (sel != null)
                {
                    if (dialogProcessor.Selection.Contains(sel))
                        dialogProcessor.Selection.Remove(sel);
                    else
                        dialogProcessor.Selection.Add(sel);

                    statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
                    dialogProcessor.IsDragging = true;
                    dialogProcessor.LastLocation = e.Location;
                    viewPort.Invalidate();
                }
            }
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetSelectedFieldColor(colorDialog1.Color);
                viewPort.Invalidate();
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElips();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupSelected();
            viewPort.Invalidate();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            
        }
    

        private void pickUpSpeedButton_Click(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomCircle();
            statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";
            viewPort.Invalidate();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dialogProcessor.Delete();
            viewPort.Invalidate();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            dialogProcessor.ChangeSizePositive();
            viewPort.Invalidate();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

            dialogProcessor.Rotate();
            viewPort.Invalidate();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            dialogProcessor.ChangeSizeNegative();
            viewPort.Invalidate();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                dialogProcessor.Write(dialogProcessor.ShapeList, saveFileDialog1.FileName);
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult== DialogResult.OK)
            {
                dialogProcessor.ShapeList=(List<Shape>)dialogProcessor.Read(openFileDialog.FileName);
                viewPort.Invalidate();
            }
          
        }
        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            dialogProcessor.SetBorderBoldPositive();
            viewPort.Invalidate();
        }

        private void speedMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            dialogProcessor.SetBorderBoldNegative();
            viewPort.Invalidate();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomMail();
            statusBar.Items[0].Text = "Последно действие: Рисуване на писмо";
            viewPort.Invalidate();
        }

        private void elipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElips();
            viewPort.Invalidate();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.Delete();
        }
    }
    
}
