using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DarktideWeapons
{
    public class DW_Bullet_Def: ThingDef
    {
        public float critChance;
        public float critMultiplier;

        public float stunChance;
        public int stunTicks;

        public int objectPenetrationBase;

        public static DW_Bullet_Def DEFAULTVALUE = new DW_Bullet_Def();


        
    }

    /*
    public class Bullet_IFAG: Bullet
    {
       
        protected void WeaponQuality_bias(ModExtension_IFAGBullet IFAG_Bullet)
        {
            switch (this.equipmentQuality)
            {
                case QualityCategory.Masterwork:
                    IFAG_Bullet.critChance *= RangedUtil.Quality_Master_Bias;
                    break;
                case QualityCategory.Legendary:
                    IFAG_Bullet.critChance *= RangedUtil.Quality_Legendary_Bias;
                    IFAG_Bullet.critMultiplier  *= RangedUtil.Quality_Legendary_Bias;
                    break;

                default:
                    break;
            }
        }
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            ModExtension_IFAGBullet IFAG_Bullet = this.def.GetModExtension<ModExtension_IFAGBullet>() ?? ModExtension_IFAGBullet.Defaultvalue;
            this.WeaponQuality_bias(IFAG_Bullet);
            bool critFlag = RangedUtil.IsCrit(IFAG_Bullet.critChance);
            if(critFlag)
            {
                this.weaponDamageMultiplier *= IFAG_Bullet.critMultiplier;
            }
            base.Impact(hitThing, blockedByShield);
            
            //this.weaponDamageMultiplier = 1f;
            //Log.Message("Crit shot?");
            Log.Message("critchance :"+ IFAG_Bullet.critChance);
            if (hitThing != null)
            {
                //Log.Message("hit");
                //Log.Message(rand);
                if (critFlag)
                {
                    MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "Crit", 2f);
                }
                
              
            }
        }
    }

    //Braced autogun bullet
    public class Bullet_BRAG : Bullet
    {
        protected void WeaponQuality_bias(ModExtension_BRAGBullet BRAGBullet)
        {
            switch (this.equipmentQuality)
            {
                
                case QualityCategory.Masterwork:
                    BRAGBullet.stunChance *= RangedUtil.Quality_Master_Bias;
                    break;
                case QualityCategory.Legendary:
                    BRAGBullet.stunChance *= RangedUtil.Quality_Legendary_Bias;
                    BRAGBullet.stunTicks += RangedUtil.Quality_Legendary_Stun_Tick;
                    break;

                default:
                    break;
            }
        }
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            ModExtension_BRAGBullet BRAGBullet = this.def.GetModExtension<ModExtension_BRAGBullet>() ?? ModExtension_BRAGBullet.Defaultvalue;
            //bool stunFlag = false;

            base.Impact(hitThing, blockedByShield);
            if (hitThing != null)
            {
                Pawn victim = hitThing as Pawn;
                if (victim != null && RangedUtil.CanBeStunned(victim, this.def.projectile.stoppingPower))
                {
                    this.WeaponQuality_bias(BRAGBullet);
                    if (RangedUtil.IsStun(BRAGBullet.stunChance)) 
                        victim.stances.stunner.StunFor(BRAGBullet.stunTicks, this);
                }
               
            }
            
        }
    }
    public class Bullet_ShotgunSlug: Bullet
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {

            base.Impact(hitThing, blockedByShield);
            if (hitThing != null)
            {
                Pawn victim = hitThing as Pawn;
                if (RangedUtil.CanBeStunned(victim, this.def.projectile.stoppingPower))
                {
                        victim.stances.stunner.StunFor(RangedUtil.SlugStunTicks, this);
                }

            }

        }
    }
    */
}
