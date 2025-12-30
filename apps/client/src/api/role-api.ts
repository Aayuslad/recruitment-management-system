import type {
    CreateRoleCommandCorrected,
    EditRoleCommandCorrected,
    Role,
} from '@/types/role-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateRole() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateRoleCommandCorrected
        ): Promise<void> => {
            await axios.post('/roles', payload);
        },
        onSuccess: () => {
            toast.success('Role Created');
            queryClient.invalidateQueries({ queryKey: ['roles'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Role Not Created'
            );
            console.error('Role creation failed:', error);
        },
    });
}

export function useEditRole() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditRoleCommandCorrected
        ): Promise<void> => {
            await axios.put(`/roles/${payload.id}`, payload);
        },
        onSuccess: () => {
            toast.success('Role Edited');
            queryClient.invalidateQueries({ queryKey: ['roles'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Role Not Edited'
            );
            console.error('Role editing failed:', error);
        },
    });
}

export function useDeleteRole() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (roleId: string): Promise<void> => {
            await axios.delete(`/roles/${roleId}`);
        },
        onSuccess: () => {
            toast.success('Role Deleted');
            queryClient.invalidateQueries({ queryKey: ['roles'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Role Not Deleted'
            );
            console.error('Role deletion failed:', error);
        },
    });
}

export function useGetRoles() {
    return useQuery({
        queryKey: ['roles'],
        queryFn: async (): Promise<Role[]> => {
            const { data } = await axios.get('/roles');
            return data;
        },
    });
}
