namespace LootSystem {
    /// <summary>
    /// The item interface, implement this interface to create custom items.
    /// </summary>
    public interface IItem {
        public int Id { get; }
        public string Name { get; }
    }
}