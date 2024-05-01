using HarmonyLib;
using RimWorld;
using Verse;

namespace PitGateDumping
{
    [HarmonyPatch(typeof(ITab_Storage))]
    [HarmonyPatch("get_IsVisible")]
    public static class Patch_ITab_Storage_get_SelStoreSettingsParent
    {
        public static void Postfix(ref bool __result)
        {
            if (!__result)
            {
                object selObject = Find.Selector.SingleSelectedObject;
                if (selObject != null && selObject is PitGate)
                {
                    __result = true;
                }
            }
        }
    }
}
