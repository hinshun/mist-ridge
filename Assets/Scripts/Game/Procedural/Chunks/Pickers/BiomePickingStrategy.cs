using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class BiomePickingStrategy
    {
        private readonly Generator generator;

        public BiomePickingStrategy(Generator generator)
        {
            this.generator = generator;
        }

        public IBiome Pick(List<IBiome> biomes, float altitude)
        {
            biomes = biomes
                .Where(biome => altitude >= biome.StartAltitude() && altitude < biome.EndAltitude())
                .ToList();

            int randomIndex = generator.Random.Next(biomes.Count);
            return biomes[randomIndex];
        }
    }
}
