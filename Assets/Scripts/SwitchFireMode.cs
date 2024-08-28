using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFireMode : MonoBehaviour
{
    private Gun _gunScript;

    // Start is called before the first frame update
    private void Start()
    {
        _gunScript = GetComponent<Gun>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetButtonDown(Constants.SwitchFireModeKey)) return;
        
        if (!ReferenceEquals(_gunScript, null))
        {
            _gunScript.CycleFireMode();
        }
    }
}
