import type { components } from './generated/api';

export interface Skill {
    id: string;
    name: string;
}

export type CreateSkillCommandCorrected = Omit<
    components['schemas']['CreateSkillCommand'],
    'name' | 'descripttion'
> & {
    name: string;
};

export type EditSkillCommandCorrected = Omit<
    components['schemas']['EditSkillCommand'],
    'id' | 'name' | 'descripttion'
> & {
    id: string;
    name: string;
};
