using System;

namespace HostileMars.UI {

    [Serializable]
    public enum ItemCategory : byte {

        Consumable = 0,
        BindsToAccount = 1,
        UpgradeWhenHolding = 2,
        Weapon = 3,
        BuildPanelAccessory = 4,
        Deployable = 5,
        Ammo = 6,
        DeployableItem = 7,
        Power = 8,
        Resource = 9,


        Building = 10,
        Machine = 11,
        Turret = 12,
        Trap = 13,
        Storage = 14,

        CellDrive = 50,

        Junk = 100,

        AllyBot = 200,

        Part = 210,
        Other = 250,
        None = 255

    }

}