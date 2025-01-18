using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Nudist_Specialists
{
    public class RoleRequirement_NudeSurvivalist : RoleRequirement
    {
        
        public override bool Met(Pawn p, Precept_Role role)
        {
            return util.PawnNudeCheck(p);
        }
    }
}
