using UnityEngine;

namespace LootSystem {
    public class DropChanceResolver : ILootResolver {
        private readonly float chance;  
        public DropChanceResolver( float chance ) => this.chance = chance;
        public bool Resolve() => Random.value <= chance;
    }
}