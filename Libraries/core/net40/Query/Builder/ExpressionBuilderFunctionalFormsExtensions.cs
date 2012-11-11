using System;
using System.Linq;
using VDS.RDF.Query.Builder.Expressions;
using VDS.RDF.Query.Expressions;
using VDS.RDF.Query.Expressions.Functions.Sparql;
using VDS.RDF.Query.Expressions.Functions.Sparql.Boolean;

namespace VDS.RDF.Query.Builder
{
    public static class ExpressionBuilderFunctionalFormsExtensions
    {
        public static BooleanExpression Bound(this ExpressionBuilder eb, VariableExpression var)
        {
            return new BooleanExpression(new BoundFunction(var.Expression));
        }

        public static BooleanExpression Bound(this ExpressionBuilder eb, string var)
        {
            return Bound(eb, eb.Variable(var));
        }

        public static IfThenPart If(this ExpressionBuilder eb, BooleanExpression ifExpression)
        {
            return new IfThenPart(ifExpression.Expression);
        }

        public static IfThenPart If(this ExpressionBuilder eb, VariableExpression ifExpression)
        {
            return new IfThenPart(ifExpression.Expression);
        }

        public static RdfTermExpression Coalesce(this ExpressionBuilder eb, params SparqlExpression[] expressions)
        {
            var coalesce = new CoalesceFunction(expressions.Select(e => e.Expression));
            return new RdfTermExpression(coalesce);
        }

        public static BooleanExpression Exists(this ExpressionBuilder eb, Action<IGraphPatternBuilder> buildExistsPattern)
        {
            GraphPatternBuilder builder = new GraphPatternBuilder(eb.Prefixes);
            buildExistsPattern(builder);
            var existsFunction = new ExistsFunction(builder.BuildGraphPattern(), true);
            return new BooleanExpression(existsFunction);
        }

        public static BooleanExpression SameTerm(this ExpressionBuilder eb, SparqlExpression left, SparqlExpression right)
        {
            var sameTerm = new SameTermFunction(left.Expression, right.Expression);
            return new BooleanExpression(sameTerm);
        }
    }

    public sealed class IfThenPart
    {
        private readonly ISparqlExpression _ifExpression;

        internal IfThenPart(ISparqlExpression ifExpression)
        {
            _ifExpression = ifExpression;
        }

        public IfElsePart Then(SparqlExpression thenExpression)
        {
            return new IfElsePart(_ifExpression, thenExpression.Expression);
        }
    }

    public sealed class IfElsePart
    {
        private readonly ISparqlExpression _ifExpression;
        private readonly ISparqlExpression _thenExpression;

        internal IfElsePart(ISparqlExpression ifExpression, ISparqlExpression thenExpression)
        {
            _ifExpression = ifExpression;
            _thenExpression = thenExpression;
        }

        public RdfTermExpression Else(SparqlExpression elseExpression)
        {
            var ifElseFunc = new IfElseFunction(_ifExpression, _thenExpression, elseExpression.Expression);
            return new RdfTermExpression(ifElseFunc);
        }
    }
}