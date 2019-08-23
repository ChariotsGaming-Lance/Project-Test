using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private HeadBomb _headBomb;
    private HeadBomb headBomb
    {
        get
        {
            if (null == _headBomb)
                _headBomb = GetComponentInChildren<HeadBomb>();
            return _headBomb;
        }
    }

    public HeadBomb GetHeadBomb()
    {
        return headBomb;
    }
}
