using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace WorkableMultipliersMod
{

	class WorkableMultipliersUtils
	{
		public static List<Type> WorkableSubTypes;
		public static List<Type> WorkableConfigTypes;


		static WorkableMultipliersUtils()
		{
			WorkableSubTypes = FindAllDerivedTypes(typeof(Workable));
			WorkableConfigTypes = new List<Type>();

			foreach (var workable in WorkableMultipliersConfig.Instance.Workables)
			{
				Type type = AccessTools.TypeByName(workable.Key);
				//Debug.Log("type: " + type);
				if (WorkableMultipliersUtils.WorkableSubTypes.Contains(type))
				{
					WorkableConfigTypes.Add(type);
				}
			}
		}

		public static float GetMultiplier(string name, float defaultValue, Workable instance)
		{
			try
			{
				if (WorkableMultipliersUtils.WorkableConfigTypes.Contains(instance.GetType()))
				{
					//Debug.Log("__instance.GetType().FullName: " + instance.GetType().FullName);
					var workable = WorkableMultipliersConfig.Instance.Workables[instance.GetType().FullName];
					//Debug.Log("workable: " + workable);
					//Debug.Log("workable[" + name + "]: " + workable[name]);
					if (workable.ContainsKey(name) && workable[name] >= 0f)
					{
						return defaultValue * workable[name];
					}
				}
				return defaultValue;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				return defaultValue;
			}
		}

		public static List<Type> FindAllDerivedTypes(Type type)
		{
            //https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
            var derivedTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
								from assemblyType in domainAssembly.GetTypes()
								where type.IsAssignableFrom(assemblyType)
								select assemblyType);
			return derivedTypes.ToList();
		}


	}

}