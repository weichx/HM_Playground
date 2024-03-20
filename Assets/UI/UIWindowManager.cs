using UnityEngine;

namespace HostileMars.UI {

    public class UIWindowManager : MonoBehaviour {


        public void TransitionTo(ScreenId screenId) {
            // I don't know yet how we want to handle animating the screen transitions. 
            // if we need a slide in effect, we probably want to render the ui that we are
            // exiting to a temporary texture and apply some kind of wipe/fade/slide animation to that 
            // if we need to continue updating the screen that is exiting for visual purposes we can 
            // consider no rendering to an offscreen texture and instead just offsetting the x transform
            // of the screens during the animation, and remove the previous view when the animation completes
        }

    }

}