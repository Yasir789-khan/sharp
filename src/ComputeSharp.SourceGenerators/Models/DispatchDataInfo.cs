﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ComputeSharp.SourceGeneration.Extensions;
using ComputeSharp.SourceGeneration.Helpers;
using ComputeSharp.SourceGeneration.Models;

namespace ComputeSharp.SourceGenerators.Models;

/// <summary>
/// A model representing gathered info on a shader dispatch data.
/// </summary>
/// <param name="Hierarchy">The hierarchy data for the shader.</param>
/// <param name="Type">The shader interface type.</param>
/// <param name="FieldInfos">The description on shader instance fields.</param>
/// <param name="ResourceCount">The total number of captured resources.</param>
/// <param name="Root32BitConstantCount">The size of the shader root signature, in 32 bit constants.</param>
internal sealed record DispatchDataInfo(
    HierarchyInfo Hierarchy,
    Type Type,
    ImmutableArray<FieldInfo> FieldInfos,
    int ResourceCount,
    int Root32BitConstantCount)
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> implementation for <see cref="DispatchDataInfo"/>.
    /// </summary>
    public sealed class Comparer : Comparer<DispatchDataInfo, Comparer>
    {
        /// <inheritdoc/>
        protected override void AddToHashCode(ref HashCode hashCode, DispatchDataInfo obj)
        {
            hashCode.Add(HierarchyInfo.Comparer.Default.GetHashCode(obj.Hierarchy));
            hashCode.Add(obj.Type);
            hashCode.AddRange(obj.FieldInfos, FieldInfo.Comparer.Default);
            hashCode.Add(obj.ResourceCount);
            hashCode.Add(obj.Root32BitConstantCount);
        }

        /// <inheritdoc/>
        protected override bool AreEqual(DispatchDataInfo x, DispatchDataInfo y)
        {
            return
                HierarchyInfo.Comparer.Default.Equals(x.Hierarchy, y.Hierarchy) &&
                x.Type == y.Type &&
                x.FieldInfos.SequenceEqual(y.FieldInfos, FieldInfo.Comparer.Default) &&
                x.ResourceCount == y.ResourceCount &&
                x.Root32BitConstantCount == y.Root32BitConstantCount;
        }
    }
}
