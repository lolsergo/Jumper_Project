using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void Load(SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
