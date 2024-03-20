using EvolveUI;
using UnityEngine;

namespace HostileMars.UI {

    public enum ScreenId {

        Splash,
        Loading,
        MainMenu,
        Game,
        Closing
    }

    public interface IInteractable {

        bool HasInteractionMenu { get; }
        
        bool HasSummaryView { get; }
        
        bool HasWindow { get; }

    }

    public class ScreenNavigationEvent : UIEvent {

        public ScreenId targetScreen;

        public ScreenNavigationEvent(ScreenId targetScreen) {
            this.targetScreen = targetScreen;
        }

    }

    public enum ItemId {

        Spring,
        Ingot, 

    }

    public struct ItemData {

        public string descriptionKey;
        public string textureId;
        public bool recycable;

    }

    public enum InteractorState {

        // there are probably more states, we likely want some kind of system for animated enter/exit into states
        None,
        Beginning,
        Interacting,
        Closing,

        Hovering,

        BeginHovering

    }
    
    public class UIInteractor {

        public InteractorState state;
        public IInteractable interactable;
        
        public bool TryPlaceItemAtMouse(ItemStack stack) {
            // do raycast checks, etc
            return true;
        }

    }

    public struct TransitionableState {

        public float progress;
        public Bezier easingCurve;

        public bool IsOpen => progress > 0;
        public bool IsClosed => progress == 0;
        public bool IsOpening => progress > 0 && progress != 1f;
        public bool IsClosing => progress != 0 && progress < 1f;
        
    }
    
    public class GameScreenUIState {

        public TransitionableState inventoryOpenState;

    }

}