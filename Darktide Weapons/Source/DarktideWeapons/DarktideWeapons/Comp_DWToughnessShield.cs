using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace DarktideWeapons
{
    [StaticConstructorOnStartup]
    public class Comp_DWToughnessShield: ThingComp
    {
        private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
        private Vector3 impactAngleVect;

        protected float energy;

        protected int lastAbsorbDamageTick = -9999;

        public float KillRechargeinGame;
        
        protected int StartTickstoResetinGame;
        
        protected float MaxToughnessinGame;
        
        public float ToughnessDamageReductionMultiplierinGame;

        protected bool AbletoCounter = false;


        protected int ticksToReset = -1;
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            KillRechargeinGame = DWTSProp.killRechargeToughnessBase;
            StartTickstoResetinGame = DWTSProp.startTickstoResetBase;
            MaxToughnessinGame = DWTSProp.maxToughnessBase;
            ToughnessDamageReductionMultiplierinGame = DWTSProp.toughnessDamageReductionMultiplier;
        }

        public CompProperties_DWToughnessShield DWTSProp
        {
            get
            {
                return (CompProperties_DWToughnessShield)this.props;
            } 
        }
        public float MaxToughness
        {
            get
            {
                return this.MaxToughnessinGame;
            }
        }
        public float Energy
        {
            get
            {
                return this.energy;
            }
        }

       
        public ShieldState ShieldState
        {
            get
            {
                if (PawnOwner == null)
                {
                    return ShieldState.Disabled;
                }

                if (ticksToReset <= 0)
                {
                    return ShieldState.Active;
                }

                return ShieldState.Resetting;
            }
        }
        public Pawn PawnOwner
        {
            get
            {
                Apparel apparel;
                if ((apparel = (this.parent as Apparel)) != null)
                {
                    return apparel.Wearer;
                }

                Pawn_EquipmentTracker P_E= this.parent.holdingOwner.Owner as Pawn_EquipmentTracker;
                Pawn result;
                if (P_E != null)
                {
                    result = P_E.pawn;
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
        }

        protected bool ShouldDisplay
        {
            get
            {
                Pawn pawnOwner = this.PawnOwner;
                return pawnOwner.Spawned && !pawnOwner.Dead && !pawnOwner.Downed
                        //&& (pawnOwner.InAggroMentalState || pawnOwner.Drafted || (pawnOwner.Faction.HostileTo(Faction.OfPlayer) && !pawnOwner.IsPrisoner) 
                        //|| Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks
                        //|| (ModsConfig.BiotechActive && pawnOwner.IsColonyMech && Find.Selector.SingleSelectedThing == pawnOwner))
                        ;
            }
        }
        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
            {
                yield return gizmo;
            }
            IEnumerator<Gizmo> enumerator = null;
            
            yield break;
            //yield break;
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            IEnumerator<Gizmo> enumerator = null;
            if (PawnOwner != null) {

                foreach (Gizmo gizmo2 in this.GetGizmos())
                {
                    yield return gizmo2;
                }
                enumerator = null;

            }

            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Break",
                    action = new Action(this.ToughnessBreak)
                };
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Output",
                    action = new Action(this.DEVoutput)
                };

                if (this.ticksToReset > 0)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "DEV: Clear reset",
                        action = delegate ()
                        {
                            this.ticksToReset = 0;
                        }
                    };
                }
            }
            yield break;
        }
        public virtual IEnumerable<Gizmo> GetGizmos()
        {
            if (Find.Selector.SingleSelectedThing == this.PawnOwner)
            {
                yield return new Gizmo_ToughnessShieldStatus
                {
                    ToughnessShield = this
                };
            }
            yield break;
        }
        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            if (this.ShieldState != ShieldState.Active || this.PawnOwner == null)
            {
                return;
            }
            float toughnessdamage = dinfo.Amount * ToughnessDamageReductionMultiplierinGame;
            ticksToReset = this.StartTickstoResetinGame;

            Log.Message("origin toughness :" + this.energy);
            if (MeleeUtil.IsMeleeDamage(dinfo))
            {

                Log.Message("ismeleedamage");
                if (this.AbletoCounter)
                {
                    //counter
                    return;
                }
                Log.Message("origin meleedamage :"+ dinfo.Amount);
                float PastToughnessDamage = (this.MaxToughnessinGame - this.energy) / this.MaxToughnessinGame * toughnessdamage;
                if (PastToughnessDamage < 0) PastToughnessDamage = 0;
                dinfo.SetAmount(PastToughnessDamage);
                if (this.energy <= toughnessdamage)
                {
                    this.ToughnessBreak();
                }
                Log.Message("origin meleedamage :" + dinfo.Amount);
                return;
            }
            else// (dinfo.Def.isRanged || dinfo.Def.isExplosive)
            {
               
                if (this.energy <= toughnessdamage)
                {
                    this.ToughnessBreak();
                }
                else
                {
                    this.energy -= toughnessdamage;
                    //this.ToughnessAbsorbedDamage(dinfo);
                }
                absorbed = true;
            }
            Log.Message("Post attack toughness :" + this.energy); 
        }

        public override void CompTick()
        {
            base.CompTick();

            //if (Find.Selector.SingleSelectedThing != null)
            //{
            //    Log.Message("selected :" + Find.Selector.SingleSelectedThing.ThingID);
            //}
            if (this.PawnOwner == null)
            {
                this.energy = 0f;
                //Log.Error("No owner");
                return;
            }
            Log.Message("Pawnowner :"+this.PawnOwner.ThingID);
            if (this.ShieldState == ShieldState.Resetting)
            {
                this.ticksToReset--;
                //Log.Message("resetting :" + this.ticksToReset);
                if (this.ticksToReset <= 0)
                {
                    this.Reset();
                    return;
                }
            }
            else if (this.ShieldState == ShieldState.Active)
            {
                this.energy = this.MaxToughnessinGame;
                if (this.energy > this.MaxToughnessinGame)
                {
                    this.energy = this.MaxToughnessinGame;
                }
                //Log.Message("energy :" + this.energy);
            }
            //
        }
        public override float CompGetSpecialApparelScoreOffset()
        {
            return this.MaxToughnessinGame; //* this.ApparelScorePerEnergyMax;
        }

        public void ToughnessBreak()
        {
            //float scale = Mathf.Lerp(this.Props.minDrawSize, this.Props.maxDrawSize, this.energy);
            //EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, scale);
            //FleckMaker.Static(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, FleckDefOf.ExplosionFlash, 12f);
            //for (int i = 0; i < 6; i++)
            //{
            //    FleckMaker.ThrowDustPuff(this.PawnOwner.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), this.PawnOwner.Map, Rand.Range(0.8f, 1.2f));
            //}
            this.energy = 0f;
            this.ticksToReset = this.StartTickstoResetinGame;

        
        }
        

        //Toughness doesn't block pawns casting
        public override bool CompAllowVerbCast(Verb verb)
        {
            return true;
        }

        public virtual void Recharge_Afterkill()
        {
            if (this.energy + KillRechargeinGame <= this.MaxToughnessinGame)
            {
                this.energy += KillRechargeinGame;
                return;
            }
            this.energy = this.MaxToughnessinGame;

            
        }

        public override void Notify_KilledPawn(Pawn pawn)
        {
            base.Notify_KilledPawn(pawn);
           

            if (pawn != null)
            {
                //Log.Message(pawn.Name + " kills a pawn");
                this.Recharge_Afterkill();
            }
        }
       
        
        public override void PostDraw()
        {
            base.PostDraw();
            
        }

        protected void Draw()
        {
            if (this.ShieldState == ShieldState.Active && this.ShouldDisplay)
            {
                float num = Mathf.Lerp(1.2f, 1.55f, this.energy);
                Vector3 vector = this.PawnOwner.Drawer.DrawPos;
                vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
                int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
                if (num2 < 8)
                {
                    float num3 = (float)(8 - num2) / 8f * 0.05f;
                    vector += this.impactAngleVect * num3;
                    num -= num3;
                }
                float angle = (float)Rand.Range(0, 360);
                Vector3 s = new Vector3(num, 1f, num);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
            }
        }

        private void Reset()
        {
            //if (this.PawnOwner.Spawned)
            //{
            //    SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map, false));
            //    FleckMaker.ThrowLightningGlow(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, 3f);
            //}
            this.ticksToReset = -1;
            //this.energy = this.Props.energyOnReset;
        }

        public void SetAbletoCounter()
        {
            this.AbletoCounter = true;
        }


        public void DEVoutput()
        {
            Log.Message("this DWTcomp parent: " + this.parent.ToString());
            Log.Message("this DWTcomp parent's holder " + this.PawnOwner.Name);
            Log.Warning("energy :" + this.energy);
            Log.Message("single selected :"+Find.Selector.SingleSelectedThing.ThingID);
            Log.Message("this DWTcomp parent's holder id" + this.PawnOwner.ThingID);
        }
    }

    public class CompProperties_DWToughnessShield : CompProperties
    {
        public CompProperties_DWToughnessShield()
        {
            this.compClass = typeof(Comp_DWToughnessShield);
        }
        public float toughnessDamageReductionMultiplier;
        public float toughnessRechargeMultiplier;
        public float maxToughnessBase;
        public int startTickstoResetBase;
        public float killRechargeToughnessBase;

    }

    [StaticConstructorOnStartup]
    public class Gizmo_ToughnessShieldStatus : Gizmo
    {
        public Comp_DWToughnessShield ToughnessShield;

        private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public Gizmo_ToughnessShieldStatus()
        {
            Order = -100f;
        }

        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);
            Rect rect3 = rect2;
            rect3.height = rect.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect3,   "ToughnessShield".Translate().Resolve());
            Rect rect4 = rect2;
            rect4.yMin = rect2.y + rect2.height / 2f;
            float fillPercent = ToughnessShield.Energy / Mathf.Max(1f, ToughnessShield.MaxToughness);
            Widgets.FillableBar(rect4, fillPercent, FullShieldBarTex, EmptyShieldBarTex, doBorder: false);
            Text.Font = GameFont.Small;
            Text.Anchor = (TextAnchor)4;
            Widgets.Label(rect4, (ToughnessShield.Energy ).ToString("F0") + " / " + (ToughnessShield.MaxToughness).ToString("F0"));
            Text.Anchor = (TextAnchor)0;
            TooltipHandler.TipRegion(rect2, "ToughnessShieldPersonalTip".Translate());
            return new GizmoResult(GizmoState.Clear);
        }
    }

    public class ToughnessShieldWorker
    {
        public Pawn Owner;
    }
}
