using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGenesisProjectModPatcher;
using UnityEngine;

namespace BetterGenesis_LunarModule {
    public class LunarPillar : MonoBehaviour {
        public static Mesh pillarHeadMesh;
        public const float PillarHeight = 5f;
        public static int pillarProspit = 0;
        public static int pillarDerse = 0;

        public GameObject pillarHead;
        public GameObject pillarBall;
        public int linkedPlayerID;
        private bool prototyped;

        private bool isPillarErect = false;
        public override string ToString() {
            return $"LunarPillar:{{PlayerID:{linkedPlayerID},Prototyped:{(prototyped ? "true" : "false" )}}}";
        }
        static LunarPillar() {
            pillarHeadMesh = new Mesh();
            Vector3 TL = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 BL = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 TR = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 BR = new Vector3(0.5f, -0.5f, 0.5f);
            Vector3 T = new Vector3(0f, 0.5f, 0f);
            Vector3[] verts = new Vector3[] { TL, TR, BL, BR, T };
            int[] triangles = new int[] {
                    4,1,0,
                    4,0,2,
                    4,3,1,
                    4,2,3,
                };
            Vector2[] uvs = new Vector2[] {
                    new Vector2(0,0),
                    new Vector2(0,0),
                    new Vector2(0,0),
                    new Vector2(0,0),
                    new Vector2(1,1),
                };
            pillarHeadMesh.vertices = verts;
            pillarHeadMesh.triangles = triangles;
            pillarHeadMesh.uv = uvs;
            pillarHeadMesh.Optimize();
            pillarHeadMesh.RecalculateNormals();
        }
        public void Start() {
            
        }
        public void ErectPillar(GameObject planet, Vector3 overridePos) {
            if(!isPillarErect) {
                transform.SetParent(planet.transform, false);
                float planetscale = transform.parent.localScale.x;
                transform.localScale = new Vector3(1, PillarHeight*planetscale, 1) * 1/planetscale;
                transform.localPosition = (overridePos.y < 0) ? new Vector3((planet.name == "Derse_Plane" ? pillarDerse : pillarProspit) * 3 / planetscale, PillarHeight*0.5f, 0) : overridePos;
                gameObject.GetComponent<Renderer>().material.color = planet.name == "Derse_Plane" ? new Color(115f/255f, 46f/255f, 188f/255f) : new Color(235f/255f, 195f/255f, 12f/255f);

                pillarHead = new GameObject("Pillar_Pyramid");
                pillarHead.AddComponent<MeshFilter>().mesh = pillarHeadMesh;
                pillarHead.AddComponent<MeshRenderer>().material.color = planet.name == "Derse_Plane" ? new Color(115f/255f, 46f/255f, 188f/255f) : new Color(235f/255f, 195f/255f, 12f/255f);
                pillarHead.transform.SetParent(transform, false);
                pillarHead.transform.localPosition = new Vector3(0, 0.6f, 0);
                pillarHead.transform.localScale = new Vector3(1, 1/PillarHeight, 1);

                pillarBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                pillarBall.name = "Pillar_Ball";
                pillarBall.GetComponent<Renderer>().material.color = new Color(20f/255f, 20f/255f, 20f/255f);
                pillarBall.transform.SetParent(pillarHead.transform, false);
                pillarBall.transform.localPosition = new Vector3(0, 1.25f, 0);
                pillarBall.transform.localScale = new Vector3(1f, 1f, 1f);

                prototyped = false;
                isPillarErect = true;
                if(planet.name == "Derse_Plane") {
                    pillarDerse += 1;
                } else {
                    pillarProspit += 1;
                }
            }
        }
        public void ActivateBall() {
            prototyped = true;
            pillarBall.GetComponent<Renderer>().material.color = new Color(255f/255f, 255f/255f, 255f/255f);
        }
        public void DeactivateBall() {
            prototyped = false;
            pillarBall.GetComponent<Renderer>().material.color = new Color(20f/255f, 20f/255f, 20f/255f);
        }
        public void Update() {
        }
    }
}
