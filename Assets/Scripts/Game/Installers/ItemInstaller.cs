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

            Container.Bind<IItemDropPickingStrategy>().ToSingle<RandomItemDropPickingStrategy>();

            Container.Bind<IItemFactory>().ToSingle();

            Container.Bind<SpeedItemEffect>().ToSingleInstance(settings.itemEffects.speedItemEffect);
            Container.Bind<StarItemEffect>().ToSingleInstance(settings.itemEffects.starItemEffect);
        }

        private void InstallSettings()
        {
            Container.Bind<ItemManager.Settings>().ToSingleInstance(settings.itemManager);
        }

        [Serializable]
        public class Settings
        {
            public ItemManager.Settings itemManager;
            public ItemEffectSettings itemEffects;

            [Serializable]
            public class ItemEffectSettings
            {
                public SpeedItemEffect speedItemEffect;
                public StarItemEffect starItemEffect;
            }
        }
    }
}
