using UnityEngine;

public class DisableAfter : MonoBehaviour
{
    private float _timeElapsed;
    [SerializeField] private float timeToDisable;
    
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gameObject.activeSelf) return;
        
        if (_timeElapsed > timeToDisable)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _timeElapsed += Time.deltaTime;
        }
    }
}
