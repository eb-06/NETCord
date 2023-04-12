using NETCord.Bot;
using System;
using System.Windows.Forms;

namespace NETCord
{
    public partial class Form1 : Form
    {
        Save save = new Save();
        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e) => textBox1.Text = save.token;

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = !hideToolStripMenuItem.Checked;
            textBox1.UseSystemPasswordChar = hideToolStripMenuItem.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Backend().Initialize(textBox1.Text).GetAwaiter();
            save.token = textBox1.Text;
            save.Save();
            button1.Enabled = false;
            textBox1.ReadOnly = true;
        }
    }
}