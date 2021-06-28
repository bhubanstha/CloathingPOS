using Microsoft.Win32;
using System;
using System.IO;

namespace POS.Core.Utilities
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

        public static bool  SaveProfileFile(string sourcefile, string destinationFileName)
        {
            try
            {
                string logoPath = FilePath.GetProfileImageFullPath("");
                CreateDirectory(logoPath);
                return CopyFile(sourcefile, Path.Combine(logoPath, destinationFileName));
            }
            catch {
                return false;
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

        private static bool CopyFile(string source, string destination)
        {
            if(!File.Exists(destination))
            {
                File.Copy(source, destination);
                return true;
            }
            return false;
            
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
