﻿using System;
using System.Runtime.InteropServices;

namespace ColorPicker.ColorPickingFunctionality.SystemEvents
{
    class RegisteredMouseEventHook : SystemHook
    {
        public delegate void EventCallback(int x, int y);

        // Low-level mouse movement hook type
        private const int WH_MOUSE_LL = 14;

        // Event code for mouse down
        private const int WM_LBUTTONDOWN = 0x0201;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private EventCallback callback;

        public RegisteredMouseEventHook(EventCallback callback)
            : base(WH_MOUSE_LL)
        {
            this.callback = callback;
        }

        public override int HookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == WM_LBUTTONDOWN)
            {
                MSLLHOOKSTRUCT mouseHookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                callback(mouseHookStruct.pt.x, mouseHookStruct.pt.y);
            }
            return CallNextHookExWrapper(nCode, wParam, lParam);
        }
    }
}
