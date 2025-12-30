import type {
    CreateDesignationCommandCorrected,
    Designation,
    EditDesignationCommandCorrected,
} from '@/types/designation-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateDesignation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateDesignationCommandCorrected
        ): Promise<void> => {
            await axios.post('/designations', payload);
        },
        onSuccess: () => {
            toast.success('Designation Created');
            queryClient.invalidateQueries({ queryKey: ['designations'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Designation Not Created'
            );
            console.error('Designation creation failed:', error);
        },
    });
}

export function useEditDesignation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditDesignationCommandCorrected
        ): Promise<void> => {
            await axios.put(`/designations/${payload.id}`, payload);
        },
        onSuccess: (_, variables) => {
            toast.success('Designation Edited');
            queryClient.invalidateQueries({ queryKey: ['designations'] });
            queryClient.invalidateQueries({
                queryKey: ['designation', variables.id],
            });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Designation Not Edited'
            );
            console.error('Designation editing failed:', error);
        },
    });
}

export function useDeleteDesignation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/designations/${id}`);
        },
        onSuccess: () => {
            toast.success('Designation Deleted');
            queryClient.invalidateQueries({ queryKey: ['designations'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Designation Not Deleted'
            );
            console.error('Designation deletion failed:', error);
        },
    });
}

export function useGetDesignation(id: string) {
    return useQuery({
        queryKey: ['designation', id],
        queryFn: async (): Promise<Designation | null> => {
            const { data } = await axios.get(`/designations/${id}`);
            return data;
        },
    });
}

export function useGetDesignations() {
    return useQuery({
        queryKey: ['designations'],
        queryFn: async (): Promise<Designation[]> => {
            const { data } = await axios.get('/designations');
            return data;
        },
    });
}
