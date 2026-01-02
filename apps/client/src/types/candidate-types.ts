import type { Gender, JobApplicationStatus } from './enums';
import type { components } from './generated/api';

export interface Candidate {
    id: string;
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    gender: Gender;
    contactNumber: string;
    dob: string;
    collegeName: string;
    resumeUrl: string;
    isBgVerificationCompleted: boolean;
    bgVerificationCompletedById?: string | null;
    bgVerificationCompletedByUserName?: string | null;
    createdAt: string;
    skills: CandidateSkillDetailDTO[];
    documents: CandidateDocumentDetailDTO[];
    jobApplications: JobApplicationDetailForCandidate[];
}

export interface CandidateSkillDetailDTO {
    skillId: string;
    skillName: string;
}

export interface CandidateDocumentDetailDTO {
    id: string;
    url: string;
    documentTypeId: string;
    documentName: string;
    isVerified: boolean;
    verifiedById?: string | null;
    verifiedByUserName?: string | null;
}

export interface CandidateSummary {
    id: string;
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    gender: Gender;
    contactNumber: string;
    dob: string;
    collegeName: string;
    resumeUrl: string;
    isBgVerificationCompleted: boolean;
    isDocumentsVerified: boolean;
    jobApplications: JobApplicationSummaryForCandidate[];
    createdAt: string;
}

export interface JobApplicationSummaryForCandidate {
    status: JobApplicationStatus;
}

export interface JobApplicationDetailForCandidate {
    id: string;
    designationName: string;
    jobLocation: string;
    appliedAt: string;
    status: JobApplicationStatus;
}

export type CreateCandidateCommandCorrected = Omit<
    components['schemas']['CreateCandidateCommand'],
    | 'email'
    | 'firstName'
    | 'middleName'
    | 'lastName'
    | 'contactNumber'
    | 'dob'
    | 'collegeName'
    | 'resumeUrl'
    | 'skills'
    | 'documents'
> & {
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    contactNumber: string;
    dob: string;
    collegeName: string;
    resumeUrl: string;
    skills: (Omit<components['schemas']['CandidateSkillDTO'], 'skillId'> & {
        skillId: string;
    })[];
};

export type EditCandidateCommandCorrected = Omit<
    components['schemas']['EditCandidateCommand'],
    | 'id'
    | 'email'
    | 'firstName'
    | 'middleName'
    | 'lastName'
    | 'contactNumber'
    | 'dob'
    | 'collegeName'
    | 'resumeUrl'
    | 'skills'
    | 'documents'
> & {
    id: string;
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    contactNumber: string;
    dob: string;
    collegeName: string;
    resumeUrl: string;
    skills: (Omit<components['schemas']['CandidateSkillDTO'], 'skillId'> & {
        skillId: string;
    })[];
    documents?:
        | (Omit<
              components['schemas']['DocumentDTO'],
              'id' | 'documentTypeId' | 'url'
          > & {
              id?: string | null;
              documentTypeId: string;
              url: string;
          })[]
        | null;
};

export type AddCandidateDocumentCommandCorrected = Omit<
    components['schemas']['AddCandidateDocumentCommand'],
    'id' | 'documentTypeId' | 'url'
> & {
    id: string;
    documentTypeId: string;
    url: string;
};

export type VerifyCandidateDocumentCommandCorrected = Omit<
    components['schemas']['VerifyCandidateDocumentCommand'],
    'candidateId' | 'documentId'
> & {
    candidateId: string;
    documentId: string;
};
