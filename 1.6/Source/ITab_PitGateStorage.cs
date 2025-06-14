namespace RimWorld
{
    public class ITab_PitGateStorage : ITab_Storage
    {
        protected override bool IsPrioritySettingVisible
        {
            get 
            { 
                return false; 
            }
        }

        public ITab_PitGateStorage()
        {
            this.labelKey = "PitGateDumping_Dumping";
        }
    }
}
