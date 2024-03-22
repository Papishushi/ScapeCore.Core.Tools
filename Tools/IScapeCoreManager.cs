using ProtoBuf.Meta;
using ScapeCore.Core.Serialization.Streamers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScapeCore.Core.Tools
{
    public interface IScapeCoreManager
    {
        protected List<IScapeCoreService?> Services { get; }
        public bool InjectDependencies(params IScapeCoreService[] services);
    }
}