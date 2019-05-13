using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace WorkableEditorMod
{

	class WorkableEditorUtils
	{
		public static List<Type> WorkableSubTypes;
		public static List<Type> WorkableConfigTypes;


		static WorkableEditorUtils()
		{
			WorkableSubTypes = FindAllDerivedTypes(typeof(Workable));
			WorkableConfigTypes = new List<Type>();

			foreach (var workable in WorkableEditorConfig.Instance.Workables)
			{
				Type type = AccessTools.TypeByName(workable.Key);
				//Debug.Log("type: " + type);
				if (WorkableEditorUtils.WorkableSubTypes.Contains(type))
				{
					WorkableConfigTypes.Add(type);
				}
			}
		}

		public static float GetMultiplier(string name, float defaultValue, Workable instance)
		{

			if (WorkableEditorUtils.WorkableConfigTypes.Contains(instance.GetType()))
			{
				//Debug.Log("__instance.GetType().FullName: " + instance.GetType().FullName);
				var workable = WorkableEditorConfig.Instance.Workables[instance.GetType().FullName];
				//Debug.Log("workable: " + workable);
				//Debug.Log("workable[" + name + "]: " + workable[name]);
				if (workable.ContainsKey(name) && workable[name] > 0f)
				{
					return defaultValue*workable[name];
				}
			}
			return defaultValue;
		}

		public static List<Type> FindAllDerivedTypes(Type type)
		{
			var derivedTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
								from assemblyType in domainAssembly.GetTypes()
								where type.IsAssignableFrom(assemblyType)
								select assemblyType);
			return derivedTypes.ToList();
		}


	}

}