using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ItemInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InstallItems();
            InstallSettings();
        }

        private void InstallItems()
        {
            Container.Bind<ItemManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ItemManager>();

            Container.Bind<IItemPickingStrategy>().ToSingle<RandomItemPickingStrategy>();
        }

        private void InstallSettings()
        {
            Container.Bind<ItemManager.Settings>().ToSingleInstance(settings.ItemManager);
        }

        [Serializable]
        public class Settings
        {
            public ItemManager.Settings ItemManager;
        }
    }
}
