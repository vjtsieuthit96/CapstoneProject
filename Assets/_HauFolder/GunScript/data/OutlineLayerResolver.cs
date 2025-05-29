using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OutlineLayerResolver", menuName = "Scriptable Objects/OutlineLayerResolver")]
public class OutlineLayerResolver : ScriptableObject, IOutlineResolver
{
    [System.Serializable]
    public class TagLayerMapping
    {
        public string tag;
        public int layer;
    }

    [SerializeField] private List<TagLayerMapping> mappings;

    private Dictionary<string, int> _map;

    private void OnEnable()
    {
        _map = new Dictionary<string, int>();
        foreach (var mapping in mappings)
        {
            _map[mapping.tag] = mapping.layer;
        }
    }

    public bool TryGetOutlineLayer(string tag, out int outlineLayer)
    {
        return _map.TryGetValue(tag, out outlineLayer);
    }
}
