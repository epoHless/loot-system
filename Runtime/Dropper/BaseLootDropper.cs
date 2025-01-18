using System;
using System.Collections.Generic;

namespace LootSystem {
    /// <summary>
    /// Implement a base loot dropper that can be extended to create custom droppers
    /// </summary>
    /// <typeparam name="TResolver"></typeparam>
    public abstract class BaseLootDropper<TResolver> where TResolver : ILootResolver {
        protected readonly LootBag<IItem> lootBag;
        private TResolver resolver;

        protected BaseLootDropper( TResolver resolver ) {
            this.resolver = resolver;
            lootBag = new LootBag<IItem>( new List<IItem>() );
        }

        public void Drop() {
            if ( resolver.Resolve() ) {
                OnLootDropped();
            }
            else {
                HandleBadResolve();
            }
        }
        
        protected abstract void OnLootDropped();
        
        protected virtual void HandleBadResolve() {
            // Handle bad resolve
        }

        public void AddItem( IItem item ) => lootBag.Add( item );
        
        #region Builder

        public class Builder {
            readonly List<IItem> items = new List<IItem>();
            ILootResolver resolver;
            
            public Builder Reset() {
                items.Clear();
                resolver = null;
                return this;
            }
            
            public Builder WithResolver( ILootResolver resolver ) {
                this.resolver = resolver;
                return this;
            }
            
            public Builder WithItems( params IItem[] items ) {
                this.items.AddRange( items );
                return this;
            }
            
            public T Build<T, TResolver>() where T : BaseLootDropper<TResolver> where TResolver : ILootResolver {

                if ( resolver == null ) {
                    throw new Exception( "Resolver is not set" );
                }

                if ( resolver is not TResolver compatibleResolver ) {
                    throw new Exception( "Resolver is not compatible" );
                }
                
                var dropper = (T) Activator.CreateInstance( typeof( T ), compatibleResolver );
                if ( dropper == null ) {
                    throw new Exception( "Failed to create dropper" );
                }
                
                foreach ( var item in items ) {
                    dropper.AddItem( item );
                }
                
                return dropper;
            }
        }

        #endregion
    }
}