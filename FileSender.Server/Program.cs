namespace FileSender.Server;

public static class Program
{
    public static TcpListener Server { get; set; }
    public static string FolderPath { get; set; }


    static Program()
    {
        var address = IPAddress.Parse(ConfigurationManager.AppSettings["IpAddress"]);
        var port = int.Parse(ConfigurationManager.AppSettings["PortNumber"]);

        FolderPath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Assembly.GetExecutingAssembly().GetName().Name);

        Server = new TcpListener(address, port);
    }


    private static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        TcpClient? client = null;

        if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
        
        try
        {
            Server.Start();
        }
        catch
        {
            Console.WriteLine("[{0}] - {1}: Failed to start server!", DateTime.Now, Server.Server.LocalEndPoint); ; Console.ReadKey(); return;
        }

        Console.WriteLine("[{0}] - {1}: Server started!", DateTime.Now, Server.Server.LocalEndPoint);

        while (true)
        {
            client = Server.AcceptTcpClient();
            CommunicationWithClientAsync(client);

            Console.WriteLine("[{0}] - {1}: Connected to server!", DateTime.Now, client.Client.RemoteEndPoint);
        }
    }

    private static void CommunicationWithClient(TcpClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        var networkStream = client.GetStream();

        string str = string.Empty;
        byte[]? bytes = null;
        int length = default;

        while (true)
        {
            if (networkStream.DataAvailable)
            {
                //////////////////////////////////////////////////////////////////////////////////////////

                bytes = new byte[1024];
                length = networkStream.Read(bytes, 0, bytes.Length);
                str = Encoding.Default.GetString(bytes, 0, length);

                if (str.Contains("DisConnected"))
                {
                    Console.WriteLine("[{0}] - {1}: Client DisConnect Server!", DateTime.Now, client.Client.RemoteEndPoint);
                    return;
                }

                var packet = JsonSerializer.Deserialize<FilePacket>(str);
                var filePath = string.Format(@"{0}\{1}", FolderPath, packet.FileName);

                if (File.Exists(filePath)) File.Delete(filePath);
                using var fileStream = File.Create(filePath);

                //////////////////////////////////////////////////////////////////////////////////////////

                bytes = Encoding.Default.GetBytes("FilePacketReceived");
                networkStream.Write(bytes, 0, bytes.Length);

                //////////////////////////////////////////////////////////////////////////////////////////

                long i = default;

                while (i < packet.Length)
                {
                    bytes = new byte[1024 * 5000];
                    length = networkStream.Read(bytes, 0, bytes.Length);

                    fileStream.Write(bytes, 0, length);
                    i += length;
                }

                //////////////////////////////////////////////////////////////////////////////////////////

                bytes = Encoding.Default.GetBytes("FileReceived");
                networkStream.Write(bytes, 0, bytes.Length);

                Console.WriteLine("[{0}] - {1}: File received successfully!", DateTime.Now, client.Client.RemoteEndPoint);
            }
        }
    }
    private static async Task CommunicationWithClientAsync(TcpClient client)
        => await Task.Factory.StartNew(() => CommunicationWithClient(client));
}
