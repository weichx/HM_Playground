using System;
using System.Collections.Generic;
using System.Linq;
using EvolveUI;
using HostileMars.UI.Mock;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HostileMars.UI {
    
    public class UISystem : UIRoot {

        private TopLevelUIState topLevelUIState;
        public UIWindowManager windowManager;
        private Queue<UIEvent> events;
        private UIInteractor interactor;

        // temp 
        private static readonly InventorySlotState[] s_MockStates = { InventorySlotState.Empty, InventorySlotState.Occupied };

        private bool initMocks;

        public static Color GetColorItemCategory(ItemCategory category) {
            return category switch {
                ItemCategory.Part => Color.red,
                ItemCategory.Turret => Color.green,
                ItemCategory.Trap => Color.yellow,
                ItemCategory.Machine => Color.magenta,
                ItemCategory.Building => Color.blue,
                ItemCategory.Consumable => Color.cyan,
                ItemCategory.Resource => Color.white,
                _ => Color.clear
            };
        }

        private ItemSlot MakeMockAmmoSlot(SItem[] itemDatabase) {
            ItemSlot slot = new ItemSlot();
            slot.state = s_MockStates[Random.Range(0, s_MockStates.Length)];

            if (slot.state == InventorySlotState.Empty) {
                slot.item = null;
                slot.stackSize = 0;
            }
            else {
                slot.item = itemDatabase[Random.Range(0, itemDatabase.Length)];
                slot.stackSize = Random.Range(1, 1000);
                slot.cornerColor = GetColorItemCategory(ItemCategory.Ammo);
            }

            return slot;
        }

        private static ImageSource[] s_ItemIcons = {
            new ImageSource("EnergyCellAmmo_Icon"),
            new ImageSource("AI Core Icon"),
            new ImageSource("IronOreIcon"),
            new ImageSource("Magnesium Ingot"),
            new ImageSource("TechDrive0")
        };

        private InventoryPage MakeMockInventoryPage(int pageId, SItem[] itemDatabase) {
            InventoryPage page = new InventoryPage();

            for (int i = 0; i < page.slots.Length; i++) {
                ref ItemSlot slot = ref page.slots[i];

                slot.state = s_MockStates[Random.Range(0, s_MockStates.Length)];
                slot.coordinate = new ItemCoordinate(0, (ushort)pageId, (ushort)i);

                if (slot.state == InventorySlotState.Empty) {
                    slot.item = null;
                    slot.item = null;
                    slot.stackSize = -1;
                    slot.cornerColor = new Color32(0, 0, 0, 0);
                }
                else {
                    slot.item = itemDatabase[Random.Range(0, itemDatabase.Length)];
                    slot.stackSize = Random.Range(1, 100);
                    slot.cornerColor = GetColorItemCategory(slot.item.category);
                }
            }

            return page;
        }

        public void Awake() {
            topLevelUIState = new TopLevelUIState();
            data = topLevelUIState;
        }

        // this is where we look at the top level UI state (ie which windows are open, what screen we are on)
        // and figure out how to fill out a data structure that the UI will work with to render data. It's
        // important for testing & authoring purposes that our TopLevelUIState doesn't contain pointers directly
        // to game world data or anything that might be a cyclic reference. Ideally we are able to serialize our 
        // entire UI state to JSON to have multiple testing scenarios that we can evaluate without loading the game.
        public void SynthesizeUIState() {
            if (!initMocks) {

                topLevelUIState.itemDatabase = MockDeserializer.DeserializeItemDatabase(Resources.Load<TextAsset>("SITEMS").text, application.GetAssetManager());

                initMocks = true;

                int pageCount = Random.Range(1, 5);

                topLevelUIState.inventory.pages = new InventoryPage[pageCount];
                for (int i = 0; i < pageCount; i++) {
                    topLevelUIState.inventory.pages[i] = MakeMockInventoryPage(i, topLevelUIState.itemDatabase);
                }

                topLevelUIState.inventory.ammoSlots = new ItemSlot[] {
                    MakeMockAmmoSlot(topLevelUIState.itemDatabase),
                    MakeMockAmmoSlot(topLevelUIState.itemDatabase),
                    MakeMockAmmoSlot(topLevelUIState.itemDatabase),
                    MakeMockAmmoSlot(topLevelUIState.itemDatabase),
                    MakeMockAmmoSlot(topLevelUIState.itemDatabase)
                };

                SItem[] craftables = topLevelUIState.itemDatabase.Where(a => a.craftable).ToArray();

                MachineInfo machineInfo = new MachineInfo();
                machineInfo.craftables = new Craftable[Random.Range(3, Math.Min(25, craftables.Length))];
                
                static void Shuffle<T>(T[] array)
                {
                    System.Random random = new System.Random();

                    for (int i = array.Length - 1; i > 0; i--)
                    {
                        // Generate a random index j such that 0 <= j <= i
                        int j = random.Next(i + 1);

                        // Swap array[i] with array[j]
                        T temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }

                Shuffle(craftables);
                 
                for (int i = 0; i < machineInfo.craftables.Length; i++) {
                    SItem item = craftables[i];
                    bool locked = Random.Range(0, 10) >= 8;
                    machineInfo.craftables[i] = new Craftable() {
                        icon = item.icon,
                        item = item,
                        name = item.name,
                        isLocked = locked,
                        canCraft = !locked && Random.Range(0, 10) >= 5
                    };
                }

                machineInfo.stats.remainingMinutes = Random.Range(0, 10);
                machineInfo.stats.remainingSeconds = Random.Range(0, 60);
                machineInfo.stats.assignedBots = Random.Range(0, 4);
                machineInfo.timePerItem = (float)Random.Range(1, 5);
                machineInfo.maxItemCount = Random.Range(0, 100);
                
                topLevelUIState.machineInfo = machineInfo;

            }

            // todo -- move to another system, need to read this in from somewhere
            topLevelUIState.machineInfo.Update();
            
            switch (topLevelUIState.screenId) {
                case ScreenId.Splash:
                    break;

                case ScreenId.Loading:
                    break;

                case ScreenId.MainMenu:
                    break;

                case ScreenId.Game:

                    if (topLevelUIState.gameScreenState.inventoryOpenState.IsOpen) {
                        // fill in inventory data
                        // I don't know what is needed here yet. 
                        // I think we can represent items in the inventory as an item id + an amount + inventory slot index 
                        // I don't know how stacks of items work yet, but that should be trivial to account for
                    }

                    // interactable things seem to have a few behaviors:
                    // 1. while not interacting -> show the interaction/pickup menu on the right of the character
                    // 2. while not interacting -> show a floating tooltip with a summary 
                    // 3. show just the window -> hide tooltip & summary views 
                    // 1 & 2 can optionally both be present

                    // for ui animations we'll want to use a Transitionable to manager their opening / closing animations
                    // when opening the window, we shortcut the animations and remove the tooltip + summary
                    switch (interactor.state) {
                        case InteractorState.None:
                            break;

                        case InteractorState.BeginHovering: {
                            if (interactor.interactable.HasSummaryView) { }

                            // if (interactor.interactable.InteractionMenu()) {
                            //     
                            // }

                            break;
                        }

                        case InteractorState.Hovering: {
                            break;
                        }

                        case InteractorState.Beginning: {
                            // get the thing we are interacting with 
                            // figure out it's type and what data we need for the window
                            // maybe toggle the inventory screen to open 
                            // maybe close conflicting windows that might be open 
                            // each thing we can interact with should have a way to get us the data we need to display it's window 
                            break;
                        }

                        case InteractorState.Interacting:
                            break;

                        case InteractorState.Closing:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;

                case ScreenId.Closing:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        protected override void Update() {
            if (application == null) {
                return;
            }
            
            SynthesizeUIState();

            if (Input.GetKey(KeyCode.Escape)) {
                Application.Quit();
            }

            while (topLevelUIState.eventQueue.TryDequeue(out UIEvent result)) {
                switch (result) {
                    
                    case SelectCraftable e: {

                        topLevelUIState.machineInfo.state = MachineInfo.State.CraftingSetup;
                        topLevelUIState.machineInfo.selectedRecipe = e.craftable;
                        topLevelUIState.machineInfo.timePerItem = e.craftable.item.craftableItemDefine.timeToCraft;
                        topLevelUIState.machineInfo.enqueuedItemCount = 0;
                        topLevelUIState.machineInfo.itemElapsedTime = 0;
                        
                        break;
                    }
                    case InventoryMoveItem e: {
                        Inventory inventory = topLevelUIState.inventory;

                        if (e.from == e.to) {
                            break;
                        }

                        ItemSlot fromItem = inventory.GetItemSlotById(e.from);
                        ItemSlot toItem = inventory.GetItemSlotById(e.to);

                        if (toItem.state == InventorySlotState.Empty) {
                            toItem = fromItem;
                            toItem.coordinate = e.to;
                            fromItem = default;
                            fromItem.coordinate = e.from;
                        }
                        else if (fromItem.item != toItem.item) {
                            break;
                        }
                        else if (e.count < 0 || e.count == fromItem.stackSize) {
                            toItem.stackSize += fromItem.stackSize;
                            fromItem = new ItemSlot() {
                                coordinate = e.from
                            };
                        }
                        else {
                            toItem.stackSize += e.count;
                            fromItem.stackSize -= e.count;
                        }

                        inventory.pages[e.from.pageIndex].slots[e.from.slotId] = fromItem;
                        inventory.pages[e.to.pageIndex].slots[e.to.slotId] = toItem;

                        break;
                    }

                    case TakeAllMachineOutput e: {
                        topLevelUIState.machineInfo.output.amount = 0;
                        // todo -- put this in the inventory somehow 
                        break;
                    }
                    case SetMachineViewState e: {
                        if (e.state == MachineInfo.State.RecipeSelection) {
                            // todo -- don't transition if we have output, other rules
                            if (topLevelUIState.machineInfo.output.amount > 0 || topLevelUIState.machineInfo.state == MachineInfo.State.Crafting) {
                                break;
                            }
                            topLevelUIState.machineInfo.enqueuedItemCount = 0;
                        }
                        else if (e.state == MachineInfo.State.Crafting) {
                            if (topLevelUIState.machineInfo.enqueuedItemCount == 0) {
                                break;
                            }

                        }
                        topLevelUIState.machineInfo.state = e.state;
                        break;
                    }

                    case SetMachineEnqueuedItemCount e: {
                        topLevelUIState.machineInfo.enqueuedItemCount = e.itemCount;
                        break;
                    }
                    
                    case InventoryPageNavigation e: {
                        switch (e.type) {
                            case InventoryPageNavigationType.NextPage:
                                topLevelUIState.inventory.NextPage();
                                break;

                            case InventoryPageNavigationType.PrevPage: {
                                topLevelUIState.inventory.PrevPage();
                                break;
                            }

                            case InventoryPageNavigationType.SetPage: {
                                topLevelUIState.inventory.SetPage(e.pageIndex);
                                break;
                            }
                        }

                        break;
                    }

                    case ScreenNavigationEvent e: {
                        windowManager.TransitionTo(e.targetScreen);
                        break;
                    }

                    case DragItemToWorld e: {
                        if (interactor.TryPlaceItemAtMouse(e.itemStack)) { }

                        break;
                    }
                }

                // re-pool the event probably
            }

            base.Update();
        }

    }

}