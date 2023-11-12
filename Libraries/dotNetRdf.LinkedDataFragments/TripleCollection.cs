﻿/*
// <copyright>
// dotNetRDF is free and open source software licensed under the MIT License
// -------------------------------------------------------------------------
// 
// Copyright (c) 2009-2023 dotNetRDF Project (http://dotnetrdf.org/)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF.LinkedPatternFragments.Hydra;

namespace VDS.RDF.LinkedPatternFragments
{
    internal class TripleCollection : BaseTripleCollection
    {
        private readonly IriTemplate template;

        internal TripleCollection(IriTemplate template)
        {
            this.template = template;
        }

        public override int Count
        {
            get
            {
                using var ts = new TripleStore(new Parameters(this.template));
                return (int)ts.Metadata.TripleCount;
            }
        }

        public override IEnumerable<INode> ObjectNodes
        {
            get
            {
                return this.Select(t => t.Object).Distinct();
            }
        }

        public override IEnumerable<INode> PredicateNodes
        {
            get
            {
                return this.Select(t => t.Predicate).Distinct();
            }
        }

        public override IEnumerable<INode> SubjectNodes
        {
            get
            {
                return this.Select(t => t.Subject).Distinct();
            }
        }

        public override Triple this[Triple t]
        {
            get
            {
                if (this.Contains(t))
                {
                    return t;
                }

                throw new KeyNotFoundException();
            }
        }

        public override bool Contains(Triple t)
        {
            return new Enumerable(new Parameters(this.template, t.Subject, t.Predicate, t.Object)).Any();
        }

        public override void Dispose()
        {
        }

        public override IEnumerator<Triple> GetEnumerator()
        {
            return new Enumerator(new Parameters(this.template));
        }

        public override IEnumerable<Triple> WithObject(INode obj)
        {
            return new Enumerable(new Parameters(this.template, @object: obj));
        }

        public override IEnumerable<Triple> WithPredicate(INode pred)
        {
            return new Enumerable(new Parameters(this.template, predicate: pred));
        }

        public override IEnumerable<Triple> WithPredicateObject(INode pred, INode obj)
        {
            return new Enumerable(new Parameters(this.template, predicate: pred, @object: obj));
        }

        public override IEnumerable<Triple> WithSubject(INode subj)
        {
            return new Enumerable(new Parameters(this.template, subj));
        }

        public override IEnumerable<Triple> WithSubjectObject(INode subj, INode obj)
        {
            return new Enumerable(new Parameters(this.template, subj, @object: obj));
        }

        public override IEnumerable<Triple> WithSubjectPredicate(INode subj, INode pred)
        {
            return new Enumerable(new Parameters(this.template, subj, pred));
        }

        public override IEnumerable<Triple> Asserted => this;

        protected override bool Add(Triple t)
        {
            throw new NotSupportedException("This triple collection is read-only.");
        }

        protected override bool Delete(Triple t)
        {
            throw new NotSupportedException("This triple collection is read-only.");
        }

        public override bool ContainsQuoted(Triple t)
        {
            throw new NotSupportedException("This triple collection does not support quoting.");
        }

        public override int QuotedCount => throw new NotSupportedException("This triple collection does not support quoting.");

        public override IEnumerable<INode> QuotedObjectNodes => throw new NotSupportedException("This triple collection does not support quoting.");

        public override IEnumerable<INode> QuotedPredicateNodes => throw new NotSupportedException("This triple collection does not support quoting.");

        public override IEnumerable<INode> QuotedSubjectNodes => throw new NotSupportedException("This triple collection does not support quoting.");

        public override IEnumerable<Triple> Quoted => throw new NotSupportedException("This triple collection does not support quoting.");
    }
}
