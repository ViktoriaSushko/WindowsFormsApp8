using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        string source;   // шлях до папки-джерела
        string receiver; // шлях до папки-призначення

        public Form1()
        {
            InitializeComponent();
        }

        // Кнопка Source — відкриває діалог вибору папки-джерела
        private void btnSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();

            if (folderBrowserDialog.SelectedPath != "")
            {
                source = folderBrowserDialog.SelectedPath;
                textBox1.Text = folderBrowserDialog.SelectedPath;
            }
        }

        // Кнопка Receiver — відкриває діалог вибору папки-призначення
        private void btn_Receiver(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();

            if (folderBrowserDialog.SelectedPath != "")
            {
                receiver = folderBrowserDialog.SelectedPath;
                textBox2.Text = folderBrowserDialog.SelectedPath;
            }
        }

        // Кнопка Copy — копіює унікальні файли з source до receiver
        private void btnCopy_Click(object sender, EventArgs e)
        {
            // Перевірка: чи вибрані обидві папки
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(receiver))
            {
                MessageBox.Show("Оберіть папки Source та Receiver!");
                return;
            }

            DirectoryInfo dir1 = new DirectoryInfo(source);
            DirectoryInfo dir2 = new DirectoryInfo(receiver);

            // Отримуємо списки файлів з обох папок
            FileInfo[] filesInfo1 = dir1.GetFiles();
            FileInfo[] filesInfo2 = dir2.GetFiles();

            // Except повертає файли з dir1, яких немає у dir2
            // FileComparer порівнює за іменем, розміром і датою зміни
            var uniqueFiles = filesInfo1.Except(filesInfo2, new FileComparer());

            // Паралельно копіюємо кожен унікальний файл до папки-призначення
            Parallel.ForEach(uniqueFiles, t =>
            {
                try
                {
                    string destPath = Path.Combine(receiver, t.Name);

                    // Копіюємо файл (overwrite: false — не перезаписуємо якщо вже існує)
                    File.Copy(t.FullName, destPath, overwrite: false);
                }
                catch (Exception ex)
                {
                    // Виводимо повідомлення про помилку (напр. немає прав доступу)
                    MessageBox.Show("INVALID: " + ex.Message);
                }
            });

            // Відкриваємо папку-призначення в Explorer ОДИН РАЗ після завершення копіювання
            // (не всередині ForEach — інакше Explorer відкривається для кожного файлу!)
            Process.Start(receiver);
        }
    }
}

