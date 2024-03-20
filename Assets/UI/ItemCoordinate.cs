using System;

namespace HostileMars.UI {

    public struct ItemCoordinate {

        public int containerId;
        public ushort pageIndex;
        public ushort slotId;

        public ItemCoordinate(int containerId, ushort pageIndex, ushort slotId) {
            this.containerId = containerId;
            this.pageIndex = pageIndex;
            this.slotId = slotId;
        }

        public bool Equals(ItemCoordinate other) {
            return containerId == other.containerId && pageIndex == other.pageIndex && slotId == other.slotId;
        }

        public override bool Equals(object obj) {
            return obj is ItemCoordinate other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(containerId, pageIndex, slotId);
        }
     
        public static bool operator ==(ItemCoordinate a, ItemCoordinate b) {
            return a.containerId == b.containerId && a.slotId == b.slotId && a.pageIndex == b.pageIndex;
        }
        
        public static bool operator !=(ItemCoordinate a, ItemCoordinate b) {
            return a.containerId != b.containerId || a.slotId != b.slotId || a.pageIndex != b.pageIndex;
        }

    }

}