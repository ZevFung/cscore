﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using CSCore.Utils;

namespace CSCore.SoundOut.MMInterop
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    internal static class MMInterops
    {
        public const int MAXERRORTEXTLENGTH = 256;

        [Flags]
        public enum WaveInOutOpenFlags
        {
            CALLBACK_NULL = 0,
            CALLBACK_FUNCTION = 0x30000,
            CALLBACK_EVENT = 0x50000,
            CALLBACK_WINDOW = 0x10000,
            CALLBACK_THREAD = 0x20000,

            WAVE_FORMAT_QUERY = 1,
            WAVE_MAPPED = 4,
            WAVE_FORMAT_DIRECT = 8
        }

        #region WaveOut

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutPrepareHeader(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutWrite(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutUnprepareHeader(IntPtr hWaveOut, WaveHeader lpWaveOutHdr, int uSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutOpen(out IntPtr hWaveOut, IntPtr uDeviceID, WaveFormat lpFormat, WaveCallback dwCallback, IntPtr dwInstance, WaveInOutOpenFlags dwFlags);

        [DllImport("winmm.dll", EntryPoint = "waveOutOpen")]
        public static extern MmResult waveOutOpenWithWindow(out IntPtr hWaveOut, IntPtr uDeviceID, WaveFormat lpFormat, IntPtr window, IntPtr dwInstance, WaveInOutOpenFlags dwFlags);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutReset(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutClose(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutPause(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutRestart(IntPtr hWaveOut);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutSetVolume(IntPtr hWaveOut, uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetVolume(IntPtr hWaveOut, out uint pdwVolume);

        // http: //msdn.microsoft.com/en-us/library/dd743863%28VS.85%29.aspx
        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetPosition(IntPtr hWaveOut, out MMTime mmTime, int uSize);

        [DllImport("winmm.dll")]
        public static extern Int32 waveOutGetNumDevs();

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetDevCaps(uint deviceID, out WaveOutCaps waveOutCaps, uint cbwaveOutCaps);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetPitch(IntPtr hWaveOut, IntPtr pdwPitch);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutSetPitch(IntPtr hWaveOut, int dwPitch);

        //http://msdn.microsoft.com/en-us/library/aa910385.aspx
        [DllImport("winmm.dll")]
        public static extern MmResult waveOutSetPlaybackRate(IntPtr hWaveOut, int dwRate);

        //http://msdn.microsoft.com/en-us/library/aa910193.aspx
        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetPlaybackRate(IntPtr hWaveOut, IntPtr pdwRate);

        [DllImport("winmm.dll")]
        public static extern MmResult waveOutGetErrorText(MmResult mmrError, StringBuilder pszText, uint cchText);

        #endregion WaveOut

        #region waveIn

        [DllImport("winmm.dll")]
        public static extern MmResult waveInOpen(out IntPtr hWaveIn, IntPtr uDeviceID, WaveFormat pwfx, WaveCallback dwCallback, IntPtr dwInstance, WaveInOutOpenFlags fdwOpen);

        [DllImport("winmm.dll", EntryPoint = "waveInOpen")]
        public static extern MmResult waveInOpenWithWindow(out IntPtr hWaveIn, IntPtr uDeviceID, WaveFormat pwfx, IntPtr window, IntPtr dwInstance, WaveInOutOpenFlags fdwOpen);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInPrepareHeader(IntPtr hWaveIn, WaveHeader waveHdr, int headerSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInUnprepareHeader(IntPtr hWaveIn, WaveHeader waveHdr, int headerSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInAddBuffer(IntPtr hWaveIn, WaveHeader waveHdr, int headerSize);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInStart(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInStop(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInReset(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInClose(IntPtr hWaveIn);

        [DllImport("winmm.dll")]
        public static extern Int32 waveInGetNumDevs();

        [DllImport("winmm.dll")]
        public static extern MmResult waveInGetDevCaps(uint deviceID, out CSCore.SoundIn.WaveInCaps waveInCaps, uint cbWaveInCaps);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInGetPosition(IntPtr hWaveIn, out MMTime time, uint cbTime);

        [DllImport("winmm.dll")]
        public static extern MmResult waveInGetErrorText(MmResult mmrError, StringBuilder pszText, int cchText);

        #endregion waveIn

        public static MmResult SetSystemVolume(uint dwVolume)
        {
            return waveOutSetVolume(IntPtr.Zero, dwVolume);
        }

        public static MmResult GetSystemVolume(out uint pdwVolume)
        {
            return waveOutGetVolume(IntPtr.Zero, out pdwVolume);
        }

        public static void SetVolume(IntPtr waveOut, float left, float right)
        {
            uint tmp = (uint)(left * 0xFFFF) + ((uint)(right * 0xFFFF) << 16);
            MmResult result = waveOutSetVolume(waveOut, tmp);
            if (result != MmResult.MMSYSERR_NOERROR)
                MmException.Try(waveOutSetVolume(waveOut, tmp),
                    "waveOutSetVolume");
        }

        public static float GetVolume(IntPtr waveOut)
        {
            uint volume;
            MmResult result = MMInterops.waveOutGetVolume(waveOut, out volume);
            if (result != MmResult.MMSYSERR_NOERROR)
                MmException.Try(result, "waveOutGetVolume");
            uint left, right;
            HightLowConverterUInt32 u = new HightLowConverterUInt32(volume);
            left = u.High;
            right = u.Low;
            return (float)(((right + left) / 2.0) * (1.0 / 0xFFFF));
        }

        public static void GetVolume(IntPtr waveOut, out float left, out float right)
        {
            uint volume;
            MmException.Try(waveOutGetVolume(waveOut, out volume), "waveOutGetVolume");
            left = (float)volume / 0xFFFF;
            right = ((volume / 0xFFFF) << 16);
        }
    }

    /// <summary>
    /// http: //msdn.microsoft.com/en-us/library/dd743869%28VS.85%29.aspx
    /// </summary>
    public delegate void WaveCallback(IntPtr handle, WaveMsg msg, IntPtr user, WaveHeader header, IntPtr reserved);
}