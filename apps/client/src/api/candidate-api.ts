import type {
    Candidate,
    CandidateSummary,
    CreateCandidateCommandCorrected,
    EditCandidateCommandCorrected,
} from '@/types/candidate-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateCandidate() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateCandidateCommandCorrected
        ): Promise<void> => {
            await axios.post('/candidate', payload);
        },
        onSuccess: () => {
            toast.success('Candidate created');
            queryClient.invalidateQueries({ queryKey: ['candidates'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to create candidate'
            );
            console.error(error);
        },
    });
}

export function useEditCandidate() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditCandidateCommandCorrected
        ): Promise<void> => {
            await axios.put(`/candidate/${payload.id}`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Candidate updated');
            queryClient.invalidateQueries({ queryKey: ['candidates'] });
            queryClient.invalidateQueries({
                queryKey: ['candidate', variables.id],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to update candidate'
            );
            console.error(error);
        },
    });
}

export function useDeleteCandidate() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/candidate/${id}`);
        },
        onSuccess: () => {
            toast.success('Candidate deleted');
            queryClient.invalidateQueries({ queryKey: ['candidates'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to delete candidate'
            );
            console.error(error);
        },
    });
}

export function useVerifyCandidateBg() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.put(`/candidate/verify-bg/${id}`);
        },
        onSuccess: () => {
            toast.success('Background verification updated');
            queryClient.invalidateQueries({ queryKey: ['candidates'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to verify background'
            );
            console.error(error);
        },
    });
}

export function useGetCandidate(id: string) {
    return useQuery({
        queryKey: ['candidate', id],
        queryFn: async (): Promise<Candidate | null> => {
            const { data } = await axios.get(`/candidate/${id}`);
            return data;
        },
    });
}

export function useGetCandidates() {
    return useQuery({
        queryKey: ['candidates'],
        queryFn: async (): Promise<CandidateSummary[]> => {
            const { data } = await axios.get('/candidate');
            return data;
        },
    });
}
