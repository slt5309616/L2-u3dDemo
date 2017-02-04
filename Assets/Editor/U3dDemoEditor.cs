using UnityEngine;
using System.Collections;
using UnityEditor;

public class csharpDemoEditor  {

	// Use this for initialization
	[MenuItem("Assets/MakeAB")]
    private static void MakeAB() {
        var obj = Selection.activeObject;
        string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
        if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies)) {
            Debug.Log(obj.name + "资源打包成功");
        } else {
            Debug.Log(obj.name + "资源打包失败");
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
