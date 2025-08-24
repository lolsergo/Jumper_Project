[System.Serializable]
public class SaveData
{
    public string profileName;
    public float maxDistanceReached;
    public float totalPlayTime;
    public int tries;

    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    public float uiVolume = 1f;

    public bool fullscreen = true;
    public int resolutionIndex = 0;

    public System.Collections.Generic.List<InputBindingOverrideData> inputBindings = new();

    [System.Serializable]
    public class InputBindingOverrideData
    {
        public string actionType;
        public int bindingIndex;
        public string overridePath;
    }

    public void SetOrReplaceBinding(string actionType, int bindingIndex, string overridePath)
    {
        var existing = inputBindings.Find(b => b.actionType == actionType && b.bindingIndex == bindingIndex);
        if (string.IsNullOrEmpty(overridePath))
        {
            if (existing != null) inputBindings.Remove(existing);
            return;
        }
        if (existing != null)
        {
            existing.overridePath = overridePath;
        }
        else
        {
            inputBindings.Add(new InputBindingOverrideData
            {
                actionType = actionType,
                bindingIndex = bindingIndex,
                overridePath = overridePath
            });
        }
    }

    public bool TryGetBinding(string actionType, int bindingIndex, out string path)
    {
        var existing = inputBindings.Find(b => b.actionType == actionType && b.bindingIndex == bindingIndex);
        if (existing != null)
        {
            path = existing.overridePath;
            return true;
        }
        path = null;
        return false;
    }
}