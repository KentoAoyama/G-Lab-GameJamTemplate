using UnityEngine;

// GameObjectの生成を行うクラス
public class ObjectCreator : MonoBehaviour
{
    [Header("生成関連のパラメーター")]
    [SerializeField]
    ObjectCreateParamSO _createParam = default;

    [Header("以下、生成位置の指定に関するパラメーター")]
    [SerializeField]
    private CreatePositionType _createPositionType = CreatePositionType.Random;

    [Header("Randomに生成する場合の範囲")]
    [SerializeField]
    private Vector2 _createRange = new(10, 2);

    [Header("Transformで指定する場合の座標の設定")]
    [SerializeField]
    private Transform[] _createPositions = default;

    /// <summary>
    /// 生成時の座標指定形式の種類
    /// </summary>
    private enum CreatePositionType
    {
        Random,
        Transform,
    }


    //========計測用メンバー変数========

    // インターバル計測用
    private float _timeCount = 0.0f;
    // 生成したオブジェクトの個数カウント用
    private int _createCount = 0;


    //========他のクラスから触る用のプロパティ========

    // オブジェクトを生成するかどうか
    private bool _isCreate = false;
    /// <summary>
    /// 他のクラスから生成するかどうかのフラグを変更する用プロパティ
    /// </summary>
    public bool IsCreate => _isCreate;


    private void Start()
    {
        if (_createParam == null)
        {
            Debug.LogError("CreateParamSOが設定されていません");
            return;
        }

        if (_createParam.CreateObjects.Length == 0)
        {
            Debug.LogError("CreateParamSOにCreateObjectsが設定されていません");
            return;
        }

        // 生成するフラグを立てる
        if (_createParam.IsAutoCreate)
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
        if (_timeCount >= _createParam.CreateInterval)
        {
            CreateObject();
            _timeCount = 0.0f;
        }
    }

    private void CreateObject()
    {
        // 生成する個数制限があるかチェック
        if (_createParam.IsCreateCountLimit)
        {
            if (_createParam.CreateCountNum <= 0)
            {
                return;
            }
            _createParam.CreateCountNum--;
        }

        // 生成時の座標指定形式に応じて座標を設定
        Vector2 createPos = Vector2.zero;
        switch (_createPositionType)
        {
            case CreatePositionType.Random:
                // ランダムな範囲にオブジェクトを生成
                createPos = new Vector2(
                    Random.Range(-_createRange.x, _createRange.x),
                    Random.Range(-_createRange.y, _createRange.y));
                break;
            case CreatePositionType.Transform:
                // Transformで指定した座標にランダムにオブジェクトを生成
                int index = Random.Range(0, _createPositions.Length);
                createPos = new Vector2(
                    _createPositions[index].position.x,
                    _createPositions[index].position.y);
                break;
        }
        createPos += (Vector2)transform.position;


        GameObject createObj = null;
        // 生成するオブジェクトをランダムにするかチェック
        if (_createParam.IsRandom)
        {
            // CreateObjectの配列の中からランダムに選択
            int index = Random.Range(0, _createParam.CreateObjects.Length);
            createObj = _createParam.CreateObjects[index];
        }
        else
        {
            // CreateObjectの配列の中から順番に選択
            int index = _createCount % _createParam.CreateObjects.Length;
            createObj = _createParam.CreateObjects[index];
        }

        // オブジェクトを生成
        Instantiate(createObj, createPos, Quaternion.identity);

        // 自動で生成を行い続ける設定ではなかった場合、生成時にフラグを下げる
        if (_createParam.IsAutoCreate == false)
        {
            _isCreate = false;
        }
    }
}
