import type { components } from './generated/api';

export interface Role {
    id: string;
    name: string;
    description?: string | null;
}

export type CreateRoleCommandCorrected = Omit<
    components['schemas']['CreateRoleCommand'],
    'name'
> & {
    name: string;
};

export type EditRoleCommandCorrected = Omit<
    components['schemas']['EditRoleCommand'],
    'id' | 'name'
> & {
    id: string;
    name: string;
};
