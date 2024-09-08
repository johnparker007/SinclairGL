using Speculator.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EmulatorController : MonoBehaviour
{
    public string RomFilePath;
    public string SnapshotFilePath;

    public ushort CPU_PC;

    public ZxDisplay ZxDisplay
    {
        get;
        private set;
    } = null;

    public ZxSpectrum ZxSpectrum
    {
        get;
        private set;
    } = null;


    private void Start()
    {
        ZxDisplay = new ZxDisplay();
        ZxSpectrum = new ZxSpectrum(ZxDisplay);

        FileInfo romFileInfo = new FileInfo(RomFilePath);
        ZxSpectrum.LoadSystemRom(romFileInfo);

        ZxSpectrum.PowerOnAsync();

        if (SnapshotFilePath.Length > 0)
        {
            FileInfo snapshotFileInfo = new FileInfo(SnapshotFilePath);
            ZxSpectrum.LoadRom(snapshotFileInfo);
        }
    }

    private void Update()
    {
        CPU_PC = ZxSpectrum.TheCpu.TheRegisters.PC;
    }

    private void OnDestroy()
    {
        ZxSpectrum.Dispose();
    }
}
