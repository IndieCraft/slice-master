using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPConfigBase : ScriptableObject
{
    private static VPConfigBase _instance;

    public static VPConfigBase instance
    {
        get
        {
            if (VPConfigBase._instance == null)
            {
                _instance = (Resources.Load("Config", typeof(VPConfigBase)) as VPConfigBase);
            }
            return _instance;
        }
    }

    public bool debug
    {
        get
        {
            return this.isDebug;
        }
    }

    [SerializeField]
    private bool isDebug;

    public int initialCoins;
}
