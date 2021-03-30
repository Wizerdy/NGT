using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

namespace Project {
    public class Grid {
        Cell[][] cells;
        Vector2 cellSize;
        Vector2 position;

        #region Constructors

        public Grid(Vector2 position, Vector2Int size, Vector2 cellSize) {
            cells = new Cell[size.x][];
            for (int x = 0; x < size.x; x++) {
                cells[x] = new Cell[size.y];
                for (int y = 0; y < size.y; y++) {
                    cells[x][y] = new Cell(x, y);
                }
            }

            this.position = position;
            this.cellSize = cellSize;
        }

        public Grid(Vector2 startPosition, Vector2 endPosition, Vector2Int size)
            : this(startPosition, size, (startPosition - endPosition).Abs() / (Vector2)size) { }

        #endregion

        #region Getters

        public Cell GetCell(int x, int y) {
            if (IsOutOfBounds(x, y)) { return null; }

            return cells[x][y];
        }

        public Cell GetCell(Vector2Int coord) {
            return GetCell(coord.x, coord.y);
        }

        public Vector2 GetCellPosition(Vector2Int coord) {
            return position + (Vector2)coord * cellSize + (cellSize / 2f);
        }

        public Vector2 GetCellPosition(int x, int y) {
            return GetCellPosition(new Vector2Int(x, y));
        }

        public Nullable<Vector2Int> GetCellCoord(Vector2 position) {
            position = position - this.position;
            if (position.x < 0 || position.y < 0) {
                return null;
            }

            Vector2Int coord = (position / cellSize).FloorToInt();

            if (IsOutOfBounds(coord)) {
                return null;
            }

            return coord;
        }

        public Nullable<Vector2Int> GetCellCoord(float x, float y) {
            return GetCellCoord(new Vector2(x, y));
        }

        public bool IsOutOfBounds(int x, int y) {
            if (x < 0 || x >= cells.Length || y < 0 || y >= cells[0].Length) {
                return true;
            }
            return false;
        }

        public bool IsOutOfBounds(Vector2Int coord) {
            return IsOutOfBounds(coord.x, coord.y);
        }

        public bool IsEmpty(int x, int y) {
            return cells[x][y].IsEmpty;
        }

        #endregion

        #region Setters

        public bool Fill(int x, int y, GameObject go, bool forced = false) {
            if (IsOutOfBounds(x, y)) { return false; }

            return cells[x][y].Fill(go, forced);
        }

        #endregion
    }

    public class Cell {
        public Nullable<Vector2Int> coordinates = null;
        public GameObject inside;

        public bool IsEmpty {
            get { return (inside == null ? true : false); }
        }

        public Cell(Vector2Int coordinates) {
            this.coordinates = coordinates;
            inside = null;
        }

        public Cell(int x, int y) : this(new Vector2Int(x, y)) { }

        public bool Fill(GameObject go, bool forced = false) {
            if (!IsEmpty && !forced) { return false; }

            inside = go;
            return true;
        }

        public void Clean() {
            if (IsEmpty) { return; }

            inside = null;
        }
    }
}