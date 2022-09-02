using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] List<Lane> lanes;
        bool isStopLanes;

        void Awake()
        {
            isStopLanes = false;

            //全てのレーンのアンカーとピボットを正しく機能する数値に変更する
            foreach (var lane in lanes)
            {
                var rect = lane.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 1);
                rect.anchorMax = new Vector2(0.5f, 1);
                rect.pivot = new Vector2(0.5f, 1);
            }
        }

        void Update()
        {
            var isAllStop = true;
            foreach (var lane in lanes)
            {
                if (!lane.isStop)
                    isAllStop = false;
            }

            if (isAllStop && !isStopLanes)
            {
                StartCoroutine(OnAllStop());
                isStopLanes = true;
            }
        }

        //レーンが全て止まったときの処理
        IEnumerator OnAllStop()
        {
            Debug.Log("全てのレーンが停止しました");
            for (int i = 0; i < lanes.Count; i++)
            {
                var lane = lanes[i];
                Debug.Log("レーン " + i + ": " + lane.stopItem.GetID());
            }

            //三秒待つ
            yield return new WaitForSeconds(3);
            //三秒後レーンを全て回す
            AllLaneStart();

            // var lane1 = lanes[0];
            // var lane2 = lanes[1];
            // if (lane1.stopItem.GetID() == "lane_item_red" && lane2.stopItem.GetID() == "lane_item_gray")
            // {
            //     Debug.Log("StopLaneItem: Red & Gray");
            // }
        }

        //全てのレーンを再度回す処理(再度回したいタイミングで呼び出す)
        public void AllLaneStart()
        {
            Debug.Log("全てのレーンを回転しました");
            foreach(var lane in lanes)
            {
                lane.OnLaneStart();
            }
            isStopLanes = false;
        }
    }
}
