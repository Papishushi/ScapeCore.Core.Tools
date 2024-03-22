using System.Collections.Generic;

namespace ScapeCore.Core.Tools
{
    public interface IScapeCoreHost
    {
        protected List<IScapeCoreManager?> Managers { get; init; }
        public void Load(IScapeCoreManager manager);
        public void Load(params IScapeCoreManager[] managers);
        public void Unload(IScapeCoreManager manager);
        public void Unload(params IScapeCoreManager[] managers);
    }
}
