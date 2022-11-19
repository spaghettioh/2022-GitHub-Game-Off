using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "ClumpScaleConfig", menuName = "Game Off/Clump Scale Config")]
public class ClumpScaleConfigSO : ScriptableObject
{
    [System.Serializable]
    public class ScaleConfig
    {
        public PropScaleCategory PropScale;
        public float ColliderRadius;
        public float Size;
        public float Torque;
    }

    [SerializeField] private List<ScaleConfig> _scaleConfigs;

    public ScaleConfig GetConfig(float size)
    {
        return _scaleConfigs.Find(s => s.Size == size);
    }

    public ScaleConfig GetConfig(PropScaleCategory scale)
    {
        return _scaleConfigs.Find(s => s.PropScale == scale);
    }
}
