using UnityEngine;

[CreateAssetMenu(fileName = nameof(ScreenLibrary), menuName = "Game/Screen Library")]
public partial class ScreenLibrary : ScriptableObject
{
    [SerializeField] private ScreenData[] screenData;

    public GameObject GetInstanceForType(string screenType)
    {
        foreach (var item in screenData)
        {
            if(item.ScreenType == screenType)
            {
                return item.Prefab;
            }
        }

        Debug.LogError($"No screen found with type of {screenType}", this);
        return null;
    }
}
