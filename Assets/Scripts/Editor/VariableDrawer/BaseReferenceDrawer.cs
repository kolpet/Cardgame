using UnityEditor;
using UnityEngine;

public abstract class BaseReferenceDrawer<T> : PropertyDrawer
{
    private readonly string[] popupOptions =
            { "Use Constant", "Use Variable" };

    private GUIStyle popupStyle;

    private int fieldHeight = 16;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        // Get properties
        SerializedProperty useConstant = property.FindPropertyRelative("useConstant");
        SerializedProperty constantValue = property.FindPropertyRelative("constant");
        SerializedProperty variable = property.FindPropertyRelative("reference");

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Calculate rect for configuration button
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.height = fieldHeight;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle);

        useConstant.boolValue = result == 0;

        position.height -= IsVariableValid(useConstant, variable) ? fieldHeight : 0;
        EditorGUI.PropertyField(position,
            useConstant.boolValue ? constantValue : variable,
            GUIContent.none);

        if (IsVariableValid(useConstant, variable))
        {
            position.y += fieldHeight;

            SerializedObject referenceObject = new SerializedObject(variable.objectReferenceValue);
            SerializedProperty referenceValue = referenceObject.FindProperty("value");
            EditorGUI.PropertyField(position, referenceValue, GUIContent.none);
        }

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty useConstant = property.FindPropertyRelative("useConstant");
        SerializedProperty variable = property.FindPropertyRelative("reference");
        return IsVariableValid(useConstant, variable) ? fieldHeight * 2 : fieldHeight;
    }

    private bool IsVariableValid(SerializedProperty useConstant, SerializedProperty variable)
    {
        return !useConstant.boolValue && variable.objectReferenceValue != null;
    }
}
