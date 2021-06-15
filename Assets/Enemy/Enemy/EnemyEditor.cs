using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        var enemyScript = target as Enemy;
        enemyScript.type = EditorGUILayout.IntField("Type", enemyScript.type);

        if (enemyScript.type != 3)
        {
            enemyScript.speed = EditorGUILayout.FloatField("Speed", enemyScript.speed);

            if (enemyScript.type == 2)
            {
                enemyScript.ammunition = EditorGUILayout.ObjectField("Ammunitions", enemyScript.ammunition, typeof(Ammunition), true) as Ammunition;
            }
        }

        else
        {
            enemyScript.speed = EditorGUILayout.FloatField("Horizontal speed", enemyScript.speed);

            enemyScript.verticalSpeed = EditorGUILayout.FloatField("Vertical Speed", enemyScript.verticalSpeed);
        }

        enemyScript.isAnEnemyPrototype = EditorGUILayout.Toggle("Is Prototype", enemyScript.isAnEnemyPrototype);

        enemyScript.scoreValue = EditorGUILayout.FloatField("Score Value", enemyScript.scoreValue);

        enemyScript.cooldownBeforeDestruction = EditorGUILayout.FloatField("Time Activated Before Death", enemyScript.cooldownBeforeDestruction);

        enemyScript.forceEjection = EditorGUILayout.FloatField("Force Ejection ", enemyScript.forceEjection);

        enemyScript.forceTorque = EditorGUILayout.FloatField("Force Torque ", enemyScript.forceTorque);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif