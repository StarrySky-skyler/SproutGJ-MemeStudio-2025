// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/06 23:02
// @version: 1.0
// @description:
// *****************************************************************************

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Editor.Tsuki
{
    [InitializeOnLoad]
    public class AutoAddObjOnStartup
    {
        // 自动添加对象
        static AutoAddObjOnStartup()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Debug.Log("播放状态改变");
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                Debug.Log("开始添加物体");
                AddObj();
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                Debug.Log("删除Managers物体");
                RemoveObj();
            }
        }

        private static void AddObj()
        {
            if (SceneManager.GetActiveScene().buildIndex < 3) return;
            Debug.Log("场景大于3，开始添加物体");
            if (GameObject.Find("Managers")) return;
            GameObject managers =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/Prefabs/Tsuki/Managers.prefab");
            if (!managers)
            {
                Debug.LogWarning("自动创建Managers失败，找不到物体");
                return;
            }

            // 创建物体
            GameObject obj =
                PrefabUtility.InstantiatePrefab(managers) as GameObject;
            obj.transform.position = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(obj, "Create Managers");
            Debug.Log("自动创建Managers成功");
        }

        private static void RemoveObj()
        {
            GameObject obj = GameObject.Find("Managers");
            if (!obj)
            {
                Debug.LogWarning("自动删除Managers失败，找不到物体");
                return;
            }
            Object.DestroyImmediate(obj);
            Debug.Log("自动删除Managers成功");
        }
    }
}
