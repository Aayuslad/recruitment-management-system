import type {
    JobApplication,
    JobApplicationSummary,
    CreateJobApplicationCommandCorrected,
    CreateJobApplicationsCommandCorrected,
    CreateJobApplicationFeedbackCommandCorrected,
    EditJobApplicationFeedbackCommandCorrected,
    MoveJobApplicationStatusCommandCorrected,
    JobOpeningApplicationSummary,
} from '@/types/job-application-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateJobApplication() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateJobApplicationCommandCorrected
        ): Promise<void> => {
            await axios.post('/job-application', payload);
        },
        onSuccess: () => {
            toast.success('Job application created');
            queryClient.invalidateQueries({ queryKey: ['job-applications'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to create job application'
            );
            console.error(error);
        },
    });
}

export function useCreateJobApplications() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateJobApplicationsCommandCorrected
        ): Promise<void> => {
            await axios.post('/job-application/bulk', payload);
        },
        onSuccess: () => {
            toast.success('Job applications created');
            queryClient.invalidateQueries({ queryKey: ['job-applications'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to create job applications'
            );
            console.error(error);
        },
    });
}

export function useCreateJobApplicationFeedback() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateJobApplicationFeedbackCommandCorrected
        ) => {
            await axios.post(
                `/job-application/${payload.jobApplicationId}/feedback`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Feedback added');
            queryClient.invalidateQueries({ queryKey: ['job-applications'] });
            queryClient.invalidateQueries({
                queryKey: ['job-application', variables.jobApplicationId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to add feedback'
            );
            console.error(error);
        },
    });
}

export function useEditJobApplicationFeedback() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditJobApplicationFeedbackCommandCorrected
        ) => {
            await axios.put(
                `/job-application/${payload.jobApplicationId}/feedback/${payload.feedbackId}`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Feedback updated');
            queryClient.invalidateQueries({ queryKey: ['job-applications'] });
            queryClient.invalidateQueries({
                queryKey: ['job-application', variables.jobApplicationId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to update feedback'
            );
            console.error(error);
        },
    });
}

export function useMoveJobApplicationStatus() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: MoveJobApplicationStatusCommandCorrected
        ) => {
            await axios.put(
                `/job-application/${payload.id}/move-status`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Job application status moved');
            queryClient.invalidateQueries({
                queryKey: ['job-applications'],
            });
            queryClient.invalidateQueries({
                queryKey: ['interviews'],
            });
            queryClient.invalidateQueries({
                queryKey: ['position-batches'],
            });
            queryClient.invalidateQueries({
                queryKey: ['job-application', variables.id],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to move status'
            );
            console.error(error);
        },
    });
}

export function useDeleteJobApplication() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/job-application/${id}`);
        },
        onSuccess: () => {
            toast.success('Job application deleted');
            queryClient.invalidateQueries({ queryKey: ['job-applications'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to delete job application'
            );
            console.error(error);
        },
    });
}

export function useDeleteJobApplicationFeedback() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (args: {
            applicationId: string;
            feedbackId: string;
        }) => {
            await axios.delete(
                `/job-application/${args.applicationId}/feedback/${args.feedbackId}`
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Feedback deleted');
            queryClient.invalidateQueries({ queryKey: ['job-applications'] });
            queryClient.invalidateQueries({
                queryKey: ['job-application', variables.applicationId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to delete feedback'
            );
            console.error(error);
        },
    });
}

export function useGetJobApplication(id: string) {
    return useQuery({
        queryKey: ['job-application', id],
        queryFn: async (): Promise<JobApplication | null> => {
            const { data } = await axios.get(`/job-application/${id}`);
            return data;
        },
    });
}

export function useGetJobApplications() {
    return useQuery({
        queryKey: ['job-applications'],
        queryFn: async (): Promise<JobApplicationSummary[]> => {
            const { data } = await axios.get('/job-application');
            return data;
        },
    });
}

export function useGetJobOpeningApplications(jobOpeningId: string) {
    return useQuery({
        queryKey: ['job-applications', jobOpeningId],
        queryFn: async (): Promise<JobOpeningApplicationSummary[]> => {
            const { data } = await axios.get(
                `/job-application/for-job-opening/${jobOpeningId}`
            );
            return data;
        },
        enabled: !!jobOpeningId,
    });
}

export function useGetJobApplicationsToReview() {
    return useQuery({
        queryKey: ['job-applications-to-review'],
        queryFn: async (): Promise<JobApplicationSummary[]> => {
            const { data } = await axios.get('/job-application/to-review');
            return data;
        },
    });
}
