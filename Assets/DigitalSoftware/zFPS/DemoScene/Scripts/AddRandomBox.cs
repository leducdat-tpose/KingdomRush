using UnityEngine;

namespace Assets.DigitalSoftware.zFPS.DemoScene.Scripts
{
    using UnityEngine.UI;

    public class AddRandomBox : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private GameObject _prefab;

        [SerializeField, Tooltip("Spacing in Ticks between instantiation of prefab"), Range(1, 30)] private int _spacingInTicks = 4;
        private int _spacingCounter = 0;


        // Update is called once per frame
        void FixedUpdate()
        {
            if (_toggle != null && _toggle.isOn)
            {
                _spacingCounter--;

                if (_spacingCounter <= 0)
                {
                    var gameObject = Instantiate(_prefab,
                        new Vector3(Random.Range(-12.5f, 12.5f), Random.Range(-25f, 0.5f), Random.Range(-5f, 12.5f)),
                        new Quaternion(Random.Range(-Mathf.PI, Mathf.PI), Random.Range(-Mathf.PI, Mathf.PI),
                            Random.Range(-Mathf.PI, Mathf.PI), Random.Range(-Mathf.PI, Mathf.PI)), null);

                    gameObject.SetActive(false);
                    gameObject.SetActive(true);

                    _spacingCounter = _spacingInTicks;
                }
            }
        }
    }
}
