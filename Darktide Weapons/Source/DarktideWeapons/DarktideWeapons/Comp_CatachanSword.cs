using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace DarktideWeapons
{
    public class Comp_CatachanSword : ThingComp
    {
        public void ct()
        {
           
        }
    }
    public class CompProperties_CatachanSword : CompProperties
    {
        public CompProperties_CatachanSword()
        {
            this.compClass = typeof(Comp_CatachanSword);
        }
        public float CounterAttackChance;
        public float CounterAttackDamage;

    }

}
