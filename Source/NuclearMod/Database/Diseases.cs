// Database.Diseases
using Klei.AI;

namespace Database
{
	public class ExtendedDiseases : Diseases
	{
        /*
		new public Disease Dweebcephaly;

		new public Disease Lazibonitis;

		new public Disease FoodPoisoning;

		new public Disease PutridOdour;

		new public Disease Spores;

		new public Disease ColdBrain;

		new public Disease HeatRash;

		new public Disease SlimeLung;

        new public Disease Sunburn;
        */
        public Disease Radioactive;
		

		public ExtendedDiseases(ResourceSet parent)
			: base(parent)
		{
			Debug.Log(" === MyDiseases.ctor === ");
			
			this.Radioactive = base.Add(new Radioactive());
			
		}

		new public static bool IsValidDiseaseID(string id)
		{
			Debug.Log(" === MyDiseases.IsValidDiseaseID === ");
			bool result = false;
			foreach (Disease resource in ((ExtendedDiseases)Db.Get().Diseases).resources)
			{
				if (resource.Id == id)
				{
					result = true;
				}
			}
			return result;
		}

		new public byte GetIndex(int hash)
		{
			Debug.Log(" === MyDiseases.( === ");
			var diseases = (ExtendedDiseases)Db.Get().Diseases;
			for (byte b = 0; b < diseases.Count; b = (byte)(b + 1))
			{
				Disease disease = ((ResourceSet<Disease>)diseases)[b];
				if (hash == disease.id.GetHashCode())
				{
					return b;
				}
			}
			return 255;
		}

		new public byte GetIndex(HashedString id)
		{
			return this.GetIndex(id.GetHashCode());
		}
	}
}