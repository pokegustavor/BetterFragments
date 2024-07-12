using HarmonyLib;
using PulsarModLoader;

namespace BetterFragments
{
    public class Mod : PulsarMod
    {
        public static bool MasterHasMod = false;
        public override string Version => "1.2";

        public override string Author => "pokegustavo";

        public override string ShortDescription => "Improves some fragments";

        public override string Name => "BetterFragments";

        public override string HarmonyIdentifier()
        {
            return "pokegustavo.betterfragments";
        }
    }

    [HarmonyPatch(typeof(PLGlobal), "EnterNewGame")]
    class OnJoin
    {
        static void Postfix()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Mod.MasterHasMod = false;
            }
            else
            {
                Mod.MasterHasMod = true;
            }
        }
    }

    class ReciveConfirmation : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (PhotonNetwork.masterClient == sender.sender)
            {
                Mod.MasterHasMod = (bool)arguments[0];
            }
        }
    }
}
