using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;

    private void Start() {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20 , 20);
        snake.Setup(levelGrid);
        Debug.Log("setting snake on levelgrid.  snake = " + snake);
        levelGrid.Setup(snake);
       
        //GameObject snakeHeadGameObject = createSpriteObject(GameAssets.i.snakeHeadSprite);
    }

    void Update() { }


    private GameObject createSpriteObject(Sprite sprite) {
        GameObject gameObject = new GameObject();
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        return gameObject;
    }
}

