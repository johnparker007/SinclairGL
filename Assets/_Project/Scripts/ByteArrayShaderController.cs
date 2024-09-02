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






    public Material Material;
    public string ScreenBytesFilename;

    private ComputeBuffer _byteBuffer = null;
    private int _bufferSize = kScreenTotalDataLength; 

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

        uint[] screenData = new uint[kScreenTotalDataLength];
        for(int byteIndex = 0; byteIndex < kScreenTotalDataLength; ++byteIndex)
        {
            screenData[byteIndex] = fileData[byteIndex];
        }

        _byteBuffer.SetData(screenData);

        // Set the buffer to the shader
        Material.SetBuffer("_ByteBuffer", _byteBuffer);
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
}