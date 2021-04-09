using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CustomBitArray
{
    class CustomBitArray 
    {
        private int[] bitArray; //actual array that holds the bits (32 bits per element)
        private int count;
        public int Count 
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }
        public CustomBitArray(int n)
        {
            count = 0;
            bitArray = new int[(n>>5)+1];   //effectively divides by 2^5 (32)
        }

        
        public void SetBitToOne(int position) //position is acting as the integer you want to store
        {
            // Find index of array that holds this bit 
            int index = (position / 32);
            //Find which position the bit takes in array[index]
            int bitNumber = (position % 32);
            //(1<<bitNumber) sets all bits to 0 except the one on the correct position
            //the | operator sets the bit at this position to One while not interfering with the other bits since (1<<bitNumber) has only one positive bit
            bitArray[index] |= (1 << bitNumber);

            count++;
        }
        public  bool GetBit(int position)
        {
            int index = (position / 32);
            int bitNumber = (position %32 );
            //returns false if both integers are not 1
            return (bitArray[index] & (1 << bitNumber)) != 0;
        }
        public void SetBitToZero(int position)
        {
            int index = (position /32 );
            int bitNumber = (position % 32);
            //reverses (1<<bitNumber) so that it has only one zero
            //it doesn't interfere with the other bits, it can only change the bit at the given position (since all other bits are 1)
            bitArray[index] &= ~(1 << bitNumber);
            count--;
        }
    } 
}
