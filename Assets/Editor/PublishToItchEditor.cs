using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class PublishToItchEditor : MonoBehaviour
{

    [MenuItem("Publish/Itch.io &d")]
    static void Publish() {
        Process.Start("unitytoitch.sh");
    }

}
