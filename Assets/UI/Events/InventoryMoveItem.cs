namespace HostileMars.UI {

    public class InventoryMoveItem : UIEvent {

        public ItemCoordinate from;
        public ItemCoordinate to;
        public int count;
        
        public InventoryMoveItem(ItemCoordinate from, ItemCoordinate to, int count) {
            this.from = from;
            this.to = to;
            this.count = count;
        }

    }

}