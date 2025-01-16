using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField]
    private LevelManager _levelManager;
    public LevelManager LevelManager=>_levelManager;
    [SerializeField]
    private MoneySystem _moneySystem;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    private void Awake() {
        Instance = this;
        _levelManager = GetComponentInChildren<LevelManager>();
        _moneySystem = GetComponentInChildren<MoneySystem>();
        _enemySpawner = GetComponentInChildren<EnemySpawner>();
    }
}
