using System.Collections;
using UnityEngine;

public class GunSwayAndRecoilBehaviour : MonoBehaviour
{
    [SerializeField] private Transform objectToLerpTowards;
    //[SerializeField] private AnimationCurve recoilCurve; 

    //Will be determined by player skill eventually;
    //[SerializeField] private float weaponSwayAmount;

    // Influenced by player skill too?
    private float _slerpTime;
    private float _timeElapsed;

    private Vector3 _swayOffset;
    private Vector3 _oldPosition;

    private IEnumerator _slerpRoutine;
    private Vector3 _recoilOffset;
    private float _weaponSwayAmount;


    // Start is called before the first frame update
    private void Start()
    {
        _slerpTime = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_timeElapsed < _slerpTime)
        {
            float t = _timeElapsed / _slerpTime;
        
            transform.position = Vector3.Slerp(_oldPosition, objectToLerpTowards.position + _swayOffset + _recoilOffset, t);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            //Debug.Log("Resetting");
            float xSway = Random.Range(-_weaponSwayAmount, _weaponSwayAmount);
            float ySway = Random.Range(-_weaponSwayAmount, _weaponSwayAmount);
            
            _oldPosition = transform.position;
            
            _swayOffset = new Vector3(xSway, ySway, 0);
            _recoilOffset = Vector3.zero;
            _slerpTime = 0.5f;
            _timeElapsed = 0;
        }
    }

    //TODO: Make this look less shit?
    //USE ANIM CURVEs
    public void AddRecoil(Vector3 recoilOffset)
    {
        _recoilOffset += recoilOffset;
        _slerpTime = 0.05f;
        _oldPosition = transform.position;
        _timeElapsed = 0;
    }

    public void SetSwayAmount(float weaponSwayAmount)
    {
        _weaponSwayAmount = weaponSwayAmount;
    }
}