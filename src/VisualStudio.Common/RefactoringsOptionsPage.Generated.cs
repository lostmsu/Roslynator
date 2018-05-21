﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// <auto-generated>

using System.Collections.Generic;
using System.ComponentModel;
using Roslynator.CSharp.Refactorings;
using Roslynator.VisualStudio.TypeConverters;

namespace Roslynator.VisualStudio
{
    public partial class RefactoringsOptionsPage
    {
        protected override string DisabledByDefault
        {
            get;
        }

        = $"{RefactoringIdentifiers.AddIdentifierToParameter},{RefactoringIdentifiers.AddIdentifierToVariableDeclaration},{RefactoringIdentifiers.IntroduceConstructor},{RefactoringIdentifiers.RemoveAllDocumentationComments},{RefactoringIdentifiers.ReplaceForEachWithForAndReverseLoop},{RefactoringIdentifiers.ReplaceMethodWithProperty},{RefactoringIdentifiers.UseStringEmptyInsteadOfEmptyStringLiteral}";
        protected override string MaxId
        {
            get;
        }

        = RefactoringIdentifiers.ReplaceForEachWithEnumerator;
        internal static void SetRefactoringsDisabledByDefault(RefactoringSettings settings)
        {
            settings.DisableRefactoring(RefactoringIdentifiers.AddIdentifierToParameter);
            settings.DisableRefactoring(RefactoringIdentifiers.AddIdentifierToVariableDeclaration);
            settings.DisableRefactoring(RefactoringIdentifiers.IntroduceConstructor);
            settings.DisableRefactoring(RefactoringIdentifiers.RemoveAllDocumentationComments);
            settings.DisableRefactoring(RefactoringIdentifiers.ReplaceForEachWithForAndReverseLoop);
            settings.DisableRefactoring(RefactoringIdentifiers.ReplaceMethodWithProperty);
            settings.DisableRefactoring(RefactoringIdentifiers.UseStringEmptyInsteadOfEmptyStringLiteral);
        }

