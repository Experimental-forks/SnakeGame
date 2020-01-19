using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CodeMonkey;

public class LevelGrid  {
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private Snake snake;
    private int width;
    private int height;
    
    public LevelGrid(int width, int height) {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake) {
        this.snake = snake;
        SpawnFood();
    }

    private void SpawnFood() {

        int halfWidth = (int) (width / 2);
        int halfHeight = (int) (height / 2);
        do {
            foodGridPosition = new Vector2Int(Random.Range(0, width) - halfWidth, Random.Range(0, height) - halfHeight);
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);
        

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition) {
        if (snakeGridPosition == foodGridPosition) {
            Object.Destroy(foodGameObject);
            SpawnFood();
            //CMDebug.TextPopupMouse("Snake ate food");
            return true;
        }
        return false;
    }
}
