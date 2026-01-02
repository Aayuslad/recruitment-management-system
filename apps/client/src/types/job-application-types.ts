import type { FeedbackStage, JobApplicationStatus } from './enums';
import type { components } from './generated/api';

export interface JobApplicationSummary {
    id: string;
    candidateId: string;
    candidateName: string;
    jobOpeningId: string;
    designation: string;
    appliedAt: string;
    status: JobApplicationStatus;
    avgRating?: number | null;
}

export interface JobOpeningApplicationSummary {
    id: string;
    candidateId: string;
    candidateName: string;
    appliedAt: string;
    avgRating?: number | null;
    status: JobApplicationStatus;
}

export interface JobApplication {
    id: string;
    candidateId: string;
    candidateName: string;
    jobOpeningId: string;
    appliedAt: string;
    status: JobApplicationStatus;
    designation: string;
    avgRating?: number | null;
    jobApplicationFeedbacks: FeedbackDetailDTO[];
    interviewFeedbacks: FeedbackDetailDTO[];
    statusMoveHistories: StatusMoveHistoryDetailDTO[];
}

export interface SkillFeedbackDetailDTO {
    skillId: string;
    skillName: string;
    rating: number;
    assessedExpYears?: number | null;
}

export interface StatusMoveHistoryDetailDTO {
    id: string;
    statusMovedTo: JobApplicationStatus;
    movedById: string;
    movedByName: string;
    movedAt: string;
    comment?: string | null;
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

export type CreateJobApplicationCommandCorrected = Omit<
    components['schemas']['CreateJobApplicationCommand'],
    'candidateId' | 'jobOpeningId'
> & {
    candidateId: string;
    jobOpeningId: string;
};

export type CreateJobApplicationsCommandCorrected = Omit<
    components['schemas']['CreateJobApplicationsCommand'],
    'applications'
> & {
    applications: (Omit<
        components['schemas']['JobApplicationDTO'],
        'candidateId' | 'jobOpeningId'
    > & {
        candidateId: string;
        jobOpeningId: string;
    })[];
};

export type CreateJobApplicationFeedbackCommandCorrected = Omit<
    components['schemas']['CreateJobApplicationFeedbackCommand'],
    'jobApplicationId' | 'comment' | 'rating' | 'skillFeedbacks'
> & {
    jobApplicationId: string;
    comment?: string | null;
    rating: number;
    skillFeedbacks: (Omit<
        components['schemas']['SkillFeedbackDTO'],
        'skillId' | 'rating' | 'assessedExpYears'
    > & {
        skillId: string;
        rating: number;
        assessedExpYears?: number | null;
    })[];
};

export type EditJobApplicationFeedbackCommandCorrected = Omit<
    components['schemas']['EditJobApplicationFeedbackCommand'],
    'jobApplicationId' | 'feedbackId' | 'comment' | 'rating' | 'skillFeedbacks'
> & {
    jobApplicationId: string;
    feedbackId: string;
    comment?: string | null;
    rating: number;
    skillFeedbacks: (Omit<
        components['schemas']['SkillFeedbackDTO'],
        'skillId' | 'rating' | 'assessedExpYears'
    > & {
        skillId: string;
        rating: number;
        assessedExpYears?: number | null;
    })[];
};

export type MoveJobApplicationStatusCommandCorrected = Omit<
    components['schemas']['MoveJobApplicationStatusCommand'],
    'id' | 'moveTo'
> & {
    id: string;
};
