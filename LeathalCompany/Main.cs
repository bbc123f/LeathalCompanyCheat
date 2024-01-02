using UnityEngine;
using GameNetcodeStuff;
using BepInEx;
using HarmonyLib;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace LeathalCompany
{
    [BepInPlugin("com.KMAN.LeathalCompany", "Leathal", "1.1.1")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            new Harmony("faggots").PatchAll();
        }
        bool toggle = false;
        bool speedB = false;
        bool esp = false;
        bool TPGun = false;
        bool objesp = false;
        bool Stamina = false;

        bool open = true;
        bool canopen = false;

        float deltaTime;
        public static float fov = 66;
        float inputfov = 66;

        GameObject Pointer = null;

        Rect newRect = new Rect(10, 10, 300, 500);

        Texture2D button = new Texture2D(1, 1);
        Texture2D buttonhovered = new Texture2D(1, 1);
        Texture2D buttonactive = new Texture2D(1, 1);

        Texture2D windowbackground = new Texture2D(1, 1);
        Texture2D windowbackgroundhover = new Texture2D(1, 1);

        public void OnGUI()
        {
            button.SetPixel(0, 0, new Color32(23, 23, 23, 255));
            button.Apply();
            buttonhovered.SetPixel(0, 0, new Color32(29, 29, 29, 255));
            buttonhovered.Apply();
            buttonactive.SetPixel(0, 0, new Color32(35, 35, 35, 255));
            buttonactive.Apply();

            GUI.skin.button.onNormal.background = button;
            GUI.skin.button.onHover.background = buttonhovered;
            GUI.skin.button.onActive.background = buttonactive;
            GUI.skin.button.normal.background = button;
            GUI.skin.button.hover.background = buttonhovered;
            GUI.skin.button.active.background = buttonactive;

            windowbackground.SetPixel(0, 0, new Color32(23, 23, 23, 255));
            windowbackground.Apply();
            windowbackgroundhover.SetPixel(0, 0, new Color32(23, 23, 23, 255));
            windowbackgroundhover.Apply();

            GUI.skin.window.onNormal.background = windowbackground;
            GUI.skin.window.onHover.background = windowbackgroundhover;
            GUI.skin.window.onActive.background = windowbackground;
            GUI.skin.window.onNormal.textColor = Color.gray * 1.8f;
            GUI.skin.window.onHover.textColor = Color.white;
            GUI.skin.window.onActive.textColor = Color.white;
            GUI.skin.window.normal.background = windowbackground;
            GUI.skin.window.hover.background = windowbackgroundhover;
            GUI.skin.window.active.background = windowbackground;
            GUI.skin.window.normal.textColor = Color.gray * 1.8f;
            GUI.skin.window.hover.textColor = Color.white;
            GUI.skin.window.active.textColor = Color.white;

            GUI.skin.button.richText = true;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;


            if (UnityInput.Current.GetKeyDown(KeyCode.RightShift))
            {
                if (canopen)
                {
                    open = !open;
                    canopen = false;
                }
            }
            else
            {
                canopen = true;
            }

            if (open)
            {
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
                newRect = GUI.Window(0, newRect, new GUI.WindowFunction(newWindow), $"Steal UI - By KMAN [FPS: {1 / deltaTime}]");
                GUI.backgroundColor = Color.white;
            }
        }
        bool player = false;
        bool visual = false;
        bool room = false;
        bool misc = false;
        public static Vector2[] scroll = new Vector2[100];
        public static Texture2D scrollView = new Texture2D(1, 1);
        public void newWindow(int id)
        {
            if (GUILayout.Button("<b>Player</b>"))
            {
                player = !player;
            }
            if (player)
            {
                string af212 = Stamina ? "Stop Inf Stamina" : "Start Inf Stamina";
                Color textcolor312 = Stamina ? Color.green : Color.white;
                GUI.skin.button.normal.textColor = textcolor312;
                GUI.skin.button.active.textColor = textcolor312;
                GUI.skin.button.hover.textColor = textcolor312;
                if (GUILayout.Button(af212))
                {
                    Stamina = !Stamina;
                }

                string af2 = speedB ? "Stop Speed Boost" : "Start Speed Boost";
                Color textcolor3 = speedB ? Color.green : Color.white;
                GUI.skin.button.normal.textColor = textcolor3;
                GUI.skin.button.active.textColor = textcolor3;
                GUI.skin.button.hover.textColor = textcolor3;
                if (GUILayout.Button(af2))
                {
                    speedB = !speedB;
                }
                GUI.skin.button.normal.textColor = Color.white;
                GUI.skin.button.active.textColor = Color.white;
                GUI.skin.button.hover.textColor = Color.white;

                if (GUILayout.Button("Heal Self"))
                {
                    GameNetworkManager.Instance.localPlayerController.DamagePlayerFromOtherClientServerRpc(-100, new Vector3(0, 0, 0), 0);
                }

                if (GUILayout.Button("Kill Self"))
                {
                    GameNetworkManager.Instance.localPlayerController.DamagePlayerFromOtherClientServerRpc(100, new Vector3(0, 0, 0), 0);
                }

                GUILayout.Label("FOV: " + inputfov);
                inputfov = GUILayout.HorizontalSlider(inputfov, 0, 220);
                if (GUILayout.Button("Set FOV"))
                {
                    fov = inputfov;
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("<b>Visual</b>"))
            {
                visual = !visual;
            }
            if (visual)
            {
                Color textcolor = toggle ? Color.green : Color.white;
                string monsteresp = toggle ? "Stop Monster ESP" : "Start Monster ESP";
                GUI.skin.button.normal.textColor = textcolor;
                GUI.skin.button.active.textColor = textcolor;
                GUI.skin.button.hover.textColor = textcolor;
                if (GUILayout.Button(monsteresp))
                {
                    toggle = !toggle;
                }

                string af = esp ? "Stop Player ESP" : "Start Player ESP";
                Color textcolor2 = esp ? Color.green : Color.white;
                GUI.skin.button.normal.textColor = textcolor2;
                GUI.skin.button.active.textColor = textcolor2;
                GUI.skin.button.hover.textColor = textcolor2;
                if (GUILayout.Button(af))
                {
                    esp = !esp;
                }

                string af213 = objesp ? "Stop Object ESP" : "Start Object ESP";
                Color textcol13or2 = objesp ? Color.green : Color.white;
                GUI.skin.button.normal.textColor = textcol13or2;
                GUI.skin.button.active.textColor = textcol13or2;
                GUI.skin.button.hover.textColor = textcol13or2;
                if (GUILayout.Button(af213))
                {
                    objesp = !objesp;
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("<b>Room</b>"))
            {
                room = !room;
            }
            if (room)
            {


                GUI.skin.button.normal.textColor = Color.white;
                GUI.skin.button.active.textColor = Color.white;
                GUI.skin.button.hover.textColor = Color.white;
                if (GUILayout.Button("Kill Others"))
                {
                    EnemyAI[] ps = FindObjectsOfType(typeof(EnemyAI)) as EnemyAI[];
                    for (int i = 0; i < ps.Length; i++)
                    {
                        EnemyAI player = ps[i];
                        player.KillEnemy(true);

                    }
                }

                if (GUILayout.Button("Heal Others"))
                {
                    PlayerControllerB[] ps = FindObjectsOfType(typeof(PlayerControllerB)) as PlayerControllerB[];
                    for (int i = 0; i < ps.Length; i++)
                    {
                        PlayerControllerB player = ps[i];
                        player.DamagePlayerFromOtherClientServerRpc(-100, new Vector3(0, 0, 0), 0);

                    }
                }

                if (GUILayout.Button("Kill All Enemies"))
                {
                    foreach (EnemyAI enemy in FindObjectsOfType(typeof(EnemyAI)) )
                    {
                        enemy.KillEnemy(true);
                    }
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("<b>Misc</b>"))
            {
                misc = !misc;
            }
            if (misc)
            {
                if (GUILayout.Button("Break Legs Sound"))
                {
                    GameNetworkManager.Instance.localPlayerController.BreakLegsSFXServerRpc();
                }
            }
            GUILayout.Space(10);

            if (GUILayout.Button("JOIN DISCORD"))
            {
                Process.Start("https://discord.gg/clients");
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public void FixedUpdate()
        {
            if (GameNetworkManager.Instance != null)
            {
                if (GameNetworkManager.Instance.localPlayerController != null && GameNetworkManager.Instance.localPlayerController.gameplayCamera != null)
                {
                    GameNetworkManager.Instance.localPlayerController.targetFOV = fov;
                    //inputfovGameNetworkManager.Instance.localPlayerController.gameplayCamera.fieldOfView = fov;
                }
                if (toggle)
                {
                    foreach (EnemyAI enemy in FindObjectsOfType(typeof(EnemyAI)))
                    {
                        if (enemy != null)
                        {
                            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            gameObject.transform.position = enemy.serverPosition;
                            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            gameObject.GetComponent<Renderer>().material.color = Color.red;
                            gameObject.transform.localScale = Vector3.one;

                            GameObject textg = new GameObject();
                            textg.transform.LookAt(textg.transform.position + GameNetworkManager.Instance.localPlayerController.transform.rotation * Vector3.forward, GameNetworkManager.Instance.localPlayerController.transform.rotation * Vector3.up);
                            textg.transform.position = enemy.transform.position + new Vector3(0, 3f, 0);
                            var text = textg.AddComponent<TextMesh>();
                            text.fontSize = 10;
                            text.alignment = TextAlignment.Center;
                            text.anchor = TextAnchor.MiddleCenter;
                            text.text = enemy.enemyType.enemyName;

                            Destroy(textg, Time.fixedDeltaTime);
                            Destroy(gameObject, Time.smoothDeltaTime);
                        }
                    }
                }

                if (esp)
                {
                    foreach (PlayerControllerB player in FindObjectsOfType(typeof(PlayerControllerB)))
                    {
                        if (player != null && player.actualClientId != GameNetworkManager.Instance.localPlayerController.actualClientId)
                        {
                            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                            gameObject.transform.position = player.transform.position + new Vector3(0, 1.5f, 0);
                            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            gameObject.GetComponent<Renderer>().material.color = Color.green;
                            gameObject.transform.localScale = Vector3.one;
                            GameObject textg = new GameObject();
                            textg.transform.LookAt(textg.transform.position + GameNetworkManager.Instance.localPlayerController.transform.rotation * Vector3.forward, GameNetworkManager.Instance.localPlayerController.transform.rotation * Vector3.up);
                            textg.transform.position = player.transform.position + new Vector3(0, 3.5f, 0);
                            var text = textg.AddComponent<TextMesh>();
                            text.fontSize = 10;
                            text.alignment = TextAlignment.Center;
                            text.anchor = TextAnchor.MiddleCenter;
                            text.text = player.playerUsername;

                            Destroy(textg, Time.fixedDeltaTime);
                            Destroy(gameObject, Time.fixedDeltaTime);
                        }
                    }
                }

                if (speedB)
                {
                    float NSpeed = 10 * Time.deltaTime;
                    var Player = GameNetworkManager.Instance.localPlayerController;
                    Player.playerRigidbody.useGravity = false;
                    if (UnityInput.Current.GetKey(KeyCode.LeftShift) || UnityInput.Current.GetKey(KeyCode.RightShift))
                    {
                        NSpeed *= 10f;
                    }
                    if (UnityInput.Current.GetKey(KeyCode.LeftArrow) || UnityInput.Current.GetKey(KeyCode.A))
                    {
                        Player.transform.position += GameNetworkManager.Instance.localPlayerController.transform.right * -1f * NSpeed;
                    }
                    if (UnityInput.Current.GetKey(KeyCode.RightArrow) || UnityInput.Current.GetKey(KeyCode.D))
                    {
                        Player.transform.position += GameNetworkManager.Instance.localPlayerController.transform.right * NSpeed;
                    }
                    if (UnityInput.Current.GetKey(KeyCode.UpArrow) || UnityInput.Current.GetKey(KeyCode.W))
                    {
                        Player.transform.position += GameNetworkManager.Instance.localPlayerController.transform.forward * NSpeed;
                    }
                    if (UnityInput.Current.GetKey(KeyCode.DownArrow) || UnityInput.Current.GetKey(KeyCode.S))
                    {
                        Player.transform.position += GameNetworkManager.Instance.localPlayerController.transform.forward * -1f * NSpeed;
                    }
                }

                if (objesp)
                {
                    foreach (GrabbableObject gb in FindObjectsOfType(typeof(GrabbableObject)))
                    {
                        if (gb != null && !gb.isHeld)
                        {
                            if (gb.itemProperties.isScrap || gb.itemProperties.creditsWorth > 1)
                            {
                                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                gameObject.transform.position = gb.transform.position;
                                gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                                gameObject.GetComponent<Renderer>().material.color = Color.green;
                                gameObject.transform.localScale = Vector3.one;
                                GameObject textg = new GameObject();
                                textg.transform.LookAt(textg.transform.position + GameNetworkManager.Instance.localPlayerController.transform.rotation * Vector3.forward, GameNetworkManager.Instance.localPlayerController.transform.rotation * Vector3.up);
                                textg.transform.position = gb.transform.position + new Vector3(0, 1f, 0);
                                var text = textg.AddComponent<TextMesh>();
                                text.fontSize = 10;
                                text.alignment = TextAlignment.Center;
                                text.anchor = TextAnchor.MiddleCenter;
                                text.text = gb.itemProperties.itemName;

                                Destroy(textg, Time.fixedDeltaTime);
                                Destroy(gameObject, Time.fixedDeltaTime);
                            }
                        }
                    }
                }

                if (TPGun)
                {
                    RaycastHit raycastHit;
                    if (Physics.Raycast(GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.position + new Vector3(0, 2, 0), GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.forward, out raycastHit) && Pointer == null)
                    {
                        Pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        UnityEngine.Object.Destroy(Pointer.GetComponent<Rigidbody>());
                        UnityEngine.Object.Destroy(Pointer.GetComponent<SphereCollider>());
                        Pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        Pointer.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1f);
                        Pointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    }
                    Pointer.transform.position = raycastHit.point;
                    if (UnityInput.Current.GetMouseButton(1))
                    {
                        GameNetworkManager.Instance.localPlayerController.transform.position = raycastHit.point;
                    }
                }

                if (Stamina)
                {
                    GameNetworkManager.Instance.localPlayerController.sprintMeter = 1;
                }
            }
        }
    }
}