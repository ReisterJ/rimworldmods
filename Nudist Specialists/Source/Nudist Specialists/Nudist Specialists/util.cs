using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Nudist_Specialists
{
    internal static class util
    {
        public static bool PawnNudeCheck(Pawn p)
        {
            if (p == null) return false;
            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                Apparel apparel = wornApparel[i];
                if (apparel.def.apparel.countsAsClothingForNudity)
                {
                    for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
                    {
                        if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
                        {
                            return false;
                        }
                        if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public static bool ClothingCountsForNudityCheck(Thing thing)
        {
            if(thing != null)
            {
                if (thing.def.IsApparel)
                {
                    Apparel apparel = thing as Apparel;
#if DEBUG
                    Log.Message("Try to wear " + apparel.def.defName.ToString());
#endif
                    if (apparel.def.apparel.countsAsClothingForNudity)
                    {
                        for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
                        {
                            if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
                            {
                                return true;
                            }
                            if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
