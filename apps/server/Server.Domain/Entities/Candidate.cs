using Server.Core.Primitives;
using Server.Domain.ValueObjects;

namespace Server.Domain.Entities
{
    public class Candidate : AuditableEntity, IAggregateRoot
    {
        private Candidate() : base(Guid.Empty, Guid.Empty) { }

        private Candidate(
            Guid? id,
            Guid createdBy,
            Email email,
            string firstName,
            string? middleName,
            string lastName,
            ContactNumber contactNumber,
            DateTime dob,
            string resumeUrl,
            IEnumerable<CandidateSkill> skills
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            Email = email;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            ContactNumber = contactNumber;
            Dob = dob;
            ResumeUrl = resumeUrl;
            IsBgVerificationCompleted = false;
            Skills = skills?.ToHashSet() ?? [];
        }

        public Email Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public ContactNumber ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string ResumeUrl { get; set; } = null!;
        public bool IsBgVerificationCompleted { get; set; }
        public Guid? BgVerifiedById { get; set; }
        public User? BgVerifiedByUser { get; set; }

        public ICollection<CandidateSkill> Skills { get; set; } = new HashSet<CandidateSkill>();
        public ICollection<CandidateDocument> Documents { get; set; } = new HashSet<CandidateDocument>();

        public static Candidate Create(
            Guid? id,
            Guid createdBy,
            Email email,
            string firstName,
            string? middleName,
            string lastName,
            ContactNumber contactNumber,
            DateTime dob,
            string resumeUrl,
            IEnumerable<CandidateSkill> skills
        )
        {
            return new Candidate(
                id,
                createdBy,
                email,
                firstName,
                middleName,
                lastName,
                contactNumber,
                dob,
                resumeUrl,
                skills
            );
        }

        public void MarkBackgroundVerified(Guid verifiedBy)
        {
            IsBgVerificationCompleted = true;
            BgVerifiedById = verifiedBy;
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void Update(
            Guid updatedBy,
            Email email,
            string firstName,
            string? middleName,
            string lastName,
            ContactNumber contactNumber,
            DateTime dob,
            string resumeUrl,
            IEnumerable<CandidateSkill> skills,
            IEnumerable<CandidateDocument> documents
        )
        {
            Email = email;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            ContactNumber = contactNumber;
            Dob = dob;
            ResumeUrl = resumeUrl;

            SyncSkills(skills);
            SyncDocuments(documents);

            MarkAsUpdated(updatedBy);
        }

        private void SyncSkills(IEnumerable<CandidateSkill> newSkills)
        {
            if (newSkills is null)
                return;

            // remove missing
            foreach (var existing in Skills.ToList())
            {
                if (!newSkills.Any(x => x.SkillId == existing.SkillId))
                    Skills.Remove(existing);
            }

            // add new
            foreach (var skill in newSkills)
            {
                if (!Skills.Any(x => x.SkillId == skill.SkillId))
                    Skills.Add(skill);
            }
        }

        private void SyncDocuments(IEnumerable<CandidateDocument> newDocuments)
        {
            if (newDocuments is null)
                return;

            // remove missing
            foreach (var existing in Documents.ToList())
            {
                if (!newDocuments.Any(x => x.Id == existing.Id))
                    Documents.Remove(existing);
            }

            // add new
            foreach (var doc in newDocuments)
            {
                if (!Documents.Any(x => x.Id == doc.Id))
                    Documents.Add(doc);
            }
        }
    }
}