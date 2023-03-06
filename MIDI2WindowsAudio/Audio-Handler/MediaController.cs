﻿using System.Runtime.InteropServices;

namespace Audio_Handler;

//https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
public static class MediaController
{
    [DllImport("user32.dll")]
    private static extern void KeybdEvent(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
    private const int KeyEventFExtendedKey = 1;
    //private const int KeyEventFKeyup = 0;
    
    public static void PlayPause()
    {
        KeybdEvent(0xB3, 0, KeyEventFExtendedKey, IntPtr.Zero);
    }

    public static void Next()
    {
        KeybdEvent(0xB0, 0, KeyEventFExtendedKey, IntPtr.Zero);
    }
    
    public static void Previous()
    {
        KeybdEvent(0xB1, 0, KeyEventFExtendedKey, IntPtr.Zero);
    }

    public static void Stop()
    {
        KeybdEvent(0xB2, 0, KeyEventFExtendedKey, IntPtr.Zero);
    }
}