using System.Collections.Immutable;
using ComputeSharp.SourceGeneration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static ComputeSharp.D2D1.WinUI.SourceGenerators.DiagnosticDescriptors;

namespace ComputeSharp.D2D1.WinUI.SourceGenerators;

/// <summary>
/// A diagnostic analyzer that generates errors when a property using [GeneratedCanvasEffectProperty] is in a project with the C# language version not set to preview.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class RequireCSharpLanguageVersionPreviewAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [CSharpLanguageVersionIsNotPreview];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            // If the language version is set to preview, we'll never emit diagnostics
            if (context.Compilation.IsLanguageVersionPreview())
            {
                return;
            }

            // Get the [GeneratedCanvasEffectProperty] symbols
            if (context.Compilation.GetTypeByMetadataName("ComputeSharp.D2D1.WinUI.GeneratedCanvasEffectPropertyAttribute") is not { } generatedCanvasEffectPropertyAttributeSymbol)
            {
                return;
            }

            context.RegisterSymbolAction(context =>
            {
                // We only want to target partial property definitions (also include non-partial ones for diagnostics)
                if (context.Symbol is not IPropertySymbol { PartialDefinitionPart: null })
                {
                    return;
                }

                // If the property is using [GeneratedCanvasEffectProperty], emit the diagnostic
                if (context.Symbol.TryGetAttributeWithType(generatedCanvasEffectPropertyAttributeSymbol, out AttributeData? generatedCanvasEffectPropertyAttribute))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        CSharpLanguageVersionIsNotPreview,
                        generatedCanvasEffectPropertyAttribute.GetLocation(),
                        context.Symbol));
                }
            }, SymbolKind.Property);
        });
    }
}