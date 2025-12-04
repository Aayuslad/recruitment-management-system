import type { components } from './generated/api';

export interface Document {
    id: string;
    name: string;
}

export type CreateDocumentTypeCommandCorrected = Omit<
    components['schemas']['CreateDocumentTypeCommand'],
    'name'
> & {
    name: string;
};

export type EditDocumentTypeCommandCorrected = Omit<
    components['schemas']['EditDocumentTypeCommand'],
    'name'
> & {
    id: string;
    name: string;
};
