// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgi1_4.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop.Windows;

namespace TerraFX.Interop.DirectX
{
    [Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
    [NativeTypeName("struct IDXGIFactory4 : IDXGIFactory3")]
    [NativeInheritance("IDXGIFactory3")]
    internal unsafe partial struct IDXGIFactory4 : IDXGIFactory4.Interface
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(0)]
        public HRESULT QueryInterface([NativeTypeName("const IID &")] Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int>)(lpVtbl[0]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(1)]
        [return: NativeTypeName("ULONG")]
        public uint AddRef()
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint>)(lpVtbl[1]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(2)]
        [return: NativeTypeName("ULONG")]
        public uint Release()
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint>)(lpVtbl[2]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(3)]
        public HRESULT SetPrivateData([NativeTypeName("const GUID &")] Guid* Name, uint DataSize, [NativeTypeName("const void *")] void* pData)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, uint, void*, int>)(lpVtbl[3]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Name, DataSize, pData);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(4)]
        public HRESULT SetPrivateDataInterface([NativeTypeName("const GUID &")] Guid* Name, [NativeTypeName("const IUnknown *")] IUnknown* pUnknown)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, IUnknown*, int>)(lpVtbl[4]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Name, pUnknown);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(5)]
        public HRESULT GetPrivateData([NativeTypeName("const GUID &")] Guid* Name, uint* pDataSize, void* pData)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, uint*, void*, int>)(lpVtbl[5]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Name, pDataSize, pData);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(6)]
        public HRESULT GetParent([NativeTypeName("const IID &")] Guid* riid, void** ppParent)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int>)(lpVtbl[6]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), riid, ppParent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(7)]
        public HRESULT EnumAdapters(uint Adapter, IDXGIAdapter** ppAdapter)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, IDXGIAdapter**, int>)(lpVtbl[7]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Adapter, ppAdapter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(8)]
        public HRESULT MakeWindowAssociation(HWND WindowHandle, uint Flags)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, int>)(lpVtbl[8]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), WindowHandle, Flags);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(9)]
        public HRESULT GetWindowAssociation(HWND* pWindowHandle)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND*, int>)(lpVtbl[9]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pWindowHandle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(10)]
        public HRESULT CreateSwapChain(IUnknown* pDevice, DXGI_SWAP_CHAIN_DESC* pDesc, IDXGISwapChain** ppSwapChain)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, DXGI_SWAP_CHAIN_DESC*, IDXGISwapChain**, int>)(lpVtbl[10]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pDevice, pDesc, ppSwapChain);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(11)]
        public HRESULT CreateSoftwareAdapter(HMODULE Module, IDXGIAdapter** ppAdapter)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HMODULE, IDXGIAdapter**, int>)(lpVtbl[11]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Module, ppAdapter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(12)]
        public HRESULT EnumAdapters1(uint Adapter, IDXGIAdapter1** ppAdapter)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, IDXGIAdapter1**, int>)(lpVtbl[12]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Adapter, ppAdapter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(13)]
        public BOOL IsCurrent()
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, int>)(lpVtbl[13]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(14)]
        public BOOL IsWindowedStereoEnabled()
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, int>)(lpVtbl[14]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(15)]
        public HRESULT CreateSwapChainForHwnd(IUnknown* pDevice, HWND hWnd, [NativeTypeName("const DXGI_SWAP_CHAIN_DESC1 *")] DXGI_SWAP_CHAIN_DESC1* pDesc, [NativeTypeName("const DXGI_SWAP_CHAIN_FULLSCREEN_DESC *")] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pFullscreenDesc, IDXGIOutput* pRestrictToOutput, IDXGISwapChain1** ppSwapChain)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, HWND, DXGI_SWAP_CHAIN_DESC1*, DXGI_SWAP_CHAIN_FULLSCREEN_DESC*, IDXGIOutput*, IDXGISwapChain1**, int>)(lpVtbl[15]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pDevice, hWnd, pDesc, pFullscreenDesc, pRestrictToOutput, ppSwapChain);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(16)]
        public HRESULT CreateSwapChainForCoreWindow(IUnknown* pDevice, IUnknown* pWindow, [NativeTypeName("const DXGI_SWAP_CHAIN_DESC1 *")] DXGI_SWAP_CHAIN_DESC1* pDesc, IDXGIOutput* pRestrictToOutput, IDXGISwapChain1** ppSwapChain)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, IUnknown*, DXGI_SWAP_CHAIN_DESC1*, IDXGIOutput*, IDXGISwapChain1**, int>)(lpVtbl[16]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pDevice, pWindow, pDesc, pRestrictToOutput, ppSwapChain);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(17)]
        public HRESULT GetSharedResourceAdapterLuid(HANDLE hResource, LUID* pLuid)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, LUID*, int>)(lpVtbl[17]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), hResource, pLuid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(18)]
        public HRESULT RegisterStereoStatusWindow(HWND WindowHandle, uint wMsg, [NativeTypeName("DWORD *")] uint* pdwCookie)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, uint*, int>)(lpVtbl[18]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), WindowHandle, wMsg, pdwCookie);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(19)]
        public HRESULT RegisterStereoStatusEvent(HANDLE hEvent, [NativeTypeName("DWORD *")] uint* pdwCookie)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, uint*, int>)(lpVtbl[19]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), hEvent, pdwCookie);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(20)]
        public void UnregisterStereoStatus([NativeTypeName("DWORD")] uint dwCookie)
        {
            ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, void>)(lpVtbl[20]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), dwCookie);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(21)]
        public HRESULT RegisterOcclusionStatusWindow(HWND WindowHandle, uint wMsg, [NativeTypeName("DWORD *")] uint* pdwCookie)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, uint*, int>)(lpVtbl[21]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), WindowHandle, wMsg, pdwCookie);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(22)]
        public HRESULT RegisterOcclusionStatusEvent(HANDLE hEvent, [NativeTypeName("DWORD *")] uint* pdwCookie)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, uint*, int>)(lpVtbl[22]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), hEvent, pdwCookie);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(23)]
        public void UnregisterOcclusionStatus([NativeTypeName("DWORD")] uint dwCookie)
        {
            ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, void>)(lpVtbl[23]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), dwCookie);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(24)]
        public HRESULT CreateSwapChainForComposition(IUnknown* pDevice, [NativeTypeName("const DXGI_SWAP_CHAIN_DESC1 *")] DXGI_SWAP_CHAIN_DESC1* pDesc, IDXGIOutput* pRestrictToOutput, IDXGISwapChain1** ppSwapChain)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, DXGI_SWAP_CHAIN_DESC1*, IDXGIOutput*, IDXGISwapChain1**, int>)(lpVtbl[24]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pDevice, pDesc, pRestrictToOutput, ppSwapChain);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(25)]
        public uint GetCreationFlags()
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint>)(lpVtbl[25]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(26)]
        public HRESULT EnumAdapterByLuid(LUID AdapterLuid, [NativeTypeName("const IID &")] Guid* riid, void** ppvAdapter)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, LUID, Guid*, void**, int>)(lpVtbl[26]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), AdapterLuid, riid, ppvAdapter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [VtblIndex(27)]
        public HRESULT EnumWarpAdapter([NativeTypeName("const IID &")] Guid* riid, void** ppvAdapter)
        {
            return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int>)(lpVtbl[27]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), riid, ppvAdapter);
        }

        public interface Interface : IDXGIFactory3.Interface
        {
            [VtblIndex(26)]
            HRESULT EnumAdapterByLuid(LUID AdapterLuid, [NativeTypeName("const IID &")] Guid* riid, void** ppvAdapter);

            [VtblIndex(27)]
            HRESULT EnumWarpAdapter([NativeTypeName("const IID &")] Guid* riid, void** ppvAdapter);
        }

        internal partial struct Vtbl
        {
            [NativeTypeName("HRESULT (const IID &, void **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int> QueryInterface;

            [NativeTypeName("ULONG () __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint> AddRef;

            [NativeTypeName("ULONG () __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint> Release;

            [NativeTypeName("HRESULT (const GUID &, UINT, const void *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, uint, void*, int> SetPrivateData;

            [NativeTypeName("HRESULT (const GUID &, const IUnknown *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, IUnknown*, int> SetPrivateDataInterface;

            [NativeTypeName("HRESULT (const GUID &, UINT *, void *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, uint*, void*, int> GetPrivateData;

            [NativeTypeName("HRESULT (const IID &, void **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int> GetParent;

            [NativeTypeName("HRESULT (UINT, IDXGIAdapter **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, IDXGIAdapter**, int> EnumAdapters;

            [NativeTypeName("HRESULT (HWND, UINT) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, int> MakeWindowAssociation;

            [NativeTypeName("HRESULT (HWND *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND*, int> GetWindowAssociation;

            [NativeTypeName("HRESULT (IUnknown *, DXGI_SWAP_CHAIN_DESC *, IDXGISwapChain **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, DXGI_SWAP_CHAIN_DESC*, IDXGISwapChain**, int> CreateSwapChain;

            [NativeTypeName("HRESULT (HMODULE, IDXGIAdapter **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HMODULE, IDXGIAdapter**, int> CreateSoftwareAdapter;

            [NativeTypeName("HRESULT (UINT, IDXGIAdapter1 **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, IDXGIAdapter1**, int> EnumAdapters1;

            [NativeTypeName("BOOL () __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, int> IsCurrent;

            [NativeTypeName("BOOL () __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, int> IsWindowedStereoEnabled;

            [NativeTypeName("HRESULT (IUnknown *, HWND, const DXGI_SWAP_CHAIN_DESC1 *, const DXGI_SWAP_CHAIN_FULLSCREEN_DESC *, IDXGIOutput *, IDXGISwapChain1 **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, HWND, DXGI_SWAP_CHAIN_DESC1*, DXGI_SWAP_CHAIN_FULLSCREEN_DESC*, IDXGIOutput*, IDXGISwapChain1**, int> CreateSwapChainForHwnd;

            [NativeTypeName("HRESULT (IUnknown *, IUnknown *, const DXGI_SWAP_CHAIN_DESC1 *, IDXGIOutput *, IDXGISwapChain1 **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, IUnknown*, DXGI_SWAP_CHAIN_DESC1*, IDXGIOutput*, IDXGISwapChain1**, int> CreateSwapChainForCoreWindow;

            [NativeTypeName("HRESULT (HANDLE, LUID *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, LUID*, int> GetSharedResourceAdapterLuid;

            [NativeTypeName("HRESULT (HWND, UINT, DWORD *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, uint*, int> RegisterStereoStatusWindow;

            [NativeTypeName("HRESULT (HANDLE, DWORD *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, uint*, int> RegisterStereoStatusEvent;

            [NativeTypeName("void (DWORD) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, void> UnregisterStereoStatus;

            [NativeTypeName("HRESULT (HWND, UINT, DWORD *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, uint*, int> RegisterOcclusionStatusWindow;

            [NativeTypeName("HRESULT (HANDLE, DWORD *) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, uint*, int> RegisterOcclusionStatusEvent;

            [NativeTypeName("void (DWORD) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, void> UnregisterOcclusionStatus;

            [NativeTypeName("HRESULT (IUnknown *, const DXGI_SWAP_CHAIN_DESC1 *, IDXGIOutput *, IDXGISwapChain1 **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, DXGI_SWAP_CHAIN_DESC1*, IDXGIOutput*, IDXGISwapChain1**, int> CreateSwapChainForComposition;

            [NativeTypeName("UINT () __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint> GetCreationFlags;

            [NativeTypeName("HRESULT (LUID, const IID &, void **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, LUID, Guid*, void**, int> EnumAdapterByLuid;

            [NativeTypeName("HRESULT (const IID &, void **) __attribute__((stdcall))")]
            public delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int> EnumWarpAdapter;
        }
    }
}
