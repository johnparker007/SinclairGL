using System;
using System.IO;
using UnityEngine;

public class ByteArrayShaderController : MonoBehaviour
{
    // screen
    public const int kScreenWidthPixels = 256;
    public const int kScreenHeightPixels = 192;

    public const int kScreenWidthCharacters = kScreenWidthPixels / 8;
    public const int kScreenHeightCharacters = kScreenHeightPixels / 8;

    public const int kScreenPixelDataLength = kScreenWidthCharacters * kScreenHeightPixels;

    public const int kScreenAttributeDataLength = kScreenWidthCharacters * kScreenHeightCharacters;

    public const int kScreenTotalDataLength = kScreenPixelDataLength + kScreenAttributeDataLength;

    // memory
    public const int kMemoryRomLength = 16384;
    public const int kMemoryRamLength = 49152;
    public const int kMemoryTotalLength = kMemoryRomLength + kMemoryRamLength;

    public const int kMemoryScreenPixelsStart = kMemoryRomLength;
    public const int kMemoryScreenAttributesStart = kMemoryScreenPixelsStart + kScreenPixelDataLength;


    public enum EmulationModeType
    {
        PortedCSharp,
        RefactoredCSharp,
        Shader
    }



    public Material Material;
    public string ScreenBytesFilename;
    public EmulationModeType EmulationMode;

    public EmulatorController EmulatorController;

    private ComputeBuffer _byteBuffer = null;
    private int _bufferSize = kMemoryTotalLength / 4; 

    private void Start()
    {
        // Create and initialize the compute buffer
        _byteBuffer = new ComputeBuffer(_bufferSize, sizeof(uint));

        // Example: Fill the buffer with some data
        //uint[] data = new uint[_bufferSize];
        //for (int i = 0; i < _bufferSize; i++)
        //{
        //    data[i] = (uint)Random.Range(0, 256);
        //}
        //_byteBuffer.SetData(data);


        // Load the binary file into a byte array
        string filePath = Path.Combine("Snapshots", ScreenBytesFilename);
        byte[] fileData = LoadBinaryFileFromResources(filePath);

        if (fileData != null)
        {
            Debug.Log("File loaded successfully. File size: " + fileData.Length + " bytes.");
            // You can now work with the byte array (fileData)
        }
        else
        {
            Debug.LogError("Failed to load file.");
            return;
        }

        uint[] memoryData = new uint[kMemoryTotalLength / 4];

        WriteBytesToUIntArray(fileData, memoryData, kMemoryScreenPixelsStart);

        _byteBuffer.SetData(memoryData);

        // Set the buffer to the shader
        Material.SetBuffer("_ByteBuffer", _byteBuffer);
    }

    private void Update()
    {
        // TODO don't new this each time
        uint[] memoryData = new uint[kMemoryTotalLength / 4];
            
        WriteBytesToUIntArray(
            EmulatorController.ZxSpectrum.TheCpu.MainMemory.Data,
            memoryData, 
            0);

        _byteBuffer.SetData(memoryData);
    }

    private void OnDestroy()
    {
        // Don't forget to release the buffer when done
        if (_byteBuffer != null)
        {
            _byteBuffer.Release();
            _byteBuffer = null;
        }
    }

    // You can update the buffer at runtime if needed
    public void UpdateBuffer(byte[] newData)
    {
        if (newData.Length != _bufferSize)
        {
            Debug.LogError("New data size doesn't match buffer size");
            return;
        }

        uint[] uintData = new uint[_bufferSize];
        for (int i = 0; i < _bufferSize; i++)
        {
            uintData[i] = newData[i];
        }

        _byteBuffer.SetData(uintData);
    }

    private byte[] LoadBinaryFileFromResources(string filePath)
    {
        // Load the binary file as a TextAsset
        TextAsset binaryFile = Resources.Load<TextAsset>(filePath);

        if (binaryFile != null)
        {
            // Convert the TextAsset's bytes to a byte array
            return binaryFile.bytes;
        }
        else
        {
            Debug.LogError("Could not find file at path: " + filePath);
            return null;
        }
    }

    public static void WriteBytesToUIntArray(byte[] source, uint[] target, int offset)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (offset < 0 || offset >= target.Length * 4) throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of range");

        int byteIndex = 0;
        int targetIndex = offset / 4;
        int byteOffset = offset % 4;

        while (byteIndex < source.Length && targetIndex < target.Length)
        {
            // Get current uint and split it into bytes
            byte[] currentUintBytes = BitConverter.GetBytes(target[targetIndex]);

            // Replace the necessary bytes starting from byteOffset
            for (int i = byteOffset; i < 4 && byteIndex < source.Length; i++, byteIndex++)
            {
                currentUintBytes[i] = source[byteIndex];
            }

            // Combine bytes back to uint and store it in the target array
            target[targetIndex] = BitConverter.ToUInt32(currentUintBytes, 0);

            // Move to the next uint in the target array
            targetIndex++;
            byteOffset = 0;  // Reset byteOffset since we are now starting at the beginning of the next uint
        }
    }
}