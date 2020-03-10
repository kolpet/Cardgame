using Assets.Scripts.Common;
using Assets.Scripts.Common.Animation;
using Assets.Scripts.Common.AspectContainer;
using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Common.Notifications;
using Assets.Scripts.Common.Pooling;
using Assets.Scripts.Common.Pooling.Poolers;
using Assets.Scripts.Components;
using Assets.Scripts.Enums;
using Assets.Scripts.GameActions;
using Assets.Scripts.Models.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageView : MonoBehaviour
{
    public SetPooler popupPooler;
    public float popupTime = 0.5f;
    public float popupDistance = 1f;

    public Color physicalColor;
    public Color magicalColor;
    public Color poisonColor;
    public Color trueColor;

    Dictionary<DamageType, Color> damageColor;

    private void OnEnable()
    {
        this.AddObserver(OnPrepareDamage, Global.PrepareNotification<DamageAction>());

        damageColor = new Dictionary<DamageType, Color>
        {
            { DamageType.Physical, physicalColor },
            { DamageType.Magical, magicalColor },
            { DamageType.Poison, poisonColor },
            { DamageType.True, trueColor },
        };
    }

    private void OnDisable()
    {
        this.AddObserver(OnPrepareDamage, Global.PrepareNotification<DamageAction>());
    }

    private void OnPrepareDamage(object sender, object args)
    {
        var action = args as DamageAction;
        action.Perform.viewer = DamageViewer;
    }
    private IEnumerator DamageViewer(IContainer game, GameAction action)
    {
        yield return true; //Get the damage number first
        var damageAction = action as DamageAction;
        var boardView = GetComponent<BoardView>();

        foreach(var target in damageAction.targets)
        {
            var targetCard = target as Card;
            var targetView = boardView.GetMatch(targetCard);
            var popup = CreateDamagePopup(targetView.transform, damageAction);

            targetView.GetComponent<BattlefieldCardView>().Refresh();

            Tweener tweener = popup.MoveTo(targetView.transform.position + Vector3.forward * popupDistance, 
                popupTime, EasingEquations.EaseOutBack);
            while (tweener != null) yield return null;

            popup.gameObject.SetActive(false);
            popup.transform.localScale = Vector3.one;
            var poolable = popup.GetComponent<Poolable>();
            popupPooler.Enqueue(poolable);
            yield return true;
        }
    }

    private Transform CreateDamagePopup(Transform target, DamageAction action)
    {
        var popup = popupPooler.Dequeue().GetComponent<PopupView>();
        var color = damageColor[action.type];
        popup.ChangeColor(color);
        var text = "-" + action.amount + " " + Enum.GetName(typeof(DamageType), action.type) + " damage";
        popup.SetText(text);

        popup.transform.ResetParent(popupPooler.transform);
        popup.transform.position = target.position;
        popup.transform.rotation = target.rotation;
        popup.transform.localScale = target.localScale;
        popup.gameObject.SetActive(true);

        return popup.transform;
    }
}
