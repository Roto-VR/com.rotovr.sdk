using System.Runtime.InteropServices;

namespace RotoVR.Communication.USB;

static class UsbNative
{
    [DllImport("kernel32.dll")]
    internal static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [DllImport("kernel32.dll")]
    internal static extern bool FreeLibrary(IntPtr hModule);
    
    [DllImport("kernel32.dll")]
    internal static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead,
        out uint lpNumberOfBytesRead, [In] ref NativeOverlapped lpOverlapped);

    [DllImport("kernel32.dll")]
    internal static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten, [In] ref NativeOverlapped lpOverlapped);
    
    [DllImport("HIDApi.dll")]
    internal static extern IntPtr OpenFirstHIDDevice(ushort vid, ushort pid, ushort usagePage = 0, ushort usage = 0,
        bool sync = true);

    [DllImport("HIDApi.dll")]
    internal static extern void CloseHIDDevice(IntPtr device);

    [DllImport("HIDApi.dll")]
    internal static extern bool SetFeature(IntPtr device, byte[] pData, ushort length);

    [DllImport("HIDApi.dll")]
    internal static extern bool GetFeature(IntPtr device, byte[] pData, ushort length);
    
    internal static bool ReadFile(IntPtr handle, out byte[] data, int length)
    {
        data = new byte[length];
        bool success = false;
        IntPtr nonManagedBuffer = Marshal.AllocHGlobal(data.Length);
        uint bytesRead;
        try
        {
            var overlapped = new NativeOverlapped();

            var result = ReadFile(handle, nonManagedBuffer, (ushort)data.Length, out bytesRead, ref overlapped);

            if (result)
            {
                Marshal.Copy(nonManagedBuffer, data, 0, (int)bytesRead);
                success = true;
            }
        }
        catch
        {
            success = false;
        }
        finally
        {
            Marshal.FreeHGlobal(nonManagedBuffer);
        }

        return success;
    }

    internal static bool WriteFile(IntPtr handle, byte[] data)
    {
        uint bytesWritten;
        var overlapped = new NativeOverlapped();
        var result = WriteFile(handle, data, (uint)data.Length, out bytesWritten, ref overlapped);
        return result;
    }
}