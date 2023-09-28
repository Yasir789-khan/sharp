using System.Collections.Generic;
using System.Linq;
using ComputeSharp.SourceGeneration.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ComputeSharp.D2D1.SourceGenerators;

/// <inheritdoc/>
partial class ID2D1ShaderGenerator
{
    /// <summary>
    /// Gets the shader type for a given shader, if any.
    /// </summary>
    /// <param name="typeSymbol">The input <see cref="INamedTypeSymbol"/> instance to check.</param>
    /// <param name="compilation">The <see cref="Compilation"/> instance currently in use.</param>
    /// <returns>Whether or not <paramref name="typeSymbol"/> is a D2D1 interface type.</returns>
    public static bool IsD2D1PixelShaderType(INamedTypeSymbol typeSymbol, Compilation compilation)
    {
        foreach (INamedTypeSymbol interfaceSymbol in typeSymbol.AllInterfaces)
        {
            if (interfaceSymbol.Name == nameof(ID2D1PixelShader))
            {
                INamedTypeSymbol d2D1PixelShaderSymbol = compilation.GetTypeByMetadataName("ComputeSharp.D2D1.ID2D1PixelShader")!;

                if (SymbolEqualityComparer.Default.Equals(interfaceSymbol, d2D1PixelShaderSymbol))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Creates a <see cref="CompilationUnitSyntax"/> instance wrapping the given method.
    /// </summary>
    /// <param name="hierarchyInfo">The <see cref="HierarchyInfo"/> instance for the current type.</param>
    /// <param name="memberDeclarations">The <see cref="MethodDeclarationSyntax"/> items to insert.</param>
    /// <param name="additionalMemberDeclarations">Additional member declarations to also emit, if any.</param>
    /// <returns>A <see cref="CompilationUnitSyntax"/> object wrapping <paramref name="memberDeclarations"/>.</returns>
    private static CompilationUnitSyntax GetCompilationUnitFromMembers(
        HierarchyInfo hierarchyInfo,
        (MemberDeclarationSyntax Member, bool SkipLocalsInit)[] memberDeclarations,
        params MemberDeclarationSyntax[] additionalMemberDeclarations)
    {
        // Method attributes
        AttributeListSyntax[] attributes =
        {
            AttributeList(SingletonSeparatedList(
                Attribute(IdentifierName("global::System.CodeDom.Compiler.GeneratedCode")).AddArgumentListArguments(
                    AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(typeof(ID2D1ShaderGenerator).FullName))),
                    AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(typeof(ID2D1ShaderGenerator).Assembly.GetName().Version.ToString())))))),
            AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.DebuggerNonUserCode")))),
            AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage")))),
            AttributeList(SingletonSeparatedList(
                Attribute(IdentifierName("global::System.ComponentModel.EditorBrowsable")).AddArgumentListArguments(
                AttributeArgument(ParseExpression("global::System.ComponentModel.EditorBrowsableState.Never"))))),
            AttributeList(SingletonSeparatedList(
                Attribute(IdentifierName("global::System.Obsolete")).AddArgumentListArguments(
                AttributeArgument(LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("This method is not intended to be used directly by user code"))))))
        };

        MemberDeclarationSyntax[] membersToAdd = memberDeclarations.Select(item =>
        {
            MemberDeclarationSyntax memberToAdd = item.Member.AddAttributeLists(attributes);

            // Add [SkipLocalsInit] if needed
            if (item.SkipLocalsInit)
            {
                memberToAdd = memberToAdd.AddAttributeLists(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Runtime.CompilerServices.SkipLocalsInit")))));
            }

            return memberToAdd;
        }).ToArray();

        return hierarchyInfo.GetSyntax(membersToAdd, additionalMemberDeclarations);
    }

    /// <summary>
    /// Creates a <see cref="CompilationUnitSyntax"/> instance wrapping the given method.
    /// </summary>
    /// <param name="hierarchyInfo">The <see cref="HierarchyInfo"/> instance for the current type.</param>
    /// <param name="memberDeclaration">The <see cref="MemberDeclarationSyntax"/> item to insert.</param>
    /// <param name="skipLocalsInit">Whether <c>[SkipLocalsInit]</c> should also be generated.</param>
    /// <param name="additionalMemberDeclarations">Additional member declarations to also emit, if any.</param>
    /// <returns>A <see cref="CompilationUnitSyntax"/> object wrapping <paramref name="memberDeclaration"/>.</returns>
    private static CompilationUnitSyntax GetCompilationUnitFromMember(
        HierarchyInfo hierarchyInfo,
        MemberDeclarationSyntax memberDeclaration,
        bool skipLocalsInit,
        params MemberDeclarationSyntax[] additionalMemberDeclarations)
    {
        // Method attributes
        List<AttributeListSyntax> attributes = new()
        {
            AttributeList(SingletonSeparatedList(
                Attribute(IdentifierName("global::System.CodeDom.Compiler.GeneratedCode")).AddArgumentListArguments(
                    AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(typeof(ID2D1ShaderGenerator).FullName))),
                    AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(typeof(ID2D1ShaderGenerator).Assembly.GetName().Version.ToString())))))),
            AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.DebuggerNonUserCode")))),
            AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage")))),
            AttributeList(SingletonSeparatedList(
                Attribute(IdentifierName("global::System.ComponentModel.EditorBrowsable")).AddArgumentListArguments(
                AttributeArgument(ParseExpression("global::System.ComponentModel.EditorBrowsableState.Never"))))),
            AttributeList(SingletonSeparatedList(
                Attribute(IdentifierName("global::System.Obsolete")).AddArgumentListArguments(
                AttributeArgument(LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("This method is not intended to be used directly by user code"))))))
        };

        // Add [SkipLocalsInit] if needed
        if (skipLocalsInit)
        {
            attributes.Add(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Runtime.CompilerServices.SkipLocalsInit")))));
        }

        return hierarchyInfo.GetSyntax(new MemberDeclarationSyntax[] { memberDeclaration.AddAttributeLists(attributes.ToArray()) }, additionalMemberDeclarations);
    }
}