import type {
    Interview,
    CreateInterviewCommandCorrected,
    EditInterviewCommandCorrected,
    CreateInterviewFeedbackCommandCorrected,
    EditInterviewFeedbackCommandCorrected,
    InterviewSummary,
    MoveInterviewStatusCommandCorrected,
    InterviewSummaryForJobApplication,
} from '@/types/interview-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateInterview() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateInterviewCommandCorrected
        ): Promise<void> => {
            await axios.post('/interviews', payload);
        },
        onSuccess: () => {
            toast.success('Interview created');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to create interview'
            );
            console.error(error);
        },
    });
}

export function useEditInterview() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditInterviewCommandCorrected
        ): Promise<void> => {
            await axios.put(`/interviews/${payload.id}`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Interview updated');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
            queryClient.invalidateQueries({
                queryKey: ['interview', variables.id],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to update interview'
            );
            console.error(error);
        },
    });
}

export function useMoveInterviewStatus() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: MoveInterviewStatusCommandCorrected
        ): Promise<void> => {
            await axios.patch(
                `/interviews/${payload.interviewId}/move-status`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Interview status moved');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
            queryClient.invalidateQueries({
                queryKey: ['interview', variables.interviewId],
            });
            queryClient.invalidateQueries({
                queryKey: ['job-applications'],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to Move interview status'
            );
            console.error(error);
        },
    });
}

export function useDeleteInterview() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/interviews/${id}`);
        },
        onSuccess: (_, id) => {
            toast.success('Interview deleted');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
            queryClient.invalidateQueries({ queryKey: ['interview', id] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to delete interview'
            );
            console.error(error);
        },
    });
}

export function useCreateInterviewFeedback() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateInterviewFeedbackCommandCorrected
        ) => {
            await axios.post(
                `/interviews/${payload.interviewId}/feedback`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Feedback added');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
            queryClient.invalidateQueries({
                queryKey: ['interview', variables.interviewId],
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

export function useEditInterviewFeedback() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (payload: EditInterviewFeedbackCommandCorrected) => {
            await axios.put(
                `/interviews/${payload.interviewId}/feedback/${payload.feedbackId}`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Feedback updated');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
            queryClient.invalidateQueries({
                queryKey: ['interview', variables.interviewId],
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

export function useDeleteInterviewFeedback() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (args: {
            interviewId: string;
            feedbackId: string;
        }) => {
            await axios.delete(
                `/interviews/${args.interviewId}/feedback/${args.feedbackId}`
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Feedback deleted');
            queryClient.invalidateQueries({ queryKey: ['interviews'] });
            queryClient.invalidateQueries({
                queryKey: ['interview', variables.interviewId],
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

export function useGetInterview(id?: string) {
    return useQuery({
        queryKey: ['interview', id],
        queryFn: async (): Promise<Interview | null> => {
            const { data } = await axios.get(`/interviews/${id}`);
            return data;
        },
        enabled: !!id,
    });
}

export function useGetAssignedInterviews() {
    return useQuery({
        queryKey: ['assigned-interviews'],
        queryFn: async (): Promise<InterviewSummary[] | null> => {
            const { data } = await axios.get('/interviews/assigned');
            return data;
        },
    });
}

export function useGetJobApplicationInterviews(jobApplicationId?: string) {
    return useQuery({
        queryKey: ['job-application-interviews', jobApplicationId],
        queryFn: async (): Promise<
            InterviewSummaryForJobApplication[] | null
        > => {
            const { data } = await axios.get(
                `/interviews/for-job-application/${jobApplicationId}`
            );
            return data;
        },
        enabled: !!jobApplicationId,
    });
}
