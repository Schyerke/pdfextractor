using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfExtractor
{
    internal class FileExtractor
    {
        public string TempExtractFolder { get; set; }
        public string Extention { get; set; }
        public string PdfDir { get; set; }
        public string Output { get; set; }

        public FileExtractor(string pdfDir, string output, string extention)
        {
            PdfDir = pdfDir;
            Output = output;
            Extention = extention;
            TempExtractFolder = "";
        }

        public void Execute()
        {
            if (!Directory.Exists(PdfDir) && !PdfDir.EndsWith(".zip"))
            {
                Console.Error.WriteLine($"{PdfDir} doesn't exist");
                return;
            }
            if (!Directory.Exists(Output))
            {
                Console.WriteLine($"Creating {Output} directory");
                Directory.CreateDirectory(Output);
            }
            PdfDir = ExtractZipFolder(PdfDir);
            ProcessDirectory(Directory.EnumerateDirectories(PdfDir));
            ProcessFile(Directory.EnumerateFiles(PdfDir));

            //ending
            if(TempExtractFolder != null && TempExtractFolder.Length >= 1)
            {
                Directory.Delete(TempExtractFolder, true);
            }
            TempExtractFolder = "";
        }

        private string ExtractZipFolder(string pdfDir)
        {
            if (!pdfDir.EndsWith(".zip"))
            {
                return pdfDir;
            }
            string tempFolder = Path.GetTempPath();
            TempExtractFolder = tempFolder + "extract";
            ZipFile.ExtractToDirectory(pdfDir, TempExtractFolder, true);
            return TempExtractFolder;
        }

        private void ProcessDirectory(IEnumerable<string> dirs)
        {
            foreach (string dir in dirs)
            {
                IEnumerable<string> childFiles = Directory.EnumerateFiles(dir);
                ProcessFile(childFiles);

                IEnumerable<string> childDirs = Directory.EnumerateDirectories(dir);
                ProcessDirectory(childDirs);
            }
        }

        private void ProcessFile(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                if (file.EndsWith(Extention))
                {
                    string filename = GetFileName(file);
                    if (File.Exists(Output + filename))
                    {
                        Console.Error.WriteLine($"{filename.Substring(1)} already exists in output's directory");
                    }
                    else
                    {
                        Console.WriteLine($"Coping {filename.Substring(1)} to {Output}");
                        File.Copy(file, Output + filename);
                    }
                }
            }
        }

        private string GetFileName(string file)
        {
            int start = file.Length - 1;
            for(int i = file.Length-1; i >= 0; i--)
            {
                if (file[i] != '/' && file[i] != '\\')
                {
                    start--;
                } 
                else
                {
                    break;
                }
            }
            return file.Substring(start);
        }
    }
}
