﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using ComputeSharp.Graphics.Helpers;
using TerraFX.Interop.Windows;

namespace ComputeSharp.Core.Extensions;

/// <summary>
/// Helper methods to efficiently throw exceptions.
/// </summary>
[DebuggerStepThrough]
internal static class HResultExtensions
{
    /// <summary>
    /// Throws a <see cref="Win32Exception"/> if <paramref name="result"/> represents an error.
    /// </summary>
    /// <param name="result">The input <see cref="HRESULT"/> to check.</param>
    /// <exception cref="Win32Exception">Thrown if <paramref name="result"/> represents an error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(this HRESULT result)
    {
        Assert((int)result);
    }

    /// <summary>
    /// Throws a <see cref="Win32Exception"/> if <paramref name="result"/> represents an error.
    /// </summary>
    /// <param name="result">The input <see cref="int"/> to check.</param>
    /// <exception cref="Win32Exception">Thrown if <paramref name="result"/> represents an error.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Assert(this int result)
    {
        // This forward branch is predicted taken by the JIT, and when tier-1 JIT kicks in
        // it will just be removed if the IsDebugOutputEnabled flag is not set. The resulting
        // single branch can then be inlined without bloating the code size in the caller.
        if (!Configuration.IsDebugOutputEnabled)
        {
            if (result < 0)
            {
                // This method just throws the exception from the helper below, which includes the
                // additional logic for the device lost events. This structure ensures this method
                // only has a single basic block, so it can be identified as a throw helper and
                // imported properly, which results in better codegen.
                static void RaiseDeviceLostEventsAndThrowWin32Exception(int result)
                {
                    throw RaiseDeviceLostEventsAndGetWin32Exception(result);
                }

                // Raises all device lost events, if needed, and then creates the Win32Exception to throw.
                // This method is never inlined, which is needed to make handling the throw helper easier.
                [MethodImpl(MethodImplOptions.NoInlining)]
                static Win32Exception RaiseDeviceLostEventsAndGetWin32Exception(int result)
                {
                    DeviceHelper.RaiseAllDeviceLostEventsIfNeeded();

                    return new Win32Exception(result);
                }

                RaiseDeviceLostEventsAndThrowWin32Exception(result);
            }
        }
        else
        {
            // Move the extended debug logic into a non inlineable method to help the inliner in the standard case
            [MethodImpl(MethodImplOptions.NoInlining)]
            static void AssertWithDebugInfo(int result)
            {
                bool hasErrorsOrWarnings = DeviceHelper.FlushAllID3D12InfoQueueMessagesAndCheckForErrorsOrWarnings();

                if (result < 0)
                {
                    ThrowHelper.ThrowWin32Exception(result);
                }

                if (hasErrorsOrWarnings)
                {
                    ThrowHelper.ThrowWin32Exception("Warning or error detected by ID3D12InfoQueue.");
                }
            }

            AssertWithDebugInfo(result);
        }
    }
}
