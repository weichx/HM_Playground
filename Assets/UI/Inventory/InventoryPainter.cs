using EvolveUI;
using Unity.Mathematics;
using UnityEngine;

namespace HostileMars.UI {

    public class InventoryPainter : Painter {

        private GlyphIcon emptyIcon;
        private GlyphIcon filledIcon;
        public Color color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        public InventoryPage page;
        
        public override PaintDetach Attach(ElementId elementId, PaintAttachment attachment) {
            filledIcon = Assets.GetGlyphIcon("T_UI_Inventory_Previewer_Filled");
            emptyIcon = Assets.GetGlyphIcon("T_UI_Inventory_Previewer_Empty");
            return base.Attach(elementId, attachment);
        }

        protected override void Paint(Gfx gfx, StyleResult styleResult) {

            Size size = new Size(8f, 8f);

            Color c = color;
            float opacity = styleResult.Opacity;
            
            c.r *= opacity;
            c.g *= opacity;
            c.b *= opacity;
            c.a *= opacity;

            int x = 0;
            int y = 7;
            
            for (int i = 0; i < 5; i++) {
                
                for (int j = 0; j < 5; j++) {
                    int slotIndex = i * 5 + j;
                    InventorySlotState state =  page.slots[slotIndex].state;
                    GlyphIcon icon = filledIcon;
                    if (state == InventorySlotState.Locked || state == InventorySlotState.Empty) {
                        icon = emptyIcon;
                    }
                    gfx.DrawFontIcon(icon, new float2(x, y), size, c, false);
                    x += 10;
                }

                x = 0;
                y += 10;
                
            }

         
            Next();
            
        }

    }

}