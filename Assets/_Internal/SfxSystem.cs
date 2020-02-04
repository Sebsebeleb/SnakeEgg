using System;
using Fabric;
using UnityEngine;
using UnityScript.Scripting.Pipeline;
using Component = Fabric.Component;

namespace _Internal
{
    public class SfxSystem : MonoBehaviour
    {
        private static Fabric.Component component;

        private void Awake()
        {
            component = GameObject.Find("[SFX] Tile Set").GetComponent<AudioComponent>();
            Debug.Log("Found component: " + component);
        }

        private const string evName = "SetNumber";
        public static void TriggerSetTile()
        {
            Debug.Log("Triggering tile set sfx");
            Fabric.EventManager.Instance.PostEvent(evName, EventAction.PlaySound);

            component.Pitch += 0.2f;
        }

        public static void ResetPitch()
        {
            component.Pitch = 1;
        }
    }
}