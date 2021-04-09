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


            CustomBitArray bitArray = new CustomBitArray(270);
            bitArray.SetBitToOne(0);
      
            bitArray.SetBitToOne(270);
            var resultMin = bitArray.GetBit(0);
            var resultMax = bitArray.GetBit(271);
           
            Console.WriteLine();
        }
        public static void FIllFile()
        {
            string binaryFileName = "NumbersSmall.bin";

            using (FileStream fileStream = new FileStream(binaryFileName, FileMode.Create))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        binaryWriter.Write(i);
                    }
               
                    //binaryWriter.Write(99999);
                    binaryWriter.Write(1);
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
                        if (bitArray.GetBit(result) == true)
                        {
                            Console.WriteLine(result);
                            break;
                        }
                        bitArray.SetBitToOne(result);
                    }


                }
            }
        }
    }
}
