using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu, viewMenu, createItem, openItem, saveItem, closeItem, nightModeItem;
        private RichTextBox richTextBox;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Безымянный";
            this.Width = 800;
            this.Height = 600;

            menuStrip = new MenuStrip();

            fileMenu = new ToolStripMenuItem("Файл");
            createItem = new ToolStripMenuItem("Создать", null, (s, e) => CreateFile());
            openItem = new ToolStripMenuItem("Открыть", null, (s, e) => OpenFile());
            saveItem = new ToolStripMenuItem("Сохранить", null, (s, e) => SaveFile());
            closeItem = new ToolStripMenuItem("Закрыть", null, (s, e) => this.Close());
            fileMenu.DropDownItems.AddRange(new[] { createItem, openItem, saveItem, closeItem });

            viewMenu = new ToolStripMenuItem("Вид");
            nightModeItem = new ToolStripMenuItem("Ночная тема", null, (s, e) => ToggleNightMode())
            {
                CheckOnClick = true
            };
            viewMenu.DropDownItems.Add(nightModeItem);

            ToolStripMenuItem secondFormItem = new ToolStripMenuItem("Открыть вторую форму", null, (s, e) => OpenSecondForm());
            fileMenu.DropDownItems.Add(secondFormItem);

            menuStrip.Items.AddRange(new[] { fileMenu, viewMenu });
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12)
            };
            this.Controls.Add(richTextBox);
        }

        private void OpenSecondForm()
        {
            SecondForm secondForm = new SecondForm();
            secondForm.ShowDialog();
        }

        private void CreateFile()
        {
            richTextBox.Clear();
            this.Text = "Безымянный";
        }

        private void OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileExtension = Path.GetExtension(filePath).ToLower();

                    if (fileExtension == ".rtf")
                        richTextBox.LoadFile(filePath, RichTextBoxStreamType.RichText);
                    else
                        richTextBox.Text = File.ReadAllText(filePath);

                    this.Text = filePath;
                }
            }
        }

        private void SaveFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    string fileExtension = Path.GetExtension(filePath).ToLower();

                    if (fileExtension == ".rtf")
                        richTextBox.SaveFile(filePath, RichTextBoxStreamType.RichText);
                    else
                        File.WriteAllText(filePath, richTextBox.Text);

                    this.Text = filePath;
                }
            }
        }

        // Обработчик "Ночная тема"
        private void ToggleNightMode()
        {
            if (nightModeItem.Checked)
            {
                richTextBox.BackColor = Color.FromArgb(30, 30, 30);
                richTextBox.ForeColor = Color.White;
            }
            else
            {
                richTextBox.BackColor = Color.White;
                richTextBox.ForeColor = Color.Black;
            }
        }
   
    }

    // Дополнительная форма
    public class SecondForm : Form
    {
        private Button greetButton, closeButton;

        public SecondForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Дополнительная форма";
            this.Width = 300;
            this.Height = 200;

            // Кнопка Привет
            greetButton = new Button
            {
                Text = "Привет",
                FlatStyle = FlatStyle.Flat,
                Location = new Point(50, 50),
                Size = new Size(100, 50)
            };
            greetButton.Click += (s, e) => MessageBox.Show("Здравствуйте!");
            this.AcceptButton = greetButton; // Кнопка для Enter

            // Кнопка "Закрыть"
            closeButton = new Button
            {
                Text = "Закрыть",
                FlatStyle = FlatStyle.Flat,
                Location = new Point(160, 50),
                Size = new Size(100, 50)
            };
            closeButton.Click += (s, e) => this.Close();
            this.CancelButton = closeButton; // Кнопка для Esc

            this.Controls.Add(greetButton);
            this.Controls.Add(closeButton);
        }
    }
}
