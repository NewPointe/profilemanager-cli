using System;

namespace org.newpointe.util
{
    public static class ArrayUtil
    {

        public static T[] Shift<T>(T[] oldArray)
        {
            if (oldArray.Length == 0)
            {
                return new T[0];
            }
            else
            {
                var newArray = new T[oldArray.Length - 1];
                Array.Copy(oldArray, 1, newArray, 0, oldArray.Length - 1);
                return newArray;
            }
        }

        public static T[] ShiftBy<T>(T[] oldArray, int amount = 1)
        {
            if (oldArray.Length < amount)
            {
                return new T[0];
            }
            else
            {
                var newArray = new T[oldArray.Length - amount];
                Array.Copy(oldArray, amount, newArray, 0, oldArray.Length - amount);
                return newArray;
            }
        }
    }
}