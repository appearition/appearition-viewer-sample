using System.Collections.Generic;
using System.Linq;

namespace Appearition.Assessments
{
	public class AssessmentOptionViewModel
	{
		public long AssessmentOptionKey { get; set; }

		public string AssessmentName { get; set; }

		public IEnumerable<ProficiencyOptionViewModel> ProficiencyLevels { get; set; }

		public AssessmentOptionViewModel ()
		{
			ProficiencyLevels = new List<ProficiencyOptionViewModel> ();
		}

		public class ProficiencyOptionViewModel
		{
			public long AssessmentId { get; set; }

			public string AssessmentName { get; set; }

			public long ProficiencyId { get; set; }

			public string ProficiencyName { get; set; }
		}

        public bool HasProficiencyId(int proficiencyId)
        {
            return ProficiencyLevels?.FirstOrDefault(o => o.ProficiencyId == (long)proficiencyId) != null;
        }
	}
}
