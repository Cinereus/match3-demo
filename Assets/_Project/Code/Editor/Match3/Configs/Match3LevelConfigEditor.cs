using Code.Runtime.Infrastructure.StaticData;
using UnityEditor;
using UnityEngine;

namespace Code.Editor.Match3.Configs
{
    [CustomEditor(typeof(Match3LevelConfig))]
    public class Match3LevelConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Match3LevelConfig levelConfig = (Match3LevelConfig) target;

            if (GUILayout.Button("Save grid spawn position"))
            {
                var marker = GameObject.FindWithTag(EditorConstants.Tags.GRID_SPAWN_MARKER);
                if (marker != null)
                    levelConfig.gridSpawnPosition = marker.transform.position;
            }
            
            EditorUtility.SetDirty(target);
        }
    }
}
