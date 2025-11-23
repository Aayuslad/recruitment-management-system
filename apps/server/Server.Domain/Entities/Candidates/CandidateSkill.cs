using Server.Domain.Entities.Skills;

namespace Server.Domain.Entities.Candidates
{
    public class CandidateSkill
    {
        private CandidateSkill() { }

        private CandidateSkill(Guid candidateId, Guid skillId)
        {
            CandidateId = candidateId;
            SkillId = skillId;
        }

        public Guid CandidateId { get; private set; }
        public Guid SkillId { get; private set; }
        public Candidate Candidate { get; private set; } = null!;
        public Skill Skill { get; private set; } = null!;

        public static CandidateSkill Create(Guid candidateId, Guid skillId)
        {
            return new CandidateSkill(candidateId, skillId);
        }
    }
}