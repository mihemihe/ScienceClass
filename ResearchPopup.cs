using System;
using System.Collections;
using UnityEngine;
using KSP.UI.Screens;
using System.IO;

namespace ScienceClass
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class ScienceClass : MonoBehaviour
    {
        public Texture2D popupImage;
        private Rect popupRect;
        private bool showPopup = false;
        private string popupText = "";

        private void Start()
        {
            DontDestroyOnLoad(this);
            GameEvents.OnTechnologyResearched.Add(OnTechnologyResearched);
            Debug.Log("[ScienceClass] Mod initialized and event listener added.");

            popupImage = LoadImage("ScienceClass/science01.png");
            popupRect = new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300);
        }

        private void OnDestroy()
        {
            GameEvents.OnTechnologyResearched.Remove(OnTechnologyResearched);
            Debug.Log("[ScienceClass] Event listener removed.");
        }

        private void OnTechnologyResearched(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> data)
        {
            Debug.Log("[ScienceClass] OnTechnologyResearched triggered. Operation Result: " + data.target);
            Debug.Log("[ScienceClass] Researched Node ID: " + data.host.techID);
            if (data.target == RDTech.OperationResult.Successful)
            {
                Debug.Log("[ScienceClass] Researched Node ID: " + data.host.techID);
                Debug.Log("[ScienceClass] Research completed: " + data.host.title);
                popupText = $"Research completed: {data.host.title}";
                showPopup = true;
            }
        }

        private void OnGUI()
        {
            if (showPopup)
            {
                popupRect = GUILayout.Window(0, popupRect, PopupWindow, "Research Completed");
            }
        }

        private void PopupWindow(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.Label(popupText);

            if (popupImage != null)
            {
                Rect imageRect = new Rect(10, 50, popupRect.width - 20, popupRect.height - 90);
                GUI.DrawTexture(imageRect, popupImage, ScaleMode.ScaleToFit);
            }
            else
            {
                GUILayout.Label("Image not loaded.");
            }

            // Close button at the bottom
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close"))
            {
                showPopup = false;
            }
            GUILayout.EndVertical();

            // Classic close window button in the right top corner
            float closeButtonSize = 20;
            Rect closeButtonRect = new Rect(popupRect.width - closeButtonSize - 8, 8, closeButtonSize, closeButtonSize);
            if (GUI.Button(closeButtonRect, "X"))
            {
                showPopup = false;
            }

            GUI.DragWindow();
        }

        private Texture2D LoadImage(string relativeFilePath)
        {
            string fullPath = Path.Combine(KSPUtil.ApplicationRootPath, "GameData", relativeFilePath);
            Debug.Log("[ScienceClass] Full image path: " + fullPath);

            if (System.IO.File.Exists(fullPath))
            {
                Debug.Log("[ScienceClass] Image file found: " + fullPath);
                byte[] fileData = System.IO.File.ReadAllBytes(fullPath);
                Texture2D texture = new Texture2D(2, 2);
                if (ImageConversion.LoadImage(texture, fileData))
                {
                    Debug.Log("[ScienceClass] Image loaded successfully.");
                    return texture;
                }
                else
                {
                    Debug.LogError("[ScienceClass] Image failed to load.");
                }
            }
            Debug.LogError("[ScienceClass] Image file not found: " + fullPath);
            return null;
        }
    }
}
