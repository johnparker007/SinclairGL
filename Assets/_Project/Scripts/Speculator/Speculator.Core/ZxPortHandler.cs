// Code authored by Dean Edis (DeanTheCoder).
// Anyone is free to copy, modify, use, compile, or distribute this software,
// either in source code form or as a compiled binary, for any non-commercial
// purpose.
//
// If you modify the code, please retain this copyright header,
// and consider contributing back to the repository or letting us know
// about your modifications. Your contributions are valued!
//
// THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND.

using CSharp.Core.Extensions;
//using CSharp.Core.ViewModels;
//using SharpHook;
//using SharpHook.Native;
//using Speculator.Core.Tape;
using System;
using System.Collections.Generic;

namespace Speculator.Core
{
    /// <summary>
    /// Detects key presses and speaker state changes, and feeds port info back to the emulator.
    /// </summary>
//    public class ZxPortHandler : ViewModelBase, IPortHandler, IDisposable
    public class ZxPortHandler : /*ViewModelBase,*/ IPortHandler/*, IDisposable*/
    {
        //private readonly SoundHandler m_soundHandler;
        //private readonly ZxDisplay m_theDisplay;
        //private readonly TapeLoader m_tapeLoader;
        //private readonly List<KeyCode> m_realKeysPressed = new List<KeyCode>();
        //private readonly List<(KeyCode[] Pc, KeyCode[] Speccy)> m_pcToSpectrumKeyMap;
        //private readonly List<(KeyCode[] Pc, KeyCode[] Speccy)> m_pcToSpectrumKeyMapWithJoystick;
        //private readonly SimpleGlobalHook m_keyboardHook;
        //private bool m_emulateCursorJoystick;
        //private bool m_handleKeyEvents = true;
        //private bool? m_tapeSignal;

        ///// <summary>
        ///// The keyboard hooks work regardless of whether the app has focus.
        ///// This flag ensures we ignore events we don't want.
        ///// </summary>
        //private bool HandleKeyEvents
        //{
        //    get => m_handleKeyEvents;
        //    set
        //    {
        //        if (m_handleKeyEvents == value)
        //            return;
        //        m_handleKeyEvents = value;

        //        lock (m_realKeysPressed)
        //            m_realKeysPressed.Clear();
        //    }
        //}

        ///// <summary>
        ///// Whether cursor or Kempston joystick is enabled.
        ///// </summary>
        //public bool EmulateCursorJoystick
        //{
        //    get => m_emulateCursorJoystick;
        //    set => SetField(ref m_emulateCursorJoystick, value);
        //}

        //public ZxPortHandler(SoundHandler soundHandler, ZxDisplay theDisplay, TapeLoader tapeLoader)
        //{
        //    m_soundHandler = soundHandler;
        //    m_theDisplay = theDisplay;
        //    m_tapeLoader = tapeLoader;

