import type {
    CreateDesignationCommandCorrected,
    DesignationDTO,
    EditDesignationCommandCorrected,
} from '@/types/designation-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

// get designations
export function useGetDesignations() {
    return useQuery({
        queryKey: ['designations'],
        queryFn: async (): Promise<DesignationDTO[]> => {
            const { data } = await axios.get('/designation');
            return data;
        },
    });
}

// get designation with id
export function useGetDesignation(id: string) {
    return useQuery({
        queryKey: ['designation', id],
        queryFn: async (): Promise<DesignationDTO | undefined> => {
            const { data } = await axios.get(`/designation/${id}`);
            return data;
        },
    });
}

// create designation
export function useCreateDesignation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateDesignationCommandCorrected
        ): Promise<void> => {
            await axios.post('/designation', payload);
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

// eidt designation
export function useEditDesignation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditDesignationCommandCorrected
        ): Promise<void> => {
            await axios.put(`/designation/${payload.id}`, payload);
        },
        onSuccess: () => {
            toast.success('Designation Edited');
            queryClient.invalidateQueries({ queryKey: ['designations'] });
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

// delete designation
export function useDeleteDesignation() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/designation/${id}`);
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
