using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DarktideWeapons
{
    public static class RangedUtil
    {
        public static float Quality_Master_Multiplier = 1.25f;
        public static float Quality_Legendary_Multiplier = 1.5f;
        public static int Quality_Legendary_Stun_Tick_Multiplier = 2;
        public static int SlugStunTicks = 120;

        public static bool IsCrit(float chance)
        {
            if (Rand.Chance(chance))
            {
                return true;
            }
            return false;
        }
        public static bool IsStun(float chance)
        {
            if (Rand.Chance(chance))
            {
                return true;
            }
            return false;
        }
        public static bool CanBeStunned(Pawn victim, float stoppingPower)
        {          
             if(victim != null)
                {
                    //victim.stances
                    
                    if (!victim.DeadOrDowned  && victim.BodySize <= stoppingPower )
                    {
                        Pawn_StanceTracker stance = victim.stances;
                        if (stance != null)
                        {
                            return true;
                        }
                    }
                   
                } 
            return false;
        }

    }
}
