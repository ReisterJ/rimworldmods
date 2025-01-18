using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace DarktideWeapons
{
    public class ModExtension_DWBullet: DefModExtension 
    {
        public float critChance;
        public float critMultiplier;
        public float stunChance;
        public int stunTicks;
        public int objectPenetrationBase;
        public ModExtension_DWBullet () {
            critChance = 0f;
            critMultiplier = 1f;
            stunChance = 0f;
            stunTicks = 0;
            objectPenetrationBase = 0;
        }


    }
    //Infantry autogun bullet 

    public class ModExtension_IFAGBullet: DefModExtension
    {
        public float critChance;
        public float critMultiplier;
        public static ModExtension_IFAGBullet Defaultvalue = new ModExtension_IFAGBullet();
        public ModExtension_IFAGBullet(float critChance, float critMultiplier)
        {
            this.critChance = critChance;
            this.critMultiplier = critMultiplier;
          
        }
        public ModExtension_IFAGBullet() {
            critChance = 0.2f;
            critMultiplier = 2f;
        }
    }
    public class ModExtension_BRAGBullet: DefModExtension
    {
        public float stunChance;
        public int stunTicks;
        public static ModExtension_BRAGBullet Defaultvalue = new ModExtension_BRAGBullet();
        public ModExtension_BRAGBullet(float stunChance, int stunTicks)
        {
            this.stunChance = stunChance;
            this.stunTicks = stunTicks;
        }
        public ModExtension_BRAGBullet() {
            this.stunChance = 0.30f;
            this.stunTicks = 60;
        }
    }
    /*
    public class ModExtension_ShotgunSlug: DefModExtension
    {
        public int stunTicksSlug = 120;

    }
    */
}
