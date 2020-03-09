using Assets.Scripts.Common.Notifications;
using Assets.Scripts.GameActions;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class NotificationEditorWindow : EditorWindow
{
    Rect menuBar;
    Rect upperPanel;
    Rect lowerPanel;
    Rect resizer;

    float menuBarHeight = 20f;
    float resizerHeight = 5f;

    float sizeRatio = 0.5f;
    bool isResizing;

    bool collapse = false;
    bool clearOnPlay = true;
    bool showSequence = true;

    Vector2 upperPanelScroll;
    Vector2 lowerPanelScroll;

    GUIStyle resizerStyle;
    GUIStyle boxStyle;
    GUIStyle textAreaStyle;

    Texture2D boxBgOdd;
    Texture2D boxBgEven;
    Texture2D boxBgSelected;
    Texture2D icon;

    List<SequenceLog> logs;
    SequenceLog selectedLog;
    SequenceLog currentSequence;
    SequenceLog rootSequence;

    [MenuItem("Window/Notification Viewer")]
    static void OpenWindow()
    {
        var window = GetWindow<NotificationEditorWindow>();
        window.titleContent = new GUIContent("Notification Viewer");
    }

    private void OnEnable()
    {
        resizerStyle = new GUIStyle();
        resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;

        boxStyle = new GUIStyle();
        boxStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

        boxBgOdd = EditorGUIUtility.Load("builtin skins/darkskin/images/cn entrybackodd.png") as Texture2D;
        boxBgEven = EditorGUIUtility.Load("builtin skins/darkskin/images/cnentrybackeven.png") as Texture2D;
        boxBgSelected = EditorGUIUtility.Load("builtin skins/darkskin/images/menuitemhover.png") as Texture2D;

        textAreaStyle = new GUIStyle();
        textAreaStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
        textAreaStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/projectbrowsericonareabg.png") as Texture2D;

        logs = new List<SequenceLog>();
        selectedLog = null;
        currentSequence = null;
        rootSequence = null;

        this.AddObserver(OnBeginSequence, ActionSystem.beginSequenceNotification);
        this.AddObserver(OnEndSequence, ActionSystem.endSequenceNotification);
        this.AddObserver(OnCompleteSequence, ActionSystem.completeNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnBeginSequence, ActionSystem.beginSequenceNotification);
        this.RemoveObserver(OnEndSequence, ActionSystem.endSequenceNotification);
        this.RemoveObserver(OnCompleteSequence, ActionSystem.completeNotification);
    }

    #region GUI drawer

    private void OnGUI()
    {
        DrawMenuBar();
        DrawUpperPanel();
        DrawLowerPanel();
        DrawResizer();

        ProcessEvent(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Clear"), EditorStyles.toolbarButton, GUILayout.Width(35)))
            logs.Clear();
        GUILayout.Space(5);

        collapse = GUILayout.Toggle(collapse, new GUIContent("Collapse"), EditorStyles.toolbarButton, GUILayout.Width(50));
        clearOnPlay = GUILayout.Toggle(clearOnPlay, new GUIContent("Clear On Play"), EditorStyles.toolbarButton, GUILayout.Width(80));
        showSequence = GUILayout.Toggle(showSequence, new GUIContent("Show Sequences"), EditorStyles.toolbarButton, GUILayout.Width(90));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawUpperPanel()
    {
        upperPanel = new Rect(0, menuBarHeight, position.width, position.height * sizeRatio - menuBarHeight);

        GUILayout.BeginArea(upperPanel);
        upperPanelScroll = GUILayout.BeginScrollView(upperPanelScroll);

        for(int i = 0; i < logs.Count; i++)
        {
            if (!collapse || logs[i].parent == null)
            {
                if (DrawBox(showSequence ? logs[i].FullInfo : logs[i].info, i % 2 == 0, logs[i].isSelected))
                {
                    if (selectedLog != null)
                    {
                        selectedLog.isSelected = false;
                    }

                    logs[i].isSelected = true;
                    selectedLog = logs[i];
                    GUI.changed = true;
                }
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void DrawLowerPanel()
    {
        lowerPanel = new Rect(0, position.height * sizeRatio + resizerHeight, position.width, position.height * (1 - sizeRatio) - resizerHeight);

        GUILayout.BeginArea(lowerPanel);
        lowerPanelScroll = GUILayout.BeginScrollView(lowerPanelScroll);

        if(selectedLog != null)
        {
            GUILayout.TextArea(selectedLog.info, textAreaStyle);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void DrawResizer()
    {
        resizer = new Rect(0, position.height * sizeRatio - resizerHeight, position.width, resizerHeight * 2);

        GUILayout.BeginArea(new Rect(resizer.position + (Vector2.up * 5f), new Vector2(position.width, 2)), resizerStyle);
        GUILayout.EndArea();

        EditorGUIUtility.AddCursorRect(resizer, MouseCursor.ResizeVertical);
    }
    
    private bool DrawBox(string content, bool isOdd, bool isSelected)
    {
        if (isSelected)
            boxStyle.normal.background = boxBgSelected;
        else if (isOdd)
            boxStyle.normal.background = boxBgOdd;
        else
            boxStyle.normal.background = boxBgEven;

        return GUILayout.Button(new GUIContent(content, icon), boxStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
    }

    private void ProcessEvent(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if(e.button == 0 && resizer.Contains(e.mousePosition))
                {
                    isResizing = true;
                }
                break;
            case EventType.MouseUp:
                isResizing = false;
                break;
        }

        Resize(e);
    }

    private void Resize(Event e)
    {
        if (isResizing)
        {
            sizeRatio = e.mousePosition.y / position.height;
            Repaint();
        }
    }

    #endregion

    #region Notification Observer

    private void OnBeginSequence(object sender, object args)
    {
        var gameAction = args as GameAction;
        var newSequence = new SequenceLog(false, true, gameAction, gameAction.GetType().Name);

        if (rootSequence == null)
            rootSequence = newSequence;
        else
            newSequence.parent = currentSequence;

        currentSequence = newSequence;
        logs.Add(newSequence);
    }

    private void OnEndSequence(object sender, object args)
    {
        var gameAction = args as GameAction;
        foreach(var log in logs)
        {
            if(log.action == gameAction)
            {
                log.isActive = false;
                currentSequence = log.parent;
            }
        }
    }

    private void OnCompleteSequence(object sender, object args)
    {
        rootSequence = null;
    }

    #endregion
}

public class SequenceLog
{
    public bool isSelected;
    public bool isActive;
    public GameAction action;
    public string info;
    public SequenceLog parent = null;

    public string Info => action.GetType().Name;

    public string FullInfo
    {
        get
        {
            if (parent == null) 
                return Info;
            else
                return parent.FullInfo + " => " + Info;
        }
    }

    public SequenceLog(bool isSelected, bool isActive, GameAction action, string info)
    {
        this.isSelected = isSelected;
        this.isActive = isActive;
        this.action = action;
        this.info = info;
    }
}

public static class ActionSystemExtension
{
    public static GameAction GetRootAction(this ActionSystem actionSystem) => GetInstanceField(typeof(GameAction), actionSystem, "rootAction") as GameAction;

    private static object GetInstanceField(Type type, object instance, string fieldName)
    {
        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            | BindingFlags.Static;
        FieldInfo field = type.GetField(fieldName, bindFlags);
        return field.GetValue(instance);
    }
}
