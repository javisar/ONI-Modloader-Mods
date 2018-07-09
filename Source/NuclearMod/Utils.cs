using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace NuclearMod
{
    class Utils
    {

        public static void Print(object obj)
        {
            Debug.Log(obj.ToString() + "[" + obj.GetType() + "]");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                Debug.Log(name + "=" + value);
            }
        }

        public static Texture2D LoadTexture2DFromFile(string path, int width, int height)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture =
            new Texture2D(width, height, TextureFormat.RGB24, false) { filterMode = FilterMode.Trilinear };

            texture.LoadImage(bytes);

            return texture;
        }

        public static Type ExtendEnum(Type baseEnum, string name, int value)
        {

            // Get the current application domain for the current thread
            AppDomain currentDomain = AppDomain.CurrentDomain;

            // Create a dynamic assembly in the current application domain,
            // and allow it to be executed and saved to disk.
            AssemblyName assemblyName = new AssemblyName("Enums");
            AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName,
                                                  AssemblyBuilderAccess.RunAndSave);

            // Define a dynamic module in "MyEnums" assembly.
            // For a single-module assembly, the module has the same name as the assembly.
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name,
                                              assemblyName.Name + ".dll");

            // Define a public enumeration with the name "MyEnum" and an underlying type of Integer.
            EnumBuilder myEnum = moduleBuilder.DefineEnum("EnumeratedTypes.Enums",
                                     TypeAttributes.Public, typeof(int));


            Array values = Enum.GetValues(baseEnum);
            foreach (int row in values)
            {
                if (row == (int)value) continue;
                myEnum.DefineLiteral(Enum.GetName(baseEnum, row), row);
            }
            myEnum.DefineLiteral(name, (int)value);
            return myEnum.CreateType();
        }
    }
}

