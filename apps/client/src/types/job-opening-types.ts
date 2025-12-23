import type {
    InterviewParticipantRole,
    InterviewType,
    JobOpeningType,
    SkillActionType,
    SkillType,
} from './enums';
import type { components } from './generated/api';

export interface JobOpeningSummary {
    id: string;
    title: string;
    type: JobOpeningType;
    designationId: string;
    designationName: string;
    jobLocation: string;
    createdById: string;
    createdByUserName: string;
    interviewRounds: InterviewRoundTemplateSummaryDTO[];
}

export interface JobOpening {
    id: string;
    title: string;
    description?: string | null;
    type: JobOpeningType;
    positionBatchId: string;
    designationId: string;
    designationName: string;
    jobLocation: string;
    minCTC: number;
    maxCTC: number;
    positionsCount: number;
    closedPositionsCount: number;
    skills: SkillDetailDTO[];
    skillOverRides: SkillOverRideDetailDTO[];
    interviewers: JobOpeningInterviewerDetailDTO[];
    interviewRounds: InterviewRoundTemplateDetailDTO[];
}

export interface SkillDetailDTO {
    skillId: string;
    skillName: string;
    skillType: SkillType;
    minExperienceYears?: number | null;
}

export interface SkillOverRideDetailDTO {
    id: string;
    skillId: string;
    comments?: string | null;
    minExperienceYears: number;
    type: SkillType;
    actionType: SkillActionType;
}

export interface JobOpeningInterviewerDetailDTO {
    id: string;
    userId: string;
    userName: string;
    email: string;
    role: InterviewParticipantRole;
}

export interface InterviewPanelRequirementDetailDTO {
    id?: string | null;
    role: InterviewParticipantRole;
    requirementCount: number;
}

export interface InterviewRoundTemplateDetailDTO {
    id: string;
    description?: string | null;
    roundNumber: number;
    durationInMinutes: number;
    type: InterviewType;
    requirements: InterviewPanelRequirementDetailDTO[];
}

export interface InterviewRoundTemplateSummaryDTO {
    roundNumber: number;
    type: InterviewType;
}

export type CreateJobOpeningCommandCorrected = Omit<
    components['schemas']['CreateJobOpeningCommand'],
    | 'title'
    | 'type'
    | 'positionBatchId'
    | 'interviewers'
    | 'interviewRounds'
    | 'skillOverRides'
> & {
    title: string;
    type: components['schemas']['JobOpeningType'];
    positionBatchId: string;
    interviewers: (Omit<
        components['schemas']['JobOpeningInterviewerDTO'],
        'userId' | 'role'
    > & {
        userId: string;
        role: components['schemas']['InterviewParticipantRole'];
    })[];
    interviewRounds: (Omit<
        components['schemas']['InterviewRoundTemplateDTO'],
        'roundNumber' | 'durationInMinutes' | 'type' | 'requirements'
    > & {
        roundNumber: number;
        durationInMinutes: number;
        type: components['schemas']['InterviewType'];
        requirements: (Omit<
            components['schemas']['InterviewPanelRequirementDTO'],
            'role' | 'requirementCount'
        > & {
            role: components['schemas']['InterviewParticipantRole'];
            requirementCount: number;
        })[];
    })[];
    skillOverRides: (Omit<
        components['schemas']['SkillOverRideDTO'],
        'skillId' | 'minExperienceYears' | 'type' | 'actionType'
    > & {
        skillId: string;
        minExperienceYears: number;
        type: components['schemas']['SkillType'];
        actionType: components['schemas']['SkillActionType'];
    })[];
};

export type EditJobOpeningCommandCorrected = Omit<
    components['schemas']['EditJobOpeningCommand'],
    | 'jobOpeningId'
    | 'title'
    | 'type'
    | 'positionBatchId'
    | 'interviewers'
    | 'interviewRounds'
    | 'skillOverRides'
> & {
    jobOpeningId: string;
    title: string;
    type: components['schemas']['JobOpeningType'];
    positionBatchId: string;
    interviewers: (Omit<
        components['schemas']['JobOpeningInterviewerDTO'],
        'userId' | 'role'
    > & {
        userId: string;
        role: components['schemas']['InterviewParticipantRole'];
    })[];
    interviewRounds: (Omit<
        components['schemas']['InterviewRoundTemplateDTO'],
        'roundNumber' | 'durationInMinutes' | 'type' | 'requirements'
    > & {
        roundNumber: number;
        durationInMinutes: number;
        type: InterviewType;
        requirements: (Omit<
            components['schemas']['InterviewPanelRequirementDTO'],
            'role' | 'requirementCount'
        > & {
            role: components['schemas']['InterviewParticipantRole'];
            requirementCount: number;
        })[];
    })[];
    skillOverRides: (Omit<
        components['schemas']['SkillOverRideDTO'],
        'skillId' | 'minExperienceYears' | 'type' | 'actionType'
    > & {
        skillId: string;
        minExperienceYears: number;
        type: components['schemas']['SkillType'];
        actionType: components['schemas']['SkillActionType'];
    })[];
};
