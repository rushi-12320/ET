﻿using System;
using UnityEditor;

namespace ET
{
    /// <summary>
    /// 代码文件导入变化记录
    /// (作用:可在大多数情况下避免IDE编译后按'F6'Unity再次触发编译)
    /// </summary>
    public class OnPostprocessScript : AssetPostprocessor
    {
        /// <summary>
        /// IDE编译后给开发人员返回Unity进行热重载的最大反应时间
        /// </summary>
        const long MaxIDEComiledResponeTime = 6000;

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] f, bool d)
        {
            if (!HasScriptChanged(importedAssets) && !HasScriptChanged(deletedAssets) && !HasScriptChanged(movedAssets))
                return;

            // IDE编译时间离现在少于6秒视为已进行最新编译
            long nowTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (nowTimestamp - AssemblyTool.IDECompileTime <= MaxIDEComiledResponeTime)
                return;

            AssemblyTool.ScriptVersion++;
        }

        /// <summary>
        /// 判断是否有代码文件变化
        /// </summary>
        static bool HasScriptChanged(string[] changedAssets)
        {
            foreach (string path in changedAssets)
            {
                if (path.EndsWith(".cs") || path.EndsWith(".asmdef") || path.EndsWith(".DISABLED") || path.Equals("Assets/Resources/GlobalConfig.asset"))
                    return true;
            }

            return false;
        }
    }
}