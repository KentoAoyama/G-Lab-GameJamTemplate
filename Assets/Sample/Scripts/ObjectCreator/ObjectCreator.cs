using UnityEngine;

/// <summary>
/// GameObjectの生成を行うクラス
/// </summary>
public class ObjectCreator : MonoBehaviour
{
    [Header("生成したいオブジェクト")]
    [SerializeField]
    private GameObject[] _createObjects = default;

    [Header("オブジェクトの種類をランダムに生成するか")]
    [SerializeField]
    private bool _isRandom = false;

    [Header("自動で生成を行い続けるか")]
    [SerializeField]
    private bool _isAutoCreate = true;

    [Header("生成のインターバル")]
    [SerializeField]
    private float _createInterval = 1.0f;

    [Header("生成する個数制限")]
    [SerializeField]
    private bool _isCreateCountLimit = false;
    [SerializeField]
    private int _createCountNum = 10;

    [Header("以下、生成位置の指定に関するパラメーター")]
    [SerializeField]
    private CreatePositionType _createPositionType = CreatePositionType.RandomValue;

    [Header("Randomに生成する場合の範囲")]
    [SerializeField]
    private Vector3 _createRange = new(6, 0, 0);

    [Header("Transformで指定する場合の座標の設定")]
    [SerializeField]
    private Transform[] _createPositions = default;

    [Header("生成したオブジェクトの親オブジェクト")]
    [SerializeField]
    private Transform _parentObject = default;

    /// <summary>
    /// 生成時の座標指定形式の種類
    /// </summary>
    private enum CreatePositionType
    {
        RandomValue,
        RandomTransform,
    }


    //========システム用メンバー変数========

    // インターバル計測用
    private float _timeCount = 0.0f;
    // 生成したオブジェクトの個数カウント用
    private int _createCount = 0;
    // オブジェクトを生成するかどうか
    private bool _isCreate = false;


    private void Start()
    {
        if (_createObjects.Length == 0)
        {
            Debug.LogError("CreateParamSOにCreateObjectsが設定されていません");
            return;
        }

        // 生成するフラグを立てる
        if (_isAutoCreate)
        {
            _isCreate = true;
        }
    }

    private void Update()
    {
        // 生成するかどうかのフラグが立っていない場合は何もしない
        if (_isCreate == false)
        {
            return;
        }

        // インターバル時間を加算
        _timeCount += Time.deltaTime;

        // インターバル時間に達したらオブジェクトを生成
        if (_timeCount >= _createInterval)
        {
            CreateObject();
            _timeCount = 0.0f;
        }
    }

    private void CreateObject()
    {
        // 生成する個数制限があるかチェック
        if (_isCreateCountLimit)
        {
            if (_createCountNum <= 0)
            {
                return;
            }
            _createCountNum--;
        }

        // 生成時の座標指定形式に応じて座標を設定
        Vector3 createPos = Vector3.zero;
        switch (_createPositionType)
        {
            case CreatePositionType.RandomValue:
                // ランダムな範囲にオブジェクトを生成
                createPos = new Vector3(
                    Random.Range(-_createRange.x, _createRange.x),
                    Random.Range(-_createRange.y, _createRange.y),
                    Random.Range(-_createRange.z, _createRange.z));
                break;
            case CreatePositionType.RandomTransform:
                // Transformで指定した座標にランダムにオブジェクトを生成
                int index = Random.Range(0, _createPositions.Length);
                createPos = new Vector3(
                    _createPositions[index].position.x,
                    _createPositions[index].position.y,
                    _createPositions[index].position.z);
                break;
        }
        createPos += transform.position;


        GameObject createObj = null;
        // 生成するオブジェクトをランダムにするかチェック
        if (_isRandom)
        {
            // CreateObjectの配列の中からランダムに選択
            int index = Random.Range(0, _createObjects.Length);
            createObj = _createObjects[index];
        }
        else
        {
            // CreateObjectの配列の中から順番に選択
            int index = _createCount % _createObjects.Length;
            createObj = _createObjects[index];

            // 生成したオブジェクトの個数カウントを加算
            _createCount++;
        }

        // オブジェクトを生成
        createObj = Instantiate(createObj, createPos, Quaternion.identity);

        if (_parentObject != null)
        {
            createObj.transform.SetParent(_parentObject);
        }

        // 自動で生成を行い続ける設定ではなかった場合、生成時にフラグを下げる
        if (_isAutoCreate == false)
        {
            _isCreate = false;
        }
    }

    /// <summary>
    /// 生成フラグを変更する関数
    /// </summary>
    public void SetCreateFlag(bool isCreate)
    {
        _isCreate = isCreate;
    }

    /// <summary>
    /// 生成インターバルを変更する関数
    /// </summary>
    public void SetCreateInterval(float interval)
    {
        _createInterval = interval;
    }
}
