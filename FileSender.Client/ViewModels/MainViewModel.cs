namespace FileSender.Client.ViewModels;

public class MainViewModel : DependencyObject
{
    public TcpClient Client { get; set; }
    public IPEndPoint ServerEndPoint { get; set; }

    public bool IsConnected { get; set; }
    public bool ProcessStarted { get; set; }

    public ObservableCollection<string> Logs { get; set; }

    public FileBox FileBox { get; set; }
    public string FilePath { get; set; }

    public SolidColorBrush Color
    {
        get { return (SolidColorBrush)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }
    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(MainViewModel));

    public RelayCommand ConnectToServerCommand { get; set; }
    public RelayCommand DisConnectServerCommand { get; set; }
    public RelayCommand SendFileToServerCommand { get; set; }


    public MainViewModel()
    {
        var address = IPAddress.Parse(ConfigurationManager.AppSettings["IpAddress"]);
        var port = int.Parse(ConfigurationManager.AppSettings["PortNumber"]);

        Client = new TcpClient();
        ServerEndPoint = new IPEndPoint(address, port);

        Logs = new();

        FilePath = string.Empty;
        Color = Brushes.Red;

        IsConnected = false;
        ProcessStarted = false;

        ConnectToServerCommand = new RelayCommand(_ => ConnectToServerAsync(), _ => !IsConnected);
        DisConnectServerCommand = new RelayCommand(_ => DisConnectServerAsync(), _ => IsConnected && !ProcessStarted);
        SendFileToServerCommand = new RelayCommand(_ => SendFileToServerAsync(), _ => IsConnected && !ProcessStarted && !string.IsNullOrWhiteSpace(FilePath));

        var timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
        timer.Tick += (_, _) => CommandManager.InvalidateRequerySuggested();
        timer.Start();
    }


    private void ConnectToServer()
    {
        var dispatcher = Application.Current.Dispatcher;

        try
        {
            Client.Connect(ServerEndPoint);
        }
        catch
        {
            dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: Failed to connect to server!", DateTime.Now.ToShortTimeString()))); return;
        }

        IsConnected = true;

        dispatcher.InvokeAsync(() =>
        {
            Color = Brushes.LimeGreen;
            Logs.Add(string.Format("{0}: Connected to server!", DateTime.Now.ToShortTimeString()));
        });
    }
    private async Task ConnectToServerAsync() => await Task.Factory.StartNew(() => ConnectToServer());

    private void DisConnectServer()
    {
        var dispatcher = Application.Current.Dispatcher;

        try
        {
            var stream = Client.GetStream();
            var data = Encoding.Default.GetBytes("DisConnected");

            stream.Write(data, 0, data.Length);

            Client.Close();
            Client.Dispose();
        }
        catch { }

        Client = new TcpClient();

        IsConnected = false;
        ProcessStarted = false;

        dispatcher.InvokeAsync(() =>
        {
            Color = Brushes.Red;
            Logs.Add(string.Format("{0}: DisConnected from server!", DateTime.Now.ToShortTimeString()));
        });
    }
    private async Task DisConnectServerAsync() => await Task.Factory.StartNew(() => DisConnectServer());

    private void SendFileToServer()
    {
        var dispatcher = Application.Current.Dispatcher;

        dispatcher.Invoke(() => FileBox.Value = default);

        ProcessStarted = true;

        if (!File.Exists(FilePath))
        {
            dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: File not found.", DateTime.Now.ToShortTimeString()))); return;
        }
        else
        {
            dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: Sending file started.", DateTime.Now.ToShortTimeString())));
        }

        //////////////////////////////////////////////////////////////////////////////////////////

        var networkStream = Client.GetStream();
        using var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

        string str = string.Empty;
        byte[]? bytes = null;
        int length = default;

        //////////////////////////////////////////////////////////////////////////////////////////

        var packet = new FilePacket(Path.GetFileName(FilePath), fileStream.Length);

        str = JsonSerializer.Serialize(packet);
        bytes = Encoding.Default.GetBytes(str);

        try
        {
            networkStream.Write(bytes, 0, bytes.Length);
        }
        catch (IOException)
        {
            dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: Server connection aborted.", DateTime.Now.ToShortTimeString())));
            DisConnectServer(); return;
        }

        //////////////////////////////////////////////////////////////////////////////////////////

        bytes = new byte[1024];
        length = networkStream.Read(bytes, 0, bytes.Length);
        str = Encoding.Default.GetString(bytes, 0, length);

        if (!str.Contains("FilePacketReceived")) return;

        //////////////////////////////////////////////////////////////////////////////////////////

        long i = default;

        while (i < packet.Length)
        {
            bytes = new byte[1024 * 5000];
            length = fileStream.Read(bytes, 0, bytes.Length);

            try
            {
                networkStream.Write(bytes, 0, length);
            }
            catch (IOException)
            {
                dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: Server connection aborted.", DateTime.Now.ToShortTimeString())));
                DisConnectServer(); return;
            }

            i += length;

            Application.Current.Dispatcher.Invoke(() => FileBox.Value += 1);
        }

        Application.Current.Dispatcher.Invoke(() => FileBox.Value = FileBox.MaximumValue);

        dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: Sending file ended.", DateTime.Now.ToShortTimeString())));

        //////////////////////////////////////////////////////////////////////////////////////////

        bytes = new byte[1024];
        length = networkStream.Read(bytes, 0, bytes.Length);
        str = Encoding.Default.GetString(bytes, 0, length);

        if (str.Contains("FileReceived"))
            dispatcher.InvokeAsync(() => Logs.Add(string.Format("{0}: The server successfully received the file.", DateTime.Now.ToShortTimeString())));

        ProcessStarted = false;
    }
    private async Task SendFileToServerAsync() => await Task.Factory.StartNew(() => SendFileToServer());
}
