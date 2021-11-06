using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

	public Vector2 mazeDimensions;
	public GameObject wallPrefab;
	public Camera mainCamera;

	Vector2 gridSize;

    void Start() {
		GameObject testWall = Instantiate(wallPrefab);
		Vector3 wallBounds = testWall.GetComponent<Renderer>().bounds.size;

		Destroy(testWall);

		Vector3 wallBoundsScreen = mainCamera.ScreenToWorldPoint(wallBounds);

		float totalGridWidth = Screen.width - (wallBoundsScreen.x * (mazeDimensions.x - 1));
		float totalGridHeight = Screen.height - (wallBoundsScreen.y * (mazeDimensions.y - 1));

		float gridWidth = totalGridWidth / mazeDimensions.x;
		float gridHeight = totalGridHeight / mazeDimensions.y;

		// Debug.Log("t.y: " + t.y);

		float wallYScale = gridHeight / wallBounds.y;
		Debug.Log("wallYScale: " + wallYScale);

		Debug.Log("screen width: " + Screen.width + " wallBounds.x: " + wallBounds.x + " mazeDim.x: " + mazeDimensions.x + " gridWidth: " + gridWidth);

		for (int y = 0; y < mazeDimensions.y; y++) {
			for (int x = 0; x < mazeDimensions.x - 1; x++) {

				float xPos = ((wallBounds.x + gridWidth) * x) + gridWidth;
				float yPos = (wallBounds.y + gridHeight) * y;

				Vector3 pos = mainCamera.ScreenToWorldPoint(new Vector3(xPos, yPos, 10f));
				
				// pos = GameObject.Find("Pacman").transform.position;

				GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
				// wall.transform.localScale = Vector3.Scale(wall.transform.localScale, new Vector3(0f, wallYScale, 0f));
			}
		}
    }

    void Update() {
            
    }
}
