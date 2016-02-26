using System;
using Zenject;

namespace MistRidge
{
    public interface IItem : IInitializable, IDisposable, ITickable
    {
        void Use();

        bool IsUsable();

        bool IsActive();

        bool IsDisposable();
    }
}
