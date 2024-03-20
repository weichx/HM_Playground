namespace HostileMars.UI {
    
    public enum ItemClass : byte {

        InventoryItem = 0,

        //Deployable = 1,
        DataFile = 2,
        GameAgent = 3,
        Placeable = 4,
        PlaceableObject = 5,
        Battery = 11,


        //currently using for machine definitions-----------

        Turret = 20,
        Trap = 21,
        Machine = 22,
        Building = 23,


        CraftItem = 30,
        Metal = 31, // for blast furnace
        AssembledPart = 32, //for 3d printer
        FilamentSpool = 33, //for filament spooler

        ProcessableMaterial = 40, //for material processor

        SalvageableItem = 50, //for recycler machine

        BuildPanel = 60,

        //---------------------------------------------------


        Robot = 64,
        WeaponMod = 90,
        ShootableWeapon = 100,
        Wearable = 150,
        Resource = 200,
        None = 255

    }
    

}