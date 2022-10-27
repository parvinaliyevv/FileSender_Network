namespace FileSender.Client.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        DataContext = new MainViewModel();
    }


    private void Border_DragAndDrop(object sender, DragEventArgs e)
    {
        var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

        if (File.Exists(filePath))
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var viewModel = DataContext as MainViewModel;

            int maximumValue = Convert.ToInt32((fileStream.Length / (1024 * 5000)) + 1);

            var element = new FileBox(Path.GetFileNameWithoutExtension(filePath), fileStream.Length, "Byte")
            {
                Extension = Path.GetExtension(filePath),
                MaximumValue = maximumValue
            };


            FileBox.Child = element;

            viewModel.FileBox = element;
            viewModel.FilePath = filePath;
        }
    }

    private void CloseApp_ButtonClicked(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel).DisConnectServerCommand.Execute(null);
        Close();
    }

    private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

    private void WindowMinimize_ButtonClicked(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
}
