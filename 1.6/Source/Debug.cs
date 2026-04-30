namespace PitGateDumping
{
    public static class Debug
    {
        public static void Log(object message)
        {
#if DEBUG
            Verse.Log.Message($"[{PitGateDumpingMod.PACKAGE_NAME}] {message}");
#endif
        }
    }
}
