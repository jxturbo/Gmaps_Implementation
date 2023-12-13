using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggles : MonoBehaviour
{
    public Toggle platformShiftToggle;
    public Toggle noGravityToggle;
    public Toggle stasisActiveToggle;
    public GravityStasis gravityStasis;
    public ZeroGravity zeroGravity;

    // Update is called once per frame
    void Update()
    {
        stasisActiveToggle.isOn = gravityStasis.stasisActive;
        noGravityToggle.isOn = zeroGravity.noGravity;
        platformShiftToggle.isOn = zeroGravity.platformShift;
    }
}
