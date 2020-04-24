using System ;
using System.Globalization ;
using System.Linq ;
using System.Windows.Data ;
using AnalysisAppLib ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.FlowAnalysis ;
using Microsoft.CodeAnalysis.Operations ;
// ReSharper disable UnusedVariable

namespace ProjInterface
{
    public class SemanticConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            
            if ( value == null ) return null ;
            var csn = value as AnalysisContext ;
            var syntaxNode = csn.Node ;
            var semanticModel = csn.Model ;
            if ( syntaxNode is MethodDeclarationSyntax m )
            {
                var controlFlowAnalysis = semanticModel.AnalyzeControlFlow ( m.Body.Statements.First(), m.Body.Statements.Last() ) ;
                return controlFlowAnalysis ;
            }

            var op = semanticModel.GetOperation ( syntaxNode ) ;
            if ( op != null )
            {
                switch ( op )
                {
                    case null : break ;
                    case ICaughtExceptionOperation caughtExceptionOperation : break ;
                    case IFlowAnonymousFunctionOperation flowAnonymousFunctionOperation : break ;
                    case IFlowCaptureOperation flowCaptureOperation : break ;
                    case IFlowCaptureReferenceOperation flowCaptureReferenceOperation : break ;
                    case IIsNullOperation isNullOperation : break ;
                    case IStaticLocalInitializationSemaphoreOperation staticLocalInitializationSemaphoreOperation : break ;
                    case IAddressOfOperation addressOfOperation : break ;
                    case IAnonymousFunctionOperation anonymousFunctionOperation : break ;
                    case IAnonymousObjectCreationOperation anonymousObjectCreationOperation : break ;
                    case IArgumentOperation argumentOperation : break ;
                    case IArrayCreationOperation arrayCreationOperation : break ;
                    case IArrayElementReferenceOperation arrayElementReferenceOperation : break ;
                    case IArrayInitializerOperation arrayInitializerOperation : break ;
                    case IAwaitOperation awaitOperation : break ;
                    case IBinaryOperation binaryOperation : break ;
                    case IBlockOperation blockOperation :
                        foreach ( var blockOperationLocal in blockOperation.Locals )
                        { 
                        }
                        break ;
                    case IBranchOperation branchOperation : break ;
                    case ICatchClauseOperation catchClauseOperation : break ;
                    case ICoalesceAssignmentOperation coalesceAssignmentOperation : break ;
                    case ICoalesceOperation coalesceOperation : break ;
                    case ICompoundAssignmentOperation compoundAssignmentOperation : break ;
                    case IConditionalAccessInstanceOperation conditionalAccessInstanceOperation : break ;
                    case IConditionalAccessOperation conditionalAccessOperation : break ;
                    case IConditionalOperation conditionalOperation : break ;
                    case IConstantPatternOperation constantPatternOperation : break ;
                    case IConstructorBodyOperation constructorBodyOperation : break ;
                    case IConversionOperation conversionOperation : break ;
                    case IDeclarationExpressionOperation declarationExpressionOperation : break ;
                    case IDeclarationPatternOperation declarationPatternOperation : break ;
                    case IDeconstructionAssignmentOperation deconstructionAssignmentOperation : break ;
                    case IDefaultCaseClauseOperation defaultCaseClauseOperation : break ;
                    case IDefaultValueOperation defaultValueOperation : break ;
                    case IDelegateCreationOperation delegateCreationOperation : break ;
                    case IDiscardOperation discardOperation : break ;
                    case IDiscardPatternOperation discardPatternOperation : break ;
                    case IDynamicIndexerAccessOperation dynamicIndexerAccessOperation : break ;
                    case IDynamicInvocationOperation dynamicInvocationOperation : break ;
                    case IDynamicMemberReferenceOperation dynamicMemberReferenceOperation : break ;
                    case IDynamicObjectCreationOperation dynamicObjectCreationOperation : break ;
                    case IEmptyOperation emptyOperation : break ;
                    case IEndOperation endOperation : break ;
                    case IEventAssignmentOperation eventAssignmentOperation : break ;
                    case IEventReferenceOperation eventReferenceOperation : break ;
                    case IExpressionStatementOperation expressionStatementOperation : break ;
                    case IFieldInitializerOperation fieldInitializerOperation : break ;
                    case IFieldReferenceOperation fieldReferenceOperation : break ;
                    case IForEachLoopOperation forEachLoopOperation : break ;
                    case IForLoopOperation forLoopOperation : break ;
                    case IForToLoopOperation forToLoopOperation : break ;
                    case IIncrementOrDecrementOperation incrementOrDecrementOperation : break ;
                    case IInstanceReferenceOperation instanceReferenceOperation : break ;
                    case IInterpolatedStringOperation interpolatedStringOperation : break ;
                    case IInterpolatedStringTextOperation interpolatedStringTextOperation : break ;
                    case IInterpolationOperation interpolationOperation : break ;
                    case IInvalidOperation invalidOperation : break ;
                    case IInvocationOperation invocationOperation : break ;
                    case IIsPatternOperation isPatternOperation : break ;
                    case IIsTypeOperation isTypeOperation : break ;
                    case ILabeledOperation labeledOperation : break ;
                    case ILiteralOperation literalOperation : break ;
                    case ILocalFunctionOperation localFunctionOperation : break ;
                    case ILocalReferenceOperation localReferenceOperation : break ;
                    case ILockOperation lockOperation : break ;
                    case IMemberInitializerOperation memberInitializerOperation : break ;
                    case IMethodBodyOperation methodBodyOperation : break ;
                    case IMethodReferenceOperation methodReferenceOperation : break ;
                    case INameOfOperation nameOfOperation : break ;
                    case IObjectCreationOperation objectCreationOperation : break ;
                    case IObjectOrCollectionInitializerOperation objectOrCollectionInitializerOperation : break ;
                    case IOmittedArgumentOperation omittedArgumentOperation : break ;
                    case IParameterInitializerOperation parameterInitializerOperation : break ;
                    case IParameterReferenceOperation parameterReferenceOperation : break ;
                    case IParenthesizedOperation parenthesizedOperation : break ;
                    case IPatternCaseClauseOperation patternCaseClauseOperation : break ;
                    case IPropertyInitializerOperation propertyInitializerOperation : break ;
                    case IPropertyReferenceOperation propertyReferenceOperation : break ;
                    case IPropertySubpatternOperation propertySubpatternOperation : break ;
                    case IRaiseEventOperation raiseEventOperation : break ;
                    case IRangeCaseClauseOperation rangeCaseClauseOperation : break ;
                    case IRangeOperation rangeOperation : break ;
                    case IRecursivePatternOperation recursivePatternOperation : break ;
                    case IReDimClauseOperation reDimClauseOperation : break ;
                    case IReDimOperation reDimOperation : break ;
                    case IRelationalCaseClauseOperation relationalCaseClauseOperation : break ;
                    case IReturnOperation returnOperation : break ;
                    case ISimpleAssignmentOperation simpleAssignmentOperation : break ;
                    case ISingleValueCaseClauseOperation singleValueCaseClauseOperation : break ;
                    case ISizeOfOperation sizeOfOperation : break ;
                    case IStopOperation stopOperation : break ;
                    case ISwitchCaseOperation switchCaseOperation : break ;
                    case ISwitchExpressionArmOperation switchExpressionArmOperation : break ;
                    case ISwitchExpressionOperation switchExpressionOperation : break ;
                    case ISwitchOperation switchOperation : break ;
                    case IThrowOperation throwOperation : break ;
                    case ITranslatedQueryOperation translatedQueryOperation : break ;
                    case ITryOperation tryOperation : break ;
                    case ITupleBinaryOperation tupleBinaryOperation : break ;
                    case ITupleOperation tupleOperation : break ;
                    case ITypeOfOperation typeOfOperation : break ;
                    case ITypeParameterObjectCreationOperation typeParameterObjectCreationOperation : break ;
                    case IUnaryOperation unaryOperation : break ;
                    case IUsingDeclarationOperation usingDeclarationOperation : break ;
                    case IUsingOperation usingOperation : break ;
                    case IVariableDeclarationGroupOperation variableDeclarationGroupOperation : break ;
                    case IVariableDeclarationOperation variableDeclarationOperation : break ;
                    case IVariableDeclaratorOperation variableDeclaratorOperation : break ;
                    case IVariableInitializerOperation variableInitializerOperation : break ;
                    case IWhileLoopOperation whileLoopOperation : break ;
                    case IAssignmentOperation assignmentOperation : break ;
                    case ICaseClauseOperation caseClauseOperation : break ;
                    case IInterpolatedStringContentOperation interpolatedStringContentOperation : break ;
                    case ILoopOperation loopOperation : break ;
                    case IMemberReferenceOperation memberReferenceOperation : break ;
                    case IMethodBodyBaseOperation methodBodyBaseOperation : break ;
                    case IPatternOperation patternOperation : break ;
                    case ISymbolInitializerOperation symbolInitializerOperation : break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( op ) ) ;
                }
                return op ;
            }
            var sym = semanticModel.GetDeclaredSymbol ( syntaxNode ) ;
            return sym ;
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}