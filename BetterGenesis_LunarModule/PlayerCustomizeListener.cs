using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGenesisProjectModPatcher;
using TheGenesisProjectModPatcher.Character;
using TheGenesisProjectModPatcher.Mod;
using TheGenesisProjectModPatcher.Mod.Event;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BetterGenesis_LunarModule {
    internal class PlayerCustomizeListener : EventListener {
        private GameObject characterCreationWindow;
        private UnityEngine.Events.UnityAction<CharacterOptionsGridComponent.SimpleExtendableEventArgs> action;
        public GameObject panel;
        private GameObject sway;
        private string lastPicked = "";
        private bool swaypicker_opened = false;
        private static ChangeSpritePart CSP;
        public PlayerCustomizeListener(TGPMod mod) : base(mod) {
            listeningFor = new Type[] { typeof(PlayerCustomizeOpenEvent), typeof(ChangeSpritePartStartEvent), typeof(ChangeSpritePartRefreshEvent), typeof(ChangeSpritePartCommitEvent), typeof(PlayerCustomizeCanceledEvent), typeof(PlayerCustomizeFinishedEvent)}.ToList();
        }
        public static Transform CreateButton(Transform parent, string txt, Color buttoncolor, Color textcolor, UnityEngine.Events.UnityAction action2) {
            GameObject button = new GameObject("BetterGenesis_LunarModule_" + txt);
            button.transform.SetParent(parent, false);
            Image img = button.AddComponent<Image>();
            img.sprite = null;
            img.color = buttoncolor;
            Button btn = button.AddComponent<Button>();
            btn.onClick.AddListener(action2);
            btn.targetGraphic = img;
            GameObject textobj = new GameObject("Text");
            textobj.transform.SetParent(button.transform, false);
            Text text = textobj.AddComponent<Text>();
            text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.fontSize = 24;
            text.color = textcolor;
            text.text = txt;
            text.alignment = TextAnchor.MiddleCenter;
            return button.transform;
        }
        public static Transform CreateButton(Transform parent, Sprite sprite, UnityEngine.Events.UnityAction action2) {
            GameObject button = new GameObject("BetterGenesis_LunarModule_Button");
            button.transform.SetParent(parent, false);
            Image img = button.AddComponent<Image>();
            img.sprite = sprite;
            Button btn = button.AddComponent<Button>();
            btn.onClick.AddListener(action2);
            btn.targetGraphic = img;
            return button.transform;
        }
        public static Transform CreateButton(Transform parent, Texture2D texture, UnityEngine.Events.UnityAction action2) {
            GameObject button = new GameObject("BetterGenesis_LunarModule_Button");
            button.transform.SetParent(parent, false);
            Image img = button.AddComponent<Image>();
            img.sprite = Sprite.Create(texture, new Rect(0f,0f,texture.width,texture.height), new Vector2(0.5f,0.5f));
            Button btn = button.AddComponent<Button>();
            btn.onClick.AddListener(action2);
            btn.targetGraphic = img;
            return button.transform;
        }
        public void LoadSwayImage(Transform parent, CharacterSettings settings) {
            Image img;
            bool swaynull = sway == null;
            if(swaynull) {
                sway = new GameObject("LunarSway");
                img = sway.AddComponent<Image>();
            } else {
                img = sway.GetComponent<Image>();
            }
            sway.transform.SetParent(parent, false);
            string sway_lower = (UnityEngine.Random.Range(0, 3) == 0 ? "prospit" : "derse");
            if(lastPicked != "") {
                sway_lower = lastPicked;
            } else if(settings.moddedFeatures.TryGetValue("BetterGenesis_LunarModule.LunarSway", out InternalPatcher.Character.ModdedCharacterFeature mcf)) {
                sway_lower = mcf.ToSaveableString().ToLowerInvariant();
            }
            string sway_icon = BetterGenesis_LunarModule.ModAssets + "/" + sway_lower + "icon.png";
            sway.name = "LunarSway_" + sway_lower;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(sway_icon));
            img.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            RectTransform rt = (RectTransform)sway.transform;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
        }
        public override bool OnEvent(IGameEvent evt) {
            switch(evt.GetType().Name) {
                case "ChangeSpritePartStartEvent":
                    action = CSP.CharacterOptionsGrid.GetExtendableHandler();
                    break;
                case "ChangeSpritePartRefreshEvent":
                    ChangeSpritePartRefreshEvent e1 = (ChangeSpritePartRefreshEvent)evt;
                    CSP = e1.CSP;
                    LoadSwayImage(CSP.CharacterOptionsGrid.transform.parent, CSP.local_character);
                    break;
                case "PlayerCustomizeCanceledEvent":
                case "PlayerCustomizeFinishedEvent":
                    lastPicked = "";
                    break;
                case "ChangeSpritePartCommitEvent":
                    CSP.ModifyFeature("BetterGenesis_LunarModule.LunarSway", new LunarSway(sway.name.Split('_')[1]));
                    LoadSwayImage(CSP.CharacterOptionsGrid.transform.parent, CSP.local_character);
                    break;
                case "PlayerCustomizeOpenEvent":
                    Transform t = CSP.CharacterOptionsGrid.transform.parent.parent;
                    characterCreationWindow = t.gameObject;
                    CharacterCustomizer.AddModToCustomizer(mod.ModName, delegate {
                        if(!swaypicker_opened) {
                            panel = new GameObject("BetterGenesis_LunarModule_Panel");
                            panel.transform.SetParent(characterCreationWindow.transform, false);
                            Image img = panel.AddComponent<Image>();
                            img.sprite = null;
                            img.color = Color.black;
                            RectTransform rt = (RectTransform)panel.transform;
                            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
                            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 112);
                            RectTransform dbrt = (RectTransform)CreateButton(panel.transform, "Derse", new Color(186f/255f, 0f, 253f/255f), new Color(253f/255f, 229f/255f, 0f), delegate {
                                CharacterOptionsGridComponent.SimpleExtendableEventArgs seea = new CharacterOptionsGridComponent.SimpleExtendableEventArgs {
                                    data = new Dictionary<string, InternalPatcher.Character.ModdedCharacterFeature>() {
                                {"BetterGenesis_LunarModule.LunarSway", new LunarSway("derse")}
                            }
                                };
                                lastPicked = "derse";
                                action(seea);
                                UnityEngine.Object.Destroy(panel);
                                swaypicker_opened = false;
                            });
                            dbrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 80);
                            dbrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);
                            dbrt.anchoredPosition = new Vector2(-40, -16);
                            RectTransform psrt = (RectTransform)CreateButton(panel.transform, "Prospit", new Color(253f/255f, 229f/255f, 0f), new Color(186f/255f, 0f, 253f/255f), delegate {
                                CharacterOptionsGridComponent.SimpleExtendableEventArgs seea = new CharacterOptionsGridComponent.SimpleExtendableEventArgs {
                                    data = new Dictionary<string, InternalPatcher.Character.ModdedCharacterFeature>() {
                                {"BetterGenesis_LunarModule.LunarSway", new LunarSway("prospit")}
                            }
                                };
                                lastPicked = "prospit";
                                action(seea);
                                UnityEngine.Object.Destroy(panel);
                                swaypicker_opened = false;
                            });
                            psrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 80);
                            psrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80);
                            psrt.anchoredPosition = new Vector2(40, -16);
                            Texture2D btntex = new Texture2D(2, 2);
                            btntex.LoadImage(File.ReadAllBytes(Application.streamingAssetsPath + "/closebutton.png"));
                            RectTransform lvrt = (RectTransform)CreateButton(panel.transform, btntex, delegate {
                                UnityEngine.Object.Destroy(panel);
                                swaypicker_opened = false;
                            });
                            lvrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 32);
                            lvrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32);
                            lvrt.anchoredPosition = new Vector2(0, 40);
                            swaypicker_opened = true;
                        }
                    });
                    break;
            }
            return true;
        }
    }
}
