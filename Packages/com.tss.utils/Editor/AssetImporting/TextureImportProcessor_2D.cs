using UnityEditor;
using UnityEngine;

namespace TSS.Utils.AssetImporting
{
    public class TextureImportProcessor_2D : AssetPostprocessor
    {
        protected void OnPreprocessTexture()
        {
            var importer = assetImporter as TextureImporter;
            if (importer == null)
                return;
            if (importer.importSettingsMissing)
            {
                if (!importer.assetPath.Contains("2D"))
                    return;
                
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                importer.crunchedCompression = true;
                importer.compressionQuality = 100;
                importer.textureType = TextureImporterType.Sprite;

                var textureSettings = new TextureImporterSettings();
                importer.ReadTextureSettings(textureSettings);
                textureSettings.spriteMode = (int) SpriteImportMode.Single;
                textureSettings.spriteMeshType = SpriteMeshType.FullRect;
                textureSettings.spriteGenerateFallbackPhysicsShape = false;
                importer.SetTextureSettings(textureSettings);
            }
        }
    }
}