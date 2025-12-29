import type { PositionStatus, SkillActionType, SkillType } from './enums';
import type { components } from './generated/api';

export interface PositionBatchSummary {
    batchId: string;
    description?: string;
    designationId: string;
    designationName: string;
    jobLocation: string;
    minCTC: number;
    maxCTC: number;
    positionsCount: number;
    closedPositionsCount: number;
    positionsOnHoldCount: number;
    createdBy?: string | null;
    createdByUserName?: string | null;
    createdAt: string;
}

export interface PositionBatch {
    batchId: string;
    description?: string;
    designationId: string;
    designationName: string;
    jobLocation: string;
    minCTC: number;
    maxCTC: number;
    positionsCount: number;
    closedPositionsCount: number;
    positionsOnHoldCount: number;
    createdBy?: string | null;
    createdByUserName?: string | null;
    reviewers: ReviewerDetailDTO[];
    createdAt: string;
    skills: SkillDetailDTO[];
    skillOverRides: SkillOverRideDetailDTO[];
}

export interface PositionSummary {
    batchId: string;
    positionId: string;
    description?: string;
    designationId: string;
    designationName: string;
    jobLocation: string;
    minCTC: number;
    maxCTC: number;
    status: PositionStatus;
    closedByCandidate?: string | null;
    closureReason?: string | null;
}

export interface BatchPositionsSummary {
    batchId: string;
    positionId: string;
    status: PositionStatus;
    closedByCandidateId?: string | null;
    closedByCandidateFullName?: string | null;
    closureReason?: string | null;
}

export interface Position {
    batchId: string;
    positionId: string;
    description?: string;
    designationId: string;
    designationName: string;
    jobLocation: string;
    minCTC: number;
    maxCTC: number;
    status: PositionStatus;
    closedByCandidate?: string | null;
    closureReason?: string | null;
    skills: SkillDetailDTO[];
    reviewers: ReviewerDetailDTO[];
    moveHistory: PositionStatusMoveHistoryDetailDTO[];
}

export interface ReviewerDetailDTO {
    reviewerUserId: string;
    reviewerUserName: string;
    reviewerUserEmail: string;
}

export interface SkillDetailDTO {
    skillId: string;
    skillName: string;
    skillType: SkillType;
}

export interface SkillOverRideDetailDTO {
    id: string | null;
    skillId: string;
    comments?: string | null;
    type: SkillType;
    actionType: SkillActionType;
}

export interface PositionStatusMoveHistoryDetailDTO {
    id: string;
    positionId: string;
    movedTo: PositionStatus;
    movedBy?: string | null;
    comments?: string | null;
    movedAt: string;
    movedById?: string | null;
    movedByUserName?: string | null;
}

export type CreatePositionBatchCommandCorrected = Omit<
    components['schemas']['CreatePositionBatchCommand'],
    | 'numberOfPositions'
    | 'designationId'
    | 'minCTC'
    | 'maxCTC'
    | 'jobLocation'
    | 'reviewers'
    | 'skillOverRides'
> & {
    numberOfPositions: number;
    designationId: string;
    minCTC: number;
    maxCTC: number;
    jobLocation: string;
    reviewers: (Omit<
        components['schemas']['PositionReviewersDTO'],
        'reviewerUserId'
    > & {
        reviewerUserId: string;
    })[];
    skillOverRides: (Omit<
        components['schemas']['PositionSkillOverRideDTO'],
        'skillId' | 'type' | 'actionType' | 'id' | 'comments'
    > & {
        id?: string | null;
        skillId: string;
        comments?: string | null;
        type: components['schemas']['SkillType'];
        actionType: components['schemas']['SkillActionType'];
    })[];
};

export type EditPositionBatchCommandCorrected = Omit<
    components['schemas']['EditPositionBatchCommand'],
    | 'positionBatchId'
    | 'minCTC'
    | 'maxCTC'
    | 'jobLocation'
    | 'reviewers'
    | 'skillOverRides'
> & {
    positionBatchId: string;
    minCTC: number;
    maxCTC: number;
    jobLocation: string;
    reviewers: (Omit<
        components['schemas']['PositionReviewersDTO'],
        'reviewerUserId'
    > & {
        reviewerUserId: string;
    })[];
    skillOverRides: (Omit<
        components['schemas']['PositionSkillOverRideDTO'],
        'skillId' | 'type' | 'actionType' | 'id' | 'comments'
    > & {
        id?: string | null;
        skillId: string;
        comments?: string | null;
        type: components['schemas']['SkillType'];
        actionType: components['schemas']['SkillActionType'];
    })[];
};

export type ClosePositionCommandCorrected = Omit<
    components['schemas']['ClosePositionCommand'],
    'positionId'
> & {
    positionId: string;
    closureReason?: string | null;
};

export type SetPositionOnHoldCommandCorrected = Omit<
    components['schemas']['SetPositionOnHoldCommand'],
    'positionId'
> & {
    positionId: string;
    comments?: string | null;
};
