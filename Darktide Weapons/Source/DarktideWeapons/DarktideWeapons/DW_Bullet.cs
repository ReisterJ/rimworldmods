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
    public class DW_Bullet : Bullet
    {
        public virtual void DW_Bullet_inGameProcess()
        {
            //Log.Message("critchance " + this.DW_BulletDef.critChance);
            this.stunChanceinGame = this.DW_BulletDef.stunChance;
            this.stunTicksinGame = this.DW_BulletDef.stunTicks;
            this.critChanceinGame = this.DW_BulletDef.critChance;
            this.critMultiplierinGame = this.DW_BulletDef.critMultiplier;
            this.objectPenetrationBaseinGame = this.DW_BulletDef.objectPenetrationBase;

            WeaponQuality_bias();
        }

        public override void Tick()
        {
            base.Tick();

        }
        public DW_Bullet_Def DW_BulletDef
        {
            get
            {
                return (this.def as DW_Bullet_Def) ?? DW_Bullet_Def.DEFAULTVALUE;   
            }
        }
        protected virtual void WeaponQuality_bias()
        {
            switch (this.equipmentQuality)
            {

                case QualityCategory.Masterwork:
                    this.critChanceinGame = this.DW_BulletDef.critChance * RangedUtil.Quality_Master_Multiplier;
                    this.stunChanceinGame = this.DW_BulletDef.stunChance * RangedUtil.Quality_Master_Multiplier;
                    break;
                case QualityCategory.Legendary:
                    this.stunChanceinGame = this.DW_BulletDef.stunChance * RangedUtil.Quality_Legendary_Multiplier;
                    this.stunTicksinGame = this.DW_BulletDef.stunTicks * RangedUtil.Quality_Legendary_Stun_Tick_Multiplier;
                    this.critChanceinGame = this.DW_BulletDef.critChance * RangedUtil.Quality_Legendary_Multiplier;
                    this.critMultiplierinGame = this.DW_BulletDef.critMultiplier * RangedUtil.Quality_Legendary_Multiplier;
                    break;

                default:
                    
                    break;
            }
        }
        
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            //base.Impact(hitThing, blockedByShield);
            Map map = base.Map;
            IntVec3 position = base.Position;
            if (this.equipmentDef != null) Log.Message(this.equipmentDef);
            
            if (hitThing != null)
            {
                DW_Bullet_inGameProcess();
                BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef);
                Find.BattleLog.Add(battleLogEntry_RangedImpact);
                this.NotifyImpact(hitThing, map, position);
                if (Critshot()) MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "Crit", 1f);
                Pawn pawn;
                bool instigatorGuilty = (pawn = (this.launcher as Pawn)) == null || !pawn.Drafted;
                DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, (float)this.DamageAmount, this.ArmorPenetration, 0f,
                    this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing, true, true, QualityCategory.Normal, true);
                dinfo.SetWeaponQuality(this.equipmentQuality);
                hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
                Pawn pawn2 = hitThing as Pawn;
                if(pawn2 != null)
                {
                    pawn2?.stances?.stagger.Notify_BulletImpact(this);
                }
                Stunshot(hitThing);
            }
            if (!blockedByShield)
            {
                SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map));
                if (base.Position.GetTerrain(map).takeSplashes)
                {
                    FleckMaker.WaterSplash(ExactPosition, map, Mathf.Sqrt(DamageAmount) * 1f, 4f);
                }
                else
                {
                    FleckMaker.Static(ExactPosition, map, FleckDefOf.ShotHit_Dirt);
                }
            }
            this.Destroy(DestroyMode.Vanish);
            //WeaponQuality_bias();
            //Critshot();
            //Log.Message(this.critChanceinGame);
            //DW_Bullet_inGameProcess();
            //if(Critshot()) MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "Crit", 2f); 
            //base.Impact(hitThing, blockedByShield);
            //Stunshot(hitThing);
        }

        
        public override bool AnimalsFleeImpact
        {
            get
            {
                return true;
            }
        }
        protected virtual bool Critshot()
        {
            Log.Message("critchance :" + this.critChanceinGame);
            if (this.critChanceinGame <= 0) return false; 
            bool critFlag = RangedUtil.IsCrit(this.critChanceinGame);
            if (critFlag)
            {
                this.weaponDamageMultiplier *= this.critMultiplierinGame;
            }

            
            return critFlag;
        }

        protected virtual void Stunshot(Thing hitThing)
        {
            if (this.stunChanceinGame <= 0) return;
            if (hitThing != null)
            {
                Pawn victim = hitThing as Pawn;
                if (victim != null && RangedUtil.CanBeStunned(victim, this.def.projectile.stoppingPower))
                {
                    if (RangedUtil.IsStun(this.stunChanceinGame))
                        victim.stances.stunner.StunFor(this.stunTicksinGame, this);
                }
            }
        }
        protected virtual void NotifyImpact(Thing hitThing, Map map, IntVec3 position)
        {
            BulletImpactData impactData = new BulletImpactData
            {
                bullet = this,
                hitThing = hitThing,
                impactPosition = position
            };
            if (hitThing != null)
            {
                hitThing.Notify_BulletImpactNearby(impactData);
            }
            int num = 9;
            for (int i = 0; i < num; i++)
            {
                IntVec3 c = position + GenRadial.RadialPattern[i];
                if (c.InBounds(map))
                {
                    List<Thing> thingList = c.GetThingList(map);
                    for (int j = 0; j < thingList.Count; j++)
                    {
                        if (thingList[j] != hitThing)
                        {
                            thingList[j].Notify_BulletImpactNearby(impactData);
                        }
                    }
                }
            }
        }

        public float critChanceinGame;
        public float critMultiplierinGame;

        public float stunChanceinGame;
        public int stunTicksinGame;

        public int objectPenetrationBaseinGame;

        public virtual bool PenetrateTarget()
        {
            if (this.objectPenetrationBaseinGame <= 0)
            {
                return false;
            }
            this.objectPenetrationBaseinGame--;
            return true;
        }

        public bool CanPenetrateTarget()
        {
            if (this.objectPenetrationBaseinGame <= 0)
            {
                return false;
            }
            return true;
        }

        public void NoMorePenetration()
        {
            this.objectPenetrationBaseinGame = 0;
        }
    }
}