        protected override void Fill(ICollection<BaseModel> refactorings)
        {
            refactorings.Clear();
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddBraces, "Add braces", IsEnabled(RefactoringIdentifiers.AddBraces)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddBracesToIfElse, "Add braces to if-else", IsEnabled(RefactoringIdentifiers.AddBracesToIfElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddBracesToSwitchSection, "Add braces to switch section", IsEnabled(RefactoringIdentifiers.AddBracesToSwitchSection)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddBracesToSwitchSections, "Add braces to switch sections", IsEnabled(RefactoringIdentifiers.AddBracesToSwitchSections)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddCastExpression, "Add cast expression", IsEnabled(RefactoringIdentifiers.AddCastExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddDefaultValueToParameter, "Add default value to parameter", IsEnabled(RefactoringIdentifiers.AddDefaultValueToParameter)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddExceptionToDocumentationComment, "Add exception to documentation comment", IsEnabled(RefactoringIdentifiers.AddExceptionToDocumentationComment)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddIdentifierToVariableDeclaration, "Add identifier to variable declaration", IsEnabled(RefactoringIdentifiers.AddIdentifierToVariableDeclaration)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddParameterNameToArgument, "Add parameter name to argument", IsEnabled(RefactoringIdentifiers.AddParameterNameToArgument)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddIdentifierToParameter, "Add identifier to parameter", IsEnabled(RefactoringIdentifiers.AddIdentifierToParameter)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddUsingDirective, "Add using directive", IsEnabled(RefactoringIdentifiers.AddUsingDirective)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddUsingStaticDirective, "Add using static directive", IsEnabled(RefactoringIdentifiers.AddUsingStaticDirective)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CallConfigureAwait, "Call 'ConfigureAwait(false)'", IsEnabled(RefactoringIdentifiers.CallConfigureAwait)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CallExtensionMethodAsInstanceMethod, "Call extension method as instance method", IsEnabled(RefactoringIdentifiers.CallExtensionMethodAsInstanceMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CallToMethod, "Call 'To...' method (ToString, ToArray, ToList)", IsEnabled(RefactoringIdentifiers.CallToMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ChangeExplicitTypeToVar, "Change explicit type to 'var'", IsEnabled(RefactoringIdentifiers.ChangeExplicitTypeToVar)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ChangeMethodReturnTypeToVoid, "Change method return type to 'void'", IsEnabled(RefactoringIdentifiers.ChangeMethodReturnTypeToVoid)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ChangeVarToExplicitType, "Change 'var' to explicit type", IsEnabled(RefactoringIdentifiers.ChangeVarToExplicitType)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CheckExpressionForNull, "Check expression for null", IsEnabled(RefactoringIdentifiers.CheckExpressionForNull)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CheckParameterForNull, "Check parameter for null", IsEnabled(RefactoringIdentifiers.CheckParameterForNull)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CollapseToInitializer, "Collapse to initalizer", IsEnabled(RefactoringIdentifiers.CollapseToInitializer)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CommentOutMember, "Comment out member", IsEnabled(RefactoringIdentifiers.CommentOutMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CommentOutStatement, "Comment out statement", IsEnabled(RefactoringIdentifiers.CommentOutStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CopyDocumentationCommentFromBaseMember, "Copy documentation comment from base member", IsEnabled(RefactoringIdentifiers.CopyDocumentationCommentFromBaseMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.DuplicateArgument, "Duplicate argument", IsEnabled(RefactoringIdentifiers.DuplicateArgument)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.DuplicateMember, "Duplicate member", IsEnabled(RefactoringIdentifiers.DuplicateMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.DuplicateParameter, "Duplicate parameter", IsEnabled(RefactoringIdentifiers.DuplicateParameter)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.DuplicateStatement, "Duplicate statement", IsEnabled(RefactoringIdentifiers.DuplicateStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandCompoundAssignmentOperator, "Expand compound assignment operator", IsEnabled(RefactoringIdentifiers.ExpandCompoundAssignmentOperator)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandCoalesceExpression, "Expand coalesce expression", IsEnabled(RefactoringIdentifiers.ExpandCoalesceExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandEvent, "Expand event", IsEnabled(RefactoringIdentifiers.ExpandEvent)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandExpressionBody, "Expand expression body", IsEnabled(RefactoringIdentifiers.ExpandExpressionBody)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandInitializer, "Expand initializer", IsEnabled(RefactoringIdentifiers.ExpandInitializer)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandLambdaExpressionBody, "Expand lambda expression body", IsEnabled(RefactoringIdentifiers.ExpandLambdaExpressionBody)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandProperty, "Expand property", IsEnabled(RefactoringIdentifiers.ExpandProperty)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExpandPropertyAndAddBackingField, "Expand property and add backing field", IsEnabled(RefactoringIdentifiers.ExpandPropertyAndAddBackingField)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExtractExpressionFromCondition, "Extract expression from condition", IsEnabled(RefactoringIdentifiers.ExtractExpressionFromCondition)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExtractGenericType, "Extract generic type", IsEnabled(RefactoringIdentifiers.ExtractGenericType)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExtractStatement, "Extract statement(s)", IsEnabled(RefactoringIdentifiers.ExtractStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExtractTypeDeclarationToNewFile, "Extract type declaration to a new file", IsEnabled(RefactoringIdentifiers.ExtractTypeDeclarationToNewFile)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatAccessorBraces, "Format accessor braces", IsEnabled(RefactoringIdentifiers.FormatAccessorBraces)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatArgumentList, "Format argument list", IsEnabled(RefactoringIdentifiers.FormatArgumentList)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatBinaryExpression, "Format binary expression", IsEnabled(RefactoringIdentifiers.FormatBinaryExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatConditionalExpression, "Format conditional expression", IsEnabled(RefactoringIdentifiers.FormatConditionalExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatExpressionChain, "Format expression chain", IsEnabled(RefactoringIdentifiers.FormatExpressionChain)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatInitializer, "Format initializer", IsEnabled(RefactoringIdentifiers.FormatInitializer)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatParameterList, "Format parameter list", IsEnabled(RefactoringIdentifiers.FormatParameterList)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GenerateBaseConstructors, "Generate base constructors", IsEnabled(RefactoringIdentifiers.GenerateBaseConstructors)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GenerateCombinedEnumMember, "Generate combined enum member", IsEnabled(RefactoringIdentifiers.GenerateCombinedEnumMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GenerateEnumMember, "Generate enum member", IsEnabled(RefactoringIdentifiers.GenerateEnumMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GenerateEnumValues, "Generate enum values", IsEnabled(RefactoringIdentifiers.GenerateEnumValues)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GenerateEventInvokingMethod, "Generate event invoking method", IsEnabled(RefactoringIdentifiers.GenerateEventInvokingMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GenerateSwitchSections, "Generate switch sections", IsEnabled(RefactoringIdentifiers.GenerateSwitchSections)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InitializeLocalWithDefaultValue, "Initialize local with default value", IsEnabled(RefactoringIdentifiers.InitializeLocalWithDefaultValue)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InlineAliasExpression, "Inline alias expression", IsEnabled(RefactoringIdentifiers.InlineAliasExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InlineMethod, "Inline method", IsEnabled(RefactoringIdentifiers.InlineMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InsertStringInterpolation, "Insert string interpolation", IsEnabled(RefactoringIdentifiers.InsertStringInterpolation)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.IntroduceAndInitializeField, "Introduce and initialize field", IsEnabled(RefactoringIdentifiers.IntroduceAndInitializeField)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.IntroduceAndInitializeProperty, "Introduce and initialize property", IsEnabled(RefactoringIdentifiers.IntroduceAndInitializeProperty)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.IntroduceConstructor, "Introduce constructor", IsEnabled(RefactoringIdentifiers.IntroduceConstructor)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.IntroduceFieldToLockOn, "Introduce field to lock on", IsEnabled(RefactoringIdentifiers.IntroduceFieldToLockOn)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.IntroduceLocalVariable, "Introduce local variable", IsEnabled(RefactoringIdentifiers.IntroduceLocalVariable)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MakeMemberAbstract, "Make member abstract", IsEnabled(RefactoringIdentifiers.MakeMemberAbstract)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MakeMemberVirtual, "Make member virtual", IsEnabled(RefactoringIdentifiers.MakeMemberVirtual)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MergeAssignmentExpressionWithReturnStatement, "Merge assignment expression with return statement", IsEnabled(RefactoringIdentifiers.MergeAssignmentExpressionWithReturnStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MergeAttributes, "Merge attributes", IsEnabled(RefactoringIdentifiers.MergeAttributes)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MergeIfStatements, "Merge if statements", IsEnabled(RefactoringIdentifiers.MergeIfStatements)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MergeLocalDeclarations, "Merge local declarations", IsEnabled(RefactoringIdentifiers.MergeLocalDeclarations)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.JoinStringExpressions, "Join string expressions", IsEnabled(RefactoringIdentifiers.JoinStringExpressions)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.NegateBinaryExpression, "Negate binary expression", IsEnabled(RefactoringIdentifiers.NegateBinaryExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.NegateBooleanLiteral, "Negate boolean literal", IsEnabled(RefactoringIdentifiers.NegateBooleanLiteral)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.NegateIsExpression, "Negate is expression", IsEnabled(RefactoringIdentifiers.NegateIsExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.NegateOperator, "Negate operator", IsEnabled(RefactoringIdentifiers.NegateOperator)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.NotifyPropertyChanged, "Notify property changed", IsEnabled(RefactoringIdentifiers.NotifyPropertyChanged)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ParenthesizeExpression, "Parenthesize expression", IsEnabled(RefactoringIdentifiers.ParenthesizeExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.PromoteLocalToParameter, "Promote local to parameter", IsEnabled(RefactoringIdentifiers.PromoteLocalToParameter)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllComments, "Remove all comments", IsEnabled(RefactoringIdentifiers.RemoveAllComments)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllCommentsExceptDocumentationComments, "Remove all comments (except documentation comments)", IsEnabled(RefactoringIdentifiers.RemoveAllCommentsExceptDocumentationComments)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllDocumentationComments, "Remove all documentation comments", IsEnabled(RefactoringIdentifiers.RemoveAllDocumentationComments)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllMemberDeclarations, "Remove all member declarations", IsEnabled(RefactoringIdentifiers.RemoveAllMemberDeclarations)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllPreprocessorDirectives, "Remove all preprocessor directives", IsEnabled(RefactoringIdentifiers.RemoveAllPreprocessorDirectives)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllRegionDirectives, "Remove all region directives", IsEnabled(RefactoringIdentifiers.RemoveAllRegionDirectives)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllStatements, "Remove all statements", IsEnabled(RefactoringIdentifiers.RemoveAllStatements)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveAllSwitchSections, "Remove all switch sections", IsEnabled(RefactoringIdentifiers.RemoveAllSwitchSections)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveBraces, "Remove braces", IsEnabled(RefactoringIdentifiers.RemoveBraces)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveBracesFromIfElse, "Remove braces from if-else", IsEnabled(RefactoringIdentifiers.RemoveBracesFromIfElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveBracesFromSwitchSection, "Remove braces from switch section", IsEnabled(RefactoringIdentifiers.RemoveBracesFromSwitchSection)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveBracesFromSwitchSections, "Remove braces from switch sections", IsEnabled(RefactoringIdentifiers.RemoveBracesFromSwitchSections)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveComment, "Remove comment", IsEnabled(RefactoringIdentifiers.RemoveComment)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveConditionFromLastElse, "Remove condition from last else clause", IsEnabled(RefactoringIdentifiers.RemoveConditionFromLastElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveDirectiveAndRelatedDirectives, "Remove directive and related directives", IsEnabled(RefactoringIdentifiers.RemoveDirectiveAndRelatedDirectives)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveEmptyLines, "Remove empty lines", IsEnabled(RefactoringIdentifiers.RemoveEmptyLines)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveInterpolation, "Remove interpolation", IsEnabled(RefactoringIdentifiers.RemoveInterpolation)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveMember, "Remove member", IsEnabled(RefactoringIdentifiers.RemoveMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveMemberDeclarations, "Remove member declarations above/below", IsEnabled(RefactoringIdentifiers.RemoveMemberDeclarations)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveParameterNameFromArgument, "Remove parameter name from argument", IsEnabled(RefactoringIdentifiers.RemoveParameterNameFromArgument)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveParentheses, "Remove parentheses", IsEnabled(RefactoringIdentifiers.RemoveParentheses)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemovePropertyInitializer, "Remove property initializer", IsEnabled(RefactoringIdentifiers.RemovePropertyInitializer)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveRegion, "Remove region", IsEnabled(RefactoringIdentifiers.RemoveRegion)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveStatement, "Remove statement", IsEnabled(RefactoringIdentifiers.RemoveStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveStatementsFromSwitchSections, "Remove statements from switch sections", IsEnabled(RefactoringIdentifiers.RemoveStatementsFromSwitchSections)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RenameBackingFieldAccordingToPropertyName, "Rename backing field according to property name", IsEnabled(RefactoringIdentifiers.RenameBackingFieldAccordingToPropertyName)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RenameIdentifierAccordingToTypeName, "Rename identifier according to type name", IsEnabled(RefactoringIdentifiers.RenameIdentifierAccordingToTypeName)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RenameMethodAccordingToTypeName, "Rename method according to type name", IsEnabled(RefactoringIdentifiers.RenameMethodAccordingToTypeName)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RenameParameterAccordingToTypeName, "Rename parameter according to its type name", IsEnabled(RefactoringIdentifiers.RenameParameterAccordingToTypeName)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RenamePropertyAccordingToTypeName, "Rename property according to type name", IsEnabled(RefactoringIdentifiers.RenamePropertyAccordingToTypeName)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceAnyWithAllOrAllWithAny, "Replace Any with All (or All with Any)", IsEnabled(RefactoringIdentifiers.ReplaceAnyWithAllOrAllWithAny)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceAsWithCast, "Replace as expression with cast expression", IsEnabled(RefactoringIdentifiers.ReplaceAsWithCast)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceCastWithAs, "Replace cast expression with as expression", IsEnabled(RefactoringIdentifiers.ReplaceCastWithAs)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceConditionalExpressionWithExpression, "Replace conditional expression with expression", IsEnabled(RefactoringIdentifiers.ReplaceConditionalExpressionWithExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceConditionalExpressionWithIfElse, "Replace ?: with if-else", IsEnabled(RefactoringIdentifiers.ReplaceConditionalExpressionWithIfElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceConstantWithField, "Replace constant with field", IsEnabled(RefactoringIdentifiers.ReplaceConstantWithField)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceDoWithWhile, "Replace do statement with while statement", IsEnabled(RefactoringIdentifiers.ReplaceDoWithWhile)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceEqualsExpressionWithStringEquals, "Replace equals expression with string.Equals", IsEnabled(RefactoringIdentifiers.ReplaceEqualsExpressionWithStringEquals)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceEqualsExpressionWithStringIsNullOrEmpty, "Replace equals expression with string.IsNullOrEmpty", IsEnabled(RefactoringIdentifiers.ReplaceEqualsExpressionWithStringIsNullOrEmpty)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceEqualsExpressionWithStringIsNullOrWhiteSpace, "Replace equals expression with string.IsNullOrWhiteSpace", IsEnabled(RefactoringIdentifiers.ReplaceEqualsExpressionWithStringIsNullOrWhiteSpace)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InlineConstantValue, "Inline constant value", IsEnabled(RefactoringIdentifiers.InlineConstantValue)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseConstantInsteadOfField, "Use constant instead of field", IsEnabled(RefactoringIdentifiers.UseConstantInsteadOfField)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceForEachWithFor, "Replace foreach statement with for statement", IsEnabled(RefactoringIdentifiers.ReplaceForEachWithFor)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceForWithForEach, "Replace for statement with foreach statement", IsEnabled(RefactoringIdentifiers.ReplaceForWithForEach)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceForWithWhile, "Replace for statement with while statement", IsEnabled(RefactoringIdentifiers.ReplaceForWithWhile)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceHexadecimalLiteralWithDecimalLiteral, "Replace hexadecimal literal with decimal literal", IsEnabled(RefactoringIdentifiers.ReplaceHexadecimalLiteralWithDecimalLiteral)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceIfWithSwitch, "Replace if with switch", IsEnabled(RefactoringIdentifiers.ReplaceIfWithSwitch)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InvertPrefixOrPostfixUnaryOperator, "Invert prefix/postfix unary operator", IsEnabled(RefactoringIdentifiers.InvertPrefixOrPostfixUnaryOperator)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceInterpolatedStringWithInterpolationExpression, "Replace interpolated string with interpolation expression", IsEnabled(RefactoringIdentifiers.ReplaceInterpolatedStringWithInterpolationExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceInterpolatedStringWithStringLiteral, "Replace interpolated string with string literal", IsEnabled(RefactoringIdentifiers.ReplaceInterpolatedStringWithStringLiteral)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceMethodGroupWithLambda, "Replace method group with lambda", IsEnabled(RefactoringIdentifiers.ReplaceMethodGroupWithLambda)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceMethodWithProperty, "Replace method with property", IsEnabled(RefactoringIdentifiers.ReplaceMethodWithProperty)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceNullLiteralExpressionWithDefaultExpression, "Replace null literal expression with default expression", IsEnabled(RefactoringIdentifiers.ReplaceNullLiteralExpressionWithDefaultExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplacePrefixOperatorWithPostfixOperator, "Replace prefix operator to postfix operator", IsEnabled(RefactoringIdentifiers.ReplacePrefixOperatorWithPostfixOperator)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplacePropertyWithMethod, "Replace property with method", IsEnabled(RefactoringIdentifiers.ReplacePropertyWithMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceRegularStringLiteralWithVerbatimStringLiteral, "Replace regular string literal with verbatim string literal", IsEnabled(RefactoringIdentifiers.ReplaceRegularStringLiteralWithVerbatimStringLiteral)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceStatementWithIfElse, "Replace (yield) return statement with if-else", IsEnabled(RefactoringIdentifiers.ReplaceStatementWithIfElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.CallIndexOfInsteadOfContains, "Call string.IndexOf instead of string.Contains", IsEnabled(RefactoringIdentifiers.CallIndexOfInsteadOfContains)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceStringFormatWithInterpolatedString, "Replace string.Format with interpolated string", IsEnabled(RefactoringIdentifiers.ReplaceStringFormatWithInterpolatedString)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceSwitchWithIf, "Replace switch with if", IsEnabled(RefactoringIdentifiers.ReplaceSwitchWithIf)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceVerbatimStringLiteralWithRegularStringLiteral, "Replace verbatim string literal with regular string literal", IsEnabled(RefactoringIdentifiers.ReplaceVerbatimStringLiteralWithRegularStringLiteral)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceVerbatimStringLiteralWithRegularStringLiterals, "Replace verbatim string literal with regular string literals", IsEnabled(RefactoringIdentifiers.ReplaceVerbatimStringLiteralWithRegularStringLiterals)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceWhileWithDo, "Replace while statement with do statement", IsEnabled(RefactoringIdentifiers.ReplaceWhileWithDo)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceWhileWithFor, "Replace while statement with for statement", IsEnabled(RefactoringIdentifiers.ReplaceWhileWithFor)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReverseForLoop, "Reverse for loop", IsEnabled(RefactoringIdentifiers.ReverseForLoop)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SimplifyIf, "Simplify if", IsEnabled(RefactoringIdentifiers.SimplifyIf)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SimplifyLambdaExpression, "Simplify lambda expression", IsEnabled(RefactoringIdentifiers.SimplifyLambdaExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SortMemberDeclarations, "Sort member declarations", IsEnabled(RefactoringIdentifiers.SortMemberDeclarations)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SplitAttributes, "Split attributes", IsEnabled(RefactoringIdentifiers.SplitAttributes)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SplitSwitchLabels, "Split switch labels", IsEnabled(RefactoringIdentifiers.SplitSwitchLabels)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SplitVariableDeclaration, "Split variable declaration", IsEnabled(RefactoringIdentifiers.SplitVariableDeclaration)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SwapBinaryOperands, "Swap binary operands", IsEnabled(RefactoringIdentifiers.SwapBinaryOperands)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SwapExpressionsInConditionalExpression, "Swap expressions in conditional expression", IsEnabled(RefactoringIdentifiers.SwapExpressionsInConditionalExpression)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SwapMemberDeclarations, "Swap member declarations", IsEnabled(RefactoringIdentifiers.SwapMemberDeclarations)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SwapIfElse, "Swap if-else", IsEnabled(RefactoringIdentifiers.SwapIfElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UncommentSingleLineComment, "UncommentSingleLineComment", IsEnabled(RefactoringIdentifiers.UncommentSingleLineComment)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseBitwiseOperationInsteadOfCallingHasFlag, "Use bitwise operation instead of calling 'HasFlag'", IsEnabled(RefactoringIdentifiers.UseBitwiseOperationInsteadOfCallingHasFlag)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseCoalesceExpressionInsteadOfIf, "Use coalesce expression instead of if", IsEnabled(RefactoringIdentifiers.UseCoalesceExpressionInsteadOfIf)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseConditionalExpressionInsteadOfIf, "Use conditional expression instead of if", IsEnabled(RefactoringIdentifiers.UseConditionalExpressionInsteadOfIf)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseElementAccessInsteadOfEnumerableMethod, "Use element access instead of 'First/Last'ElementAt' method", IsEnabled(RefactoringIdentifiers.UseElementAccessInsteadOfEnumerableMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseEmptyStringLiteralInsteadOfStringEmpty, "Use \"\" instead of string.Empty", IsEnabled(RefactoringIdentifiers.UseEmptyStringLiteralInsteadOfStringEmpty)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseExpressionBodiedMember, "Use expression-bodied member", IsEnabled(RefactoringIdentifiers.UseExpressionBodiedMember)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseLambdaExpressionInsteadOfAnonymousMethod, "Use lambda expression instead of anonymous method", IsEnabled(RefactoringIdentifiers.UseLambdaExpressionInsteadOfAnonymousMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseStringEmptyInsteadOfEmptyStringLiteral, "Use string.Empty instead of \"\"", IsEnabled(RefactoringIdentifiers.UseStringEmptyInsteadOfEmptyStringLiteral)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.WrapInCondition, "Wrap in condition", IsEnabled(RefactoringIdentifiers.WrapInCondition)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.WrapInElseClause, "Wrap in else clause", IsEnabled(RefactoringIdentifiers.WrapInElseClause)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.WrapInIfDirective, "Wrap in #if directive", IsEnabled(RefactoringIdentifiers.WrapInIfDirective)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.WrapInRegion, "Wrap in region", IsEnabled(RefactoringIdentifiers.WrapInRegion)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.WrapInTryCatch, "Wrap in try-catch", IsEnabled(RefactoringIdentifiers.WrapInTryCatch)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.WrapInUsingStatement, "Wrap in using statement", IsEnabled(RefactoringIdentifiers.WrapInUsingStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddTypeParameter, "Add type parameter", IsEnabled(RefactoringIdentifiers.AddTypeParameter)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ImplementIEquatableOfT, "Implement IEquatable<T>", IsEnabled(RefactoringIdentifiers.ImplementIEquatableOfT)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InlineUsingStatic, "Inline using static", IsEnabled(RefactoringIdentifiers.InlineUsingStatic)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InlineConstant, "Inline constant", IsEnabled(RefactoringIdentifiers.InlineConstant)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseStringBuilderInsteadOfConcatenation, "Use StringBuilder instead of concatenation", IsEnabled(RefactoringIdentifiers.UseStringBuilderInsteadOfConcatenation)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseListInsteadOfYield, "Use List<T> instead of yield", IsEnabled(RefactoringIdentifiers.UseListInsteadOfYield)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SplitIfStatement, "Split if statement", IsEnabled(RefactoringIdentifiers.SplitIfStatement)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceObjectCreationWithDefaultValue, "Replace object creation with default value", IsEnabled(RefactoringIdentifiers.ReplaceObjectCreationWithDefaultValue)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ChangeAccessibility, "Change accessibility", IsEnabled(RefactoringIdentifiers.ChangeAccessibility)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.FormatConstraintClauses, "Format constraint clauses", IsEnabled(RefactoringIdentifiers.FormatConstraintClauses)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceForEachWithForAndReverseLoop, "Replace foreach with for and reverse loop", IsEnabled(RefactoringIdentifiers.ReplaceForEachWithForAndReverseLoop)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReduceIfNesting, "Reduce if nesting", IsEnabled(RefactoringIdentifiers.ReduceIfNesting)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SplitIfElse, "Split if-else", IsEnabled(RefactoringIdentifiers.SplitIfElse)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UseCSharp6DictionaryInitializer, "Use C# 6.0 dictionary initializer", IsEnabled(RefactoringIdentifiers.UseCSharp6DictionaryInitializer)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceCommentWithDocumentationComment, "Replace comment with documentation comment", IsEnabled(RefactoringIdentifiers.ReplaceCommentWithDocumentationComment)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceInterpolatedStringWithConcatenation, "Replace interpolated string with concatenation", IsEnabled(RefactoringIdentifiers.ReplaceInterpolatedStringWithConcatenation)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.SplitDeclarationAndInitialization, "Split declaration and initialization", IsEnabled(RefactoringIdentifiers.SplitDeclarationAndInitialization)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddMemberToInterface, "Add member to interface", IsEnabled(RefactoringIdentifiers.AddMemberToInterface)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MergeIfWithParentIf, "Merge if with parent if", IsEnabled(RefactoringIdentifiers.MergeIfWithParentIf)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InitializeFieldFromConstructor, "Initialize field from constructor", IsEnabled(RefactoringIdentifiers.InitializeFieldFromConstructor)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.InlineProperty, "Inline property", IsEnabled(RefactoringIdentifiers.InlineProperty)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.RemoveEnumMemberValue, "Remove enum member value(s)", IsEnabled(RefactoringIdentifiers.RemoveEnumMemberValue)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.UncommentMultiLineComment, "Uncomment multi-line comment", IsEnabled(RefactoringIdentifiers.UncommentMultiLineComment)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceInterpolatedStringWithStringFormat, "Replace interpolated string with string.Format", IsEnabled(RefactoringIdentifiers.ReplaceInterpolatedStringWithStringFormat)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.MoveUnsafeContextToContainingDeclaration, "Move unsafe context to containing declaration", IsEnabled(RefactoringIdentifiers.MoveUnsafeContextToContainingDeclaration)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ExtractEventHandlerMethod, "Extract event handler method", IsEnabled(RefactoringIdentifiers.ExtractEventHandlerMethod)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.GeneratePropertyForDebuggerDisplayAttribute, "Generate property for DebuggerDisplay attribute", IsEnabled(RefactoringIdentifiers.GeneratePropertyForDebuggerDisplayAttribute)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceForEachWithEnumerator, "Replace foreach with enumerator", IsEnabled(RefactoringIdentifiers.ReplaceForEachWithEnumerator)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.AddEmptyLineBetweenDeclarations, "Add empty line between declarations", IsEnabled(RefactoringIdentifiers.AddEmptyLineBetweenDeclarations)));
            refactorings.Add(new BaseModel(RefactoringIdentifiers.ReplaceForEachWithEnumerator, "Replace foreach with enumerator", IsEnabled(RefactoringIdentifiers.ReplaceForEachWithEnumerator)));
        }
    }
}