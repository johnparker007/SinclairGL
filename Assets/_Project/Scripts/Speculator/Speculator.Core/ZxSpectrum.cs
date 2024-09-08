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

//using CSharp.Core.ViewModels;
using Speculator.Core.Tape;
using System;
using System.IO;

namespace Speculator.Core
{

    /// <summary>
    /// The main emulation entry point object.
    /// </summary>
//    public class ZxSpectrum : ViewModelBase, IDisposable
    public class ZxSpectrum : /*ViewModelBase,*/ IDisposable
    {
        private SoundHandler m_soundHandler;
        private readonly ZxFileIo m_zxFileIo;
        private ClockSync.Speed m_emulationSpeed;

        private ZxDisplay TheDisplay { get; }
        public CPU TheCpu { get; }
        public ZxPortHandler PortHandler { get; }
        public SoundHandler SoundHandler => m_soundHandler ??= new SoundHandler();
        public TapeLoader TheTapeLoader { get; } = new TapeLoader();
        public Debugger.Debugger TheDebugger { get; }
        public CpuHistory CpuHistory { get; }

        public ClockSync.Speed EmulationSpeed
        {
            get => m_emulationSpeed;
            set
            {
                //if (!SetField(ref m_emulationSpeed, value))
                //    return;

                m_emulationSpeed = value;

                TheCpu.SetSpeed(value);
                TheDisplay.IsPaused = value == ClockSync.Speed.Pause;
            }
        }

        public ZxSpectrum(ZxDisplay display)
        {
            TheDisplay = display;
            PortHandler = new ZxPortHandler(SoundHandler, TheDisplay, TheTapeLoader);
            TheCpu = new CPU(new Memory(), PortHandler, SoundHandler);
            TheTapeLoader.SetCpu(TheCpu);
            TheDebugger = new Debugger.Debugger(TheCpu);

            TheCpu.RenderScanline += TheDisplay.OnRenderScanline;

            TheDebugger.IsSteppingChanged += (_, _) =>
            {
                if (TheDebugger.IsStepping)
                    SoundHandler.SetEnabled(false);
            };

            m_zxFileIo = new ZxFileIo(TheCpu, TheDisplay, TheTapeLoader);
            CpuHistory = new CpuHistory(TheCpu, m_zxFileIo);
        }

        public void PowerOnAsync() =>
            TheCpu.PowerOnAsync();

        public void LoadSystemRom(FileInfo systemRom)
        {
            EmulationSpeed = ClockSync.Speed.Actual;
            m_zxFileIo.LoadSystemRom(systemRom);
        }

        public void LoadRom(FileInfo romFile)
        {
            EmulationSpeed = ClockSync.Speed.Actual;
            m_zxFileIo.LoadFile(romFile);
        }

        public void SaveRom(FileInfo romFile) =>
            m_zxFileIo.SaveFile(romFile);

        public void ResetAsync()
        {
            EmulationSpeed = ClockSync.Speed.Actual;
            TheCpu.ResetAsync();
        }

        public void Dispose()
        {
            m_soundHandler?.Dispose();
            PortHandler?.Dispose();
            TheCpu?.PowerOffAsync();
        }
    }
}