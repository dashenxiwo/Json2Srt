using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Json2Srt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string ConvertToSrt(List<RootObject> subtitleData)
        {
            StringBuilder srtBuilder = new StringBuilder();
            foreach (var data in subtitleData)
            {
                foreach (var item in data.Properties.SubtitleDataList)
                {
                    srtBuilder.AppendLine(item.SubtitleNumber.ToString());
                    srtBuilder.AppendLine($"{FormatTime(item.SubtitleBegin)} --> {FormatTime(item.SubtitleEnd)}");
                    srtBuilder.AppendLine(item.SubtitleText);
                    srtBuilder.AppendLine();
                }
            }
            return srtBuilder.ToString();
        }

        private string FormatTime(double time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            return timeSpan.ToString(@"hh\:mm\:ss\,fff");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            string jsonInput = textBox1.Text;
            var subtitleData = JsonConvert.DeserializeObject<List<RootObject>>(jsonInput);
            string srtContent = ConvertToSrt(subtitleData);
            textBox_FileName.Text = srtContent;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "SRT files (*.srt)|*.srt|All files (*.*)|*.*";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllText(filePath, textBox_FileName.Text);
                   // MessageBox.Show("文件已保存: " + filePath, "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text="";
            if (Clipboard.ContainsText())
            {
                textBox1.Text = Clipboard.GetText();
            }
        }
    }

    public class RootObject
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public SubtitleProperties Properties { get; set; }
    }

    public class SubtitleProperties
    {
        public List<SubtitleData> SubtitleDataList { get; set; }
    }

    public class SubtitleData
    {
        public int SubtitleNumber { get; set; }
        public double SubtitleBegin { get; set; }
        public double SubtitleEnd { get; set; }
        public string SubtitleText { get; set; }
    }
}
