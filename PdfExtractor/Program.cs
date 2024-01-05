using System;

namespace PdfExtractor
{
    public class Program
    {
        //Input: [pdf] dir
        //Output: [output] dir
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Type --help");
                return;
            }
            if (args[0] == "--help")
            {
                Console.WriteLine("expdf [pdf dir] [output dir]");
                Console.WriteLine("expdf [pdf dir] [output dir] [extension] // .pdf .txt");
                return;
            }
            if (args.Length < 2)
            {
                Console.WriteLine("Type --help");
                return;
            }
            string pdfDir = args[0];
            string output = args[1];

            string extention = ".pdf";
            if(args.Length < 3)
            {
                extention = args[2];
            }

            FileExtractor fileExtractor = new FileExtractor(pdfDir, output, extention);
            fileExtractor.Execute();
        }

        
    }
}