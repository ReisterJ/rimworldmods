using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DarktideWeapons
{
    [StaticConstructorOnStartup]
    internal class Comp_EquipmentTicks : ThingComp
    {
        public int Equiptick;
        public int UnEquiptick;

        public override void CompTick()
        {
            base.CompTick();
            
        }

    }
}
