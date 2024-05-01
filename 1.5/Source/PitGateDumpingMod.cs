using HarmonyLib;
using Verse;

namespace PitGateDumping
{
    public class PitGateDumpingMod : Mod
    {
        public const string PACKAGE_ID = "pitgatedumping.1trickPwnyta";
        public const string PACKAGE_NAME = "Pit Gate Dumping";

        public PitGateDumpingMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }
    }
}
