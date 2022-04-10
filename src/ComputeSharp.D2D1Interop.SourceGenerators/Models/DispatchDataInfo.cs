﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ComputeSharp.SourceGeneration.Extensions;

namespace ComputeSharp.D2D1Interop.SourceGenerators.Models;

/// <summary>
/// A model representing gathered info on a shader dispatch data.
/// </summary>
/// <param name="FieldInfos">The description on shader instance fields.</param>
/// <param name="Root32BitConstantCount">The size of the shader root signature, in 32 bit constants.</param>
internal sealed record DispatchDataInfo(ImmutableArray<FieldInfo> FieldInfos, int Root32BitConstantCount)
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> implementation for <see cref="DispatchDataInfo"/>.
    /// </summary>
    public sealed class Comparer : IEqualityComparer<DispatchDataInfo>
    {
        /// <summary>
        /// The singleton <see cref="Comparer"/> instance.
        /// </summary>
        public static Comparer Default { get; } = new();

        /// <inheritdoc/>
        public bool Equals(DispatchDataInfo? x, DispatchDataInfo? y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return
                x.FieldInfos.SequenceEqual(y.FieldInfos, FieldInfo.Comparer.Default) &&
                x.Root32BitConstantCount == y.Root32BitConstantCount;
        }

        /// <inheritdoc/>
        public int GetHashCode(DispatchDataInfo obj)
        {
            HashCode hashCode = default;

            hashCode.AddRange(obj.FieldInfos, FieldInfo.Comparer.Default);
            hashCode.Add(obj.Root32BitConstantCount);

            return hashCode.ToHashCode();
        }
    }
}
