using EvolveUI.UIInput;

namespace HostileMars.UI {

    
    public class DragItemEvent : DragEvent {

        public ItemSlot itemSlot;
        
        public DragItemEvent(ItemSlot item) {
            this.itemSlot = itemSlot;
        }


    }

}