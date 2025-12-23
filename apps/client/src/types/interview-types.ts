import type {
    FeedbackStage,
    InterviewParticipantRole,
    InterviewStatus,
    InterviewType,
} from './enums';
import type { components } from './generated/api';

export interface InterviewSummary {
    id: string;
    candidateId: string;
    candidateName: string;
    designationId: string;
    designationName: string;
    roundNumber: number;
    interviewType: InterviewType;
    scheduledAt?: string | null;
    durationInMinutes: number;
    status: InterviewStatus;
}

export interface Interview {
    id: string;
    jobApplicationId: string;
    candidateId: string;
    candidateName: string;
    designationId: string;
    designationName: string;
    roundNumber: number;
    interviewType: InterviewType;
    scheduledAt?: string | null;
    durationInMinutes: number;
    meetingLink?: string | null;
    status: InterviewStatus;
    feedbacks: FeedbackDetailDTO[];
    participants: InterviewParticipantDetailDTO[];
}

export interface SkillFeedbackDetailDTO {
    skillId: string;
    skillName: string;
    rating: number;
    assessedExpYears?: number | null;
}

export interface InterviewParticipantDetailDTO {
    id: string;
    userId: string;
    participantUserName: string;
    role: InterviewParticipantRole;
}

export interface FeedbackDetailDTO {
    id: string;
    givenById: string;
    givenByName: string;
    stage: FeedbackStage;
    comment?: string | null;
    rating: number;
    skillFeedbacks: SkillFeedbackDetailDTO[];
}

export type CreateInterviewCommandCorrected = Omit<
    components['schemas']['CreateInterviewCommand'],
    | 'jobApplicationId'
    | 'roundNumber'
    | 'interviewType'
    | 'scheduledAt'
    | 'durationInMinutes'
    | 'meetingLink'
    | 'participants'
> & {
    jobApplicationId: string;
    roundNumber: number;
    interviewType: components['schemas']['InterviewType'];
    scheduledAt?: string | null;
    durationInMinutes: number;
    meetingLink?: string | null;
    participants: (Omit<
        components['schemas']['InterviewParticipantDTO'],
        'userId' | 'role' | 'id'
    > & {
        userId: string;
        role: components['schemas']['InterviewParticipantRole'];
        id?: string | null;
    })[];
};

export type EditInterviewCommandCorrected = Omit<
    components['schemas']['EditInterviewCommand'],
    | 'id'
    | 'roundNumber'
    | 'interviewType'
    | 'scheduledAt'
    | 'durationInMinutes'
    | 'meetingLink'
    | 'status'
    | 'participants'
> & {
    id: string;
    roundNumber: number;
    interviewType: components['schemas']['InterviewType'];
    scheduledAt?: string | null;
    durationInMinutes: number;
    meetingLink?: string | null;
    status: components['schemas']['InterviewStatus'];
    participants: (Omit<
        components['schemas']['InterviewParticipantDTO'],
        'userId' | 'role' | 'id'
    > & {
        userId: string;
        role: components['schemas']['InterviewParticipantRole'];
        id?: string | null;
    })[];
};

export type MoveInterviewStatusCommandCorrected = Omit<
    components['schemas']['MoveInterviewStatusCommand'],
    'interviewId' | 'moveTo' | 'scheduledAt'
> & {
    interviewId: string;
    moveTo: components['schemas']['InterviewStatus'];
    scheduledAt?: Date | null;
};

export type CreateInterviewFeedbackCommandCorrected = Omit<
    components['schemas']['CreateInterviewFeedbackCommand'],
    'interviewId' | 'comment' | 'rating' | 'skillFeedbacks'
> & {
    interviewId: string;
    comment?: string | null;
    rating: number;
    skillFeedbacks: (Omit<
        components['schemas']['InterviewSkillFeedbackDTO'],
        'skillId' | 'rating' | 'assessedExpYears'
    > & {
        skillId: string;
        rating: number;
        assessedExpYears?: number | null;
    })[];
};

export type EditInterviewFeedbackCommandCorrected = Omit<
    components['schemas']['EditInterviewFeedbackCommand'],
    'interviewId' | 'feedbackId' | 'comment' | 'rating' | 'skillFeedbacks'
> & {
    interviewId: string;
    feedbackId: string;
    comment?: string | null;
    rating: number;
    skillFeedbacks: (Omit<
        components['schemas']['InterviewSkillFeedbackDTO'],
        'skillId' | 'rating' | 'assessedExpYears'
    > & {
        skillId: string;
        rating: number;
        assessedExpYears?: number | null;
    })[];
};
