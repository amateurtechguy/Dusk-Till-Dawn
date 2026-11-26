using UnityEditor;
using UnityEngine;

public static class TreeColliderFix
{
    private const float TrunkRadiusFactor = 0.12f;
    private const float MinRadius = 0.15f;
    private const float MaxRadius = 1.5f;

    [MenuItem("Tools/Trees/Fix Terrain Tree Colliders")]
    public static void FixTerrainTrees()
    {
        Terrain[] terrains = Object.FindObjectsByType<Terrain>(FindObjectsSortMode.None);
        if (terrains.Length == 0)
        {
            Debug.LogWarning("No terrains found in the open scene.");
            return;
        }

        int prefabsFixed = 0;
        var done = new System.Collections.Generic.HashSet<string>();

        foreach (Terrain terrain in terrains)
        {
            TerrainCollider tc = terrain.GetComponent<TerrainCollider>();
            if (tc == null)
            {
                tc = terrain.gameObject.AddComponent<TerrainCollider>();
            }
            tc.terrainData = terrain.terrainData;
            SerializedObject tcSerialized = new SerializedObject(tc);
            SerializedProperty treeFlag = tcSerialized.FindProperty("m_EnableTreeColliders");
            if (treeFlag != null)
            {
                treeFlag.boolValue = true;
                tcSerialized.ApplyModifiedProperties();
            }

            foreach (TreePrototype proto in terrain.terrainData.treePrototypes)
            {
                if (proto.prefab == null) continue;
                string path = AssetDatabase.GetAssetPath(proto.prefab);
                if (string.IsNullOrEmpty(path) || !done.Add(path)) continue;

                GameObject root = PrefabUtility.LoadPrefabContents(path);
                bool changed = false;

                if (root.GetComponentInChildren<Collider>() == null)
                {
                    AddTrunkCollider(root);
                    changed = true;
                }
                else
                {
                    foreach (Collider c in root.GetComponentsInChildren<Collider>())
                    {
                        if (c.isTrigger)
                        {
                            c.isTrigger = false;
                            changed = true;
                        }
                    }
                }

                if (changed)
                {
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                    prefabsFixed++;
                }
                PrefabUtility.UnloadPrefabContents(root);
            }

            terrain.treeBillboardDistance = Mathf.Max(terrain.treeBillboardDistance, 100f);
            terrain.Flush();
            EditorUtility.SetDirty(terrain);
            EditorUtility.SetDirty(tc);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Terrain tree colliders enabled. Prefabs updated: " + prefabsFixed);
    }

    [MenuItem("Tools/Trees/Add Colliders To Selected Trees")]
    public static void AddToSelected()
    {
        GameObject[] selection = Selection.gameObjects;
        if (selection.Length == 0)
        {
            Debug.LogWarning("Select the parent object holding your trees, or the trees themselves.");
            return;
        }

        int count = 0;
        foreach (GameObject sel in selection)
        {
            Transform[] targets = sel.transform.childCount > 0 && sel.GetComponent<Renderer>() == null
                ? GetDirectChildren(sel.transform)
                : new[] { sel.transform };

            foreach (Transform t in targets)
            {
                if (t.GetComponent<Collider>() != null) continue;
                if (t.GetComponentInChildren<Renderer>() == null) continue;
                Undo.RegisterFullObjectHierarchyUndo(t.gameObject, "Add Tree Collider");
                AddTrunkCollider(t.gameObject);
                EditorUtility.SetDirty(t.gameObject);
                count++;
            }
        }

        Debug.Log("Colliders added to " + count + " trees.");
    }

    private static Transform[] GetDirectChildren(Transform parent)
    {
        var list = new Transform[parent.childCount];
        for (int i = 0; i < list.Length; i++) list[i] = parent.GetChild(i);
        return list;
    }

    private static void AddTrunkCollider(GameObject go)
    {
        Renderer[] renderers = GetMainRenderers(go);
        if (renderers.Length == 0) return;

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);

        Vector3 scale = go.transform.lossyScale;
        float sx = Mathf.Max(Mathf.Abs(scale.x), 0.0001f);
        float sy = Mathf.Max(Mathf.Abs(scale.y), 0.0001f);
        float sz = Mathf.Max(Mathf.Abs(scale.z), 0.0001f);

        Vector3 localCenter = go.transform.InverseTransformPoint(b.center);
        float radiusWorld = Mathf.Clamp(Mathf.Min(b.size.x, b.size.z) * TrunkRadiusFactor, MinRadius, MaxRadius);

        CapsuleCollider cc = go.AddComponent<CapsuleCollider>();
        cc.direction = 1;
        cc.radius = radiusWorld / Mathf.Max(sx, sz);
        cc.height = b.size.y / sy;
        cc.center = localCenter;
    }

    private static Renderer[] GetMainRenderers(GameObject go)
    {
        LODGroup lodGroup = go.GetComponentInChildren<LODGroup>();
        if (lodGroup != null)
        {
            LOD[] lods = lodGroup.GetLODs();
            if (lods.Length > 0 && lods[0].renderers != null && lods[0].renderers.Length > 0)
            {
                var valid = new System.Collections.Generic.List<Renderer>();
                foreach (Renderer r in lods[0].renderers)
                {
                    if (r != null) valid.Add(r);
                }
                if (valid.Count > 0) return valid.ToArray();
            }
        }
        return go.GetComponentsInChildren<Renderer>();
    }
}