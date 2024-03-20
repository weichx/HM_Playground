namespace HostileMars.UI {

    public class SetMachineViewState : UIEvent {

        public MachineInfo.State state;
        
        public SetMachineViewState(MachineInfo.State state) {
            this.state = state;
        }

    }

}