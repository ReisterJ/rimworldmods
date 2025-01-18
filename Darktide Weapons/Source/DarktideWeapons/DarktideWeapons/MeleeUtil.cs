using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DarktideWeapons
{
    public static class MeleeUtil
    {
        public static float Quality_Excellent_Bias = 1.25f;
        public static float Quality_Master_Bias = 1.5f;
        public static float Quality_Legendary_Bias = 2f;

        public static float CounterAttackStabChance = 0.4f;
        public static float CounterAttackArmorPenetrationBase = 0.5f;
        public static float CounterAttackStabArmorPenetration = CounterAttackArmorPenetrationBase * 2.5f;
        
        public static bool IsMeleeDamage(DamageInfo? dinfo)
        {
            if (((dinfo != null) ? dinfo.GetValueOrDefault().WeaponBodyPartGroup : null) != null 
                || ((dinfo != null) ? dinfo.GetValueOrDefault().WeaponLinkedHediff : null) != null
                || (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon))
            {
                return true;
            }
            return false;
        }
        public static bool Can_CounterAttack(Pawn pawn, DamageInfo dinfo, float chance)
        {
            if (pawn != null )
            {
                if(!pawn.DeadOrDowned && pawn.Drafted && IsMeleeDamage(dinfo))
                {
                    if (Rand.Chance(chance))
                    {
                        return true;
                    }
                }
                //if(pawn.equipment.Primary == )
            }
            return false;
        }
        public static void CounterAttack(Pawn wielder, Pawn opponent ,float damage)
        {
            
            if(wielder != null && opponent!= null)
            {
                DamageDef def;
                float armorPenetration = CounterAttackArmorPenetrationBase;
                if (Rand.Chance(CounterAttackStabChance))
                {
                    def = DamageDefOf.Stab;
                    armorPenetration = CounterAttackStabArmorPenetration;
                }
                def = DamageDefOf.Cut;
                //DamageInfo dinfo = new DamageInfo();
                DamageInfo dinfo = new DamageInfo(def, damage , armorPenetration , -1
                    , wielder, null, wielder.equipment.Primary.def, DamageInfo.SourceCategory.ThingOrUnknown, 
                    opponent, true, true, QualityCategory.Normal, true);
                opponent.TakeDamage(dinfo);
                MoteMaker.ThrowText(wielder.PositionHeld.ToVector3(), wielder.MapHeld, "CounterAttack", 1f);
            }

        }

        public static void ApplyBleeding(Pawn victim , int ticks)
        {
            if(victim != null)
            {
               
            }
        }

        public static void AbilityThunderHammerChargedStrike()
        {
            //
        }
        public static void AbilityPoweredSlash()
        {
            
        }

        public static void Ability_Test()
        {

        }
    }
}
