using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TimeLapser
{
    class Utilities
    {
        // Import the user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        // Declare some keyboard keys as constants with its respective code
        // See Virtual Code Keys: https://msdn.microsoft.com/en-us/library/dd375731(v=vs.85).aspx
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int VK_F12 = 0x7B; //Right Control key code

        public static void PressScreenShotKey()
        {

            // Simulate a key press event
            keybd_event(VK_F12, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_F12, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
