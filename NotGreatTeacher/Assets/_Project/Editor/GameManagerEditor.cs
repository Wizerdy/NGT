using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToolsBoxEngine;

namespace Project {
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

        }

        public void OnSceneGUI() {
            serializedObject.Update();

            Handles.color = Color.red;

            SerializedProperty gridStartPosProp = serializedObject.FindProperty("gridStartPosition");
            SerializedProperty gridEndPosProp = serializedObject.FindProperty("gridEndPosition");

            gridStartPosProp.vector2Value = Handles.PositionHandle(gridStartPosProp.vector2Value.To3D(), Quaternion.identity).To2D();
            Handles.Label(gridStartPosProp.vector2Value.To3D(), "Start position");
            gridEndPosProp.vector2Value = Handles.PositionHandle(gridEndPosProp.vector2Value.To3D(), Quaternion.identity).To2D();
            Handles.Label(gridEndPosProp.vector2Value.To3D(), "End position");

            Vector2 startPos = gridStartPosProp.vector2Value;
            Vector2 endPos = gridEndPosProp.vector2Value;
            Vector2 size = (startPos - endPos).Abs();

            SerializedProperty gridSizeProp = serializedObject.FindProperty("gridSize");

            Handles.color = Color.green;

            for (int i = 0; i < gridSizeProp.vector2IntValue.x; i++) {
                Vector2 columns = new Vector2(size.x / (float)gridSizeProp.vector2IntValue.x, size.y);
                DrawHandlesRect(startPos + new Vector2(columns.x * i, 0), startPos + new Vector2(columns.x * (i + 1), columns.y));
            }

            for (int i = 0; i < gridSizeProp.vector2IntValue.y; i++) {
                Vector2 columns = new Vector2(size.x, size.y / (float)gridSizeProp.vector2IntValue.y);
                DrawHandlesRect(startPos + new Vector2(0, columns.y * i), startPos + new Vector2(columns.x, columns.y * (i + 1)));
            }

            //Gizmos.color = Color.red;
            //Gizmos.DrawCube((startPos + endPos) / 2f, (startPos - endPos).Abs());

            //script.patrolPoints[i] = Handles.PositionHandle(script.patrolPoints[i].ConvertTo3D(), Quaternion.identity).ConvertTo2D();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHandlesRect(Vector2 startPosition, Vector2 endPosition) {
            Vector3[] pos = new Vector3[] {
            startPosition.To3D(),
            new Vector3(startPosition.x, endPosition.y, 0),
            endPosition.To3D(),
            new Vector3(endPosition.x, startPosition.y, 0),
            startPosition.To3D(),
        };

            Handles.DrawAAPolyLine(pos);
        }
    }
}