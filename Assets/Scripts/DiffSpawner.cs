using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public DiffBehaviour ddPrefab;
    public int numDiffDots;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private float xAvailable;
    private float yAvailable;
    private int numRows;
    private int numColumns;
    private int maxDots;

    private Vector3[,] grid;

    private void Start()
    {
        // Start spawning items
        Vector3 zeros;
        zeros.x = 0f;
        zeros.y = 0f;
        zeros.z = 0f;
        transform.position = zeros;

        numRows = (int)Mathf.Sqrt((float)numDiffDots / 2f);
        numColumns = 2 * numRows;

        maxDots = numRows * numColumns;

        xAvailable = maxBounds.x - minBounds.x;
        yAvailable = maxBounds.y - minBounds.y;

        InitializeCoordianteGrid();
        SpawnDots();
    }

    private void InitializeCoordianteGrid()
    {
        grid = new Vector3[numRows,numColumns];
        Vector3 currentVector;
        currentVector.z = 0;
        for (int i = 0; i < numRows; i++)
        {
            currentVector.y = (yAvailable / numRows) * ((float)i + 0.5f) + minBounds.y;
            for (int j = 0; j < numColumns; j++)
            {
                currentVector.x = (xAvailable / numColumns) * ((float)j + 0.5f) + minBounds.x;
                grid[i, j] = currentVector;
            }
        }
    }

    private void SpawnDots()
    {
        Vector3 currentPosition;
        for (int i = 0; i < maxDots; i++)
        {
            currentPosition = grid[(i / numColumns), (i % numColumns)];
            DiffBehaviour newDiffDot = Instantiate(ddPrefab, currentPosition, Quaternion.identity);

            newDiffDot.SetSpawner(this);
        }
    }

}
