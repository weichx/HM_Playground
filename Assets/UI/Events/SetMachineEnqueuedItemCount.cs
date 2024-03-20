namespace HostileMars.UI {

    public class SetMachineEnqueuedItemCount : UIEvent {

        public int itemCount;

        public SetMachineEnqueuedItemCount(int itemCount) {
            this.itemCount = itemCount;
        }

    }

}