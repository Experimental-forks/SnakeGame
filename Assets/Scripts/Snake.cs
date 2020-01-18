using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;

    public void Setup(LevelGrid levelGrid) {
        Debug.Log("snake: setting levelGrid");
        this.levelGrid = levelGrid;
    }

    private void Awake() {
        gridPosition = new Vector2Int(5, 5);
        gridMoveTimerMax = 0.5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);
    }

    void Start() {}

    void Update() {
        HandleInput();
        HandleGridMovement();
    }

    // don't allow reversing direction when moving
    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (gridMoveDirection.y != -1) {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (gridMoveDirection.y != 1) {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (gridMoveDirection.x != 1) {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (gridMoveDirection.x != -1) {
                gridMoveDirection.x = 1;
                gridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
        }
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

        levelGrid.SnakeMoved(gridPosition);
    }

    private float GetAngleFromVector(Vector2Int dir) {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n - 90;
    }

    public Vector2Int GetGridPosition() {
        return gridPosition;
    }

}
