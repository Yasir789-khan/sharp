﻿namespace ComputeSharp.D2D1Interop.SourceGenerators.Models;

/// <summary>
/// A model for extracted info on a processed HLSL shader.
/// </summary>
/// <param name="HlslSource">The HLSL source.</param>
/// <param name="ShaderProfile">The shader profile to use to compile the shader, if requested.</param>
/// <param name="IsLinkingSupported">Whether the shader supports linking.</param>
internal sealed record HlslShaderSourceInfo(string HlslSource, D2D1ShaderProfile? ShaderProfile, bool IsLinkingSupported);
