using CodeMonkey;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    private enum Direction {
        Left, Right, Up, Down
    }
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
    }

    private void Awake() {
        gridPosition = new Vector2Int(5, 5);
        gridMoveTimerMax = 0.3f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    void Start() { }

    void Update() {
        HandleInput();
        HandleGridMovement();
    }

    // don't allow reversing direction when moving
    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (gridMoveDirection != Direction.Down) {
                gridMoveDirection = Direction.Up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (gridMoveDirection != Direction.Up) {
                gridMoveDirection = Direction.Down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (gridMoveDirection != Direction.Right) {
                gridMoveDirection = Direction.Left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (gridMoveDirection != Direction.Left) {
                gridMoveDirection = Direction.Right;
            }
        }
    }

    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0) {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            snakeMovePositionList.Insert(0, new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection));

            gridPosition += getDirectionAsVector2Int();

            if (levelGrid.TrySnakeEatFood(gridPosition)) {
                CreateSnakeBody();
                snakeBodySize++;
            }

            if (snakeMovePositionList.Count > snakeBodySize + 1) {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(getDirectionAsVector2Int()));

            UpdateSnakeBodyParts();
        }
    }

    private Vector2Int getDirectionAsVector2Int() {
        Vector2Int d;
        switch (gridMoveDirection) {
            default:
            case Direction.Right: d = new Vector2Int(+1, 0); break;
            case Direction.Left: d = new Vector2Int(-1, 0); break;
            case Direction.Up: d = new Vector2Int(0, +1); break;
            case Direction.Down: d = new Vector2Int(0, -1); break;
        }
        return d;
    }

    private void CreateSnakeBody() {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts() {
        for (int i = 0; i < snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir) {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n - 90;
    }

    public Vector2Int GetGridPosition() {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList() {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition pos in snakeMovePositionList) {
            gridPositionList.Add(pos.GetGridPosition());
        }
        
        return gridPositionList;
    }

    private class SnakeBodyPart {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex) {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
            transform.position = new Vector3(-1000, -1000); // something offscreen
        }

        public void SetSnakeMovePosition(SnakeMovePosition movePos) {
            this.snakeMovePosition = movePos;
            Vector2Int pos = movePos.GetGridPosition();
            transform.position = new Vector3(pos.x, pos.y);
            float angle = snakeMovePosition.GetBodyAngle();
            transform.eulerAngles = new Vector3(0, 0, angle); ;
        }
    }


    private class SnakeMovePosition {

        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        // todo: make previousSnakeMovePosition be a Direction
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction) {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition() {
            return gridPosition;
        }

        public Direction GetDirection() {
            return direction;
        }

        public Direction GetPreviousDirection() {
            if (previousSnakeMovePosition == null) {
                return Direction.Right;
            }
            return previousSnakeMovePosition.direction;
        }

        public float GetBodyAngle() {
            Direction prevDir = GetPreviousDirection();
            float angle;
            switch (direction) {
                default:
                case Direction.Up:
                    angle = 0;
                    break;
                case Direction.Down:
                    angle = 180;
                    break;
                case Direction.Left:
                    angle = -90;
                    break;
                case Direction.Right:
                    angle = 90;
                    break;
            }
            
            if (direction != prevDir) {
                switch (prevDir) {
                    case Direction.Left:
                        if (direction == Direction.Up) angle += 45;
                        else angle -= 45;
                        break;
                    case Direction.Right:
                        if (direction == Direction.Down) angle += 45;
                        else angle -= 45;
                        break;
                    case Direction.Down:
                        if (direction == Direction.Left) angle += 45;
                        else angle -= 45;
                        break;
                    case Direction.Up:
                        if (direction == Direction.Right) angle += 45;
                        else angle -= 45;
                        break;
                }
            }

            return angle;
        }
    }
}
