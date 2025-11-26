import type { components } from './generated/api';

export interface Candidate {
    id: string;
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    contactNumber: string;
    dob: string;
    resumeUrl: string;
    isBgVerificationCompleted: boolean;
    bgVerificationCompletedById?: string | null;
    bgVerificationCompletedByUserName?: string | null;
    skills: CandidateSkillDetailDTO[];
    documents: CandidateDocumentDetailDTO[];
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
    verifiedBy?: string | null;
}

export interface CandidateSummary {
    id: string;
    email: string;
    firstName: string;
    middleName?: string | null;
    lastName: string;
    contactNumber: string;
    dob: string;
    resumeUrl: string;
    isBgVerificationCompleted: boolean;
}

export type CreateCandidateCommandCorrected = Omit<
    components['schemas']['CreateCandidateCommand'],
    | 'email'
    | 'firstName'
    | 'middleName'
    | 'lastName'
    | 'contactNumber'
    | 'dob'
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
