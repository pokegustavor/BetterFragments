using PulsarModLoader;

namespace BetterFragments
{
    public class Mod : PulsarMod
    {
        public override string Version => "1.0.0";

        public override string Author => "pokegustavo";

        public override string ShortDescription => "Improves some fragments";

        public override string Name => "BetterFragments";

        public override string HarmonyIdentifier()
        {
            return "pokegustavo.betterfragments";
        }
    }
}
