import type {
    CreateSkillCommandCorrected,
    EditSkillCommandCorrected,
    SkillDTO,
} from '@/types/skill-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

// create skill
export function useCreateSkill() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateSkillCommandCorrected
        ): Promise<undefined> => {
            await axios.post('/skill', payload);
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

// get a skill
export function useGetSkill(id: string) {
    return useQuery({
        queryKey: ['skill', id],
        queryFn: async (): Promise<SkillDTO | undefined> => {
            const { data } = await axios.get(`/skill/${id}`);
            return data;
        },
    });
}

// edit a skill
export function useEditSkill() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditSkillCommandCorrected
        ): Promise<undefined> => {
            await axios.put(`/skill/${payload.id}`, payload);
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

// get skill in bulk
export function useGetSkills() {
    return useQuery({
        queryKey: ['skills'],
        queryFn: async (): Promise<SkillDTO[] | undefined> => {
            const { data } = await axios.get('/skill');
            return data;
        },
    });
}

// delete a skill
export function useDeleteSkill() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string): Promise<void> => {
            await axios.delete(`/skill/${id}`);
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
