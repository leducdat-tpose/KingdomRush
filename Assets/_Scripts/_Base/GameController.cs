using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameController : MonoBehaviour, IEvent
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
    private EventBinding<GameController> _event;
    private void Awake() {
        if(Instance != null || Instance != this)
        {
            Destroy(Instance);
            Instance = this;
        }
        _levelManager = GetComponentInChildren<LevelManager>();
        _resourceManagement = GetComponentInChildren<ResourceManagement>();
        _enemySpawner = GetComponentInChildren<EnemySpawner>();
        PlayerHeartRemain = EnemiesSceneInfo.InitialPlayerHeart;
    }
    private void Start() {
        UIController.Instance.Initialise();
    }
    
    private void OnEnable() {
        _event = new EventBinding<GameController>(LostPlayerHeart);
        EventBus<GameController>.Register(_event);
    }
    private void OnDisable() {
        EventBus<GameController>.Deregister(_event);
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
    private void Update() {
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
