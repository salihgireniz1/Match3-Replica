
using System;
using System.Collections.Generic;
using ModestTree;
using TMPro;
using UnityEngine;

namespace Match3
{
    public class GridSystem<T>
    {
        int width;
        int height;
        float cellSize;
        Vector3 origin;
        T[,] gridArray;
        CoordinateConverter coordinateConverter;
        public event Action<int, int, T> OnValueChanged;
        // public static GridSystem<T> VerticalGrid(int width, int height, float cellSize, Vector3 origin, bool debug = false)
        // {
        //     return new GridSystem<T>(width, height, cellSize, origin, new VerticalConverter(), debug);
        // }
        public static GridSystem<T> CreateVerticalInstance(Match3Data gridData)
        {
            return new GridSystem<T>(
                gridData.width,
                gridData.height,
                gridData.cellSize,
                gridData.originPosition,
                new VerticalConverter(),
                gridData.debug);
        }
        public GridSystem(int width, int height, float cellSize, Vector3 origin, CoordinateConverter coordinateConverter, bool debug)
        {
            this.width = width;
            this.height = height;
            this.origin = origin;
            this.cellSize = cellSize;
            gridArray = new T[width, height];
            this.coordinateConverter = coordinateConverter ?? new VerticalConverter();

            if (debug) DrawDebugLines();
        }
        public void SetValue(int x, int y, T value)
        {
            if (IsValid(x, y))
            {
                gridArray[x, y] = value;
                OnValueChanged?.Invoke(x, y, value);
            }
        }
        public T GetValue(int x, int y)
        {
            return IsValid(x, y) ? gridArray[x, y] : default;
        }
        public bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;
        public Vector2Int GetXY(T gridObject)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (gridArray[x, y].Equals(gridObject))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            // Return an invalid position if the object is not found
            return default;
        }
        public List<T> GetNeighbors2D(T gridObject)
        {
            Vector2Int indices = GetXY(gridObject);
            List<T> neighbors = new();
            int[] differences = new int[] { -1, 1 };

            foreach (int difference in differences)
            {
                if (IsValid(indices.x, indices.y + difference)) neighbors.Add(GetValue(indices.x, indices.y + difference));
                if (IsValid(indices.x + difference, indices.y)) neighbors.Add(GetValue(indices.x + difference, indices.y));
            }
            return neighbors;
        }
        public Vector2Int GetXY(Vector3 worldPosition) => coordinateConverter.WorldToGrid(worldPosition, cellSize, origin);
        public Vector3 GetWorldPositionCenter(int x, int y) => coordinateConverter.GridToWorldCenter(x, y, cellSize, origin);
        Vector3 GetWorldPosition(int x, int y) => coordinateConverter.GridToWorld(x, y, cellSize, origin);
        void DrawDebugLines()
        {
            const float duration = 100f;
            var parent = new GameObject("Debugging");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CreateWorldText(parent, x + "," + y, GetWorldPositionCenter(x, y), coordinateConverter.Forward);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, duration);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, duration);
        }
        TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir,
        int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = position;
            gameObject.transform.forward = dir;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color == default ? Color.white : color;
            textMeshPro.alignment = textAnchor;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }
    }
}