using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;

class CustomImportProcessor : AssetPostprocessor
{
    void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] names, System.Object[] values)
    {
        ModelImporter importer = (ModelImporter)assetImporter;
        var asset_name = Path.GetFileName(importer.assetPath);
        Debug.LogFormat("OnPostprocessGameObjectWithUserProperties(go = {0}) asset = {1}", go.name, asset_name);

        if (names.Length == 0)
        {
            Debug.Log("No user properties found.");
            return;
        }

        var variables = go.GetComponent<Variables>();
        if (variables == null)
        {
            variables = go.AddComponent<Variables>();
        }

        for (int i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var val = values[i];
            Debug.LogFormat("Processing Property : {0} : {1} : {2}", name, val.GetType().Name, val.ToString());
            
            if (val is Vector4)
            {
                // Convert Vector4 to Vector3
                val = (Vector3)(Vector4)val;
            }

            variables.declarations.Set(name, val);
        }
    }
}
