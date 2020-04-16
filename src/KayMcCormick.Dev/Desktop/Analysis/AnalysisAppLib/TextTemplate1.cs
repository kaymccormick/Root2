﻿using System ;

using System.Linq;
using Microsoft.CodeAnalysis.CSharp ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using PocoSyntax ;


/// <summary/>
public static class GenTransforms {

        /// <summary></summary>
        [NotNull]
        public static PocoTypeArgumentListSyntax Transform_Type_Argument_List ([NotNull] TypeArgumentListSyntax node) {
            return new PocoTypeArgumentListSyntax() { 
                Arguments = new PocoTypeSyntaxCollection(node?.Arguments.Select(Transform_Type).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoArrayRankSpecifierSyntax Transform_Array_Rank_Specifier ([NotNull] ArrayRankSpecifierSyntax node) {
            return new PocoArrayRankSpecifierSyntax() { 
                Sizes = new PocoExpressionSyntaxCollection(node?.Sizes.Select(Transform_Expression).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTupleElementSyntax Transform_Tuple_Element ([NotNull] TupleElementSyntax node) {
            return new PocoTupleElementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIdentifierNameSyntax Transform_Identifier_Name ([NotNull] IdentifierNameSyntax node) {
            return new PocoIdentifierNameSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoGenericNameSyntax Transform_Generic_Name ([NotNull] GenericNameSyntax node) {
            return new PocoGenericNameSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSimpleNameSyntax Transform_Simple_Name ([NotNull] SimpleNameSyntax node) {
            switch(node) {
            case IdentifierNameSyntax _: return Transform_Identifier_Name((IdentifierNameSyntax)node); 
            case GenericNameSyntax _: return Transform_Generic_Name((GenericNameSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoQualifiedNameSyntax Transform_Qualified_Name ([NotNull] QualifiedNameSyntax node) {
            return new PocoQualifiedNameSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAliasQualifiedNameSyntax Transform_Alias_Qualified_Name ([NotNull] AliasQualifiedNameSyntax node) {
            return new PocoAliasQualifiedNameSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNameSyntax Transform_Name ([NotNull] NameSyntax node) {
            switch(node) {
            case IdentifierNameSyntax _: return Transform_Identifier_Name((IdentifierNameSyntax)node); 
            case GenericNameSyntax _: return Transform_Generic_Name((GenericNameSyntax)node); 
            case QualifiedNameSyntax _: return Transform_Qualified_Name((QualifiedNameSyntax)node); 
            case AliasQualifiedNameSyntax _: return Transform_Alias_Qualified_Name((AliasQualifiedNameSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPredefinedTypeSyntax Transform_Predefined_Type ([NotNull] PredefinedTypeSyntax node) {
            return new PocoPredefinedTypeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoArrayTypeSyntax Transform_Array_Type ([NotNull] ArrayTypeSyntax node) {
            return new PocoArrayTypeSyntax() { 
                RankSpecifiers = new PocoArrayRankSpecifierSyntaxCollection(node?.RankSpecifiers.Select(Transform_Array_Rank_Specifier).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPointerTypeSyntax Transform_Pointer_Type ([NotNull] PointerTypeSyntax node) {
            return new PocoPointerTypeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNullableTypeSyntax Transform_Nullable_Type ([NotNull] NullableTypeSyntax node) {
            return new PocoNullableTypeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTupleTypeSyntax Transform_Tuple_Type ([NotNull] TupleTypeSyntax node) {
            return new PocoTupleTypeSyntax() { 
                Elements = new PocoTupleElementSyntaxCollection(node?.Elements.Select(Transform_Tuple_Element).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoOmittedTypeArgumentSyntax Transform_Omitted_Type_Argument ([NotNull] OmittedTypeArgumentSyntax node) {
            return new PocoOmittedTypeArgumentSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRefTypeSyntax Transform_Ref_Type ([NotNull] RefTypeSyntax node) {
            return new PocoRefTypeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeSyntax Transform_Type ([NotNull] TypeSyntax node) {
            switch(node) {
            case IdentifierNameSyntax _: return Transform_Identifier_Name((IdentifierNameSyntax)node); 
            case GenericNameSyntax _: return Transform_Generic_Name((GenericNameSyntax)node); 
            case QualifiedNameSyntax _: return Transform_Qualified_Name((QualifiedNameSyntax)node); 
            case AliasQualifiedNameSyntax _: return Transform_Alias_Qualified_Name((AliasQualifiedNameSyntax)node); 
            case PredefinedTypeSyntax _: return Transform_Predefined_Type((PredefinedTypeSyntax)node); 
            case ArrayTypeSyntax _: return Transform_Array_Type((ArrayTypeSyntax)node); 
            case PointerTypeSyntax _: return Transform_Pointer_Type((PointerTypeSyntax)node); 
            case NullableTypeSyntax _: return Transform_Nullable_Type((NullableTypeSyntax)node); 
            case TupleTypeSyntax _: return Transform_Tuple_Type((TupleTypeSyntax)node); 
            case OmittedTypeArgumentSyntax _: return Transform_Omitted_Type_Argument((OmittedTypeArgumentSyntax)node); 
            case RefTypeSyntax _: return Transform_Ref_Type((RefTypeSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoParenthesizedExpressionSyntax Transform_Parenthesized_Expression ([NotNull] ParenthesizedExpressionSyntax node) {
            return new PocoParenthesizedExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTupleExpressionSyntax Transform_Tuple_Expression ([NotNull] TupleExpressionSyntax node) {
            return new PocoTupleExpressionSyntax() { 
                Arguments = new PocoArgumentSyntaxCollection(node?.Arguments.Select(Transform_Argument).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPrefixUnaryExpressionSyntax Transform_Prefix_Unary_Expression ([NotNull] PrefixUnaryExpressionSyntax node) {
            return new PocoPrefixUnaryExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAwaitExpressionSyntax Transform_Await_Expression ([NotNull] AwaitExpressionSyntax node) {
            return new PocoAwaitExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPostfixUnaryExpressionSyntax Transform_Postfix_Unary_Expression ([NotNull] PostfixUnaryExpressionSyntax node) {
            return new PocoPostfixUnaryExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoMemberAccessExpressionSyntax Transform_Member_Access_Expression ([NotNull] MemberAccessExpressionSyntax node) {
            return new PocoMemberAccessExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConditionalAccessExpressionSyntax Transform_Conditional_Access_Expression ([NotNull] ConditionalAccessExpressionSyntax node) {
            return new PocoConditionalAccessExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoMemberBindingExpressionSyntax Transform_Member_Binding_Expression ([NotNull] MemberBindingExpressionSyntax node) {
            return new PocoMemberBindingExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoElementBindingExpressionSyntax Transform_Element_Binding_Expression ([NotNull] ElementBindingExpressionSyntax node) {
            return new PocoElementBindingExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRangeExpressionSyntax Transform_Range_Expression ([NotNull] RangeExpressionSyntax node) {
            return new PocoRangeExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoImplicitElementAccessSyntax Transform_Implicit_Element_Access ([NotNull] ImplicitElementAccessSyntax node) {
            return new PocoImplicitElementAccessSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBinaryExpressionSyntax Transform_Binary_Expression ([NotNull] BinaryExpressionSyntax node) {
            return new PocoBinaryExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAssignmentExpressionSyntax Transform_Assignment_Expression ([NotNull] AssignmentExpressionSyntax node) {
            return new PocoAssignmentExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConditionalExpressionSyntax Transform_Conditional_Expression ([NotNull] ConditionalExpressionSyntax node) {
            return new PocoConditionalExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoThisExpressionSyntax Transform_This_Expression ([NotNull] ThisExpressionSyntax node) {
            return new PocoThisExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseExpressionSyntax Transform_Base_Expression ([NotNull] BaseExpressionSyntax node) {
            return new PocoBaseExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInstanceExpressionSyntax Transform_Instance_Expression ([NotNull] InstanceExpressionSyntax node) {
            switch(node) {
            case ThisExpressionSyntax _: return Transform_This_Expression((ThisExpressionSyntax)node); 
            case BaseExpressionSyntax _: return Transform_Base_Expression((BaseExpressionSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLiteralExpressionSyntax Transform_Literal_Expression ([NotNull] LiteralExpressionSyntax node) {
            return new PocoLiteralExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoMakeRefExpressionSyntax Transform_Make_Ref_Expression ([NotNull] MakeRefExpressionSyntax node) {
            return new PocoMakeRefExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRefTypeExpressionSyntax Transform_Ref_Type_Expression ([NotNull] RefTypeExpressionSyntax node) {
            return new PocoRefTypeExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRefValueExpressionSyntax Transform_Ref_Value_Expression ([NotNull] RefValueExpressionSyntax node) {
            return new PocoRefValueExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCheckedExpressionSyntax Transform_Checked_Expression ([NotNull] CheckedExpressionSyntax node) {
            return new PocoCheckedExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDefaultExpressionSyntax Transform_Default_Expression ([NotNull] DefaultExpressionSyntax node) {
            return new PocoDefaultExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeOfExpressionSyntax Transform_Type_Of_Expression ([NotNull] TypeOfExpressionSyntax node) {
            return new PocoTypeOfExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSizeOfExpressionSyntax Transform_Size_Of_Expression ([NotNull] SizeOfExpressionSyntax node) {
            return new PocoSizeOfExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInvocationExpressionSyntax Transform_Invocation_Expression ([NotNull] InvocationExpressionSyntax node) {
            return new PocoInvocationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoElementAccessExpressionSyntax Transform_Element_Access_Expression ([NotNull] ElementAccessExpressionSyntax node) {
            return new PocoElementAccessExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDeclarationExpressionSyntax Transform_Declaration_Expression ([NotNull] DeclarationExpressionSyntax node) {
            return new PocoDeclarationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCastExpressionSyntax Transform_Cast_Expression ([NotNull] CastExpressionSyntax node) {
            return new PocoCastExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAnonymousMethodExpressionSyntax Transform_Anonymous_Method_Expression ([NotNull] AnonymousMethodExpressionSyntax node) {
            return new PocoAnonymousMethodExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSimpleLambdaExpressionSyntax Transform_Simple_Lambda_Expression ([NotNull] SimpleLambdaExpressionSyntax node) {
            return new PocoSimpleLambdaExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoParenthesizedLambdaExpressionSyntax Transform_Parenthesized_Lambda_Expression ([NotNull] ParenthesizedLambdaExpressionSyntax node) {
            return new PocoParenthesizedLambdaExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLambdaExpressionSyntax Transform_Lambda_Expression ([NotNull] LambdaExpressionSyntax node) {
            switch(node) {
            case SimpleLambdaExpressionSyntax _: return Transform_Simple_Lambda_Expression((SimpleLambdaExpressionSyntax)node); 
            case ParenthesizedLambdaExpressionSyntax _: return Transform_Parenthesized_Lambda_Expression((ParenthesizedLambdaExpressionSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAnonymousFunctionExpressionSyntax Transform_Anonymous_Function_Expression ([NotNull] AnonymousFunctionExpressionSyntax node) {
            switch(node) {
            case AnonymousMethodExpressionSyntax _: return Transform_Anonymous_Method_Expression((AnonymousMethodExpressionSyntax)node); 
            case SimpleLambdaExpressionSyntax _: return Transform_Simple_Lambda_Expression((SimpleLambdaExpressionSyntax)node); 
            case ParenthesizedLambdaExpressionSyntax _: return Transform_Parenthesized_Lambda_Expression((ParenthesizedLambdaExpressionSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRefExpressionSyntax Transform_Ref_Expression ([NotNull] RefExpressionSyntax node) {
            return new PocoRefExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInitializerExpressionSyntax Transform_Initializer_Expression ([NotNull] InitializerExpressionSyntax node) {
            return new PocoInitializerExpressionSyntax() { 
                Expressions = new PocoExpressionSyntaxCollection(node?.Expressions.Select(Transform_Expression).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoObjectCreationExpressionSyntax Transform_Object_Creation_Expression ([NotNull] ObjectCreationExpressionSyntax node) {
            return new PocoObjectCreationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAnonymousObjectCreationExpressionSyntax Transform_Anonymous_Object_Creation_Expression ([NotNull] AnonymousObjectCreationExpressionSyntax node) {
            return new PocoAnonymousObjectCreationExpressionSyntax() { 
                Initializers = new PocoAnonymousObjectMemberDeclaratorSyntaxCollection(node?.Initializers.Select(Transform_Anonymous_Object_Member_Declarator).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoArrayCreationExpressionSyntax Transform_Array_Creation_Expression ([NotNull] ArrayCreationExpressionSyntax node) {
            return new PocoArrayCreationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoImplicitArrayCreationExpressionSyntax Transform_Implicit_Array_Creation_Expression ([NotNull] ImplicitArrayCreationExpressionSyntax node) {
            return new PocoImplicitArrayCreationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoStackAllocArrayCreationExpressionSyntax Transform_Stack_Alloc_Array_Creation_Expression ([NotNull] StackAllocArrayCreationExpressionSyntax node) {
            return new PocoStackAllocArrayCreationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoImplicitStackAllocArrayCreationExpressionSyntax Transform_Implicit_Stack_Alloc_Array_Creation_Expression ([NotNull] ImplicitStackAllocArrayCreationExpressionSyntax node) {
            return new PocoImplicitStackAllocArrayCreationExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoQueryExpressionSyntax Transform_Query_Expression ([NotNull] QueryExpressionSyntax node) {
            return new PocoQueryExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoOmittedArraySizeExpressionSyntax Transform_Omitted_Array_Size_Expression ([NotNull] OmittedArraySizeExpressionSyntax node) {
            return new PocoOmittedArraySizeExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterpolatedStringExpressionSyntax Transform_Interpolated_String_Expression ([NotNull] InterpolatedStringExpressionSyntax node) {
            return new PocoInterpolatedStringExpressionSyntax() { 
                Contents = new PocoInterpolatedStringContentSyntaxCollection(node?.Contents.Select(Transform_Interpolated_String_Content).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIsPatternExpressionSyntax Transform_Is_Pattern_Expression ([NotNull] IsPatternExpressionSyntax node) {
            return new PocoIsPatternExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoThrowExpressionSyntax Transform_Throw_Expression ([NotNull] ThrowExpressionSyntax node) {
            return new PocoThrowExpressionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSwitchExpressionSyntax Transform_Switch_Expression ([NotNull] SwitchExpressionSyntax node) {
            return new PocoSwitchExpressionSyntax() { 
                Arms = new PocoSwitchExpressionArmSyntaxCollection(node?.Arms.Select(Transform_Switch_Expression_Arm).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoExpressionSyntax Transform_Expression ([NotNull] ExpressionSyntax node) {
            switch(node) {
            case IdentifierNameSyntax _: return Transform_Identifier_Name((IdentifierNameSyntax)node); 
            case GenericNameSyntax _: return Transform_Generic_Name((GenericNameSyntax)node); 
            case QualifiedNameSyntax _: return Transform_Qualified_Name((QualifiedNameSyntax)node); 
            case AliasQualifiedNameSyntax _: return Transform_Alias_Qualified_Name((AliasQualifiedNameSyntax)node); 
            case PredefinedTypeSyntax _: return Transform_Predefined_Type((PredefinedTypeSyntax)node); 
            case ArrayTypeSyntax _: return Transform_Array_Type((ArrayTypeSyntax)node); 
            case PointerTypeSyntax _: return Transform_Pointer_Type((PointerTypeSyntax)node); 
            case NullableTypeSyntax _: return Transform_Nullable_Type((NullableTypeSyntax)node); 
            case TupleTypeSyntax _: return Transform_Tuple_Type((TupleTypeSyntax)node); 
            case OmittedTypeArgumentSyntax _: return Transform_Omitted_Type_Argument((OmittedTypeArgumentSyntax)node); 
            case RefTypeSyntax _: return Transform_Ref_Type((RefTypeSyntax)node); 
            case ParenthesizedExpressionSyntax _: return Transform_Parenthesized_Expression((ParenthesizedExpressionSyntax)node); 
            case TupleExpressionSyntax _: return Transform_Tuple_Expression((TupleExpressionSyntax)node); 
            case PrefixUnaryExpressionSyntax _: return Transform_Prefix_Unary_Expression((PrefixUnaryExpressionSyntax)node); 
            case AwaitExpressionSyntax _: return Transform_Await_Expression((AwaitExpressionSyntax)node); 
            case PostfixUnaryExpressionSyntax _: return Transform_Postfix_Unary_Expression((PostfixUnaryExpressionSyntax)node); 
            case MemberAccessExpressionSyntax _: return Transform_Member_Access_Expression((MemberAccessExpressionSyntax)node); 
            case ConditionalAccessExpressionSyntax _: return Transform_Conditional_Access_Expression((ConditionalAccessExpressionSyntax)node); 
            case MemberBindingExpressionSyntax _: return Transform_Member_Binding_Expression((MemberBindingExpressionSyntax)node); 
            case ElementBindingExpressionSyntax _: return Transform_Element_Binding_Expression((ElementBindingExpressionSyntax)node); 
            case RangeExpressionSyntax _: return Transform_Range_Expression((RangeExpressionSyntax)node); 
            case ImplicitElementAccessSyntax _: return Transform_Implicit_Element_Access((ImplicitElementAccessSyntax)node); 
            case BinaryExpressionSyntax _: return Transform_Binary_Expression((BinaryExpressionSyntax)node); 
            case AssignmentExpressionSyntax _: return Transform_Assignment_Expression((AssignmentExpressionSyntax)node); 
            case ConditionalExpressionSyntax _: return Transform_Conditional_Expression((ConditionalExpressionSyntax)node); 
            case ThisExpressionSyntax _: return Transform_This_Expression((ThisExpressionSyntax)node); 
            case BaseExpressionSyntax _: return Transform_Base_Expression((BaseExpressionSyntax)node); 
            case LiteralExpressionSyntax _: return Transform_Literal_Expression((LiteralExpressionSyntax)node); 
            case MakeRefExpressionSyntax _: return Transform_Make_Ref_Expression((MakeRefExpressionSyntax)node); 
            case RefTypeExpressionSyntax _: return Transform_Ref_Type_Expression((RefTypeExpressionSyntax)node); 
            case RefValueExpressionSyntax _: return Transform_Ref_Value_Expression((RefValueExpressionSyntax)node); 
            case CheckedExpressionSyntax _: return Transform_Checked_Expression((CheckedExpressionSyntax)node); 
            case DefaultExpressionSyntax _: return Transform_Default_Expression((DefaultExpressionSyntax)node); 
            case TypeOfExpressionSyntax _: return Transform_Type_Of_Expression((TypeOfExpressionSyntax)node); 
            case SizeOfExpressionSyntax _: return Transform_Size_Of_Expression((SizeOfExpressionSyntax)node); 
            case InvocationExpressionSyntax _: return Transform_Invocation_Expression((InvocationExpressionSyntax)node); 
            case ElementAccessExpressionSyntax _: return Transform_Element_Access_Expression((ElementAccessExpressionSyntax)node); 
            case DeclarationExpressionSyntax _: return Transform_Declaration_Expression((DeclarationExpressionSyntax)node); 
            case CastExpressionSyntax _: return Transform_Cast_Expression((CastExpressionSyntax)node); 
            case AnonymousMethodExpressionSyntax _: return Transform_Anonymous_Method_Expression((AnonymousMethodExpressionSyntax)node); 
            case SimpleLambdaExpressionSyntax _: return Transform_Simple_Lambda_Expression((SimpleLambdaExpressionSyntax)node); 
            case ParenthesizedLambdaExpressionSyntax _: return Transform_Parenthesized_Lambda_Expression((ParenthesizedLambdaExpressionSyntax)node); 
            case RefExpressionSyntax _: return Transform_Ref_Expression((RefExpressionSyntax)node); 
            case InitializerExpressionSyntax _: return Transform_Initializer_Expression((InitializerExpressionSyntax)node); 
            case ObjectCreationExpressionSyntax _: return Transform_Object_Creation_Expression((ObjectCreationExpressionSyntax)node); 
            case AnonymousObjectCreationExpressionSyntax _: return Transform_Anonymous_Object_Creation_Expression((AnonymousObjectCreationExpressionSyntax)node); 
            case ArrayCreationExpressionSyntax _: return Transform_Array_Creation_Expression((ArrayCreationExpressionSyntax)node); 
            case ImplicitArrayCreationExpressionSyntax _: return Transform_Implicit_Array_Creation_Expression((ImplicitArrayCreationExpressionSyntax)node); 
            case StackAllocArrayCreationExpressionSyntax _: return Transform_Stack_Alloc_Array_Creation_Expression((StackAllocArrayCreationExpressionSyntax)node); 
            case ImplicitStackAllocArrayCreationExpressionSyntax _: return Transform_Implicit_Stack_Alloc_Array_Creation_Expression((ImplicitStackAllocArrayCreationExpressionSyntax)node); 
            case QueryExpressionSyntax _: return Transform_Query_Expression((QueryExpressionSyntax)node); 
            case OmittedArraySizeExpressionSyntax _: return Transform_Omitted_Array_Size_Expression((OmittedArraySizeExpressionSyntax)node); 
            case InterpolatedStringExpressionSyntax _: return Transform_Interpolated_String_Expression((InterpolatedStringExpressionSyntax)node); 
            case IsPatternExpressionSyntax _: return Transform_Is_Pattern_Expression((IsPatternExpressionSyntax)node); 
            case ThrowExpressionSyntax _: return Transform_Throw_Expression((ThrowExpressionSyntax)node); 
            case SwitchExpressionSyntax _: return Transform_Switch_Expression((SwitchExpressionSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoArgumentListSyntax Transform_Argument_List ([NotNull] ArgumentListSyntax node) {
            return new PocoArgumentListSyntax() { 
                Arguments = new PocoArgumentSyntaxCollection(node?.Arguments.Select(Transform_Argument).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBracketedArgumentListSyntax Transform_Bracketed_Argument_List ([NotNull] BracketedArgumentListSyntax node) {
            return new PocoBracketedArgumentListSyntax() { 
                Arguments = new PocoArgumentSyntaxCollection(node?.Arguments.Select(Transform_Argument).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseArgumentListSyntax Transform_Base_Argument_List ([NotNull] BaseArgumentListSyntax node) {
            switch(node) {
            case ArgumentListSyntax _: return Transform_Argument_List((ArgumentListSyntax)node); 
            case BracketedArgumentListSyntax _: return Transform_Bracketed_Argument_List((BracketedArgumentListSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoArgumentSyntax Transform_Argument ([NotNull] ArgumentSyntax node) {
            return new PocoArgumentSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNameColonSyntax Transform_Name_Colon ([NotNull] NameColonSyntax node) {
            return new PocoNameColonSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAnonymousObjectMemberDeclaratorSyntax Transform_Anonymous_Object_Member_Declarator ([NotNull] AnonymousObjectMemberDeclaratorSyntax node) {
            return new PocoAnonymousObjectMemberDeclaratorSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoFromClauseSyntax Transform_From_Clause ([NotNull] FromClauseSyntax node) {
            return new PocoFromClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLetClauseSyntax Transform_Let_Clause ([NotNull] LetClauseSyntax node) {
            return new PocoLetClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoJoinClauseSyntax Transform_Join_Clause ([NotNull] JoinClauseSyntax node) {
            return new PocoJoinClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoWhereClauseSyntax Transform_Where_Clause ([NotNull] WhereClauseSyntax node) {
            return new PocoWhereClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoOrderByClauseSyntax Transform_Order_By_Clause ([NotNull] OrderByClauseSyntax node) {
            return new PocoOrderByClauseSyntax() { 
                Orderings = new PocoOrderingSyntaxCollection(node?.Orderings.Select(Transform_Ordering).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoQueryClauseSyntax Transform_Query_Clause ([NotNull] QueryClauseSyntax node) {
            switch(node) {
            case FromClauseSyntax _: return Transform_From_Clause((FromClauseSyntax)node); 
            case LetClauseSyntax _: return Transform_Let_Clause((LetClauseSyntax)node); 
            case JoinClauseSyntax _: return Transform_Join_Clause((JoinClauseSyntax)node); 
            case WhereClauseSyntax _: return Transform_Where_Clause((WhereClauseSyntax)node); 
            case OrderByClauseSyntax _: return Transform_Order_By_Clause((OrderByClauseSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSelectClauseSyntax Transform_Select_Clause ([NotNull] SelectClauseSyntax node) {
            return new PocoSelectClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoGroupClauseSyntax Transform_Group_Clause ([NotNull] GroupClauseSyntax node) {
            return new PocoGroupClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSelectOrGroupClauseSyntax Transform_Select_Or_Group_Clause ([NotNull] SelectOrGroupClauseSyntax node) {
            switch(node) {
            case SelectClauseSyntax _: return Transform_Select_Clause((SelectClauseSyntax)node); 
            case GroupClauseSyntax _: return Transform_Group_Clause((GroupClauseSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoQueryBodySyntax Transform_Query_Body ([NotNull] QueryBodySyntax node) {
            return new PocoQueryBodySyntax() { 
                Clauses = new PocoQueryClauseSyntaxCollection(node?.Clauses.Select(Transform_Query_Clause).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoJoinIntoClauseSyntax Transform_Join_Into_Clause ([NotNull] JoinIntoClauseSyntax node) {
            return new PocoJoinIntoClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoOrderingSyntax Transform_Ordering ([NotNull] OrderingSyntax node) {
            return new PocoOrderingSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoQueryContinuationSyntax Transform_Query_Continuation ([NotNull] QueryContinuationSyntax node) {
            return new PocoQueryContinuationSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoWhenClauseSyntax Transform_When_Clause ([NotNull] WhenClauseSyntax node) {
            return new PocoWhenClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDiscardPatternSyntax Transform_Discard_Pattern ([NotNull] DiscardPatternSyntax node) {
            return new PocoDiscardPatternSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDeclarationPatternSyntax Transform_Declaration_Pattern ([NotNull] DeclarationPatternSyntax node) {
            return new PocoDeclarationPatternSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoVarPatternSyntax Transform_Var_Pattern ([NotNull] VarPatternSyntax node) {
            return new PocoVarPatternSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRecursivePatternSyntax Transform_Recursive_Pattern ([NotNull] RecursivePatternSyntax node) {
            return new PocoRecursivePatternSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConstantPatternSyntax Transform_Constant_Pattern ([NotNull] ConstantPatternSyntax node) {
            return new PocoConstantPatternSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPatternSyntax Transform_Pattern ([NotNull] PatternSyntax node) {
            switch(node) {
            case DiscardPatternSyntax _: return Transform_Discard_Pattern((DiscardPatternSyntax)node); 
            case DeclarationPatternSyntax _: return Transform_Declaration_Pattern((DeclarationPatternSyntax)node); 
            case VarPatternSyntax _: return Transform_Var_Pattern((VarPatternSyntax)node); 
            case RecursivePatternSyntax _: return Transform_Recursive_Pattern((RecursivePatternSyntax)node); 
            case ConstantPatternSyntax _: return Transform_Constant_Pattern((ConstantPatternSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPositionalPatternClauseSyntax Transform_Positional_Pattern_Clause ([NotNull] PositionalPatternClauseSyntax node) {
            return new PocoPositionalPatternClauseSyntax() { 
                Subpatterns = new PocoSubpatternSyntaxCollection(node?.Subpatterns.Select(Transform_Subpattern).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPropertyPatternClauseSyntax Transform_Property_Pattern_Clause ([NotNull] PropertyPatternClauseSyntax node) {
            return new PocoPropertyPatternClauseSyntax() { 
                Subpatterns = new PocoSubpatternSyntaxCollection(node?.Subpatterns.Select(Transform_Subpattern).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSubpatternSyntax Transform_Subpattern ([NotNull] SubpatternSyntax node) {
            return new PocoSubpatternSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterpolatedStringTextSyntax Transform_Interpolated_String_Text ([NotNull] InterpolatedStringTextSyntax node) {
            return new PocoInterpolatedStringTextSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterpolationSyntax Transform_Interpolation ([NotNull] InterpolationSyntax node) {
            return new PocoInterpolationSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterpolatedStringContentSyntax Transform_Interpolated_String_Content ([NotNull] InterpolatedStringContentSyntax node) {
            switch(node) {
            case InterpolatedStringTextSyntax _: return Transform_Interpolated_String_Text((InterpolatedStringTextSyntax)node); 
            case InterpolationSyntax _: return Transform_Interpolation((InterpolationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterpolationAlignmentClauseSyntax Transform_Interpolation_Alignment_Clause ([NotNull] InterpolationAlignmentClauseSyntax node) {
            return new PocoInterpolationAlignmentClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterpolationFormatClauseSyntax Transform_Interpolation_Format_Clause ([NotNull] InterpolationFormatClauseSyntax node) {
            return new PocoInterpolationFormatClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBlockSyntax Transform_Block ([NotNull] BlockSyntax node) {
            return new PocoBlockSyntax() { 
                Statements = new PocoStatementSyntaxCollection(node?.Statements.Select(Transform_Statement).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLocalFunctionStatementSyntax Transform_Local_Function_Statement ([NotNull] LocalFunctionStatementSyntax node) {
            return new PocoLocalFunctionStatementSyntax() { 
                ConstraintClauses = new PocoTypeParameterConstraintClauseSyntaxCollection(node?.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLocalDeclarationStatementSyntax Transform_Local_Declaration_Statement ([NotNull] LocalDeclarationStatementSyntax node) {
            return new PocoLocalDeclarationStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoExpressionStatementSyntax Transform_Expression_Statement ([NotNull] ExpressionStatementSyntax node) {
            return new PocoExpressionStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEmptyStatementSyntax Transform_Empty_Statement ([NotNull] EmptyStatementSyntax node) {
            return new PocoEmptyStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLabeledStatementSyntax Transform_Labeled_Statement ([NotNull] LabeledStatementSyntax node) {
            return new PocoLabeledStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoGotoStatementSyntax Transform_Goto_Statement ([NotNull] GotoStatementSyntax node) {
            return new PocoGotoStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBreakStatementSyntax Transform_Break_Statement ([NotNull] BreakStatementSyntax node) {
            return new PocoBreakStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoContinueStatementSyntax Transform_Continue_Statement ([NotNull] ContinueStatementSyntax node) {
            return new PocoContinueStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoReturnStatementSyntax Transform_Return_Statement ([NotNull] ReturnStatementSyntax node) {
            return new PocoReturnStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoThrowStatementSyntax Transform_Throw_Statement ([NotNull] ThrowStatementSyntax node) {
            return new PocoThrowStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoYieldStatementSyntax Transform_Yield_Statement ([NotNull] YieldStatementSyntax node) {
            return new PocoYieldStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoWhileStatementSyntax Transform_While_Statement ([NotNull] WhileStatementSyntax node) {
            return new PocoWhileStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDoStatementSyntax Transform_Do_Statement ([NotNull] DoStatementSyntax node) {
            return new PocoDoStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoForStatementSyntax Transform_For_Statement ([NotNull] ForStatementSyntax node) {
            return new PocoForStatementSyntax() { 
                Incrementors = new PocoExpressionSyntaxCollection(node?.Incrementors.Select(Transform_Expression).ToList()), 
                Initializers = new PocoExpressionSyntaxCollection(node?.Initializers.Select(Transform_Expression).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoForEachStatementSyntax Transform_For_Each_Statement ([NotNull] ForEachStatementSyntax node) {
            return new PocoForEachStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoForEachVariableStatementSyntax Transform_For_Each_Variable_Statement ([NotNull] ForEachVariableStatementSyntax node) {
            return new PocoForEachVariableStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCommonForEachStatementSyntax Transform_Common_For_Each_Statement ([NotNull] CommonForEachStatementSyntax node) {
            switch(node) {
            case ForEachStatementSyntax _: return Transform_For_Each_Statement((ForEachStatementSyntax)node); 
            case ForEachVariableStatementSyntax _: return Transform_For_Each_Variable_Statement((ForEachVariableStatementSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoUsingStatementSyntax Transform_Using_Statement ([NotNull] UsingStatementSyntax node) {
            return new PocoUsingStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoFixedStatementSyntax Transform_Fixed_Statement ([NotNull] FixedStatementSyntax node) {
            return new PocoFixedStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCheckedStatementSyntax Transform_Checked_Statement ([NotNull] CheckedStatementSyntax node) {
            return new PocoCheckedStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoUnsafeStatementSyntax Transform_Unsafe_Statement ([NotNull] UnsafeStatementSyntax node) {
            return new PocoUnsafeStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLockStatementSyntax Transform_Lock_Statement ([NotNull] LockStatementSyntax node) {
            return new PocoLockStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIfStatementSyntax Transform_If_Statement ([NotNull] IfStatementSyntax node) {
            return new PocoIfStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSwitchStatementSyntax Transform_Switch_Statement ([NotNull] SwitchStatementSyntax node) {
            return new PocoSwitchStatementSyntax() { 
                Sections = new PocoSwitchSectionSyntaxCollection(node?.Sections.Select(Transform_Switch_Section).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTryStatementSyntax Transform_Try_Statement ([NotNull] TryStatementSyntax node) {
            return new PocoTryStatementSyntax() { 
                Catches = new PocoCatchClauseSyntaxCollection(node?.Catches.Select(Transform_Catch_Clause).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoStatementSyntax Transform_Statement ([NotNull] StatementSyntax node) {
            switch(node) {
            case BlockSyntax _: return Transform_Block((BlockSyntax)node); 
            case LocalFunctionStatementSyntax _: return Transform_Local_Function_Statement((LocalFunctionStatementSyntax)node); 
            case LocalDeclarationStatementSyntax _: return Transform_Local_Declaration_Statement((LocalDeclarationStatementSyntax)node); 
            case ExpressionStatementSyntax _: return Transform_Expression_Statement((ExpressionStatementSyntax)node); 
            case EmptyStatementSyntax _: return Transform_Empty_Statement((EmptyStatementSyntax)node); 
            case LabeledStatementSyntax _: return Transform_Labeled_Statement((LabeledStatementSyntax)node); 
            case GotoStatementSyntax _: return Transform_Goto_Statement((GotoStatementSyntax)node); 
            case BreakStatementSyntax _: return Transform_Break_Statement((BreakStatementSyntax)node); 
            case ContinueStatementSyntax _: return Transform_Continue_Statement((ContinueStatementSyntax)node); 
            case ReturnStatementSyntax _: return Transform_Return_Statement((ReturnStatementSyntax)node); 
            case ThrowStatementSyntax _: return Transform_Throw_Statement((ThrowStatementSyntax)node); 
            case YieldStatementSyntax _: return Transform_Yield_Statement((YieldStatementSyntax)node); 
            case WhileStatementSyntax _: return Transform_While_Statement((WhileStatementSyntax)node); 
            case DoStatementSyntax _: return Transform_Do_Statement((DoStatementSyntax)node); 
            case ForStatementSyntax _: return Transform_For_Statement((ForStatementSyntax)node); 
            case ForEachStatementSyntax _: return Transform_For_Each_Statement((ForEachStatementSyntax)node); 
            case ForEachVariableStatementSyntax _: return Transform_For_Each_Variable_Statement((ForEachVariableStatementSyntax)node); 
            case UsingStatementSyntax _: return Transform_Using_Statement((UsingStatementSyntax)node); 
            case FixedStatementSyntax _: return Transform_Fixed_Statement((FixedStatementSyntax)node); 
            case CheckedStatementSyntax _: return Transform_Checked_Statement((CheckedStatementSyntax)node); 
            case UnsafeStatementSyntax _: return Transform_Unsafe_Statement((UnsafeStatementSyntax)node); 
            case LockStatementSyntax _: return Transform_Lock_Statement((LockStatementSyntax)node); 
            case IfStatementSyntax _: return Transform_If_Statement((IfStatementSyntax)node); 
            case SwitchStatementSyntax _: return Transform_Switch_Statement((SwitchStatementSyntax)node); 
            case TryStatementSyntax _: return Transform_Try_Statement((TryStatementSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoVariableDeclarationSyntax Transform_Variable_Declaration ([NotNull] VariableDeclarationSyntax node) {
            return new PocoVariableDeclarationSyntax() { 
                Variables = new PocoVariableDeclaratorSyntaxCollection(node?.Variables.Select(Transform_Variable_Declarator).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoVariableDeclaratorSyntax Transform_Variable_Declarator ([NotNull] VariableDeclaratorSyntax node) {
            return new PocoVariableDeclaratorSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEqualsValueClauseSyntax Transform_Equals_Value_Clause ([NotNull] EqualsValueClauseSyntax node) {
            return new PocoEqualsValueClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSingleVariableDesignationSyntax Transform_Single_Variable_Designation ([NotNull] SingleVariableDesignationSyntax node) {
            return new PocoSingleVariableDesignationSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDiscardDesignationSyntax Transform_Discard_Designation ([NotNull] DiscardDesignationSyntax node) {
            return new PocoDiscardDesignationSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoParenthesizedVariableDesignationSyntax Transform_Parenthesized_Variable_Designation ([NotNull] ParenthesizedVariableDesignationSyntax node) {
            return new PocoParenthesizedVariableDesignationSyntax() { 
                Variables = new PocoVariableDesignationSyntaxCollection(node?.Variables.Select(Transform_Variable_Designation).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoVariableDesignationSyntax Transform_Variable_Designation ([NotNull] VariableDesignationSyntax node) {
            switch(node) {
            case SingleVariableDesignationSyntax _: return Transform_Single_Variable_Designation((SingleVariableDesignationSyntax)node); 
            case DiscardDesignationSyntax _: return Transform_Discard_Designation((DiscardDesignationSyntax)node); 
            case ParenthesizedVariableDesignationSyntax _: return Transform_Parenthesized_Variable_Designation((ParenthesizedVariableDesignationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoElseClauseSyntax Transform_Else_Clause ([NotNull] ElseClauseSyntax node) {
            return new PocoElseClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSwitchSectionSyntax Transform_Switch_Section ([NotNull] SwitchSectionSyntax node) {
            return new PocoSwitchSectionSyntax() { 
                Labels = new PocoSwitchLabelSyntaxCollection(node?.Labels.Select(Transform_Switch_Label).ToList()), 
                Statements = new PocoStatementSyntaxCollection(node?.Statements.Select(Transform_Statement).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCasePatternSwitchLabelSyntax Transform_Case_Pattern_Switch_Label ([NotNull] CasePatternSwitchLabelSyntax node) {
            return new PocoCasePatternSwitchLabelSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCaseSwitchLabelSyntax Transform_Case_Switch_Label ([NotNull] CaseSwitchLabelSyntax node) {
            return new PocoCaseSwitchLabelSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDefaultSwitchLabelSyntax Transform_Default_Switch_Label ([NotNull] DefaultSwitchLabelSyntax node) {
            return new PocoDefaultSwitchLabelSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSwitchLabelSyntax Transform_Switch_Label ([NotNull] SwitchLabelSyntax node) {
            switch(node) {
            case CasePatternSwitchLabelSyntax _: return Transform_Case_Pattern_Switch_Label((CasePatternSwitchLabelSyntax)node); 
            case CaseSwitchLabelSyntax _: return Transform_Case_Switch_Label((CaseSwitchLabelSyntax)node); 
            case DefaultSwitchLabelSyntax _: return Transform_Default_Switch_Label((DefaultSwitchLabelSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSwitchExpressionArmSyntax Transform_Switch_Expression_Arm ([NotNull] SwitchExpressionArmSyntax node) {
            return new PocoSwitchExpressionArmSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCatchClauseSyntax Transform_Catch_Clause ([NotNull] CatchClauseSyntax node) {
            return new PocoCatchClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCatchDeclarationSyntax Transform_Catch_Declaration ([NotNull] CatchDeclarationSyntax node) {
            return new PocoCatchDeclarationSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCatchFilterClauseSyntax Transform_Catch_Filter_Clause ([NotNull] CatchFilterClauseSyntax node) {
            return new PocoCatchFilterClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoFinallyClauseSyntax Transform_Finally_Clause ([NotNull] FinallyClauseSyntax node) {
            return new PocoFinallyClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCompilationUnitSyntax Transform_Compilation_Unit ([NotNull] CompilationUnitSyntax node) {
            return new PocoCompilationUnitSyntax() { 
                Externs = new PocoExternAliasDirectiveSyntaxCollection(node?.Externs.Select(Transform_Extern_Alias_Directive).ToList()), 
                Usings = new PocoUsingDirectiveSyntaxCollection(node?.Usings.Select(Transform_Using_Directive).ToList()), 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                Members = new PocoMemberDeclarationSyntaxCollection(node?.Members.Select(Transform_Member_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoExternAliasDirectiveSyntax Transform_Extern_Alias_Directive ([NotNull] ExternAliasDirectiveSyntax node) {
            return new PocoExternAliasDirectiveSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoUsingDirectiveSyntax Transform_Using_Directive ([NotNull] UsingDirectiveSyntax node) {
            return new PocoUsingDirectiveSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoGlobalStatementSyntax Transform_Global_Statement ([NotNull] GlobalStatementSyntax node) {
            return new PocoGlobalStatementSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNamespaceDeclarationSyntax Transform_Namespace_Declaration ([NotNull] NamespaceDeclarationSyntax node) {
            return new PocoNamespaceDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                Externs = new PocoExternAliasDirectiveSyntaxCollection(node?.Externs.Select(Transform_Extern_Alias_Directive).ToList()), 
                Usings = new PocoUsingDirectiveSyntaxCollection(node?.Usings.Select(Transform_Using_Directive).ToList()), 
                Members = new PocoMemberDeclarationSyntaxCollection(node?.Members.Select(Transform_Member_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoClassDeclarationSyntax Transform_Class_Declaration ([NotNull] ClassDeclarationSyntax node) {
            return new PocoClassDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                ConstraintClauses = new PocoTypeParameterConstraintClauseSyntaxCollection(node?.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList()), 
                Members = new PocoMemberDeclarationSyntaxCollection(node?.Members.Select(Transform_Member_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoStructDeclarationSyntax Transform_Struct_Declaration ([NotNull] StructDeclarationSyntax node) {
            return new PocoStructDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                ConstraintClauses = new PocoTypeParameterConstraintClauseSyntaxCollection(node?.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList()), 
                Members = new PocoMemberDeclarationSyntaxCollection(node?.Members.Select(Transform_Member_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoInterfaceDeclarationSyntax Transform_Interface_Declaration ([NotNull] InterfaceDeclarationSyntax node) {
            return new PocoInterfaceDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                ConstraintClauses = new PocoTypeParameterConstraintClauseSyntaxCollection(node?.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList()), 
                Members = new PocoMemberDeclarationSyntaxCollection(node?.Members.Select(Transform_Member_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeDeclarationSyntax Transform_Type_Declaration ([NotNull] TypeDeclarationSyntax node) {
            switch(node) {
            case ClassDeclarationSyntax _: return Transform_Class_Declaration((ClassDeclarationSyntax)node); 
            case StructDeclarationSyntax _: return Transform_Struct_Declaration((StructDeclarationSyntax)node); 
            case InterfaceDeclarationSyntax _: return Transform_Interface_Declaration((InterfaceDeclarationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEnumDeclarationSyntax Transform_Enum_Declaration ([NotNull] EnumDeclarationSyntax node) {
            return new PocoEnumDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                Members = new PocoEnumMemberDeclarationSyntaxCollection(node?.Members.Select(Transform_Enum_Member_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseTypeDeclarationSyntax Transform_Base_Type_Declaration ([NotNull] BaseTypeDeclarationSyntax node) {
            switch(node) {
            case ClassDeclarationSyntax _: return Transform_Class_Declaration((ClassDeclarationSyntax)node); 
            case StructDeclarationSyntax _: return Transform_Struct_Declaration((StructDeclarationSyntax)node); 
            case InterfaceDeclarationSyntax _: return Transform_Interface_Declaration((InterfaceDeclarationSyntax)node); 
            case EnumDeclarationSyntax _: return Transform_Enum_Declaration((EnumDeclarationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDelegateDeclarationSyntax Transform_Delegate_Declaration ([NotNull] DelegateDeclarationSyntax node) {
            return new PocoDelegateDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                ConstraintClauses = new PocoTypeParameterConstraintClauseSyntaxCollection(node?.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEnumMemberDeclarationSyntax Transform_Enum_Member_Declaration ([NotNull] EnumMemberDeclarationSyntax node) {
            return new PocoEnumMemberDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoFieldDeclarationSyntax Transform_Field_Declaration ([NotNull] FieldDeclarationSyntax node) {
            return new PocoFieldDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEventFieldDeclarationSyntax Transform_Event_Field_Declaration ([NotNull] EventFieldDeclarationSyntax node) {
            return new PocoEventFieldDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseFieldDeclarationSyntax Transform_Base_Field_Declaration ([NotNull] BaseFieldDeclarationSyntax node) {
            switch(node) {
            case FieldDeclarationSyntax _: return Transform_Field_Declaration((FieldDeclarationSyntax)node); 
            case EventFieldDeclarationSyntax _: return Transform_Event_Field_Declaration((EventFieldDeclarationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoMethodDeclarationSyntax Transform_Method_Declaration ([NotNull] MethodDeclarationSyntax node) {
            return new PocoMethodDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()), 
                ConstraintClauses = new PocoTypeParameterConstraintClauseSyntaxCollection(node?.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoOperatorDeclarationSyntax Transform_Operator_Declaration ([NotNull] OperatorDeclarationSyntax node) {
            return new PocoOperatorDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConversionOperatorDeclarationSyntax Transform_Conversion_Operator_Declaration ([NotNull] ConversionOperatorDeclarationSyntax node) {
            return new PocoConversionOperatorDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConstructorDeclarationSyntax Transform_Constructor_Declaration ([NotNull] ConstructorDeclarationSyntax node) {
            return new PocoConstructorDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDestructorDeclarationSyntax Transform_Destructor_Declaration ([NotNull] DestructorDeclarationSyntax node) {
            return new PocoDestructorDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseMethodDeclarationSyntax Transform_Base_Method_Declaration ([NotNull] BaseMethodDeclarationSyntax node) {
            switch(node) {
            case MethodDeclarationSyntax _: return Transform_Method_Declaration((MethodDeclarationSyntax)node); 
            case OperatorDeclarationSyntax _: return Transform_Operator_Declaration((OperatorDeclarationSyntax)node); 
            case ConversionOperatorDeclarationSyntax _: return Transform_Conversion_Operator_Declaration((ConversionOperatorDeclarationSyntax)node); 
            case ConstructorDeclarationSyntax _: return Transform_Constructor_Declaration((ConstructorDeclarationSyntax)node); 
            case DestructorDeclarationSyntax _: return Transform_Destructor_Declaration((DestructorDeclarationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPropertyDeclarationSyntax Transform_Property_Declaration ([NotNull] PropertyDeclarationSyntax node) {
            return new PocoPropertyDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEventDeclarationSyntax Transform_Event_Declaration ([NotNull] EventDeclarationSyntax node) {
            return new PocoEventDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIndexerDeclarationSyntax Transform_Indexer_Declaration ([NotNull] IndexerDeclarationSyntax node) {
            return new PocoIndexerDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBasePropertyDeclarationSyntax Transform_Base_Property_Declaration ([NotNull] BasePropertyDeclarationSyntax node) {
            switch(node) {
            case PropertyDeclarationSyntax _: return Transform_Property_Declaration((PropertyDeclarationSyntax)node); 
            case EventDeclarationSyntax _: return Transform_Event_Declaration((EventDeclarationSyntax)node); 
            case IndexerDeclarationSyntax _: return Transform_Indexer_Declaration((IndexerDeclarationSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIncompleteMemberSyntax Transform_Incomplete_Member ([NotNull] IncompleteMemberSyntax node) {
            return new PocoIncompleteMemberSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoMemberDeclarationSyntax Transform_Member_Declaration ([NotNull] MemberDeclarationSyntax node) {
            switch(node) {
            case GlobalStatementSyntax _: return Transform_Global_Statement((GlobalStatementSyntax)node); 
            case NamespaceDeclarationSyntax _: return Transform_Namespace_Declaration((NamespaceDeclarationSyntax)node); 
            case ClassDeclarationSyntax _: return Transform_Class_Declaration((ClassDeclarationSyntax)node); 
            case StructDeclarationSyntax _: return Transform_Struct_Declaration((StructDeclarationSyntax)node); 
            case InterfaceDeclarationSyntax _: return Transform_Interface_Declaration((InterfaceDeclarationSyntax)node); 
            case EnumDeclarationSyntax _: return Transform_Enum_Declaration((EnumDeclarationSyntax)node); 
            case DelegateDeclarationSyntax _: return Transform_Delegate_Declaration((DelegateDeclarationSyntax)node); 
            case EnumMemberDeclarationSyntax _: return Transform_Enum_Member_Declaration((EnumMemberDeclarationSyntax)node); 
            case FieldDeclarationSyntax _: return Transform_Field_Declaration((FieldDeclarationSyntax)node); 
            case EventFieldDeclarationSyntax _: return Transform_Event_Field_Declaration((EventFieldDeclarationSyntax)node); 
            case MethodDeclarationSyntax _: return Transform_Method_Declaration((MethodDeclarationSyntax)node); 
            case OperatorDeclarationSyntax _: return Transform_Operator_Declaration((OperatorDeclarationSyntax)node); 
            case ConversionOperatorDeclarationSyntax _: return Transform_Conversion_Operator_Declaration((ConversionOperatorDeclarationSyntax)node); 
            case ConstructorDeclarationSyntax _: return Transform_Constructor_Declaration((ConstructorDeclarationSyntax)node); 
            case DestructorDeclarationSyntax _: return Transform_Destructor_Declaration((DestructorDeclarationSyntax)node); 
            case PropertyDeclarationSyntax _: return Transform_Property_Declaration((PropertyDeclarationSyntax)node); 
            case EventDeclarationSyntax _: return Transform_Event_Declaration((EventDeclarationSyntax)node); 
            case IndexerDeclarationSyntax _: return Transform_Indexer_Declaration((IndexerDeclarationSyntax)node); 
            case IncompleteMemberSyntax _: return Transform_Incomplete_Member((IncompleteMemberSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAttributeListSyntax Transform_Attribute_List ([NotNull] AttributeListSyntax node) {
            return new PocoAttributeListSyntax() { 
                Attributes = new PocoAttributeSyntaxCollection(node?.Attributes.Select(Transform_Attribute).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAttributeTargetSpecifierSyntax Transform_Attribute_Target_Specifier ([NotNull] AttributeTargetSpecifierSyntax node) {
            return new PocoAttributeTargetSpecifierSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAttributeSyntax Transform_Attribute ([NotNull] AttributeSyntax node) {
            return new PocoAttributeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAttributeArgumentListSyntax Transform_Attribute_Argument_List ([NotNull] AttributeArgumentListSyntax node) {
            return new PocoAttributeArgumentListSyntax() { 
                Arguments = new PocoAttributeArgumentSyntaxCollection(node?.Arguments.Select(Transform_Attribute_Argument).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAttributeArgumentSyntax Transform_Attribute_Argument ([NotNull] AttributeArgumentSyntax node) {
            return new PocoAttributeArgumentSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNameEqualsSyntax Transform_Name_Equals ([NotNull] NameEqualsSyntax node) {
            return new PocoNameEqualsSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeParameterListSyntax Transform_Type_Parameter_List ([NotNull] TypeParameterListSyntax node) {
            return new PocoTypeParameterListSyntax() { 
                Parameters = new PocoTypeParameterSyntaxCollection(node?.Parameters.Select(Transform_Type_Parameter).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeParameterSyntax Transform_Type_Parameter ([NotNull] TypeParameterSyntax node) {
            return new PocoTypeParameterSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseListSyntax Transform_Base_List ([NotNull] BaseListSyntax node) {
            return new PocoBaseListSyntax() { 
                Types = new PocoBaseTypeSyntaxCollection(node?.Types.Select(Transform_Base_Type).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSimpleBaseTypeSyntax Transform_Simple_Base_Type ([NotNull] SimpleBaseTypeSyntax node) {
            return new PocoSimpleBaseTypeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseTypeSyntax Transform_Base_Type ([NotNull] BaseTypeSyntax node) {
            switch(node) {
            case SimpleBaseTypeSyntax _: return Transform_Simple_Base_Type((SimpleBaseTypeSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeParameterConstraintClauseSyntax Transform_Type_Parameter_Constraint_Clause ([NotNull] TypeParameterConstraintClauseSyntax node) {
            return new PocoTypeParameterConstraintClauseSyntax() { 
                Constraints = new PocoTypeParameterConstraintSyntaxCollection(node?.Constraints.Select(Transform_Type_Parameter_Constraint).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConstructorConstraintSyntax Transform_Constructor_Constraint ([NotNull] ConstructorConstraintSyntax node) {
            return new PocoConstructorConstraintSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoClassOrStructConstraintSyntax Transform_Class_Or_Struct_Constraint ([NotNull] ClassOrStructConstraintSyntax node) {
            return new PocoClassOrStructConstraintSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeConstraintSyntax Transform_Type_Constraint ([NotNull] TypeConstraintSyntax node) {
            return new PocoTypeConstraintSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeParameterConstraintSyntax Transform_Type_Parameter_Constraint ([NotNull] TypeParameterConstraintSyntax node) {
            switch(node) {
            case ConstructorConstraintSyntax _: return Transform_Constructor_Constraint((ConstructorConstraintSyntax)node); 
            case ClassOrStructConstraintSyntax _: return Transform_Class_Or_Struct_Constraint((ClassOrStructConstraintSyntax)node); 
            case TypeConstraintSyntax _: return Transform_Type_Constraint((TypeConstraintSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoExplicitInterfaceSpecifierSyntax Transform_Explicit_Interface_Specifier ([NotNull] ExplicitInterfaceSpecifierSyntax node) {
            return new PocoExplicitInterfaceSpecifierSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConstructorInitializerSyntax Transform_Constructor_Initializer ([NotNull] ConstructorInitializerSyntax node) {
            return new PocoConstructorInitializerSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoArrowExpressionClauseSyntax Transform_Arrow_Expression_Clause ([NotNull] ArrowExpressionClauseSyntax node) {
            return new PocoArrowExpressionClauseSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAccessorListSyntax Transform_Accessor_List ([NotNull] AccessorListSyntax node) {
            return new PocoAccessorListSyntax() { 
                Accessors = new PocoAccessorDeclarationSyntaxCollection(node?.Accessors.Select(Transform_Accessor_Declaration).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoAccessorDeclarationSyntax Transform_Accessor_Declaration ([NotNull] AccessorDeclarationSyntax node) {
            return new PocoAccessorDeclarationSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoParameterListSyntax Transform_Parameter_List ([NotNull] ParameterListSyntax node) {
            return new PocoParameterListSyntax() { 
                Parameters = new PocoParameterSyntaxCollection(node?.Parameters.Select(Transform_Parameter).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBracketedParameterListSyntax Transform_Bracketed_Parameter_List ([NotNull] BracketedParameterListSyntax node) {
            return new PocoBracketedParameterListSyntax() { 
                Parameters = new PocoParameterSyntaxCollection(node?.Parameters.Select(Transform_Parameter).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseParameterListSyntax Transform_Base_Parameter_List ([NotNull] BaseParameterListSyntax node) {
            switch(node) {
            case ParameterListSyntax _: return Transform_Parameter_List((ParameterListSyntax)node); 
            case BracketedParameterListSyntax _: return Transform_Bracketed_Parameter_List((BracketedParameterListSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoParameterSyntax Transform_Parameter ([NotNull] ParameterSyntax node) {
            return new PocoParameterSyntax() { 
                AttributeLists = new PocoAttributeListSyntaxCollection(node?.AttributeLists.Select(Transform_Attribute_List).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoTypeCrefSyntax Transform_Type_Cref ([NotNull] TypeCrefSyntax node) {
            return new PocoTypeCrefSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoQualifiedCrefSyntax Transform_Qualified_Cref ([NotNull] QualifiedCrefSyntax node) {
            return new PocoQualifiedCrefSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNameMemberCrefSyntax Transform_Name_Member_Cref ([NotNull] NameMemberCrefSyntax node) {
            return new PocoNameMemberCrefSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIndexerMemberCrefSyntax Transform_Indexer_Member_Cref ([NotNull] IndexerMemberCrefSyntax node) {
            return new PocoIndexerMemberCrefSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoOperatorMemberCrefSyntax Transform_Operator_Member_Cref ([NotNull] OperatorMemberCrefSyntax node) {
            return new PocoOperatorMemberCrefSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConversionOperatorMemberCrefSyntax Transform_Conversion_Operator_Member_Cref ([NotNull] ConversionOperatorMemberCrefSyntax node) {
            return new PocoConversionOperatorMemberCrefSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoMemberCrefSyntax Transform_Member_Cref ([NotNull] MemberCrefSyntax node) {
            switch(node) {
            case NameMemberCrefSyntax _: return Transform_Name_Member_Cref((NameMemberCrefSyntax)node); 
            case IndexerMemberCrefSyntax _: return Transform_Indexer_Member_Cref((IndexerMemberCrefSyntax)node); 
            case OperatorMemberCrefSyntax _: return Transform_Operator_Member_Cref((OperatorMemberCrefSyntax)node); 
            case ConversionOperatorMemberCrefSyntax _: return Transform_Conversion_Operator_Member_Cref((ConversionOperatorMemberCrefSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCrefSyntax Transform_Cref ([NotNull] CrefSyntax node) {
            switch(node) {
            case TypeCrefSyntax _: return Transform_Type_Cref((TypeCrefSyntax)node); 
            case QualifiedCrefSyntax _: return Transform_Qualified_Cref((QualifiedCrefSyntax)node); 
            case NameMemberCrefSyntax _: return Transform_Name_Member_Cref((NameMemberCrefSyntax)node); 
            case IndexerMemberCrefSyntax _: return Transform_Indexer_Member_Cref((IndexerMemberCrefSyntax)node); 
            case OperatorMemberCrefSyntax _: return Transform_Operator_Member_Cref((OperatorMemberCrefSyntax)node); 
            case ConversionOperatorMemberCrefSyntax _: return Transform_Conversion_Operator_Member_Cref((ConversionOperatorMemberCrefSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCrefParameterListSyntax Transform_Cref_Parameter_List ([NotNull] CrefParameterListSyntax node) {
            return new PocoCrefParameterListSyntax() { 
                Parameters = new PocoCrefParameterSyntaxCollection(node?.Parameters.Select(Transform_Cref_Parameter).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCrefBracketedParameterListSyntax Transform_Cref_Bracketed_Parameter_List ([NotNull] CrefBracketedParameterListSyntax node) {
            return new PocoCrefBracketedParameterListSyntax() { 
                Parameters = new PocoCrefParameterSyntaxCollection(node?.Parameters.Select(Transform_Cref_Parameter).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBaseCrefParameterListSyntax Transform_Base_Cref_Parameter_List ([NotNull] BaseCrefParameterListSyntax node) {
            switch(node) {
            case CrefParameterListSyntax _: return Transform_Cref_Parameter_List((CrefParameterListSyntax)node); 
            case CrefBracketedParameterListSyntax _: return Transform_Cref_Bracketed_Parameter_List((CrefBracketedParameterListSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCrefParameterSyntax Transform_Cref_Parameter ([NotNull] CrefParameterSyntax node) {
            return new PocoCrefParameterSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlElementSyntax Transform_Xml_Element ([NotNull] XmlElementSyntax node) {
            return new PocoXmlElementSyntax() { 
                Content = new PocoXmlNodeSyntaxCollection(node?.Content.Select(Transform_Xml_Node).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlEmptyElementSyntax Transform_Xml_Empty_Element ([NotNull] XmlEmptyElementSyntax node) {
            return new PocoXmlEmptyElementSyntax() { 
                Attributes = new PocoXmlAttributeSyntaxCollection(node?.Attributes.Select(Transform_Xml_Attribute).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlTextSyntax Transform_Xml_Text ([NotNull] XmlTextSyntax node) {
            return new PocoXmlTextSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlCDataSectionSyntax Transform_Xml_CData_Section ([NotNull] XmlCDataSectionSyntax node) {
            return new PocoXmlCDataSectionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlProcessingInstructionSyntax Transform_Xml_Processing_Instruction ([NotNull] XmlProcessingInstructionSyntax node) {
            return new PocoXmlProcessingInstructionSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlCommentSyntax Transform_Xml_Comment ([NotNull] XmlCommentSyntax node) {
            return new PocoXmlCommentSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlNodeSyntax Transform_Xml_Node ([NotNull] XmlNodeSyntax node) {
            switch(node) {
            case XmlElementSyntax _: return Transform_Xml_Element((XmlElementSyntax)node); 
            case XmlEmptyElementSyntax _: return Transform_Xml_Empty_Element((XmlEmptyElementSyntax)node); 
            case XmlTextSyntax _: return Transform_Xml_Text((XmlTextSyntax)node); 
            case XmlCDataSectionSyntax _: return Transform_Xml_CData_Section((XmlCDataSectionSyntax)node); 
            case XmlProcessingInstructionSyntax _: return Transform_Xml_Processing_Instruction((XmlProcessingInstructionSyntax)node); 
            case XmlCommentSyntax _: return Transform_Xml_Comment((XmlCommentSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlElementStartTagSyntax Transform_Xml_Element_Start_Tag ([NotNull] XmlElementStartTagSyntax node) {
            return new PocoXmlElementStartTagSyntax() { 
                Attributes = new PocoXmlAttributeSyntaxCollection(node?.Attributes.Select(Transform_Xml_Attribute).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlElementEndTagSyntax Transform_Xml_Element_End_Tag ([NotNull] XmlElementEndTagSyntax node) {
            return new PocoXmlElementEndTagSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlNameSyntax Transform_Xml_Name ([NotNull] XmlNameSyntax node) {
            return new PocoXmlNameSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlPrefixSyntax Transform_Xml_Prefix ([NotNull] XmlPrefixSyntax node) {
            return new PocoXmlPrefixSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlTextAttributeSyntax Transform_Xml_Text_Attribute ([NotNull] XmlTextAttributeSyntax node) {
            return new PocoXmlTextAttributeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlCrefAttributeSyntax Transform_Xml_Cref_Attribute ([NotNull] XmlCrefAttributeSyntax node) {
            return new PocoXmlCrefAttributeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlNameAttributeSyntax Transform_Xml_Name_Attribute ([NotNull] XmlNameAttributeSyntax node) {
            return new PocoXmlNameAttributeSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoXmlAttributeSyntax Transform_Xml_Attribute ([NotNull] XmlAttributeSyntax node) {
            switch(node) {
            case XmlTextAttributeSyntax _: return Transform_Xml_Text_Attribute((XmlTextAttributeSyntax)node); 
            case XmlCrefAttributeSyntax _: return Transform_Xml_Cref_Attribute((XmlCrefAttributeSyntax)node); 
            case XmlNameAttributeSyntax _: return Transform_Xml_Name_Attribute((XmlNameAttributeSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoSkippedTokensTriviaSyntax Transform_Skipped_Tokens_Trivia ([NotNull] SkippedTokensTriviaSyntax node) {
            return new PocoSkippedTokensTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDocumentationCommentTriviaSyntax Transform_Documentation_Comment_Trivia ([NotNull] DocumentationCommentTriviaSyntax node) {
            return new PocoDocumentationCommentTriviaSyntax() { 
                Content = new PocoXmlNodeSyntaxCollection(node?.Content.Select(Transform_Xml_Node).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoIfDirectiveTriviaSyntax Transform_If_Directive_Trivia ([NotNull] IfDirectiveTriviaSyntax node) {
            return new PocoIfDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoElifDirectiveTriviaSyntax Transform_Elif_Directive_Trivia ([NotNull] ElifDirectiveTriviaSyntax node) {
            return new PocoElifDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoConditionalDirectiveTriviaSyntax Transform_Conditional_Directive_Trivia ([NotNull] ConditionalDirectiveTriviaSyntax node) {
            switch(node) {
            case IfDirectiveTriviaSyntax _: return Transform_If_Directive_Trivia((IfDirectiveTriviaSyntax)node); 
            case ElifDirectiveTriviaSyntax _: return Transform_Elif_Directive_Trivia((ElifDirectiveTriviaSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoElseDirectiveTriviaSyntax Transform_Else_Directive_Trivia ([NotNull] ElseDirectiveTriviaSyntax node) {
            return new PocoElseDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBranchingDirectiveTriviaSyntax Transform_Branching_Directive_Trivia ([NotNull] BranchingDirectiveTriviaSyntax node) {
            switch(node) {
            case IfDirectiveTriviaSyntax _: return Transform_If_Directive_Trivia((IfDirectiveTriviaSyntax)node); 
            case ElifDirectiveTriviaSyntax _: return Transform_Elif_Directive_Trivia((ElifDirectiveTriviaSyntax)node); 
            case ElseDirectiveTriviaSyntax _: return Transform_Else_Directive_Trivia((ElseDirectiveTriviaSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEndIfDirectiveTriviaSyntax Transform_End_If_Directive_Trivia ([NotNull] EndIfDirectiveTriviaSyntax node) {
            return new PocoEndIfDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoRegionDirectiveTriviaSyntax Transform_Region_Directive_Trivia ([NotNull] RegionDirectiveTriviaSyntax node) {
            return new PocoRegionDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoEndRegionDirectiveTriviaSyntax Transform_End_Region_Directive_Trivia ([NotNull] EndRegionDirectiveTriviaSyntax node) {
            return new PocoEndRegionDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoErrorDirectiveTriviaSyntax Transform_Error_Directive_Trivia ([NotNull] ErrorDirectiveTriviaSyntax node) {
            return new PocoErrorDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoWarningDirectiveTriviaSyntax Transform_Warning_Directive_Trivia ([NotNull] WarningDirectiveTriviaSyntax node) {
            return new PocoWarningDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoBadDirectiveTriviaSyntax Transform_Bad_Directive_Trivia ([NotNull] BadDirectiveTriviaSyntax node) {
            return new PocoBadDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDefineDirectiveTriviaSyntax Transform_Define_Directive_Trivia ([NotNull] DefineDirectiveTriviaSyntax node) {
            return new PocoDefineDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoUndefDirectiveTriviaSyntax Transform_Undef_Directive_Trivia ([NotNull] UndefDirectiveTriviaSyntax node) {
            return new PocoUndefDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLineDirectiveTriviaSyntax Transform_Line_Directive_Trivia ([NotNull] LineDirectiveTriviaSyntax node) {
            return new PocoLineDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPragmaWarningDirectiveTriviaSyntax Transform_Pragma_Warning_Directive_Trivia ([NotNull] PragmaWarningDirectiveTriviaSyntax node) {
            return new PocoPragmaWarningDirectiveTriviaSyntax() { 
                ErrorCodes = new PocoExpressionSyntaxCollection(node?.ErrorCodes.Select(Transform_Expression).ToList()),  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoPragmaChecksumDirectiveTriviaSyntax Transform_Pragma_Checksum_Directive_Trivia ([NotNull] PragmaChecksumDirectiveTriviaSyntax node) {
            return new PocoPragmaChecksumDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoReferenceDirectiveTriviaSyntax Transform_Reference_Directive_Trivia ([NotNull] ReferenceDirectiveTriviaSyntax node) {
            return new PocoReferenceDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoLoadDirectiveTriviaSyntax Transform_Load_Directive_Trivia ([NotNull] LoadDirectiveTriviaSyntax node) {
            return new PocoLoadDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoShebangDirectiveTriviaSyntax Transform_Shebang_Directive_Trivia ([NotNull] ShebangDirectiveTriviaSyntax node) {
            return new PocoShebangDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoNullableDirectiveTriviaSyntax Transform_Nullable_Directive_Trivia ([NotNull] NullableDirectiveTriviaSyntax node) {
            return new PocoNullableDirectiveTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoDirectiveTriviaSyntax Transform_Directive_Trivia ([NotNull] DirectiveTriviaSyntax node) {
            switch(node) {
            case IfDirectiveTriviaSyntax _: return Transform_If_Directive_Trivia((IfDirectiveTriviaSyntax)node); 
            case ElifDirectiveTriviaSyntax _: return Transform_Elif_Directive_Trivia((ElifDirectiveTriviaSyntax)node); 
            case ElseDirectiveTriviaSyntax _: return Transform_Else_Directive_Trivia((ElseDirectiveTriviaSyntax)node); 
            case EndIfDirectiveTriviaSyntax _: return Transform_End_If_Directive_Trivia((EndIfDirectiveTriviaSyntax)node); 
            case RegionDirectiveTriviaSyntax _: return Transform_Region_Directive_Trivia((RegionDirectiveTriviaSyntax)node); 
            case EndRegionDirectiveTriviaSyntax _: return Transform_End_Region_Directive_Trivia((EndRegionDirectiveTriviaSyntax)node); 
            case ErrorDirectiveTriviaSyntax _: return Transform_Error_Directive_Trivia((ErrorDirectiveTriviaSyntax)node); 
            case WarningDirectiveTriviaSyntax _: return Transform_Warning_Directive_Trivia((WarningDirectiveTriviaSyntax)node); 
            case BadDirectiveTriviaSyntax _: return Transform_Bad_Directive_Trivia((BadDirectiveTriviaSyntax)node); 
            case DefineDirectiveTriviaSyntax _: return Transform_Define_Directive_Trivia((DefineDirectiveTriviaSyntax)node); 
            case UndefDirectiveTriviaSyntax _: return Transform_Undef_Directive_Trivia((UndefDirectiveTriviaSyntax)node); 
            case LineDirectiveTriviaSyntax _: return Transform_Line_Directive_Trivia((LineDirectiveTriviaSyntax)node); 
            case PragmaWarningDirectiveTriviaSyntax _: return Transform_Pragma_Warning_Directive_Trivia((PragmaWarningDirectiveTriviaSyntax)node); 
            case PragmaChecksumDirectiveTriviaSyntax _: return Transform_Pragma_Checksum_Directive_Trivia((PragmaChecksumDirectiveTriviaSyntax)node); 
            case ReferenceDirectiveTriviaSyntax _: return Transform_Reference_Directive_Trivia((ReferenceDirectiveTriviaSyntax)node); 
            case LoadDirectiveTriviaSyntax _: return Transform_Load_Directive_Trivia((LoadDirectiveTriviaSyntax)node); 
            case ShebangDirectiveTriviaSyntax _: return Transform_Shebang_Directive_Trivia((ShebangDirectiveTriviaSyntax)node); 
            case NullableDirectiveTriviaSyntax _: return Transform_Nullable_Directive_Trivia((NullableDirectiveTriviaSyntax)node); 
            
            }
            return null;
            

        }


        /// <summary></summary>
        [NotNull]
        public static PocoStructuredTriviaSyntax Transform_Structured_Trivia ([NotNull] StructuredTriviaSyntax node) {
            return new PocoStructuredTriviaSyntax() {  };

        }


        /// <summary></summary>
        [NotNull]
        public static PocoCSharpSyntaxNode Transform_CSharp_Node ([NotNull] CSharpSyntaxNode node) {
            return new PocoCSharpSyntaxNode() {  };

        }

}
