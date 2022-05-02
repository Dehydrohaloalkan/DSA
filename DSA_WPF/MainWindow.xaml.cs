using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DSA;
using Microsoft.Win32;

namespace DSA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileChoosingButton_OnClick(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = string.Empty;

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                FilePathLabel.Text = _filePath;
            }
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = string.Empty;

            try
            {
                byte[] message = File.ReadAllBytes(_filePath);
                var sc = new SignatureCreator(int.Parse(QField.Text), int.Parse(PField.Text), int.Parse(HField.Text),
                    int.Parse(XField.Text));
                var (data, hash, r, s) = sc.Create(message, int.Parse(KField.Text));

                HashLabel.Text = hash.ToString();
                RLabel.Text = r.ToString();
                SLabel.Text = s.ToString();

                var newFileName = Path.GetDirectoryName(_filePath) + "\\" + "New" + "_" + Path.GetFileName(_filePath);
                File.WriteAllBytes(newFileName, data);

            }
            catch (LogicException exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
            catch (ArgumentNullException exception)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }
            catch (FormatException exception)
            {
                MessageBox.Show("Значения должны быть числами");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Что-то пошло не так, {exception.GetType()}");
                return;
            }
        }

        private void CheckButton_OnClick(object sender, RoutedEventArgs e)
        {
            HashLabel.Text = String.Empty;
            RLabel.Text = String.Empty;
            SLabel.Text = String.Empty;

            try
            {
                byte[] message = File.ReadAllBytes(_filePath);
                var sc = new SignatureCreator(int.Parse(QField.Text), int.Parse(PField.Text), int.Parse(HField.Text),
                    int.Parse(XField.Text));
                var (v, r, s) = sc.Check(message);

                RLabel.Text = r.ToString();
                SLabel.Text = s.ToString();

                if (r == v)
                {
                    StatusLabel.Content = $"Подпись верна, (r){r} == (v){v}";
                    StatusLabel.Foreground = Brushes.Green;

                }
                else
                {
                    StatusLabel.Content = $"Подпись не верна, (r){r} != (v){v}";
                    StatusLabel.Foreground = Brushes.Red;
                }
            }
            catch (Exception exception)
            {
                StatusLabel.Content = "Подпись не верна";
                StatusLabel.Foreground = Brushes.Red;
            }
            



        }
    }
}
