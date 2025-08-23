using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Library")]
public class AudioLibrary : ScriptableObject
{
    [Serializable]
    public class Sound
    {
        public SoundID ID;
        public AudioClip Clip;
        [Range(0f, 1f)] public float BaseVolume = 1f;
        public AudioCategory Category = AudioCategory.SFX;
    }

    [Serializable]
    public class SoundGroup
    {
        public SoundGroupID GroupId = SoundGroupID.Footstep_Default;
        public AudioCategory Category = AudioCategory.SFX;
        [Range(0f, 1f)] public float VolumeMultiplier = 1f;
        public Sound[] Items;
    }

    public enum AudioCategory { SFX, Music, UI }

    [Header("Sound Groups")]
    public SoundGroup[] Groups;

    private Dictionary<SoundID, Sound> _byId;
    private Dictionary<SoundGroupID, SoundGroup> _byGroupId;

    private void OnEnable() => RebuildCache();

    public void RebuildCache()
    {
        _byId = new Dictionary<SoundID, Sound>();
        _byGroupId = new Dictionary<SoundGroupID, SoundGroup>();

        if (Groups == null) return;

        foreach (var group in Groups)
        {
            if (group == null) continue;
            _byGroupId[group.GroupId] = group;

            if (group.Items == null) continue;

            foreach (var s in group.Items)
            {
                if (s == null) continue;
                _byId[s.ID] = s;
            }
        }
    }

    public Sound GetSound(SoundID id)
    {
        if (_byId == null || _byId.Count == 0) RebuildCache();
        _byId.TryGetValue(id, out var s);
        return s;
    }

    public SoundGroup GetGroup(SoundGroupID groupId)
    {
        if (_byGroupId == null || _byGroupId.Count == 0) RebuildCache();
        _byGroupId.TryGetValue(groupId, out var g);
        return g;
    }

    public Sound[] GetGroupItems(SoundGroupID groupId) =>
        GetGroup(groupId)?.Items ?? Array.Empty<Sound>();

    public Sound GetRandomFromGroup(SoundGroupID groupId)
    {
        var items = GetGroupItems(groupId);
        if (items.Length == 0) return null;
        return items[UnityEngine.Random.Range(0, items.Length)];
    }

    public Sound GetFromGroupByIndex(SoundGroupID groupId, int index)
    {
        var items = GetGroupItems(groupId);
        if (items.Length == 0) return null;
        index = Mathf.Clamp(index, 0, items.Length - 1);
        return items[index];
    }

    public SoundID GetRandomIDFromGroup(SoundGroupID groupId)
    {
        var s = GetRandomFromGroup(groupId);
        return s != null ? s.ID : default;
    }

    public SoundGroupID[] GetAllGroupIds()
    {
        if (_byGroupId == null || _byGroupId.Count == 0) RebuildCache();
        var arr = new SoundGroupID[_byGroupId.Count];
        _byGroupId.Keys.CopyTo(arr, 0);
        return arr;
    }
}