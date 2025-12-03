import type { SkillType } from './enums';
import type { components } from './generated/api';

export interface Designation {
    id: string;
    name: string;
    designationSkills: DesignationSkillDTO[];
}

export interface DesignationSkillDTO {
    skillId: string;
    name: string;
    skillType: SkillType;
    minExperienceYears: number;
}

export type CreateDesignationCommandCorrected = Omit<
    components['schemas']['CreateDesignationCommand'],
    'name' | 'designationSkills'
> & {
    name: string;
    designationSkills: {
        skillId: string;
        skillType: 'Required' | 'Preferred' | 'NiceToHave';
        minExperienceYears: number;
    }[];
};

export type EditDesignationCommandCorrected = Omit<
    components['schemas']['EditDesignationCommand'],
    'id' | 'name' | 'designationSkills'
> & {
    id: string;
    name: string;
    designationSkills: {
        skillId: string;
        skillType: 'Required' | 'Preferred' | 'NiceToHave';
        minExperienceYears: number;
    }[];
};
