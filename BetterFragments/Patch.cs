using HarmonyLib;
using UnityEngine;
namespace BetterFragments
{
    internal class Patch
    {
        [HarmonyPatch(typeof(PLTabMenu), "GetFragmentInfo")]
        class Descriptions 
        {
            static void Postfix(int fragID, out string fragName, out string fragDesc) 
            {
				if (Mod.MasterHasMod)
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
							fragDesc = "20% Player Health Boost\n10% turret damage";
							return;
						case 8:
							fragName = "Warger's Fragment";
							fragDesc = "+10 Weight capacity\n20% Speed boost";
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
				else 
				{
                    switch (fragID)
                    {
                        case 0:
                            fragName = "A.O.G. Fragment";
                            fragDesc = "Your ship will become unflagged each jump";
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
                            fragDesc = "Installed tracker missiles are refilled by 10% each jump";
                            return;
                    }
                }
			}
        }
		[HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
		class ShipBuffs 
		{
			static void Postfix(PLShipStats __instance) 
			{
                if (!Mod.MasterHasMod) return;
                if (__instance.Ship.GetIsPlayerShip() && PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(13)) 
				{
					__instance.HullArmor *= 1.2f;
				}
				if (__instance.Ship.GetIsPlayerShip() && PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(7))
				{
					__instance.TurretDamageFactor *= 1.1f;
				}
			}
		}

		[HarmonyPatch(typeof(PLShipInfoBase), "AboutToBeDestroyed")]
		class MoreMoneyOnDeath 
		{
			static void Prefix(PLShipInfoBase __instance) 
			{
                if (!Mod.MasterHasMod) return;
                if (__instance.MyStats != null && __instance.MyHull != null && __instance.MyHull.Current <= 0f && !__instance.GetIsPlayerShip() && PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(0)) 
				{
					__instance.CreditsLeftBehind = (int)(__instance.CreditsLeftBehind * 1.15f);
				}
			}
		}

		[HarmonyPatch(typeof(PLPawn),"Update")]
		class HealthBoost 
		{
			static void Postfix(PLPawn __instance) 
			{
                if (!Mod.MasterHasMod) return;
                if (__instance.GetPlayer() != null) 
				{
					float num10 = 100f;
					if (__instance.GetPlayer().RaceID == 2)
					{
						num10 = 60f;
					}
					float num11 = num10 + (float)__instance.GetPlayer().Talents[0] * 20f;
					num11 += (float)__instance.GetPlayer().Talents[57] * 20f;
					foreach (PawnStatusEffect pawnStatusEffect5 in __instance.MyStatusEffects)
					{
						if (pawnStatusEffect5 != null && pawnStatusEffect5.Type == EPawnStatusEffectType.HEALTH_REGEN)
						{
							num11 += 20f;
						}
					}
					float value2 = num11;
					if (__instance.GetPlayer().GetClassID() != -1 && __instance.GetPlayer().GetClassID() < 5 && __instance.GetPlayer().TeamID == 0)
					{
						PLServerClassInfo plserverClassInfo = PLServer.Instance.ClassInfos[__instance.GetPlayer().GetClassID()];
						num11 += (float)plserverClassInfo.SurvivalBonusCounter * 5f;
					}
					if(PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(7) && __instance.GetPlayer().TeamID == 0) 
					{
						num11 *= 1.2f;
					}
					if (__instance.MaxHealth != num11)
					{
						__instance.Health = __instance.Health / __instance.MaxHealth * num11;
						__instance.MaxHealth = num11;
						__instance.MaxHealth_Normal = value2;
					}
				}
			}
		}
		[HarmonyPatch(typeof(PLPlayer),"Update")]
		class PlayerBoost 
		{
			static void Postfix(PLPlayer __instance) 
			{
				if (!Mod.MasterHasMod) return;
				float num3 = 10f;
				if (__instance.RaceID == 1)
				{
					num3 = 20f;
				}
				else if (__instance.RaceID == 2)
				{
					num3 = 30f;
				}
				if (__instance.MyInventory != null)
				{
					__instance.MyInventory.WeightCapacity = num3 + (float)__instance.Talents[26] * 10f;
					if(PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(8) && __instance.TeamID == 0) 
					{
						__instance.MyInventory.WeightCapacity += 10;
					}
				}
			}
		}
		[HarmonyPatch(typeof(PLController),"Update")]
		class SpeedBoost 
		{
			static void Postfix(PLController __instance) 
			{
                if (!Mod.MasterHasMod) return;
                if (PLServer.Instance != null && __instance.MyPawn.MyPlayer != null)
				{
					PLPlayer cachedFriendlyPlayerOfClass = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0, __instance.MyPawn.MyPlayer.StartingShip);
					if (__instance.MyPawn.GetPlayer() != null && cachedFriendlyPlayerOfClass != null)
					{
						__instance.PawnSpeedModifier = 0.9f + (float)cachedFriendlyPlayerOfClass.Talents[5] * 0.06f;
					}
					else
					{
						__instance.PawnSpeedModifier = 0.9f;
					}
				}
				else
				{
					__instance.PawnSpeedModifier = 0.9f;
				}
				if(PLServer.Instance != null && PLServer.Instance.IsFragmentCollected(8)) 
				{
					__instance.PawnSpeedModifier *= 1.20f;
				}
			}
		}
		[HarmonyPatch(typeof(PLServer), "AddToShipLog")]
		class MoreMoney 
		{
			static void Postfix(string msg) 
			{
                if (!Mod.MasterHasMod) return;
                if (msg == "+50 Cr due to High Roller Fragment") 
				{
					PLServer.Instance.AddToShipLog("FRG", $"+{(int)(PLServer.Instance.CurrentCrewCredits * 0.05)} Cr due to High Roller Fragment", Color.white, true, null, null, -1, 0);
					if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0)
					{
						PLServer.Instance.AddNotification($"+{(int)(PLServer.Instance.CurrentCrewCredits * 0.05)} Cr due to High Roller Fragment", -1, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
					}
					PLServer.Instance.CurrentCrewCredits += (int)(PLServer.Instance.CurrentCrewCredits * 0.05);
				}
			}
		}
    }
}
