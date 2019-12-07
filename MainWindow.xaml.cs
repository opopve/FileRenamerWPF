using System.Windows;

namespace FileRenamerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
            var VM = new MainWindowVM("D:\\Temp\\tst");
            trvHierarchy.ItemsSource = VM.Tree;
        }
    }
}
