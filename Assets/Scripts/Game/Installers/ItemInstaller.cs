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

            Container.Bind<IItemDropPickingStrategy>().ToSingle<StandardItemDropPickingStrategy>();

            Container.Bind<IItemFactory>().ToSingle();

            Container.Bind<AetherItemEffect>().ToSingleInstance(settings.itemEffects.aetherItemEffect);
            Container.Bind<QuicknessItemEffect>().ToSingleInstance(settings.itemEffects.quicknessItemEffect);
            Container.Bind<BubbleTrapItemEffect>().ToSingleInstance(settings.itemEffects.bubbleTrapItemEffect);
            Container.Bind<TimeSlowItemEffect>().ToSingleInstance(settings.itemEffects.timeSlowItemEffect);
        }

        private void InstallSettings()
        {
            Container.Bind<ItemManager.Settings>().ToSingleInstance(settings.itemManagerSettings);
        }

        [Serializable]
        public class Settings
        {
            public ItemEffectSettings itemEffects;
            public ItemManager.Settings itemManagerSettings;

            [Serializable]
            public class ItemEffectSettings
            {
                public AetherItemEffect aetherItemEffect;
                public QuicknessItemEffect quicknessItemEffect;
                public BubbleTrapItemEffect bubbleTrapItemEffect;
                public TimeSlowItemEffect timeSlowItemEffect;
            }
        }
    }
}
