using Unity.Mathematics;
using UnityEngine;

namespace HostileMars.UI {

    public class MachineInfo {

        public Craftable[] craftables;

        public ItemStack output;
        public ItemStack[] inputs;

        public State state = State.RecipeSelection;
        
        public Craftable selectedRecipe;
        
        public enum State {
            
            Invalid,
            RecipeSelection,
            CraftingSetup,
            Crafting
            
        }

        public Stats stats;
        public int enqueuedItemCount;
        public int maxItemCount;
        public float itemProgress => math.saturate(itemElapsedTime / math.max(1, timePerItem));
        public float itemElapsedTime;
        public float timePerItem;

        public bool IsCrafting => state == State.Crafting;
        
        public struct Stats {

            public int remainingMinutes;
            public int remainingSeconds;
            public int assignedBots;

        }

        public void Update() {
            
            if (state == State.Crafting) {
                
                itemElapsedTime += Time.deltaTime;

                if (itemElapsedTime < timePerItem) {
                    return;
                }
                
                itemElapsedTime = 0;
                enqueuedItemCount--;
                output.amount++;
                    
                if (enqueuedItemCount <= 0) {
                    state = State.CraftingSetup;
                }

            }
            
        }

    }

}