using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Nudist_Specialists
{
    internal class ThoughtWorker_Precept_Nudism_Naked : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            return util.PawnNudeCheck(p);
        }
    }
}
