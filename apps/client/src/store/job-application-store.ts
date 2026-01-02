import {
    JOB_APPLICATION_STATUS,
    type JobApplicationStatus,
} from '@/types/enums';
import type { StateCreator } from 'zustand';

export const JobApplicationIndexTabs: {
    value: JobApplicationStatus | 'All';
    label: string;
}[] = [
    {
        value: JOB_APPLICATION_STATUS.APPLIED,
        label: 'Review Pending',
    },
    {
        value: JOB_APPLICATION_STATUS.SHORTLISTED,
        label: 'Shortlisted',
    },
    {
        value: JOB_APPLICATION_STATUS.INTERVIEWED,
        label: 'Interviewed',
    },
    {
        value: JOB_APPLICATION_STATUS.OFFERED,
        label: 'Offered',
    },
    {
        value: JOB_APPLICATION_STATUS.REJECTED,
        label: 'Rejected',
    },
    {
        value: JOB_APPLICATION_STATUS.HIRED,
        label: 'Hired',
    },
    {
        value: 'All',
        label: 'All',
    },
];

export type JobApplicationStoreSliceType = {
    currentJobApplicationTab: JobApplicationStatus | 'All';

    setCurrentJobApplicationTab: (tab: JobApplicationStatus | 'All') => void;
};

export const createJobApplicationSlice: StateCreator<
    JobApplicationStoreSliceType
> = (set) => ({
    currentJobApplicationTab: 'Applied',

    setCurrentJobApplicationTab: (tab) =>
        set({
            currentJobApplicationTab:
                JobApplicationIndexTabs.find((t) => t.value === tab)?.value ??
                'Applied',
        }),
});
