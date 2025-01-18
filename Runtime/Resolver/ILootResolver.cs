namespace LootSystem {
    /// <summary>
    /// The loot resolver interface, implement this interface to create custom loot resolvers such as drop chance resolver.
    /// </summary>
    public interface ILootResolver {
        public bool Resolve();
    }
}