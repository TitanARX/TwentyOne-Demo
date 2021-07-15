using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICameraFX
{
    Camera GetCamera { get; }

    void ShakeCamera();
}
