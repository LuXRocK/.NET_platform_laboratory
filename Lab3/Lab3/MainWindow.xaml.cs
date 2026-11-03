using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


public class MainWindowViewModel : INotifyPropertyChanged
{
    ///<summary>
    ///Property Changed Event Handler 
    /// </summary>

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(name));
        }
    }
    private String _numOfElemsBox = "";
    public String numOfElemsBox
    {
        get { return _numOfElemsBox; }
        set
        {
            _numOfElemsBox = value;
            OnPropertyChanged(nameof(numOfElemsBox));
        }
    }
    private String _rangeFromBox = "";
    public String rangeFromBox
    {
        get { return _rangeFromBox; }
        set
        {
            _rangeFromBox = value;
            OnPropertyChanged(nameof(rangeFromBox));
        }
    }
    private String _rangeToBox = "";
    public String rangeToBox
    {
        get { return _rangeToBox; }
        set
        {
            _rangeToBox = value;
            OnPropertyChanged(nameof(rangeToBox));
        }
    }
    private String _outputBlock = "";
    public String outputBlock
    {
        get { return _outputBlock; }
        set
        {
            _outputBlock = value;
            OnPropertyChanged(nameof(outputBlock));
        }
    }

    private double _progressBar1;
    public double progressBar1
    {
        get { return _progressBar1; }
        set
        {
            _progressBar1 = value;
            OnPropertyChanged(nameof(progressBar1));
        }
    }
    private string _statusBarText = "Ready";
    public string statusBarText
    {
        get { return _statusBarText; }
        set
        {
            _statusBarText = value;
            OnPropertyChanged(nameof(statusBarText));
        }
    }


}

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel CurrentModel = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = CurrentModel;
        }

        private void FileNew_Click(object senser, RoutedEventArgs e)
        {
            Console.WriteLine($"New file clicked!");
        }
        private void FileLoad_Click(object senser, RoutedEventArgs e)
        {
            Console.WriteLine($"Load file clicked!");
        }
        private void FileSave_Click(object senser, RoutedEventArgs e)
        {
            Console.WriteLine($"Save file clicked!");
        }
        private void FileExit_Click(object senser, RoutedEventArgs e)
        {
            Console.WriteLine($"Exit clicked!");
        }
        private void FileAbout_Click(object senser, RoutedEventArgs e)
        {
            Console.WriteLine($"About clicked!");
        }
        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            CurrentModel.statusBarText = "Generating";
            CurrentModel.outputBlock = "";
            CurrentModel.progressBar1 = 0;
            start.IsEnabled = false;
            var progress = new Progress<double>(percentValue =>
            {
                CurrentModel.progressBar1 = percentValue;
            });
            var numbers = await Generator.generateAsync(int.Parse(CurrentModel.numOfElemsBox), int.Parse(CurrentModel.rangeFromBox), int.Parse(CurrentModel.rangeToBox), progress);
            String result = string.Join("; ", numbers);
            CurrentModel.outputBlock = result;
            start.IsEnabled = true;
            CurrentModel.statusBarText = "Ready";
        }
    }
}


