using System;
using System.Collections;
using System.IO;

namespace CustomBitArray
{
    class Program
    {
        static void Main(string[] args)
        {
            //FIllFile();
            //CheckDuplicate();


            CustomBitArray bitArray = new CustomBitArray(10);
            bitArray.SetBitToOne(7);
            foreach (bool item in bitArray)
            {
                Console.WriteLine(item);
            }
            var resulT = bitArray[7];
            var result2 = bitArray[5];
            var result3 = bitArray[10];
            int[] test = new int[50];
            bitArray.CopyTo(test, 0);
            Console.WriteLine();

        }
        public static void FIllFile()
        {
            string binaryFileName = "NumbersSmall.bin";

            using (FileStream fileStream = new FileStream(binaryFileName, FileMode.Create))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    //for (int i = 0; i < 1000000; i++)
                    //{
                    //    binaryWriter.Write(i);
                    //}
               
                    binaryWriter.Write(100);
                    binaryWriter.Write(100);
                }
            }
        }
        public static void CheckDuplicate()
        {
            CustomBitArray bitArray = new CustomBitArray(int.MaxValue); // file currently holds only integers
            string binaryFileName = "NumbersSmall.bin";
            long toRead = new FileInfo(binaryFileName).Length;
            using (FileStream fileStream = new FileStream(binaryFileName, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    for (int i = 0; i < toRead / 4; i++)
                    {
                        var result = binaryReader.ReadInt32();
                        if (bitArray[result])
                        {
                            Console.WriteLine(result);
                            break;
                        }
                        bitArray[result] = true;
                    }
                }
            }
        }
    }
}
