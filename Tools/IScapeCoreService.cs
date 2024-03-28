using System;

namespace ScapeCore.Core.Tools
{
    public interface IScapeCoreService 
    { 
        public string Name { get; }
        public Guid Id { get; }
    }
}