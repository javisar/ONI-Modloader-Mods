using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MiniCanvasMod
{
    [HarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
    internal class MiniCanvasMod_BuildingTemplates_CreateBuildingDef
    {
        /*
        public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
        {
            if (null == texture) return;

            string assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.textureType = TextureImporterType.Advanced;

                tImporter.isReadable = isReadable;

                AssetDatabase.ImportAsset(assetPath);
                AssetDatabase.Refresh();
            }
        }
       */
        private static bool Prefix(string id, ref int width, ref int height)
        {
            Debug.Log("=== MiniCanvasMod_BuildingTemplates_CreateBuildingDef Prefix ===" + id);
            if (id == "Canvas")
            {
                width = 1;
                height = 1;
            }
            return true;
        }

        /*
        private static void Postfix(string id, ref BuildingDef __result)
        {
            Debug.Log("=== MiniCanvasMod_BuildingTemplates_CreateBuildingDef Postfix ==="+id);
            if (id == "Canvas")
            {
               
                
                KAnimFile anim = __result.AnimFiles[0];
                List<Texture2D> nTextures = new List<Texture2D>();
                foreach (Texture2D texture in anim.textures)
                {
                    Texture2D nText = new Texture2D(texture.width, texture.height);

                    Texture2D nText2 = new Texture2D(texture.width, texture.height, texture.format, true);

                    Graphics.CopyTexture(texture, nText2);
                    nText.SetPixels(nText2.GetPixels());
                    nText.Apply();
                    bool result = nText.Resize(texture.width / 2, texture.height / 2);
                    nText.Apply();
                    
                    //MethodInfo mi = AccessTools.Method(typeof(Texture2D), "ResizeImpl");
                    //bool result = (bool)mi.Invoke(texture, new object[] { texture.width / 2, texture.height / 2 });
                    
                    Debug.Log("result: "+result);
                    nTextures.Add(nText);
                    // texture.width = texture.width / 2;
                    //texture.height = texture.height / 2;
                }
                anim.textures = nTextures;
                //__result.GenerateOffsets();
                
            }
        }
        */
    }
}
