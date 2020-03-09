/*using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KnownCard))]
public class KnownCardInspector : Editor
{
    public override void OnInspectorGUI()
    {
        KnownCard script = (KnownCard)target; 
        Object obj = EditorGUILayout.ObjectField("Card Data", (Object)script.CardData, typeof(ICardData), true);
        script.CardData = obj as ICardData;
        DrawDefaultInspector();
    }
}*/