using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    private static GameHandler instance; 

    private static int score;

    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;

    private void Awake() {
        instance = this;
        score = 0;
    }

    private void Start() {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20 , 20);
        snake.Setup(levelGrid);
        Debug.Log("setting snake on levelgrid.  snake = " + snake);
        levelGrid.Setup(snake);

        CMDebug.ButtonUI(Vector2.zero, "Reload Scene", () => {
            Loader.Load(Loader.Scene.GameScene);
        });
    }

    void Update() { }

    public static int GetScore() {
        return score;
    }

    public static void AddScore() {
        score += 100;
    }

    private GameObject createSpriteObject(Sprite sprite) {
        GameObject gameObject = new GameObject();
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        return gameObject;
    }


}

