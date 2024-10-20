using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrickEffect : MonoBehaviour
{
    public abstract void Effect(Collision2D ball);
}
