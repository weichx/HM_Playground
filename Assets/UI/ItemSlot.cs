using UnityEngine;

namespace HostileMars.UI {

    public struct ItemSlot {

        public InventorySlotState state; // kind of separate from the rest I think, probably ok to remove it
        
        public ItemCoordinate coordinate;
        public SItem item;
        public int stackSize;
        public Color32 cornerColor;

    }

}