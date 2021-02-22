﻿using System;
using System.Drawing;
using ComputeSharp.Interop;
using Microsoft.Toolkit.Diagnostics;
using TerraFX.Interop;
using FX = TerraFX.Interop.Windows;

namespace ComputeSharp.Sample.SwapChain
{
    class Program
    {
        static void Main(string[] args)
        {
            Win32ApplicationRunner.Run<FractalTilesApplication>();
        }
    }

    internal sealed class FractalTilesApplication : Win32Application
    {
        /// <summary>
        /// The <see cref="ID3D12Device"/> pointer for the device currently in use.
        /// </summary>
        private ComPtr<ID3D12Device> d3D12Device;

        /// <summary>
        /// The <see cref="ID3D12CommandQueue"/> instance to use for graphics operations.
        /// </summary>
        private ComPtr<ID3D12CommandQueue> d3D12CommandQueue;

        /// <summary>
        /// The <see cref="ID3D12Fence"/> instance used for graphics operations.
        /// </summary>
        private ComPtr<ID3D12Fence> d3D12Fence;

        /// <summary>
        /// The next fence value for graphics operations using <see cref="d3D12CommandQueue"/>.
        /// </summary>
        private ulong nextD3D12FenceValue = 1;

        /// <summary>
        /// The <see cref="ID3D12CommandAllocator"/> object to create command lists.
        /// </summary>
        private ComPtr<ID3D12CommandAllocator> d3D12CommandAllocator;

        /// <summary>
        /// The <see cref="ID3D12GraphicsCommandList"/> instance used to copy data to the back buffers.
        /// </summary>
        private ComPtr<ID3D12GraphicsCommandList> d3D12GraphicsCommandList;

        /// <summary>
        /// The <see cref="IDXGISwapChain1"/> instance used to display content onto the target window.
        /// </summary>
        private ComPtr<IDXGISwapChain1> dxgiSwapChain1;

        /// <summary>
        /// The first buffer within <see cref="dxgiSwapChain1"/>.
        /// </summary>
        private ComPtr<ID3D12Resource> d3D12Resource0;

        /// <summary>
        /// The second buffer within <see cref="dxgiSwapChain1"/>.
        /// </summary>
        private ComPtr<ID3D12Resource> d3D12Resource1;

        /// <summary>
        /// The index of the next buffer that can be used to present content.
        /// </summary>
        private uint currentBufferIndex;

        /// <summary>
        /// The <see cref="ReadWriteTexture2D{T, TPixel}"/> instance used to prepare frames to display.
        /// </summary>
        private ReadWriteTexture2D<Rgba32, Float4> texture = null!;

        public override string Title => "Fractal tiles";