        //    // Map PC key to a sequence of emulated Speccy keys.
        //    m_pcToSpectrumKeyMap = new List<(KeyCode[], KeyCode[])>();
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcBackspace), K(KeyCode.VcLeftShift, KeyCode.Vc0)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcComma), K(KeyCode.VcRightShift, KeyCode.VcN)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcComma, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.VcR)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcPeriod), K(KeyCode.VcRightShift, KeyCode.VcM)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcPeriod, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.VcT)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcEquals), K(KeyCode.VcRightShift, KeyCode.VcL)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcEquals, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.VcK)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcMinus), K(KeyCode.VcRightShift, KeyCode.VcJ)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcMinus, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.Vc0)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcSlash), K(KeyCode.VcRightShift, KeyCode.VcV)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcSlash, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.VcC)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcQuote), K(KeyCode.VcRightShift, KeyCode.Vc7)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcQuote, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.VcP)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcSemicolon), K(KeyCode.VcRightShift, KeyCode.VcO)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcSemicolon, KeyCode.VcLeftShift), K(KeyCode.VcRightShift, KeyCode.VcZ)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.VcLeftAlt), K(KeyCode.VcLeftShift, KeyCode.VcRightShift))); // Note: Left shift will act like CAPS SHIFT to allow access to the Speccy'))s
        //    // special keys (cursors, GRAPHICS, etc).
        //    // Right-shift maps PC keys to Speccy equivalent. (E.g. Round brackets)
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.Vc7, KeyCode.VcRightShift), K(KeyCode.VcRightShift, KeyCode.Vc6)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.Vc8, KeyCode.VcRightShift), K(KeyCode.VcRightShift, KeyCode.VcB)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.Vc9, KeyCode.VcRightShift), K(KeyCode.VcRightShift, KeyCode.Vc8)));
        //    m_pcToSpectrumKeyMap.Add((K(KeyCode.Vc0, KeyCode.VcRightShift), K(KeyCode.VcRightShift, KeyCode.Vc9)));

        //    // Make right-shift mirror left shift.
        //    m_pcToSpectrumKeyMap
        //        .Where(o => o.Pc.Length == 2 && o.Pc[1] == KeyCode.VcLeftShift)
        //        .ToList()
        //        .ForEach(o => m_pcToSpectrumKeyMap.Add((K(o.Pc[0], KeyCode.VcRightShift), o.Speccy)));

        //    // Make extended key map for Cursor Joystick support.
        //    m_pcToSpectrumKeyMapWithJoystick = m_pcToSpectrumKeyMap.ToList();
        //    m_pcToSpectrumKeyMapWithJoystick.Add((K(KeyCode.VcUp), K(KeyCode.Vc7)));
        //    m_pcToSpectrumKeyMapWithJoystick.Add((K(KeyCode.VcDown), K(KeyCode.Vc6)));
        //    m_pcToSpectrumKeyMapWithJoystick.Add((K(KeyCode.VcLeft), K(KeyCode.Vc5)));
        //    m_pcToSpectrumKeyMapWithJoystick.Add((K(KeyCode.VcRight), K(KeyCode.Vc8)));
        //    m_pcToSpectrumKeyMapWithJoystick.Add((K(KeyCode.VcBackQuote), K(KeyCode.Vc0)));
        //    m_pcToSpectrumKeyMapWithJoystick.Add((K(KeyCode.VcBackslash), K(KeyCode.Vc0)));

        //    m_keyboardHook = new SimpleGlobalHook();
        //    m_keyboardHook.KeyPressed += (_, args) => SetKeyDown(args.Data.KeyCode);
        //    m_keyboardHook.KeyReleased += (_, args) => SetKeyUp(args.Data.KeyCode);
        //}

        //public void StartKeyboardHook() => m_keyboardHook.RunAsync();

        //private static KeyCode[] K(params KeyCode[] keyCodes) => keyCodes;

        public byte In(ushort portAddress)
        {
            var result = (byte)0xFF; // 'floating' bux value.

            //if ((portAddress & 0x00FF) == 0xFE)
            //{
            //    result = ReadKeyboardPort(portAddress);

            //    // Read from the tape, if loaded.
            //    m_tapeSignal = m_tapeLoader.GetTapeSignal();
            //    result = m_tapeSignal == true ? result.SetBit(6) : result.ResetBit(6);
            //}
            //else if ((portAddress & 0x001F) == 0x1F)
            //{
            //    result = ReadJoystickPort();
            //}

            return result;
        }

        //private byte ReadJoystickPort()
        //{
        //    lock (m_realKeysPressed)
        //    {
        //        byte b = 0x00;
        //        if (IsZxKeyPressed(KeyCode.VcBackQuote) || IsZxKeyPressed(KeyCode.VcBackslash))
        //            b = (byte)(b | 0x10); // Fire.
        //        if (IsZxKeyPressed(KeyCode.VcUp))
        //            b = (byte)(b | 0x8);
        //        if (IsZxKeyPressed(KeyCode.VcDown))
        //            b = (byte)(b | 0x4);
        //        if (IsZxKeyPressed(KeyCode.VcLeft))
        //            b = (byte)(b | 0x2);
        //        if (IsZxKeyPressed(KeyCode.VcRight))
        //            b = (byte)(b | 0x1);
        //        return b;
        //    }
        //}

        //private byte ReadKeyboardPort(ushort portAddress)
        //{
        //    lock (m_realKeysPressed)
        //    {
        //        byte result = 0x00;
        //        var hi = (byte)(portAddress >> 8);

        //        if ((hi & 0x80) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.VcB)) result |= 1 << 4;
        //            if (IsZxKeyPressed(KeyCode.VcN)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.VcM)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.VcRightShift)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.VcSpace)) result |= 1 << 0;
        //        }

        //        if ((hi & 0x08) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.Vc1)) result |= 1 << 0;
        //            if (IsZxKeyPressed(KeyCode.Vc2)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.Vc3)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.Vc4)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.Vc5)) result |= 1 << 4;
        //        }

        //        if ((hi & 0x10) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.Vc6)) result |= 1 << 4;
        //            if (IsZxKeyPressed(KeyCode.Vc7)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.Vc8)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.Vc9)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.Vc0)) result |= 1 << 0;
        //        }

        //        if ((hi & 0x04) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.VcQ)) result |= 1 << 0;
        //            if (IsZxKeyPressed(KeyCode.VcW)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.VcE)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.VcR)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.VcT)) result |= 1 << 4;
        //        }

        //        if ((hi & 0x20) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.VcY)) result |= 1 << 4;
        //            if (IsZxKeyPressed(KeyCode.VcU)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.VcI)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.VcO)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.VcP)) result |= 1 << 0;
        //        }

        //        if ((hi & 0x02) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.VcA)) result |= 1 << 0;
        //            if (IsZxKeyPressed(KeyCode.VcS)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.VcD)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.VcF)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.VcG)) result |= 1 << 4;
        //        }

        //        if ((hi & 0x40) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.VcH)) result |= 1 << 4;
        //            if (IsZxKeyPressed(KeyCode.VcJ)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.VcK)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.VcL)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.VcEnter)) result |= 1 << 0;
        //        }

        //        if ((hi & 0x01) == 0)
        //        {
        //            if (IsZxKeyPressed(KeyCode.VcLeftShift)) result |= 1 << 0;
        //            if (IsZxKeyPressed(KeyCode.VcZ)) result |= 1 << 1;
        //            if (IsZxKeyPressed(KeyCode.VcX)) result |= 1 << 2;
        //            if (IsZxKeyPressed(KeyCode.VcC)) result |= 1 << 3;
        //            if (IsZxKeyPressed(KeyCode.VcV)) result |= 1 << 4;
        //        }

        //        return (byte)~result;
        //    }
        //}

        public void Out(byte port, byte b)
        {
            // We only care about writes to port 0xFE.
            if (port != 0xFE)
                return;

            //// Sounds.
            //var speakerState = (byte)((b & 0x18) >> 3);
            //if (m_tapeSignal.HasValue)
            //{
            //    // Tape loading is active, so ensure the sound is piped out.
            //    speakerState = (byte)((speakerState & 0xfe) + (m_tapeSignal.Value ? 1 : 0));
            //}

            //m_soundHandler?.SetSpeakerState(speakerState);

            //// Lower 3 bits will set the border color.
            //if (m_theDisplay != null)
            //    m_theDisplay.BorderAttr = (byte)(b & 0x07);
        }

        //private bool IsZxKeyPressed(KeyCode key)
        //{
        //    if (!HandleKeyEvents)
        //        return false;

        //    if (m_realKeysPressed.Count == 0)
        //        return false; // Nothing pressed.

        //    if (m_realKeysPressed.Contains(KeyCode.VcLeftMeta) || m_realKeysPressed.Contains(KeyCode.VcRightMeta))
        //        return false; // Mac user probably triggering a menu item.

        //    var keyMap = EmulateCursorJoystick ? m_pcToSpectrumKeyMapWithJoystick : m_pcToSpectrumKeyMap;

        //    // Match double key press combos first.
        //    var zxPressed = m_realKeysPressed.ToList();
        //    for (var comboLength = 2; comboLength >= 1; comboLength--)
        //    {
        //        var didRemap = false;

        //        for (var i = 0; i < keyMap.Count; i++)
        //        {
        //            if (keyMap[i].Pc.Length != comboLength)
        //                continue;
        //            if (!AreAllKeysPressed(keyMap[i].Pc))
        //                continue;

        //            var (keyCodes, replacements) = keyMap[i];
        //            for (var j = 0; j < keyCodes.Length; j++)
        //                zxPressed.Remove(keyCodes[j]);
        //            zxPressed.AddRange(replacements);
        //            didRemap = true;
        //        }

        //        if (didRemap)
        //            break;
        //    }

        //    return zxPressed.Contains(key);
        //}

        //private bool AreAllKeysPressed(KeyCode[] keyCodes)
        //{
        //    for (var i = 0; i < keyCodes.Length; i++)
        //    {
        //        if (!m_realKeysPressed.Contains(keyCodes[i]))
        //            return false;
        //    }

        //    return true;
        //}

        //private void SetKeyDown(KeyCode keyCode)
        //{
        //    lock (m_realKeysPressed)
        //    {
        //        if (!m_realKeysPressed.Contains(keyCode))
        //            m_realKeysPressed.Add(keyCode);
        //    }
        //}

        //private void SetKeyUp(KeyCode keyCode)
        //{
        //    lock (m_realKeysPressed)
        //    {
        //        if (keyCode is KeyCode.VcLeftShift or KeyCode.VcRightShift)
        //        {
        //            // Special case - Key detection only reporting one key up event in this case.
        //            m_realKeysPressed.Remove(KeyCode.VcLeftShift);
        //            m_realKeysPressed.Remove(KeyCode.VcRightShift);
        //            return;
        //        }

        //        m_realKeysPressed.Remove(keyCode);
        //    }
        //}

        //public void Dispose() =>
        //    m_keyboardHook?.Dispose();

        //public IDisposable CreateKeyBlocker() => new KeyBlocker(this);

        /// <summary>
        /// Blocks keyboard input from being registered.
        /// Useful when we know the Speccy doesn't have focus (like when a file open dialog is active).
        /// </summary>
        //private class KeyBlocker : IDisposable
        //{
        //    private readonly ZxPortHandler m_portHandler;
        //    private readonly bool m_oldHandleKeyEvents;

        //    internal KeyBlocker(ZxPortHandler portHandler)
        //    {
        //        m_portHandler = portHandler;
        //        m_oldHandleKeyEvents = portHandler.HandleKeyEvents;
        //        portHandler.HandleKeyEvents = false;
        //    }

        //    public void Dispose() =>
        //        m_portHandler.HandleKeyEvents = m_oldHandleKeyEvents;
        //}
    }
}