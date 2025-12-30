import type {
    CreateSkillCommandCorrected,
    EditSkillCommandCorrected,
    Skill,
} from '@/types/skill-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateSkill() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateSkillCommandCorrected
        ): Promise<void> => {
            await axios.post('/skills', payload);
        },
        onSuccess: () => {
            toast.success('Skill Created');
            queryClient.invalidateQueries({ queryKey: ['skills'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Skill Not Created'
            );
            console.error('Skill creation failed:', error);
        },
    });
}

export function useEditSkill() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditSkillCommandCorrected
        ): Promise<undefined> => {
            await axios.put(`/skills/${payload.id}`, payload);
        },
        onSuccess: () => {
            toast.success('Skill Edited');
            queryClient.invalidateQueries({ queryKey: ['skills'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Skill Not Edited'
            );
            console.error('Skill editing failed:', error);
        },
    });
}

export function useDeleteSkill() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/skills/${id}`);
        },
        onSuccess: () => {
            toast.success('Skill Deleted');
            queryClient.invalidateQueries({ queryKey: ['skills'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Skill Not Deleted'
            );
            console.error('Skill deletion failed:', error);
        },
    });
}

export function useGetSkill(id: string) {
    return useQuery({
        queryKey: ['skill', id],
        queryFn: async (): Promise<Skill | null> => {
            const { data } = await axios.get(`/skills/${id}`);
            return data;
        },
    });
}

export function useGetSkills() {
    return useQuery({
        queryKey: ['skills'],
        queryFn: async (): Promise<Skill[]> => {
            const { data } = await axios.get('/skills');
            return data;
        },
    });
}
