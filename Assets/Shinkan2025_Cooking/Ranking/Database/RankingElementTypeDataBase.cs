using System;
using System.Collections.Generic;
using Ranking.Scripts.Interface;
using UnityEditor;
using UnityEngine;
using Shinkan2025_Cooking.Ranking.Scripts;

namespace Ranking.Scripts.DataBase
{
    /// <summary>
    /// ランキングに登録する要素の型データを格納するデータベース
    /// </summary>
    [CreateAssetMenu(menuName = "Ranking")]
    public class RankingElementTypeDataBase : ScriptableObject
    {
        public List<TypeData> typeData;
        [Serializable]
        public class TypeData
        {
            //クラス名
            [SerializeField,Tooltip("名前空間を含めてクラス名を記述してください")]
            public string className;
            //説明文
            [TextArea(1,3)]
            public string what_is_this;
        }
        /// <summary>
        /// ランキングデータオブジェクトを作成
        /// </summary>
        public RankingData CreateRankingData()
        {
            Dictionary<Type,object> dictionary = new Dictionary<Type, object>();
            
            foreach (var data in typeData)
            {
                //stringからTypeを取得
                Type type = Type.GetType(data.className);
                if(type == null)
                    Debug.LogError("キャストできない型クラスです");
                
                //もっともらしい型にリフレクション
                object obj = Activator.CreateInstance(type);
                
                if (dictionary.ContainsKey(type))
                    throw new Exception("指定する型が重複しています");

                dictionary.Add(type, obj);
            }

            return RankingData.GenerateRankingDataWithDictionary(dictionary);
        }
    }
    
    /// <summary>
    /// ランキングに登録する要素の型データをキーとしてobject型のインスタンスを保持するクラス
    /// </summary>
    [Serializable]
    public class RankingData
    {
        
        
        /// <summary>
        /// 型データとobject型のインスタンスを保持するDictionary
        /// </summary>
        private Dictionary<Type, object> _dataDictionary;
        
        /// <summary>
        /// クラス内からしか呼べないコンストラクタ
        /// </summary>
        private RankingData(Dictionary<Type, object> dictionary)
        {
            _dataDictionary = dictionary;
        }
        
        /// <summary>
        /// 初期の辞書データを指定してRankingDataクラスを作成する
        /// </summary>
        public static RankingData GenerateRankingDataWithDictionary(Dictionary<Type,object> dictionary)
        {
            return new RankingData(dictionary);
        }
        
        /// <summary>
        /// 辞書データなしでRankingDataクラスを作成する
        /// </summary>
        public static RankingData GenerateRankingDataWithoutDictionary()
        {
            return new RankingData(new Dictionary<Type, object>());
        }

        /// <summary>
        /// 指定した型のランキングデータを取得する
        /// </summary>
        /// <typeparam name="T">指定する型</typeparam>
        public T GetData<T>()
        where T : IRankingDataElement<T>
        {
            Type assignedType = typeof(T);
            object obj = _dataDictionary[assignedType];
            if (obj == null)
                throw new KeyNotFoundException("指定された型オブジェクトが見つかりません");

            T data = (T)obj;
            if (data == null)
                throw new InvalidCastException("無効なキャストです");

            return data;
        }
        /// <summary>
        /// 指定した型のランキングデータを更新する
        /// Dictionaryになければ新たに登録する
        /// </summary>
        /// <typeparam name="T">指定する型</typeparam>
        public void UpdateData<T>(T newData)
        where T : IRankingDataElement<T>
        {
            Type assignedType = typeof(T);

            if (_dataDictionary.ContainsKey(assignedType))
            {
                //データを更新
                _dataDictionary[assignedType] = newData;
            }
            else
            {
                //データを追加
                _dataDictionary.Add(assignedType,newData);
            }
        }
    }
}