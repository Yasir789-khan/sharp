using System.Collections.Immutable;
using ComputeSharp.SourceGeneration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static ComputeSharp.D2D1.WinUI.SourceGenerators.DiagnosticDescriptors;

namespace ComputeSharp.D2D1.WinUI.SourceGenerators;

/// <summary>
/// A diagnostic analyzer that generates errors when a property using [GeneratedCanvasEffectProperty] has invalid accessors.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class InvalidGeneratedCanvasEffectPropertyDeclarationAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
    [
        InvalidGeneratedCanvasEffectPropertyDeclarationIsStatic,
        InvalidGeneratedCanvasEffectPropertyDeclarationIsNotIncompletePartialDefinition,
        InvalidGeneratedCanvasEffectPropertyDeclarationReturnsByRef,
        InvalidGeneratedCanvasEffectPropertyDeclarationReturnsRefLikeType
    ];

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(static context =>
        {
            // Get the [GeneratedCanvasEffectProperty] symbols
            if (context.Compilation.GetTypeByMetadataName("ComputeSharp.D2D1.WinUI.GeneratedCanvasEffectPropertyAttribute") is not { } generatedCanvasEffectPropertyAttributeSymbol)
            {
                return;
            }

            context.RegisterSymbolAction(context =>
            {
                // Ensure that we have some target property to analyze
                if (context.Symbol is not IPropertySymbol propertySymbol)
                {
                    return;
                }

                // If the property is not using [GeneratedCanvasEffectProperty], there's nothing to do
                if (!context.Symbol.TryGetAttributeWithType(generatedCanvasEffectPropertyAttributeSymbol, out AttributeData? generatedCanvasEffectPropertyAttribute))
                {
                    return;
                }

                // Emit an error if the property is static
                if (propertySymbol.IsStatic)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        InvalidGeneratedCanvasEffectPropertyDeclarationIsStatic,
                        generatedCanvasEffectPropertyAttribute.GetLocation(),
                        context.Symbol));
                }

                // Emit an error if the property is not a partial definition with no implementation
                if (propertySymbol is not { IsPartialDefinition: true, PartialImplementationPart: null })
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        InvalidGeneratedCanvasEffectPropertyDeclarationIsNotIncompletePartialDefinition,
                        generatedCanvasEffectPropertyAttribute.GetLocation(),
                        context.Symbol));
                }

                // Emit an error if the property returns a value by ref
                if (propertySymbol.ReturnsByRef || propertySymbol.ReturnsByRefReadonly)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        InvalidGeneratedCanvasEffectPropertyDeclarationReturnsByRef,
                        generatedCanvasEffectPropertyAttribute.GetLocation(),
                        context.Symbol));
                }

                // Emit an error if the property type is a ref struct
                if (propertySymbol.Type.IsRefLikeType)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        InvalidGeneratedCanvasEffectPropertyDeclarationReturnsRefLikeType,
                        generatedCanvasEffectPropertyAttribute.GetLocation(),
                        context.Symbol));
                }
            }, SymbolKind.Property);
        });
    }
}