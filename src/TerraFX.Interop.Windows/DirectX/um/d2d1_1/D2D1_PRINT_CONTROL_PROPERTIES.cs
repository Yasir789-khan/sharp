// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um/d2d1_1.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DirectX
{
    internal partial struct D2D1_PRINT_CONTROL_PROPERTIES
    {
        public D2D1_PRINT_FONT_SUBSET_MODE fontSubset;

        public float rasterDPI;

        public D2D1_COLOR_SPACE colorSpace;
    }
}
