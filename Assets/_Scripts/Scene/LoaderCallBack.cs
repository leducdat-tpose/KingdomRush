using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaderCallBack : DetMonobehaviour
{
    [SerializeField]
    private Slider _loadingSlideBar;
    private bool _isFirstUpdate = true;
    private void Start() {
        _loadingSlideBar.value = 0;
    }
    private void Update() {
        if(_isFirstUpdate)
        {
            _isFirstUpdate = false;
            Loader.OnLoaderCallBack();
            return;
        }
        _loadingSlideBar.value = Loader.GetLoadingProgress();
    }
}
