using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveSpawner))]
public class WaveSpawnerEditor : Editor {

    private WaveSpawner Spawner => (WaveSpawner)target;

    private void Draw() {
        Handles.color = Color.red;
        for (int i = 0; i < Spawner.spawnPoints.Count; i++) {
            var pos = Handles.FreeMoveHandle(Spawner.spawnPoints[i], Quaternion.identity, 1f, Vector3.zero, Handles.SphereHandleCap);
            if (pos != Spawner.spawnPoints[i]) {
                Undo.RecordObject(Spawner, "Move Spawnpoint");
                Spawner.spawnPoints[i] = pos;
            }
        }
    }

    private void Input() {
        var guiEvent = Event.current;
        var ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        var position = ray.origin - (ray.origin.y / ray.direction.y) * ray.direction;
        position.y = 0;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift) {
            Undo.RecordObject(Spawner, "Add Spawnpoint");
            Spawner.spawnPoints.Add(position);
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1) {
            float minDstToPoint = 1.75f;
            int index = -1;
            for (int i = 0; i < Spawner.spawnPoints.Count; i++) {
                var dist = HandleUtility.DistancePointLine(Spawner.spawnPoints[i], ray.origin, ray.GetPoint(10000f));
                if (dist < minDstToPoint) {
                    minDstToPoint = dist;
                    index = i;
                }
            }
            if (index != -1) {
                Undo.RecordObject(Spawner, "Remove Spawnpoint");
                Spawner.spawnPoints.RemoveAt(index);
            }
        }
    }

    private void OnSceneGUI() {
        Draw();
        Input();
    }
}
