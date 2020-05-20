using FileSizeToString.Classes;
using System;

namespace FileSizeToString
{
    class Startup
    {
        static void Main(string[] args)
        {
            Console.WriteLine(FormattingExtensions.FileSizeToString(00000000000));
            Console.WriteLine(FormattingExtensions.FileSizeToString(107374182));
            Console.WriteLine(FormattingExtensions.FileSizeToString(0));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1));
            Console.WriteLine(FormattingExtensions.FileSizeToString(2));
            Console.WriteLine(FormattingExtensions.FileSizeToString(511));
            Console.WriteLine(FormattingExtensions.FileSizeToString(512));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1023));  
            Console.WriteLine(FormattingExtensions.FileSizeToString(1024));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1025));
            Console.WriteLine(FormattingExtensions.FileSizeToString(100000));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1010000));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1048575));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1048576));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1048577));
            Console.WriteLine(FormattingExtensions.FileSizeToString(14288043651787));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1126,1));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1127,1));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1178,1));
            Console.WriteLine(FormattingExtensions.FileSizeToString(20,0));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1024,0));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1127, 0));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1536, 0));
            Console.WriteLine(FormattingExtensions.FileSizeToString(1025899906842623));

        }
    }
}
