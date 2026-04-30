using PitGateDumping;
using Verse;

namespace RimWorld
{
    public class ITab_PitGateStorage : ITab_Storage
    {
        public override bool IsVisible => base.IsVisible || SelThing.TryGetComp<CompPitGateStorage>()?.StorageTabVisible == true;

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
