using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPTool
{
    public partial class PassWordInput : Form
    {
        public PassWordInput()
        {
            InitializeComponent();
            var screen = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));
            var x = screen.WorkingArea.X + screen.WorkingArea.Width - this.Width;
            var y = screen.WorkingArea.Y + screen.WorkingArea.Height - this.Height;
            this.Location = new Point(x, y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime  dt = DateTime.Now;
            string temp = dt.Hour.ToString() + dt.Minute.ToString();
            string pwd = ReverseB(temp);
            if (this.textBox1.Text == pwd)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.textBox1.Text = "";
            }
        }

        public string ReverseB(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
