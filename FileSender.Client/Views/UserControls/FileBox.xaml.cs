namespace FileSender.Client.Views.UserControls;

public partial class FileBox : UserControl
{
    public string Filename { get; set; }

    public string Extension { get; set; }

    public decimal Length { get; set; }

    public string LengthType
    {
        get { return (string)GetValue(LengthTypeProperty); }
        set { SetValue(LengthTypeProperty, value); }
    }
    public static readonly DependencyProperty LengthTypeProperty =
        DependencyProperty.Register("LengthType", typeof(string), typeof(FileBox));

    public int Value
    {
        get { return (int)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(int), typeof(FileBox));

    public int MaximumValue
    {
        get { return (int)GetValue(MaximumValueProperty); }
        set { SetValue(MaximumValueProperty, value); }
    }
    public static readonly DependencyProperty MaximumValueProperty =
        DependencyProperty.Register("MaximumValue", typeof(int), typeof(FileBox));


    public FileBox(string filename, decimal length, string lengthType)
    {
        InitializeComponent();

        Filename = filename;
        Length = length;
        LengthType = lengthType;

        DataContext = this;

        if (Length > 1024)
        {
            LengthType = "Kb";
            Length /= 1024;

            if (Length > 1024)
            {
                LengthType = "Mb";
                Length /= 1024;

                if (Length > 1024)
                {
                    LengthType = "Gb";
                    Length /= 1024;
                }
            }

            Length = Math.Round(Length, 2);
        }

        if (Filename.Length > 20) Filename = Filename.Substring(0, 20) + "...";
    }
}
