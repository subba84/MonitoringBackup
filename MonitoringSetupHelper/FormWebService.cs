using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MonitoringSetupHelper
{
    public partial class FormWebService : Form
    {

        public FormWebService()
        {
            InitializeComponent();
        }

        private void FormSerialNumber_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            label1.Visible = false;
            textBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            try
            {
                if (radioButton2.Checked)
                {
                    File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "\\WebServiceUrl.txt", textBox1.Text);
                }
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.Message + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                }

            }
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                label1.Hide();
                textBox1.Hide();
                
            }

          
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show();
            textBox1.Show();
           
        }
    }
}
