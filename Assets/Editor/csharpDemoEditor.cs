using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Collections.Generic;
using System.IO;

public class U3dDemoEditor  {

	// Use this for initialization
	[MenuItem("Assets/MakeAB")]
    private static void MakeAB() {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
