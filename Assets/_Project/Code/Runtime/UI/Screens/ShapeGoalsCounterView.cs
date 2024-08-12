using System;
using System.Collections.Generic;
using Code.Runtime.Match3.Services;
using UnityEngine;

namespace Code.Runtime.UI.Screens
{
    public class ShapeGoalsCounterView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _container;
        
        private List<Match3ShapeGoalView> _goalViews = new List<Match3ShapeGoalView>();

        public void Initialize(List<Match3ShapeGoalView> views)
        {
            _goalViews = views;
            foreach (var view in _goalViews)
            {
                view.transform.SetParent(_container);
                view.transform.localScale = Vector3.one; // TODO fix later
            }
        }
        
        public void SetCount(Match3GoalData data) => 
            _goalViews.Find(g => g.type == data.shapeType)?.SetCount(data.count);

        public void Clear()
        {
            foreach (var goal in _goalViews)
                goal.ReturnToPool();
        }
    }
}