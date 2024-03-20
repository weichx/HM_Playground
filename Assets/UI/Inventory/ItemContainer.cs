namespace HostileMars.UI {

    public class ItemContainer {

        public DragItemEvent CreateDrag(ItemSlot item) {
            
            if (item.state == InventorySlotState.Empty || item.state == InventorySlotState.Locked) {
                return null;
            }

            return  new DragItemEvent(item);
            
        }

    }

}