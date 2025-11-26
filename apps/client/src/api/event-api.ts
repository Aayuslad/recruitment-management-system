import type {
    CreateEventCommandCorrected,
    EditEventCommandCorrected,
} from '@/types/event-types';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import axios, { AxiosError } from 'axios';
import { toast } from 'sonner';

export function useCreateEvent() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: CreateEventCommandCorrected
        ): Promise<void> => {
            await axios.post('/event', payload);
        },
        onSuccess: () => {
            toast.success('Event Created');
            queryClient.invalidateQueries({ queryKey: ['events'] });
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Event Not Created'
            );
            console.error('Event Creation Failed:', error);
        },
    });
}

export function useEditEvent() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (
            payload: EditEventCommandCorrected
        ): Promise<void> => {
            await axios.put(`/event/${payload.id}`, payload);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['events'] });
            toast.success('Event Edited');
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Event Not Edited'
            );
            console.error('Event Editing Failed:', error);
        },
    });
}

export function useDeleteEvent() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (id: string) => {
            await axios.delete(`/event/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['events'] });
            toast.success('Event Deleted');
        },
        onError: (error: AxiosError<{ error: string }>) => {
            toast.error(
                error.response?.data?.error ||
                    error.message ||
                    'Event Not Deleted'
            );
            console.error('Event Deletion Failed:', error);
        },
    });
}

export function useGetEvents() {
    return useQuery({
        queryKey: ['events'],
        queryFn: async () => {
            const { data } = await axios.get('/event');
            return data;
        },
    });
}
