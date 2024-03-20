using EvolveUI;

namespace HostileMars.UI {

    public struct SalvagedItemDefine {

        public SItem item;
        public int minCount;
        public int maxCount;
        public float chance;

    }

    public struct SalvageInfo {

        public float salvageTime;
        public SalvagedItemDefine[] items;

    }

    public struct RequiredMachine {

        public byte requiredEquipmentRank;
        public SItem requiredEquipment;

    }

    public struct CraftableSItemIngredient {

        public SItem item;
        public int quantity;

    }

    public struct CraftableItemDefine {

        public bool excludeFromCraftBench;
        public float timeToCraft;
        public int quantity;
        public CraftableSItemIngredient[] ingredients;
        public RequiredMachine[] requiredMachines;

    }

    public class SItem {

        public ItemCategory category;
        public int id;
        public string name;
        public string description;
        public int stackSize;
        public bool craftable;
        public bool deconstructable;
        public ImageSource icon;
        public CraftableItemDefine craftableItemDefine;
        public SalvageInfo salvageInfo;

    }

}