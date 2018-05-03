using EyeXFramework;
using EyeXFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ImageProcessing;
using WindowsAPI;

namespace GazeToolBar
{
    public partial class ZoomLens : Form
    {
        DrawingForm drawingForm;

        public Point Offset { get; set; }
        public Point CrossHairPos { get; set; }

        public ZoomLens()
        {
            InitializeComponent();
            ShowInTaskbar = false;

            this.FormBorderStyle = FormBorderStyle.None;
            drawingForm = new DrawingForm();
            TopMost = true;

            Location = new Point(200, 200);
        }


        public void Start()
        {
            // Show the form that the user feedback image is drawn on
            drawingForm.Show();
            DrawTimer.Start();
        }


        public void ResetZoomLens()
        {
            // Stop timer, and hide drawing form and zoom form
            DrawTimer.Stop();
            drawingForm.ClearForm();
            drawingForm.Refresh();
            drawingForm.Close();
            drawingForm = new DrawingForm();

            Hide();
            Location = new Point(-200, -200);
        }


        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            // Show drawing form and tell him to draw
            drawingForm.SetCrossHairPos(CrossHairPos);
            drawingForm.Show();
            drawingForm.Draw();
        }
    }
}