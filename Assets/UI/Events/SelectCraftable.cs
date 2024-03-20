namespace HostileMars.UI {

    public class SelectCraftable : UIEvent {

        public Craftable craftable;

        public SelectCraftable(Craftable craftable) {
            this.craftable = craftable;
        }

    }

}