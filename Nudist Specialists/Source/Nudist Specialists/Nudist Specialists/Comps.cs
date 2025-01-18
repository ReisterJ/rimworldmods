using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nudist_Specialists
{
    internal class CompAbilityEffect_CheckNudeSpecialists : CompAbilityEffect_WithDest
    {
        private new CompProperties_CheckNudeSpecialists Props
        {
            get
            {
                return (CompProperties_CheckNudeSpecialists)this.props;
            }
        }
        public override bool GizmoDisabled(out string reason)
        {
            if (base.CasterPawn != null)
            {
                if (util.PawnNudeCheck(base.CasterPawn))
                {
                    reason = null;
                    return false;
                }
                else
                {
                    reason = base.CasterPawn.Name.ToStringFull + " is not a nudist ";
                    return true;
                }
            }
            reason = null;
            return true;
        }
    }

    public class CompProperties_CheckNudeSpecialists : CompProperties_EffectWithDest
    {
        // Token: 0x060001B9 RID: 441 RVA: 0x0000A58D File Offset: 0x0000878D
        public CompProperties_CheckNudeSpecialists()
        {
            this.compClass = typeof(CompAbilityEffect_CheckNudeSpecialists);
        }
    }
}
