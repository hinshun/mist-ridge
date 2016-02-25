using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlaytestManager : IInitializable
    {
        private readonly SpawnManager spawnManager;
        private readonly Spawn.Factory spawnFactory;
        private readonly GameStateSignal.Trigger gameStateTrigger;

        public PlaytestManager(
                SpawnManager spawnManager,
                Spawn.Factory spawnFactory,
                GameStateSignal.Trigger gameStateTrigger)
        {
            this.spawnManager = spawnManager;
            this.spawnFactory = spawnFactory;
            this.gameStateTrigger = gameStateTrigger;
        }

        public void Initialize()
        {
            gameStateTrigger.Fire(GameStateType.Play);

            Spawn spawn = spawnFactory.Create();
            spawnManager.CurrentSpawn = spawn;
        }
    }
}
