using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Harmony;

using BattleTech;

namespace BTDebug {
  [HarmonyPatch(typeof(UnityGameInstance), "Update")]
  public class UnityGameInstancePatch {
    static void Postfix(UnityGameInstance __instance) {
      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.I)) {
        InspectorManager.GetInstance().ToggleInspector();
      }

      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.A)) {
        InspectorManager.GetInstance().ToggleDebugMode();
      }

      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F)) {
        Main.Logger.LogDebug($"[BTDebug] Toggling Fog of War");
        FogOfWarManager.GetInstance().ToggleFogOfWar();
      }

      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.C)) {
        Main.Logger.LogDebug($"[BTDebug] Toggling Freeform Camera");
        CameraManager.GetInstance().ToggleFreeformCamera();
      }

      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.U)) {
        Main.Logger.LogDebug($"[BTDebug] Toggling User Interface");
        CameraManager.GetInstance().ToggleUi();
      }

      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.G)) {
        Main.Logger.LogDebug($"[BTDebug] Toggling Gizmos");
        GizmoManager.GetInstance().ToggleGizmos();
      }

      if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.R)) {
        Main.Logger.LogDebug($"[BTDebug] Toggling Gizmos Regions");
        GizmoManager.GetInstance().ToggleGizmoRegionMode();
      }
    }

    static void RecursivePrintGameObject(GameObject go, string indentation) {
      if (go.activeInHierarchy) {
        Main.Logger.LogDebug($"{indentation}- [GO] {go.name} - {go.tag} - {go.layer}");
        Component[] components = go.GetComponents<Component>();
        indentation += "  ";
        foreach (Component component in components) {
          if (component is MonoBehaviour) {
            MonoBehaviour mb = (MonoBehaviour)component;
            if (!mb.enabled) continue;
          }

          if (component is Collider) {
            Collider col = (Collider)component;
            if (!col.enabled) continue;
          }

          Main.Logger.LogDebug($"{indentation}- [Component] {component.GetType().Name}");
        }

        foreach (Transform t in go.transform) {
          RecursivePrintGameObject(t.gameObject, indentation);
          indentation.Remove(indentation.Length - 2);
        }
      }
    }
  }
}