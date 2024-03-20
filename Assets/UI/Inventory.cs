namespace HostileMars.UI {

    public class InventoryPage {

        public ItemSlot[] slots = new ItemSlot[25];

    }
    
    public class Inventory : ItemContainer {

        public InventoryPage[] pages;
        public int selectedPageIndex;
        public ItemSlot[] ammoSlots;

        public Inventory() {
                
        }
        
        public void NextPage() {
            selectedPageIndex++;
            if (selectedPageIndex >= pages.Length) {
                selectedPageIndex = 0;
            }
        }
        
        public void PrevPage() {
            selectedPageIndex--;
            if (selectedPageIndex < 0 ) {
                selectedPageIndex = pages.Length - 1;
            }
        }

        public void SetPage(int pageIndex) {
            if (pageIndex < 0) pageIndex = 0;
            if (pageIndex >= pages.Length) pageIndex = pages.Length - 1;
            this.selectedPageIndex = pageIndex;
        }
        
        public InventoryPage GetSelectedPage() {
            return pages[GetSelectedPageIndex()];
        }
        
        public int GetSelectedPageIndex() {
            if (selectedPageIndex < 0) selectedPageIndex = 0;
            if (selectedPageIndex >= pages.Length) selectedPageIndex = pages.Length - 1;
            return selectedPageIndex;
        }
        
        public ItemSlot GetItemSlotById(ItemCoordinate slotId) {
            InventoryPage page = pages[slotId.pageIndex];
            return page.slots[slotId.slotId];
        }

    }

}