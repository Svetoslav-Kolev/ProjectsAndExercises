using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CustomBitArray
{
    class CustomBitArray :IEnumerable,ICollection
    {
        private int[] bitArray; //actual array that holds the bits (32 bits per element)
        private int count;
        public int maxCapacity;
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
        public bool this[int index]
        {
            get => GetBit(index);
            set
            {
                if (value == true)
                {
                    SetBitToOne(index);
                }
                else
                {
                    SetBitToZero(index);
                }
            }
        }

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public CustomBitArray(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException("Capacity cannot be a negative number.");
            }
            count = 0;
            maxCapacity = n;
            bitArray = new int[(n>>5)+1];   //effectively divides by 2^5 (32)
        }

        
        public void SetBitToOne(int position) //position is acting as the integer you want to store
        {
            var indexAndBitNumber = ReturnIndexAndPosition(position);
            //(1<<bitNumber) sets all bits to 0 except the one on the correct position
            //the | operator sets the bit at this position to One while not interfering with the other bits since (1<<bitNumber) has only one positive bit
            bitArray[indexAndBitNumber.Item1] |= (1 << indexAndBitNumber.Item2);

            count++;
        }
        public  bool GetBit(int position)
        {
            var indexAndBitNumber = ReturnIndexAndPosition(position);
            //returns false if both integers are not 1
            return (bitArray[indexAndBitNumber.Item1] & (1 << indexAndBitNumber.Item2)) != 0;
        }
        public void SetBitToZero(int position)
        {
            var indexAndBitNumber = ReturnIndexAndPosition(position);
            //reverses (1<<bitNumber) so that it has only one zero
            //it doesn't interfere with the other bits, it can only change the bit at the given position (since all other bits are 1)
            bitArray[indexAndBitNumber.Item1] &= ~(1 << indexAndBitNumber.Item2);
            count--;
        }
        private (int,int) ReturnIndexAndPosition(int position)
        {
            if (position > maxCapacity || position<0)
            {
                throw new ArgumentOutOfRangeException("Position is out of range or is a negative value!");
            }
            // Find index of array that holds this bit 
            int index = (position / 32);
            //Find which position the bit takes in array[index]
            int bitNumber = (position % 32);

            return (index, bitNumber);
            
        }

        public IEnumerator GetEnumerator() 
        {
            lock (bitArray)
            {
                int currentPosition = 0;
                foreach (var integer in bitArray)
                {
                    for (int i = 0; i <= 31; i++)
                    {
                        if (currentPosition < maxCapacity)
                        {
                            yield return GetBit(currentPosition);
                            currentPosition++;
                        }
                    }
                }
            }         
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (index < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (array.Rank > 1)
                throw new ArgumentException("array is multidimensional.");
            if (array.Length - index < maxCapacity)
                throw new ArgumentException("Not enough elements after index in the destination array.");
            
           
            if(array.GetType().GetElementType()== typeof(int))
            {
                for (int i = 0; i < maxCapacity; i++)
                {
                    bool currentValue = this[i];
                    if (currentValue)
                    {
                        array.SetValue(1, i + index);
                    }
                    else
                    {
                        array.SetValue(0, i + index);
                    }
                  
                }
            }
            else if (array.GetType().GetElementType() == typeof(byte))
            {
                for (int i = 0; i < maxCapacity; i++)
                {
                    bool currentValue = this[i];
                    if (currentValue)
                    {
                        array.SetValue((byte)1, i + index);
                    }
                    else
                    {
                        array.SetValue((byte)0, i + index);
                    }

                }
            }
            else if (array.GetType().GetElementType() == typeof(bool))
            {
                for (int i = 0; i < maxCapacity; i++)
                {
                    array.SetValue(this[i], i + index);
                }
            }
            else
            {
                throw new InvalidCastException("type not supported");
            }
        }
    } 
}
