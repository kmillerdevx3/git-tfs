using System;
using System.Runtime.InteropServices;

namespace GitTfs.Util
{
    public class DateTimeWrapper : IDisposable
    {
        private DateTimeOffset _originalDateTimeOffset;
        private DateTimeOffset _newDateTimeOffset;

        public DateTimeWrapper(DateTimeOffset newOffset)
        {
            _newDateTimeOffset = newOffset;
            _originalDateTimeOffset = DateTimeOffset.Now;

            SetCurrentTime(_newDateTimeOffset.ToUniversalTime());
        }

        public void Dispose()
        {
            var currentOffset = DateTimeOffset.Now;

            SetCurrentTime(_originalDateTimeOffset.AddSeconds((currentOffset - _newDateTimeOffset).TotalSeconds).ToUniversalTime());
        }

        private void SetCurrentTime(DateTimeOffset current)
        {

            var st = new SYSTEMTIME
            {
                wYear = (short)current.Year,
                wMonth = (short)current.Month,
                wDay = (short)current.Day,
                wDayOfWeek = (short)current.DayOfWeek,
                wHour = (short)current.Hour,
                wMinute = (short)current.Minute,
                wSecond = (short)current.Second,
                wMilliseconds = (short)current.Millisecond
            };

            SetSystemTime(ref st);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);
    }
}
