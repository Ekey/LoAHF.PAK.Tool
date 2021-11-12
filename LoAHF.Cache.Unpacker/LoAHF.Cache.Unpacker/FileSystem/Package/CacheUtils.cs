using System;

namespace LoAHF.Cache.Unpacker
{
    class CacheUtils
    {
        public static UInt32 iAlignUInt32(UInt32 dwValue, UInt32 dwAlignSize)
        {
            if (dwValue == 0)
            {
                return dwValue;
            }

            return dwValue + ((dwAlignSize - (dwValue % dwAlignSize)) % dwAlignSize);
        }
    }
}
