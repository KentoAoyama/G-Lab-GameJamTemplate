using UnityEngine;

/// <summary>
/// オブジェクトの生成に関するパラメーターを保持するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "ObjectCreateParamSO", menuName = "ScriptableObject/ScenarioFileNameSO")]
public class ObjectCreateParamSO : ScriptableObject
{
    [Header("生成したいオブジェクト")]
    public GameObject[] CreateObjects = default;

    [Header("オブジェクトの種類をランダムに生成するか")]
    public bool IsRandom = false;

    [Header("自動で生成を行い続けるか")]
    public bool IsAutoCreate = true;

    [Header("生成のインターバル")]
    public float CreateInterval = 1.0f;

    [Header("生成する個数制限")]
    public bool IsCreateCountLimit = false;
    public int CreateCountNum = 10;
}
