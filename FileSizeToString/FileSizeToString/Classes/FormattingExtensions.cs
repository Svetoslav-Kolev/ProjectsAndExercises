using System;
using System.Collections.Generic;
using System.Text;

namespace FileSizeToString.Classes
{
   public static class FormattingExtensions
    {
        //fileSize base is bytes
        public static string FileSizeToString(long fileSize, int precision = 2)
        {
            string result = "";
            // file Size lengths - 4 - KB , 7 - MB, 10 - GB , 13 - TB, 16 - PB
            int fileSizeLength = fileSize.ToString().Length;
            double tempSize = 0;
            string[] sizes = new string[6];
            sizes[1] = "bytes KB";
            sizes[2] = "KB MB";
            sizes[3] = "MB GB";
            sizes[4] = "GB TB";
            sizes[5] = "TB PB";
            if (fileSize < 1024)
            {
                result = Convert.ToInt32(fileSize) + " bytes";
            }
            else
            {
                string[] size = sizes[(fileSizeLength - 1) / 3].Split();
                string lower = size[0];
                string higher = size[1];
                tempSize = (Convert.ToDouble(fileSize) / Math.Pow(1024, (fileSizeLength-1)/3));
                result = CheckSize(tempSize, lower, higher, precision,fileSizeLength);
            }
            return result;
        }
        private static string CheckSize(double tempSize , string sizeLower, string sizeUpper,int precision,int fileSizeLength)
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
