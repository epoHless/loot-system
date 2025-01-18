# Loot System

A robust loot system that allows for the generation of loot based on a set of rules and conditions. The system is designed to be flexible and extensible, allowing for the easy addition of new loot types and conditions.

---

### Features

- Generate loot based on a set of rules and conditions
- Flexible and extensible design
- Easy to add new loot types and conditions

---

### Core Classes

#### `LootBag` & `IItem`

The loot system comes with a base interface `IItem` that can be implemented to create new item types. To create a new item type, create a new class that implements `IItem`.  
The `LootBag` class is used to store and manage the items that are dropped by the loot system.

---

#### `ILootResolver`

The loot system comes with a base interface `ILootResolver` that can be implemented to create new loot resolvers. To create a new loot resolver, create a new class that implements `ILootResolver`. 
A custom `DropChanceResolver` comes with the loot system that can be used to resolve the drop chance of loot based on a set of rules and conditions. 

```csharp   
public class DropChanceResolver : ILootResolver {
        private readonly float chance;  
        public DropChanceResolver( float chance ) => this.chance = chance;
        public bool Resolve() => Random.value <= chance;
}
``` 

---

#### `BaseLootDropper`

The `BaseLootDropper` class is a base class that can be extended to create new loot droppers. To create a new loot dropper, create a new class that extends `BaseLootDropper` and implement the `OnLootDropped` method.
The method `HandleBadResolve` can be overridden to handle cases where the loot resolver returns false.

```csharp
public class EnemyLootDropper : BaseLootDropper<DropChanceResolver> {
        public EnemyLootDropper( DropChanceResolver resolver ) : base( resolver ) { }
        
        protected override void OnLootDropped() {
            foreach ( var item in lootBag.GetAllItems() ) {
                Debug.Log( $"Dropped {item.Name}" );
            }
        }
    }
```

---

### Usage

It is recommended to implement a factory pattern to create new loot droppers. This allows for easy creation without modifying existing code.

```csharp
public class LootDropperFactory {
    protected readonly BaseLootDropper< ILootResolver >.Builder builder = new();
    
    public EnemyLootDropper CreateEnemyLootDropper( IItem[] items, float dropChance = 0.5f ) {
        return builder.Reset()
            .WithResolver( new DropChanceResolver( dropChance ) )
            .WithItems( items )
            .Build<EnemyLootDropper, DropChanceResolver>();
    }
}
```

_**The method `Reset` must be always called before creating a new loot dropper to reset the builder state and avoid conflicts.**_

---

Here is an example of how to use the loot dropper by creating a simple loot system for an enemy.

```csharp
public class Item : IItem {
    public int Id { get; }
    public string Name { get; }
    public Item( string name ) => Name = name;
}
```

```csharp
public class LootSystemTest : MonoBehaviour {
        LootDropperFactory factory = new LootDropperFactory(); // Ideally this reference should be injected or passed trough a service locator

        private void Start() {
            var items = new IItem[] {
                new Item( "Sword" ),
                new Item( "Shield" ),
                new Item( "Potion" )
            };

            var dropper = factory.CreateEnemyLootDropper( items, 1f );
            dropper.Drop();
        }
    }
```