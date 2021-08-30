using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using TheGenesisProjectModPatcher;
using TheGenesisProjectModPatcher.Mod;
using TheGenesisProjectModPatcher.Mod.Event;
using UnityEngine;

namespace BetterGenesis_LunarModule {
    public class WorldListener : EventListener {
        public static GameObject derse, prospit;
        public static Dictionary<int, LunarPillar> dersepillars = new Dictionary<int, LunarPillar>();
        public static Dictionary<int, LunarPillar> prospitpillars = new Dictionary<int, LunarPillar>();
        public static GameObject basePillar;
        public static Vector3 wakePos;
        public WorldListener(TGPMod mod) : base(mod) {
            this.listeningFor = new Type[] { typeof(WorldManagerCreatedEvent), typeof(PlayerJoinEvent), typeof(SpritePrototypedEvent), typeof(PlayerSleepEvent) }.ToList();
        }
        public static void AddPillar(int playerid) {
            Vector3 vecOverride = new Vector3(0, -1, 0) ;
            foreach(KeyValuePair<int, LunarPillar> dp in dersepillars) {
                if(playerid < dp.Key) {
                    vecOverride = dp.Value.transform.localPosition;
                    dp.Value.transform.localPosition = new Vector3(3 / derse.transform.localScale.x, LunarPillar.PillarHeight*0.5f, 0);
                }
            } 
            foreach(KeyValuePair<int, LunarPillar> pp in prospitpillars) {
                if(playerid < pp.Key) {
                    vecOverride = pp.Value.transform.localPosition;
                    pp.Value.transform.localPosition = new Vector3(3 / prospit.transform.localScale.x, LunarPillar.PillarHeight*0.5f, 0);
                }
            }
            Vector3 vecOverrideD = new Vector3(LunarPillar.pillarDerse * vecOverride.x, vecOverride.y, vecOverride.z);
            Vector3 vecOverrideP = new Vector3(LunarPillar.pillarProspit * vecOverride.x, vecOverride.y, vecOverride.z);
            LunarPillar dersepillar = UnityEngine.Object.Instantiate(basePillar).AddComponent<LunarPillar>();
            dersepillar.ErectPillar(derse, vecOverrideD);
            dersepillar.gameObject.SetActive(true);
            dersepillar.linkedPlayerID = playerid;
            dersepillars.Add(playerid, dersepillar);
            LunarPillar prospitpillar = UnityEngine.Object.Instantiate(basePillar).AddComponent<LunarPillar>();
            prospitpillar.ErectPillar(prospit, vecOverrideP);
            prospitpillar.gameObject.SetActive(true);
            prospitpillar.linkedPlayerID = playerid;
            prospitpillars.Add(playerid, prospitpillar);
        }
        public override bool OnEvent(IGameEvent evt) {
            if(evt.GetType() == typeof(SpritePrototypedEvent)) {
                SpritePrototypedEvent e = (SpritePrototypedEvent)evt;
                dersepillars[e.PlayerID].ActivateBall();
                prospitpillars[e.PlayerID].ActivateBall();
            } else if(evt.GetType() == typeof(PlayerJoinEvent)) {
                PlayerJoinEvent e = (PlayerJoinEvent)evt;
                if(!dersepillars.ContainsKey(e.PlayerID)) {
                    AddPillar(e.PlayerID);
                }
            } else if(evt.GetType() == typeof(WorldManagerCreatedEvent)) {
                derse = GameObject.CreatePrimitive(PrimitiveType.Plane);
                derse.name = "Derse_Plane";
                prospit = GameObject.CreatePrimitive(PrimitiveType.Plane);
                prospit.name = "Prospit_Plane";
                derse.transform.localScale = new Vector3(20, 1, 20);
                prospit.transform.localScale = new Vector3(20, 1, 20);
                derse.GetComponent<Renderer>().material.color = new Color(186f/255f, 0f, 253f/255f);
                prospit.GetComponent<Renderer>().material.color = new Color(253f/255f, 229f/255f, 0f);
                derse.transform.position = new Vector3(20*1024, 0, 0);
                prospit.transform.position = new Vector3(21*1024, 0, 0);
                basePillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                basePillar.name = "Pillar";
                basePillar.SetActive(false);
                basePillar.AddComponent<LunarPillar>();
                AddPillar(NetcodeManager.LocalPlayerId);
                if(PlayerPrefs.GetString("BetterGenesis_LunarModule.LunarSway.Data", "prospit").ToLowerInvariant() == "derse") {
                    wakePos = derse.transform.position + new Vector3(-5, 0, -5);
                } else {
                    wakePos = prospit.transform.position + new Vector3(-5, 0, -5);
                }
            } else if(evt.GetType() == typeof(PlayerSleepEvent)) {
                //PlayerSleepEvent e = (PlayerSleepEvent)evt;
                Player p = NetcodeManager.Instance.GetPlayer(NetcodeManager.LocalPlayerId);
                if(p == null) {
                    ModLogger.WriteLine(mod.ModName,"Error, player null!");
                } else if(WorldManager.GetManager().GetHouse(p.GetPosition()) != null) {
                    WorldManager.GetManager().GoOutside();
                }
                Vector3 sleeppos = p.GetPosition();
                p.SetPosition(wakePos + 0.1f * Vector3.up);
                wakePos = sleeppos;
            }
            return true;
        }
    }
}
