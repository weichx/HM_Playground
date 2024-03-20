using System;
using EvolveUI;
using UnityEngine;

namespace HostileMars.UI.Mock {

    [Serializable]
    public struct JsonRoot {

        public SItemDeserialized[] items;

    }

    [Serializable]
    public class SItemDeserialized {

        public ItemCategory category;
        public int id;
        public int instanceId;
        public string name;
        public string description;
        public int stackSize;
        public string textureId;
        public bool Craftable;
        public bool Deconstructable;
        public ItemDefineDeserialize CraftableSItem;
        public SalvageInfoDeserialize SalvageInfo;

    }

    [Serializable]
    public struct SalvagedItemDefineDeserialize {

        public SItemReference Item;
        public int MinCount;
        public int MaxCount;
        public float Chance;

    }

    [Serializable]
    public struct SalvageInfoDeserialize {

        public float SalvageTime;
        public SalvagedItemDefineDeserialize[] Items;

    }

    [Serializable]
    public struct RequiredMachineDeserialize {

        public byte _requiredEquipmentRank;
        public SItemReference _requiredEquipment;

    }
    
    [Serializable]
    public struct CraftableSItemIngredientDeserialize {

        public SItemReference m_SItem;
        public int m_Quantity;

    }

    [Serializable]
    public struct SItemReference {

        public int instanceID;

    }

    [Serializable]
    public struct ItemDefineDeserialize {

        public bool _excludeFromCraftBench;
        public float _timeToCraft;
        public CraftableSItemIngredientDeserialize[] _ingredients;
        public int _quantity;
        public RequiredMachineDeserialize[] _requiredMachines;

    }

    [Serializable]
    public struct CraftableItemDefineDeserialize {

        public bool _excludeFromCraftBench;
        public float _timeToCraft;
        public CraftableSItemIngredientDeserialize[] _ingredients;
        public int _quantity;
        public RequiredMachineDeserialize[] _requiredMachines;

    }

    public static class MockDeserializer {

        public static SItem[] DeserializeItemDatabase(string json, UIAssetManager assetManager) {
            JsonRoot root = JsonUtility.FromJson<JsonRoot>(json);

            SItemDeserialized[] rawItems = root.items;
            SItem[] itemDb = new SItem[root.items.Length];

            for (int i = 0; i < rawItems.Length; i++) {
                SItemDeserialized deserialized = rawItems[i];
                itemDb[i] = new SItem() {
                    id = deserialized.id,
                    description = deserialized.description,
                    stackSize = deserialized.stackSize,
                    name = deserialized.name,
                    category = deserialized.category,
                    icon = assetManager.GetTextureId(deserialized.textureId),
                    craftable = deserialized.Craftable,
                    deconstructable = deserialized.Deconstructable,
                    craftableItemDefine = new CraftableItemDefine() {
                        ingredients = new CraftableSItemIngredient[deserialized.CraftableSItem._ingredients.Length],
                        requiredMachines = new RequiredMachine[deserialized.CraftableSItem._requiredMachines.Length],
                        quantity = deserialized.CraftableSItem._quantity,
                        timeToCraft = deserialized.CraftableSItem._timeToCraft,
                        excludeFromCraftBench = deserialized.CraftableSItem._excludeFromCraftBench,
                    },
                    salvageInfo = new SalvageInfo() {
                        items = new SalvagedItemDefine[deserialized.SalvageInfo.Items.Length],
                        salvageTime = deserialized.SalvageInfo.SalvageTime
                    }
                };
            }

            for (int i = 0; i < rawItems.Length; i++) {
                SItemDeserialized deserialized = rawItems[i];
                SItem item = itemDb[i];

                CraftableSItemIngredient[] ingredients = item.craftableItemDefine.ingredients;
                CraftableSItemIngredientDeserialize[] sources = deserialized.CraftableSItem._ingredients;

                for (int j = 0; j < ingredients.Length; j++) {
                    ref CraftableSItemIngredientDeserialize src = ref sources[j];
                    ingredients[j] = new CraftableSItemIngredient() {
                        item = FindByInstanceId(src.m_SItem.instanceID),
                        quantity = src.m_Quantity
                    };
                }

                RequiredMachineDeserialize[] srcMachines = deserialized.CraftableSItem._requiredMachines;
                RequiredMachine[] dstMachines = item.craftableItemDefine.requiredMachines;

                for (int j = 0; j < srcMachines.Length; j++) {
                    ref RequiredMachineDeserialize src = ref srcMachines[j];
                    ref RequiredMachine dst = ref dstMachines[j];
                    dst.requiredEquipment = FindByInstanceId(src._requiredEquipment.instanceID);
                    dst.requiredEquipmentRank = src._requiredEquipmentRank;
                }

                SalvagedItemDefineDeserialize[] srcItems = deserialized.SalvageInfo.Items;
                SalvagedItemDefine[] dstItems = item.salvageInfo.items;

                for (int j = 0; j < srcItems.Length; j++) {
                    ref SalvagedItemDefineDeserialize src = ref srcItems[j];
                    ref SalvagedItemDefine dst = ref dstItems[j];
                    dst.chance = src.Chance;
                    dst.item = FindByInstanceId(src.Item.instanceID);
                    dst.maxCount = src.MaxCount;
                    dst.minCount = src.MinCount;
                }
            }

            return itemDb;

            SItem FindByInstanceId(int instanceId) {
                if (instanceId == 0) {
                    return null;
                }

                for (int i = 0; i < rawItems.Length; i++) {
                    if (rawItems[i].instanceId == instanceId) {
                        return itemDb[i];
                    }
                }

                return null;
            }
        }


    }

}