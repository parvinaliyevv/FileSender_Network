namespace FileSender.Client.Models;

public class FilePacket
{
    public string FileName { get; set; }
    public long Length { get; set; }

    
    public FilePacket(string fileName, long length)
    {
        FileName = fileName;
        Length = length;
    }
}
