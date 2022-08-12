﻿using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
#if !NET6_0_OR_GREATER
using UnmanagedCallersOnlyAttribute = ComputeSharp.NetStandard.System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute;
#endif

namespace ComputeSharp.D2D1.Shaders.Interop.Effects.ResourceManagers;

/// <inheritdoc/>
partial struct ResourceTextureManager
{
    /// <summary>
    /// The implementation for <c>ID2D1ResourceTextureManager</c>.
    /// </summary>
    private static unsafe class ID2D1ResourceTextureManagerMethods
    {
#if !NET6_0_OR_GREATER
        /// <inheritdoc cref="Initialize"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int InitializeDelegate(
            ResourceTextureManager* @this,
            Guid* resourceId,
            D2D1_RESOURCE_TEXTURE_PROPERTIES* resourceTextureProperties,
            byte* data,
            uint* strides,
            uint dataSize);

        /// <inheritdoc cref="Update"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int UpdateDelegate(
            ResourceTextureManager* @this,
            uint* minimumExtents,
            uint* maximimumExtents,
            uint* strides,
            uint dimensions,
            byte* data,
            uint dataCount);

        /// <summary>
        /// A cached <see cref="QueryInterfaceDelegate"/> instance wrapping <see cref="QueryInterface"/>.
        /// </summary>
        public static readonly QueryInterfaceDelegate QueryInterfaceWrapper = QueryInterface;

        /// <summary>
        /// A cached <see cref="AddRefDelegate"/> instance wrapping <see cref="AddRef"/>.
        /// </summary>
        public static readonly AddRefDelegate AddRefWrapper = AddRef;

        /// <summary>
        /// A cached <see cref="ReleaseDelegate"/> instance wrapping <see cref="Release"/>.
        /// </summary>
        public static readonly ReleaseDelegate ReleaseWrapper = Release;

        /// <summary>
        /// A cached <see cref="InitializeDelegate"/> instance wrapping <see cref="Initialize"/>.
        /// </summary>
        public static readonly InitializeDelegate InitializeWrapper = Initialize;

        /// <summary>
        /// A cached <see cref="UpdateDelegate"/> instance wrapping <see cref="Update"/>.
        /// </summary>
        public static readonly UpdateDelegate UpdateWrapper = Update;
