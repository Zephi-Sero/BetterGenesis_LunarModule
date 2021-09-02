using System;
using System.Collections.Generic;
using TheGenesisProjectModPatcher;
using TheGenesisProjectModPatcher.Mod;

namespace BetterGenesis_LunarModule {
    public class BetterGenesis_LunarModule : TGPMod {
        public override string ModName => "BetterGenesis_LunarModule";
        public override string ModVersion => "0.1.1";
        public override string ModAuthor => "zephyrouSerotonin";
        private static readonly List<string> loadafter = new List<string>();
        private static readonly List<string> modtags = new List<string>(new string[] { "worldgen", "character_customization" });
        public override Pair<string, string>[] Dependencies => new Pair<string, string>[]{
            new Pair<string,string>("TheGenesisProjectModPatcher","[0.2.0,0.3.0)")
        };

        public override void PatchMod() {
            ModPatcher.AddEventListener(new PlayerCustomizeListener(this));
            ModPatcher.AddEventListener(new WorldListener(this));
        }
    }
}
