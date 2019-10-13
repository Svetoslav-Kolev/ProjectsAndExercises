using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursova
{
    
    public partial class MovingStuff : Form
    {
        enum Position
        {
            Left,Right,Up,Down
        }
        SolidBrush myBrush = new SolidBrush(Color.Violet);

        public int horizontalPosition;
        public int Verticalvelocity = 10;
        public int HorizontalVelocity = 10;
        public int verticalPosition;
        public string type = "rectangle";

        public PictureBox pic = new PictureBox();
        
        

        public int lastSpeedVertical;
        public int lastSpeedHorizontal;
        private Position position;
        public MovingStuff()
        {
            InitializeComponent();
            horizontalPosition = 350;
            verticalPosition = 200;
            position = Position.Down;
            this.Controls.Add(pic);
            pic.ImageLocation = "mushroom.png";
            pic.Size = new Size(140, 140);
            pic.SizeMode = PictureBoxSizeMode.Zoom; 
            pic.Size = new Size(120, 120);
            pic.Hide();

        }      
       public void MovingStuff_Paint(object sender , PaintEventArgs e)
       {
         


            if (type == "rectangle")
               e.Graphics.FillRectangle(myBrush, horizontalPosition, verticalPosition, 100, 100);
            else if (type == "circle")
               e.Graphics.FillPie(myBrush, horizontalPosition, verticalPosition, 100, 100, 0, 360);
           else if (type == "ellipse")
               e.Graphics.FillEllipse(myBrush, horizontalPosition, verticalPosition, 70, 100);
            else if (type == "picture")
                pic.Location = new Point(horizontalPosition, verticalPosition);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            verticalPosition += Verticalvelocity;
            horizontalPosition += HorizontalVelocity;
            if (verticalPosition >= this.Size.Height-120)
            {
                Verticalvelocity = -Verticalvelocity;              
            }
            if (verticalPosition <= 0)
            {
                Verticalvelocity = -Verticalvelocity;
            }
            if (horizontalPosition >= this.Size.Width-100)
            {
                HorizontalVelocity = -HorizontalVelocity;
            }
            if (horizontalPosition <= 0)
            {
                HorizontalVelocity = -HorizontalVelocity;
            }
            Refresh();
        }

        private void MovingStuff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                position = Position.Left;
            }
            if (e.KeyCode == Keys.Right)
            {
                position = Position.Right;
            }
            if (e.KeyCode == Keys.Down)
            {
                position = Position.Down;
            }
            if (e.KeyCode == Keys.Up)
            {
                position = Position.Up;
            }

        }

        private void MovingStuff_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
        
            tmrMovement.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tmrMovement.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFast_Click(object sender, EventArgs e)
        {
            if (HorizontalVelocity <= 2 == false)
            {
                HorizontalVelocity += 3;
            }
            if (Verticalvelocity <= 2 == false)
            {
                Verticalvelocity += 3;
            }
        }

        private void btnSlow_Click(object sender, EventArgs e)
        {
          
            if (HorizontalVelocity <= 3 ==false)
            {
                HorizontalVelocity -= 1;
            }
            if (Verticalvelocity <= 3 == false)
            {
                Verticalvelocity -= 1;
            }
           
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a program made by a student for his University Project" +
                "\n Created with C# GUI" +
                "\n Simple program very much like a screensaver with extra options","Information");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Menu - options for properties of the Application " +
                "\n Decrease/Increase Speed - increases or decreases the speed of the object " +
                "\n Start/Stop - starts or stops movement of the object" +
                "\n Object Type - changes appearance of object" +
                "\n Object Color - changes color of object" +
                "\n Form Color - changes color of background" +
                "\n Back - Returns to Starting screen", "Help");
        }

        //
        // Menu Objects
        //

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            myBrush.Color = Color.Red;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myBrush.Color = Color.Green;
        }

        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            myBrush.Color = Color.Blue;
        }

        private void violetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myBrush.Color = Color.Violet;
        }

        private void lightRedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.LightPink;
        }

        private void lightGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.LightGreen;
        }

        private void lightCyanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.LightCyan;
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type = "circle";
            pic.Hide();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type = "rectangle";
            pic.Hide();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type = "ellipse";
            pic.Hide();
        }

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            type = "picture";
            pic.Show();
        }

    }
}
