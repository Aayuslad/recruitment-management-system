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
