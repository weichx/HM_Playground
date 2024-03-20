using System;

namespace HostileMars.UI {

    public class TopLevelUIState {

        public ScreenId screenId;
        public GameScreenUIState gameScreenState;
        public Inventory inventory = new Inventory();
        public UIEventQueue eventQueue = new UIEventQueue();
        public MachineInfo machineInfo;
        public SItem[] itemDatabase;

        public DragItemEvent activeItemDragEvent;

        public bool IsDragOrigin(ItemCoordinate coordinate) {
            return activeItemDragEvent != null && activeItemDragEvent.itemSlot.coordinate == coordinate;
        }

        public Func<ItemSlot, ItemSlot, bool> isDroppable;

        public string GetDragAttribute(ItemSlot itemSlot) {
            if (activeItemDragEvent == null) {
                return "valid"; // always valid with no drag 
            }

            if (itemSlot.coordinate == activeItemDragEvent.itemSlot.coordinate) {
                return "invalid";
            }

            if (isDroppable != null) {
                return isDroppable(itemSlot, activeItemDragEvent.itemSlot)
                    ? "valid"
                    : "invalid";
            }

            return "valid";
        }

    }

}