using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGenesisProjectModPatcher;
using TheGenesisProjectModPatcher.Character;

namespace BetterGenesis_LunarModule {
    public class LunarSway : ModdedCharacterFeature {
        readonly string sway = "prospit";
        public LunarSway(string loadedData) : base(loadedData) {
            this.sway=loadedData;
        }
        public override string ToSaveableString() {
            return sway;
        }
    }
}
