using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;


namespace SurgicallyExtractPsylinkNeuroformer
{
    
    public class Recipes_Surgically_Extract_Psylink_Neuroformer: Recipe_RemoveImplant
    {
        public bool PawnPsylinkCheck(Pawn pawn)
        {
#if DEBUG
            //Log.Message("pawn's name: " + pawn.Name);
            //Log.Message("pawn haspsylink: " + pawn.HasPsylink);
#endif
            if (pawn.HasPsylink)
            {
                if (pawn.GetPsylinkLevel() >= 1)
                {
#if DEBUG
                    //Log.Message("pawn haspsylink level : " + pawn.GetPsylinkLevel());
#endif
                    return true;
                }
            }
            return false; 
        }
        
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            if(pawn != null)
            {
                if (PawnPsylinkCheck(pawn))
                {
                    List<BodyPartRecord> Partsdef = pawn.RaceProps.body.GetPartsWithDef(recipe.appliedOnFixedBodyParts.First<BodyPartDef>());
                    if(Partsdef != null)
                    {
                        BodyPartRecord bodypartrecord = Partsdef.FirstOrDefault<BodyPartRecord>();
                        if (bodypartrecord == null) {
                            yield break;
                        }
                        
                        yield return bodypartrecord;
                    }
                }
            }
            yield break;
        }
        
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            MedicalRecipesUtility.IsClean(pawn, part);
            bool violationflag = IsViolationOnPawn(pawn, part, Faction.OfPlayer);
            if (billDoer != null)
            {
                if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
                {
                    return;
                }

                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
                pawn.ChangePsylinkLevel(-1, true);
                if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null), pawn.Position, pawn.Map, ThingPlaceMode.Near, null, (IntVec3 x) => x.InBounds(pawn.Map) && x.Standable(pawn.Map) && !x.Fogged(pawn.Map), default(Rot4)))
                {
                    Log.Error("Could not drop psylink neuroformer " + pawn.Position);
                }
            }

            if (violationflag)
            {
                ReportViolation(pawn, billDoer, pawn.HomeFaction, -30);
            }
        }
    }
}
