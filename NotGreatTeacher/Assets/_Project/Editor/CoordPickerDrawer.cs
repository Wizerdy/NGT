using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Project {
    [CustomPropertyDrawer(typeof(CoordPicker))]
    public class CoordPickerDrawer : PropertyDrawer {
        private const float BOX_MARGINS_Y = 5f;
        private const float BOX_MARGINS_X = 10f;

        private enum ButtonType { NONE, ATTACK, CENTER, BOTH }

        private int actualType = 0;
        private Vector2Int gridSize = Vector2Int.one * 5;
        private Rect pos = Rect.zero;
        private GUIStyle informationText;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            // Initialization
            Color[] buttonsColors = new Color[] {
                GUI.backgroundColor,
                Color.cyan,
                Color.red,
                Color.magenta
            };

            informationText = new GUIStyle();
            informationText.normal.textColor = Color.grey;

            //property.serializedObject.Update();

            pos = position;
            pos.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty coordProp = property.FindPropertyRelative("coordinates");
            SerializedProperty centerProp = property.FindPropertyRelative("center");

            if (coordProp == null) { return; }

            // Set grid Size
            List<Vector2Int> coord = new List<Vector2Int>();
            for (int i = 0; i < coordProp.arraySize; i++) {
                coord.Add(coordProp.GetArrayElementAtIndex(i).vector2IntValue);

                if (coord[i].x >= gridSize.x) { gridSize.x = coord[i].x + 1; }
                if (coord[i].y >= gridSize.y) { gridSize.y = coord[i].y + 1; }
            }

            Vector2Int center = centerProp.vector2IntValue;

            if (center.x >= gridSize.x) { gridSize.x = center.x + 1; }
            if (center.y >= gridSize.y) { gridSize.y = center.y + 1; }

            // Grid size fields
            Rect box = pos;
            box.height = GetHeight();

            GUI.Box(box, GUIContent.none);

            EditorGUIUtility.labelWidth = 75f;

            AddHeight(BOX_MARGINS_Y);
            pos.width = box.width / 2f - BOX_MARGINS_X * 2f;
            pos.x += BOX_MARGINS_X;
            gridSize.x = EditorGUI.IntField(pos, "width", gridSize.x);
            pos.x += box.width / 2f + BOX_MARGINS_X / 2f;
            gridSize.y = EditorGUI.IntField(pos, "height", gridSize.y);

            AddHeight(EditorGUIUtility.singleLineHeight);
            AddHeight(EditorGUIUtility.singleLineHeight);

            // Grid box
            Rect grid = position;
            grid.y = pos.y;
            grid.width = EditorGUIUtility.singleLineHeight * gridSize.x;
            grid.height = EditorGUIUtility.singleLineHeight * gridSize.y;
            grid.x += position.width / 2f;
            grid.x -= grid.width / 2f;

            pos.width = EditorGUIUtility.singleLineHeight;

            pos.x = grid.x;
            GUI.Box(grid, GUIContent.none);

            // Grid
            int[][] tiles = new int[gridSize.x][];

            for (int x = 0; x < tiles.Length; x++) {
                tiles[x] = new int[gridSize.y];
                for (int y = 0; y < tiles[x].Length; y++) {
                    tiles[x][y] = 0;
                }
            }

            for (int i = 0; i < coord.Count; i++) {
                tiles[coord[i].x][coord[i].y] = 1;
            }

            if (center != Vector2Int.zero) {
                tiles[center.x][center.y] += 2;
            }

            Color storedColor = GUI.backgroundColor;

            GUI.backgroundColor = buttonsColors[actualType];

            if (GUI.Button(new Rect(
                position.x + position.width - pos.width * 2f,
                pos.y + (gridSize.y - 1) / 2f * pos.height,
                pos.width,
                pos.height
                ), " ")) {

                actualType++;
                actualType %= 3;
            }

            // Buttons
            for (int y = 0; y < gridSize.y; y++) {
                for (int x = 0; x < gridSize.x; x++) {
                    int oldTile = tiles[x][y];
                    GUI.backgroundColor = buttonsColors[tiles[x][y]];

                    if (GUI.Button(pos, " ")) {
                        if (tiles[x][y] + actualType == 3) {
                            tiles[x][y] = 3;
                        } else {
                            tiles[x][y] = actualType;
                        }

                        switch ((ButtonType)actualType) {
                            case ButtonType.NONE:
                                for (int i = 0; i < coord.Count; i++) {
                                    if (coord[i].x == x && coord[i].y == y) {
                                        coord.RemoveAt(i);
                                        coordProp.DeleteArrayElementAtIndex(i);
                                    }
                                }

                                if (oldTile == (int)ButtonType.CENTER) {
                                    center = Vector2Int.zero;
                                }
                                break;
                            case ButtonType.ATTACK:
                                AddCoord(ref coordProp, x, y);
                                break;
                            case ButtonType.CENTER:
                                center = new Vector2Int(x, y);
                                break;
                            case ButtonType.BOTH:
                                center = new Vector2Int(x, y);
                                AddCoord(ref coordProp, x, y);
                                break;
                            default:
                                break;
                        }

                        //Debug.Log("Actual : " + tiles[x][y] + " / Old : " + oldTile + " . Type : " + actualType +
                        //    " // Center : " + center);
                    }
                    pos.x += pos.width;
                }
                AddHeight(EditorGUIUtility.singleLineHeight);
                pos.x = grid.x;
            }

            centerProp.vector2IntValue = center;

            AddHeight(EditorGUIUtility.singleLineHeight /2f);
            pos.x = position.x + BOX_MARGINS_X;
            pos.width = position.width;
            EditorGUI.LabelField(pos, "Array size (" + coordProp.arraySize + ")", informationText);

            // End
            GUI.backgroundColor = storedColor;

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return GetHeight();
        }

        private void AddHeight(float height) {
            pos.y += height;
        }

        private float GetHeight() {
            return (EditorGUIUtility.singleLineHeight * (4 + gridSize.y));
        }

        private void AddCoord(ref SerializedProperty array, int x, int y) {
            array.InsertArrayElementAtIndex(array.arraySize);
            array.GetArrayElementAtIndex(array.arraySize - 1).vector2IntValue = new Vector2Int(x, y);
        }
    }
}