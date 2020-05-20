using System;
using System.Collections.Generic;
using System.Text;

namespace FileSizeToString.Classes
{
   public static class FormattingExtensions
    {
        public static string[] sizes = new string[6] { "bytes", "KB", "MB", "GB", "TB", "PB" };
        //fileSize base is bytes
        // file Size lengths - 4 - KB , 7 - MB, 10 - GB , 13 - TB, 16 - PB
        public static string FileSizeToString(long fileSize, int precision = 2)
        {
            string result = "";
            int fileSizeLength = (int)Math.Ceiling(Math.Log10(fileSize));
            double tempSize = 0;    
            int order = (fileSizeLength - 1) / 3;
            if (fileSize < 1024)
            {
                result = Convert.ToInt32(fileSize) + " bytes";
            }
            else
            {
                tempSize = Convert.ToDouble(fileSize);
                for (int i = 0; i < order; i++)
                {
                    tempSize /= 1024;
                }
                result = CheckSize(tempSize, sizes[order-1], sizes[order], precision);
            }
            return result;
        }
        private static string CheckSize(double tempSize , string sizeLower, string sizeUpper,int precision)
        {
            string result = "";
            if (tempSize < 1)
            {
                tempSize *= 1024;
                tempSize = Math.Round(tempSize, precision); //Math Round here for most accurate results
                if(tempSize/ 1024 == 1)
                {
                    //After rounding the size it may need to be reverted to the upper designation - 1023,9990234375KB rounds to 1024 rounds to 1 mb
                    tempSize /= 1024;
                    result = $"{tempSize.ToString("N" + precision)} {sizeUpper}";
                }
                else
                {
                    result = $"{tempSize.ToString("N" + precision)} {sizeLower}";
                }
               
            }
            else
            {
                tempSize = Math.Round(tempSize, precision);
                result = $"{tempSize.ToString("N" + precision)} {sizeUpper}";
            }
            return result;
        }
    }
}
