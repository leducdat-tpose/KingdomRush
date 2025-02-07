using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private int _playerHeartRemain;
    [field:SerializeField]
    public SpawnEnemiesInfo EnemiesSceneInfo {get; private set;}

    [SerializeField]
    private LevelManager _levelManager;
    public LevelManager LevelManager=>_levelManager;
    [SerializeField]
    private ResourceManagement _resourceManagement;
    public ResourceManagement ResourceManagement=>_resourceManagement;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    private void Awake() {
        Instance = this;
        _levelManager = GetComponentInChildren<LevelManager>();
        _resourceManagement = GetComponentInChildren<ResourceManagement>();
        _enemySpawner = GetComponentInChildren<EnemySpawner>();
        _playerHeartRemain = EnemiesSceneInfo.InitialPlayerHeart;
    }

    public void LostPlayerHeart()
    => _playerHeartRemain -= EnemiesSceneInfo.PlayerHeartLost;
}
