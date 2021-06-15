using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities
{
    public class FileUtility
    {
        public static string OpenImageFilePicker()
        {
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            opfd.FilterIndex = 1;
            opfd.Multiselect = false;
            opfd.RestoreDirectory = true;
            if (opfd.ShowDialog() == true)
            {
                return opfd.FileName;
            }
            return "";
        }


        public static void SaveLogoFile(string sourcefile, string destinationFileName)
        {
            try
            {
                string logoPath = FilePath.GetLogoFullPath("");                
                CreateDirectory(logoPath);
                CleanupDirectory(logoPath);
                File.Copy(sourcefile, Path.Combine(logoPath, destinationFileName));
            }
            catch
            {
                throw;
            }
        }

        public static void SaveProfileFile(string sourcefile, string destinationFileName)
        {
            try
            {
                string logoPath = FilePath.GetProfileImageFullPath("");
                CreateDirectory(logoPath);
                CopyFile(sourcefile, Path.Combine(logoPath, destinationFileName));
            }
            catch             {

                throw;
            }
        }

        private static void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private static void CopyFile(string source, string destination)
        {
            File.Copy(source, destination);
        }
        private static void CleanupDirectory(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
