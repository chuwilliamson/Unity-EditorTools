using System.Collections.Generic;

namespace Interface
{
    public interface IPackable
    {
        void TryAddItem(IPacker packer);
    }

    public interface IPacker
    {
        bool PackItem(IPackable packable);        
        void UnpackItem(IPackable packable);
    }
}