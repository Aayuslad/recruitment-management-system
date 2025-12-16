export const USER_STATUS = {
    ON_HOLD: 'OnHold',
    ACTIVE: 'Active',
    INACTIVE: 'Inactive',
    PENDING: 'Pending',
} as const;
export type UserStatus = (typeof USER_STATUS)[keyof typeof USER_STATUS];

export const GENDER = {
    MALE: 'Male',
    FEMALE: 'Female',
    OTHER: 'Other',
    PREFER_NOT_TO_SAY: 'PreferNotToSay',
} as const;
export type Gender = (typeof GENDER)[keyof typeof GENDER];

export const SKILL_TYPE = {
    REQUIRED: 'Required',
    PREFERRED: 'Preferred',
    NICE_TO_HAVE: 'NiceToHave',
} as const;
export type SkillType = (typeof SKILL_TYPE)[keyof typeof SKILL_TYPE];

export const SKILL_ACTION_TYPE = {
    ADD: 'Add',
    REMOVE: 'Remove',
    UPDATE: 'Update',
} as const;
export type SkillActionType =
    (typeof SKILL_ACTION_TYPE)[keyof typeof SKILL_ACTION_TYPE];

export const SKILL_SOURCE_TYPE = {
    POSITION: 'Position',
    JOB_OPENING: 'JobOpening',
} as const;
export type SkillSourceType =
    (typeof SKILL_SOURCE_TYPE)[keyof typeof SKILL_SOURCE_TYPE];

export const POSITION_STATUS = {
    OPEN: 'Open',
    ON_HOLD: 'OnHold',
    CLOSED: 'Closed',
} as const;
export type PositionStatus =
    (typeof POSITION_STATUS)[keyof typeof POSITION_STATUS];

export const JOB_OPENING_TYPE = {
    NORMAL: 'Normal',
    CAMPUS_DRIVE: 'CampusDrive',
    WALK_IN: 'WalkIn',
} as const;
export type JobOpeningType =
    (typeof JOB_OPENING_TYPE)[keyof typeof JOB_OPENING_TYPE];

export const JOB_APPLICATION_STATUS = {
    APPLIED: 'Applied',
    SHORTLISTED: 'Shortlisted',
    INTERVIEWED: 'Interviewed',
    OFFERED: 'Offered',
    REJECTED: 'Rejected',
    HIRED: 'Hired',
} as const;
export type JobApplicationStatus =
    (typeof JOB_APPLICATION_STATUS)[keyof typeof JOB_APPLICATION_STATUS];

export const INTERVIEW_TYPE = {
    TECHNICAL: 'Technical',
    HR: 'HR',
    ONLINE_TEST: 'OnlineTest',
} as const;
export type InterviewType =
    (typeof INTERVIEW_TYPE)[keyof typeof INTERVIEW_TYPE];

export const INTERVIEW_STATUS = {
    UNSCHEDULED: 'Unscheduled',
    SCHEDULED: 'Scheduled',
    COMPLETED: 'Completed',
} as const;
export type InterviewStatus =
    (typeof INTERVIEW_STATUS)[keyof typeof INTERVIEW_STATUS];

export const INTERVIEW_PARTICIPANT_ROLE = {
    INTERVIEWER: 'Interviewer',
    OBSERVER: 'Observer',
    NOTE_TAKER: 'NoteTaker',
    HR_REPRESENTATIVE: 'HRRepresentative',
    HIRING_MANAGER: 'HiringManager',
} as const;
export type InterviewParticipantRole =
    (typeof INTERVIEW_PARTICIPANT_ROLE)[keyof typeof INTERVIEW_PARTICIPANT_ROLE];

export const FEEDBACK_STAGE = {
    REVIEW: 'Review',
    INTERVIEW: 'Interview',
} as const;
export type FeedbackStage =
    (typeof FEEDBACK_STAGE)[keyof typeof FEEDBACK_STAGE];

export const EVENT_TYPE = {
    WALK_IN: 'WalkIn',
    CAMPUS_DRIVE: 'CampusDrive',
} as const;
export type EventType = (typeof EVENT_TYPE)[keyof typeof EVENT_TYPE];
