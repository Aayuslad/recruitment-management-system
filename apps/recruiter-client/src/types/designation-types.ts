import type { components } from './generated/api';

export interface DesignationDTO {
    id: string;
    name: string;
    description: string;
    designationSkills: DesignationSkillDTO[];
    createdBy?: string;
    createdAt?: string;
    lastUpdatedBy?: string;
    lastUpdatedAt?: string;
}

export interface DesignationSkillDTO {
    skillId: string;
    name: string;
    skillType: string;
    minExperienceYears: number;
}

export type CreateDesignationCommandCorrected = Omit<
    components['schemas']['CreateDesignationCommand'],
    'name' | 'descripttion' | 'designationSkills'
> & {
    name: string;
    description: string;
    designationSkills:
        | {
              skillId: string;
              skillType: 'Required' | 'Preferred' | 'NiceToHave';
              minExperienceYears: number;
          }[]
        | null;
};

export type EditDesignationCommandCorrected = Omit<
    components['schemas']['EditDesignationCommand'],
    'id' | 'name' | 'descripttion' | 'designationSkills'
> & {
    id: string;
    name: string;
    description: string;
    designationSkills: {
        skillId: string;
        skillType: 'Required' | 'Preferred' | 'NiceToHave';
        minExperienceYears: number;
    }[];
};
