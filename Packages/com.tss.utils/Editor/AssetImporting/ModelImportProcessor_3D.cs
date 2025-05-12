using UnityEditor;

namespace TSS.Utils.AssetImporting
{
    public class ModelImportProcessor_3D : AssetPostprocessor
    {
        private void OnPreprocessModel()
        {
            var importer = assetImporter as ModelImporter;
            if (importer == null)
                return;
            
            if (importer.importSettingsMissing)
            {
                if (!assetPath.Contains("3D"))
                    return;

                importer.useFileScale = false;
                importer.bakeAxisConversion = true;
                importer.importVisibility = false;
                importer.importCameras = false;
                importer.importLights = false;
                importer.animationType = ModelImporterAnimationType.None;
            }
        }
    }
}