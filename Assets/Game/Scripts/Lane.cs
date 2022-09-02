using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] List<LaneItem> items;//レーン内の回転する項目数
        [SerializeField] float laneSpeed;//レーンの回る速度
        [SerializeField] float itemSize = 80;
        RectTransform laneRectTransform;
        bool isButtonClick;
        public bool isStop { get; private set; }
        float stopPosition;
        public LaneItem stopItem { get; private set; }

        //初期化
        void Awake()
        {
            isButtonClick = false;
            isStop = false;

            //Update内でGetComponentを繰り返さないでいいように予め取得しておく
            laneRectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            //isStopがtrueの場合はレーン内のアイテムを回さない(最初にreturnしてその先を実行しない)
            if (isStop)
                return;

            //forループでレーン内の項目一つ一つを少しづつ動かし、一番下まで行くと一番上まで移動する
            for (int i = 0; i < items.Count; i++)
            {
                //リストから番号が一致する項目とそのRectTransformを取得する
                var item = items[i];

                //項目をスピードの数値だけマイナスして下に動かす(unityでは上が+で下が-)
                //transformのポジションを使用する時、親子関係を考慮するならlocalPositionを使用する
                item.transform.localPosition -= new Vector3(0, laneSpeed, 0);

                //項目がレーンの下まではみ出すとその項目を一番上まで移動する
                //下に行くほど-になるのでレーンの高さを判定時だけマイナスにするため laneRectTransform.sizeDelta.y ではなく -laneRectTransform.sizeDelta.y
                //intやfloatの先頭に-をつけるとその変数が一時的にマイナスになる
                if (-laneRectTransform.sizeDelta.y >= item.transform.localPosition.y)
                {
                    //現在の一番上の項目のlocalPositionを取得する
                    var topItemPosition = transform.GetChild(0).localPosition;
                    //取得したポジションを一番上に動かす項目の高さの分だけ+する
                    topItemPosition += new Vector3(0, itemSize, 0);
                    //計算したポジションを使用して項目を一番上に移動させる
                    item.transform.localPosition = topItemPosition;
                    //項目のオブジェクトの順番がレーン内の順番と一致するようにするため、一番上まで移動させるオブジェクトの順番も一番上にする
                    item.transform.SetSiblingIndex(0);
                }
            }

            if (isButtonClick)
            {
                //停止する位置までアイテムが移動したらisStopをtrueにして停止する
                if (-stopPosition >= stopItem.transform.localPosition.y)
                {
                    isStop = true;
                    Debug.Log("レーンを停止しました");

                    //レーンが停止したあとの処理↓

                }
            }
        }

        public void OnButtonClick()
        {
            //ボタンが押されたら停止する位置を計算して、停止する対象のアイテムを変数に入れておく
            stopPosition = (laneRectTransform.sizeDelta.y - itemSize) / 2;
            stopItem = transform.GetChild(0).GetComponent<LaneItem>();
            isButtonClick = true;

            //ボタンを押した直後の処理↓

        }

        public void OnLaneStart()
        {
            //レーンが再度回る処理が呼び出されるとこのレーン内の変数を初期化する(isStopとisButtonClickがfalseになるとレーンは勝手に回る)
            isStop = false;
            isButtonClick = false;
            stopItem = null;

            //レーンが再度回り始めたときの処理↓
        }
    }
}
