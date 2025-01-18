using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Nudist_Specialists
{
    internal class RoleEffect_NoClothing : RoleEffect
    {
        public override bool IsBad
        {
            get
            {
                return true;
            }
        }
        public RoleEffect_NoClothing() {
            this.labelKey = "RoleEffectWontWearClothing";
        }
        public override bool CanEquip(Pawn pawn, Thing thing)
        {
            if(thing != null)
            {
                if (!util.ClothingCountsForNudityCheck(thing))
                {
                    return true;
                }   
            }
            return false;
        }
    }
}
