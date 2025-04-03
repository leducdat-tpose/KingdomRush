using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : DetMonobehaviour
{
    [SerializeField]
    private Button _startGameBtn;
    protected override void LoadComponents()
    {
        _startGameBtn?.onClick.AddListener(() => {
            Loader.LoadScene(Loader.SceneType.GameScene);
        });
    }
}
