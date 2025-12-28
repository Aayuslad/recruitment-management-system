import type {
    AddCandidateDocumentCommandCorrected,
    Candidate,
    CandidateSummary,
    CreateCandidateCommandCorrected,
    EditCandidateCommandCorrected,
    VerifyCandidateDocumentCommandCorrected,
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

export function useVerifyDocument() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: VerifyCandidateDocumentCommandCorrected
        ): Promise<void> => {
            await axios.put(
                `/candidate/${payload.candidateId}/verify-doc/${payload.documentId}`,
                payload
            );
        },
        onSuccess: (_, variables) => {
            toast.success('Document verification updated');
            queryClient.invalidateQueries({ queryKey: ['candidates'] });
            queryClient.invalidateQueries({
                queryKey: ['candidate', variables.candidateId],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to verify document'
            );
            console.error(error);
        },
    });
}

export function useAddDocument() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: AddCandidateDocumentCommandCorrected
        ): Promise<void> => {
            await axios.put(`/candidate/${payload.id}/add-document`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Document verification updated');
            queryClient.invalidateQueries({ queryKey: ['candidates'] });
            queryClient.invalidateQueries({
                queryKey: ['candidate', variables.id],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Failed to verify document'
            );
            console.error(error);
        },
    });
}

export function useGetCandidate(id?: string) {
    return useQuery<Candidate | null>({
        queryKey: ['candidate', id],
        queryFn: async () => {
            const { data } = await axios.get(`/candidate/${id}`);
            return data;
        },
        enabled: !!id,
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
