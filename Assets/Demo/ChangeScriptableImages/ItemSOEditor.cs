using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
 
 namespace Demo.ChangeScriptableImages
 {
     [CustomEditor(typeof(ScriptableLevel),true)]
     [CanEditMultipleObjects]
     public class ItemSoEditor : Editor
     {
         private ScriptableLevel Item => target as ScriptableLevel;

         public override Texture2D RenderStaticPreview(string assetPath,Object[] subAssets,int width,int height)
         {
             if (Item.icon == null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
             var t = GetType("UnityEditor.SpriteUtility");
             
             if (t == null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
             
             var method = t.GetMethod("RenderStaticPreview",new Type[] { typeof(Sprite),typeof(Color),typeof(int),typeof(int) });
             
             if (method == null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
             
             var renderTexture = method.Invoke("RenderStaticPreview",new object[] { Item.icon,Color.white,width,height });
             
             if(renderTexture is Texture2D texture2D) return texture2D;
             return base.RenderStaticPreview(assetPath,subAssets,width,height);
         }
 
         private static Type GetType(string typeName)
         {
             var type = Type.GetType(typeName);
         
             if(type!=null) return type;
 
             if(typeName.Contains("."))
             {
                 var assemblyName = typeName.Substring(0,typeName.IndexOf('.'));
                 var assembly = Assembly.Load(assemblyName);
                 if(assembly==null)
                     return null;
                 type=assembly.GetType(typeName);
                 if(type!=null)
                     return type;
             }
 
             var currentAssembly = Assembly.GetExecutingAssembly();
             var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
             foreach(var assemblyName in referencedAssemblies)
             {
                 var assembly = Assembly.Load(assemblyName);
                 if (assembly == null) continue;
             
                 type=assembly.GetType(typeName);
                 if(type!=null) return type;
             }
             return null;
         }
     }
 }