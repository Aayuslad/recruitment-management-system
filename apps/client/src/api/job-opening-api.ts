import type {
    JobOpening,
    JobOpeningSummary,
    CreateJobOpeningCommandCorrected,
    EditJobOpeningCommandCorrected,
} from '@/types/job-opening-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateJobOpening() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateJobOpeningCommandCorrected
        ): Promise<void> => {
            await axios.post('/job-opening', payload);
        },
        onSuccess: () => {
            toast.success('Job opening created');
            queryClient.invalidateQueries({ queryKey: ['job-openings'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to create job opening'
            );
            console.error('JobOpening creation failed', error);
        },
    });
}

export function useEditJobOpening() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditJobOpeningCommandCorrected
        ): Promise<void> => {
            await axios.put(`/job-opening/${payload.jobOpeningId}`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Job opening updated');
            queryClient.invalidateQueries({ queryKey: ['job-openings'] });
            if (variables?.jobOpeningId) {
                queryClient.invalidateQueries({
                    queryKey: ['job-opening', variables.jobOpeningId],
                });
            }
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to update job opening'
            );
            console.error('EditJobOpening failed', error);
        },
    });
}

export function useDeleteJobOpening() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/job-opening/${id}`);
        },
        onSuccess: () => {
            toast.success('Job opening deleted');
            queryClient.invalidateQueries({ queryKey: ['job-openings'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to delete job opening'
            );
            console.error('DeleteJobOpening failed', error);
        },
    });
}

export function useGetJobOpenings() {
    return useQuery({
        queryKey: ['job-openings'],
        queryFn: async (): Promise<JobOpeningSummary[]> => {
            const { data } = await axios.get('/job-opening');
            return data;
        },
    });
}

export function useGetJobOpening(id: string) {
    return useQuery({
        queryKey: ['job-opening', id],
        queryFn: async (): Promise<JobOpening | null> => {
            const { data } = await axios.get(`/job-opening/${id}`);
            return data;
        },
    });
}
