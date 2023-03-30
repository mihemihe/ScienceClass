using System;
using System.Collections;
using UnityEngine;
using KSP.UI.Screens;

namespace ScienceClass
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class ScienceClass : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
            GameEvents.OnTechnologyResearched.Add(OnTechnologyResearched);
            Debug.Log("[ScienceClass] Mod initialized and event listener added.");
        }

        private void OnDestroy()
        {
            GameEvents.OnTechnologyResearched.Remove(OnTechnologyResearched);
            Debug.Log("[ScienceClass] Event listener removed.");
        }

        private void OnTechnologyResearched(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult> data)
        {
            Debug.Log("[ScienceClass] OnTechnologyResearched triggered. Operation Result: " + data.target);

            if (data.target == RDTech.OperationResult.Successful)
            {
                Debug.Log("[ScienceClass] Research completed!!!!!!!!!!!!!!!!!!!: " + data.host.title);
                StartCoroutine(ShowPopup(data.host.title));
            }
        }

        private IEnumerator ShowPopup(string researchName)
        {
            yield return new WaitForSeconds(1f);

            var message = new ScreenMessage($"Research completed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!: {researchName}", 5f, ScreenMessageStyle.UPPER_CENTER);
            ScreenMessages.PostScreenMessage(message);
        }
    }
}




