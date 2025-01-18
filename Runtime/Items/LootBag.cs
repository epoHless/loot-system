using System.Collections.Generic;

namespace LootSystem {
    
    /// <summary>
    /// The loot bag class that holds the items dropped by the dropper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LootBag< T > where T : IItem {
        private readonly List<T> items;
        
        public LootBag( List<T> items ) => this.items = items;
        
        public void Add( T item ) => items.Add( item );
        public void AddRange( IEnumerable<T> items ) => this.items.AddRange( items );
        
        public void Remove( T item ) => items.Remove( item );
        
        public IEnumerable<T> GetAllItems() => items;
    }
    
}