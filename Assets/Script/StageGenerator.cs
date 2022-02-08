using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    int currentChipIndex;//生成済みの最大インデックス

    public Transform character;//ターゲットキャラクターの指定　ね
    public GameObject[] stageChips;//ステージチッププレハブ配列
    public int startChipIndex;//自動生成開始インデックス
    public int preInstantiate;//生成先読み個数
    //生成済ステージチップ保持リスト
    public List<GameObject> generatedStageList = new List<GameObject>();

    private void Start()
    {
        //初期化処理
        currentChipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
    }

    private void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);
        //次のステージに入ったらステージの更新処理を行う
        if(charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //指定のIndexまでのステージチップを生成して管理下に置く
    void UpdateStage(int toChipIndex)
    {
        if (toChipIndex <= currentChipIndex)
        {
            return;
        }
        //指定のステージチップまでを作成
        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        //ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2)
        {
            DestroyOldestStage();

        }
        currentChipIndex = toChipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity
        );

        return stageObject;
    }

    //一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}

