﻿using System.Runtime.InteropServices;

namespace Audio_Handler;

//https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
public static class MediaController
{
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
    private const int KeyeventfExtentedkey = 1;
    //private const int KeyeventfKeyup = 0;
    
    public static void PlayPause()
    {
        keybd_event(0xB3, 0, KeyeventfExtentedkey, IntPtr.Zero);
    }

    public static void Next()
    {
        keybd_event(0xB0, 0, KeyeventfExtentedkey, IntPtr.Zero);
    }
    
    public static void Previous()
    {
        keybd_event(0xB1, 0, KeyeventfExtentedkey, IntPtr.Zero);
    }

    public static void Stop()
    {
        keybd_event(0xB2, 0, KeyeventfExtentedkey, IntPtr.Zero);
    }
}