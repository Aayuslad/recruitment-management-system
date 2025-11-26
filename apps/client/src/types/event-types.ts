import type { EventType } from './enums';
import type { components } from './generated/api';

export interface Event {
    id: string;
    name: string;
    type: EventType;
    jobOpenings: EventJobOpeningDetailDTO[];
}

export interface EventJobOpeningDetailDTO {
    jobOpeningId: string;
    jobOpeningTitle: string;
    designationId: string;
    designationName: string;
}

export type CreateEventCommandCorrected = Omit<
    components['schemas']['CreateEventCommand'],
    'name' | 'type' | 'jobOpenings'
> & {
    name: string;
    type: components['schemas']['EventType'];
    jobOpenings: (Omit<
        components['schemas']['EventJobOpeningDTO'],
        'jobOpeningId'
    > & {
        jobOpeningId: string;
    })[];
};

export type EditEventCommandCorrected = Omit<
    components['schemas']['EditEventCommand'],
    'id' | 'name' | 'type' | 'jobOpenings'
> & {
    id: string;
    name: string;
    type: components['schemas']['EventType'];
    jobOpenings: (Omit<
        components['schemas']['EventJobOpeningDTO'],
        'jobOpeningId'
    > & {
        jobOpeningId: string;
    })[];
};
