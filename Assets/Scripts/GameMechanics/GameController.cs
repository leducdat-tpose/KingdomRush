using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool isGameOver{get;private set;} = false;
    public int PlayerHeartRemain{get; private set;}
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
    public EnemySpawner EnemySpawner => _enemySpawner;
    private void Awake() {
        Instance = this;
        _levelManager = GetComponentInChildren<LevelManager>();
        _resourceManagement = GetComponentInChildren<ResourceManagement>();
        _enemySpawner = GetComponentInChildren<EnemySpawner>();
        PlayerHeartRemain = EnemiesSceneInfo.InitialPlayerHeart;
    }

    private void Start() {
        UIController.Instance.Initialise();
    }

    public void LostPlayerHeart()
    {
        if(isGameOver) return;
        PlayerHeartRemain -= EnemiesSceneInfo.PlayerHeartLost;
        if(PlayerHeartRemain <= 0)
        {
            isGameOver = true;
            PlayerHeartRemain = 0;
        }
        UIController.Instance.UpdateHeartRemain();
    }
}
