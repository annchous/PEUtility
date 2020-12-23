using System;

namespace PEUtility.Tools
{
    public static class TimeDateStampToDateTimeConverter
    {
        /// <summary>
        /// Converts TimeDateStamp given in seconds
        /// to UTC DateTime format.
        /// </summary>
        /// <param name="timeDateStamp">Value in seconds</param>
        /// <returns></returns>
        public static DateTime ConvertTimeDateStamp(UInt32 timeDateStamp)
        {
            var result = new DateTime(1970, 1, 1, 0, 0, 0);
            return result.AddSeconds(timeDateStamp);
        }
    }
}