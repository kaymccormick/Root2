
using System ;

using System.Linq;
using Microsoft.CodeAnalysis.CSharp ;

using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace FindLogUsages {
/// <summary/>
public static class GenTransforms {

/// <summary></summary>

public static PocoCSharpSyntaxNode Transform_CSharp_Node ( CSharpSyntaxNode node) {
if(node == null) {
return null;
}
	switch(node) {
case TypeArgumentListSyntax _: return Transform_Type_Argument_List((TypeArgumentListSyntax)node); 
case ArrayRankSpecifierSyntax _: return Transform_Array_Rank_Specifier((ArrayRankSpecifierSyntax)node); 
case TupleElementSyntax _: return Transform_Tuple_Element((TupleElementSyntax)node); 
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
case ArgumentListSyntax _: return Transform_Argument_List((ArgumentListSyntax)node); 
case BracketedArgumentListSyntax _: return Transform_Bracketed_Argument_List((BracketedArgumentListSyntax)node); 
case ArgumentSyntax _: return Transform_Argument((ArgumentSyntax)node); 
case NameColonSyntax _: return Transform_Name_Colon((NameColonSyntax)node); 
case AnonymousObjectMemberDeclaratorSyntax _: return Transform_Anonymous_Object_Member_Declarator((AnonymousObjectMemberDeclaratorSyntax)node); 
case FromClauseSyntax _: return Transform_From_Clause((FromClauseSyntax)node); 
case LetClauseSyntax _: return Transform_Let_Clause((LetClauseSyntax)node); 
case JoinClauseSyntax _: return Transform_Join_Clause((JoinClauseSyntax)node); 
case WhereClauseSyntax _: return Transform_Where_Clause((WhereClauseSyntax)node); 
case OrderByClauseSyntax _: return Transform_Order_By_Clause((OrderByClauseSyntax)node); 
case SelectClauseSyntax _: return Transform_Select_Clause((SelectClauseSyntax)node); 
case GroupClauseSyntax _: return Transform_Group_Clause((GroupClauseSyntax)node); 
case QueryBodySyntax _: return Transform_Query_Body((QueryBodySyntax)node); 
case JoinIntoClauseSyntax _: return Transform_Join_Into_Clause((JoinIntoClauseSyntax)node); 
case OrderingSyntax _: return Transform_Ordering((OrderingSyntax)node); 
case QueryContinuationSyntax _: return Transform_Query_Continuation((QueryContinuationSyntax)node); 
case WhenClauseSyntax _: return Transform_When_Clause((WhenClauseSyntax)node); 
case DiscardPatternSyntax _: return Transform_Discard_Pattern((DiscardPatternSyntax)node); 
case DeclarationPatternSyntax _: return Transform_Declaration_Pattern((DeclarationPatternSyntax)node); 
case VarPatternSyntax _: return Transform_Var_Pattern((VarPatternSyntax)node); 
case RecursivePatternSyntax _: return Transform_Recursive_Pattern((RecursivePatternSyntax)node); 
case ConstantPatternSyntax _: return Transform_Constant_Pattern((ConstantPatternSyntax)node); 
case PositionalPatternClauseSyntax _: return Transform_Positional_Pattern_Clause((PositionalPatternClauseSyntax)node); 
case PropertyPatternClauseSyntax _: return Transform_Property_Pattern_Clause((PropertyPatternClauseSyntax)node); 
case SubpatternSyntax _: return Transform_Subpattern((SubpatternSyntax)node); 
case InterpolatedStringTextSyntax _: return Transform_Interpolated_String_Text((InterpolatedStringTextSyntax)node); 
case InterpolationSyntax _: return Transform_Interpolation((InterpolationSyntax)node); 
case InterpolationAlignmentClauseSyntax _: return Transform_Interpolation_Alignment_Clause((InterpolationAlignmentClauseSyntax)node); 
case InterpolationFormatClauseSyntax _: return Transform_Interpolation_Format_Clause((InterpolationFormatClauseSyntax)node); 
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
case VariableDeclarationSyntax _: return Transform_Variable_Declaration((VariableDeclarationSyntax)node); 
case VariableDeclaratorSyntax _: return Transform_Variable_Declarator((VariableDeclaratorSyntax)node); 
case EqualsValueClauseSyntax _: return Transform_Equals_Value_Clause((EqualsValueClauseSyntax)node); 
case SingleVariableDesignationSyntax _: return Transform_Single_Variable_Designation((SingleVariableDesignationSyntax)node); 
case DiscardDesignationSyntax _: return Transform_Discard_Designation((DiscardDesignationSyntax)node); 
case ParenthesizedVariableDesignationSyntax _: return Transform_Parenthesized_Variable_Designation((ParenthesizedVariableDesignationSyntax)node); 
case ElseClauseSyntax _: return Transform_Else_Clause((ElseClauseSyntax)node); 
case SwitchSectionSyntax _: return Transform_Switch_Section((SwitchSectionSyntax)node); 
case CasePatternSwitchLabelSyntax _: return Transform_Case_Pattern_Switch_Label((CasePatternSwitchLabelSyntax)node); 
case CaseSwitchLabelSyntax _: return Transform_Case_Switch_Label((CaseSwitchLabelSyntax)node); 
case DefaultSwitchLabelSyntax _: return Transform_Default_Switch_Label((DefaultSwitchLabelSyntax)node); 
case SwitchExpressionArmSyntax _: return Transform_Switch_Expression_Arm((SwitchExpressionArmSyntax)node); 
case CatchClauseSyntax _: return Transform_Catch_Clause((CatchClauseSyntax)node); 
case CatchDeclarationSyntax _: return Transform_Catch_Declaration((CatchDeclarationSyntax)node); 
case CatchFilterClauseSyntax _: return Transform_Catch_Filter_Clause((CatchFilterClauseSyntax)node); 
case FinallyClauseSyntax _: return Transform_Finally_Clause((FinallyClauseSyntax)node); 
case CompilationUnitSyntax _: return Transform_Compilation_Unit((CompilationUnitSyntax)node); 
case ExternAliasDirectiveSyntax _: return Transform_Extern_Alias_Directive((ExternAliasDirectiveSyntax)node); 
case UsingDirectiveSyntax _: return Transform_Using_Directive((UsingDirectiveSyntax)node); 
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
case AttributeListSyntax _: return Transform_Attribute_List((AttributeListSyntax)node); 
case AttributeTargetSpecifierSyntax _: return Transform_Attribute_Target_Specifier((AttributeTargetSpecifierSyntax)node); 
case AttributeSyntax _: return Transform_Attribute((AttributeSyntax)node); 
case AttributeArgumentListSyntax _: return Transform_Attribute_Argument_List((AttributeArgumentListSyntax)node); 
case AttributeArgumentSyntax _: return Transform_Attribute_Argument((AttributeArgumentSyntax)node); 
case NameEqualsSyntax _: return Transform_Name_Equals((NameEqualsSyntax)node); 
case TypeParameterListSyntax _: return Transform_Type_Parameter_List((TypeParameterListSyntax)node); 
case TypeParameterSyntax _: return Transform_Type_Parameter((TypeParameterSyntax)node); 
case BaseListSyntax _: return Transform_Base_List((BaseListSyntax)node); 
case SimpleBaseTypeSyntax _: return Transform_Simple_Base_Type((SimpleBaseTypeSyntax)node); 
case TypeParameterConstraintClauseSyntax _: return Transform_Type_Parameter_Constraint_Clause((TypeParameterConstraintClauseSyntax)node); 
case ConstructorConstraintSyntax _: return Transform_Constructor_Constraint((ConstructorConstraintSyntax)node); 
case ClassOrStructConstraintSyntax _: return Transform_Class_Or_Struct_Constraint((ClassOrStructConstraintSyntax)node); 
case TypeConstraintSyntax _: return Transform_Type_Constraint((TypeConstraintSyntax)node); 
case ExplicitInterfaceSpecifierSyntax _: return Transform_Explicit_Interface_Specifier((ExplicitInterfaceSpecifierSyntax)node); 
case ConstructorInitializerSyntax _: return Transform_Constructor_Initializer((ConstructorInitializerSyntax)node); 
case ArrowExpressionClauseSyntax _: return Transform_Arrow_Expression_Clause((ArrowExpressionClauseSyntax)node); 
case AccessorListSyntax _: return Transform_Accessor_List((AccessorListSyntax)node); 
case AccessorDeclarationSyntax _: return Transform_Accessor_Declaration((AccessorDeclarationSyntax)node); 
case ParameterListSyntax _: return Transform_Parameter_List((ParameterListSyntax)node); 
case BracketedParameterListSyntax _: return Transform_Bracketed_Parameter_List((BracketedParameterListSyntax)node); 
case ParameterSyntax _: return Transform_Parameter((ParameterSyntax)node); 
case TypeCrefSyntax _: return Transform_Type_Cref((TypeCrefSyntax)node); 
case QualifiedCrefSyntax _: return Transform_Qualified_Cref((QualifiedCrefSyntax)node); 
case NameMemberCrefSyntax _: return Transform_Name_Member_Cref((NameMemberCrefSyntax)node); 
case IndexerMemberCrefSyntax _: return Transform_Indexer_Member_Cref((IndexerMemberCrefSyntax)node); 
case OperatorMemberCrefSyntax _: return Transform_Operator_Member_Cref((OperatorMemberCrefSyntax)node); 
case ConversionOperatorMemberCrefSyntax _: return Transform_Conversion_Operator_Member_Cref((ConversionOperatorMemberCrefSyntax)node); 
case CrefParameterListSyntax _: return Transform_Cref_Parameter_List((CrefParameterListSyntax)node); 
case CrefBracketedParameterListSyntax _: return Transform_Cref_Bracketed_Parameter_List((CrefBracketedParameterListSyntax)node); 
case CrefParameterSyntax _: return Transform_Cref_Parameter((CrefParameterSyntax)node); 
case XmlElementSyntax _: return Transform_Xml_Element((XmlElementSyntax)node); 
case XmlEmptyElementSyntax _: return Transform_Xml_Empty_Element((XmlEmptyElementSyntax)node); 
case XmlTextSyntax _: return Transform_Xml_Text((XmlTextSyntax)node); 
case XmlCDataSectionSyntax _: return Transform_Xml_CData_Section((XmlCDataSectionSyntax)node); 
case XmlProcessingInstructionSyntax _: return Transform_Xml_Processing_Instruction((XmlProcessingInstructionSyntax)node); 
case XmlCommentSyntax _: return Transform_Xml_Comment((XmlCommentSyntax)node); 
case XmlElementStartTagSyntax _: return Transform_Xml_Element_Start_Tag((XmlElementStartTagSyntax)node); 
case XmlElementEndTagSyntax _: return Transform_Xml_Element_End_Tag((XmlElementEndTagSyntax)node); 
case XmlNameSyntax _: return Transform_Xml_Name((XmlNameSyntax)node); 
case XmlPrefixSyntax _: return Transform_Xml_Prefix((XmlPrefixSyntax)node); 
case XmlTextAttributeSyntax _: return Transform_Xml_Text_Attribute((XmlTextAttributeSyntax)node); 
case XmlCrefAttributeSyntax _: return Transform_Xml_Cref_Attribute((XmlCrefAttributeSyntax)node); 
case XmlNameAttributeSyntax _: return Transform_Xml_Name_Attribute((XmlNameAttributeSyntax)node); 
case SkippedTokensTriviaSyntax _: return Transform_Skipped_Tokens_Trivia((SkippedTokensTriviaSyntax)node); 
case DocumentationCommentTriviaSyntax _: return Transform_Documentation_Comment_Trivia((DocumentationCommentTriviaSyntax)node); 
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

public static PocoAccessorDeclarationSyntax Transform_Accessor_Declaration ( AccessorDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoAccessorDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoAccessorListSyntax Transform_Accessor_List ( AccessorListSyntax node) {
if(node == null) {
return null;
}
	return new PocoAccessorListSyntax() { 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Accessors = node.Accessors.Select(Transform_Accessor_Declaration).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoAliasQualifiedNameSyntax Transform_Alias_Qualified_Name ( AliasQualifiedNameSyntax node) {
if(node == null) {
return null;
}
	return new PocoAliasQualifiedNameSyntax() { 
Alias = Transform_Identifier_Name(node.Alias), 
ColonColonToken = new PocoSyntaxToken {RawKind = node.ColonColonToken.RawKind, Kind = node.ColonColonToken.Kind().ToString(), Value = node.ColonColonToken.Value, ValueText = node.ColonColonToken.ValueText }, 
Name = Transform_Simple_Name(node.Name),  };

}


/// <summary></summary>

public static PocoAnonymousFunctionExpressionSyntax Transform_Anonymous_Function_Expression ( AnonymousFunctionExpressionSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case AnonymousMethodExpressionSyntax _: return Transform_Anonymous_Method_Expression((AnonymousMethodExpressionSyntax)node); 
case SimpleLambdaExpressionSyntax _: return Transform_Simple_Lambda_Expression((SimpleLambdaExpressionSyntax)node); 
case ParenthesizedLambdaExpressionSyntax _: return Transform_Parenthesized_Lambda_Expression((ParenthesizedLambdaExpressionSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoAnonymousMethodExpressionSyntax Transform_Anonymous_Method_Expression ( AnonymousMethodExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoAnonymousMethodExpressionSyntax() { 
AsyncKeyword = new PocoSyntaxToken {RawKind = node.AsyncKeyword.RawKind, Kind = node.AsyncKeyword.Kind().ToString(), Value = node.AsyncKeyword.Value, ValueText = node.AsyncKeyword.ValueText }, 
DelegateKeyword = new PocoSyntaxToken {RawKind = node.DelegateKeyword.RawKind, Kind = node.DelegateKeyword.Kind().ToString(), Value = node.DelegateKeyword.Value, ValueText = node.DelegateKeyword.ValueText }, 
ParameterList = Transform_Parameter_List(node.ParameterList), 
Block = Transform_Block(node.Block), 
ExpressionBody = Transform_Expression(node.ExpressionBody),  };

}


/// <summary></summary>

public static PocoAnonymousObjectCreationExpressionSyntax Transform_Anonymous_Object_Creation_Expression ( AnonymousObjectCreationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoAnonymousObjectCreationExpressionSyntax() { 
NewKeyword = new PocoSyntaxToken {RawKind = node.NewKeyword.RawKind, Kind = node.NewKeyword.Kind().ToString(), Value = node.NewKeyword.Value, ValueText = node.NewKeyword.ValueText }, 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Initializers = node.Initializers.Select(Transform_Anonymous_Object_Member_Declarator).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoAnonymousObjectMemberDeclaratorSyntax Transform_Anonymous_Object_Member_Declarator ( AnonymousObjectMemberDeclaratorSyntax node) {
if(node == null) {
return null;
}
	return new PocoAnonymousObjectMemberDeclaratorSyntax() { 
NameEquals = Transform_Name_Equals(node.NameEquals), 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoArgumentListSyntax Transform_Argument_List ( ArgumentListSyntax node) {
if(node == null) {
return null;
}
	return new PocoArgumentListSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Arguments = node.Arguments.Select(Transform_Argument).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoArgumentSyntax Transform_Argument ( ArgumentSyntax node) {
if(node == null) {
return null;
}
	return new PocoArgumentSyntax() { 
NameColon = Transform_Name_Colon(node.NameColon), 
RefKindKeyword = new PocoSyntaxToken {RawKind = node.RefKindKeyword.RawKind, Kind = node.RefKindKeyword.Kind().ToString(), Value = node.RefKindKeyword.Value, ValueText = node.RefKindKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoArrayCreationExpressionSyntax Transform_Array_Creation_Expression ( ArrayCreationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoArrayCreationExpressionSyntax() { 
NewKeyword = new PocoSyntaxToken {RawKind = node.NewKeyword.RawKind, Kind = node.NewKeyword.Kind().ToString(), Value = node.NewKeyword.Value, ValueText = node.NewKeyword.ValueText }, 
Type = Transform_Array_Type(node.Type), 
Initializer = Transform_Initializer_Expression(node.Initializer),  };

}


/// <summary></summary>

public static PocoArrayRankSpecifierSyntax Transform_Array_Rank_Specifier ( ArrayRankSpecifierSyntax node) {
if(node == null) {
return null;
}
	return new PocoArrayRankSpecifierSyntax() { 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
Sizes = node.Sizes.Select(Transform_Expression).ToList(), 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText },  };

}


/// <summary></summary>

public static PocoArrayTypeSyntax Transform_Array_Type ( ArrayTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoArrayTypeSyntax() { 
ElementType = Transform_Type(node.ElementType), 
RankSpecifiers = node.RankSpecifiers.Select(Transform_Array_Rank_Specifier).ToList(),  };

}


/// <summary></summary>

public static PocoArrowExpressionClauseSyntax Transform_Arrow_Expression_Clause ( ArrowExpressionClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoArrowExpressionClauseSyntax() { 
ArrowToken = new PocoSyntaxToken {RawKind = node.ArrowToken.RawKind, Kind = node.ArrowToken.Kind().ToString(), Value = node.ArrowToken.Value, ValueText = node.ArrowToken.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoAssignmentExpressionSyntax Transform_Assignment_Expression ( AssignmentExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoAssignmentExpressionSyntax() { 
Left = Transform_Expression(node.Left), 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
Right = Transform_Expression(node.Right),  };

}


/// <summary></summary>

public static PocoAttributeArgumentListSyntax Transform_Attribute_Argument_List ( AttributeArgumentListSyntax node) {
if(node == null) {
return null;
}
	return new PocoAttributeArgumentListSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Arguments = node.Arguments.Select(Transform_Attribute_Argument).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoAttributeArgumentSyntax Transform_Attribute_Argument ( AttributeArgumentSyntax node) {
if(node == null) {
return null;
}
	return new PocoAttributeArgumentSyntax() { 
Expression = Transform_Expression(node.Expression), 
NameEquals = Transform_Name_Equals(node.NameEquals), 
NameColon = Transform_Name_Colon(node.NameColon),  };

}


/// <summary></summary>

public static PocoAttributeListSyntax Transform_Attribute_List ( AttributeListSyntax node) {
if(node == null) {
return null;
}
	return new PocoAttributeListSyntax() { 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
Target = Transform_Attribute_Target_Specifier(node.Target), 
Attributes = node.Attributes.Select(Transform_Attribute).ToList(), 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText },  };

}


/// <summary></summary>

public static PocoAttributeSyntax Transform_Attribute ( AttributeSyntax node) {
if(node == null) {
return null;
}
	return new PocoAttributeSyntax() { 
Name = Transform_Name(node.Name), 
ArgumentList = Transform_Attribute_Argument_List(node.ArgumentList),  };

}


/// <summary></summary>

public static PocoAttributeTargetSpecifierSyntax Transform_Attribute_Target_Specifier ( AttributeTargetSpecifierSyntax node) {
if(node == null) {
return null;
}
	return new PocoAttributeTargetSpecifierSyntax() { 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText },  };

}


/// <summary></summary>

public static PocoAwaitExpressionSyntax Transform_Await_Expression ( AwaitExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoAwaitExpressionSyntax() { 
AwaitKeyword = new PocoSyntaxToken {RawKind = node.AwaitKeyword.RawKind, Kind = node.AwaitKeyword.Kind().ToString(), Value = node.AwaitKeyword.Value, ValueText = node.AwaitKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoBadDirectiveTriviaSyntax Transform_Bad_Directive_Trivia ( BadDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoBadDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoBaseArgumentListSyntax Transform_Base_Argument_List ( BaseArgumentListSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ArgumentListSyntax _: return Transform_Argument_List((ArgumentListSyntax)node); 
case BracketedArgumentListSyntax _: return Transform_Bracketed_Argument_List((BracketedArgumentListSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBaseCrefParameterListSyntax Transform_Base_Cref_Parameter_List ( BaseCrefParameterListSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case CrefParameterListSyntax _: return Transform_Cref_Parameter_List((CrefParameterListSyntax)node); 
case CrefBracketedParameterListSyntax _: return Transform_Cref_Bracketed_Parameter_List((CrefBracketedParameterListSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBaseExpressionSyntax Transform_Base_Expression ( BaseExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoBaseExpressionSyntax() { 
Token = new PocoSyntaxToken {RawKind = node.Token.RawKind, Kind = node.Token.Kind().ToString(), Value = node.Token.Value, ValueText = node.Token.ValueText },  };

}


/// <summary></summary>

public static PocoBaseFieldDeclarationSyntax Transform_Base_Field_Declaration ( BaseFieldDeclarationSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case FieldDeclarationSyntax _: return Transform_Field_Declaration((FieldDeclarationSyntax)node); 
case EventFieldDeclarationSyntax _: return Transform_Event_Field_Declaration((EventFieldDeclarationSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBaseListSyntax Transform_Base_List ( BaseListSyntax node) {
if(node == null) {
return null;
}
	return new PocoBaseListSyntax() { 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText }, 
Types = node.Types.Select(Transform_Base_Type).ToList(),  };

}


/// <summary></summary>

public static PocoBaseMethodDeclarationSyntax Transform_Base_Method_Declaration ( BaseMethodDeclarationSyntax node) {
if(node == null) {
return null;
}
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

public static PocoBaseParameterListSyntax Transform_Base_Parameter_List ( BaseParameterListSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ParameterListSyntax _: return Transform_Parameter_List((ParameterListSyntax)node); 
case BracketedParameterListSyntax _: return Transform_Bracketed_Parameter_List((BracketedParameterListSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBasePropertyDeclarationSyntax Transform_Base_Property_Declaration ( BasePropertyDeclarationSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case PropertyDeclarationSyntax _: return Transform_Property_Declaration((PropertyDeclarationSyntax)node); 
case EventDeclarationSyntax _: return Transform_Event_Declaration((EventDeclarationSyntax)node); 
case IndexerDeclarationSyntax _: return Transform_Indexer_Declaration((IndexerDeclarationSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBaseTypeDeclarationSyntax Transform_Base_Type_Declaration ( BaseTypeDeclarationSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ClassDeclarationSyntax _: return Transform_Class_Declaration((ClassDeclarationSyntax)node); 
case StructDeclarationSyntax _: return Transform_Struct_Declaration((StructDeclarationSyntax)node); 
case InterfaceDeclarationSyntax _: return Transform_Interface_Declaration((InterfaceDeclarationSyntax)node); 
case EnumDeclarationSyntax _: return Transform_Enum_Declaration((EnumDeclarationSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBaseTypeSyntax Transform_Base_Type ( BaseTypeSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case SimpleBaseTypeSyntax _: return Transform_Simple_Base_Type((SimpleBaseTypeSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBinaryExpressionSyntax Transform_Binary_Expression ( BinaryExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoBinaryExpressionSyntax() { 
Left = Transform_Expression(node.Left), 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
Right = Transform_Expression(node.Right),  };

}


/// <summary></summary>

public static PocoBlockSyntax Transform_Block ( BlockSyntax node) {
if(node == null) {
return null;
}
	return new PocoBlockSyntax() { 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Statements = node.Statements.Select(Transform_Statement).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoBracketedArgumentListSyntax Transform_Bracketed_Argument_List ( BracketedArgumentListSyntax node) {
if(node == null) {
return null;
}
	return new PocoBracketedArgumentListSyntax() { 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
Arguments = node.Arguments.Select(Transform_Argument).ToList(), 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText },  };

}


/// <summary></summary>

public static PocoBracketedParameterListSyntax Transform_Bracketed_Parameter_List ( BracketedParameterListSyntax node) {
if(node == null) {
return null;
}
	return new PocoBracketedParameterListSyntax() { 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
Parameters = node.Parameters.Select(Transform_Parameter).ToList(), 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText },  };

}


/// <summary></summary>

public static PocoBranchingDirectiveTriviaSyntax Transform_Branching_Directive_Trivia ( BranchingDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case IfDirectiveTriviaSyntax _: return Transform_If_Directive_Trivia((IfDirectiveTriviaSyntax)node); 
case ElifDirectiveTriviaSyntax _: return Transform_Elif_Directive_Trivia((ElifDirectiveTriviaSyntax)node); 
case ElseDirectiveTriviaSyntax _: return Transform_Else_Directive_Trivia((ElseDirectiveTriviaSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoBreakStatementSyntax Transform_Break_Statement ( BreakStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoBreakStatementSyntax() { 
BreakKeyword = new PocoSyntaxToken {RawKind = node.BreakKeyword.RawKind, Kind = node.BreakKeyword.Kind().ToString(), Value = node.BreakKeyword.Value, ValueText = node.BreakKeyword.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoCasePatternSwitchLabelSyntax Transform_Case_Pattern_Switch_Label ( CasePatternSwitchLabelSyntax node) {
if(node == null) {
return null;
}
	return new PocoCasePatternSwitchLabelSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Pattern = Transform_Pattern(node.Pattern), 
WhenClause = Transform_When_Clause(node.WhenClause), 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText },  };

}


/// <summary></summary>

public static PocoCaseSwitchLabelSyntax Transform_Case_Switch_Label ( CaseSwitchLabelSyntax node) {
if(node == null) {
return null;
}
	return new PocoCaseSwitchLabelSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Value = Transform_Expression(node.Value), 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText },  };

}


/// <summary></summary>

public static PocoCastExpressionSyntax Transform_Cast_Expression ( CastExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoCastExpressionSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Type = Transform_Type(node.Type), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoCatchClauseSyntax Transform_Catch_Clause ( CatchClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoCatchClauseSyntax() { 
CatchKeyword = new PocoSyntaxToken {RawKind = node.CatchKeyword.RawKind, Kind = node.CatchKeyword.Kind().ToString(), Value = node.CatchKeyword.Value, ValueText = node.CatchKeyword.ValueText }, 
Declaration = Transform_Catch_Declaration(node.Declaration), 
Filter = Transform_Catch_Filter_Clause(node.Filter), 
Block = Transform_Block(node.Block),  };

}


/// <summary></summary>

public static PocoCatchDeclarationSyntax Transform_Catch_Declaration ( CatchDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoCatchDeclarationSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Type = Transform_Type(node.Type), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoCatchFilterClauseSyntax Transform_Catch_Filter_Clause ( CatchFilterClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoCatchFilterClauseSyntax() { 
WhenKeyword = new PocoSyntaxToken {RawKind = node.WhenKeyword.RawKind, Kind = node.WhenKeyword.Kind().ToString(), Value = node.WhenKeyword.Value, ValueText = node.WhenKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
FilterExpression = Transform_Expression(node.FilterExpression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoCheckedExpressionSyntax Transform_Checked_Expression ( CheckedExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoCheckedExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoCheckedStatementSyntax Transform_Checked_Statement ( CheckedStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoCheckedStatementSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Block = Transform_Block(node.Block),  };

}


/// <summary></summary>

public static PocoClassDeclarationSyntax Transform_Class_Declaration ( ClassDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoClassDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeParameterList = Transform_Type_Parameter_List(node.TypeParameterList), 
BaseList = Transform_Base_List(node.BaseList), 
ConstraintClauses = node.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList(), 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Members = node.Members.Select(Transform_Member_Declaration).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoClassOrStructConstraintSyntax Transform_Class_Or_Struct_Constraint ( ClassOrStructConstraintSyntax node) {
if(node == null) {
return null;
}
	return new PocoClassOrStructConstraintSyntax() { 
ClassOrStructKeyword = new PocoSyntaxToken {RawKind = node.ClassOrStructKeyword.RawKind, Kind = node.ClassOrStructKeyword.Kind().ToString(), Value = node.ClassOrStructKeyword.Value, ValueText = node.ClassOrStructKeyword.ValueText }, 
QuestionToken = new PocoSyntaxToken {RawKind = node.QuestionToken.RawKind, Kind = node.QuestionToken.Kind().ToString(), Value = node.QuestionToken.Value, ValueText = node.QuestionToken.ValueText },  };

}


/// <summary></summary>

public static PocoCommonForEachStatementSyntax Transform_Common_For_Each_Statement ( CommonForEachStatementSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ForEachStatementSyntax _: return Transform_For_Each_Statement((ForEachStatementSyntax)node); 
case ForEachVariableStatementSyntax _: return Transform_For_Each_Variable_Statement((ForEachVariableStatementSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoCompilationUnitSyntax Transform_Compilation_Unit ( CompilationUnitSyntax node) {
if(node == null) {
return null;
}
	return new PocoCompilationUnitSyntax() { 
Externs = node.Externs.Select(Transform_Extern_Alias_Directive).ToList(), 
Usings = node.Usings.Select(Transform_Using_Directive).ToList(), 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Members = node.Members.Select(Transform_Member_Declaration).ToList(), 
EndOfFileToken = new PocoSyntaxToken {RawKind = node.EndOfFileToken.RawKind, Kind = node.EndOfFileToken.Kind().ToString(), Value = node.EndOfFileToken.Value, ValueText = node.EndOfFileToken.ValueText },  };

}


/// <summary></summary>

public static PocoConditionalAccessExpressionSyntax Transform_Conditional_Access_Expression ( ConditionalAccessExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoConditionalAccessExpressionSyntax() { 
Expression = Transform_Expression(node.Expression), 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
WhenNotNull = Transform_Expression(node.WhenNotNull),  };

}


/// <summary></summary>

public static PocoConditionalDirectiveTriviaSyntax Transform_Conditional_Directive_Trivia ( ConditionalDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case IfDirectiveTriviaSyntax _: return Transform_If_Directive_Trivia((IfDirectiveTriviaSyntax)node); 
case ElifDirectiveTriviaSyntax _: return Transform_Elif_Directive_Trivia((ElifDirectiveTriviaSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoConditionalExpressionSyntax Transform_Conditional_Expression ( ConditionalExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoConditionalExpressionSyntax() { 
Condition = Transform_Expression(node.Condition), 
QuestionToken = new PocoSyntaxToken {RawKind = node.QuestionToken.RawKind, Kind = node.QuestionToken.Kind().ToString(), Value = node.QuestionToken.Value, ValueText = node.QuestionToken.ValueText }, 
WhenTrue = Transform_Expression(node.WhenTrue), 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText }, 
WhenFalse = Transform_Expression(node.WhenFalse),  };

}


/// <summary></summary>

public static PocoConstantPatternSyntax Transform_Constant_Pattern ( ConstantPatternSyntax node) {
if(node == null) {
return null;
}
	return new PocoConstantPatternSyntax() { 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoConstructorConstraintSyntax Transform_Constructor_Constraint ( ConstructorConstraintSyntax node) {
if(node == null) {
return null;
}
	return new PocoConstructorConstraintSyntax() { 
NewKeyword = new PocoSyntaxToken {RawKind = node.NewKeyword.RawKind, Kind = node.NewKeyword.Kind().ToString(), Value = node.NewKeyword.Value, ValueText = node.NewKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoConstructorDeclarationSyntax Transform_Constructor_Declaration ( ConstructorDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoConstructorDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
ParameterList = Transform_Parameter_List(node.ParameterList), 
Initializer = Transform_Constructor_Initializer(node.Initializer), 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoConstructorInitializerSyntax Transform_Constructor_Initializer ( ConstructorInitializerSyntax node) {
if(node == null) {
return null;
}
	return new PocoConstructorInitializerSyntax() { 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText }, 
ThisOrBaseKeyword = new PocoSyntaxToken {RawKind = node.ThisOrBaseKeyword.RawKind, Kind = node.ThisOrBaseKeyword.Kind().ToString(), Value = node.ThisOrBaseKeyword.Value, ValueText = node.ThisOrBaseKeyword.ValueText }, 
ArgumentList = Transform_Argument_List(node.ArgumentList),  };

}


/// <summary></summary>

public static PocoContinueStatementSyntax Transform_Continue_Statement ( ContinueStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoContinueStatementSyntax() { 
ContinueKeyword = new PocoSyntaxToken {RawKind = node.ContinueKeyword.RawKind, Kind = node.ContinueKeyword.Kind().ToString(), Value = node.ContinueKeyword.Value, ValueText = node.ContinueKeyword.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoConversionOperatorDeclarationSyntax Transform_Conversion_Operator_Declaration ( ConversionOperatorDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoConversionOperatorDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
ImplicitOrExplicitKeyword = new PocoSyntaxToken {RawKind = node.ImplicitOrExplicitKeyword.RawKind, Kind = node.ImplicitOrExplicitKeyword.Kind().ToString(), Value = node.ImplicitOrExplicitKeyword.Value, ValueText = node.ImplicitOrExplicitKeyword.ValueText }, 
OperatorKeyword = new PocoSyntaxToken {RawKind = node.OperatorKeyword.RawKind, Kind = node.OperatorKeyword.Kind().ToString(), Value = node.OperatorKeyword.Value, ValueText = node.OperatorKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
ParameterList = Transform_Parameter_List(node.ParameterList), 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoConversionOperatorMemberCrefSyntax Transform_Conversion_Operator_Member_Cref ( ConversionOperatorMemberCrefSyntax node) {
if(node == null) {
return null;
}
	return new PocoConversionOperatorMemberCrefSyntax() { 
ImplicitOrExplicitKeyword = new PocoSyntaxToken {RawKind = node.ImplicitOrExplicitKeyword.RawKind, Kind = node.ImplicitOrExplicitKeyword.Kind().ToString(), Value = node.ImplicitOrExplicitKeyword.Value, ValueText = node.ImplicitOrExplicitKeyword.ValueText }, 
OperatorKeyword = new PocoSyntaxToken {RawKind = node.OperatorKeyword.RawKind, Kind = node.OperatorKeyword.Kind().ToString(), Value = node.OperatorKeyword.Value, ValueText = node.OperatorKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
Parameters = Transform_Cref_Parameter_List(node.Parameters),  };

}


/// <summary></summary>

public static PocoCrefBracketedParameterListSyntax Transform_Cref_Bracketed_Parameter_List ( CrefBracketedParameterListSyntax node) {
if(node == null) {
return null;
}
	return new PocoCrefBracketedParameterListSyntax() { 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
Parameters = node.Parameters.Select(Transform_Cref_Parameter).ToList(), 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText },  };

}


/// <summary></summary>

public static PocoCrefParameterListSyntax Transform_Cref_Parameter_List ( CrefParameterListSyntax node) {
if(node == null) {
return null;
}
	return new PocoCrefParameterListSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Parameters = node.Parameters.Select(Transform_Cref_Parameter).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoCrefParameterSyntax Transform_Cref_Parameter ( CrefParameterSyntax node) {
if(node == null) {
return null;
}
	return new PocoCrefParameterSyntax() { 
RefKindKeyword = new PocoSyntaxToken {RawKind = node.RefKindKeyword.RawKind, Kind = node.RefKindKeyword.Kind().ToString(), Value = node.RefKindKeyword.Value, ValueText = node.RefKindKeyword.ValueText }, 
Type = Transform_Type(node.Type),  };

}


/// <summary></summary>

public static PocoCrefSyntax Transform_Cref ( CrefSyntax node) {
if(node == null) {
return null;
}
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

public static PocoDeclarationExpressionSyntax Transform_Declaration_Expression ( DeclarationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoDeclarationExpressionSyntax() { 
Type = Transform_Type(node.Type), 
Designation = Transform_Variable_Designation(node.Designation),  };

}


/// <summary></summary>

public static PocoDeclarationPatternSyntax Transform_Declaration_Pattern ( DeclarationPatternSyntax node) {
if(node == null) {
return null;
}
	return new PocoDeclarationPatternSyntax() { 
Type = Transform_Type(node.Type), 
Designation = Transform_Variable_Designation(node.Designation),  };

}


/// <summary></summary>

public static PocoDefaultExpressionSyntax Transform_Default_Expression ( DefaultExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoDefaultExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Type = Transform_Type(node.Type), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoDefaultSwitchLabelSyntax Transform_Default_Switch_Label ( DefaultSwitchLabelSyntax node) {
if(node == null) {
return null;
}
	return new PocoDefaultSwitchLabelSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText },  };

}


/// <summary></summary>

public static PocoDefineDirectiveTriviaSyntax Transform_Define_Directive_Trivia ( DefineDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoDefineDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
DefineKeyword = new PocoSyntaxToken {RawKind = node.DefineKeyword.RawKind, Kind = node.DefineKeyword.Kind().ToString(), Value = node.DefineKeyword.Value, ValueText = node.DefineKeyword.ValueText }, 
Name = new PocoSyntaxToken {RawKind = node.Name.RawKind, Kind = node.Name.Kind().ToString(), Value = node.Name.Value, ValueText = node.Name.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoDelegateDeclarationSyntax Transform_Delegate_Declaration ( DelegateDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoDelegateDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
DelegateKeyword = new PocoSyntaxToken {RawKind = node.DelegateKeyword.RawKind, Kind = node.DelegateKeyword.Kind().ToString(), Value = node.DelegateKeyword.Value, ValueText = node.DelegateKeyword.ValueText }, 
ReturnType = Transform_Type(node.ReturnType), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeParameterList = Transform_Type_Parameter_List(node.TypeParameterList), 
ParameterList = Transform_Parameter_List(node.ParameterList), 
ConstraintClauses = node.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList(), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoDestructorDeclarationSyntax Transform_Destructor_Declaration ( DestructorDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoDestructorDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
TildeToken = new PocoSyntaxToken {RawKind = node.TildeToken.RawKind, Kind = node.TildeToken.Kind().ToString(), Value = node.TildeToken.Value, ValueText = node.TildeToken.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
ParameterList = Transform_Parameter_List(node.ParameterList), 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoDirectiveTriviaSyntax Transform_Directive_Trivia ( DirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
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

public static PocoDiscardDesignationSyntax Transform_Discard_Designation ( DiscardDesignationSyntax node) {
if(node == null) {
return null;
}
	return new PocoDiscardDesignationSyntax() { 
UnderscoreToken = new PocoSyntaxToken {RawKind = node.UnderscoreToken.RawKind, Kind = node.UnderscoreToken.Kind().ToString(), Value = node.UnderscoreToken.Value, ValueText = node.UnderscoreToken.ValueText },  };

}


/// <summary></summary>

public static PocoDiscardPatternSyntax Transform_Discard_Pattern ( DiscardPatternSyntax node) {
if(node == null) {
return null;
}
	return new PocoDiscardPatternSyntax() { 
UnderscoreToken = new PocoSyntaxToken {RawKind = node.UnderscoreToken.RawKind, Kind = node.UnderscoreToken.Kind().ToString(), Value = node.UnderscoreToken.Value, ValueText = node.UnderscoreToken.ValueText },  };

}


/// <summary></summary>

public static PocoDocumentationCommentTriviaSyntax Transform_Documentation_Comment_Trivia ( DocumentationCommentTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoDocumentationCommentTriviaSyntax() { 
Content = node.Content.Select(Transform_Xml_Node).ToList(), 
EndOfComment = new PocoSyntaxToken {RawKind = node.EndOfComment.RawKind, Kind = node.EndOfComment.Kind().ToString(), Value = node.EndOfComment.Value, ValueText = node.EndOfComment.ValueText },  };

}


/// <summary></summary>

public static PocoDoStatementSyntax Transform_Do_Statement ( DoStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoDoStatementSyntax() { 
DoKeyword = new PocoSyntaxToken {RawKind = node.DoKeyword.RawKind, Kind = node.DoKeyword.Kind().ToString(), Value = node.DoKeyword.Value, ValueText = node.DoKeyword.ValueText }, 
Statement = Transform_Statement(node.Statement), 
WhileKeyword = new PocoSyntaxToken {RawKind = node.WhileKeyword.RawKind, Kind = node.WhileKeyword.Kind().ToString(), Value = node.WhileKeyword.Value, ValueText = node.WhileKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Condition = Transform_Expression(node.Condition), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoElementAccessExpressionSyntax Transform_Element_Access_Expression ( ElementAccessExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoElementAccessExpressionSyntax() { 
Expression = Transform_Expression(node.Expression), 
ArgumentList = Transform_Bracketed_Argument_List(node.ArgumentList),  };

}


/// <summary></summary>

public static PocoElementBindingExpressionSyntax Transform_Element_Binding_Expression ( ElementBindingExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoElementBindingExpressionSyntax() { 
ArgumentList = Transform_Bracketed_Argument_List(node.ArgumentList),  };

}


/// <summary></summary>

public static PocoElifDirectiveTriviaSyntax Transform_Elif_Directive_Trivia ( ElifDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoElifDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
ElifKeyword = new PocoSyntaxToken {RawKind = node.ElifKeyword.RawKind, Kind = node.ElifKeyword.Kind().ToString(), Value = node.ElifKeyword.Value, ValueText = node.ElifKeyword.ValueText }, 
Condition = Transform_Expression(node.Condition), 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive, 
BranchTaken = node.BranchTaken, 
ConditionValue = node.ConditionValue,  };

}


/// <summary></summary>

public static PocoElseClauseSyntax Transform_Else_Clause ( ElseClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoElseClauseSyntax() { 
ElseKeyword = new PocoSyntaxToken {RawKind = node.ElseKeyword.RawKind, Kind = node.ElseKeyword.Kind().ToString(), Value = node.ElseKeyword.Value, ValueText = node.ElseKeyword.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoElseDirectiveTriviaSyntax Transform_Else_Directive_Trivia ( ElseDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoElseDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
ElseKeyword = new PocoSyntaxToken {RawKind = node.ElseKeyword.RawKind, Kind = node.ElseKeyword.Kind().ToString(), Value = node.ElseKeyword.Value, ValueText = node.ElseKeyword.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive, 
BranchTaken = node.BranchTaken,  };

}


/// <summary></summary>

public static PocoEmptyStatementSyntax Transform_Empty_Statement ( EmptyStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoEmptyStatementSyntax() { 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoEndIfDirectiveTriviaSyntax Transform_End_If_Directive_Trivia ( EndIfDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoEndIfDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
EndIfKeyword = new PocoSyntaxToken {RawKind = node.EndIfKeyword.RawKind, Kind = node.EndIfKeyword.Kind().ToString(), Value = node.EndIfKeyword.Value, ValueText = node.EndIfKeyword.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoEndRegionDirectiveTriviaSyntax Transform_End_Region_Directive_Trivia ( EndRegionDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoEndRegionDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
EndRegionKeyword = new PocoSyntaxToken {RawKind = node.EndRegionKeyword.RawKind, Kind = node.EndRegionKeyword.Kind().ToString(), Value = node.EndRegionKeyword.Value, ValueText = node.EndRegionKeyword.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoEnumDeclarationSyntax Transform_Enum_Declaration ( EnumDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoEnumDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
EnumKeyword = new PocoSyntaxToken {RawKind = node.EnumKeyword.RawKind, Kind = node.EnumKeyword.Kind().ToString(), Value = node.EnumKeyword.Value, ValueText = node.EnumKeyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
BaseList = Transform_Base_List(node.BaseList), 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Members = node.Members.Select(Transform_Enum_Member_Declaration).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoEnumMemberDeclarationSyntax Transform_Enum_Member_Declaration ( EnumMemberDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoEnumMemberDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
EqualsValue = Transform_Equals_Value_Clause(node.EqualsValue),  };

}


/// <summary></summary>

public static PocoEqualsValueClauseSyntax Transform_Equals_Value_Clause ( EqualsValueClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoEqualsValueClauseSyntax() { 
EqualsToken = new PocoSyntaxToken {RawKind = node.EqualsToken.RawKind, Kind = node.EqualsToken.Kind().ToString(), Value = node.EqualsToken.Value, ValueText = node.EqualsToken.ValueText }, 
Value = Transform_Expression(node.Value),  };

}


/// <summary></summary>

public static PocoErrorDirectiveTriviaSyntax Transform_Error_Directive_Trivia ( ErrorDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoErrorDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
ErrorKeyword = new PocoSyntaxToken {RawKind = node.ErrorKeyword.RawKind, Kind = node.ErrorKeyword.Kind().ToString(), Value = node.ErrorKeyword.Value, ValueText = node.ErrorKeyword.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoEventDeclarationSyntax Transform_Event_Declaration ( EventDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoEventDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
EventKeyword = new PocoSyntaxToken {RawKind = node.EventKeyword.RawKind, Kind = node.EventKeyword.Kind().ToString(), Value = node.EventKeyword.Value, ValueText = node.EventKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
ExplicitInterfaceSpecifier = Transform_Explicit_Interface_Specifier(node.ExplicitInterfaceSpecifier), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
AccessorList = Transform_Accessor_List(node.AccessorList), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoEventFieldDeclarationSyntax Transform_Event_Field_Declaration ( EventFieldDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoEventFieldDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
EventKeyword = new PocoSyntaxToken {RawKind = node.EventKeyword.RawKind, Kind = node.EventKeyword.Kind().ToString(), Value = node.EventKeyword.Value, ValueText = node.EventKeyword.ValueText }, 
Declaration = Transform_Variable_Declaration(node.Declaration), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoExplicitInterfaceSpecifierSyntax Transform_Explicit_Interface_Specifier ( ExplicitInterfaceSpecifierSyntax node) {
if(node == null) {
return null;
}
	return new PocoExplicitInterfaceSpecifierSyntax() { 
Name = Transform_Name(node.Name), 
DotToken = new PocoSyntaxToken {RawKind = node.DotToken.RawKind, Kind = node.DotToken.Kind().ToString(), Value = node.DotToken.Value, ValueText = node.DotToken.ValueText },  };

}


/// <summary></summary>

public static PocoExpressionStatementSyntax Transform_Expression_Statement ( ExpressionStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoExpressionStatementSyntax() { 
Expression = Transform_Expression(node.Expression), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoExpressionSyntax Transform_Expression ( ExpressionSyntax node) {
if(node == null) {
return null;
}
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

public static PocoExternAliasDirectiveSyntax Transform_Extern_Alias_Directive ( ExternAliasDirectiveSyntax node) {
if(node == null) {
return null;
}
	return new PocoExternAliasDirectiveSyntax() { 
ExternKeyword = new PocoSyntaxToken {RawKind = node.ExternKeyword.RawKind, Kind = node.ExternKeyword.Kind().ToString(), Value = node.ExternKeyword.Value, ValueText = node.ExternKeyword.ValueText }, 
AliasKeyword = new PocoSyntaxToken {RawKind = node.AliasKeyword.RawKind, Kind = node.AliasKeyword.Kind().ToString(), Value = node.AliasKeyword.Value, ValueText = node.AliasKeyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoFieldDeclarationSyntax Transform_Field_Declaration ( FieldDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoFieldDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Declaration = Transform_Variable_Declaration(node.Declaration), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoFinallyClauseSyntax Transform_Finally_Clause ( FinallyClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoFinallyClauseSyntax() { 
FinallyKeyword = new PocoSyntaxToken {RawKind = node.FinallyKeyword.RawKind, Kind = node.FinallyKeyword.Kind().ToString(), Value = node.FinallyKeyword.Value, ValueText = node.FinallyKeyword.ValueText }, 
Block = Transform_Block(node.Block),  };

}


/// <summary></summary>

public static PocoFixedStatementSyntax Transform_Fixed_Statement ( FixedStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoFixedStatementSyntax() { 
FixedKeyword = new PocoSyntaxToken {RawKind = node.FixedKeyword.RawKind, Kind = node.FixedKeyword.Kind().ToString(), Value = node.FixedKeyword.Value, ValueText = node.FixedKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Declaration = Transform_Variable_Declaration(node.Declaration), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoForEachStatementSyntax Transform_For_Each_Statement ( ForEachStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoForEachStatementSyntax() { 
AwaitKeyword = new PocoSyntaxToken {RawKind = node.AwaitKeyword.RawKind, Kind = node.AwaitKeyword.Kind().ToString(), Value = node.AwaitKeyword.Value, ValueText = node.AwaitKeyword.ValueText }, 
ForEachKeyword = new PocoSyntaxToken {RawKind = node.ForEachKeyword.RawKind, Kind = node.ForEachKeyword.Kind().ToString(), Value = node.ForEachKeyword.Value, ValueText = node.ForEachKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Type = Transform_Type(node.Type), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
InKeyword = new PocoSyntaxToken {RawKind = node.InKeyword.RawKind, Kind = node.InKeyword.Kind().ToString(), Value = node.InKeyword.Value, ValueText = node.InKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoForEachVariableStatementSyntax Transform_For_Each_Variable_Statement ( ForEachVariableStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoForEachVariableStatementSyntax() { 
AwaitKeyword = new PocoSyntaxToken {RawKind = node.AwaitKeyword.RawKind, Kind = node.AwaitKeyword.Kind().ToString(), Value = node.AwaitKeyword.Value, ValueText = node.AwaitKeyword.ValueText }, 
ForEachKeyword = new PocoSyntaxToken {RawKind = node.ForEachKeyword.RawKind, Kind = node.ForEachKeyword.Kind().ToString(), Value = node.ForEachKeyword.Value, ValueText = node.ForEachKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Variable = Transform_Expression(node.Variable), 
InKeyword = new PocoSyntaxToken {RawKind = node.InKeyword.RawKind, Kind = node.InKeyword.Kind().ToString(), Value = node.InKeyword.Value, ValueText = node.InKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoForStatementSyntax Transform_For_Statement ( ForStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoForStatementSyntax() { 
ForKeyword = new PocoSyntaxToken {RawKind = node.ForKeyword.RawKind, Kind = node.ForKeyword.Kind().ToString(), Value = node.ForKeyword.Value, ValueText = node.ForKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
FirstSemicolonToken = new PocoSyntaxToken {RawKind = node.FirstSemicolonToken.RawKind, Kind = node.FirstSemicolonToken.Kind().ToString(), Value = node.FirstSemicolonToken.Value, ValueText = node.FirstSemicolonToken.ValueText }, 
Condition = Transform_Expression(node.Condition), 
SecondSemicolonToken = new PocoSyntaxToken {RawKind = node.SecondSemicolonToken.RawKind, Kind = node.SecondSemicolonToken.Kind().ToString(), Value = node.SecondSemicolonToken.Value, ValueText = node.SecondSemicolonToken.ValueText }, 
Incrementors = node.Incrementors.Select(Transform_Expression).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement), 
Declaration = Transform_Variable_Declaration(node.Declaration), 
Initializers = node.Initializers.Select(Transform_Expression).ToList(),  };

}


/// <summary></summary>

public static PocoFromClauseSyntax Transform_From_Clause ( FromClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoFromClauseSyntax() { 
FromKeyword = new PocoSyntaxToken {RawKind = node.FromKeyword.RawKind, Kind = node.FromKeyword.Kind().ToString(), Value = node.FromKeyword.Value, ValueText = node.FromKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
InKeyword = new PocoSyntaxToken {RawKind = node.InKeyword.RawKind, Kind = node.InKeyword.Kind().ToString(), Value = node.InKeyword.Value, ValueText = node.InKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoGenericNameSyntax Transform_Generic_Name ( GenericNameSyntax node) {
if(node == null) {
return null;
}
	return new PocoGenericNameSyntax() { 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeArgumentList = Transform_Type_Argument_List(node.TypeArgumentList),  };

}


/// <summary></summary>

public static PocoGlobalStatementSyntax Transform_Global_Statement ( GlobalStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoGlobalStatementSyntax() { 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoGotoStatementSyntax Transform_Goto_Statement ( GotoStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoGotoStatementSyntax() { 
GotoKeyword = new PocoSyntaxToken {RawKind = node.GotoKeyword.RawKind, Kind = node.GotoKeyword.Kind().ToString(), Value = node.GotoKeyword.Value, ValueText = node.GotoKeyword.ValueText }, 
CaseOrDefaultKeyword = new PocoSyntaxToken {RawKind = node.CaseOrDefaultKeyword.RawKind, Kind = node.CaseOrDefaultKeyword.Kind().ToString(), Value = node.CaseOrDefaultKeyword.Value, ValueText = node.CaseOrDefaultKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoGroupClauseSyntax Transform_Group_Clause ( GroupClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoGroupClauseSyntax() { 
GroupKeyword = new PocoSyntaxToken {RawKind = node.GroupKeyword.RawKind, Kind = node.GroupKeyword.Kind().ToString(), Value = node.GroupKeyword.Value, ValueText = node.GroupKeyword.ValueText }, 
GroupExpression = Transform_Expression(node.GroupExpression), 
ByKeyword = new PocoSyntaxToken {RawKind = node.ByKeyword.RawKind, Kind = node.ByKeyword.Kind().ToString(), Value = node.ByKeyword.Value, ValueText = node.ByKeyword.ValueText }, 
ByExpression = Transform_Expression(node.ByExpression),  };

}


/// <summary></summary>

public static PocoIdentifierNameSyntax Transform_Identifier_Name ( IdentifierNameSyntax node) {
if(node == null) {
return null;
}
	return new PocoIdentifierNameSyntax() { 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText },  };

}


/// <summary></summary>

public static PocoIfDirectiveTriviaSyntax Transform_If_Directive_Trivia ( IfDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoIfDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
IfKeyword = new PocoSyntaxToken {RawKind = node.IfKeyword.RawKind, Kind = node.IfKeyword.Kind().ToString(), Value = node.IfKeyword.Value, ValueText = node.IfKeyword.ValueText }, 
Condition = Transform_Expression(node.Condition), 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive, 
BranchTaken = node.BranchTaken, 
ConditionValue = node.ConditionValue,  };

}


/// <summary></summary>

public static PocoIfStatementSyntax Transform_If_Statement ( IfStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoIfStatementSyntax() { 
IfKeyword = new PocoSyntaxToken {RawKind = node.IfKeyword.RawKind, Kind = node.IfKeyword.Kind().ToString(), Value = node.IfKeyword.Value, ValueText = node.IfKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Condition = Transform_Expression(node.Condition), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement), 
Else = Transform_Else_Clause(node.Else),  };

}


/// <summary></summary>

public static PocoImplicitArrayCreationExpressionSyntax Transform_Implicit_Array_Creation_Expression ( ImplicitArrayCreationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoImplicitArrayCreationExpressionSyntax() { 
NewKeyword = new PocoSyntaxToken {RawKind = node.NewKeyword.RawKind, Kind = node.NewKeyword.Kind().ToString(), Value = node.NewKeyword.Value, ValueText = node.NewKeyword.ValueText }, 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
Commas = node.Commas.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText }, 
Initializer = Transform_Initializer_Expression(node.Initializer),  };

}


/// <summary></summary>

public static PocoImplicitElementAccessSyntax Transform_Implicit_Element_Access ( ImplicitElementAccessSyntax node) {
if(node == null) {
return null;
}
	return new PocoImplicitElementAccessSyntax() { 
ArgumentList = Transform_Bracketed_Argument_List(node.ArgumentList),  };

}


/// <summary></summary>

public static PocoImplicitStackAllocArrayCreationExpressionSyntax Transform_Implicit_Stack_Alloc_Array_Creation_Expression ( ImplicitStackAllocArrayCreationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoImplicitStackAllocArrayCreationExpressionSyntax() { 
StackAllocKeyword = new PocoSyntaxToken {RawKind = node.StackAllocKeyword.RawKind, Kind = node.StackAllocKeyword.Kind().ToString(), Value = node.StackAllocKeyword.Value, ValueText = node.StackAllocKeyword.ValueText }, 
OpenBracketToken = new PocoSyntaxToken {RawKind = node.OpenBracketToken.RawKind, Kind = node.OpenBracketToken.Kind().ToString(), Value = node.OpenBracketToken.Value, ValueText = node.OpenBracketToken.ValueText }, 
CloseBracketToken = new PocoSyntaxToken {RawKind = node.CloseBracketToken.RawKind, Kind = node.CloseBracketToken.Kind().ToString(), Value = node.CloseBracketToken.Value, ValueText = node.CloseBracketToken.ValueText }, 
Initializer = Transform_Initializer_Expression(node.Initializer),  };

}


/// <summary></summary>

public static PocoIncompleteMemberSyntax Transform_Incomplete_Member ( IncompleteMemberSyntax node) {
if(node == null) {
return null;
}
	return new PocoIncompleteMemberSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Type = Transform_Type(node.Type),  };

}


/// <summary></summary>

public static PocoIndexerDeclarationSyntax Transform_Indexer_Declaration ( IndexerDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoIndexerDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Type = Transform_Type(node.Type), 
ExplicitInterfaceSpecifier = Transform_Explicit_Interface_Specifier(node.ExplicitInterfaceSpecifier), 
ThisKeyword = new PocoSyntaxToken {RawKind = node.ThisKeyword.RawKind, Kind = node.ThisKeyword.Kind().ToString(), Value = node.ThisKeyword.Value, ValueText = node.ThisKeyword.ValueText }, 
ParameterList = Transform_Bracketed_Parameter_List(node.ParameterList), 
AccessorList = Transform_Accessor_List(node.AccessorList),  };

}


/// <summary></summary>

public static PocoIndexerMemberCrefSyntax Transform_Indexer_Member_Cref ( IndexerMemberCrefSyntax node) {
if(node == null) {
return null;
}
	return new PocoIndexerMemberCrefSyntax() { 
ThisKeyword = new PocoSyntaxToken {RawKind = node.ThisKeyword.RawKind, Kind = node.ThisKeyword.Kind().ToString(), Value = node.ThisKeyword.Value, ValueText = node.ThisKeyword.ValueText }, 
Parameters = Transform_Cref_Bracketed_Parameter_List(node.Parameters),  };

}


/// <summary></summary>

public static PocoInitializerExpressionSyntax Transform_Initializer_Expression ( InitializerExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoInitializerExpressionSyntax() { 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Expressions = node.Expressions.Select(Transform_Expression).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoInstanceExpressionSyntax Transform_Instance_Expression ( InstanceExpressionSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ThisExpressionSyntax _: return Transform_This_Expression((ThisExpressionSyntax)node); 
case BaseExpressionSyntax _: return Transform_Base_Expression((BaseExpressionSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoInterfaceDeclarationSyntax Transform_Interface_Declaration ( InterfaceDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoInterfaceDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeParameterList = Transform_Type_Parameter_List(node.TypeParameterList), 
BaseList = Transform_Base_List(node.BaseList), 
ConstraintClauses = node.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList(), 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Members = node.Members.Select(Transform_Member_Declaration).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoInterpolatedStringContentSyntax Transform_Interpolated_String_Content ( InterpolatedStringContentSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case InterpolatedStringTextSyntax _: return Transform_Interpolated_String_Text((InterpolatedStringTextSyntax)node); 
case InterpolationSyntax _: return Transform_Interpolation((InterpolationSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoInterpolatedStringExpressionSyntax Transform_Interpolated_String_Expression ( InterpolatedStringExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoInterpolatedStringExpressionSyntax() { 
StringStartToken = new PocoSyntaxToken {RawKind = node.StringStartToken.RawKind, Kind = node.StringStartToken.Kind().ToString(), Value = node.StringStartToken.Value, ValueText = node.StringStartToken.ValueText }, 
Contents = node.Contents.Select(Transform_Interpolated_String_Content).ToList(), 
StringEndToken = new PocoSyntaxToken {RawKind = node.StringEndToken.RawKind, Kind = node.StringEndToken.Kind().ToString(), Value = node.StringEndToken.Value, ValueText = node.StringEndToken.ValueText },  };

}


/// <summary></summary>

public static PocoInterpolatedStringTextSyntax Transform_Interpolated_String_Text ( InterpolatedStringTextSyntax node) {
if(node == null) {
return null;
}
	return new PocoInterpolatedStringTextSyntax() { 
TextToken = new PocoSyntaxToken {RawKind = node.TextToken.RawKind, Kind = node.TextToken.Kind().ToString(), Value = node.TextToken.Value, ValueText = node.TextToken.ValueText },  };

}


/// <summary></summary>

public static PocoInterpolationAlignmentClauseSyntax Transform_Interpolation_Alignment_Clause ( InterpolationAlignmentClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoInterpolationAlignmentClauseSyntax() { 
CommaToken = new PocoSyntaxToken {RawKind = node.CommaToken.RawKind, Kind = node.CommaToken.Kind().ToString(), Value = node.CommaToken.Value, ValueText = node.CommaToken.ValueText }, 
Value = Transform_Expression(node.Value),  };

}


/// <summary></summary>

public static PocoInterpolationFormatClauseSyntax Transform_Interpolation_Format_Clause ( InterpolationFormatClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoInterpolationFormatClauseSyntax() { 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText }, 
FormatStringToken = new PocoSyntaxToken {RawKind = node.FormatStringToken.RawKind, Kind = node.FormatStringToken.Kind().ToString(), Value = node.FormatStringToken.Value, ValueText = node.FormatStringToken.ValueText },  };

}


/// <summary></summary>

public static PocoInterpolationSyntax Transform_Interpolation ( InterpolationSyntax node) {
if(node == null) {
return null;
}
	return new PocoInterpolationSyntax() { 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
AlignmentClause = Transform_Interpolation_Alignment_Clause(node.AlignmentClause), 
FormatClause = Transform_Interpolation_Format_Clause(node.FormatClause), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoInvocationExpressionSyntax Transform_Invocation_Expression ( InvocationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoInvocationExpressionSyntax() { 
Expression = Transform_Expression(node.Expression), 
ArgumentList = Transform_Argument_List(node.ArgumentList),  };

}


/// <summary></summary>

public static PocoIsPatternExpressionSyntax Transform_Is_Pattern_Expression ( IsPatternExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoIsPatternExpressionSyntax() { 
Expression = Transform_Expression(node.Expression), 
IsKeyword = new PocoSyntaxToken {RawKind = node.IsKeyword.RawKind, Kind = node.IsKeyword.Kind().ToString(), Value = node.IsKeyword.Value, ValueText = node.IsKeyword.ValueText }, 
Pattern = Transform_Pattern(node.Pattern),  };

}


/// <summary></summary>

public static PocoJoinClauseSyntax Transform_Join_Clause ( JoinClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoJoinClauseSyntax() { 
JoinKeyword = new PocoSyntaxToken {RawKind = node.JoinKeyword.RawKind, Kind = node.JoinKeyword.Kind().ToString(), Value = node.JoinKeyword.Value, ValueText = node.JoinKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
InKeyword = new PocoSyntaxToken {RawKind = node.InKeyword.RawKind, Kind = node.InKeyword.Kind().ToString(), Value = node.InKeyword.Value, ValueText = node.InKeyword.ValueText }, 
InExpression = Transform_Expression(node.InExpression), 
OnKeyword = new PocoSyntaxToken {RawKind = node.OnKeyword.RawKind, Kind = node.OnKeyword.Kind().ToString(), Value = node.OnKeyword.Value, ValueText = node.OnKeyword.ValueText }, 
LeftExpression = Transform_Expression(node.LeftExpression), 
EqualsKeyword = new PocoSyntaxToken {RawKind = node.EqualsKeyword.RawKind, Kind = node.EqualsKeyword.Kind().ToString(), Value = node.EqualsKeyword.Value, ValueText = node.EqualsKeyword.ValueText }, 
RightExpression = Transform_Expression(node.RightExpression), 
Into = Transform_Join_Into_Clause(node.Into),  };

}


/// <summary></summary>

public static PocoJoinIntoClauseSyntax Transform_Join_Into_Clause ( JoinIntoClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoJoinIntoClauseSyntax() { 
IntoKeyword = new PocoSyntaxToken {RawKind = node.IntoKeyword.RawKind, Kind = node.IntoKeyword.Kind().ToString(), Value = node.IntoKeyword.Value, ValueText = node.IntoKeyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText },  };

}


/// <summary></summary>

public static PocoLabeledStatementSyntax Transform_Labeled_Statement ( LabeledStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoLabeledStatementSyntax() { 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoLambdaExpressionSyntax Transform_Lambda_Expression ( LambdaExpressionSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case SimpleLambdaExpressionSyntax _: return Transform_Simple_Lambda_Expression((SimpleLambdaExpressionSyntax)node); 
case ParenthesizedLambdaExpressionSyntax _: return Transform_Parenthesized_Lambda_Expression((ParenthesizedLambdaExpressionSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoLetClauseSyntax Transform_Let_Clause ( LetClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoLetClauseSyntax() { 
LetKeyword = new PocoSyntaxToken {RawKind = node.LetKeyword.RawKind, Kind = node.LetKeyword.Kind().ToString(), Value = node.LetKeyword.Value, ValueText = node.LetKeyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
EqualsToken = new PocoSyntaxToken {RawKind = node.EqualsToken.RawKind, Kind = node.EqualsToken.Kind().ToString(), Value = node.EqualsToken.Value, ValueText = node.EqualsToken.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoLineDirectiveTriviaSyntax Transform_Line_Directive_Trivia ( LineDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoLineDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
LineKeyword = new PocoSyntaxToken {RawKind = node.LineKeyword.RawKind, Kind = node.LineKeyword.Kind().ToString(), Value = node.LineKeyword.Value, ValueText = node.LineKeyword.ValueText }, 
Line = new PocoSyntaxToken {RawKind = node.Line.RawKind, Kind = node.Line.Kind().ToString(), Value = node.Line.Value, ValueText = node.Line.ValueText }, 
File = new PocoSyntaxToken {RawKind = node.File.RawKind, Kind = node.File.Kind().ToString(), Value = node.File.Value, ValueText = node.File.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoLiteralExpressionSyntax Transform_Literal_Expression ( LiteralExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoLiteralExpressionSyntax() { 
Token = new PocoSyntaxToken {RawKind = node.Token.RawKind, Kind = node.Token.Kind().ToString(), Value = node.Token.Value, ValueText = node.Token.ValueText },  };

}


/// <summary></summary>

public static PocoLoadDirectiveTriviaSyntax Transform_Load_Directive_Trivia ( LoadDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoLoadDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
LoadKeyword = new PocoSyntaxToken {RawKind = node.LoadKeyword.RawKind, Kind = node.LoadKeyword.Kind().ToString(), Value = node.LoadKeyword.Value, ValueText = node.LoadKeyword.ValueText }, 
File = new PocoSyntaxToken {RawKind = node.File.RawKind, Kind = node.File.Kind().ToString(), Value = node.File.Value, ValueText = node.File.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoLocalDeclarationStatementSyntax Transform_Local_Declaration_Statement ( LocalDeclarationStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoLocalDeclarationStatementSyntax() { 
AwaitKeyword = new PocoSyntaxToken {RawKind = node.AwaitKeyword.RawKind, Kind = node.AwaitKeyword.Kind().ToString(), Value = node.AwaitKeyword.Value, ValueText = node.AwaitKeyword.ValueText }, 
UsingKeyword = new PocoSyntaxToken {RawKind = node.UsingKeyword.RawKind, Kind = node.UsingKeyword.Kind().ToString(), Value = node.UsingKeyword.Value, ValueText = node.UsingKeyword.ValueText }, 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Declaration = Transform_Variable_Declaration(node.Declaration), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoLocalFunctionStatementSyntax Transform_Local_Function_Statement ( LocalFunctionStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoLocalFunctionStatementSyntax() { 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
ReturnType = Transform_Type(node.ReturnType), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeParameterList = Transform_Type_Parameter_List(node.TypeParameterList), 
ParameterList = Transform_Parameter_List(node.ParameterList), 
ConstraintClauses = node.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList(), 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoLockStatementSyntax Transform_Lock_Statement ( LockStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoLockStatementSyntax() { 
LockKeyword = new PocoSyntaxToken {RawKind = node.LockKeyword.RawKind, Kind = node.LockKeyword.Kind().ToString(), Value = node.LockKeyword.Value, ValueText = node.LockKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoMakeRefExpressionSyntax Transform_Make_Ref_Expression ( MakeRefExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoMakeRefExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoMemberAccessExpressionSyntax Transform_Member_Access_Expression ( MemberAccessExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoMemberAccessExpressionSyntax() { 
Expression = Transform_Expression(node.Expression), 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
Name = Transform_Simple_Name(node.Name),  };

}


/// <summary></summary>

public static PocoMemberBindingExpressionSyntax Transform_Member_Binding_Expression ( MemberBindingExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoMemberBindingExpressionSyntax() { 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
Name = Transform_Simple_Name(node.Name),  };

}


/// <summary></summary>

public static PocoMemberCrefSyntax Transform_Member_Cref ( MemberCrefSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case NameMemberCrefSyntax _: return Transform_Name_Member_Cref((NameMemberCrefSyntax)node); 
case IndexerMemberCrefSyntax _: return Transform_Indexer_Member_Cref((IndexerMemberCrefSyntax)node); 
case OperatorMemberCrefSyntax _: return Transform_Operator_Member_Cref((OperatorMemberCrefSyntax)node); 
case ConversionOperatorMemberCrefSyntax _: return Transform_Conversion_Operator_Member_Cref((ConversionOperatorMemberCrefSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoMemberDeclarationSyntax Transform_Member_Declaration ( MemberDeclarationSyntax node) {
if(node == null) {
return null;
}
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

public static PocoMethodDeclarationSyntax Transform_Method_Declaration ( MethodDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoMethodDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
ReturnType = Transform_Type(node.ReturnType), 
ExplicitInterfaceSpecifier = Transform_Explicit_Interface_Specifier(node.ExplicitInterfaceSpecifier), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeParameterList = Transform_Type_Parameter_List(node.TypeParameterList), 
ParameterList = Transform_Parameter_List(node.ParameterList), 
ConstraintClauses = node.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList(), 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoNameColonSyntax Transform_Name_Colon ( NameColonSyntax node) {
if(node == null) {
return null;
}
	return new PocoNameColonSyntax() { 
Name = Transform_Identifier_Name(node.Name), 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText },  };

}


/// <summary></summary>

public static PocoNameEqualsSyntax Transform_Name_Equals ( NameEqualsSyntax node) {
if(node == null) {
return null;
}
	return new PocoNameEqualsSyntax() { 
Name = Transform_Identifier_Name(node.Name), 
EqualsToken = new PocoSyntaxToken {RawKind = node.EqualsToken.RawKind, Kind = node.EqualsToken.Kind().ToString(), Value = node.EqualsToken.Value, ValueText = node.EqualsToken.ValueText },  };

}


/// <summary></summary>

public static PocoNameMemberCrefSyntax Transform_Name_Member_Cref ( NameMemberCrefSyntax node) {
if(node == null) {
return null;
}
	return new PocoNameMemberCrefSyntax() { 
Name = Transform_Type(node.Name), 
Parameters = Transform_Cref_Parameter_List(node.Parameters),  };

}


/// <summary></summary>

public static PocoNamespaceDeclarationSyntax Transform_Namespace_Declaration ( NamespaceDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoNamespaceDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
NamespaceKeyword = new PocoSyntaxToken {RawKind = node.NamespaceKeyword.RawKind, Kind = node.NamespaceKeyword.Kind().ToString(), Value = node.NamespaceKeyword.Value, ValueText = node.NamespaceKeyword.ValueText }, 
Name = Transform_Name(node.Name), 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Externs = node.Externs.Select(Transform_Extern_Alias_Directive).ToList(), 
Usings = node.Usings.Select(Transform_Using_Directive).ToList(), 
Members = node.Members.Select(Transform_Member_Declaration).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoNameSyntax Transform_Name ( NameSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case IdentifierNameSyntax _: return Transform_Identifier_Name((IdentifierNameSyntax)node); 
case GenericNameSyntax _: return Transform_Generic_Name((GenericNameSyntax)node); 
case QualifiedNameSyntax _: return Transform_Qualified_Name((QualifiedNameSyntax)node); 
case AliasQualifiedNameSyntax _: return Transform_Alias_Qualified_Name((AliasQualifiedNameSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoNullableDirectiveTriviaSyntax Transform_Nullable_Directive_Trivia ( NullableDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoNullableDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
NullableKeyword = new PocoSyntaxToken {RawKind = node.NullableKeyword.RawKind, Kind = node.NullableKeyword.Kind().ToString(), Value = node.NullableKeyword.Value, ValueText = node.NullableKeyword.ValueText }, 
SettingToken = new PocoSyntaxToken {RawKind = node.SettingToken.RawKind, Kind = node.SettingToken.Kind().ToString(), Value = node.SettingToken.Value, ValueText = node.SettingToken.ValueText }, 
TargetToken = new PocoSyntaxToken {RawKind = node.TargetToken.RawKind, Kind = node.TargetToken.Kind().ToString(), Value = node.TargetToken.Value, ValueText = node.TargetToken.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoNullableTypeSyntax Transform_Nullable_Type ( NullableTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoNullableTypeSyntax() { 
ElementType = Transform_Type(node.ElementType), 
QuestionToken = new PocoSyntaxToken {RawKind = node.QuestionToken.RawKind, Kind = node.QuestionToken.Kind().ToString(), Value = node.QuestionToken.Value, ValueText = node.QuestionToken.ValueText },  };

}


/// <summary></summary>

public static PocoObjectCreationExpressionSyntax Transform_Object_Creation_Expression ( ObjectCreationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoObjectCreationExpressionSyntax() { 
NewKeyword = new PocoSyntaxToken {RawKind = node.NewKeyword.RawKind, Kind = node.NewKeyword.Kind().ToString(), Value = node.NewKeyword.Value, ValueText = node.NewKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
ArgumentList = Transform_Argument_List(node.ArgumentList), 
Initializer = Transform_Initializer_Expression(node.Initializer),  };

}


/// <summary></summary>

public static PocoOmittedArraySizeExpressionSyntax Transform_Omitted_Array_Size_Expression ( OmittedArraySizeExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoOmittedArraySizeExpressionSyntax() { 
OmittedArraySizeExpressionToken = new PocoSyntaxToken {RawKind = node.OmittedArraySizeExpressionToken.RawKind, Kind = node.OmittedArraySizeExpressionToken.Kind().ToString(), Value = node.OmittedArraySizeExpressionToken.Value, ValueText = node.OmittedArraySizeExpressionToken.ValueText },  };

}


/// <summary></summary>

public static PocoOmittedTypeArgumentSyntax Transform_Omitted_Type_Argument ( OmittedTypeArgumentSyntax node) {
if(node == null) {
return null;
}
	return new PocoOmittedTypeArgumentSyntax() { 
OmittedTypeArgumentToken = new PocoSyntaxToken {RawKind = node.OmittedTypeArgumentToken.RawKind, Kind = node.OmittedTypeArgumentToken.Kind().ToString(), Value = node.OmittedTypeArgumentToken.Value, ValueText = node.OmittedTypeArgumentToken.ValueText },  };

}


/// <summary></summary>

public static PocoOperatorDeclarationSyntax Transform_Operator_Declaration ( OperatorDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoOperatorDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
ReturnType = Transform_Type(node.ReturnType), 
OperatorKeyword = new PocoSyntaxToken {RawKind = node.OperatorKeyword.RawKind, Kind = node.OperatorKeyword.Kind().ToString(), Value = node.OperatorKeyword.Value, ValueText = node.OperatorKeyword.ValueText }, 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
ParameterList = Transform_Parameter_List(node.ParameterList), 
Body = Transform_Block(node.Body),  };

}


/// <summary></summary>

public static PocoOperatorMemberCrefSyntax Transform_Operator_Member_Cref ( OperatorMemberCrefSyntax node) {
if(node == null) {
return null;
}
	return new PocoOperatorMemberCrefSyntax() { 
OperatorKeyword = new PocoSyntaxToken {RawKind = node.OperatorKeyword.RawKind, Kind = node.OperatorKeyword.Kind().ToString(), Value = node.OperatorKeyword.Value, ValueText = node.OperatorKeyword.ValueText }, 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
Parameters = Transform_Cref_Parameter_List(node.Parameters),  };

}


/// <summary></summary>

public static PocoOrderByClauseSyntax Transform_Order_By_Clause ( OrderByClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoOrderByClauseSyntax() { 
OrderByKeyword = new PocoSyntaxToken {RawKind = node.OrderByKeyword.RawKind, Kind = node.OrderByKeyword.Kind().ToString(), Value = node.OrderByKeyword.Value, ValueText = node.OrderByKeyword.ValueText }, 
Orderings = node.Orderings.Select(Transform_Ordering).ToList(),  };

}


/// <summary></summary>

public static PocoOrderingSyntax Transform_Ordering ( OrderingSyntax node) {
if(node == null) {
return null;
}
	return new PocoOrderingSyntax() { 
Expression = Transform_Expression(node.Expression), 
AscendingOrDescendingKeyword = new PocoSyntaxToken {RawKind = node.AscendingOrDescendingKeyword.RawKind, Kind = node.AscendingOrDescendingKeyword.Kind().ToString(), Value = node.AscendingOrDescendingKeyword.Value, ValueText = node.AscendingOrDescendingKeyword.ValueText },  };

}


/// <summary></summary>

public static PocoParameterListSyntax Transform_Parameter_List ( ParameterListSyntax node) {
if(node == null) {
return null;
}
	return new PocoParameterListSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Parameters = node.Parameters.Select(Transform_Parameter).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoParameterSyntax Transform_Parameter ( ParameterSyntax node) {
if(node == null) {
return null;
}
	return new PocoParameterSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Type = Transform_Type(node.Type), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
Default = Transform_Equals_Value_Clause(node.Default),  };

}


/// <summary></summary>

public static PocoParenthesizedExpressionSyntax Transform_Parenthesized_Expression ( ParenthesizedExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoParenthesizedExpressionSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoParenthesizedLambdaExpressionSyntax Transform_Parenthesized_Lambda_Expression ( ParenthesizedLambdaExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoParenthesizedLambdaExpressionSyntax() { 
AsyncKeyword = new PocoSyntaxToken {RawKind = node.AsyncKeyword.RawKind, Kind = node.AsyncKeyword.Kind().ToString(), Value = node.AsyncKeyword.Value, ValueText = node.AsyncKeyword.ValueText }, 
ParameterList = Transform_Parameter_List(node.ParameterList), 
ArrowToken = new PocoSyntaxToken {RawKind = node.ArrowToken.RawKind, Kind = node.ArrowToken.Kind().ToString(), Value = node.ArrowToken.Value, ValueText = node.ArrowToken.ValueText }, 
Block = Transform_Block(node.Block), 
ExpressionBody = Transform_Expression(node.ExpressionBody),  };

}


/// <summary></summary>

public static PocoParenthesizedVariableDesignationSyntax Transform_Parenthesized_Variable_Designation ( ParenthesizedVariableDesignationSyntax node) {
if(node == null) {
return null;
}
	return new PocoParenthesizedVariableDesignationSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Variables = node.Variables.Select(Transform_Variable_Designation).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoPatternSyntax Transform_Pattern ( PatternSyntax node) {
if(node == null) {
return null;
}
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

public static PocoPointerTypeSyntax Transform_Pointer_Type ( PointerTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoPointerTypeSyntax() { 
ElementType = Transform_Type(node.ElementType), 
AsteriskToken = new PocoSyntaxToken {RawKind = node.AsteriskToken.RawKind, Kind = node.AsteriskToken.Kind().ToString(), Value = node.AsteriskToken.Value, ValueText = node.AsteriskToken.ValueText },  };

}


/// <summary></summary>

public static PocoPositionalPatternClauseSyntax Transform_Positional_Pattern_Clause ( PositionalPatternClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoPositionalPatternClauseSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Subpatterns = node.Subpatterns.Select(Transform_Subpattern).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoPostfixUnaryExpressionSyntax Transform_Postfix_Unary_Expression ( PostfixUnaryExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoPostfixUnaryExpressionSyntax() { 
Operand = Transform_Expression(node.Operand), 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText },  };

}


/// <summary></summary>

public static PocoPragmaChecksumDirectiveTriviaSyntax Transform_Pragma_Checksum_Directive_Trivia ( PragmaChecksumDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoPragmaChecksumDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
PragmaKeyword = new PocoSyntaxToken {RawKind = node.PragmaKeyword.RawKind, Kind = node.PragmaKeyword.Kind().ToString(), Value = node.PragmaKeyword.Value, ValueText = node.PragmaKeyword.ValueText }, 
ChecksumKeyword = new PocoSyntaxToken {RawKind = node.ChecksumKeyword.RawKind, Kind = node.ChecksumKeyword.Kind().ToString(), Value = node.ChecksumKeyword.Value, ValueText = node.ChecksumKeyword.ValueText }, 
File = new PocoSyntaxToken {RawKind = node.File.RawKind, Kind = node.File.Kind().ToString(), Value = node.File.Value, ValueText = node.File.ValueText }, 
Guid = new PocoSyntaxToken {RawKind = node.Guid.RawKind, Kind = node.Guid.Kind().ToString(), Value = node.Guid.Value, ValueText = node.Guid.ValueText }, 
Bytes = new PocoSyntaxToken {RawKind = node.Bytes.RawKind, Kind = node.Bytes.Kind().ToString(), Value = node.Bytes.Value, ValueText = node.Bytes.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoPragmaWarningDirectiveTriviaSyntax Transform_Pragma_Warning_Directive_Trivia ( PragmaWarningDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoPragmaWarningDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
PragmaKeyword = new PocoSyntaxToken {RawKind = node.PragmaKeyword.RawKind, Kind = node.PragmaKeyword.Kind().ToString(), Value = node.PragmaKeyword.Value, ValueText = node.PragmaKeyword.ValueText }, 
WarningKeyword = new PocoSyntaxToken {RawKind = node.WarningKeyword.RawKind, Kind = node.WarningKeyword.Kind().ToString(), Value = node.WarningKeyword.Value, ValueText = node.WarningKeyword.ValueText }, 
DisableOrRestoreKeyword = new PocoSyntaxToken {RawKind = node.DisableOrRestoreKeyword.RawKind, Kind = node.DisableOrRestoreKeyword.Kind().ToString(), Value = node.DisableOrRestoreKeyword.Value, ValueText = node.DisableOrRestoreKeyword.ValueText }, 
ErrorCodes = node.ErrorCodes.Select(Transform_Expression).ToList(), 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoPredefinedTypeSyntax Transform_Predefined_Type ( PredefinedTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoPredefinedTypeSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText },  };

}


/// <summary></summary>

public static PocoPrefixUnaryExpressionSyntax Transform_Prefix_Unary_Expression ( PrefixUnaryExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoPrefixUnaryExpressionSyntax() { 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
Operand = Transform_Expression(node.Operand),  };

}


/// <summary></summary>

public static PocoPropertyDeclarationSyntax Transform_Property_Declaration ( PropertyDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoPropertyDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Type = Transform_Type(node.Type), 
ExplicitInterfaceSpecifier = Transform_Explicit_Interface_Specifier(node.ExplicitInterfaceSpecifier), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
AccessorList = Transform_Accessor_List(node.AccessorList),  };

}


/// <summary></summary>

public static PocoPropertyPatternClauseSyntax Transform_Property_Pattern_Clause ( PropertyPatternClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoPropertyPatternClauseSyntax() { 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Subpatterns = node.Subpatterns.Select(Transform_Subpattern).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoQualifiedCrefSyntax Transform_Qualified_Cref ( QualifiedCrefSyntax node) {
if(node == null) {
return null;
}
	return new PocoQualifiedCrefSyntax() { 
Container = Transform_Type(node.Container), 
DotToken = new PocoSyntaxToken {RawKind = node.DotToken.RawKind, Kind = node.DotToken.Kind().ToString(), Value = node.DotToken.Value, ValueText = node.DotToken.ValueText }, 
Member = Transform_Member_Cref(node.Member),  };

}


/// <summary></summary>

public static PocoQualifiedNameSyntax Transform_Qualified_Name ( QualifiedNameSyntax node) {
if(node == null) {
return null;
}
	return new PocoQualifiedNameSyntax() { 
Left = Transform_Name(node.Left), 
DotToken = new PocoSyntaxToken {RawKind = node.DotToken.RawKind, Kind = node.DotToken.Kind().ToString(), Value = node.DotToken.Value, ValueText = node.DotToken.ValueText }, 
Right = Transform_Simple_Name(node.Right),  };

}


/// <summary></summary>

public static PocoQueryBodySyntax Transform_Query_Body ( QueryBodySyntax node) {
if(node == null) {
return null;
}
	return new PocoQueryBodySyntax() { 
Clauses = node.Clauses.Select(Transform_Query_Clause).ToList(), 
SelectOrGroup = Transform_Select_Or_Group_Clause(node.SelectOrGroup), 
Continuation = Transform_Query_Continuation(node.Continuation),  };

}


/// <summary></summary>

public static PocoQueryClauseSyntax Transform_Query_Clause ( QueryClauseSyntax node) {
if(node == null) {
return null;
}
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

public static PocoQueryContinuationSyntax Transform_Query_Continuation ( QueryContinuationSyntax node) {
if(node == null) {
return null;
}
	return new PocoQueryContinuationSyntax() { 
IntoKeyword = new PocoSyntaxToken {RawKind = node.IntoKeyword.RawKind, Kind = node.IntoKeyword.Kind().ToString(), Value = node.IntoKeyword.Value, ValueText = node.IntoKeyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
Body = Transform_Query_Body(node.Body),  };

}


/// <summary></summary>

public static PocoQueryExpressionSyntax Transform_Query_Expression ( QueryExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoQueryExpressionSyntax() { 
FromClause = Transform_From_Clause(node.FromClause), 
Body = Transform_Query_Body(node.Body),  };

}


/// <summary></summary>

public static PocoRangeExpressionSyntax Transform_Range_Expression ( RangeExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoRangeExpressionSyntax() { 
LeftOperand = Transform_Expression(node.LeftOperand), 
OperatorToken = new PocoSyntaxToken {RawKind = node.OperatorToken.RawKind, Kind = node.OperatorToken.Kind().ToString(), Value = node.OperatorToken.Value, ValueText = node.OperatorToken.ValueText }, 
RightOperand = Transform_Expression(node.RightOperand),  };

}


/// <summary></summary>

public static PocoRecursivePatternSyntax Transform_Recursive_Pattern ( RecursivePatternSyntax node) {
if(node == null) {
return null;
}
	return new PocoRecursivePatternSyntax() { 
Type = Transform_Type(node.Type), 
PositionalPatternClause = Transform_Positional_Pattern_Clause(node.PositionalPatternClause), 
PropertyPatternClause = Transform_Property_Pattern_Clause(node.PropertyPatternClause), 
Designation = Transform_Variable_Designation(node.Designation),  };

}


/// <summary></summary>

public static PocoReferenceDirectiveTriviaSyntax Transform_Reference_Directive_Trivia ( ReferenceDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoReferenceDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
ReferenceKeyword = new PocoSyntaxToken {RawKind = node.ReferenceKeyword.RawKind, Kind = node.ReferenceKeyword.Kind().ToString(), Value = node.ReferenceKeyword.Value, ValueText = node.ReferenceKeyword.ValueText }, 
File = new PocoSyntaxToken {RawKind = node.File.RawKind, Kind = node.File.Kind().ToString(), Value = node.File.Value, ValueText = node.File.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoRefExpressionSyntax Transform_Ref_Expression ( RefExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoRefExpressionSyntax() { 
RefKeyword = new PocoSyntaxToken {RawKind = node.RefKeyword.RawKind, Kind = node.RefKeyword.Kind().ToString(), Value = node.RefKeyword.Value, ValueText = node.RefKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoRefTypeExpressionSyntax Transform_Ref_Type_Expression ( RefTypeExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoRefTypeExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoRefTypeSyntax Transform_Ref_Type ( RefTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoRefTypeSyntax() { 
RefKeyword = new PocoSyntaxToken {RawKind = node.RefKeyword.RawKind, Kind = node.RefKeyword.Kind().ToString(), Value = node.RefKeyword.Value, ValueText = node.RefKeyword.ValueText }, 
ReadOnlyKeyword = new PocoSyntaxToken {RawKind = node.ReadOnlyKeyword.RawKind, Kind = node.ReadOnlyKeyword.Kind().ToString(), Value = node.ReadOnlyKeyword.Value, ValueText = node.ReadOnlyKeyword.ValueText }, 
Type = Transform_Type(node.Type),  };

}


/// <summary></summary>

public static PocoRefValueExpressionSyntax Transform_Ref_Value_Expression ( RefValueExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoRefValueExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
Comma = new PocoSyntaxToken {RawKind = node.Comma.RawKind, Kind = node.Comma.Kind().ToString(), Value = node.Comma.Value, ValueText = node.Comma.ValueText }, 
Type = Transform_Type(node.Type), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoRegionDirectiveTriviaSyntax Transform_Region_Directive_Trivia ( RegionDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoRegionDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
RegionKeyword = new PocoSyntaxToken {RawKind = node.RegionKeyword.RawKind, Kind = node.RegionKeyword.Kind().ToString(), Value = node.RegionKeyword.Value, ValueText = node.RegionKeyword.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoReturnStatementSyntax Transform_Return_Statement ( ReturnStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoReturnStatementSyntax() { 
ReturnKeyword = new PocoSyntaxToken {RawKind = node.ReturnKeyword.RawKind, Kind = node.ReturnKeyword.Kind().ToString(), Value = node.ReturnKeyword.Value, ValueText = node.ReturnKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoSelectClauseSyntax Transform_Select_Clause ( SelectClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoSelectClauseSyntax() { 
SelectKeyword = new PocoSyntaxToken {RawKind = node.SelectKeyword.RawKind, Kind = node.SelectKeyword.Kind().ToString(), Value = node.SelectKeyword.Value, ValueText = node.SelectKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoSelectOrGroupClauseSyntax Transform_Select_Or_Group_Clause ( SelectOrGroupClauseSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case SelectClauseSyntax _: return Transform_Select_Clause((SelectClauseSyntax)node); 
case GroupClauseSyntax _: return Transform_Group_Clause((GroupClauseSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoShebangDirectiveTriviaSyntax Transform_Shebang_Directive_Trivia ( ShebangDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoShebangDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
ExclamationToken = new PocoSyntaxToken {RawKind = node.ExclamationToken.RawKind, Kind = node.ExclamationToken.Kind().ToString(), Value = node.ExclamationToken.Value, ValueText = node.ExclamationToken.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoSimpleBaseTypeSyntax Transform_Simple_Base_Type ( SimpleBaseTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoSimpleBaseTypeSyntax() { 
Type = Transform_Type(node.Type),  };

}


/// <summary></summary>

public static PocoSimpleLambdaExpressionSyntax Transform_Simple_Lambda_Expression ( SimpleLambdaExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoSimpleLambdaExpressionSyntax() { 
AsyncKeyword = new PocoSyntaxToken {RawKind = node.AsyncKeyword.RawKind, Kind = node.AsyncKeyword.Kind().ToString(), Value = node.AsyncKeyword.Value, ValueText = node.AsyncKeyword.ValueText }, 
Parameter = Transform_Parameter(node.Parameter), 
ArrowToken = new PocoSyntaxToken {RawKind = node.ArrowToken.RawKind, Kind = node.ArrowToken.Kind().ToString(), Value = node.ArrowToken.Value, ValueText = node.ArrowToken.ValueText }, 
Block = Transform_Block(node.Block), 
ExpressionBody = Transform_Expression(node.ExpressionBody),  };

}


/// <summary></summary>

public static PocoSimpleNameSyntax Transform_Simple_Name ( SimpleNameSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case IdentifierNameSyntax _: return Transform_Identifier_Name((IdentifierNameSyntax)node); 
case GenericNameSyntax _: return Transform_Generic_Name((GenericNameSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoSingleVariableDesignationSyntax Transform_Single_Variable_Designation ( SingleVariableDesignationSyntax node) {
if(node == null) {
return null;
}
	return new PocoSingleVariableDesignationSyntax() { 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText },  };

}


/// <summary></summary>

public static PocoSizeOfExpressionSyntax Transform_Size_Of_Expression ( SizeOfExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoSizeOfExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Type = Transform_Type(node.Type), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoSkippedTokensTriviaSyntax Transform_Skipped_Tokens_Trivia ( SkippedTokensTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoSkippedTokensTriviaSyntax() { 
Tokens = node.Tokens.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(),  };

}


/// <summary></summary>

public static PocoStackAllocArrayCreationExpressionSyntax Transform_Stack_Alloc_Array_Creation_Expression ( StackAllocArrayCreationExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoStackAllocArrayCreationExpressionSyntax() { 
StackAllocKeyword = new PocoSyntaxToken {RawKind = node.StackAllocKeyword.RawKind, Kind = node.StackAllocKeyword.Kind().ToString(), Value = node.StackAllocKeyword.Value, ValueText = node.StackAllocKeyword.ValueText }, 
Type = Transform_Type(node.Type), 
Initializer = Transform_Initializer_Expression(node.Initializer),  };

}


/// <summary></summary>

public static PocoStatementSyntax Transform_Statement ( StatementSyntax node) {
if(node == null) {
return null;
}
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

public static PocoStructDeclarationSyntax Transform_Struct_Declaration ( StructDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoStructDeclarationSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
Modifiers = node.Modifiers.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
TypeParameterList = Transform_Type_Parameter_List(node.TypeParameterList), 
BaseList = Transform_Base_List(node.BaseList), 
ConstraintClauses = node.ConstraintClauses.Select(Transform_Type_Parameter_Constraint_Clause).ToList(), 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Members = node.Members.Select(Transform_Member_Declaration).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText }, 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoStructuredTriviaSyntax Transform_Structured_Trivia ( StructuredTriviaSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case SkippedTokensTriviaSyntax _: return Transform_Skipped_Tokens_Trivia((SkippedTokensTriviaSyntax)node); 
case DocumentationCommentTriviaSyntax _: return Transform_Documentation_Comment_Trivia((DocumentationCommentTriviaSyntax)node); 
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

public static PocoSubpatternSyntax Transform_Subpattern ( SubpatternSyntax node) {
if(node == null) {
return null;
}
	return new PocoSubpatternSyntax() { 
NameColon = Transform_Name_Colon(node.NameColon), 
Pattern = Transform_Pattern(node.Pattern),  };

}


/// <summary></summary>

public static PocoSwitchExpressionArmSyntax Transform_Switch_Expression_Arm ( SwitchExpressionArmSyntax node) {
if(node == null) {
return null;
}
	return new PocoSwitchExpressionArmSyntax() { 
Pattern = Transform_Pattern(node.Pattern), 
WhenClause = Transform_When_Clause(node.WhenClause), 
EqualsGreaterThanToken = new PocoSyntaxToken {RawKind = node.EqualsGreaterThanToken.RawKind, Kind = node.EqualsGreaterThanToken.Kind().ToString(), Value = node.EqualsGreaterThanToken.Value, ValueText = node.EqualsGreaterThanToken.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoSwitchExpressionSyntax Transform_Switch_Expression ( SwitchExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoSwitchExpressionSyntax() { 
GoverningExpression = Transform_Expression(node.GoverningExpression), 
SwitchKeyword = new PocoSyntaxToken {RawKind = node.SwitchKeyword.RawKind, Kind = node.SwitchKeyword.Kind().ToString(), Value = node.SwitchKeyword.Value, ValueText = node.SwitchKeyword.ValueText }, 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Arms = node.Arms.Select(Transform_Switch_Expression_Arm).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoSwitchLabelSyntax Transform_Switch_Label ( SwitchLabelSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case CasePatternSwitchLabelSyntax _: return Transform_Case_Pattern_Switch_Label((CasePatternSwitchLabelSyntax)node); 
case CaseSwitchLabelSyntax _: return Transform_Case_Switch_Label((CaseSwitchLabelSyntax)node); 
case DefaultSwitchLabelSyntax _: return Transform_Default_Switch_Label((DefaultSwitchLabelSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoSwitchSectionSyntax Transform_Switch_Section ( SwitchSectionSyntax node) {
if(node == null) {
return null;
}
	return new PocoSwitchSectionSyntax() { 
Labels = node.Labels.Select(Transform_Switch_Label).ToList(), 
Statements = node.Statements.Select(Transform_Statement).ToList(),  };

}


/// <summary></summary>

public static PocoSwitchStatementSyntax Transform_Switch_Statement ( SwitchStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoSwitchStatementSyntax() { 
SwitchKeyword = new PocoSyntaxToken {RawKind = node.SwitchKeyword.RawKind, Kind = node.SwitchKeyword.Kind().ToString(), Value = node.SwitchKeyword.Value, ValueText = node.SwitchKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Expression = Transform_Expression(node.Expression), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
OpenBraceToken = new PocoSyntaxToken {RawKind = node.OpenBraceToken.RawKind, Kind = node.OpenBraceToken.Kind().ToString(), Value = node.OpenBraceToken.Value, ValueText = node.OpenBraceToken.ValueText }, 
Sections = node.Sections.Select(Transform_Switch_Section).ToList(), 
CloseBraceToken = new PocoSyntaxToken {RawKind = node.CloseBraceToken.RawKind, Kind = node.CloseBraceToken.Kind().ToString(), Value = node.CloseBraceToken.Value, ValueText = node.CloseBraceToken.ValueText },  };

}


/// <summary></summary>

public static PocoThisExpressionSyntax Transform_This_Expression ( ThisExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoThisExpressionSyntax() { 
Token = new PocoSyntaxToken {RawKind = node.Token.RawKind, Kind = node.Token.Kind().ToString(), Value = node.Token.Value, ValueText = node.Token.ValueText },  };

}


/// <summary></summary>

public static PocoThrowExpressionSyntax Transform_Throw_Expression ( ThrowExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoThrowExpressionSyntax() { 
ThrowKeyword = new PocoSyntaxToken {RawKind = node.ThrowKeyword.RawKind, Kind = node.ThrowKeyword.Kind().ToString(), Value = node.ThrowKeyword.Value, ValueText = node.ThrowKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoThrowStatementSyntax Transform_Throw_Statement ( ThrowStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoThrowStatementSyntax() { 
ThrowKeyword = new PocoSyntaxToken {RawKind = node.ThrowKeyword.RawKind, Kind = node.ThrowKeyword.Kind().ToString(), Value = node.ThrowKeyword.Value, ValueText = node.ThrowKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}


/// <summary></summary>

public static PocoTryStatementSyntax Transform_Try_Statement ( TryStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoTryStatementSyntax() { 
TryKeyword = new PocoSyntaxToken {RawKind = node.TryKeyword.RawKind, Kind = node.TryKeyword.Kind().ToString(), Value = node.TryKeyword.Value, ValueText = node.TryKeyword.ValueText }, 
Block = Transform_Block(node.Block), 
Catches = node.Catches.Select(Transform_Catch_Clause).ToList(), 
Finally = Transform_Finally_Clause(node.Finally),  };

}


/// <summary></summary>

public static PocoTupleElementSyntax Transform_Tuple_Element ( TupleElementSyntax node) {
if(node == null) {
return null;
}
	return new PocoTupleElementSyntax() { 
Type = Transform_Type(node.Type), 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText },  };

}


/// <summary></summary>

public static PocoTupleExpressionSyntax Transform_Tuple_Expression ( TupleExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoTupleExpressionSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Arguments = node.Arguments.Select(Transform_Argument).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoTupleTypeSyntax Transform_Tuple_Type ( TupleTypeSyntax node) {
if(node == null) {
return null;
}
	return new PocoTupleTypeSyntax() { 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Elements = node.Elements.Select(Transform_Tuple_Element).ToList(), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoTypeArgumentListSyntax Transform_Type_Argument_List ( TypeArgumentListSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeArgumentListSyntax() { 
LessThanToken = new PocoSyntaxToken {RawKind = node.LessThanToken.RawKind, Kind = node.LessThanToken.Kind().ToString(), Value = node.LessThanToken.Value, ValueText = node.LessThanToken.ValueText }, 
Arguments = node.Arguments.Select(Transform_Type).ToList(), 
GreaterThanToken = new PocoSyntaxToken {RawKind = node.GreaterThanToken.RawKind, Kind = node.GreaterThanToken.Kind().ToString(), Value = node.GreaterThanToken.Value, ValueText = node.GreaterThanToken.ValueText },  };

}


/// <summary></summary>

public static PocoTypeConstraintSyntax Transform_Type_Constraint ( TypeConstraintSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeConstraintSyntax() { 
Type = Transform_Type(node.Type),  };

}


/// <summary></summary>

public static PocoTypeCrefSyntax Transform_Type_Cref ( TypeCrefSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeCrefSyntax() { 
Type = Transform_Type(node.Type),  };

}


/// <summary></summary>

public static PocoTypeDeclarationSyntax Transform_Type_Declaration ( TypeDeclarationSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ClassDeclarationSyntax _: return Transform_Class_Declaration((ClassDeclarationSyntax)node); 
case StructDeclarationSyntax _: return Transform_Struct_Declaration((StructDeclarationSyntax)node); 
case InterfaceDeclarationSyntax _: return Transform_Interface_Declaration((InterfaceDeclarationSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoTypeOfExpressionSyntax Transform_Type_Of_Expression ( TypeOfExpressionSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeOfExpressionSyntax() { 
Keyword = new PocoSyntaxToken {RawKind = node.Keyword.RawKind, Kind = node.Keyword.Kind().ToString(), Value = node.Keyword.Value, ValueText = node.Keyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Type = Transform_Type(node.Type), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText },  };

}


/// <summary></summary>

public static PocoTypeParameterConstraintClauseSyntax Transform_Type_Parameter_Constraint_Clause ( TypeParameterConstraintClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeParameterConstraintClauseSyntax() { 
WhereKeyword = new PocoSyntaxToken {RawKind = node.WhereKeyword.RawKind, Kind = node.WhereKeyword.Kind().ToString(), Value = node.WhereKeyword.Value, ValueText = node.WhereKeyword.ValueText }, 
Name = Transform_Identifier_Name(node.Name), 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText }, 
Constraints = node.Constraints.Select(Transform_Type_Parameter_Constraint).ToList(),  };

}


/// <summary></summary>

public static PocoTypeParameterConstraintSyntax Transform_Type_Parameter_Constraint ( TypeParameterConstraintSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case ConstructorConstraintSyntax _: return Transform_Constructor_Constraint((ConstructorConstraintSyntax)node); 
case ClassOrStructConstraintSyntax _: return Transform_Class_Or_Struct_Constraint((ClassOrStructConstraintSyntax)node); 
case TypeConstraintSyntax _: return Transform_Type_Constraint((TypeConstraintSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoTypeParameterListSyntax Transform_Type_Parameter_List ( TypeParameterListSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeParameterListSyntax() { 
LessThanToken = new PocoSyntaxToken {RawKind = node.LessThanToken.RawKind, Kind = node.LessThanToken.Kind().ToString(), Value = node.LessThanToken.Value, ValueText = node.LessThanToken.ValueText }, 
Parameters = node.Parameters.Select(Transform_Type_Parameter).ToList(), 
GreaterThanToken = new PocoSyntaxToken {RawKind = node.GreaterThanToken.RawKind, Kind = node.GreaterThanToken.Kind().ToString(), Value = node.GreaterThanToken.Value, ValueText = node.GreaterThanToken.ValueText },  };

}


/// <summary></summary>

public static PocoTypeParameterSyntax Transform_Type_Parameter ( TypeParameterSyntax node) {
if(node == null) {
return null;
}
	return new PocoTypeParameterSyntax() { 
AttributeLists = node.AttributeLists.Select(Transform_Attribute_List).ToList(), 
VarianceKeyword = new PocoSyntaxToken {RawKind = node.VarianceKeyword.RawKind, Kind = node.VarianceKeyword.Kind().ToString(), Value = node.VarianceKeyword.Value, ValueText = node.VarianceKeyword.ValueText }, 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText },  };

}


/// <summary></summary>

public static PocoTypeSyntax Transform_Type ( TypeSyntax node) {
if(node == null) {
return null;
}
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

public static PocoUndefDirectiveTriviaSyntax Transform_Undef_Directive_Trivia ( UndefDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoUndefDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
UndefKeyword = new PocoSyntaxToken {RawKind = node.UndefKeyword.RawKind, Kind = node.UndefKeyword.Kind().ToString(), Value = node.UndefKeyword.Value, ValueText = node.UndefKeyword.ValueText }, 
Name = new PocoSyntaxToken {RawKind = node.Name.RawKind, Kind = node.Name.Kind().ToString(), Value = node.Name.Value, ValueText = node.Name.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoUnsafeStatementSyntax Transform_Unsafe_Statement ( UnsafeStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoUnsafeStatementSyntax() { 
UnsafeKeyword = new PocoSyntaxToken {RawKind = node.UnsafeKeyword.RawKind, Kind = node.UnsafeKeyword.Kind().ToString(), Value = node.UnsafeKeyword.Value, ValueText = node.UnsafeKeyword.ValueText }, 
Block = Transform_Block(node.Block),  };

}


/// <summary></summary>

public static PocoUsingDirectiveSyntax Transform_Using_Directive ( UsingDirectiveSyntax node) {
if(node == null) {
return null;
}
	return new PocoUsingDirectiveSyntax() { 
UsingKeyword = new PocoSyntaxToken {RawKind = node.UsingKeyword.RawKind, Kind = node.UsingKeyword.Kind().ToString(), Value = node.UsingKeyword.Value, ValueText = node.UsingKeyword.ValueText }, 
Name = Transform_Name(node.Name), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText }, 
StaticKeyword = new PocoSyntaxToken {RawKind = node.StaticKeyword.RawKind, Kind = node.StaticKeyword.Kind().ToString(), Value = node.StaticKeyword.Value, ValueText = node.StaticKeyword.ValueText }, 
Alias = Transform_Name_Equals(node.Alias),  };

}


/// <summary></summary>

public static PocoUsingStatementSyntax Transform_Using_Statement ( UsingStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoUsingStatementSyntax() { 
AwaitKeyword = new PocoSyntaxToken {RawKind = node.AwaitKeyword.RawKind, Kind = node.AwaitKeyword.Kind().ToString(), Value = node.AwaitKeyword.Value, ValueText = node.AwaitKeyword.ValueText }, 
UsingKeyword = new PocoSyntaxToken {RawKind = node.UsingKeyword.RawKind, Kind = node.UsingKeyword.Kind().ToString(), Value = node.UsingKeyword.Value, ValueText = node.UsingKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement), 
Declaration = Transform_Variable_Declaration(node.Declaration), 
Expression = Transform_Expression(node.Expression),  };

}


/// <summary></summary>

public static PocoVariableDeclarationSyntax Transform_Variable_Declaration ( VariableDeclarationSyntax node) {
if(node == null) {
return null;
}
	return new PocoVariableDeclarationSyntax() { 
Type = Transform_Type(node.Type), 
Variables = node.Variables.Select(Transform_Variable_Declarator).ToList(),  };

}


/// <summary></summary>

public static PocoVariableDeclaratorSyntax Transform_Variable_Declarator ( VariableDeclaratorSyntax node) {
if(node == null) {
return null;
}
	return new PocoVariableDeclaratorSyntax() { 
Identifier = new PocoSyntaxToken {RawKind = node.Identifier.RawKind, Kind = node.Identifier.Kind().ToString(), Value = node.Identifier.Value, ValueText = node.Identifier.ValueText }, 
ArgumentList = Transform_Bracketed_Argument_List(node.ArgumentList), 
Initializer = Transform_Equals_Value_Clause(node.Initializer),  };

}


/// <summary></summary>

public static PocoVariableDesignationSyntax Transform_Variable_Designation ( VariableDesignationSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case SingleVariableDesignationSyntax _: return Transform_Single_Variable_Designation((SingleVariableDesignationSyntax)node); 
case DiscardDesignationSyntax _: return Transform_Discard_Designation((DiscardDesignationSyntax)node); 
case ParenthesizedVariableDesignationSyntax _: return Transform_Parenthesized_Variable_Designation((ParenthesizedVariableDesignationSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoVarPatternSyntax Transform_Var_Pattern ( VarPatternSyntax node) {
if(node == null) {
return null;
}
	return new PocoVarPatternSyntax() { 
VarKeyword = new PocoSyntaxToken {RawKind = node.VarKeyword.RawKind, Kind = node.VarKeyword.Kind().ToString(), Value = node.VarKeyword.Value, ValueText = node.VarKeyword.ValueText }, 
Designation = Transform_Variable_Designation(node.Designation),  };

}


/// <summary></summary>

public static PocoWarningDirectiveTriviaSyntax Transform_Warning_Directive_Trivia ( WarningDirectiveTriviaSyntax node) {
if(node == null) {
return null;
}
	return new PocoWarningDirectiveTriviaSyntax() { 
HashToken = new PocoSyntaxToken {RawKind = node.HashToken.RawKind, Kind = node.HashToken.Kind().ToString(), Value = node.HashToken.Value, ValueText = node.HashToken.ValueText }, 
WarningKeyword = new PocoSyntaxToken {RawKind = node.WarningKeyword.RawKind, Kind = node.WarningKeyword.Kind().ToString(), Value = node.WarningKeyword.Value, ValueText = node.WarningKeyword.ValueText }, 
EndOfDirectiveToken = new PocoSyntaxToken {RawKind = node.EndOfDirectiveToken.RawKind, Kind = node.EndOfDirectiveToken.Kind().ToString(), Value = node.EndOfDirectiveToken.Value, ValueText = node.EndOfDirectiveToken.ValueText }, 
IsActive = node.IsActive,  };

}


/// <summary></summary>

public static PocoWhenClauseSyntax Transform_When_Clause ( WhenClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoWhenClauseSyntax() { 
WhenKeyword = new PocoSyntaxToken {RawKind = node.WhenKeyword.RawKind, Kind = node.WhenKeyword.Kind().ToString(), Value = node.WhenKeyword.Value, ValueText = node.WhenKeyword.ValueText }, 
Condition = Transform_Expression(node.Condition),  };

}


/// <summary></summary>

public static PocoWhereClauseSyntax Transform_Where_Clause ( WhereClauseSyntax node) {
if(node == null) {
return null;
}
	return new PocoWhereClauseSyntax() { 
WhereKeyword = new PocoSyntaxToken {RawKind = node.WhereKeyword.RawKind, Kind = node.WhereKeyword.Kind().ToString(), Value = node.WhereKeyword.Value, ValueText = node.WhereKeyword.ValueText }, 
Condition = Transform_Expression(node.Condition),  };

}


/// <summary></summary>

public static PocoWhileStatementSyntax Transform_While_Statement ( WhileStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoWhileStatementSyntax() { 
WhileKeyword = new PocoSyntaxToken {RawKind = node.WhileKeyword.RawKind, Kind = node.WhileKeyword.Kind().ToString(), Value = node.WhileKeyword.Value, ValueText = node.WhileKeyword.ValueText }, 
OpenParenToken = new PocoSyntaxToken {RawKind = node.OpenParenToken.RawKind, Kind = node.OpenParenToken.Kind().ToString(), Value = node.OpenParenToken.Value, ValueText = node.OpenParenToken.ValueText }, 
Condition = Transform_Expression(node.Condition), 
CloseParenToken = new PocoSyntaxToken {RawKind = node.CloseParenToken.RawKind, Kind = node.CloseParenToken.Kind().ToString(), Value = node.CloseParenToken.Value, ValueText = node.CloseParenToken.ValueText }, 
Statement = Transform_Statement(node.Statement),  };

}


/// <summary></summary>

public static PocoXmlAttributeSyntax Transform_Xml_Attribute ( XmlAttributeSyntax node) {
if(node == null) {
return null;
}
	switch(node) {
case XmlTextAttributeSyntax _: return Transform_Xml_Text_Attribute((XmlTextAttributeSyntax)node); 
case XmlCrefAttributeSyntax _: return Transform_Xml_Cref_Attribute((XmlCrefAttributeSyntax)node); 
case XmlNameAttributeSyntax _: return Transform_Xml_Name_Attribute((XmlNameAttributeSyntax)node); 

}
return null;


}


/// <summary></summary>

public static PocoXmlCDataSectionSyntax Transform_Xml_CData_Section ( XmlCDataSectionSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlCDataSectionSyntax() { 
StartCDataToken = new PocoSyntaxToken {RawKind = node.StartCDataToken.RawKind, Kind = node.StartCDataToken.Kind().ToString(), Value = node.StartCDataToken.Value, ValueText = node.StartCDataToken.ValueText }, 
TextTokens = node.TextTokens.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
EndCDataToken = new PocoSyntaxToken {RawKind = node.EndCDataToken.RawKind, Kind = node.EndCDataToken.Kind().ToString(), Value = node.EndCDataToken.Value, ValueText = node.EndCDataToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlCommentSyntax Transform_Xml_Comment ( XmlCommentSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlCommentSyntax() { 
LessThanExclamationMinusMinusToken = new PocoSyntaxToken {RawKind = node.LessThanExclamationMinusMinusToken.RawKind, Kind = node.LessThanExclamationMinusMinusToken.Kind().ToString(), Value = node.LessThanExclamationMinusMinusToken.Value, ValueText = node.LessThanExclamationMinusMinusToken.ValueText }, 
TextTokens = node.TextTokens.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
MinusMinusGreaterThanToken = new PocoSyntaxToken {RawKind = node.MinusMinusGreaterThanToken.RawKind, Kind = node.MinusMinusGreaterThanToken.Kind().ToString(), Value = node.MinusMinusGreaterThanToken.Value, ValueText = node.MinusMinusGreaterThanToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlCrefAttributeSyntax Transform_Xml_Cref_Attribute ( XmlCrefAttributeSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlCrefAttributeSyntax() { 
Name = Transform_Xml_Name(node.Name), 
EqualsToken = new PocoSyntaxToken {RawKind = node.EqualsToken.RawKind, Kind = node.EqualsToken.Kind().ToString(), Value = node.EqualsToken.Value, ValueText = node.EqualsToken.ValueText }, 
StartQuoteToken = new PocoSyntaxToken {RawKind = node.StartQuoteToken.RawKind, Kind = node.StartQuoteToken.Kind().ToString(), Value = node.StartQuoteToken.Value, ValueText = node.StartQuoteToken.ValueText }, 
Cref = Transform_Cref(node.Cref), 
EndQuoteToken = new PocoSyntaxToken {RawKind = node.EndQuoteToken.RawKind, Kind = node.EndQuoteToken.Kind().ToString(), Value = node.EndQuoteToken.Value, ValueText = node.EndQuoteToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlElementEndTagSyntax Transform_Xml_Element_End_Tag ( XmlElementEndTagSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlElementEndTagSyntax() { 
LessThanSlashToken = new PocoSyntaxToken {RawKind = node.LessThanSlashToken.RawKind, Kind = node.LessThanSlashToken.Kind().ToString(), Value = node.LessThanSlashToken.Value, ValueText = node.LessThanSlashToken.ValueText }, 
Name = Transform_Xml_Name(node.Name), 
GreaterThanToken = new PocoSyntaxToken {RawKind = node.GreaterThanToken.RawKind, Kind = node.GreaterThanToken.Kind().ToString(), Value = node.GreaterThanToken.Value, ValueText = node.GreaterThanToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlElementStartTagSyntax Transform_Xml_Element_Start_Tag ( XmlElementStartTagSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlElementStartTagSyntax() { 
LessThanToken = new PocoSyntaxToken {RawKind = node.LessThanToken.RawKind, Kind = node.LessThanToken.Kind().ToString(), Value = node.LessThanToken.Value, ValueText = node.LessThanToken.ValueText }, 
Name = Transform_Xml_Name(node.Name), 
Attributes = node.Attributes.Select(Transform_Xml_Attribute).ToList(), 
GreaterThanToken = new PocoSyntaxToken {RawKind = node.GreaterThanToken.RawKind, Kind = node.GreaterThanToken.Kind().ToString(), Value = node.GreaterThanToken.Value, ValueText = node.GreaterThanToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlElementSyntax Transform_Xml_Element ( XmlElementSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlElementSyntax() { 
StartTag = Transform_Xml_Element_Start_Tag(node.StartTag), 
Content = node.Content.Select(Transform_Xml_Node).ToList(), 
EndTag = Transform_Xml_Element_End_Tag(node.EndTag),  };

}


/// <summary></summary>

public static PocoXmlEmptyElementSyntax Transform_Xml_Empty_Element ( XmlEmptyElementSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlEmptyElementSyntax() { 
LessThanToken = new PocoSyntaxToken {RawKind = node.LessThanToken.RawKind, Kind = node.LessThanToken.Kind().ToString(), Value = node.LessThanToken.Value, ValueText = node.LessThanToken.ValueText }, 
Name = Transform_Xml_Name(node.Name), 
Attributes = node.Attributes.Select(Transform_Xml_Attribute).ToList(), 
SlashGreaterThanToken = new PocoSyntaxToken {RawKind = node.SlashGreaterThanToken.RawKind, Kind = node.SlashGreaterThanToken.Kind().ToString(), Value = node.SlashGreaterThanToken.Value, ValueText = node.SlashGreaterThanToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlNameAttributeSyntax Transform_Xml_Name_Attribute ( XmlNameAttributeSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlNameAttributeSyntax() { 
Name = Transform_Xml_Name(node.Name), 
EqualsToken = new PocoSyntaxToken {RawKind = node.EqualsToken.RawKind, Kind = node.EqualsToken.Kind().ToString(), Value = node.EqualsToken.Value, ValueText = node.EqualsToken.ValueText }, 
StartQuoteToken = new PocoSyntaxToken {RawKind = node.StartQuoteToken.RawKind, Kind = node.StartQuoteToken.Kind().ToString(), Value = node.StartQuoteToken.Value, ValueText = node.StartQuoteToken.ValueText }, 
Identifier = Transform_Identifier_Name(node.Identifier), 
EndQuoteToken = new PocoSyntaxToken {RawKind = node.EndQuoteToken.RawKind, Kind = node.EndQuoteToken.Kind().ToString(), Value = node.EndQuoteToken.Value, ValueText = node.EndQuoteToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlNameSyntax Transform_Xml_Name ( XmlNameSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlNameSyntax() { 
Prefix = Transform_Xml_Prefix(node.Prefix), 
LocalName = new PocoSyntaxToken {RawKind = node.LocalName.RawKind, Kind = node.LocalName.Kind().ToString(), Value = node.LocalName.Value, ValueText = node.LocalName.ValueText },  };

}


/// <summary></summary>

public static PocoXmlNodeSyntax Transform_Xml_Node ( XmlNodeSyntax node) {
if(node == null) {
return null;
}
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

public static PocoXmlPrefixSyntax Transform_Xml_Prefix ( XmlPrefixSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlPrefixSyntax() { 
Prefix = new PocoSyntaxToken {RawKind = node.Prefix.RawKind, Kind = node.Prefix.Kind().ToString(), Value = node.Prefix.Value, ValueText = node.Prefix.ValueText }, 
ColonToken = new PocoSyntaxToken {RawKind = node.ColonToken.RawKind, Kind = node.ColonToken.Kind().ToString(), Value = node.ColonToken.Value, ValueText = node.ColonToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlProcessingInstructionSyntax Transform_Xml_Processing_Instruction ( XmlProcessingInstructionSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlProcessingInstructionSyntax() { 
StartProcessingInstructionToken = new PocoSyntaxToken {RawKind = node.StartProcessingInstructionToken.RawKind, Kind = node.StartProcessingInstructionToken.Kind().ToString(), Value = node.StartProcessingInstructionToken.Value, ValueText = node.StartProcessingInstructionToken.ValueText }, 
Name = Transform_Xml_Name(node.Name), 
TextTokens = node.TextTokens.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
EndProcessingInstructionToken = new PocoSyntaxToken {RawKind = node.EndProcessingInstructionToken.RawKind, Kind = node.EndProcessingInstructionToken.Kind().ToString(), Value = node.EndProcessingInstructionToken.Value, ValueText = node.EndProcessingInstructionToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlTextAttributeSyntax Transform_Xml_Text_Attribute ( XmlTextAttributeSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlTextAttributeSyntax() { 
Name = Transform_Xml_Name(node.Name), 
EqualsToken = new PocoSyntaxToken {RawKind = node.EqualsToken.RawKind, Kind = node.EqualsToken.Kind().ToString(), Value = node.EqualsToken.Value, ValueText = node.EqualsToken.ValueText }, 
StartQuoteToken = new PocoSyntaxToken {RawKind = node.StartQuoteToken.RawKind, Kind = node.StartQuoteToken.Kind().ToString(), Value = node.StartQuoteToken.Value, ValueText = node.StartQuoteToken.ValueText }, 
TextTokens = node.TextTokens.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(), 
EndQuoteToken = new PocoSyntaxToken {RawKind = node.EndQuoteToken.RawKind, Kind = node.EndQuoteToken.Kind().ToString(), Value = node.EndQuoteToken.Value, ValueText = node.EndQuoteToken.ValueText },  };

}


/// <summary></summary>

public static PocoXmlTextSyntax Transform_Xml_Text ( XmlTextSyntax node) {
if(node == null) {
return null;
}
	return new PocoXmlTextSyntax() { 
TextTokens = node.TextTokens.Select(v => new PocoSyntaxToken {RawKind = v.RawKind, Kind = v.Kind().ToString(), Value = v.Value, ValueText = v.ValueText }).ToList(),  };

}


/// <summary></summary>

public static PocoYieldStatementSyntax Transform_Yield_Statement ( YieldStatementSyntax node) {
if(node == null) {
return null;
}
	return new PocoYieldStatementSyntax() { 
YieldKeyword = new PocoSyntaxToken {RawKind = node.YieldKeyword.RawKind, Kind = node.YieldKeyword.Kind().ToString(), Value = node.YieldKeyword.Value, ValueText = node.YieldKeyword.ValueText }, 
ReturnOrBreakKeyword = new PocoSyntaxToken {RawKind = node.ReturnOrBreakKeyword.RawKind, Kind = node.ReturnOrBreakKeyword.Kind().ToString(), Value = node.ReturnOrBreakKeyword.Value, ValueText = node.ReturnOrBreakKeyword.ValueText }, 
Expression = Transform_Expression(node.Expression), 
SemicolonToken = new PocoSyntaxToken {RawKind = node.SemicolonToken.RawKind, Kind = node.SemicolonToken.Kind().ToString(), Value = node.SemicolonToken.Value, ValueText = node.SemicolonToken.ValueText },  };

}

}
}