        public override unsafe void OnInitialize(Size size, HWND hwnd)
        {
            // Get the underlying ID3D12Device in use
            fixed (ID3D12Device** d3D12Device = this.d3D12Device)
            {
                _ = InteropServices.TryGetID3D12Device(Gpu.Default, FX.__uuidof<ID3D12Device>(), (void**)d3D12Device);
            }

            // Create the direct command queue to use
            fixed (ID3D12CommandQueue** d3D12CommandQueue = this.d3D12CommandQueue)
            {
                D3D12_COMMAND_QUEUE_DESC d3D12CommandQueueDesc;
                d3D12CommandQueueDesc.Type = D3D12_COMMAND_LIST_TYPE.D3D12_COMMAND_LIST_TYPE_DIRECT;
                d3D12CommandQueueDesc.Priority = (int)D3D12_COMMAND_QUEUE_PRIORITY.D3D12_COMMAND_QUEUE_PRIORITY_NORMAL;
                d3D12CommandQueueDesc.Flags = D3D12_COMMAND_QUEUE_FLAGS.D3D12_COMMAND_QUEUE_FLAG_NONE;
                d3D12CommandQueueDesc.NodeMask = 0;

                _ = d3D12Device.Get()->CreateCommandQueue(
                    &d3D12CommandQueueDesc,
                    FX.__uuidof<ID3D12CommandQueue>(),
                    (void**)d3D12CommandQueue);
            }

            // Create the direct fence
            fixed (ID3D12Fence** d3D12Fence = this.d3D12Fence)
            {
                _ = this.d3D12Device.Get()->CreateFence(
                    0,
                    D3D12_FENCE_FLAGS.D3D12_FENCE_FLAG_NONE,
                    FX.__uuidof<ID3D12Fence>(),
                    (void**)d3D12Fence);
            }

            // Create the swap chain to display frames
            fixed (IDXGISwapChain1** dxgiSwapChain1 = this.dxgiSwapChain1)
            {
                using ComPtr<IDXGIFactory2> dxgiFactory2 = default;

                _ = FX.CreateDXGIFactory2(FX.DXGI_CREATE_FACTORY_DEBUG, FX.__uuidof<IDXGIFactory2>(), (void**)dxgiFactory2.GetAddressOf());

                DXGI_SWAP_CHAIN_DESC1 dxgiSwapChainDesc1 = default;
                dxgiSwapChainDesc1.AlphaMode = DXGI_ALPHA_MODE.DXGI_ALPHA_MODE_IGNORE;
                dxgiSwapChainDesc1.BufferCount = 2;
                dxgiSwapChainDesc1.Flags = 0;
                dxgiSwapChainDesc1.Format = DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;
                dxgiSwapChainDesc1.Width = 0;
                dxgiSwapChainDesc1.Height = 0;
                dxgiSwapChainDesc1.SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0);
                dxgiSwapChainDesc1.Scaling = DXGI_SCALING.DXGI_SCALING_STRETCH;
                dxgiSwapChainDesc1.Stereo = 0;
                dxgiSwapChainDesc1.SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;

                _ = dxgiFactory2.Get()->CreateSwapChainForHwnd(
                    (IUnknown*)d3D12CommandQueue.Get(),
                    hwnd,
                    &dxgiSwapChainDesc1,
                    null,
                    null,
                    dxgiSwapChain1);
            }

            // Retrieve the back buffers for the swap chain
            fixed (ID3D12Resource** d3D12Resource0 = this.d3D12Resource0)
            fixed (ID3D12Resource** d3D12Resource1 = this.d3D12Resource1)
            {
                _ = dxgiSwapChain1.Get()->GetBuffer(0, FX.__uuidof<ID3D12Resource>(), (void**)d3D12Resource0);
                _ = dxgiSwapChain1.Get()->GetBuffer(1, FX.__uuidof<ID3D12Resource>(), (void**)d3D12Resource1);
            }

            // Get the index of the initial back buffer
            using (ComPtr<IDXGISwapChain3> dxgiSwapChain3 = default)
            {
                _ = this.dxgiSwapChain1.CopyTo(dxgiSwapChain3.GetAddressOf());

                this.currentBufferIndex = dxgiSwapChain3.Get()->GetCurrentBackBufferIndex();
            }

            D3D12_RESOURCE_DESC d3D12Resource0Description = this.d3D12Resource0.Get()->GetDesc();

            // Create the command allocator to use
            fixed (ID3D12CommandAllocator** d3D12CommandAllocator = this.d3D12CommandAllocator)
            {
                this.d3D12Device.Get()->CreateCommandAllocator(
                    D3D12_COMMAND_LIST_TYPE.D3D12_COMMAND_LIST_TYPE_DIRECT,
                    FX.__uuidof<ID3D12CommandAllocator>(),
                    (void**)d3D12CommandAllocator);
            }

            // Create the reusable command list to copy data to the back buffers
            fixed (ID3D12GraphicsCommandList** d3D12GraphicsCommandList = this.d3D12GraphicsCommandList)
            {
                this.d3D12Device.Get()->CreateCommandList(
                    0,
                    D3D12_COMMAND_LIST_TYPE.D3D12_COMMAND_LIST_TYPE_DIRECT,
                    d3D12CommandAllocator,
                    null,
                    FX.__uuidof<ID3D12GraphicsCommandList>(),
                    (void**)d3D12GraphicsCommandList);
            }

            // Close the command list to prepare it for future use
            this.d3D12GraphicsCommandList.Get()->Close();

            // Create the 2D texture to use to generate frames to display
            this.texture = Gpu.Default.AllocateReadWriteTexture2D<Rgba32, Float4>(
                (int)d3D12Resource0Description.Width,
                (int)d3D12Resource0Description.Height);
        }

        public override void OnResize(Size size)
        {
        }

