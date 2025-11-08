import type { components } from './generated/api';

export interface SkillDTO {
    id: string;
    name: string;
    description: string;
    createdBy?: string;
    createdAt?: string;
    lastUpdatedBy?: string;
    lastUpdatedAt?: string;
}

export type CreateSkillCommandCorrected = Omit<
    components['schemas']['CreateSkillCommand'],
    'name' | 'descripttion'
> & {
    name: string;
    description: string;
};

export type EditSkillCommandCorrected = Omit<
    components['schemas']['EditSkillCommand'],
    'id' | 'name' | 'descripttion'
> & {
    id: string;
    name: string;
    description: string;
};
