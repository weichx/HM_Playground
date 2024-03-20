namespace HostileMars.UI {

    public class DragItemToWorld : UIEvent {

        public ItemStack itemStack;

        public DragItemToWorld(ItemStack itemStack) {
            this.itemStack = itemStack;
        }

    }

}