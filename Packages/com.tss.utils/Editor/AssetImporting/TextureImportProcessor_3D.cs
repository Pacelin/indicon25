using System;
using UnityEditor;

namespace TSS.Utils.AssetImporting
{
    public class TextureImportProcessor_3D : AssetPostprocessor
    {
        protected void OnPreprocessTexture()
        {
            var importer = assetImporter as TextureImporter;
            if (importer == null)
                return;
            if (importer.importSettingsMissing)
            {
                if (!assetPath.Contains("3D"))
                    return;
                
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                importer.crunchedCompression = true;
                importer.compressionQuality = 100;

                if (assetPath.Contains("_N"))
                {
                    importer.textureType = TextureImporterType.NormalMap;
                }
                else
                {
                    importer.textureType = TextureImporterType.Default;
                }
            }
        }
    }
}