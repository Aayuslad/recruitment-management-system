namespace Server.Domain.Entities
{
    public class CandidateSkill
    {
        private CandidateSkill() { }

        private CandidateSkill(Guid candidateId, Guid skillId)
        {
            CandidateId = candidateId;
            SkillId = skillId;
        }

        public Guid CandidateId { get; set; }
        public Guid SkillId { get; set; }
        public Candidate Candidate { get; set; } = null!;
        public Skill Skill { get; set; } = null!;

        public static CandidateSkill Create(Guid candidateId, Guid skillId)
        {
            return new CandidateSkill(candidateId, skillId);
        }
    }
}