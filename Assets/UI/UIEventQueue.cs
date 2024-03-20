using System.Collections.Generic;

namespace HostileMars.UI {

    public class UIEventQueue {

        private Queue<UIEvent> queue = new Queue<UIEvent>();

        public virtual void NextInventoryPage() {
            queue.Enqueue(new InventoryPageNavigation(InventoryPageNavigationType.NextPage));
        }
        
        public virtual void PrevInventoryPage() {
            queue.Enqueue(new InventoryPageNavigation(InventoryPageNavigationType.PrevPage));
        }

        public virtual void SetInventoryPage(int pageIndex) {
            queue.Enqueue(new InventoryPageNavigation(InventoryPageNavigationType.SetPage, pageIndex));
        }

        public virtual void MoveInventoryItem(ItemCoordinate from, ItemCoordinate to, int count = -1) {
            queue.Enqueue(new InventoryMoveItem(from, to, count));
        }

        public virtual void SelectCraftable(Craftable craftable) {
            queue.Enqueue(new SelectCraftable(craftable));    
        }

        public virtual void SetMachineViewState(MachineInfo.State state) {
            queue.Enqueue(new SetMachineViewState(state));
        }

        public virtual void SetMachineEnqueuedItemCount(int itemCount) {
            queue.Enqueue(new SetMachineEnqueuedItemCount(itemCount));
        }
        
        public virtual void TakeAllMachineOutput() {
            queue.Enqueue(new TakeAllMachineOutput());
        }
        
        public bool TryDequeue(out UIEvent evt) {
            return queue.TryDequeue(out evt);
        }
        
    }

}