using UnityEngine;

public class ScreenData
{
    public string ScreenType { get { return screenType; } }
    [FieldDropDown(typeof(string), typeof(ScreenType)), SerializeField]
    private string screenType;

    public GameObject Prefab { get { return prefab; } }
    [SerializeField] private GameObject prefab; //TODO make weak resource so less data is loaded.
}