using HarmonyLib;

namespace BetterFragments
{
    internal class Patch
    {
        [HarmonyPatch(typeof(PLTabMenu), "GetFragmentInfo")]
        class Descriptions 
        {
            static void Postfix(int fragID, out string fragName, out string fragDesc) 
            {
				switch (fragID)
				{
					case 0:
						fragName = "A.O.G. Fragment";
						fragDesc = "Your ship will become unflagged each jump\n+15% Credits from destroyed ships";
						return;
					case 1:
						fragName = "Auction Fragment";
						fragDesc = "Gain access to special deals in general stores across the galaxy";
						return;
					case 2:
						fragName = "Colonial Union Fragment";
						fragDesc = "+50% max shield integrity\n+15% warp range";
						return;
					case 3:
						fragName = "High Roller Fragment";
						fragDesc = "Random chance to either gain credits, processed scrap or coolant each jump";
						return;
					default:
						fragName = "Fragment";
						fragDesc = "";
						return;
					case 6:
						fragName = "Fluffy Biscuit Fragment";
						fragDesc = "2x fluffy oven food supply\n2x credits earned for biscuit sales";
						return;
					case 7:
						fragName = "Grey Huntsmen Fragment";
						fragDesc = "No effect";
						return;
					case 8:
						fragName = "Warger's Fragment";
						fragDesc = "No effect";
						return;
					case 9:
						fragName = "Cursed Fragment";
						fragDesc = "A supposedly 'cursed' data fragment. If you believe in that sort of thing ...";
						return;
					case 10:
						fragName = "Racing Fragment";
						fragDesc = "+30% thrust";
						return;
					case 11:
						fragName = "Commander Fragment";
						fragDesc = "5% hull repair per jump";
						return;
					case 12:
						fragName = "Cypher Fragment";
						fragDesc = "Increases ship program slots to 12";
						return;
					case 13:
						fragName = "W.D. Corp Fragment";
						fragDesc = "Installed tracker missiles are refilled by 10% each jump\n+20% armor";
						return;
				}
			}
        }
		[HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
		class ShipBuffs 
		{
			static void Postfix(PLShipStats __instance) 
			{
                if (__instance.Ship.GetIsPlayerShip() && PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(13)) 
				{
					__instance.HullArmor *= 1.2f;
				}
			}
		}

		[HarmonyPatch(typeof(PLShipInfoBase), "AboutToBeDestroyed")]
		class MoreMoneyOnDeath 
		{
			static void Prefix(PLShipInfoBase __instance) 
			{
				if(__instance.MyStats != null && __instance.MyHull.Current <= 0f && !__instance.GetIsPlayerShip() && PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(0)) 
				{
					__instance.CreditsLeftBehind = (int)(__instance.CreditsLeftBehind * 1.15f);
				}
			}
		}
    }
}
