export type UserStatus = 'OnHold' | 'Active' | 'Inactive' | 'Pending';
export type Gender = 'Male' | 'Female' | 'Other' | 'PreferNotToSay';

export type SkillType = 'Required' | 'Preferred' | 'NiceToHave';
export type SkillActionType = 'Add' | 'Remove' | 'Update';
export type SkillSourceType = 'Position' | 'JobOpening';

export type PositionStatus = 'Open' | 'OnHold' | 'Closed';

export type JobOpeningType = 'Normal' | 'CampusDrive' | 'WalkIn';

export type JobApplicationStatus =
    | 'Applied'
    | 'Shortlisted'
    | 'Interviwed'
    | 'Offered'
    | 'Rejected'
    | 'Hired';

export type InterviewType = 'Technical' | 'HR' | 'OnlineTest';
export type InterviewStatus = 'Unscheduled' | 'Scheduled' | 'Completed';
export type InterviewParticipantRole =
    | 'Interviewer'
    | 'Observer'
    | 'NoteTaker'
    | 'HRRepresentative'
    | 'HiringManager';

export type FeedbackStage = 'Review' | 'Interview';

export type EventType = 'WalkIn' | 'CampusDrive';
