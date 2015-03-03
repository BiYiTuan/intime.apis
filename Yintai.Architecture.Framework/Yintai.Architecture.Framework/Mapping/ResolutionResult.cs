using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Yintai.Architecture.Framework.Mapping
{
    public class ResolutionResult
    {
        private readonly ResolutionContext _context;
        private List<MemberBinding> _memberBindings;

        public ResolutionResult(ResolutionContext context)
        {
            this._context = context;
            _memberBindings = new List<MemberBinding>();
        }

        public Type SourceType
        {
            get { return _context.SourceType; }
        }

        public Type TargetType
        {
            get { return _context.TargetType; }
        }

        public ResolutionContext Context
        {
            get { return this._context; }
        }

        public List<MemberBinding> MemberBindings
        {
            get { return this._memberBindings; }
            set { this._memberBindings = value; }
        }
    }
}
