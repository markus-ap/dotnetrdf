﻿using System.Collections.Generic;
using VDS.RDF.Query.Engine;
using VDS.RDF.Query.Engine.Algebra;

namespace VDS.RDF.Query.Algebra
{
    public class Reduced
        : BaseUnaryAlgebra
    {
        public Reduced(IAlgebra innerAlgebra) 
            : base(innerAlgebra) { }

        public override void Accept(IAlgebraVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IEnumerable<ISet> Execute(IAlgebraExecutor executor, IExecutionContext context)
        {
            return executor.Execute(this, context);
        }

        public override bool Equals(IAlgebra other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            if (!(other is Reduced)) return false;

            return this.InnerAlgebra.Equals(((Reduced) other).InnerAlgebra);
        }
    }
}
