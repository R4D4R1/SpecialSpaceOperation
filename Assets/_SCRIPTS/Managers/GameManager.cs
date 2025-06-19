using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTime;
    [SerializeField] private float startTime;

    [SerializeField] private GameObject obstaclesParent;
    [SerializeField] private List<Obstacle> obstacles;

    public int _asteroidAmountShot { get; private set; } = 0;
    public Action<int> OnAsteroidDestroyedAmountChanged;
    public bool GameOver { get; private set; } = false;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = 60;
        }
    }

    private void Start()
    {
        for (int i = 0; i < obstaclesParent.transform.childCount; i++)
        {
            obstacles.Add(obstaclesParent.transform.GetChild(i).GetComponent<Obstacle>());
        }

        foreach (Obstacle item in obstacles)
        {
            item.OnAsteroidDestroyed += AsteroidDestroyed;
        }
    }

    private void OnEnable()
    {
        foreach (Obstacle item in obstacles)
        {
            item.OnAsteroidDestroyed += AsteroidDestroyed;
        }
    }

    private void OnDisable()
    {
        foreach (Obstacle item in obstacles)
        {
            item.OnAsteroidDestroyed -= AsteroidDestroyed;
        }
    }

    private void AsteroidDestroyed()
    {
        _asteroidAmountShot++;
        OnAsteroidDestroyedAmountChanged?.Invoke(_asteroidAmountShot);
    }

    public void StartGameOver()
    {
        GameOverCoroutineAsync().Forget();
    }

    private async UniTaskVoid GameOverCoroutineAsync()
    {
        GameOver = true;
        SpawnerAsteroid.Instance.StopAllCoroutines();
        ShipShooting.Instance.DisableShooting();

        canvasGroup.DOFade(1, fadeTime);
        await UniTask.Delay(TimeSpan.FromSeconds(fadeTime));

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void StartGame()
    {
        StartGameCoroutineAsync().Forget();
    }

    private async UniTaskVoid StartGameCoroutineAsync()
    {
        ShipShooting.Instance.StartFly(startTime);
        await UniTask.Delay(TimeSpan.FromSeconds(startTime));
        SpawnerAsteroid.Instance.StartAsteroids();
        SpawnerHP.Instance.StartSpecial();
    }
}
