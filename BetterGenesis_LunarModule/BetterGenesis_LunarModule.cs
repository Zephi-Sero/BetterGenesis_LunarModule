using System;
using TheGenesisProjectModPatcher;
using TheGenesisProjectModPatcher.Mod;

namespace BetterGenesis_LunarModule {
    public class BetterGenesis_LunarModule : TGPMod {
        public override string ModName => "BetterGenesis_LunarModule";

        public override Version ModVersion => new Version("0.1.0");

        public override string ModAuthor => "zephyrouSerotonin";

        public override void AfterPatch() {}

        public override void BeforePatch() {
            ModPatcher.AddEventListener(new PlayerCustomizeListener(this));
            ModPatcher.AddEventListener(new WorldListener(this));
        }
    }
}