        public override unsafe void OnUpdate(TimeSpan time)
        {
            // Generate the new frame
            Gpu.Default.For(texture.Width, texture.Height, new FractalTiling(texture, (float)time.TotalSeconds));

            using ComPtr<ID3D12Resource> d3D12Resource = default;

            // Get the underlying ID3D12Resource pointer for the texture
            _ = InteropServices.TryGetID3D12Resource(texture, FX.__uuidof<ID3D12Resource>(), (void**)d3D12Resource.GetAddressOf());

            // Get the target back buffer to update
            ID3D12Resource* d3D12ResourceBackBuffer = this.currentBufferIndex switch
            {
                0 => this.d3D12Resource0.Get(),
                1 => this.d3D12Resource1.Get(),
                _ => null
            };

            this.currentBufferIndex ^= 1;

            // Reset the command list to reuse
            this.d3D12GraphicsCommandList.Get()->Reset(this.d3D12CommandAllocator.Get(), null);

            D3D12_RESOURCE_BARRIER* d3D12ResourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[]
            {
                D3D12_RESOURCE_BARRIER.InitTransition(
                    d3D12Resource.Get(),
                    D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_UNORDERED_ACCESS,
                    D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_SOURCE),
                D3D12_RESOURCE_BARRIER.InitTransition(
                    d3D12ResourceBackBuffer,
                    D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON,
                    D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST)
            };

            // Transition the resources to COPY_DEST and COPY_SOURCE respectively
            d3D12GraphicsCommandList.Get()->ResourceBarrier(2, d3D12ResourceBarriers);

            // Copy the generated frame to the target back buffer
            d3D12GraphicsCommandList.Get()->CopyResource(d3D12ResourceBackBuffer, d3D12Resource.Get());

            d3D12ResourceBarriers[0] = D3D12_RESOURCE_BARRIER.InitTransition(
                d3D12Resource.Get(),
                D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_SOURCE,
                D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_UNORDERED_ACCESS);

            d3D12ResourceBarriers[1] = D3D12_RESOURCE_BARRIER.InitTransition(
                d3D12ResourceBackBuffer,
                D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST,
                D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON);

            // Transition the resources back to COMMON and UNORDERED_ACCESS respectively
            d3D12GraphicsCommandList.Get()->ResourceBarrier(2, d3D12ResourceBarriers);

            d3D12GraphicsCommandList.Get()->Close();

            // Execute the command list to perform the copy
            this.d3D12CommandQueue.Get()->ExecuteCommandLists(1, (ID3D12CommandList**)d3D12GraphicsCommandList.GetAddressOf());
            this.d3D12CommandQueue.Get()->Signal(this.d3D12Fence.Get(), this.nextD3D12FenceValue);

            if (this.nextD3D12FenceValue > this.d3D12Fence.Get()->GetCompletedValue())
            {
                this.d3D12Fence.Get()->SetEventOnCompletion(this.nextD3D12FenceValue, default);
            }

            this.nextD3D12FenceValue++;

            // Present the new frame
            this.dxgiSwapChain1.Get()->Present(0, 0);
        }

        public override void Dispose()
        {
        }
    }

    [AutoConstructor]
    internal readonly partial struct FractalTiling : IComputeShader
    {
        public readonly IReadWriteTexture2D<Float4> texture;
        public readonly float time;

        /// <inheritdoc/>
        public void Execute()
        {
            Float2 position = ((Float2)(256 * ThreadIds.XY)) / texture.Width + time;
            Float4 color = 0;

            for (int i = 0; i < 6; i++)
            {
                Float2 a = Hlsl.Floor(position);
                Float2 b = Hlsl.Frac(position);
                Float4 w = Hlsl.Frac(
                    (Hlsl.Sin(a.X * 7 + 31.0f * a.Y + 0.01f * time) +
                     new Float4(0.035f, 0.01f, 0, 0.7f))
                     * 13.545317f);

                color.XYZ += w.XYZ *
                       2.0f * Hlsl.SmoothStep(0.45f, 0.55f, w.W) *
                       Hlsl.Sqrt(16.0f * b.X * b.Y * (1.0f - b.X) * (1.0f - b.Y));

                position /= 2.0f;
                color /= 2.0f;
            }

            color.XYZ = Hlsl.Pow(color.XYZ, new Float3(0.7f, 0.8f, 0.5f));

            texture[ThreadIds.XY] = color;
        }
    }
}
