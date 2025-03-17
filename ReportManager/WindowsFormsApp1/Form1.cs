using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using WindowsFormsApp1;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public class ReportForm : Form
        {
            private ReportManager reportManager;
            private TextBox titleTextBox;
            private TextBox contentTextBox;
            private Button addReportButton;
            private Button removeReportButton;
            private Button updateReportButton;
            private ListBox reportsListBox;

            public ReportForm()
            {
                this.Text = "Управление отчётами";
                this.Width = 600;
                this.Height = 500;

                Label titleLabel = new Label
                {
                    Text = "Заголовок:",
                    Location = new System.Drawing.Point(10, 5),
                    AutoSize = true
                };
                Label contentLabel = new Label
                {
                    Text = "Содержание:",
                    Location = new System.Drawing.Point(10, 45),
                    AutoSize = true
                };

                titleTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(10, 20),
                    Width = 200
                };
                contentTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(10, 60), 
                    Width = 200,
                    Height = 100,
                    Multiline = true,
                    ScrollBars = ScrollBars.Both
                };

                addReportButton = new Button
                {
                    Location = new System.Drawing.Point(10, 170), 
                    Text = "Добавить",
                    Width = 100
                };
                addReportButton.Click += AddReportButton_Click;

                removeReportButton = new Button
                {
                    Location = new System.Drawing.Point(120, 170),
                    Text = "Удалить",
                    Width = 100
                };
                removeReportButton.Click += RemoveReportButton_Click;

                updateReportButton = new Button
                {
                    Location = new System.Drawing.Point(220, 170),
                    Text = "Обновить",
                    Width = 100
                };
                updateReportButton.Click += UpdateReportButton_Click;

                reportsListBox = new ListBox
                {
                    Location = new System.Drawing.Point(10, 200), 
                    Width = 560,
                    Height = 200
                };

                this.Controls.Add(titleLabel);
                this.Controls.Add(contentLabel);
                this.Controls.Add(titleTextBox);
                this.Controls.Add(contentTextBox);
                this.Controls.Add(addReportButton);
                this.Controls.Add(removeReportButton);
                this.Controls.Add(updateReportButton);
                this.Controls.Add(reportsListBox);

                reportManager = new ReportManager();
                UpdateReportsList();
            }

            private void UpdateReportsList()
            {
                reportsListBox.Items.Clear();
                foreach (var report in reportManager.Reports)
                {
                    reportsListBox.Items.Add($"{report.Title} ({report.CreationDate.ToString("yyyy-MM-dd")})");
                }
            }

            private void AddReportButton_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(titleTextBox.Text) || string.IsNullOrEmpty(contentTextBox.Text))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                Report newReport = new Report(titleTextBox.Text, contentTextBox.Text, DateTime.Now);
                try
                {
                    reportManager.AddReport(newReport);
                    titleTextBox.Clear();
                    contentTextBox.Clear();
                    UpdateReportsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            private void RemoveReportButton_Click(object sender, EventArgs e)
            {
                if (reportsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите отчёт для удаления!");
                    return;
                }

                try
                {
                    var selectedReport = reportManager.Reports[reportsListBox.SelectedIndex];
                    reportManager.RemoveReport(selectedReport);
                    UpdateReportsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении отчёта: {ex.Message}");
                }
            }

            private void UpdateReportButton_Click(object sender, EventArgs e)
            {
                if (reportsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите отчёт для обновления!");
                    return;
                }

                var selectedReport = reportManager.Reports[reportsListBox.SelectedIndex];

                if (string.IsNullOrEmpty(titleTextBox.Text) || string.IsNullOrEmpty(contentTextBox.Text))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                try
                {
                    reportManager.UpdateReport(selectedReport, titleTextBox.Text, contentTextBox.Text);
                    UpdateReportsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ReportForm());
            }
        }
    }
}