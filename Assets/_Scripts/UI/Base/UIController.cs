using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField]
    private Text _moneyAmount;
    [SerializeField]
    private Text _heartRemainAmount;
    [SerializeField]
    private Text _waveAmount;
    private EventBinding<ResourceManagement> _resourceEventBinding;
    private void Awake() {
        Instance = this;
    }
    public void Initialise()
    {
        UpdateMoneyAmount();
        UpdateHeartRemain();
        UpdateWave();
    }

    private void OnEnable() {
        _resourceEventBinding = new EventBinding<ResourceManagement>(UpdateMoneyAmount);
        EventBus<ResourceManagement>.Register(_resourceEventBinding);
    }

    private void OnDisable() {
        EventBus<ResourceManagement>.Deregister(_resourceEventBinding);
    }

    public void UpdateMoneyAmount() 
    => _moneyAmount.text = GameController.Instance.ResourceManagement.TotalMoney.ToString();

    public void UpdateHeartRemain()
    => _heartRemainAmount.text = GameController.Instance.PlayerHeartRemain.ToString();

    public void UpdateWave()
    {
        EnemySpawner enemySpawner = GameController.Instance.EnemySpawner;
        _waveAmount.text = $"Wave {enemySpawner.CurrentWave:D2}/{enemySpawner.TotalWaves:D2}";
    }
}
