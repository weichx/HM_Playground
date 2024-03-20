namespace HostileMars.UI {

    public class InventoryPageNavigation : UIEvent {

        public InventoryPageNavigationType type;
        public int pageIndex;

        public InventoryPageNavigation(InventoryPageNavigationType type, int pageIndex = 0) {
            this.type = type;
            this.pageIndex = pageIndex;
        }

    }

}