#endif

        /// <inheritdoc cref="ResourceTextureManager.QueryInterface"/>
        [UnmanagedCallersOnly]
        public static int QueryInterface(ResourceTextureManager* @this, Guid* riid, void** ppvObject)
        {
            return @this->QueryInterface(riid, ppvObject);
        }

        /// <inheritdoc cref="ResourceTextureManager.AddRef"/>
        [UnmanagedCallersOnly]
        public static uint AddRef(ResourceTextureManager* @this)
        {
            return @this->AddRef();
        }

        /// <inheritdoc cref="ResourceTextureManager.Release"/>
        [UnmanagedCallersOnly]
        public static uint Release(ResourceTextureManager* @this)
        {
            return @this->Release();
        }

        /// <inheritdoc cref="ID2D1ResourceTextureManager.Initialize"/>
        [UnmanagedCallersOnly]
        public static int Initialize(
            ResourceTextureManager* @this,
            Guid* resourceId,
            D2D1_RESOURCE_TEXTURE_PROPERTIES* resourceTextureProperties,
            byte* data,
            uint* strides,
            uint dataSize)
        {
            // Return E_POINTER if any input pointer is null
            if (resourceTextureProperties is null ||
                data is null ||
                resourceTextureProperties->extents is null ||
                resourceTextureProperties->extendModes is null)
            {
                return E.E_POINTER;
            }

            lock (@this->lockHandle.Target!)
            {
                // If the method has already been called, fail
                if (@this->d2D1ResourceTexture is not null ||
                    @this->data is not null)
                {
                    return E.E_NOT_VALID_STATE;
                }

                // If the method has already been called, just forward the call
                if (@this->d2D1EffectContext is not null)
                {
                    return @this->d2D1EffectContext->CreateResourceTexture(
                        resourceId: resourceId,
                        resourceTextureProperties: resourceTextureProperties,
                        data: data,
                        strides: strides,
                        dataSize: dataSize,
                        resourceTexture: &@this->d2D1ResourceTexture);
                }

                // Initialize into a staging buffer
                return InitializeWithStagingBuffer(
                    @this: @this,
                    resourceId: resourceId,
                    resourceTextureProperties: resourceTextureProperties,
                    data: data,
                    strides: strides,
                    dataSize: dataSize);
            }
        }

        /// <inheritdoc cref="ID2D1ResourceTextureManager.Update"/>
        [UnmanagedCallersOnly]
        public static int Update(
            ResourceTextureManager* @this,
            uint* minimumExtents,
            uint* maximimumExtents,
            uint* strides,
            uint dimensions,
            byte* data,
            uint dataCount)
        {
            if (minimumExtents is null ||
                maximimumExtents is null ||
                data is null)
            {
                return E.E_POINTER;
            }

            lock (@this->lockHandle.Target!)
            {
                if (@this->d2D1ResourceTexture is null &&
                    @this->data is null)
                {
                    return E.E_NOT_VALID_STATE;
                }

                // If a resource texture already exists, just forward the call
                if (@this->d2D1ResourceTexture is not null)
                {
                    return @this->d2D1ResourceTexture->Update(
                        minimumExtents: minimumExtents,
                        maximimumExtents: maximimumExtents,
                        strides: strides,
                        dimensions: dimensions,
                        data: data,
                        dataCount: dataCount);
                }

                // Otherwise update the staging buffer
                return UpdateWithStagingBuffer(
                    @this: @this,
                    minimumExtents: minimumExtents,
                    maximimumExtents: maximimumExtents,
                    strides: strides,
                    dimensions: dimensions,
                    data: data,
                    dataCount: dataCount);
            }
        }

        /// <inheritdoc cref="Initialize"/>
        private static int InitializeWithStagingBuffer(
            ResourceTextureManager* @this,
            Guid* resourceId,
            D2D1_RESOURCE_TEXTURE_PROPERTIES* resourceTextureProperties,
            byte* data,
            uint* strides,
            uint dataSize)
        {
            // Allocate a buffer for the resource id, if there is one
            if (resourceId is not null)
            {
                try
                {
                    @this->resourceId = (Guid*)NativeMemory.Alloc((nuint)sizeof(Guid));
                }
                catch (OutOfMemoryException)
                {
                    return E.E_OUTOFMEMORY;
                }

                *@this->resourceId = *resourceId;
            }

            uint* extents;

            // Allocate a buffer for the extents (one per dimension)
            try
            {
                extents = (uint*)NativeMemory.Alloc(sizeof(uint) * resourceTextureProperties->dimensions);
            }
            catch (OutOfMemoryException)
            {
                return E.E_OUTOFMEMORY;
            }

            D2D1_EXTEND_MODE* extendModes;

            // Allocate a buffer for the extend modes (one per dimension)
            try
            {
                extendModes = (D2D1_EXTEND_MODE*)NativeMemory.Alloc(sizeof(D2D1_EXTEND_MODE) * resourceTextureProperties->dimensions);
            }
            catch (OutOfMemoryException)
            {
                return E.E_OUTOFMEMORY;
            }

            // Copy the extents into the buffer
            Buffer.MemoryCopy(
                source: resourceTextureProperties->extents,
                destination: extents,
                destinationSizeInBytes: sizeof(uint) * resourceTextureProperties->dimensions,
                sourceBytesToCopy: sizeof(uint) * resourceTextureProperties->dimensions);

            // Copy the extend modes into the buffer
            Buffer.MemoryCopy(
                source: resourceTextureProperties->extendModes,
                destination: extendModes,
                destinationSizeInBytes: sizeof(D2D1_EXTEND_MODE) * resourceTextureProperties->dimensions,
                sourceBytesToCopy: sizeof(D2D1_EXTEND_MODE) * resourceTextureProperties->dimensions);

            // Initialize the other resource texture properties
            @this->resourceTextureProperties.extents = extents;
            @this->resourceTextureProperties.dimensions = resourceTextureProperties->dimensions;
            @this->resourceTextureProperties.bufferPrecision = resourceTextureProperties->bufferPrecision;
            @this->resourceTextureProperties.channelDepth = resourceTextureProperties->channelDepth;
            @this->resourceTextureProperties.filter = resourceTextureProperties->filter;
            @this->resourceTextureProperties.extendModes = extendModes;

            // Allocate the data buffer (this is the one most likely to potentially throw)
            try
            {
                @this->data = (byte*)NativeMemory.Alloc(dataSize);
            }
            catch (OutOfMemoryException)
            {
                return E.E_OUTOFMEMORY;
            }

            // Copy the data buffer
            Buffer.MemoryCopy(data, @this->data, dataSize, dataSize);

            @this->dataSize = dataSize;

            // If there are strides (ie. the texture has dimension greater than 1), initialize them as well
            if (strides is not null)
            {
                try
                {
                    @this->strides = (uint*)NativeMemory.Alloc(sizeof(uint) * (resourceTextureProperties->dimensions - 1));
                }
                catch (OutOfMemoryException)
                {
                    return E.E_OUTOFMEMORY;
                }

                Buffer.MemoryCopy(
                    source: strides,
                    destination: @this->strides,
                    destinationSizeInBytes: sizeof(uint) * (resourceTextureProperties->dimensions - 1),
                    sourceBytesToCopy: sizeof(uint) * (resourceTextureProperties->dimensions - 1));
            }

            return S.S_OK;
        }

        /// <inheritdoc cref="ID2D1ResourceTextureManager.Update"/>
        private static int UpdateWithStagingBuffer(
            ResourceTextureManager* @this,
            uint* minimumExtents,
            uint* maximimumExtents,
            uint* strides,
            uint dimensions,
            byte* data,
            uint dataCount)
        {
            return E.E_NOTIMPL;
        }
    }
}
